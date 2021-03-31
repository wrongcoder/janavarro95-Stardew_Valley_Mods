using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Factories.Objects.Furniture;
using Revitalize.Framework.Objects.Extras;
using Revitalize.Framework.Objects.InformationFiles.Furniture;

namespace Revitalize.Framework.Factories.Objects.Extras
{
    public class ArcadeFactoryInfo:FactoryInfo
    {
        public ArcadeCabinetInformation arcadeInfo;


        public ArcadeFactoryInfo()
        {

        }

        public ArcadeFactoryInfo(ArcadeCabinetOBJ game) : base(game)
        {
            this.arcadeInfo = null;
        }

        public ArcadeFactoryInfo(ArcadeCabinetTile game) : base(game)
        {
            this.arcadeInfo = game.arcadeInfo;
        }
    }
}
