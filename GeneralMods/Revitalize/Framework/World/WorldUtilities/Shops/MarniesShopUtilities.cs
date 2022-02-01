using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Constants.ItemIds.Objects;
using Revitalize.Framework.World.Objects.Machines;
using StardewValley.Menus;

namespace Revitalize.Framework.World.WorldUtilities.Shops
{
    public class MarniesShopUtilities
    {
        /// <summary>
        /// Adds stock to marnies shop based on various conditions.
        /// </summary>
        /// <param name="shopMenu"></param>
        public static void AddStockToMarniesShop(ShopMenu shopMenu)
        {
            if (BuildingUtilities.HasBuiltTier2OrHigherBarnOrCoop() || RevitalizeModCore.SaveDataManager.shopSaveData.animalShopSaveData.getHasBuiltTier2OrHigherBarnOrCoop())
            {
                HayMaker hayMaker = RevitalizeModCore.ObjectManager.GetItem<HayMaker>(Machines.HayMaker, 1);
               ShopUtilities.AddItemToShop(shopMenu, hayMaker, RevitalizeModCore.Configs.shopsConfigManager.animalShopStockConfig.HayMakerPrice, -1);
            }
        }
    }
}
