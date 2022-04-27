using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Resources.EarthenResources;
using StardewValley.Menus;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Shops
{
    public static class ClintsShopUtilities
    {

        /// <summary>
        /// Adds in ore to clint's shop.
        /// </summary>
        public static void AddOreToClintsShop(ShopMenu Menu)
        {
            ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ObjectManager.getItem(Ores.TinOre, 1), RevitalizeModCore.Configs.shopsConfigManager.blacksmithShopsConfig.tinOreSellPrice, -1);
            ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ObjectManager.getItem(Ores.BauxiteOre, 1), RevitalizeModCore.Configs.shopsConfigManager.blacksmithShopsConfig.bauxiteOreSellPrice, -1);
            ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ObjectManager.getItem(Ores.LeadOre, 1), RevitalizeModCore.Configs.shopsConfigManager.blacksmithShopsConfig.leadOreSellPrice, -1);
            ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ObjectManager.getItem(Ores.SilverOre, 1), RevitalizeModCore.Configs.shopsConfigManager.blacksmithShopsConfig.silverOreSellPrice, -1);
            ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ObjectManager.getItem(Ores.TitaniumOre, 1), RevitalizeModCore.Configs.shopsConfigManager.blacksmithShopsConfig.titaniumOreSellPrice, -1);
        }

    }
}
