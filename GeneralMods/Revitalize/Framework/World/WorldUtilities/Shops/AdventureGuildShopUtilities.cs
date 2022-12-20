using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Configs.ShopConfigs;
using Omegasis.Revitalize.Framework.Constants.Ids.Objects;
using Omegasis.Revitalize.Framework.World.Objects;
using StardewValley.Menus;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Shops
{
    public class AdventureGuildShopUtilities
    {


        public static void AddStockToAdventureGuildShop(ShopMenu Menu)
        {
            //ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(Ores.TinOre, 1), RevitalizeModCore.Configs.shopsConfigManager.blacksmithShopsConfig.tinOrePrice, -1);
            //ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(Ores.BauxiteOre, 1), RevitalizeModCore.Configs.shopsConfigManager.blacksmithShopsConfig.bauxiteOreSellPrice, -1);
            //ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(Ores.LeadOre, 1), RevitalizeModCore.Configs.shopsConfigManager.blacksmithShopsConfig.leadOrePrice, -1);
            //ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(Ores.SilverOre, 1), RevitalizeModCore.Configs.shopsConfigManager.blacksmithShopsConfig.silverOrePrice, -1);
            //ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(Ores.TitaniumOre, 1), RevitalizeModCore.Configs.shopsConfigManager.blacksmithShopsConfig.titaniumOreSellPrice, -1);

            ObjectManager objectManager = RevitalizeModCore.ModContentManager.objectManager;

            AdventureGuildShopConfig shopConfig = RevitalizeModCore.Configs.shopsConfigManager.adventureGuildShopConfig;

            ShopUtilities.AddItemToShop(Menu,objectManager.getItem(MiscObjectIds.StatueOfStatistics), shopConfig.StatueOfStatisticsPrice, 1);

        }
    }
}
