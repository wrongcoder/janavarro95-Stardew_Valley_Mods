using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Configs
{
    /// <summary>
    /// Handles holding all of the config information.
    /// </summary>
    public class ConfigManager
    {

        public ObjectsConfigs.ObjectConfigManager objectConfigManager;

        public ShopConfigs.ShopsConfigManager shopsConfigManager;

        public WorldConfigs.WorldConfigManager worldConfigManager;

        public ConfigManager()
        {

            this.objectConfigManager = new ObjectsConfigs.ObjectConfigManager();
            this.shopsConfigManager = new ShopConfigs.ShopsConfigManager();
            this.worldConfigManager = new WorldConfigs.WorldConfigManager();
        }
    }
}
