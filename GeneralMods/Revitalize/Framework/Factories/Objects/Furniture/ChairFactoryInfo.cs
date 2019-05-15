using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Objects;
using Revitalize.Framework.Objects.Furniture;
using Revitalize.Framework.Objects.InformationFiles.Furniture;

namespace Revitalize.Framework.Factories.Objects.Furniture
{
    public class ChairFactoryInfo
    {
        public ChairInformation chairInfo;
        public BasicItemInformation itemInfo;

        public ChairFactoryInfo()
        {

        }

        public ChairFactoryInfo(ChairMultiTiledObject chair)
        {
            this.chairInfo = null;
            this.itemInfo = chair.info;
        }

        public ChairFactoryInfo(ChairTileComponent chair)
        {
            this.chairInfo = chair.furnitureInfo;
            this.itemInfo = chair.info;
        }

    }
}
