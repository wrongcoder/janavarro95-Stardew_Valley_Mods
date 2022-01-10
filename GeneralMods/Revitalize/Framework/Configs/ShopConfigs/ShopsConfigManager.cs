using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Configs.ShopConfigs
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
            this.animalShopStockConfig = AnimalShopStockConfig.InitializeConfig();
            this.blacksmithShopsConfig = BlacksmithShopConfig.InitializeConfig();
            this.dwarfShopConfig = DwarfShopConfig.InitializeConfig();
            this.hayMakerShopConfig = HayMakerShopConfig.InitializeConfig();
            this.robinsShopConfig = RobinsShopConfig.InitializeConfig();
        }

    }
}
