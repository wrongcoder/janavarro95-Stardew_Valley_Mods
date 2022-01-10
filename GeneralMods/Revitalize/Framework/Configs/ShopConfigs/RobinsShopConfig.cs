using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Configs.ShopConfigs
{
    /// <summary>
    /// Deals with adding stock and items to Robin's shop.
    /// </summary>
    public class RobinsShopConfig
    {
        public int WorkStationSellPrice = 500;
        public int HardwoodSellPrice = 1000;

        public bool SellsInfiniteHardWood = false;
        public int HardwoodMinStockAmount = 5;
        public int HardwoodMaxStockAmount = 10;

        public int ClaySellPrice = 50;
        public int ClaySellPriceYear2AndBeyond = 100;


        //Currently unused.
       // public int SandSellPrice = 25;

        public RobinsShopConfig()
        {

        }

        public static RobinsShopConfig InitializeConfig()
        {
            if (File.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Configs", "Shops", "RobinsShopConfig.json")))
                return ModCore.ModHelper.Data.ReadJsonFile<RobinsShopConfig>(Path.Combine("Configs", "Shops", "RobinsShopConfig.json"));
            else
            {
                RobinsShopConfig Config = new RobinsShopConfig();
                ModCore.ModHelper.Data.WriteJsonFile(Path.Combine("Configs", "Shops", "RobinsShopConfig.json"), Config);
                return Config;
            }
        }

    }
}
