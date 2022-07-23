using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Configs.ShopConfigs
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

        public int ElectricFurnaceBlueprintPrice = 5000;

        //Currently unused.
        // public int SandSellPrice = 25;

        public RobinsShopConfig()
        {

        }

    }
}
