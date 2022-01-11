using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Revitalize.Framework.World.WorldUtilities.Shops.ShopUtilities;

namespace Revitalize.Framework.World.WorldUtilities.Shops
{
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
}
