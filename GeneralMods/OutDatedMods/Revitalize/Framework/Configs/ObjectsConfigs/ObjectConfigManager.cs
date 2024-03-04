using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Configs.ObjectsConfigs
{
    /// <summary>
    /// A config manager that specifically deals with loading config files for objects.
    /// </summary>
    public class ObjectConfigManager
    {
        public FurnitureConfig furnitureConfig;
        public ObjectsConfig objectsConfig;

        public ObjectConfigManager()
        {
            this.furnitureConfig = FurnitureConfig.InitializeConfig();
            this.objectsConfig = ObjectsConfig.InitializeConfig();
        }

    }
}
