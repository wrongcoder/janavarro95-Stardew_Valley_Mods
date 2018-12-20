using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PyTK.CustomElementHandler;
using Revitalize.Framework.Graphics;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Objects
{
    /// <summary>
    /// Change draw functions.
    /// </summary>
    public class CustomObject : PySObject
    {

        public string id;
        public Texture2DExtended texture;
        public BasicItemInformation info;
        public Texture2D displayTexture
        {
            get
            {
                return texture.texture;
            }
        }


        public CustomObject()
        {
            
        }

        public CustomObject(BasicItemInformation info):base(info,Vector2.Zero)
        {
            this.info = info;
            this.initializeBasics();
        }

        public CustomObject(BasicItemInformation info,Vector2 TileLocation) : base(info, TileLocation)
        {
            this.info = info;
            this.initializeBasics();
        }

        public virtual void initializeBasics()
        {
            this.name = info.name;
            this.displayName = getDisplayNameFromStringsFile(this.id);
            this.Edibility = info.edibility;
            this.Category = -9; //For crafting.
            this.displayName = info.name;
            this.setOutdoors.Value = true;
            this.setIndoors.Value = true;
            this.isLamp.Value = false;
            this.fragility.Value = 0;
        }

        public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
        {
            if (justCheckingForActivity)
                return true;
            Revitalize.ModCore.ModMonitor.Log("Interact with core object!");
            return true;
        }

        public override ICustomObject recreate(Dictionary<string, string> additionalSaveData, object replacement)
        {
            BasicItemInformation data =(BasicItemInformation) CustomObjectData.collection[additionalSaveData["id"]];
            return new CustomObject((BasicItemInformation) CustomObjectData.collection[additionalSaveData["id"]], (replacement as Chest).TileLocation);
        }


        public override Color getCategoryColor()
        {
            return info.categoryColor;
            //return data.categoryColor;
        }

        public override string getCategoryName()
        {
            return info.categoryName;
        }

        public override string getDescription()
        {
            string text = info.description;
            SpriteFont smallFont = Game1.smallFont;
            int width = Game1.tileSize * 4 + Game1.tileSize / 4;
            return Game1.parseText(text, smallFont, width);
        }

        public override Item getOne()
        {
            return new CustomObject((BasicItemInformation)this.data);
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            base.draw(spriteBatch, x, y, alpha);
        }

        public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1)
        {
            base.draw(spriteBatch, xNonTile, yNonTile, layerDepth, alpha);
        }

        public override void drawAsProp(SpriteBatch b)
        {
            base.drawAsProp(b);
        }

        public override void drawAttachments(SpriteBatch b, int x, int y)
        {
            base.drawAttachments(b, x, y);
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber, Color color, bool drawShadow)
        {
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
        }

        public override void drawPlacementBounds(SpriteBatch spriteBatch, GameLocation location)
        {
            base.drawPlacementBounds(spriteBatch, location);
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            base.drawWhenHeld(spriteBatch, objectPosition, f);
        }


        public string getDisplayNameFromStringsFile(string objectID)
        {
            //Load in a file that has all object names referenced here or something.
            return info.name;
        }

    }
}
