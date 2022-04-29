using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Items;
using Omegasis.Revitalize.Framework.Objects;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using Omegasis.Revitalize.Framework.World.Objects.Items.Farming;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using Omegasis.StardustCore.Animations;
using StardewValley;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;

namespace Omegasis.Revitalize.Framework.World.Objects.Farming
{
    /// <summary>
    /// A variation of the <see cref="StardewValley.Objects.IndoorPot"/> which keeps watered every day. Credits goes to the game for most of this code, modifications were made to accomidate it being always being irrigated and having a custom animation. 
    /// </summary>
    [XmlType("Mods_Revitalize.Framework.World.Objects.Farming.IrrigatedGardenPot")]
    public class IrrigatedGardenPot : CustomObject, ICustomModObject
    {

        public const string DEFAULT_ANIMATION_KEY = "Default";
        public const string DRIPPING_ANIMATION_KEY = "Dripping";

        public const string DRIPPING_WITH_ENRICHER_AND_PLANTER_ATTACHMENT_ANIMATION_KEY = "Dripping_With_Enricher_And_Planter_Attachments";
        public const string DEFAULT_WITH_ENRICHER_AND_PLANTER_ATTACHMENT_ANIMATION_KEY = "Default_With_Enricher_And_Planter_Attachments";

        public const string DRIPPING_WITH_PLANTER_ATTACHMENT_ANIMATION_KEY = "Dripping_With_Planter_Attachment";
        public const string DEFAULT_WITH_PLANTER_ATTACHMENT_ANIMATION_KEY = "Default_With_Planter_Attachment";

        public const string DRIPPING_WITH_ENRICHER_ATTACHMENT_ANIMATION_KEY = "Dripping_With_Enricher_Attachment";
        public const string DEFAULT_WITH_ENRICHER_ATTACHMENT_ANIMATION_KEY = "Default_With_Enricher_Attachment";

        [XmlElement("hoeDirt")]
        public readonly NetRef<HoeDirt> hoeDirt = new NetRef<HoeDirt>();
        [XmlIgnore]
        public Crop Crop
        {
            get
            {
                return this.hoeDirt.Value.crop;
            }
            set
            {
                this.hoeDirt.Value.crop = value;
            }
        }

        [XmlElement("bush")]
        public readonly NetRef<Bush> bush = new NetRef<Bush>();

        [XmlIgnore]
        private readonly NetBool bushLoadDirty = new NetBool(value: true);

        public readonly NetBool hasPlanterAttachment = new NetBool(false);
        public readonly NetBool hasEnricherAttachment = new NetBool(false);


        public IrrigatedGardenPot()
        {
        }

        public IrrigatedGardenPot(BasicItemInformation Info) : this(Info, Vector2.Zero)
        {

        }

        public IrrigatedGardenPot(BasicItemInformation Info, Vector2 TilePosition) : base(Info, TilePosition)
        {
            this.basicItemInformation = Info;

            this.hoeDirt.Value = new HoeDirt();
            this.makeSoilWet();
            base.showNextIndex.Value = (int)this.hoeDirt.Value.state == 1;
        }

        protected override void initNetFieldsPostConstructor()
        {
            base.initNetFieldsPostConstructor();
            this.NetFields.AddFields(this.bush, this.hoeDirt, this.bushLoadDirty);
        }

        public override bool performObjectDropInAction(Item dropInItem, bool probe, Farmer who)
        {
            if (who != null && dropInItem != null && this.bush.Value == null && this.hoeDirt.Value.canPlantThisSeedHere(dropInItem.parentSheetIndex, (int)base.tileLocation.X, (int)base.tileLocation.Y, dropInItem.Category == -19))
            {
                if ((int)dropInItem.parentSheetIndex == 805)
                {
                    if (!probe)
                    {
                        Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.13053"));
                    }
                    return false;
                }
                if ((int)dropInItem.parentSheetIndex == 499)
                {
                    if (!probe)
                    {
                        Game1.playSound("cancel");
                        Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Objects:AncientFruitPot"));
                    }
                    return false;
                }
                if (!probe)
                {
                    if (!this.hoeDirt.Value.plant(dropInItem.parentSheetIndex, (int)base.tileLocation.X, (int)base.tileLocation.Y, who, dropInItem.Category == -19, who.currentLocation))
                    {
                        return false;
                    }
                }
                else
                {
                    base.heldObject.Value = new StardewValley.Object();
                }
                return true;
            }
            if (who != null && dropInItem != null && this.hoeDirt.Value.crop == null && this.bush.Value == null && dropInItem is StardewValley.Object && !(dropInItem as StardewValley.Object).bigCraftable && (int)dropInItem.parentSheetIndex == 251)
            {
                if (probe)
                {
                    base.heldObject.Value = new StardewValley.Object();
                }
                else
                {
                    this.bush.Value = new Bush(base.tileLocation, 3, who.currentLocation);
                    if (!who.currentLocation.IsOutdoors)
                    {
                        this.bush.Value.greenhouseBush.Value = true;
                        this.bush.Value.loadSprite();
                        Game1.playSound("coin");
                    }
                }
                return true;
            }

            //Probe is always checking regardless of actioning.
            if (!probe && dropInItem != null)
            {

                if (dropInItem.ParentSheetIndex == (int)Enums.SDVObject.Enricher)
                {
                    this.hasEnricherAttachment.Value = true;
                    this.updateAnimation(true);
                    Game1.player.reduceActiveItemByOne();
                    return true;
                }

                if (dropInItem is AutoPlanterGardenPotAttachment)
                {
                    this.hasPlanterAttachment.Value = true;
                    this.updateAnimation(true);
                    Game1.player.reduceActiveItemByOne();
                    return true;
                }
            }


            return false;
        }

        public override bool pickupFromGameWorld(Vector2 tileLocation, GameLocation environment, Farmer who)
        {
            bool canPickupGardenPot = base.pickupFromGameWorld(tileLocation, environment, who);
            if (canPickupGardenPot == false) return false;

            if (this.hasPlanterAttachment.Value)
            {
                Item autoPlanter = RevitalizeModCore.ObjectManager.getItem(FarmingItems.AutoPlanterGardenPotAttachment);
                if (Game1.player.isInventoryFull())
                {
                    Game1.createItemDebris(autoPlanter, Game1.player.getTileLocation(), Game1.player.FacingDirection);
                }
                else
                {
                    Game1.player.addItemToInventoryBool(autoPlanter);
                    this.hasEnricherAttachment.Value = false;
                    this.updateAnimation(true);
                }
            }
            if (this.hasEnricherAttachment.Value)
            {
                Item enricher = RevitalizeModCore.ObjectManager.getItem(Enums.SDVObject.Enricher);
                if (Game1.player.isInventoryFull())
                {
                    Game1.createItemDebris(enricher, Game1.player.getTileLocation(), Game1.player.FacingDirection);
                }
                else
                {
                    Game1.player.addItemToInventoryBool(enricher);
                    this.hasEnricherAttachment.Value = false;
                    this.updateAnimation(true);
                }
            }
            return canPickupGardenPot;

        }

        public override bool performToolAction(Tool t, GameLocation location)
        {
            if (t != null)
            {

                this.hoeDirt.Value.performToolAction(t, -1, base.tileLocation, location);
                if (this.bush.Value != null)
                {
                    if (this.bush.Value.performToolAction(t, -1, base.tileLocation, location))
                    {
                        this.bush.Value = null;
                    }
                    return false;
                }

            }
            if ((int)this.hoeDirt.Value.state == 1)
            {
                base.showNextIndex.Value = true;
            }


            return base.performToolAction(t, location);
        }

        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {
            return base.canBePlacedHere(l, tile);
        }

        public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
        {
            bool placed = base.placementAction(location, x, y, who);

            if (placed)
            {
                StardewValley.Object obj = location.getObjectAtTile(x, y);
                if (obj is IrrigatedGardenPot)
                {
                    (obj as IrrigatedGardenPot).Crop = this.Crop;
                }
            }
            return placed;
        }

        public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
        {
            if (who != null)
            {
                if (justCheckingForActivity)
                {
                    string season = Game1.GetSeasonForLocation(who.currentLocation);
                    if (this.bush.Value != null && (int)this.bush.Value.overrideSeason != -1)
                    {
                        season = Utility.getSeasonNameFromNumber(this.bush.Value.overrideSeason);
                    }
                    if (!this.hoeDirt.Value.readyForHarvest() && base.heldObject.Value == null)
                    {
                        if (this.bush.Value != null)
                        {
                            return this.bush.Value.inBloom(season, Game1.dayOfMonth);
                        }
                        return false;
                    }
                    return true;
                }
                if (who.isMoving())
                {
                    Game1.haltAfterCheck = false;
                }
                if (base.heldObject.Value != null)
                {
                    bool num = who.addItemToInventoryBool(base.heldObject.Value);
                    if (num)
                    {
                        base.heldObject.Value = null;
                        base.readyForHarvest.Value = false;
                        Game1.playSound("coin");
                    }
                    return num;
                }
                bool b = this.hoeDirt.Value.performUseAction(base.tileLocation, who.currentLocation);
                if (b)
                {
                    return b;
                }
                if (this.hoeDirt.Value.crop != null && (int)this.hoeDirt.Value.crop.currentPhase > 0 && this.hoeDirt.Value.getMaxShake() == 0f)
                {
                    this.hoeDirt.Value.shake((float)Math.PI / 32f, (float)Math.PI / 50f, Game1.random.NextDouble() < 0.5);
                    DelayedAction.playSoundAfterDelay("leafrustle", Game1.random.Next(100));
                }
                if (this.bush.Value != null)
                {
                    this.bush.Value.performUseAction(base.tileLocation, who.currentLocation);
                }
            }
            return false;
        }

        protected virtual void updateAnimation(bool ShowDrippingAnimation)
        {
            if (this.hasEnricherAttachment.Value && this.hasPlanterAttachment.Value)
            {
                if (ShowDrippingAnimation)
                {
                    this.AnimationManager.playAnimation(DRIPPING_WITH_ENRICHER_AND_PLANTER_ATTACHMENT_ANIMATION_KEY, true);
                }
                else
                {
                    this.AnimationManager.playAnimation(DEFAULT_WITH_ENRICHER_AND_PLANTER_ATTACHMENT_ANIMATION_KEY, true);
                }
                return;
            }
            if (this.hasEnricherAttachment.Value && !this.hasPlanterAttachment.Value)
            {
                if (ShowDrippingAnimation)
                {
                    this.AnimationManager.playAnimation(DRIPPING_WITH_ENRICHER_ATTACHMENT_ANIMATION_KEY, true);
                }
                else
                {
                    this.AnimationManager.playAnimation(DEFAULT_WITH_ENRICHER_ATTACHMENT_ANIMATION_KEY, true);
                }
                return;
            }
            if (!this.hasEnricherAttachment.Value && this.hasPlanterAttachment.Value)
            {
                if (ShowDrippingAnimation)
                {
                    this.AnimationManager.playAnimation(DRIPPING_WITH_PLANTER_ATTACHMENT_ANIMATION_KEY, true);
                }
                else
                {
                    this.AnimationManager.playAnimation(DEFAULT_WITH_PLANTER_ATTACHMENT_ANIMATION_KEY, true);
                }
                return;
            }

            if (ShowDrippingAnimation)
            {
                this.AnimationManager.playAnimation(DRIPPING_ANIMATION_KEY, true);
            }
            else
            {
                this.AnimationManager.playAnimation(DEFAULT_ANIMATION_KEY, true);
            }
            return;

        }

        public override void actionOnPlayerEntry()
        {
            //base.actionOnPlayerEntry();
            this.updateAnimation(true);
            if (this.hoeDirt.Value != null)
            {
                this.hoeDirt.Value.performPlayerEntryAction(base.tileLocation);
            }
        }

        public override void updateWhenCurrentLocation(GameTime time, GameLocation environment)
        {
            //base.updateWhenCurrentLocation(time, environment);
            this.hoeDirt.Value.tickUpdate(time, base.tileLocation, environment);
            this.bush.Value?.tickUpdate(time, environment);
            if ((bool)this.bushLoadDirty)
            {
                this.bush.Value?.loadSprite();
                this.bushLoadDirty.Value = false;
            }
        }


        public virtual void makeSoilWet()
        {
            this.hoeDirt.Value.state.Value = 1;
        }


        public override void DayUpdate(GameLocation location)
        {
            // base.DayUpdate(location)

            this.hoeDirt.Value.dayUpdate(location, base.tileLocation);
            this.makeSoilWet();
            base.showNextIndex.Value = (int)this.hoeDirt.Value.state == 1;
            if (base.heldObject.Value != null)
            {
                base.readyForHarvest.Value = true;
            }
            if (this.bush.Value != null)
            {
                this.bush.Value.dayUpdate(location);
            }
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            this.DrawICustomModObjectInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
        }

        public override void drawAttachments(SpriteBatch b, int x, int y)
        {
            base.drawAttachments(b, x, y);
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            this.DrawICustomModObjectWhenHeld(spriteBatch, objectPosition, f);
        }

        /// <summary>What happens when the object is drawn at a tile location.</summary>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            this.DrawICustomModObject(spriteBatch, alpha);

            if ((int)this.hoeDirt.Value.fertilizer != 0)
            {
                Rectangle fertilizer_rect = this.hoeDirt.Value.GetFertilizerSourceRect(this.hoeDirt.Value.fertilizer);
                fertilizer_rect.Width = 13;
                fertilizer_rect.Height = 13;
                spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(base.tileLocation.X * 64f + 4f, base.tileLocation.Y * 64f - 12f)), fertilizer_rect, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (base.tileLocation.Y + 0.65f) * 64f / 10000f + (float)x * 1E-05f);
            }
            if (this.hoeDirt.Value.crop != null)
            {
                //  this.hoeDirt.Value.crop.drawWithOffset(spriteBatch, base.tileLocation, ((int)this.hoeDirt.Value.state == 1 && (int)this.hoeDirt.Value.crop.currentPhase == 0 && !this.hoeDirt.Value.crop.raisedSeeds) ? (new Color(180, 100, 200) * 1f) : Color.White, this.hoeDirt.Value.getShakeRotation(), new Vector2(32f, 8f));

                this.drawCropWithOffset(spriteBatch, this.TileLocation, ((int)this.hoeDirt.Value.state == 1 && (int)this.hoeDirt.Value.crop.currentPhase == 0 && !this.hoeDirt.Value.crop.raisedSeeds) ? (new Color(180, 100, 200) * 1f) : Color.White, this.hoeDirt.Value.getShakeRotation(), new Vector2(32f, 8f), (this.TileLocation.Y - this.basicItemInformation.drawOffset.Y));
            }
            if (base.heldObject.Value != null)
            {
                base.heldObject.Value.draw(spriteBatch, x * 64, y * 64 - 48 + 64, (base.tileLocation.Y + 0.66f) * 64f / 10000f + (float)x * 1E-05f, 1f);
            }
            if (this.bush.Value != null)
            {
                this.bush.Value.draw(spriteBatch, new Vector2(x, y + 64), -24f);
            }
        }

        public virtual void drawCropWithOffset(SpriteBatch b, Vector2 tileLocation, Color toTint, float rotation, Vector2 offset, float YTileDepthOffset)
        {

            if ((bool)this.Crop.forageCrop)
            {
                b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, offset + new Vector2(tileLocation.X * 64f, tileLocation.Y * 64f)), this.sourceRect, Color.White, 0f, new Vector2(8f, 8f), 4f, SpriteEffects.None, (tileLocation.Y + 0.66f) * 64f / 10000f + tileLocation.X * 1E-05f);
                return;
            }
            Rectangle coloredSourceRect = new Rectangle(((!this.Crop.fullyGrown) ? ((int)this.Crop.currentPhase + 1 + 1) : (((int)this.Crop.dayOfCurrentPhase <= 0) ? 6 : 7)) * 16 + (((int)this.Crop.rowInSpriteSheet % 2 != 0) ? 128 : 0), (int)this.Crop.rowInSpriteSheet / 2 * 16 * 2, 16, 32); ;

            float originalDepth = (YTileDepthOffset - .5f + 0.66f) * 64f / 10000f + tileLocation.X * 1E-05f;
            float modDepth = Math.Max(0f, (float)((YTileDepthOffset) * Game1.tileSize) / 10000f) + .00001f;

            float depth = originalDepth;//Math.Max(originalDepth, modDepth);

            b.Draw(Game1.cropSpriteSheet, Game1.GlobalToLocal(Game1.viewport, offset + new Vector2(tileLocation.X * 64f, tileLocation.Y * 64f)), this.Crop.getSourceRect((int)tileLocation.X * 7 + (int)tileLocation.Y * 11), toTint, rotation, new Vector2(8f, 24f), 4f, this.Crop.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, depth + .00001f);
            if (!this.Crop.tintColor.Equals(Color.White) && (int)this.Crop.currentPhase == this.Crop.phaseDays.Count - 1 && !this.Crop.dead)
            {
                b.Draw(Game1.cropSpriteSheet, Game1.GlobalToLocal(Game1.viewport, offset + new Vector2(tileLocation.X * 64f, tileLocation.Y * 64f)), coloredSourceRect, this.Crop.tintColor, rotation, new Vector2(8f, 24f), 4f, this.Crop.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, depth + .00002f);
            }
        }

        public override Item getOne()
        {
            IrrigatedGardenPot pot = new IrrigatedGardenPot(this.basicItemInformation.Copy(), Vector2.Zero);
            pot.Crop = this.Crop;
            return pot;
        }


        public override bool canStackWith(ISalable other)
        {
            if (other is IrrigatedGardenPot)
            {

                IrrigatedGardenPot otherPot = (IrrigatedGardenPot)other;
                if (this.Crop != null && otherPot.Crop != null)
                {

                    if (this.Crop.GetType() == typeof(Crop) && otherPot.Crop.GetType() == typeof(Crop))
                    {

                        if (this.Crop.netSeedIndex.Value == otherPot.Crop.netSeedIndex.Value)
                        {
                            if (this.Crop.dayOfCurrentPhase.Value == otherPot.Crop.dayOfCurrentPhase.Value)
                            {
                                if (this.Crop.currentPhase.Value == otherPot.Crop.currentPhase.Value)
                                {
                                    if (this.hoeDirt.Value.fertilizer.Value == otherPot.hoeDirt.Value.fertilizer.Value)
                                    {
                                        if (this.Crop.dead == otherPot.Crop.dead)
                                        {

                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }

                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }


            return base.canStackWith(other);
        }
    }
}
