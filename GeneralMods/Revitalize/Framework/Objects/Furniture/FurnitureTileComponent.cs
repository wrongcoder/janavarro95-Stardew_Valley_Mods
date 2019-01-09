using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Revitalize.Framework.Objects.InformationFiles;
using Revitalize.Framework.Objects.InformationFiles.Furniture;

namespace Revitalize.Framework.Objects.Furniture
{
    public class FurnitureTileComponent:MultiTiledComponent
    {
        public FurnitureInformation furnitureInfo;


        public FurnitureTileComponent():base()
        {

        }

        public FurnitureTileComponent(BasicItemInformation itemInfo,FurnitureInformation furnitureInfo):base(itemInfo)
        {
            this.furnitureInfo = furnitureInfo;
        }

        public FurnitureTileComponent(BasicItemInformation itemInfo,Vector2 TileLocation ,FurnitureInformation furnitureInfo) : base(itemInfo,TileLocation)
        {
            this.furnitureInfo = furnitureInfo;
        }

    }
}
