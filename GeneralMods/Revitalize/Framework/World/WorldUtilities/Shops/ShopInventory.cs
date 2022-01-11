using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Revitalize.Framework.World.WorldUtilities.Shops
{
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
}
