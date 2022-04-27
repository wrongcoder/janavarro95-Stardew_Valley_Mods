using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Configs.ShopConfigs
{
    /// <summary>
    /// Deals with configs for shops.
    /// </summary>
    public class ShopsConfigManager
    {
        /// <summary>
        /// Config file for marnie's normal non-animal stock.
        /// </summary>
        public AnimalShopStockConfig animalShopStockConfig;

        /// <summary>
        /// Config file for maanging extra items added to Clint's shop.
        /// </summary>
        public BlacksmithShopConfig blacksmithShopsConfig;

        /// <summary>
        /// Config file for managing extra items added to dwarf's shop.
        /// </summary>
        public DwarfShopConfig dwarfShopConfig;

        /// <summary>
        /// Config file for the custom hay maker shop outside of Marnie's ranch.
        /// </summary>
        public HayMakerShopConfig hayMakerShopConfig;

        public RobinsShopConfig robinsShopConfig;

        public ShopsConfigManager()
        {
            this.animalShopStockConfig = ConfigManager.initializeConfig<AnimalShopStockConfig>("Configs", "Shops", "AnimalShopConfig.json");
            this.blacksmithShopsConfig = ConfigManager.initializeConfig<BlacksmithShopConfig>("Configs", "Shops", "BlacksmithShopConfig.json");
            this.dwarfShopConfig = ConfigManager.initializeConfig<DwarfShopConfig>("Configs", "Shops", "DwarfShopConfig.json");
            this.hayMakerShopConfig = ConfigManager.initializeConfig<HayMakerShopConfig>("Configs", "Shops", "HayMakerShopConfig.json");
            this.robinsShopConfig = ConfigManager.initializeConfig<RobinsShopConfig>("Configs", "Shops", "RobinsShopConfig.json");
        }



    }
}
