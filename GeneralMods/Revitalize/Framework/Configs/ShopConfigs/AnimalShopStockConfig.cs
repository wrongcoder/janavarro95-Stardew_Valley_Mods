using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Configs.ShopConfigs
{
    /// <summary>
    /// Config related to selling various items at Marnie's shop.
    /// </summary>
    public class AnimalShopStockConfig
    {

        /// <summary>
        /// The price that a hay maker costs in Marnie's shop.
        /// </summary>
        public int HayMakerPrice = 2000;
        public AnimalShopStockConfig()
        {

        }

        public static AnimalShopStockConfig InitializeConfig()
        {
            if (File.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Configs", "Shops", "AnimalShopConfig.json")))
                return ModCore.ModHelper.Data.ReadJsonFile<AnimalShopStockConfig>(Path.Combine("Configs", "Shops", "AnimalShopConfig.json"));
            else
            {
                AnimalShopStockConfig Config = new AnimalShopStockConfig();
                ModCore.ModHelper.Data.WriteJsonFile(Path.Combine("Configs", "Shops", "AnimalShopConfig.json"), Config);
                return Config;
            }
        }
    }
}
