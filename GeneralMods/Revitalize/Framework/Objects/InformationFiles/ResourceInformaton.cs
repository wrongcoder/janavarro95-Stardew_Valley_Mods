using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Revitalize.Framework.Objects.InformationFiles
{
    /// <summary>
    /// Deals with information reguarding resources.
    /// </summary>
    public class ResourceInformaton
    {
        /// <summary>
        /// The item to drop.
        /// </summary>
        public Item droppedItem;
        /// <summary>
        /// The min amount to drop.
        /// </summary>
        public int minDropAmount;
        /// <summary>
        /// The max amount to drop.
        /// </summary>
        public int maxDropAmount;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public ResourceInformaton()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="I">The item to drop.</param>
        /// <param name="Min">The min amount to drop.</param>
        /// <param name="Max">The max amount to drop.</param>
        public ResourceInformaton(Item I, int Min, int Max)
        {
            this.droppedItem = I;
            this.minDropAmount = Min;
            this.maxDropAmount = Max;
        }
    }
}
