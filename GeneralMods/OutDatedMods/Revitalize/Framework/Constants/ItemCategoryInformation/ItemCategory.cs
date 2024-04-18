using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Omegasis.Revitalize.Framework.Constants.ItemCategoryInformation
{
    /// <summary>
    /// A given category for a specific <see cref="StardewValley.Item"/>
    /// </summary>
    public class ItemCategory
    {
        /// <summary>
        /// The name of the category.
        /// </summary>
        public string name;
        /// <summary>
        /// The color of the category.
        /// </summary>
        public Color color;

        public ItemCategory()
        {

        }

        public ItemCategory(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }
    }
}
