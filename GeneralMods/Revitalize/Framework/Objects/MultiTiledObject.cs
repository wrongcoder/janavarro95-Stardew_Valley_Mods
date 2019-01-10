using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using PyTK.CustomElementHandler;
using StardewValley;
using StardewValley.Objects;

namespace Revitalize.Framework.Objects
{
    public class MultiTiledObject : CustomObject
    {
        public Dictionary<Vector2, StardewValley.Object> objects;
        [JsonIgnore]
        public Dictionary<MultiTiledComponent, Vector2> offSets;

        public Guid guid;

        private int width;
        private int height;
        public int Width
        {
            get
            {
                return this.width+1;
            }
        }
        public int Height
        {
            get
            {
                return this.height+1;
            }
        }

        public MultiTiledObject()
        {
            this.objects = new Dictionary<Vector2, StardewValley.Object>();
            this.offSets = new Dictionary<MultiTiledComponent, Vector2>();
            this.guid = Guid.NewGuid();
        }

        public MultiTiledObject(BasicItemInformation info)
            : base(info)
        {
            this.objects = new Dictionary<Vector2, StardewValley.Object>();
            this.offSets = new Dictionary<MultiTiledComponent, Vector2>();
            this.guid = Guid.NewGuid();
        }

        public MultiTiledObject(BasicItemInformation info, Vector2 TileLocation)
            : base(info, TileLocation)
        {
            this.objects = new Dictionary<Vector2, StardewValley.Object>();
            this.offSets = new Dictionary<MultiTiledComponent, Vector2>();
            this.guid = Guid.NewGuid();
        }

        public MultiTiledObject(BasicItemInformation info, Vector2 TileLocation, Dictionary<Vector2, StardewValley.Object> ObjectsList)
            : base(info, TileLocation)
        {
            this.objects = new Dictionary<Vector2, StardewValley.Object>();
            this.offSets = new Dictionary<MultiTiledComponent, Vector2>();
            foreach (var v in ObjectsList)
            {
                MultiTiledComponent component = (MultiTiledComponent)v.Value.getOne();
                this.addComponent(v.Key, component);
            }
            this.guid = Guid.NewGuid();

        }

        public bool addComponent(Vector2 key, MultiTiledComponent obj)
        {
            if (this.objects.ContainsKey(key))
                return false;

            this.objects.Add(key, obj);
            this.offSets.Add(obj, key);
            if (key.X > this.width) this.width = (int)key.X;
            if (key.Y > this.height) this.height = (int)key.Y;
            obj.containerObject = this;
            obj.offsetKey = key;
            return true;
        }

        public bool removeComponent(Vector2 key)
        {
            if (!this.objects.ContainsKey(key))
                return false;

            this.objects.Remove(key);
            return true;
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            foreach (KeyValuePair<Vector2, StardewValley.Object> pair in this.objects)
                pair.Value.draw(spriteBatch, x + (int)pair.Key.X * Game1.tileSize, y + (int)pair.Key.Y * Game1.tileSize, alpha);
        }

        public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1)
        {
            foreach (KeyValuePair<Vector2, StardewValley.Object> pair in this.objects)
                pair.Value.draw(spriteBatch, xNonTile + (int)pair.Key.X * Game1.tileSize, yNonTile + (int)pair.Key.Y * Game1.tileSize, layerDepth, alpha);

            //base.draw(spriteBatch, xNonTile, yNonTile, layerDepth, alpha);
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber, Color c, bool drawShadow)
        {
            foreach (KeyValuePair<Vector2, StardewValley.Object> pair in this.objects)
                pair.Value.drawInMenu(spriteBatch, location + (pair.Key * 16), 1.0f, transparency, layerDepth, drawStackNumber, c, drawShadow);
            //base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, c, drawShadow);
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            foreach (KeyValuePair<Vector2, StardewValley.Object> pair in this.objects)
                pair.Value.drawWhenHeld(spriteBatch, objectPosition + (pair.Key * Game1.tileSize), f);
            //base.drawWhenHeld(spriteBatch, objectPosition, f);
        }

        //IMPLEMENT THESE!


        public virtual void pickUp()
        {
            bool canPickUp = this.removeAndAddToPlayersInventory();
            if (canPickUp)
            {
                foreach (KeyValuePair<Vector2, StardewValley.Object> pair in this.objects)
                    (pair.Value as MultiTiledComponent).removeFromLocation((pair.Value as MultiTiledComponent).location, pair.Key);
                this.location = null;
            }
            else
                Game1.showRedMessage("NOOOOOOOO");
        }

        public override bool removeAndAddToPlayersInventory()
        {
            if (Game1.player.isInventoryFull())
            {
                Game1.showRedMessage("Inventory full.");
                return false;
            }
            Game1.player.addItemToInventory(this);
            return true;
        }

        public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
        {
            foreach (KeyValuePair<Vector2, StardewValley.Object> pair in this.objects)
            {
                pair.Value.placementAction(location, x + (int)pair.Key.X * Game1.tileSize, y + (int)pair.Key.Y * Game1.tileSize, who);
                ModCore.log(pair.Value.TileLocation);
            }
            this.location = location;
            return true;
            //return base.placementAction(location, x, y, who);
        }

        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {
            foreach (KeyValuePair<Vector2, StardewValley.Object> pair in this.objects)
            {
                if (!pair.Value.canBePlacedHere(l, tile + pair.Key))
                    return false;
            }
            return true;

        }
        public override bool clicked(Farmer who)
        {
            ModCore.log("WTF IS HAPPENING???");
            bool cleanUp = this.clicked(who);
            if (cleanUp)
                this.pickUp();
            return cleanUp;
        }

        public override bool rightClicked(Farmer who)
        {
            return base.rightClicked(who);
        }

        public override bool shiftRightClicked(Farmer who)
        {
            return base.shiftRightClicked(who);
        }

        public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
        {

            return base.checkForAction(who, justCheckingForActivity);
        }

        public override Item getOne()
        {
            return new MultiTiledObject(this.info, this.TileLocation, this.objects);
        }

        public override ICustomObject recreate(Dictionary<string, string> additionalSaveData, object replacement)
        {

            

            MultiTiledObject self=(MultiTiledObject)Revitalize.ModCore.customObjects[additionalSaveData["id"]].getOne();

            if (!Revitalize.ModCore.ObjectGroups.ContainsKey(additionalSaveData["GUID"]))
            {
                Revitalize.ModCore.ObjectGroups.Add(additionalSaveData["GUID"], self);
                return self;
            }
            else
            {
                return Revitalize.ModCore.ObjectGroups[additionalSaveData["GUID"]];
            }

            
        }

        public override Dictionary<string, string> getAdditionalSaveData()
        {
            Dictionary<string,string> saveData= base.getAdditionalSaveData();
            saveData.Add("GUID", this.guid.ToString());
            return saveData;
        }

        public void setAllAnimationsToDefault()
        {
            foreach(KeyValuePair<Vector2, StardewValley.Object> pair in this.objects)
            {
                string animationKey = (pair.Value as MultiTiledComponent) .generateDefaultRotationalAnimationKey();
                if ((pair.Value as MultiTiledComponent).animationManager.animations.ContainsKey(animationKey))
                {
                    (pair.Value as MultiTiledComponent).animationManager.setAnimation(animationKey);
                }
            }
        }
    }
}
