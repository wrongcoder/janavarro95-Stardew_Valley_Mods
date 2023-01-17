using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.HUD;
using Omegasis.Revitalize.Framework.Player;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using Omegasis.Revitalize.Framework.World.WorldUtilities.Items;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines
{
    public class ItemRecipeDropInMachine:Machine
    {
        public ItemRecipeDropInMachine()
        {

        }

        public ItemRecipeDropInMachine(BasicItemInformation info) : base(info)
        {
        }

        public ItemRecipeDropInMachine(BasicItemInformation info, Vector2 TileLocation) : base(info, TileLocation) { 

        }

        public override bool performItemDropInAction(Item dropInItem, bool probe, Farmer who)
        {
            if (probe) return false;
            //Prevent overriding and destroying the previous operation.
            if (who != null && this.finishedProduction())
            {
                bool added=Game1.player.addItemToInventoryBool(this.heldObject.Value);
                if (!added)
                {
                    HudUtilities.ShowInventoryFullErrorMessage();
                    return false;
                }
                else
                {
                    SoundUtilities.PlaySound(Enums.StardewSound.coin);
                    this.heldObject.Value = null;
                }
            }
            bool success = base.performItemDropInAction(dropInItem, probe, who);
            if (!success) return false;
            CraftingResult result = this.processInput(dropInItem, who, true);


            return result.successful;
        }

        /// <summary>
        /// Processes a player's item that they are holding to set recipe to be processed for the machine.
        /// </summary>
        /// <param name="dropInItem"></param>
        /// <param name="who"></param>
        /// <param name="ShowRedMessage"></param>
        /// <returns></returns>
        public override CraftingResult processInput(Item dropInItem, Farmer who, bool ShowRedMessage = true)
        {
            if (string.IsNullOrEmpty(this.getCraftingRecipeBookId()) || this.isWorking() || this.finishedProduction())
            {
                return new CraftingResult(false);
            }
            foreach (ProcessingRecipe<LootTableEntry> craftingRecipe in RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.getProcessingRecipesForObject(this.getCraftingRecipeBookId()))
            {
                Item neededDropInItem = craftingRecipe.inputs[0].getItem();
                int amountRequired = craftingRecipe.inputs[0].StackSize;

                ItemReference itemRef = new ItemReference(neededDropInItem);

                if (neededDropInItem.canStackWith(dropInItem) || itemRef.itemEquals(dropInItem))
                {
                    //Check to make sure the player has enough, otherwise display an error!
                    if (amountRequired > dropInItem.Stack)
                    {
                        if (ShowRedMessage)
                        {
                            Game1.showRedMessage(this.getErrorString_NeedMoreInputItems(amountRequired, dropInItem));
                        }
                        return new CraftingResult(false);
                    }

                    return this.onSuccessfulRecipeFound(dropInItem,craftingRecipe, who);
                }
            }
            return new CraftingResult(false);
        }

        public virtual CraftingResult onSuccessfulRecipeFound(Item dropInItem, ProcessingRecipe<LootTableEntry> craftingRecipe, Farmer who=null)
        {
            Item outputItem = craftingRecipe.outputs[0].item.getItem();
            int amountRequired = craftingRecipe.inputs[0].StackSize;

            outputItem.Stack = craftingRecipe.outputs[0].getFinalOutputAmount();
            this.heldObject.Value = (StardewValley.Object)outputItem;
            this.MinutesUntilReady = (int)(craftingRecipe.timeToProcess.toInGameMinutes());
            if (who != null)
            {
                this.playDropInSound();
            }
            PlayerUtilities.ReduceInventoryItemStackSize(who, dropInItem, amountRequired);
            this.updateAnimation();

            return new CraftingResult(new ItemReference(outputItem, amountRequired), true); //Found a sucessful recipe.
        }

        public virtual void playDropInSound()
        {
            //SoundUtilities.PlaySound(Enums.StardewSound.Ship);
        }

        public override void updateAnimation()
        {
            if (this.isWorking())
            {
                this.AnimationManager.playAnimation(Machine.WORKING_ANIMATION_KEY);
            }
            else
            {
                this.AnimationManager.playDefaultAnimation();
            }
        }


        public override Item getOne()
        {
            ItemRecipeDropInMachine component = new ItemRecipeDropInMachine(this.basicItemInformation.Copy());
            return component;
        }

        public virtual string getCraftingRecipeBookId()
        {
            return this.Id;
        }
    


    }
}
