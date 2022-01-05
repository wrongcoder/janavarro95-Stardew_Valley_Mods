using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Constants.ItemIds.Resources.EarthenResources;
using Revitalize;
using Revitalize.Framework;
using StardewValley;
using StardewValley.Menus;

namespace Revitalize.Framework.Hacks
{
    /// <summary>
    /// Deals with modifications for SDV shops.
    /// </summary>
    public class ShopHacks
    {

        public static bool ShouldAddGeodesToDwarfShopToday;

        public static int NormalGeodesRemainingToday;
        public static int FrozenGeodesRemainingToday;
        public static int MagmaGeodesRemainingToday;
        public static int OmniGeodesRemainingToday;


        public static void OnNewDay(object Sender, StardewModdingAPI.Events.DayStartedEventArgs args)
        {

            ShouldAddGeodesToDwarfShopToday = true;
            NormalGeodesRemainingToday = ModCore.Configs.shopsConfigManager.dwarfShopConfig.NumberOfNormalGeodesToSell;
            if (Game1.player.deepestMineLevel >= 40)
            {
                FrozenGeodesRemainingToday = ModCore.Configs.shopsConfigManager.dwarfShopConfig.NumberOfFrozenGeodesToSell;
            }
            if (Game1.player.deepestMineLevel >= 80)
            {
                MagmaGeodesRemainingToday = ModCore.Configs.shopsConfigManager.dwarfShopConfig.NumberOfMagmaGeodesToSell;
            }
            if (Game1.player.hasSkullKey && (Game1.dayOfMonth % 7 == 0 ||  ModCore.Configs.shopsConfigManager.dwarfShopConfig.SellOmniGeodesEveryDayInsteadOnJustSundays))
            {
                //Add 1 omni geode on sundays.
                OmniGeodesRemainingToday = ModCore.Configs.shopsConfigManager.dwarfShopConfig.NumberOfOmniGeodesToSell;
            }
            else
            {
                OmniGeodesRemainingToday = 0;
            }

        }

        public static void OnNewMenuOpened(object Sender, StardewModdingAPI.Events.MenuChangedEventArgs args)
        {
            if (args.NewMenu != null)
            {
                if (args.NewMenu is ShopMenu)
                {
                    ShopMenu menu = (args.NewMenu as ShopMenu);
                    if (menu.portraitPerson != null)
                    {
                        string npcName = menu.portraitPerson.Name;
                        if (npcName.Equals("Robin"))
                        {
                            AddItemsToRobinsShop(menu);
                        }
                        else if (npcName.Equals("Clint"))
                        {
                            AddOreToClintsShop(menu);
                        }
                        else if (npcName.Equals("Dwarf"))
                        {
                            AddGeodesToDwarfShop(menu);
                        }
                    }
                }
            }
        }

        public static void AddItemToShop(ShopMenu Menu, ISalable Item, int Price, int Stock)
        {
            Menu.forSale.Add(Item);
            Menu.itemPriceAndStock.Add(Item, new int[2] { Price, Stock });
        }

        private static void AddItemsToRobinsShop(ShopMenu Menu)
        {
            AddItemToShop(Menu, ModCore.ObjectManager.GetItem("Workbench", 1), 500, 1);
            AddItemToShop(Menu, ModCore.ObjectManager.GetItem(MiscEarthenResources.Sand, 1), 50, -1);
            AddItemToShop(Menu, new StardewValley.Object((int)Enums.SDVObject.Clay, 1), 50, -1);

        }
        /// <summary>
        /// Adds in ore to clint's shop.
        /// </summary>
        private static void AddOreToClintsShop(ShopMenu Menu)
        {
            AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Ores.TinOre, 1), ModCore.Configs.shopsConfigManager.blacksmithShopsConfig.tinOreSellPrice, -1);
            AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Ores.BauxiteOre, 1), ModCore.Configs.shopsConfigManager.blacksmithShopsConfig.bauxiteOreSellPrice, -1);
            AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Ores.LeadOre, 1), ModCore.Configs.shopsConfigManager.blacksmithShopsConfig.leadOreSellPrice, -1);
            AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Ores.SilverOre, 1), ModCore.Configs.shopsConfigManager.blacksmithShopsConfig.silverOreSellPrice, -1);
            AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Ores.TitaniumOre, 1), ModCore.Configs.shopsConfigManager.blacksmithShopsConfig.titaniumOreSellPrice, -1);
        }

        private static void AddGeodesToDwarfShop(ShopMenu Menu)
        {

            Menu.onPurchase = OnPurchaseFromDwarfShop;

            if (NormalGeodesRemainingToday > 0)
            {
                AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Enums.SDVObject.Geode, NormalGeodesRemainingToday), ModCore.Configs.shopsConfigManager.dwarfShopConfig.NormalGeodePrice, NormalGeodesRemainingToday);
            }
            if (FrozenGeodesRemainingToday > 0)
            {
                AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Enums.SDVObject.FrozenGeode, FrozenGeodesRemainingToday), ModCore.Configs.shopsConfigManager.dwarfShopConfig.FrozenGeodePrice, FrozenGeodesRemainingToday);
            }
            if (MagmaGeodesRemainingToday > 0)
            {
                AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Enums.SDVObject.MagmaGeode, MagmaGeodesRemainingToday), ModCore.Configs.shopsConfigManager.dwarfShopConfig.MagmaGeodePrice, MagmaGeodesRemainingToday);
            }
            if (OmniGeodesRemainingToday > 0)
            {
                AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Enums.SDVObject.OmniGeode, OmniGeodesRemainingToday), ModCore.Configs.shopsConfigManager.dwarfShopConfig.OmniGeodePrice, OmniGeodesRemainingToday);
            }
        }

        private static bool OnPurchaseFromDwarfShop(ISalable purchasedItem, Farmer who, int AmountPurchased)
        {
            if (purchasedItem is StardewValley.Object)
            {
                StardewValley.Object itemForSale = (purchasedItem as StardewValley.Object);
                if (itemForSale.parentSheetIndex == (int)Enums.SDVObject.Geode)
                {
                    NormalGeodesRemainingToday -= AmountPurchased;
                    return false;
                }
                if (itemForSale.parentSheetIndex == (int)Enums.SDVObject.FrozenGeode)
                {
                    FrozenGeodesRemainingToday -= AmountPurchased;
                    return false;
                }
                if (itemForSale.parentSheetIndex == (int)Enums.SDVObject.MagmaGeode)
                {
                    MagmaGeodesRemainingToday -= AmountPurchased;
                    return false;
                }
                if (itemForSale.parentSheetIndex == (int)Enums.SDVObject.OmniGeode)
                {
                    OmniGeodesRemainingToday -= AmountPurchased;
                    return false;
                }
                return false;
            }
            return false;
        }

    }
}
