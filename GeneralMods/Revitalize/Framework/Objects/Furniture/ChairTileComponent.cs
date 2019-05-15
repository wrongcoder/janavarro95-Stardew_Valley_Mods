using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PyTK.CustomElementHandler;
using Revitalize.Framework.Objects.InformationFiles.Furniture;
using StardewValley;
using StardewValley.Objects;

namespace Revitalize.Framework.Objects.Furniture
{
    /// <summary>
    /// Chair "piece" which represents one of the objects in the game that takes up roughly one tile.
    /// </summary>
    public class ChairTileComponent:FurnitureTileComponent
    {
        public ChairInformation furnitureInfo;

        /// <summary>
        /// Checks if the player can sit "on" this component.
        /// </summary>
        public bool CanSitHere
        {
            get
            {
                return (this.furnitureInfo as InformationFiles.Furniture.ChairInformation).canSitHere;
            }
        }

        public ChairTileComponent():base()
        {

        }

        public ChairTileComponent(BasicItemInformation Info,ChairInformation FurnitureInfo) : base(Info)
        {
            this.furnitureInfo = FurnitureInfo;
        }

        public ChairTileComponent(BasicItemInformation Info,Vector2 TileLocation, ChairInformation FurnitureInfo) : base(Info, TileLocation)
        {
            this.furnitureInfo = FurnitureInfo;
        }

        

        /// <summary>
        /// When the chair is right clicked ensure that all pieces associated with it are also rotated.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool rightClicked(Farmer who)
        {
            this.containerObject.rotate(); //Ensure that all of the chair pieces rotate at the same time.

            this.checkForSpecialUpSittingAnimation();
            return true;
            //return base.rightClicked(who);
        }

        /// <summary>
        /// Used for more object interactions.
        /// When the chair is shift right clicked sit on that specific chair tile if you can sit there.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool shiftRightClicked(Farmer who)
        {
            if (this.CanSitHere)
            {
                Revitalize.ModCore.playerInfo.sittingInfo.sit(this.containerObject, this.TileLocation*Game1.tileSize);
                if(this.containerObject is Bench)
                {
                    (this.containerObject as Bench).playersSittingHere.Add(Game1.player.uniqueMultiplayerID);
                }
                foreach(KeyValuePair<Vector2, StardewValley.Object> pair in this.containerObject.objects)
                {
                    (pair.Value as ChairTileComponent).checkForSpecialUpSittingAnimation();
                }
                
            }
            return base.shiftRightClicked(who);
        }


        public override Item getOne()
        {
            ChairTileComponent component = new ChairTileComponent(this.info, (ChairInformation)this.furnitureInfo);
            component.containerObject = this.containerObject;
            component.offsetKey = this.offsetKey;
            return component;
        }

        public override ICustomObject recreate(Dictionary<string, string> additionalSaveData, object replacement)
        {
            //instead of using this.offsetkey.x use get additional save data function and store offset key there

            Vector2 offsetKey = new Vector2(Convert.ToInt32(additionalSaveData["offsetKeyX"]), Convert.ToInt32(additionalSaveData["offsetKeyY"]));
            ChairTileComponent self = Revitalize.ModCore.Serializer.DeserializeGUID<ChairTileComponent>(additionalSaveData["GUID"]);
            if (self == null)
            {
                return null;
            }

            if (!Revitalize.ModCore.ObjectGroups.ContainsKey(additionalSaveData["ParentGUID"]))
            {
                //Get new container
                ChairMultiTiledObject obj = (ChairMultiTiledObject)Revitalize.ModCore.Serializer.DeserializeGUID<ChairMultiTiledObject>(additionalSaveData["ParentGUID"]);
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
        }

        public override Dictionary<string, string> getAdditionalSaveData()
        {
            Dictionary<string, string> saveData = base.getAdditionalSaveData();
            Revitalize.ModCore.Serializer.SerializeGUID(this.containerObject.childrenGuids[this.offsetKey].ToString(), this);

            return saveData;

        }


        /// <summary>
        ///Used to manage graphics for chairs that need to deal with special "layering" for transparent chair backs. Otherwise the player would be hidden.
        /// </summary>
        public void checkForSpecialUpSittingAnimation()
        {
            if (this.info.facingDirection == Enums.Direction.Up && Revitalize.ModCore.playerInfo.sittingInfo.SittingObject == this.containerObject)
            {
                string animationKey = "Sitting_" + (int)Enums.Direction.Up;
                if (this.animationManager.animations.ContainsKey(animationKey))
                {
                    this.animationManager.setAnimation(animationKey);
                }
            }
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
            }

            // spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((double)tileLocation.X * (double)Game1.tileSize + (((double)tileLocation.X * 11.0 + (double)tileLocation.Y * 7.0) % 10.0 - 5.0)) + (float)(Game1.tileSize / 2), (float)((double)tileLocation.Y * (double)Game1.tileSize + (((double)tileLocation.Y * 11.0 + (double)tileLocation.X * 7.0) % 10.0 - 5.0)) + (float)(Game1.tileSize / 2))), new Rectangle?(new Rectangle((int)((double)tileLocation.X * 51.0 + (double)tileLocation.Y * 77.0) % 3 * 16, 128 + this.whichForageCrop * 16, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)(((double)tileLocation.Y * (double)Game1.tileSize + (double)(Game1.tileSize / 2) + (((double)tileLocation.Y * 11.0 + (double)tileLocation.X * 7.0) % 10.0 - 5.0)) / 10000.0));

        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            if (objectPosition.X < 0) objectPosition.X *= -1;
            if (objectPosition.Y < 0) objectPosition.Y *= -1;
            base.drawWhenHeld(spriteBatch, objectPosition, f);
        }


    }
}
