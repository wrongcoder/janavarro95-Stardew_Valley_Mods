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
    public class MultiTiledObject:CustomObject
    {
        public Dictionary<Vector2, MultiTiledComponent> objects;

        public MultiTiledObject() : base()
        {

            this.objects = new Dictionary<Vector2, MultiTiledComponent>();
        }

        public MultiTiledObject(BasicItemInformation info) : base(info)
        {
            this.objects = new Dictionary<Vector2, MultiTiledComponent>();
        }

        public MultiTiledObject(BasicItemInformation info, Vector2 TileLocation) : base(info, TileLocation)
        {
            this.objects = new Dictionary<Vector2, MultiTiledComponent>();
        }

        public MultiTiledObject(BasicItemInformation info, Vector2 TileLocation, Dictionary<Vector2,MultiTiledComponent> ObjectsList) : base(info, TileLocation)
        {
            this.objects = new Dictionary<Vector2, MultiTiledComponent>();
            foreach(var v in ObjectsList)
            {
                MultiTiledComponent component =(MultiTiledComponent)v.Value.getOne();
                this.addComponent(v.Key, component);
            }
        }

        public bool addComponent(Vector2 key, MultiTiledComponent obj)
        {
            if (this.objects.ContainsKey(key))
            {
                return false;
            }
            else
            {
                this.objects.Add(key, obj);
                obj.containerObject = this;
                return true;
            }
        }

        public bool removeComponent(Vector2 key)
        {
            if (!this.objects.ContainsKey(key))
            {
                return false;
            }
            else
            {
                this.objects.Remove(key);
                return true;
            }
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            foreach (KeyValuePair<Vector2, MultiTiledComponent> pair in this.objects)
            {
                pair.Value.draw(spriteBatch, x + (int)pair.Key.X * Game1.tileSize, y + (int)pair.Key.Y * Game1.tileSize, alpha);
            }
        }

        public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1)
        {
            foreach (KeyValuePair<Vector2, MultiTiledComponent> pair in this.objects)
            {
                pair.Value.draw(spriteBatch, xNonTile + (int)pair.Key.X * Game1.tileSize, yNonTile+ (int)pair.Key.Y * Game1.tileSize, layerDepth, alpha);        
            }

            //base.draw(spriteBatch, xNonTile, yNonTile, layerDepth, alpha);
        }

        public override void drawAsProp(SpriteBatch b)
        {
            base.drawAsProp(b);
        }

        public override void drawAttachments(SpriteBatch b, int x, int y)
        {
            base.drawAttachments(b, x, y);
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber, Color c, bool drawShadow)
        {
            foreach (KeyValuePair<Vector2, MultiTiledComponent> pair in this.objects)
            {
                pair.Value.drawInMenu(spriteBatch, location + (pair.Key*16), 1.0f, transparency, layerDepth, drawStackNumber, c, drawShadow);
            }
                //base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, c, drawShadow);
        }

        public override void drawPlacementBounds(SpriteBatch spriteBatch, GameLocation location)
        {
            base.drawPlacementBounds(spriteBatch, location);
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {

            foreach(KeyValuePair<Vector2,MultiTiledComponent> pair in this.objects)
            {
                pair.Value.drawWhenHeld(spriteBatch, objectPosition + (pair.Key * Game1.tileSize), f);
            }
            //base.drawWhenHeld(spriteBatch, objectPosition, f);
        }



        //IMPLEMENT THESE!


        public virtual void pickUp()
        {
            bool canPickUp= this.removeAndAddToPlayersInventory();
            if (canPickUp)
            {
                foreach (KeyValuePair<Vector2, MultiTiledComponent> pair in this.objects)
                {
                    pair.Value.removeFromLocation(pair.Value.location,pair.Key);
                }
                this.location = null;
            }
            else
            {
                Game1.showRedMessage("NOOOOOOOO");
            }
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

            foreach(KeyValuePair<Vector2,MultiTiledComponent> pair in this.objects)
            {              
                pair.Value.placementAction(location, x + (int)pair.Key.X*Game1.tileSize, y + (int)pair.Key.Y*Game1.tileSize, who);
                Revitalize.ModCore.log(pair.Value.TileLocation);
            }
            this.location = location;
            return true;
            //return base.placementAction(location, x, y, who);
        }

        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {
            foreach(KeyValuePair<Vector2,MultiTiledComponent> pair in this.objects)
            {
                if (pair.Value.canBePlacedHere(l, tile + pair.Key) == false) return false;
            }
            return true;

        }
        public override bool clicked(Farmer who)
        {
            Revitalize.ModCore.log("WTF IS HAPPENING???");
            bool cleanUp=clicked(who);
            if (cleanUp)
            {
                pickUp();
            }
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
            return new MultiTiledObject(this.info, this.TileLocation,this.objects);
        }

        public override ICustomObject recreate(Dictionary<string, string> additionalSaveData, object replacement)
        {
            BasicItemInformation data = (BasicItemInformation)CustomObjectData.collection[additionalSaveData["id"]];
            return new MultiTiledObject((BasicItemInformation)CustomObjectData.collection[additionalSaveData["id"]], (replacement as Chest).TileLocation,this.objects);
        }
    }
}
