using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.Utilities.Extensions;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.StardustCore.Animations;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Items.Tools
{
    public class CustomItem: StardewValley.Object
    {
        public readonly NetRef<BasicItemInformation> netBasicItemInformation = new NetRef<BasicItemInformation>();

        [XmlElement("basicItemInfo")]
        public BasicItemInformation basicItemInformation
        {
            get
            {
                return this.netBasicItemInformation.Value;
            }
            set
            {
                this.netBasicItemInformation.Value = value;
            }
        }

        [XmlIgnore]
        public AnimationManager AnimationManager
        {
            get
            {
                if (this.basicItemInformation == null) return null;
                if (this.basicItemInformation.animationManager == null) return null;
                return this.basicItemInformation.animationManager;
            }
        }

        [XmlIgnore]
        public Texture2D CurrentTextureToDisplay
        {

            get
            {
                if (this.AnimationManager == null) return null;
                return this.AnimationManager.getTexture();
            }
        }

        public override string Name
        {
            get
            {
                if (this.basicItemInformation == null) return null;
                return this.basicItemInformation.name.Value;
            }
            set
            {
                if (this.basicItemInformation != null)
                {
                    this.basicItemInformation.name.Value = value;
                }
            }


        }
        public override string DisplayName
        {
            get
            {
                if (this.basicItemInformation == null) return null;
                return this.basicItemInformation.name.Value;
            }
            set
            {
                if (this.basicItemInformation != null)
                {
                    this.basicItemInformation.name.Value = value;
                }
            }
        }

        public CustomItem()
        {


        }

        public CustomItem(BasicItemInformation info):base(Vector2.Zero,0)
        {
            this.basicItemInformation = info;
            this.initNetFieldsPostConstructor();
            
        }

        /// <summary>
        /// When this item is clicked in the world.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool clicked(Farmer who)
        {
            //Game1.showRedMessage("Clicked");
            return base.clicked(who);
        }

        /// <summary>
        /// When item is using the right click input.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public override bool performUseAction(GameLocation location)
        {
          //  Game1.showRedMessage("Use action");
            return base.performUseAction(location);
        }

        /// <summary>
        /// When placed into the world.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool performDropDownAction(Farmer who)
        {
         //   Game1.showRedMessage("DropDown");
            return base.performDropDownAction(who);
        }

        /// <summary>
        /// Used when something is dropped into this object, such as fruit into a keg.
        /// </summary>
        /// <param name="dropInItem"></param>
        /// <param name="probe"></param>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool performObjectDropInAction(Item dropInItem, bool probe, Farmer who)
        {
           // Game1.showRedMessage("Drop-in");
            return base.performObjectDropInAction(dropInItem, probe, who);
        }

        /// <summary>
        /// When a tool is used on this placed object.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public override bool performToolAction(Tool t, GameLocation location)
        {
           // Game1.showRedMessage("Tool action");
            return base.performToolAction(t, location);
        }

        /// <summary>
        /// When removed from world
        /// </summary>
        /// <param name="tileLocation"></param>
        /// <param name="environment"></param>
        public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
        {
           // Game1.showRedMessage("Remove action");
            base.performRemoveAction(tileLocation, environment);
        }

        /// <summary>
        /// Initializes NetFields to send information for multiplayer after all of the constructor initialization for this object has taken place.
        /// </summary>
        protected virtual void initNetFieldsPostConstructor()
        {
            if (this.basicItemInformation != null)
            {
                this.NetFields.AddFields(this.netBasicItemInformation);
            }

        }

        public override Color getCategoryColor()
        {
            return this.basicItemInformation.categoryColor.Value;
        }

        public override string getCategoryName()
        {
            return this.basicItemInformation.categoryName.Value;
        }

        public override string getDescription()
        {
            return this.basicItemInformation.description.Value;
        }


        public override Item getOne()
        {
            return new CustomItem(this.basicItemInformation.Copy());
        }

        public override void drawAsProp(SpriteBatch b)
        {
            base.drawAsProp(b);
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            if (this.AnimationManager == null)
            {

                RevitalizeModCore.log("Animation Manager null for item: " + this.basicItemInformation.id);
                return;
            }
            if (this.CurrentTextureToDisplay == null)
            {
                RevitalizeModCore.log("Texture null for item: " + this.basicItemInformation.id);
                RevitalizeModCore.log("Expected path should be: " + this.basicItemInformation.animationManager.objectTexture.path);


                return;
            }

            if (this.basicItemInformation == null) return;

            int scaleNerfing = Math.Max(this.AnimationManager.getCurrentAnimationFrameRectangle().Width, this.AnimationManager.getCurrentAnimationFrameRectangle().Height) / 16;

            spriteBatch.Draw(this.CurrentTextureToDisplay, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize)), new Rectangle?(this.AnimationManager.getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * transparency, 0f, new Vector2((float)(this.AnimationManager.getCurrentAnimationFrameRectangle().Width / 2), (float)(this.AnimationManager.getCurrentAnimationFrameRectangle().Height)), (scaleSize * 4f) / scaleNerfing, SpriteEffects.None, layerDepth);

            if (drawStackNumber.ShouldDrawFor(this) && this.maximumStackSize() > 1 && ((double)scaleSize > 0.3 && this.Stack != int.MaxValue) && this.Stack > 1)
                Utility.drawTinyDigits(this.Stack, spriteBatch, location + new Vector2((float)(Game1.tileSize - Utility.getWidthOfTinyDigitString(this.Stack, 3f * scaleSize)) + 3f * scaleSize, (float)((double)Game1.tileSize - 18.0 * (double)scaleSize + 2.0)), 3f * scaleSize, 1f, Color.White);
            if (drawStackNumber.ShouldDrawFor(this) && this.Quality > 0)
            {
                float num = this.Quality < 4 ? 0.0f : (float)((Math.Cos((double)Game1.currentGameTime.TotalGameTime.Milliseconds * Math.PI / 512.0) + 1.0) * 0.0500000007450581);
                spriteBatch.Draw(Game1.mouseCursors, location + new Vector2(12f, (float)(Game1.tileSize - 12) + num), new Microsoft.Xna.Framework.Rectangle?(this.Quality < 4 ? new Microsoft.Xna.Framework.Rectangle(338 + (this.Quality - 1) * 8, 400, 8, 8) : new Microsoft.Xna.Framework.Rectangle(346, 392, 8, 8)), Color.White * transparency, 0.0f, new Vector2(4f, 4f), (float)(3.0 * (double)scaleSize * (1.0 + (double)num)), SpriteEffects.None, layerDepth);
            }
        }

        public override void drawAttachments(SpriteBatch b, int x, int y)
        {
            base.drawAttachments(b, x, y);
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            if (this.AnimationManager == null)
            {
                if (this.CurrentTextureToDisplay == null)
                {
                    RevitalizeModCore.log("Texture null for item: " + this.basicItemInformation.id);
                    return;
                }
            }

            if (f.ActiveObject.bigCraftable.Value)
            {
                spriteBatch.Draw(this.CurrentTextureToDisplay, objectPosition, this.AnimationManager.getCurrentAnimationFrameRectangle(), this.basicItemInformation.DrawColor, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
                return;
            }

            spriteBatch.Draw(this.CurrentTextureToDisplay, objectPosition, this.AnimationManager.getCurrentAnimationFrameRectangle(), this.basicItemInformation.DrawColor, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
            if (f.ActiveObject != null && f.ActiveObject.Name.Contains("="))
            {
                spriteBatch.Draw(this.CurrentTextureToDisplay, objectPosition + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), this.AnimationManager.getCurrentAnimationFrameRectangle(), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), (float)Game1.pixelZoom + Math.Abs(Game1.starCropShimmerPause) / 8f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
                if (Math.Abs(Game1.starCropShimmerPause) <= 0.05f && Game1.random.NextDouble() < 0.97)
                {
                    return;
                }
                Game1.starCropShimmerPause += 0.04f;
                if (Game1.starCropShimmerPause >= 0.8f)
                {
                    Game1.starCropShimmerPause = -0.8f;
                }
            }
            //base.drawWhenHeld(spriteBatch, objectPosition, f);
        }

        /// <summary>What happens when the object is drawn at a tile location.</summary>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            if (x <= -1)
            {
                x = (int)this.TileLocation.X;
                //spriteBatch.Draw(this.basicItemInfo.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, this.TileLocation), new Rectangle?(this.AnimationManager.currentAnimation.sourceRectangle), this.basicItemInfo.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)(this.TileLocation.Y * Game1.tileSize) / 10000f));
            }
            if (y <= -1)
            {
                y = (int)this.TileLocation.Y;
            }

            if (this.AnimationManager == null)
            {
                spriteBatch.Draw(this.basicItemInformation.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize) + this.basicItemInformation.shakeTimerOffset(), (y * Game1.tileSize) + this.basicItemInformation.shakeTimerOffset())), new Rectangle?(this.AnimationManager.getCurrentAnimation().getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)(y * Game1.tileSize) / 10000f));
            }
            else
            {
                this.basicItemInformation.animationManager.draw(spriteBatch, this.basicItemInformation.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize) + this.basicItemInformation.shakeTimerOffset(), (y * Game1.tileSize) + this.basicItemInformation.shakeTimerOffset())), new Rectangle?(this.AnimationManager.getCurrentAnimation().getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)((y) * Game1.tileSize) / 10000f) + .00001f);
                if (this.heldObject.Value != null) SpriteBatchUtilities.Draw(spriteBatch, this, this.heldObject.Value, alpha, 0);
            }
        }

        public override void drawPlacementBounds(SpriteBatch spriteBatch, GameLocation location)
        {
            //Need to update this????
            base.drawPlacementBounds(spriteBatch, location);
        }

        /// <summary>Draw the game object at a non-tile spot. Aka like debris.</summary>
        public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1f)
        {

            if (this.AnimationManager == null)
            {
                spriteBatch.Draw(this.CurrentTextureToDisplay, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile), yNonTile)), new Rectangle?(this.AnimationManager.getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, layerDepth));
            }

            else
            {
                //Log.AsyncC("Animation Manager is working!");
                this.AnimationManager.draw(spriteBatch, this.CurrentTextureToDisplay, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile), yNonTile)), new Rectangle?(this.AnimationManager.getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, layerDepth));
            }
        }

        public override int maximumStackSize()
        {
            return 999;
        }

        public override bool isPlaceable()
        {
            return false;
        }
    }
}
