using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize;

namespace Revitalize.Framework.Configs.ShopConfigs
{
    public class DwarfShopConfig
    {

        public int NumberOfNormalGeodesToSell;
        public int NumberOfFrozenGeodesToSell;
        public int NumberOfMagmaGeodesToSell;
        public int NumberOfOmniGeodesToSell;

        public int NormalGeodePrice;
        public int FrozenGeodePrice;
        public int MagmaGeodePrice;
        public int OmniGeodePrice;

        public bool SellOmniGeodesEveryDayInsteadOnJustSundays;

        public DwarfShopConfig()
        {

            this.NumberOfNormalGeodesToSell = 3;
            this.NumberOfFrozenGeodesToSell = 3;
            this.NumberOfMagmaGeodesToSell = 3;
            this.NumberOfOmniGeodesToSell = 1;

            this.NormalGeodePrice = 250;
            this.FrozenGeodePrice = 400;
            this.MagmaGeodePrice = 750;
            this.OmniGeodePrice = 1250;

            this.SellOmniGeodesEveryDayInsteadOnJustSundays = false;
        }


        /// <summary>
        /// Initializes the config for the blacksmith shop prices.
        /// </summary>
        /// <returns></returns>
        public static DwarfShopConfig InitializeConfig()
        {
            if (File.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Configs", "Shops", "DwarfShopConfig.json")))
                return ModCore.ModHelper.Data.ReadJsonFile<DwarfShopConfig>(Path.Combine("Configs", "Shops", "DwarfShopConfig.json"));
            else
            {
                DwarfShopConfig Config = new DwarfShopConfig();
                ModCore.ModHelper.Data.WriteJsonFile(Path.Combine("Configs", "Shops", "DwarfShopConfig.json"), Config);
                return Config;
            }
        }

    }
}
