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


        public TableTileComponent() : base()
        {

        }

        public TableTileComponent(BasicItemInformation Info, TableInformation FurnitureInfo) : base(Info)
        {
            this.furnitureInfo = FurnitureInfo;
        }

        public TableTileComponent(BasicItemInformation Info, Vector2 TileLocation, TableInformation FurnitureInfo) : base(Info, TileLocation)
        {
            this.furnitureInfo = FurnitureInfo;
        }


        public override bool clicked(Farmer who)
        {
            return base.clicked(who);
        }

        public override bool rightClicked(Farmer who)
        {
            ///Not sure.
            return true;
            //return base.rightClicked(who);
        }

        public override bool shiftRightClicked(Farmer who)
        {
            return base.shiftRightClicked(who);
        }


        public override Item getOne()
        {
            TableTileComponent component = new TableTileComponent(this.info, (TableInformation)this.furnitureInfo);
            component.containerObject = this.containerObject;
            component.offsetKey = this.offsetKey;
            return component;
        }

        public override ICustomObject recreate(Dictionary<string, string> additionalSaveData, object replacement)
        {
            //instead of using this.offsetkey.x use get additional save data function and store offset key there

            Vector2 offsetKey = new Vector2(Convert.ToInt32(additionalSaveData["offsetKeyX"]), Convert.ToInt32(additionalSaveData["offsetKeyY"]));
            TableTileComponent self = Revitalize.ModCore.Serializer.DeserializeGUID<TableTileComponent>(additionalSaveData["GUID"]);
            if (self == null)
            {
                return null;
            }

            if (!Revitalize.ModCore.ObjectGroups.ContainsKey(additionalSaveData["ParentGUID"]))
            {
                //Get new container
                TableMultiTiledObject obj = (TableMultiTiledObject)Revitalize.ModCore.Serializer.DeserializeGUID<TableMultiTiledObject>(additionalSaveData["ParentGUID"]);
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

    }
}
