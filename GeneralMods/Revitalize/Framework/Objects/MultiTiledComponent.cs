using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PyTK.CustomElementHandler;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Objects
{
    public class MultiTiledComponent:CustomObject
    {

        public MultiTiledObject containerObject;

        public MultiTiledComponent():base()
        {

        }

        public MultiTiledComponent(BasicItemInformation info):base(info)
        {

        }

        public MultiTiledComponent(BasicItemInformation info, Vector2 TileLocation): base(info, TileLocation)
        {

        }

        public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
        {
            //Revitalize.ModCore.log("Checking for a clicky click???");
            return base.checkForAction(who, justCheckingForActivity);
        }

        public override bool clicked(Farmer who)
        {

            Revitalize.ModCore.log("Clicked a multiTiledComponent!");
            this.containerObject.pickUp();
            return true;
            //return base.clicked(who);
        }

        

        public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
        {
            base.performRemoveAction(this.TileLocation, environment);
        }

        public virtual void removeFromLocation(GameLocation location,Vector2 offset)
        {
            location.removeObject(this.TileLocation,false);
            //this.performRemoveAction(this.TileLocation,location);
        }

        /// <summary>
        /// Places an object down.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
        {
            Revitalize.ModCore.ModMonitor.Log("SCREAMING!!!!");
            this.updateDrawPosition(x,y);
            this.location = location;

            this.TileLocation = new Vector2((int)(x / Game1.tileSize), (int)(y / Game1.tileSize));

            return base.placementAction(location, x, y, who);
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber, Color c, bool drawShadow)
        {
            if (drawStackNumber && this.maximumStackSize() > 1 && ((double)scaleSize > 0.3 && this.Stack != int.MaxValue) && this.Stack > 1)
                Utility.drawTinyDigits(this.Stack, spriteBatch, location + new Vector2((float)(Game1.tileSize - Utility.getWidthOfTinyDigitString(this.Stack, 3f * scaleSize)) + 3f * scaleSize, (float)((double)Game1.tileSize - 18.0 * (double)scaleSize + 2.0)), 3f * scaleSize, 1f, Color.White);
            if (drawStackNumber && this.Quality > 0)
            {
                float num = this.Quality < 4 ? 0.0f : (float)((Math.Cos((double)Game1.currentGameTime.TotalGameTime.Milliseconds * Math.PI / 512.0) + 1.0) * 0.0500000007450581);
                spriteBatch.Draw(Game1.mouseCursors, location + new Vector2(12f, (float)(Game1.tileSize - 12) + num), new Microsoft.Xna.Framework.Rectangle?(this.Quality < 4 ? new Microsoft.Xna.Framework.Rectangle(338 + (this.Quality - 1) * 8, 400, 8, 8) : new Microsoft.Xna.Framework.Rectangle(346, 392, 8, 8)), Color.White * transparency, 0.0f, new Vector2(4f, 4f), (float)(3.0 * (double)scaleSize * (1.0 + (double)num)), SpriteEffects.None, layerDepth);
            }
            spriteBatch.Draw(this.displayTexture, location + new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize * .75)), new Rectangle?(this.animationManager.currentAnimation.sourceRectangle), info.drawColor * transparency, 0f, new Vector2((float)(this.animationManager.currentAnimation.sourceRectangle.Width / 2), (float)(this.animationManager.currentAnimation.sourceRectangle.Height)), scaleSize, SpriteEffects.None, layerDepth);
        }

        public override Item getOne()
        {
            MultiTiledComponent component=new MultiTiledComponent(this.info, this.TileLocation);
            component.containerObject = this.containerObject;
            return component;

        }

        public override ICustomObject recreate(Dictionary<string, string> additionalSaveData, object replacement)
        {
            BasicItemInformation data = (BasicItemInformation)CustomObjectData.collection[additionalSaveData["id"]];
            MultiTiledComponent component= new MultiTiledComponent((BasicItemInformation)CustomObjectData.collection[additionalSaveData["id"]], (replacement as Chest).TileLocation);
            component.containerObject = this.containerObject;
            return containerObject;
        }

    }
}
