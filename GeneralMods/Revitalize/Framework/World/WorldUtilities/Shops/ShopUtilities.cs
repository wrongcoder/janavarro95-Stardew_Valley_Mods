using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Shops
{
    public class ShopUtilities
    {

        /// <summary>
        /// Delegate method to find if a given item being searched matches a given condiiton. Also can include additional paramaters to determine if the item should be added or not.
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


        public static void OnNewDay(object Sender, StardewModdingAPI.Events.DayStartedEventArgs args)
        {
            DwarfShopUtilities.OnNewDay(Sender, args);
            RobinsShopUtilities.OnNewDay(Sender, args);

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
                            RobinsShopUtilities.AddItemsToRobinsShop(menu);
                        }
                        else if (npcName.Equals("Clint"))
                        {
                            ClintsShopUtilities.AddStockToClintsShop(menu);
                        }
                        else if (npcName.Equals("Dwarf"))
                        {
                            DwarfShopUtilities.AddGeodesToDwarfShop(menu);
                        }
                        else if (npcName.Equals("Marnie"))
                        {
                            MarniesShopUtilities.AddStockToMarniesShop(menu);
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
        /// Helper method for creating <see cref="ShopInventoryProbe"/>s which allow for adding items to a shop in a specific order based on given conditions. If true, then the new item is found after the one matching this param.
        /// </summary>
        /// <param name="itemToAdd"></param>
        /// <param name="SellingPrice"></param>
        /// <param name="AmountToAdd"></param>
        /// <returns></returns>
        public static ShopInventoryProbe CreateInventoryShopProbe(ItemFoundInShopInventory ItemFoundInShopConditional, Enums.SDVObject itemToAdd, int SellingPrice, int AmountToAdd=-1)
        {
            return CreateInventoryShopProbe(ItemFoundInShopConditional ,new ItemReference(itemToAdd), SellingPrice, AmountToAdd);
        }

        /// <summary>
        /// Helper method for creating <see cref="ShopInventoryProbe"/>s which allow for adding items to a shop in a specific order based on given conditions. If true, then the new item is found after the one matching this param.
        /// </summary>
        /// <param name="itemToAdd"></param>
        /// <param name="SellingPrice"></param>
        /// <param name="AmountToAdd"></param>
        /// <returns></returns>
        public static ShopInventoryProbe CreateInventoryShopProbe(ItemFoundInShopInventory ItemFoundInShopConditional , Enums.SDVBigCraftable itemToAdd, int SellingPrice, int AmountToAdd=-1)
        {
            return CreateInventoryShopProbe(ItemFoundInShopConditional, new ItemReference(itemToAdd), SellingPrice, AmountToAdd);
        }

        /// <summary>
        /// Helper method for creating <see cref="ShopInventoryProbe"/>s which allow for adding items to a shop in a specific order based on given conditions. If true, then the new item is found after the one matching this param.
        /// </summary>
        /// <param name="itemToAdd"></param>
        /// <param name="SellingPrice"></param>
        /// <param name="AmountToAdd"></param>
        /// <returns></returns>
        public static ShopInventoryProbe CreateInventoryShopProbe(ItemFoundInShopInventory ItemFoundInShopConditional , string itemToAdd, int SellingPrice, int AmountToAdd=-1)
        {
            return CreateInventoryShopProbe(ItemFoundInShopConditional, new ItemReference(itemToAdd), SellingPrice, AmountToAdd);
        }

        /// <summary>
        /// Helper method for creating <see cref="ShopInventoryProbe"/>s which allow for adding items to a shop in a specific order based on given conditions. If true, then the new item is found after the one matching this param.
        /// </summary>
        /// <param name="ItemFoundInShopConditional"></param>
        /// <param name="itemToAdd"></param>
        /// <param name="SellingPrice"></param>
        /// <param name="AmountToAdd"></param>
        /// <returns></returns>
        public static ShopInventoryProbe CreateInventoryShopProbe(ItemFoundInShopInventory ItemFoundInShopConditional ,ItemReference itemToAdd, int SellingPrice, int AmountToAdd=-1)
        {
            return new ShopInventoryProbe(
                ItemFoundInShopConditional,
                new UpdateShopInventory((ShopInventory, ItemForSale, Price, Stock) =>
                {
                    ShopInventory.addItemForSale(itemToAdd.getItem(), SellingPrice, AmountToAdd);
                    return ShopInventory;
                }
            ));
        }


        /// <summary>
        /// Updates a stock of a shop in a given order based on various conditions.
        /// </summary>
        /// <param name="Menu"></param>
        /// <param name="shopPopulationMethods"></param>
        /// <returns></returns>
        public static void UpdateShopStockAndPriceInSortedOrder(ShopMenu Menu, List<ShopInventoryProbe> shopPopulationMethods)
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
    }
}
