using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

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
            this.darkerNightConfig = Revitalize.ModCore.Configs.initializeConfig<DarkerNightConfig>("Configs", "WorldConfigs", "DarkerNightConfig.json");
        }
    }
}
