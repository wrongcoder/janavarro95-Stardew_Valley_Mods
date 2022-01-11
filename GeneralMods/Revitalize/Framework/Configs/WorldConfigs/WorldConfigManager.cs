using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Configs.WorldConfigs
{
    /// <summary>
    /// Deals with configurations that affect the game world.
    /// </summary>
    public class WorldConfigManager
    {

        public DarkerNightConfig darkerNightConfig;

        public WorldConfigManager()
        {
            this.darkerNightConfig = DarkerNightConfig.InitializeConfig();
        }
    }
}
