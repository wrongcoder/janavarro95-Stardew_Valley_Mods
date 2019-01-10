using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
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

        public ChairTileComponent(BasicItemInformation Info,ChairInformation FurnitureInfo) : base(Info,FurnitureInfo)
        {
           
        }

        public ChairTileComponent(BasicItemInformation Info,Vector2 TileLocation, ChairInformation FurnitureInfo) : base(Info, TileLocation,FurnitureInfo)
        {
            
        }

        

        /// <summary>
        /// When the chair is right clicked ensure that all pieces associated with it are also rotated.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool rightClicked(Farmer who)
        {
            this.containerObject.rotate(); //Ensure that all of the chair pieces rotate at the same time.

            checkForSpecialUpSittingAnimation();
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
            BasicItemInformation data = (BasicItemInformation)CustomObjectData.collection[additionalSaveData["id"]];
            return new ChairTileComponent(data, (replacement as Chest).TileLocation,(ChairInformation)this.furnitureInfo)
            {
                containerObject = this.containerObject,
                offsetKey = this.offsetKey
            };
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
    }
}
