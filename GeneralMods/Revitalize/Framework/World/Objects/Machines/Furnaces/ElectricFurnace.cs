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
    public class ElectricFurnace : Machine
    {
        public const string ELECTRIC_WORKING_ANIMATION_KEY = "Electric_Working";
        public const string ELECTRIC_IDLE_ANIMATION_KEY = "Electric_Idle";

        public const string NUCLEAR_WORKING_ANIMATION_KEY = "Nuclear_Working";
        public const string NUCLEAR_IDLE_ANIMATION_KEY = "Nuclear_Idle";

        public const string MAGICAL_WORKING_ANIMATION_KEY = "Magical_Working";
        public const string MAGICAL_IDLE_ANIMATION_KEY = "Magical_Idle";

        public enum FurnaceType
        {
            Electric,
            Nuclear,
            Magical
        }

        public readonly NetInt chargesRemaining = new NetInt();
        public readonly NetRef<ItemReference> smeltingItem = new NetRef<ItemReference>(new ItemReference());
        public readonly NetEnum<FurnaceType> furnaceType = new NetEnum<FurnaceType>(FurnaceType.Electric);

        public ElectricFurnace()
        {

        }


        public ElectricFurnace(BasicItemInformation info, FurnaceType furnaceType) : base(info)
        {
            this.createStatusBubble();
            this.furnaceType.Value = furnaceType;
        }

        public ElectricFurnace(BasicItemInformation info, Vector2 TileLocation, FurnaceType furnaceType) : base(info, TileLocation)
        {
            this.createStatusBubble();
            this.furnaceType.Value = furnaceType;
        }

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.chargesRemaining, this.smeltingItem, this.furnaceType);
        }

        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            base.minutesElapsed(minutes, environment);

            if (this.MinutesUntilReady == 0 && this.smeltingItem.Value.isNotNull())
            {
                this.updateAnimation();
            }

            return true;
        }

        public override bool rightClicked(Farmer who)
        {

            if (this.heldObject.Value != null && who.IsLocalPlayer && this.finishedProduction())
            {
                this.cleanOutFurnace(who != null);
            }

            return base.rightClicked(who);
        }

        /// <summary>
        /// Cleans out the furnace to produce more items.
        /// </summary>
        /// <param name="addToPlayersInventory"></param>
        public virtual void cleanOutFurnace(bool addToPlayersInventory)
        {
            if (addToPlayersInventory)
            {
                SoundUtilities.PlaySound(Enums.StardewSound.coin);
                bool added = Game1.player.addItemToInventoryBool(this.heldObject.Value);
                if (added == false) return;
            }
            this.heldObject.Value = null;
            this.updateAnimation();
            this.smeltingItem.Value.clearItemReference();

        }

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
                    if (this.furnaceType.Value == FurnaceType.Electric)
                    {
                        multiplier = .75f;
                    }
                    if (this.furnaceType.Value == FurnaceType.Nuclear)
                    {
                        multiplier = .5f;
                    }
                    if (this.furnaceType.Value == FurnaceType.Magical)
                    {
                        multiplier = .25f;
                    }

                    //Make sure enough fue is present for the furnace to operate (if necessary!)
                    bool success = this.chargesRemaining.Value <= 0 ? this.consumeFuelItemFromFarmersInventory(who) : true;

                    if (success == false && ShowRedMessage)
                    {
                        this.showRedMessageForMissingFuel();
                        return new CraftingRecipeResult(craftingRecipe, false);
                    }

                    if (this.chargesRemaining.Value <= 0)
                    {
                        this.increaseFuelCharges();
                    }

                    this.smeltingItem.Value.setItemReference(neededDropInItem);
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
            return Constants.CraftingIds.CraftingRecipeBooks.ElectricFurnaceCraftingRecipies;
        }

        public override bool performObjectDropInAction(Item dropInItem, bool probe, Farmer who)
        {
            if (probe == true || this.MinutesUntilReady > 0) return false;

            //Cleans out the furnace as necessary to ensure it works properly when dropping in another item.
            if (this.finishedProduction())
            {
                this.cleanOutFurnace(who != null);
            }
            this.processItemFromRecipe(dropInItem, who,true);

            //return base.performObjectDropInAction(dropInItem, probe, who);
            return false;
        }

        /// <summary>
        /// Updates the animation manager to play the correct animation.
        /// </summary>
        protected virtual void updateAnimation()
        {
            if (this.furnaceType.Value == FurnaceType.Electric)
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
            if (this.furnaceType.Value == FurnaceType.Nuclear)
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
            if (this.furnaceType.Value == FurnaceType.Magical)
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

        /// <summary>
        /// Attempts to consume the necessary fuel item from the player's inventory.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        protected virtual bool consumeFuelItemFromFarmersInventory(Farmer who)
        {
            if (who == null) return true; //Used for automate compatibility
            if (this.furnaceType.Value == FurnaceType.Magical)
            {
                return true;
            }
            if (this.furnaceType.Value == FurnaceType.Electric)
            {
                return PlayerUtilities.ReduceInventoryItemIfEnoughFound(who, Enums.SDVObject.BatteryPack, 1);
            }
            if (this.furnaceType.Value == FurnaceType.Nuclear)
            {
                return PlayerUtilities.ReduceInventoryItemIfEnoughFound(who, MiscItemIds.RadioactiveFuel, 1);
            }
            return true;
            //Magical does not consume fuel.

        }

        /// <summary>
        /// Consumes a single charge of fuel used on this funace.
        /// </summary>
        protected virtual void consumeFuelCharge()
        {
            if (this.furnaceType.Value == FurnaceType.Magical) return;
            this.chargesRemaining.Value--;
            if (this.chargesRemaining.Value <= 0) this.chargesRemaining.Value = 0;
        }

        /// <summary>
        /// Increases the fuel type for the furnace.
        /// </summary>
        public virtual void increaseFuelCharges()
        {
            if (this.furnaceType.Value == FurnaceType.Electric)
            {
                this.chargesRemaining.Value = 5;
            }
            if (this.furnaceType.Value == FurnaceType.Nuclear)
            {
                this.chargesRemaining.Value = 25;
            }

            if (this.furnaceType.Value == FurnaceType.Magical)
            {
                this.chargesRemaining.Value = 999;
            }
        }

        /// <summary>
        /// Shows an error message if there is no correct fuel present for the furnace.
        /// </summary>
        protected virtual void showRedMessageForMissingFuel()
        {
            if (this.furnaceType.Value == FurnaceType.Electric)
            {
                Game1.showRedMessage(JsonContentLoaderUtilities.LoadErrorString(this.getErrorStringFile(), "NeedBatteryPack"));
                return;
            }
            if (this.furnaceType.Value == FurnaceType.Nuclear)
            {
                Game1.showRedMessage(JsonContentLoaderUtilities.LoadErrorString(this.getErrorStringFile(), "NeedNuclearFuel"));
                return;
            }
            Game1.showRedMessage(JsonContentLoaderUtilities.LoadErrorString(this.getErrorStringFile(), "MagicalFurnaceFuelError"));
            return;
        }


        protected override void drawStatusBubble(SpriteBatch b, int x, int y, float Alpha)
        {
            if (this.machineStatusBubbleBox == null || this.machineStatusBubbleBox.Value == null) this.createStatusBubble();
            if (this.finishedProduction())
            {
                y--;
                float num = (float)(4.0 * Math.Round(Math.Sin(DateTime.UtcNow.TimeOfDay.TotalMilliseconds / 250.0), 2));
                this.machineStatusBubbleBox.Value.playAnimation(MachineStatusBubble_BlankBubbleAnimationKey);
                this.machineStatusBubbleBox.Value.draw(b, this.machineStatusBubbleBox.Value.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2(x * Game1.tileSize, y * Game1.tileSize + num)), new Rectangle?(this.machineStatusBubbleBox.Value.getCurrentAnimationFrameRectangle()), Color.White, 0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (y + 2) * Game1.tileSize / 10000f) + .00001f);

                Rectangle itemSourceRectangle = GameLocation.getSourceRectForObject(this.heldObject.Value.ParentSheetIndex);
                this.machineStatusBubbleBox.Value.draw(b, Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize) + 8, y * Game1.tileSize + num + 16)), new Rectangle?(itemSourceRectangle), Color.White, 0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (y + 2) * Game1.tileSize / 10000f) + .00002f);

            }
        }

        /// <summary>What happens when the object is drawn at a tile location.</summary>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {

            x = (int)this.TileLocation.X;

            y = (int)this.TileLocation.Y;

            if (this.MinutesUntilReady > 0)
            {
                Vector2 origin = new Vector2(this.AnimationManager.getCurrentAnimationFrameRectangle().Width / 2, this.AnimationManager.getCurrentAnimationFrameRectangle().Height);

                this.basicItemInformation.animationManager.draw(spriteBatch, this.basicItemInformation.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((x + this.basicItemInformation.drawOffset.X) * Game1.tileSize) + this.basicItemInformation.shakeTimerOffset() + Game1.tileSize * origin.X / this.AnimationManager.getCurrentAnimationFrameRectangle().Width, (y + this.basicItemInformation.drawOffset.Y) * Game1.tileSize + this.basicItemInformation.shakeTimerOffset() + Game1.tileSize * (origin.Y / this.AnimationManager.getCurrentAnimationFrameRectangle().Height + 1))), new Rectangle?(this.AnimationManager.getCurrentAnimation().getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * alpha, 0f, origin, this.getScaleSizeForWorkingMachine(), this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (this.TileLocation.Y - this.basicItemInformation.drawOffset.Y) * Game1.tileSize / 10000f) + .00001f);
            }
            else
                this.basicItemInformation.animationManager.draw(spriteBatch, this.basicItemInformation.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((x + this.basicItemInformation.drawOffset.X) * Game1.tileSize) + this.basicItemInformation.shakeTimerOffset(), (y + this.basicItemInformation.drawOffset.Y) * Game1.tileSize + this.basicItemInformation.shakeTimerOffset())), new Rectangle?(this.AnimationManager.getCurrentAnimation().getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * alpha, 0f, Vector2.Zero, Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (this.TileLocation.Y - this.basicItemInformation.drawOffset.Y) * Game1.tileSize / 10000f) + .00001f);

            if (this.finishedProduction())
                this.drawStatusBubble(spriteBatch, x + (int)this.basicItemInformation.drawOffset.X, y + (int)this.basicItemInformation.drawOffset.Y, alpha);

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

        /// <summary>
        /// Gets the relative path to the file to load the error strings from the ErrorStrings directory.
        /// </summary>
        /// <returns></returns>
        public virtual string getErrorStringFile()
        {
            return Path.Combine("Objects", "Machines", "ElectricFurnace.json");
        }

    }
}
