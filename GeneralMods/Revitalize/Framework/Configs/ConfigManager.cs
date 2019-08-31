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
        /// <summary>
        /// The config file for vanilla machine recipes.
        /// </summary>
        public VanillaMachineRecipeConfig vanillaMachineConfig;

        public Shops_BlacksmithConfig shops_blacksmithConfig;

        public ConfigManager()
        {
            this.vanillaMachineConfig = VanillaMachineRecipeConfig.InitializeConfig();
            this.shops_blacksmithConfig = Shops_BlacksmithConfig.InitializeConfig();
        }
    }
}
