using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Configs.ObjectsConfigs
{
    /// <summary>
    /// A config manager that specifically deals with loading config files for objects.
    /// </summary>
    public class ObjectConfigManager
    {
        public FurnitureConfig furnitureConfig;
        public HayMakerConfig hayMakerConfig;
        public MiningDrillConfig miningDrillConfig;
        public ObjectsConfig objectsConfig;

        public ObjectConfigManager()
        {
            this.furnitureConfig = FurnitureConfig.InitializeConfig();
            this.hayMakerConfig = HayMakerConfig.InitializeConfig();
            this.miningDrillConfig = MiningDrillConfig.InitializeConfig();
            this.objectsConfig = ObjectsConfig.InitializeConfig();
        }

    }
}
