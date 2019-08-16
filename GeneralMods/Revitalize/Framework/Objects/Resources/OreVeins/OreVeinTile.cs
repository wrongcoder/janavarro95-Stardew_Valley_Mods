using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PyTK.CustomElementHandler;
using Revitalize.Framework.Objects.InformationFiles;
using Revitalize.Framework.Utilities.Serialization;
using StardewValley;

namespace Revitalize.Framework.Objects.Resources.OreVeins
{
    public class OreVeinTile:MultiTiledComponent
    {
        /// <summary>
        /// Deals with information tied to the resource itself.
        /// </summary>
        public ResourceInformaton resourceInfo;
        public List<ResourceInformaton> extraDrops;
        public int healthValue;


        public OreVeinTile() : base()
        {

        }

        public OreVeinTile(CustomObjectData PyTKData, BasicItemInformation Info,ResourceInformaton Resource,List<ResourceInformaton> ExtraDrops,int Health) : base(PyTKData, Info)
        {
            this.healthValue = Health;
            this.resourceInfo = Resource;
            this.extraDrops = ExtraDrops != null ? ExtraDrops : new List<ResourceInformaton>();
            this.setHealth(this.healthValue);
        }

        public OreVeinTile(CustomObjectData PyTKData, BasicItemInformation Info, Vector2 TileLocation,ResourceInformaton Resource, List<ResourceInformaton> ExtraDrops,int Health) : base(PyTKData, Info, TileLocation)
        {

            this.healthValue = Health;
            this.resourceInfo = Resource;
            this.extraDrops = ExtraDrops != null ? ExtraDrops : new List<ResourceInformaton>();
            this.setHealth(this.healthValue);
        }


        public override bool performObjectDropInAction(Item dropInItem, bool probe, Farmer who)
        {
            return false; //this.pickUpItem()==PickUpState.DoNothing;
            //return base.performObjectDropInAction(dropInItem, probe, who);
        }

        public override bool performDropDownAction(Farmer who)
        {
            return base.performDropDownAction(who);
        }

        public override void actionOnPlayerEntry()
        {
            base.actionOnPlayerEntry();
            this.setHealth(this.healthValue);
        }

        public override void updateWhenCurrentLocation(GameTime time, GameLocation environment)
        {
            base.updateWhenCurrentLocation(time, environment);
            this.setHealth(this.healthValue);
        }

        public override void DayUpdate(GameLocation location)
        {
            base.DayUpdate(location);
            this.setHealth(this.healthValue);
        }

        //Checks for any sort of interaction IF and only IF there is a held object on this tile.
        public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
        {
            MouseState mState = Mouse.GetState();
            KeyboardState keyboardState = Game1.GetKeyboardState();

            if (mState.RightButton == ButtonState.Pressed && (!keyboardState.IsKeyDown(Keys.LeftShift) || !keyboardState.IsKeyDown(Keys.RightShift)))
            {
                return this.rightClicked(who);
            }

            if (mState.RightButton == ButtonState.Pressed && (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift)))
                return this.shiftRightClicked(who);


            //return base.checkForAction(who, justCheckingForActivity);

            if (justCheckingForActivity)
                return true;

            return true;

            //return this.clicked(who);
            //return false;
        }

        /// <summary>
        /// What happens when the player hits this with a tool.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public override bool performToolAction(Tool t, GameLocation location)
        {

            if(t is StardewValley.Tools.Pickaxe)
            {
                //ModCore.log("Hit the ore vein with a pickaxe!");
                this.damage((t as StardewValley.Tools.Pickaxe).UpgradeLevel+1);
                if (this.location != null)
                {
                    this.location.playSound("hammer");
                    ModCore.log("Ore has this much health left: "+this.healthValue);
                }
                return false;
            }
            else
            {
                return false;
            }

            //return base.performToolAction(t, location);
        }

        /// <summary>
        /// What happens when an explosion occurs for this object.
        /// </summary>
        /// <param name="who"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public override bool onExplosion(Farmer who, GameLocation location)
        {
            this.destoryVein();
            return true;
            //return base.onExplosion(who, location);
        }

        /// <summary>
        /// Applies damage to the ore vein.
        /// </summary>
        /// <param name="amount"></param>
        public void damage(int amount)
        {
            if (amount <= 0) return;
            else
            {
                this.healthValue -= amount;
                if (this.healthValue <= 0)
                {
                    this.destoryVein();
                }
            }
        }

        /// <summary>
        /// Destroys this tile for the ore vein.
        /// </summary>
        public void destoryVein()
        {
            int amount = this.resourceInfo.getNumberOfDropsToSpawn();
            Item newItem = this.resourceInfo.droppedItem.getOne();
            for(int i = 0; i < amount; i++)
            {
                Game1.createItemDebris(newItem.getOne(), this.TileLocation*Game1.tileSize, Game1.random.Next(0, 3), this.location);
            }

            if (this.extraDrops != null)
            {
                foreach (ResourceInformaton extra in this.extraDrops)
                {
                    if (extra.shouldDropResource())
                    {
                        Item extraItem = extra.droppedItem.getOne();
                        int extraAmount = extra.getNumberOfDropsToSpawn();
                        for (int i = 0; i < amount; i++)
                        {
                            Game1.createItemDebris(extraItem.getOne(), this.TileLocation * Game1.tileSize, Game1.random.Next(0, 3), this.location);
                        }
                    }
                    else
                    {
                        //Resource did not meet spawn chance.
                    }
                }
            }

            if (this.location != null)
            {
                this.location.playSound("stoneCrack");
                this.location.removeObject(this.TileLocation, false);
                this.containerObject.removeComponent(this.offsetKey);
            }
        }

        public override bool performUseAction(GameLocation location)
        {
            return base.performUseAction(location);
        }

        /// <summary>
        /// Gets called when there is no actively held item on the tile.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool clicked(Farmer who)
        {
  
            return false;
        }

        public override bool rightClicked(Farmer who)
        {
            return false;
        }


        public override bool shiftRightClicked(Farmer who)
        {
            return base.shiftRightClicked(who);
        }

        public override Item getOne()
        {
            OreVeinTile component = new OreVeinTile(this.data, this.info,this.resourceInfo,this.extraDrops,this.healthValue);
            component.containerObject = this.containerObject;
            component.offsetKey = this.offsetKey;
            return component;
        }

        public override ICustomObject recreate(Dictionary<string, string> additionalSaveData, object replacement)
        {
            //instead of using this.offsetkey.x use get additional save data function and store offset key there
            Vector2 offsetKey = new Vector2(Convert.ToInt32(additionalSaveData["offsetKeyX"]), Convert.ToInt32(additionalSaveData["offsetKeyY"]));
            OreVeinTile self = Revitalize.ModCore.Serializer.DeserializeGUID<OreVeinTile>(additionalSaveData["GUID"]);
            if (self == null)
            {
                return null;
            }

            if (!Revitalize.ModCore.ObjectGroups.ContainsKey(additionalSaveData["ParentGUID"]))
            {
                //Get new container
                OreVeinObj obj = (OreVeinObj)Revitalize.ModCore.Serializer.DeserializeGUID<OreVeinObj>(additionalSaveData["ParentGUID"]);
                self.containerObject = obj;
                obj.addComponent(offsetKey, self);
                //Revitalize.ModCore.log("ADD IN AN OBJECT!!!!");
                Revitalize.ModCore.ObjectGroups.Add(additionalSaveData["ParentGUID"], obj);
            }
            else
            {
                self.containerObject = Revitalize.ModCore.ObjectGroups[additionalSaveData["ParentGUID"]];
                Revitalize.ModCore.ObjectGroups[additionalSaveData["GUID"]].addComponent(offsetKey, self);
                //Revitalize.ModCore.log("READD AN OBJECT!!!!");
            }

            return (ICustomObject)self;
            
            //throw new Exception("Why am I trying to recreate an ore vein?");
        }

        public override Dictionary<string, string> getAdditionalSaveData()
        {
            Dictionary<string, string> saveData = base.getAdditionalSaveData();
            Revitalize.ModCore.Serializer.SerializeGUID(this.containerObject.childrenGuids[this.offsetKey].ToString(), this);

            return saveData;

        }

        /// <summary>What happens when the object is drawn at a tile location.</summary>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {

            if (this.info == null)
            {
                Revitalize.ModCore.log("info is null");
                if (this.syncObject == null) Revitalize.ModCore.log("DEAD SYNC");
            }
            if (this.animationManager == null) Revitalize.ModCore.log("Animation Manager Null");
            if (this.displayTexture == null) Revitalize.ModCore.log("Display texture is null");

            //The actual planter box being drawn.
            if (this.animationManager == null)
            {
                if (this.animationManager.getExtendedTexture() == null)
                    ModCore.ModMonitor.Log("Tex Extended is null???");

                spriteBatch.Draw(this.displayTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize)), new Rectangle?(this.animationManager.currentAnimation.sourceRectangle), this.info.drawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)(y * Game1.tileSize) / 10000f));
                // Log.AsyncG("ANIMATION IS NULL?!?!?!?!");
            }

            else
            {
                //Log.AsyncC("Animation Manager is working!");
                float addedDepth = 0;


                if (Revitalize.ModCore.playerInfo.sittingInfo.SittingObject == this.containerObject && this.info.facingDirection == Enums.Direction.Up)
                {
                    addedDepth += (this.containerObject.Height - 1) - ((int)(this.offsetKey.Y));
                    if (this.info.ignoreBoundingBox) addedDepth += 1.5f;
                }
                else if (this.info.ignoreBoundingBox)
                {
                    addedDepth += 1.0f;
                }
                this.animationManager.draw(spriteBatch, this.displayTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize)), new Rectangle?(this.animationManager.currentAnimation.sourceRectangle), this.info.drawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)((y + addedDepth) * Game1.tileSize) / 10000f) + .00001f);
                try
                {
                    this.animationManager.tickAnimation();
                    // Log.AsyncC("Tick animation");
                }
                catch (Exception err)
                {
                    ModCore.ModMonitor.Log(err.ToString());
                }
                if (this.heldObject.Value != null) SpriteBatchUtilities.Draw(spriteBatch, this, this.heldObject.Value, alpha, addedDepth);
            }
        }


    }
}
