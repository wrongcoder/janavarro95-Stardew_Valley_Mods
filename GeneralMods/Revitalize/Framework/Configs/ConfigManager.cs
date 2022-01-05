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

        public ShopConfigs.ShopsConfigManager shopsConfigManager;
        public FurnitureConfig furnitureConfig;

        /// <summary>
        /// The config file to be used for machines.
        /// </summary>
        public GlobalMachineConfig machinesConfig;

        public MiningDrillConfig miningDrillConfig;

        public ObjectsConfigs.ObjectConfigManager objectConfigManager;

        public ConfigManager()
        {
            this.vanillaMachineConfig = VanillaMachineRecipeConfig.InitializeConfig();
            this.furnitureConfig = FurnitureConfig.InitializeConfig();
            this.machinesConfig = GlobalMachineConfig.InitializeConfig();
            this.miningDrillConfig = MiningDrillConfig.InitializeConfig();

            this.shopsConfigManager = new ShopConfigs.ShopsConfigManager();
            this.objectConfigManager = new ObjectsConfigs.ObjectConfigManager();
        }
    }
}
