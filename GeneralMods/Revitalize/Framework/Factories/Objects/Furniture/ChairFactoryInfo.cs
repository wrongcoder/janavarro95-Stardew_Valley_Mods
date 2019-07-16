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
    public class ChairFactoryInfo:FactoryInfo
    {
        public ChairInformation chairInfo;

        public ChairFactoryInfo()
        {

        }

        public ChairFactoryInfo(ChairMultiTiledObject chair): base(chair)
        {
            this.chairInfo = null;
        }

        public ChairFactoryInfo(ChairTileComponent chair):base(chair)
        {
            this.chairInfo = chair.furnitureInfo;
        }

    }
}
