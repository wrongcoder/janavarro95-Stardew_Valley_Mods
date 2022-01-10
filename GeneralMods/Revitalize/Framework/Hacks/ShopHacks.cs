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
using Revitalize.Framework.Constants.ItemIds.Objects;
using Revitalize.Framework.World.Objects.Machines;
using Revitalize.Framework.World.WorldUtilities;
using Omegasis.Revitalize.Framework.Utilities;
using System.IO;
using Revitalize.Framework.Player;
using StardewValley.Objects;

namespace Revitalize.Framework.Hacks
{
    /// <summary>
    /// Deals with modifications for SDV shops.
    /// </summary>
    public class ShopHacks
    {

        /// <summary>
        /// Delegate method to find if a given item being searched matches a given condiiton.
        /// </summary>
        /// <param name="ItemForSale"></param>
        /// <param name="ItemPrice"></param>
        /// <param name="AmountForSale"></param>
        /// <returns></returns>
        public delegate bool ItemFoundInShopInventory(ISalable ItemForSale, int ItemPrice, int AmountForSale);
        /// <summary>
        /// Used to update the shop's inventory. Currently sued only to add, but not modifiy a shop's contents.
        /// </summary>
        /// <param name="initialShopInventory"></param>
        /// <param name="currentItemForSale"></param>
        /// <param name="price"></param>
        /// <param name="amountForSale"></param>
        /// <returns></returns>
        public delegate ShopInventory UpdateShopInventory(ShopInventory initialShopInventory, ISalable currentItemForSale, int price, int amountForSale);


        /// <summary>
        /// Represents a shop's given inventiry at any time.
        /// </summary>
        public class ShopInventory
        {

            public Dictionary<ISalable, int[]> itemPriceAndStock;
            public List<ISalable> itemsForSale;

            public ShopInventory(Dictionary<ISalable, int[]> itemPriceAndStock, List<ISalable> itemsForSale)
            {
                this.itemPriceAndStock = itemPriceAndStock;
                this.itemsForSale = itemsForSale;
            }

            public virtual void addItemForSale(ISalable itemForSale, int Price, int Stock)
            {
                this.itemsForSale.Add(itemForSale);
                this.itemPriceAndStock.Add(itemForSale, new int[] { Price, Stock });
            }

        }

        /// <summary>
        /// Keeps track of methods to update a shop.
        /// Used because I can't initialize a KeyValue pair with a {} constructor.
        /// </summary>
        public class ShopInventoryProbe
        {

            public ItemFoundInShopInventory searchCondition;
            public UpdateShopInventory onSearchConditionMetAddItems;

            public ShopInventoryProbe(ItemFoundInShopInventory SearchCondition, UpdateShopInventory OnSearchConditionMetAddItems)
            {
                this.searchCondition = SearchCondition;
                this.onSearchConditionMetAddItems = OnSearchConditionMetAddItems;
            }

        }





        public static int DwarfShop_NormalGeodesRemainingToday;
        public static int DwarfShop_FrozenGeodesRemainingToday;
        public static int DwarfShop_MagmaGeodesRemainingToday;
        public static int DwarfShop_OmniGeodesRemainingToday;

        public static Func<ISalable, Farmer, int, bool> DwarfShop_DefaultOnPurchaseMethod;


        /// <summary>
        /// Keeps track of the number of hardwood pieces to sell in Robin's shop for a given day.
        /// </summary>
        public static int RobinsShop_NumberOfHardwoodToSellToday;
        public static Func<ISalable, Farmer, int, bool> RobinsShop_DefaultOnPurchaseMethod;




        public static void OnNewDay(object Sender, StardewModdingAPI.Events.DayStartedEventArgs args)
        {
            DwarfShop_NormalGeodesRemainingToday = ModCore.Configs.shopsConfigManager.dwarfShopConfig.NumberOfNormalGeodesToSell;
            if (Game1.player.deepestMineLevel >= 40)
            {
                DwarfShop_FrozenGeodesRemainingToday = ModCore.Configs.shopsConfigManager.dwarfShopConfig.NumberOfFrozenGeodesToSell;
            }
            if (Game1.player.deepestMineLevel >= 80)
            {
                DwarfShop_MagmaGeodesRemainingToday = ModCore.Configs.shopsConfigManager.dwarfShopConfig.NumberOfMagmaGeodesToSell;
            }
            if (Game1.player.hasSkullKey && (Game1.dayOfMonth % 7 == 0 || ModCore.Configs.shopsConfigManager.dwarfShopConfig.SellOmniGeodesEveryDayInsteadOnJustSundays))
            {
                //Add 1 omni geode on sundays.
                DwarfShop_OmniGeodesRemainingToday = ModCore.Configs.shopsConfigManager.dwarfShopConfig.NumberOfOmniGeodesToSell;
            }
            else
            {
                DwarfShop_OmniGeodesRemainingToday = 0;
            }


            RobinsShop_NumberOfHardwoodToSellToday = Game1.random.Next(ModCore.Configs.shopsConfigManager.robinsShopConfig.HardwoodMinStockAmount, ModCore.Configs.shopsConfigManager.robinsShopConfig.HardwoodMaxStockAmount + 1);

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
                        else if (npcName.Equals("Marnie"))
                        {
                            ModCore.log("Accessing marnies shop!");
                            AddStockToMarniesShop(menu);
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

        /// <summary>
        /// Updates a stock of a shop in a given order based on various conditions.
        /// </summary>
        /// <param name="Menu"></param>
        /// <param name="shopPopulationMethods"></param>
        /// <returns></returns>
        public static void updateShopStockAndPriceInSortedOrder(ShopMenu Menu, List<ShopInventoryProbe> shopPopulationMethods)
        {
            Dictionary<ISalable, int[]> sortedPriceAndStock = new Dictionary<ISalable, int[]>();
            List<ISalable> forSaleItems = new List<ISalable>();

            foreach (KeyValuePair<ISalable, int[]> itemPriceAndStock in Menu.itemPriceAndStock)
            {

                ISalable currentItemForSaleInList = itemPriceAndStock.Key;
                int price = itemPriceAndStock.Value[0];
                int amountForSale = itemPriceAndStock.Value[1];
                forSaleItems.Add(currentItemForSaleInList);
                sortedPriceAndStock.Add(itemPriceAndStock.Key, itemPriceAndStock.Value);

                foreach (var v in shopPopulationMethods)
                {
                    if (v.searchCondition.Invoke(currentItemForSaleInList, price, amountForSale))
                    {
                        ShopInventory shopInventory = new ShopInventory(sortedPriceAndStock, forSaleItems);
                        ShopInventory updatedShopInventory = v.onSearchConditionMetAddItems.Invoke(shopInventory, currentItemForSaleInList, price, amountForSale);
                        sortedPriceAndStock = updatedShopInventory.itemPriceAndStock;
                        forSaleItems = updatedShopInventory.itemsForSale;
                    }
                }

                Menu.forSale = forSaleItems;
                Menu.itemPriceAndStock = sortedPriceAndStock;
            }
        }

        private static void AddItemsToRobinsShop(ShopMenu Menu)
        {
            RobinsShop_DefaultOnPurchaseMethod = Menu.onPurchase;
            Menu.onPurchase = OnPurchaseFromRobinsShop;
             updateShopStockAndPriceInSortedOrder(Menu, new List<ShopInventoryProbe>()
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
            DwarfShop_DefaultOnPurchaseMethod = Menu.onPurchase;
            Menu.onPurchase = OnPurchaseFromDwarfShop;

            if (DwarfShop_NormalGeodesRemainingToday > 0)
            {
                AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Enums.SDVObject.Geode, DwarfShop_NormalGeodesRemainingToday), ModCore.Configs.shopsConfigManager.dwarfShopConfig.NormalGeodePrice, DwarfShop_NormalGeodesRemainingToday);
            }
            if (DwarfShop_FrozenGeodesRemainingToday > 0)
            {
                AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Enums.SDVObject.FrozenGeode, DwarfShop_FrozenGeodesRemainingToday), ModCore.Configs.shopsConfigManager.dwarfShopConfig.FrozenGeodePrice, DwarfShop_FrozenGeodesRemainingToday);
            }
            if (DwarfShop_MagmaGeodesRemainingToday > 0)
            {
                AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Enums.SDVObject.MagmaGeode, DwarfShop_MagmaGeodesRemainingToday), ModCore.Configs.shopsConfigManager.dwarfShopConfig.MagmaGeodePrice, DwarfShop_MagmaGeodesRemainingToday);
            }
            if (DwarfShop_OmniGeodesRemainingToday > 0)
            {
                AddItemToShop(Menu, ModCore.ObjectManager.GetItem(Enums.SDVObject.OmniGeode, DwarfShop_OmniGeodesRemainingToday), ModCore.Configs.shopsConfigManager.dwarfShopConfig.OmniGeodePrice, DwarfShop_OmniGeodesRemainingToday);
            }
        }

        /// <summary>
        /// Adds stock to marnies shop based on various conditions.
        /// </summary>
        /// <param name="shopMenu"></param>
        private static void AddStockToMarniesShop(ShopMenu shopMenu)
        {
            if (BuildingUtilities.HasBuiltTier2OrHigherBarnOrCoop() || ModCore.SaveDataManager.shopSaveData.animalShopSaveData.getHasBuiltTier2OrHigherBarnOrCoop())
            {
                HayMaker hayMaker = ModCore.ObjectManager.GetItem<HayMaker>(Machines.HayMaker, 1);
                AddItemToShop(shopMenu, hayMaker, ModCore.Configs.shopsConfigManager.animalShopStockConfig.HayMakerPrice, -1);
            }
        }

        private static bool OnPurchaseFromDwarfShop(ISalable purchasedItem, Farmer who, int AmountPurchased)
        {
            if (purchasedItem is StardewValley.Object)
            {
                StardewValley.Object itemForSale = (purchasedItem as StardewValley.Object);
                if (itemForSale.parentSheetIndex == (int)Enums.SDVObject.Geode)
                {
                    DwarfShop_NormalGeodesRemainingToday -= AmountPurchased;
                    return false;
                }
                if (itemForSale.parentSheetIndex == (int)Enums.SDVObject.FrozenGeode)
                {
                    DwarfShop_FrozenGeodesRemainingToday -= AmountPurchased;
                    return false;
                }
                if (itemForSale.parentSheetIndex == (int)Enums.SDVObject.MagmaGeode)
                {
                    DwarfShop_MagmaGeodesRemainingToday -= AmountPurchased;
                    return false;
                }
                if (itemForSale.parentSheetIndex == (int)Enums.SDVObject.OmniGeode)
                {
                    DwarfShop_OmniGeodesRemainingToday -= AmountPurchased;
                    return false;
                }
            }

            if (DwarfShop_DefaultOnPurchaseMethod != null)
            {
                return DwarfShop_DefaultOnPurchaseMethod.Invoke(purchasedItem, who, AmountPurchased);
            }

            return false;
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
