using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Resources.EarthenResources;
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
                    }
                }
            }
        }

        public static void AddItemToShop(ShopMenu Menu,ISalable Item, int Price, int Stock)
        {
            Menu.forSale.Add(Item);
            Menu.itemPriceAndStock.Add(Item, new int[2] { Price, Stock });
        }

        private static void AddItemsToRobinsShop(ShopMenu Menu)
        {
            AddItemToShop(Menu,ModCore.ObjectManager.GetItem("Workbench", 1), 500, 1);
            AddItemToShop(Menu,ModCore.ObjectManager.GetItem(MiscEarthenResources.Sand, 1), 50, -1);
            AddItemToShop(Menu, new StardewValley.Object((int)Enums.SDVObject.Clay, 1), 50, -1);

        }
        /// <summary>
        /// Adds in ore to clint's shop.
        /// </summary>
        private static void AddOreToClintsShop(ShopMenu Menu)
        {
            AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Ores.TinOre, 1), ModCore.Configs.shops_blacksmithConfig.tinOreSellPrice, -1);
            AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Ores.BauxiteOre, 1), ModCore.Configs.shops_blacksmithConfig.bauxiteOreSellPrice, -1);
            AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Ores.LeadOre, 1), ModCore.Configs.shops_blacksmithConfig.leadOreSellPrice, -1);
            AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Ores.SilverOre, 1), ModCore.Configs.shops_blacksmithConfig.silverOreSellPrice, -1);
            AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Ores.TitaniumOre, 1), ModCore.Configs.shops_blacksmithConfig.titaniumOreSellPrice, -1);
        }
    }
}
