using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Items;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.Player;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.Utilities.JsonContentLoading;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines.Furnaces
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.Furnaces.ElectricFurnace")]
    public class ElectricFurnace : PoweredMachine
    {
        public const string ELECTRIC_WORKING_ANIMATION_KEY = "Electric_Working";
        public const string ELECTRIC_IDLE_ANIMATION_KEY = "Electric_Idle";

        public const string NUCLEAR_WORKING_ANIMATION_KEY = "Nuclear_Working";
        public const string NUCLEAR_IDLE_ANIMATION_KEY = "Nuclear_Idle";

        public const string MAGICAL_WORKING_ANIMATION_KEY = "Magical_Working";
        public const string MAGICAL_IDLE_ANIMATION_KEY = "Magical_Idle";

        public readonly NetInt chargesRemaining = new NetInt();
        public readonly NetEnum<PoweredMachineTier> furnaceType = new NetEnum<PoweredMachineTier>(PoweredMachineTier.Electric);

        public ElectricFurnace()
        {

        }


        public ElectricFurnace(BasicItemInformation info, PoweredMachineTier furnaceType) : base(info,furnaceType)
        {
            this.createStatusBubble();
            this.furnaceType.Value = furnaceType;
        }

        public ElectricFurnace(BasicItemInformation info, Vector2 TileLocation, PoweredMachineTier furnaceType) : base(info, TileLocation,furnaceType)
        {
            this.createStatusBubble();
            this.furnaceType.Value = furnaceType;
        }

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.chargesRemaining, this.furnaceType);
        }

        /// <summary>
        /// Processes a player's item that they are holding to set recipe to be processed for the furnace.
        /// </summary>
        /// <param name="dropInItem"></param>
        /// <param name="who"></param>
        /// <param name="ShowRedMessage"></param>
        /// <returns></returns>
        public virtual CraftingRecipeResult processItemFromRecipe(Item dropInItem, Farmer who, bool ShowRedMessage=true)
        {
            foreach(var craftingRecipe in RevitalizeModCore.ModContentManager.craftingManager.getUnlockedCraftingRecipes(this.getCraftingBookName()))
            {
                Item neededDropInItem = craftingRecipe.ingredients[0].item;
                int amountRequired = craftingRecipe.ingredients[0].requiredAmount;

                ItemReference itemRef = new ItemReference(neededDropInItem);

                if (neededDropInItem.canStackWith(dropInItem) || itemRef.itemEquals(dropInItem))
                {
                    //Check to make sure the player has enough, otherwise display an error!
                    if (amountRequired > dropInItem.Stack)
                    {
                        if (ShowRedMessage)
                        {
                            Game1.showRedMessage(JsonContentLoaderUtilities.LoadErrorString(this.getErrorStringFile(), "NeedMoreInputItems", amountRequired, neededDropInItem.DisplayName));
                        }
                        return new CraftingRecipeResult(craftingRecipe,false);
                    }

                    float multiplier = 1f;
                    if (this.furnaceType.Value == PoweredMachineTier.Electric)
                    {
                        multiplier = .75f;
                    }
                    if (this.furnaceType.Value == PoweredMachineTier.Nuclear)
                    {
                        multiplier = .5f;
                    }
                    if (this.furnaceType.Value == PoweredMachineTier.Magical)
                    {
                        multiplier = .25f;
                    }

                    //Make sure enough fue is present for the furnace to operate (if necessary!)
                    bool success = this.useFuelItemToIncreaseCharges(who,false ,ShowRedMessage);

                    if (success == false)
                    {
                        return new CraftingRecipeResult(craftingRecipe, false);
                    }

                    Item outputItem = craftingRecipe.outputs[0].item.getOne();
                    outputItem.Stack = craftingRecipe.outputs[0].requiredAmount;
                    this.heldObject.Value = (StardewValley.Object)outputItem;
                    this.MinutesUntilReady = (int)(craftingRecipe.timeToCraft * multiplier);
                    this.MinutesUntilReady -= this.MinutesUntilReady % 10; //Want to make sure the time remaining is divisible by 10, so we will just round down.
                    if (this.MinutesUntilReady < 10)
                    {
                        this.MinutesUntilReady = 10; //Make sure there is at least 10 minues to craft something.
                    }

                    if (who != null)
                    {
                        SoundUtilities.PlaySound(Enums.StardewSound.furnace);
                    }
                    this.consumeFuelCharge();
                    PlayerUtilities.ReduceInventoryItemStackSize(who, dropInItem, amountRequired);
                    this.updateAnimation();

                    return new CraftingRecipeResult(craftingRecipe, true); //Found a sucessful recipe.
                }
            }
            return new CraftingRecipeResult(null,false);
        }

        public virtual string getCraftingBookName()
        {
            return Constants.CraftingIds.MachineCraftingRecipeBooks.ElectricFurnaceCraftingRecipies;
        }

        public override bool performObjectDropInAction(Item dropInItem, bool probe, Farmer who)
        {
            if (probe == true || this.MinutesUntilReady > 0) return false;

            //Cleans out the furnace as necessary to ensure it works properly when dropping in another item.
            if (this.finishedProduction())
            {
                this.getMachineOutputs(true, false, true);
            }
            this.processItemFromRecipe(dropInItem, who,true);

            //return base.performObjectDropInAction(dropInItem, probe, who);
            return false;
        }

        /// <summary>
        /// Updates the animation manager to play the correct animation.
        /// </summary>
        public override void updateAnimation()
        {
            if (this.furnaceType.Value == PoweredMachineTier.Electric)
            {
                if (this.MinutesUntilReady > 0)
                {
                    this.AnimationManager.playAnimation(ELECTRIC_WORKING_ANIMATION_KEY);
                    return;
                }
                else
                {
                    this.AnimationManager.playAnimation(ELECTRIC_IDLE_ANIMATION_KEY);
                    return;
                }

            }
            if (this.furnaceType.Value == PoweredMachineTier.Nuclear)
            {
                if (this.MinutesUntilReady > 0)
                {
                    this.AnimationManager.playAnimation(NUCLEAR_WORKING_ANIMATION_KEY);
                    return;
                }
                else
                {
                    this.AnimationManager.playAnimation(NUCLEAR_IDLE_ANIMATION_KEY);
                    return;
                }
            }
            if (this.furnaceType.Value == PoweredMachineTier.Magical)
            {
                if (this.MinutesUntilReady > 0)
                {
                    this.AnimationManager.playAnimation(MAGICAL_WORKING_ANIMATION_KEY);
                    return;
                }
                else
                {
                    this.AnimationManager.playAnimation(MAGICAL_IDLE_ANIMATION_KEY);
                    return;
                }
            }
        }

        public override int getElectricFuelChargeIncreaseAmount()
        {
            return 5;
        }

        public override int getNuclearFuelChargeIncreaseAmount()
        {
            return 25;
        }


        public override Item getOne()
        {
            return new ElectricFurnace(this.basicItemInformation.Copy(), this.furnaceType.Value);
        }

        public override bool canStackWith(ISalable other)
        {
            if (!(other is ElectricFurnace)) return false;
            ElectricFurnace otherFurnace = (ElectricFurnace)other;
            return base.canStackWith(other) && otherFurnace.furnaceType.Value == this.furnaceType.Value;
        }

    }
}
