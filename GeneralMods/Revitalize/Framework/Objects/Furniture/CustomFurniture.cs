using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PyTK.CustomElementHandler;
using Revitalize.Framework.Objects.InformationFiles.Furniture;

namespace Revitalize.Framework.Objects.Furniture
{
    public class CustomFurniture:CustomObject
    {
        public FurnitureInformation furnitureInfo;


        public CustomFurniture() : base()
        {

        }

        public CustomFurniture(CustomObjectData PyTKData,BasicItemInformation itemInfo, FurnitureInformation furnitureInfo) : base(PyTKData,itemInfo)
        {
            this.furnitureInfo = furnitureInfo;
        }

        public CustomFurniture(CustomObjectData PyTKData, BasicItemInformation itemInfo, Vector2 TileLocation, FurnitureInformation furnitureInfo) : base(PyTKData,itemInfo, TileLocation)
        {
            this.furnitureInfo = furnitureInfo;
        }

    }
}
