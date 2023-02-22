using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Configs.ShopConfigs;
using Omegasis.Revitalize.Framework.Constants.Ids.Objects;
using Omegasis.Revitalize.Framework.World.Objects;
using StardewValley;
using StardewValley.Menus;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Shops
{
    public class AdventureGuildShopUtilities
    {


        public static void AddStockToAdventureGuildShop(ShopMenu Menu)
        {

            ObjectManager objectManager = RevitalizeModCore.ModContentManager.objectManager;

            AdventureGuildShopConfig shopConfig = RevitalizeModCore.Configs.shopsConfigManager.adventureGuildShopConfig;

            ShopUtilities.AddItemToShop(Menu,objectManager.getItem(MiscObjectIds.StatueOfStatistics), shopConfig.StatueOfStatisticsPrice, 1);

            //Require first backback upgrade.
            if (Game1.player.MaxItems > 12)
            {
                ShopUtilities.AddItemToShop(Menu, objectManager.getItem(StorageIds.SmallItemBag), shopConfig.SmallItemBagPrice, 1);
            }
            //Require second backpack upgrade.
            if (Game1.player.MaxItems > 24)
            {
                ShopUtilities.AddItemToShop(Menu, objectManager.getItem(StorageIds.BigItemBag), shopConfig.BigItemBagPrice, 1);
            }

        }
    }
}
