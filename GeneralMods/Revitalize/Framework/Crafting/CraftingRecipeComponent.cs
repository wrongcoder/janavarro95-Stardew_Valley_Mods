using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Revitalize.Framework.Crafting
{
    public class CraftingRecipeComponent
    {
        public Item item;
        public int requiredAmount;

        public CraftingRecipeComponent()
        {

        }

        public CraftingRecipeComponent(Item I, int RequiredAmount)
        {
            this.item = I;
            this.requiredAmount = RequiredAmount;
        }
    }
}
