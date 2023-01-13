using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.Revitalize.Framework.Crafting
{
    public class CraftingRecipeComponent
    {
        public Item item;
        protected int requiredAmount;
        protected int minAmount;
        protected int maxAmount;


        public CraftingRecipeComponent()
        {

        }

        public CraftingRecipeComponent(Item I, int RequiredAmount)
        {
            this.item = I;
            this.requiredAmount = RequiredAmount;
        }

        public CraftingRecipeComponent(Item I, int MinAmount, int MaxAmount)
        {
            this.item = I;
            this.minAmount = MinAmount;
            this.maxAmount = MaxAmount;
        }

        /// <summary>
        /// Returns the 
        /// </summary>
        /// <returns></returns>
        public virtual int getRequiredAmount()
        {
            if(this.requiredAmount!=0) return this.requiredAmount;
            return Game1.random.Next(this.minAmount, this.maxAmount + 1);
        }

        public virtual int getMinStackSize()
        {
            if(this.requiredAmount!=0) return this.requiredAmount;
            return this.minAmount;
        }

        public virtual int getMaxStackSize()
        {
            if (this.requiredAmount != 0) return this.requiredAmount;
            return this.maxAmount;
        }

    }
}
