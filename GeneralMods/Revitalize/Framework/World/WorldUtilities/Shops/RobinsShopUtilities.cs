using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Player;
using StardewValley;
using StardewValley.Menus;
using static Revitalize.Framework.World.WorldUtilities.Shops.ShopUtilities;

namespace Revitalize.Framework.World.WorldUtilities.Shops
{
    public static class RobinsShopUtilities
    {


        /// <summary>
        /// Keeps track of the number of hardwood pieces to sell in Robin's shop for a given day.
        /// </summary>
        public static int RobinsShop_NumberOfHardwoodToSellToday;
        public static Func<ISalable, Farmer, int, bool> RobinsShop_DefaultOnPurchaseMethod;

        public static void OnNewDay(object sender, StardewModdingAPI.Events.DayStartedEventArgs args)
        {
            RobinsShop_NumberOfHardwoodToSellToday = Game1.random.Next(ModCore.Configs.shopsConfigManager.robinsShopConfig.HardwoodMinStockAmount, ModCore.Configs.shopsConfigManager.robinsShopConfig.HardwoodMaxStockAmount + 1);
        }

        public static void AddItemsToRobinsShop(ShopMenu Menu)
        {
            RobinsShop_DefaultOnPurchaseMethod = Menu.onPurchase;
            Menu.onPurchase = OnPurchaseFromRobinsShop;
           ShopUtilities.UpdateShopStockAndPriceInSortedOrder(Menu, new List<ShopInventoryProbe>()
            {

                new ShopInventoryProbe(
                    new ItemFoundInShopInventory((itemForSale, Price,Stock)=> itemForSale.GetType().Equals(typeof(StardewValley.Object)) && (itemForSale as StardewValley.Object).parentSheetIndex== (int)Enums.SDVObject.Stone),
                    new UpdateShopInventory((ShopInventory,ItemForSale,Price,Stock)=>{
                        Item clay = ModCore.ObjectManager.GetItem(Enums.SDVObject.Clay, -1);
                        ShopInventory.addItemForSale(clay,Game1.year>1? ModCore.Configs.shopsConfigManager.robinsShopConfig.ClaySellPriceYear2AndBeyond: ModCore.Configs.shopsConfigManager.robinsShopConfig.ClaySellPrice, -1);
                        return ShopInventory ;
                }
                )),

                new ShopInventoryProbe(
                    new ItemFoundInShopInventory((itemForSale, Price,Stock)=> itemForSale.GetType().Equals(typeof(StardewValley.Object)) && (itemForSale as StardewValley.Object).parentSheetIndex== (int)Enums.SDVObject.Stone && PlayerUtilities.HasCompletedHardwoodDonationSpecialOrderForRobin()),
                    new UpdateShopInventory((ShopInventory,ItemForSale,Price,Stock)=>{
                        StardewValley.Item hardwood = ModCore.ObjectManager.GetItem(Enums.SDVObject.Hardwood, 1);
                        if (ModCore.Configs.shopsConfigManager.robinsShopConfig.SellsInfiniteHardWood)
                        {
                            ShopInventory.addItemForSale(hardwood,ModCore.Configs.shopsConfigManager.robinsShopConfig.HardwoodSellPrice, -1);
                        }
                        else
                        {
                            hardwood.Stack = RobinsShop_NumberOfHardwoodToSellToday;
                            ShopInventory.addItemForSale(hardwood,ModCore.Configs.shopsConfigManager.robinsShopConfig.HardwoodSellPrice, RobinsShop_NumberOfHardwoodToSellToday);
                        }
                        return ShopInventory;
                }
                )),

                 new ShopInventoryProbe(
                    new ItemFoundInShopInventory((itemForSale, Price,Stock)=>itemForSale.GetType().Equals(typeof(StardewValley.Object)) && (itemForSale as StardewValley.Object).parentSheetIndex==(int)Enums.SDVBigCraftable.Workbench && (itemForSale as StardewValley.Object).bigCraftable == true),
                    new UpdateShopInventory((ShopInventory,ItemForSale,Price,Stock)=>{
                        Item workbench = ModCore.ObjectManager.GetItem(Constants.ItemIds.Objects.CraftingStations.WorkStation, 1);
                        ShopInventory.addItemForSale(workbench,ModCore.Configs.shopsConfigManager.robinsShopConfig.WorkStationSellPrice, -1);
                        return ShopInventory;
                }
                )),

            });


        }


        /// <summary>
        /// Called when purchasing an item from robins shop.
        /// </summary>
        /// <param name="purchasedItem"></param>
        /// <param name="who"></param>
        /// <param name="AmountPurchased"></param>
        /// <returns>A bool representing if the menu should be closed or not.</returns>
        private static bool OnPurchaseFromRobinsShop(ISalable purchasedItem, Farmer who, int AmountPurchased)
        {
            if (purchasedItem is StardewValley.Object)
            {
                StardewValley.Object itemForSale = (purchasedItem as StardewValley.Object);
                if (itemForSale.parentSheetIndex == (int)Enums.SDVObject.Hardwood)
                {
                    RobinsShop_NumberOfHardwoodToSellToday -= AmountPurchased;
                    return false;
                }
            }

            if (RobinsShop_DefaultOnPurchaseMethod != null)
            {
                return RobinsShop_DefaultOnPurchaseMethod.Invoke(purchasedItem, who, AmountPurchased);
            }
            return false;
        }

    }
}
