using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PyTK.CustomElementHandler;
using Revitalize.Framework.Objects.InformationFiles.Furniture;
using Revitalize.Framework.Utilities.Serialization;
using StardewValley;

namespace Revitalize.Framework.Objects.Furniture
{
    public class TableTileComponent : FurnitureTileComponent
    {
        public TableInformation furnitureInfo;


        public bool CanPlaceItemsHere
        {
            get
            {
                return this.furnitureInfo.canPlaceItemsHere;
            }
        }

        public enum PickUpState
        {
            RemoveContainer,
            DoNothing,
        }


        public TableTileComponent() : base()
        {

        }

        public TableTileComponent(CustomObjectData PyTKData,BasicItemInformation Info, TableInformation FurnitureInfo) : base(PyTKData,Info)
        {
            this.furnitureInfo = FurnitureInfo;
            this.Price = Info.price;
        }

        public TableTileComponent(CustomObjectData PyTKData,BasicItemInformation Info, Vector2 TileLocation, TableInformation FurnitureInfo) : base(PyTKData,Info, TileLocation)
        {
            this.furnitureInfo = FurnitureInfo;
            this.Price = Info.price;
        }

        /// <summary>
        /// Forcefully clears the held object without much fuss.
        /// </summary>
        public void clearHeldObject()
        {
            if (this.heldObject.Value != null)
            {
                if (Game1.player.isInventoryFull() == false)
                {                   
                    Game1.player.addItemToInventoryBool(this.heldObject.Value, false);
                    this.heldObject.Value = null;
                    return;
                }
                else
                {
                    Game1.createItemDebris(this.heldObject.Value, Vector2.Zero, 0);
                }
            }
        }

        /// <summary>
        /// Picks up the held item from this tile.
        /// </summary>
        /// <param name="justChecking"></param>
        /// <returns></returns>
        public PickUpState pickUpItem(bool justChecking = true)
        {
            if (this.heldObject.Value == null && Game1.player.ActiveObject != null)
            {
                if (justChecking == false)
                {
                    this.heldObject.Value = (StardewValley.Object)Game1.player.ActiveObject.getOne();
                    Game1.player.reduceActiveItemByOne();
                }
                return PickUpState.DoNothing;
            }
            else if (this.heldObject.Value != null)
            {
                if (justChecking == false)
                {
                    if (Game1.player.isInventoryFull() == false)
                    {
                        Game1.player.addItemToInventoryBool(this.heldObject.Value, false);
                        this.heldObject.Value = null;
                    }
                    else
                    {
                        Game1.createItemDebris(this.heldObject.Value, Vector2.Zero, 0);
                    }
                }
                return PickUpState.DoNothing;
            }
            else if (this.heldObject.Value == null && Game1.player.ActiveObject == null)
            {
                return PickUpState.RemoveContainer;
            }


            return PickUpState.DoNothing;
            
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

        //Checks for any sort of interaction IF and only IF there is a held object on this tile.
        public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
        {
            MouseState mState = Mouse.GetState();
            KeyboardState keyboardState = Game1.GetKeyboardState();

            if (mState.RightButton == ButtonState.Pressed && (keyboardState.IsKeyDown(Keys.LeftShift) || !keyboardState.IsKeyDown(Keys.RightShift)))
            {
                return this.rightClicked(who);
            }

            if (mState.RightButton == ButtonState.Pressed && (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift)))
                return this.shiftRightClicked(who);

           
            //return base.checkForAction(who, justCheckingForActivity);

            if (justCheckingForActivity)
                return true;

            this.pickUpItem(false);
            return true;

            //return this.clicked(who);
            //return false;
        }

        public override bool performToolAction(Tool t, GameLocation location)
        {
            return base.performToolAction(t, location);
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
            if (this.pickUpItem() ==  PickUpState.DoNothing) return false;
            return base.clicked(who);
        }


        public override bool shiftRightClicked(Farmer who)
        {
            return base.shiftRightClicked(who);
        }


        public override Item getOne()
        {
            TableTileComponent component = new TableTileComponent(this.data,this.info.Copy(), (TableInformation)this.furnitureInfo);
            component.containerObject = this.containerObject;
            component.offsetKey = this.offsetKey;
            return component;
        }

        public override ICustomObject recreate(Dictionary<string, string> additionalSaveData, object replacement)
        {
            //instead of using this.offsetkey.x use get additional save data function and store offset key there
            //ModCore.log("Recreate a table tile component!");
            Vector2 offsetKey = new Vector2(Convert.ToInt32(additionalSaveData["offsetKeyX"]), Convert.ToInt32(additionalSaveData["offsetKeyY"]));
            //ModCore.log("Got the offset key!");
            string GUID = additionalSaveData["GUID"];
            //ModCore.log("This tile has a parent guid of: " + additionalSaveData["ParentGUID"]);
            TableTileComponent self = Revitalize.ModCore.Serializer.DeserializeGUID<TableTileComponent>(additionalSaveData["GUID"]);
            
            if (ModCore.IsNullOrDefault<TableTileComponent>(self))
            {
                //ModCore.log("SELF IS NULL");
                return null;
            }
            try
            {
                if (!Revitalize.ModCore.ObjectGroups.ContainsKey(additionalSaveData["ParentGUID"]))
                {
                    //ModCore.log("Load in the parent!");
                    //Get new container
                    TableMultiTiledObject obj = (TableMultiTiledObject)Revitalize.ModCore.Serializer.DeserializeGUID<TableMultiTiledObject>(additionalSaveData["ParentGUID"]);
                    self.containerObject = obj;
                    self.containerObject.removeComponent(offsetKey);
                    self.containerObject.addComponent(offsetKey, self);
                    Revitalize.ModCore.ObjectGroups.Add(additionalSaveData["ParentGUID"], obj);
                }
                else
                {
                    //ModCore.log("Parent already exists!");
                    self.containerObject = Revitalize.ModCore.ObjectGroups[additionalSaveData["ParentGUID"]];
                    self.containerObject.removeComponent(offsetKey);
                    self.containerObject.addComponent(offsetKey, self);
                    //Revitalize.ModCore.log("READD AN OBJECT!!!!");
                }
            }
            catch(Exception err)
            {
                ModCore.log(err);
            }

            return self;
        }

        public override Dictionary<string, string> getAdditionalSaveData()
        {
            Dictionary<string, string> saveData = base.getAdditionalSaveData();
            Revitalize.ModCore.Serializer.SerializeGUID(this.containerObject.childrenGuids[this.offsetKey].ToString(), this);
            this.containerObject.getAdditionalSaveData();
            return saveData;

        }

        /// <summary>What happens when the object is drawn at a tile location.</summary>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            /*
            if (this.info.ignoreBoundingBox == true)
            {
                x *= -1;
                y *= -1;
            }
            */

            if (this.info == null)
            {
                Revitalize.ModCore.log("info is null");
                this.info = this.netInfo.Value;
                if (this.syncObject == null) Revitalize.ModCore.log("DEAD SYNC");
            }
            if (this.animationManager == null)
            {
                Revitalize.ModCore.log("Animation Manager Null");
                if (this.info == null)
                {
                    ModCore.log("And info is still null!");
                    return;
                }
                else
                {
                    this.info.animationManager = this.netInfo.Value.animationManager;
                    return;
                }
            }
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

            // spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((double)tileLocation.X * (double)Game1.tileSize + (((double)tileLocation.X * 11.0 + (double)tileLocation.Y * 7.0) % 10.0 - 5.0)) + (float)(Game1.tileSize / 2), (float)((double)tileLocation.Y * (double)Game1.tileSize + (((double)tileLocation.Y * 11.0 + (double)tileLocation.X * 7.0) % 10.0 - 5.0)) + (float)(Game1.tileSize / 2))), new Rectangle?(new Rectangle((int)((double)tileLocation.X * 51.0 + (double)tileLocation.Y * 77.0) % 3 * 16, 128 + this.whichForageCrop * 16, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)(((double)tileLocation.Y * (double)Game1.tileSize + (double)(Game1.tileSize / 2) + (((double)tileLocation.Y * 11.0 + (double)tileLocation.X * 7.0) % 10.0 - 5.0)) / 10000.0));

        }

    }
}
