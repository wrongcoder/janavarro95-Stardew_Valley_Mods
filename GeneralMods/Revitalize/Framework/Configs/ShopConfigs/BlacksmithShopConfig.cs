using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize;

namespace Revitalize.Framework.Configs.ShopConfigs
{
    /// <summary>
    /// Deals with the sell prices for ores in the blacksmith shop.
    /// </summary>
    public class BlacksmithShopConfig
    {
        /// <summary>
        /// The sell price for tin ore from the blacksmith.
        /// </summary>
        public int tinOreSellPrice;
        /// <summary>
        /// The sell price for bauxite ore from the blacksmith.
        /// </summary>
        public int bauxiteOreSellPrice;
        /// <summary>
        /// The sell price for lead ore from the blacksmith.
        /// </summary>
        public int leadOreSellPrice;
        /// <summary>
        /// The sell price for silver ore from the blacksmith.
        /// </summary>
        public int silverOreSellPrice;
        /// <summary>
        /// The sell price for titanium ore from the blacksmith.
        /// </summary>
        public int titaniumOreSellPrice;

        /// <summary>
        /// Constructor.
        /// </summary>
        public BlacksmithShopConfig()
        {
            this.tinOreSellPrice = 100;
            this.bauxiteOreSellPrice = 150;
            this.leadOreSellPrice = 200;
            this.silverOreSellPrice = 250;
            this.titaniumOreSellPrice = 300;
        }

        /// <summary>
        /// Initializes the config for the blacksmith shop prices.
        /// </summary>
        /// <returns></returns>
        public static BlacksmithShopConfig InitializeConfig()
        {
            if (File.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Configs", "Shops", "BlacksmithShopPricesConfig.json")))
                return ModCore.ModHelper.Data.ReadJsonFile<BlacksmithShopConfig>(Path.Combine("Configs", "Shops", "BlacksmithShopPricesConfig.json"));
            else
            {
                BlacksmithShopConfig Config = new BlacksmithShopConfig();
                ModCore.ModHelper.Data.WriteJsonFile(Path.Combine("Configs", "Shops", "BlacksmithShopPricesConfig.json"), Config);
                return Config;
            }
        }

    }
}
