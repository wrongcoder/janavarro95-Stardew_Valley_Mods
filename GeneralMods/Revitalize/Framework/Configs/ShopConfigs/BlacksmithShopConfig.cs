using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize;

namespace Revitalize.Framework.Configs.ShopConfigs
{
    /// <summary>
    /// Deals with the sell prices for ores in the blacksmith shop.
    /// </summary>
    public class BlacksmithShopConfig
    {
        /// <summary>
        /// The sell price for tin ore from the blacksmith.
        /// </summary>
        public int tinOreSellPrice;
        /// <summary>
        /// The sell price for bauxite ore from the blacksmith.
        /// </summary>
        public int bauxiteOreSellPrice;
        /// <summary>
        /// The sell price for lead ore from the blacksmith.
        /// </summary>
        public int leadOreSellPrice;
        /// <summary>
        /// The sell price for silver ore from the blacksmith.
        /// </summary>
        public int silverOreSellPrice;
        /// <summary>
        /// The sell price for titanium ore from the blacksmith.
        /// </summary>
        public int titaniumOreSellPrice;

        /// <summary>
        /// Constructor.
        /// </summary>
        public BlacksmithShopConfig()
        {
            this.tinOreSellPrice = 100;
            this.bauxiteOreSellPrice = 150;
            this.leadOreSellPrice = 200;
            this.silverOreSellPrice = 250;
            this.titaniumOreSellPrice = 300;
        }

    }
}
