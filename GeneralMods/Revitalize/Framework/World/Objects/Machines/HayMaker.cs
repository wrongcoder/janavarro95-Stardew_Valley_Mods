using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Revitalize.Framework.Objects;
using Revitalize.Framework.World.Objects.InformationFiles;
using StardewValley;

namespace Revitalize.Framework.World.Objects.Machines
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.HayMaker")]
    public class HayMaker : Machine
    {
        public NetEnum<Enums.SDVObject> feedType = new NetEnum<Enums.SDVObject>(Enums.SDVObject.NULL);
        public NetBool lerpScaleIncreasing = new NetBool(true);

        public HayMaker()
        {

        }

        public HayMaker(BasicItemInformation info) : base(info)
        {

        }

        protected override void initNetFieldsPostConstructor()
        {
            base.initNetFieldsPostConstructor();
            this.NetFields.AddField(this.feedType);
        }

        public override bool rightClicked(Farmer who)
        {
            if (this.heldObject.Value != null)
            {
                if (who.IsLocalPlayer)
                {
                    this.cleanOutHayMaker(true);
                }
            }

            return base.rightClicked(who);
        }

        /// <summary>
        /// Cleans out the hay maker to produce more hay.
        /// </summary>
        /// <param name="addToPlayersInventory"></param>
        protected virtual void cleanOutHayMaker(bool addToPlayersInventory)
        {
            if (addToPlayersInventory)
            {
                Game1.playSound("coin");
                bool added = Game1.player.addItemToInventoryBool(this.heldObject.Value);
                if (added == false) return;
            }
            this.heldObject.Value = null;
            this.AnimationManager.playDefaultAnimation();
            this.feedType.Value = Enums.SDVObject.NULL;

        }

        /// <summary>
        /// Performed when dropping in an object into this feeder.
        /// </summary>
        /// <param name="dropInItem"></param>
        /// <param name="probe"></param>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool performObjectDropInAction(Item dropInItem, bool probe, Farmer who)
        {
            if (probe == true) return false; //Just checking for action.
            if (who.ActiveObject == null) return false;
            if (dropInItem == null) return false;
            if (this.MinutesUntilReady > 0) return false;
            if (this.heldObject.Value != null)
            {
                this.cleanOutHayMaker(true);
            }


            if (dropInItem.parentSheetIndex == (int)Enums.SDVObject.Corn && who.ActiveObject.Stack >= ModCore.Configs.objectConfigManager.hayMakerConfig.NumberOfCornRequired)
            {
                this.AnimationManager.playAnimation("Corn");
                this.feedType.Value = Enums.SDVObject.Corn;
                who.ActiveObject.Stack -= ModCore.Configs.objectConfigManager.hayMakerConfig.NumberOfCornRequired;
                this.MinutesUntilReady = ModCore.Configs.objectConfigManager.hayMakerConfig.MinutesToProcess;
                who.currentLocation.playSound("Ship");
                return true;
            }
            if (dropInItem.parentSheetIndex == (int)Enums.SDVObject.Fiber && who.ActiveObject.Stack >= ModCore.Configs.objectConfigManager.hayMakerConfig.NumberOfFiberRequired)
            {
                this.AnimationManager.playAnimation("Fiber");
                this.feedType.Value = Enums.SDVObject.Fiber;
                who.ActiveObject.Stack -= ModCore.Configs.objectConfigManager.hayMakerConfig.NumberOfFiberRequired;
                this.MinutesUntilReady = ModCore.Configs.objectConfigManager.hayMakerConfig.MinutesToProcess;
                who.currentLocation.playSound("Ship");
                return true;
            }
            if (dropInItem.parentSheetIndex == (int)Enums.SDVObject.Wheat && who.ActiveObject.Stack >= ModCore.Configs.objectConfigManager.hayMakerConfig.NumberOfWheatRequired)
            {
                this.AnimationManager.playAnimation("Wheat");
                this.feedType.Value = Enums.SDVObject.Hay;
                who.ActiveObject.Stack -= ModCore.Configs.objectConfigManager.hayMakerConfig.NumberOfWheatRequired;
                this.MinutesUntilReady = ModCore.Configs.objectConfigManager.hayMakerConfig.MinutesToProcess;
                who.currentLocation.playSound("Ship");
                return true;
            }
            if (dropInItem.parentSheetIndex == (int)Enums.SDVObject.Amaranth && who.ActiveObject.Stack >= ModCore.Configs.objectConfigManager.hayMakerConfig.NumberOfAmaranthRequired)
            {
                this.AnimationManager.playAnimation("Amaranth");
                this.feedType.Value = Enums.SDVObject.Amaranth;
                who.ActiveObject.Stack -= ModCore.Configs.objectConfigManager.hayMakerConfig.NumberOfAmaranthRequired;
                this.MinutesUntilReady = ModCore.Configs.objectConfigManager.hayMakerConfig.MinutesToProcess;
                who.currentLocation.playSound("Ship");
                return true;
            }


            return base.performObjectDropInAction(dropInItem, probe, who);
        }

        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            if (this.heldObject.Value != null) return false;
            this.MinutesUntilReady -= minutes;
            if (this.MinutesUntilReady < 0) this.MinutesUntilReady = 0;

            if (this.MinutesUntilReady == 0 && this.feedType.Value!= Enums.SDVObject.NULL)
            {
                if (this.feedType.Value == Enums.SDVObject.Corn)
                {
                    this.heldObject.Value = ModCore.ObjectManager.GetObject(Enums.SDVObject.Hay, ModCore.Configs.objectConfigManager.hayMakerConfig.CornToHayOutput);
                }
                if (this.feedType.Value == Enums.SDVObject.Fiber)
                {
                    this.heldObject.Value = ModCore.ObjectManager.GetObject(Enums.SDVObject.Hay, ModCore.Configs.objectConfigManager.hayMakerConfig.FiberToHayOutput);
                }
                if (this.feedType.Value == Enums.SDVObject.Wheat)
                {
                    this.heldObject.Value = ModCore.ObjectManager.GetObject(Enums.SDVObject.Hay, ModCore.Configs.objectConfigManager.hayMakerConfig.WheatToHayOutput);
                }
                if (this.feedType.Value == Enums.SDVObject.Amaranth)
                {
                    this.heldObject.Value = ModCore.ObjectManager.GetObject(Enums.SDVObject.Hay, ModCore.Configs.objectConfigManager.hayMakerConfig.AmaranthToHayOutput);
                }
                this.AnimationManager.playAnimation("Hay");
                bool noHayRemainsInFeedMaker=this.attemptToFillFarmSilos();
                if (noHayRemainsInFeedMaker == false)
                {
                    //swip and coin are valid sounds too.
                    Game1.playSound("dwop");
                }

            }
            return base.minutesElapsed(minutes, environment);
        }

        /// <summary>
        /// Attempts to automatically remove hay in the hay maker and put it into farm's Silo.
        /// </summary>
        protected virtual bool attemptToFillFarmSilos()
        {
            if (this.heldObject.Value == null) return false;
            int remainder=Game1.getFarm().tryToAddHay(this.heldObject.Value.Stack);
            if (remainder == 0)
            {
                this.cleanOutHayMaker(false);
                return true;
            }
            else
            {
                this.heldObject.Value.Stack = remainder;
                return false;
            }
        }

        protected override void drawStatusBubble(SpriteBatch b, int x, int y, float Alpha)
        {
            if (this.machineStatusBubbleBox == null) this.createStatusBubble();
            if (this.MinutesUntilReady == 0 && this.heldObject.Value != null)
            {
                y--;
                float num = (float)(4.0 * Math.Round(Math.Sin(DateTime.UtcNow.TimeOfDay.TotalMilliseconds / 250.0), 2));
                this.machineStatusBubbleBox.playAnimation("Blank");
                this.machineStatusBubbleBox.draw(b, this.machineStatusBubbleBox.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize + num)), new Rectangle?(this.machineStatusBubbleBox.getCurrentAnimationFrameRectangle()), Color.White * ModCore.Configs.machinesConfig.machineNotificationBubbleAlpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)((y + 2) * Game1.tileSize) / 10000f) + .00001f);

                Rectangle itemSourceRectangle = GameLocation.getSourceRectForObject(this.heldObject.Value.ParentSheetIndex);
                this.machineStatusBubbleBox.draw(b, Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize)+8, y * Game1.tileSize + num+16)), new Rectangle?(itemSourceRectangle), Color.White * ModCore.Configs.machinesConfig.machineNotificationBubbleAlpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)((y + 2) * Game1.tileSize) / 10000f) + .00002f);

            }
        }

        /// <summary>
        /// A simple method for calculating the scale size for showing a machine working.
        /// </summary>
        /// <returns></returns>
        public virtual float getScaleSizeForWorkingMachine()
        {
            float zoomSpeed = 0.01f;
            if (this.Scale.X < Game1.pixelZoom)
            {
                this.Scale = new Vector2(Game1.pixelZoom, Game1.pixelZoom);
            }

            if (this.feedType.Value != Enums.SDVObject.NULL && base.MinutesUntilReady > 0)
            {
                if (this.lerpScaleIncreasing.Value == true)
                {
                    this.Scale = new Vector2(this.scale.X + zoomSpeed, this.scale.Y + zoomSpeed);
                    if (this.Scale.X >= 5.0)
                    {
                        this.lerpScaleIncreasing.Value = false;
                    }
                }
                else
                {
                    this.Scale = new Vector2(this.scale.X - zoomSpeed, this.scale.Y - zoomSpeed);
                    if (this.Scale.X <= Game1.pixelZoom)
                    {
                        this.lerpScaleIncreasing.Value = true;
                    }
                }
                return this.Scale.X * Game1.options.zoomLevel;
               
            }
            else
            {
                float zoom = Game1.pixelZoom * Game1.options.zoomLevel;
                return zoom;
            }
        }

        /// <summary>What happens when the object is drawn at a tile location.</summary>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            if (x <= -1)
            {
                x = (int)this.TileLocation.X;
            }
            if (y <= -1)
            {
                y = (int)this.TileLocation.Y;
            }

            if (this.AnimationManager == null)
            {
                spriteBatch.Draw(this.basicItemInfo.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize) + this.basicItemInfo.shakeTimerOffset(), (y * Game1.tileSize) + this.basicItemInfo.shakeTimerOffset())), new Rectangle?(this.AnimationManager.getCurrentAnimation().getCurrentAnimationFrameRectangle()), this.basicItemInfo.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)(y * Game1.tileSize) / 10000f));
            }
            else
            {
                if (this.feedType.Value != Enums.SDVObject.NULL && base.MinutesUntilReady > 0)
                {
                    Vector2 origin = new Vector2(this.AnimationManager.getCurrentAnimationFrameRectangle().Width/2, this.AnimationManager.getCurrentAnimationFrameRectangle().Height);
                    
                    this.basicItemInfo.animationManager.draw(spriteBatch, this.basicItemInfo.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize) + this.basicItemInfo.shakeTimerOffset()+(Game1.tileSize * origin.X/ this.AnimationManager.getCurrentAnimationFrameRectangle().Width) , (y * Game1.tileSize) + this.basicItemInfo.shakeTimerOffset()+(Game1.tileSize * (origin.Y/ this.AnimationManager.getCurrentAnimationFrameRectangle().Height +1) ))), new Rectangle?(this.AnimationManager.getCurrentAnimation().getCurrentAnimationFrameRectangle()), this.basicItemInfo.DrawColor * alpha, 0f, origin, this.getScaleSizeForWorkingMachine(), this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)((y) * Game1.tileSize) / 10000f) + .00001f);
                }

                else
                {

                    this.basicItemInfo.animationManager.draw(spriteBatch, this.basicItemInfo.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize) + this.basicItemInfo.shakeTimerOffset(), (y * Game1.tileSize) + this.basicItemInfo.shakeTimerOffset())), new Rectangle?(this.AnimationManager.getCurrentAnimation().getCurrentAnimationFrameRectangle()), this.basicItemInfo.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)((y) * Game1.tileSize) / 10000f) + .00001f);
                }
            }

            if(this.MinutesUntilReady==0 && this.heldObject.Value != null)
            {
                this.drawStatusBubble(spriteBatch, x, y, alpha);
            }

        }

        public override bool doesMachineProduceItems()
        {
            return true;
        }


        public override Item getOne()
        {
            return new HayMaker(this.basicItemInfo.Copy());
        }
    }
}
