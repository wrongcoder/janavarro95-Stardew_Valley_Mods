using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Crafting
{
    public class UnlockableCraftingRecipe
    {
        public Recipe recipe;
        public bool hasUnlocked;
        public string whichTab;

        public UnlockableCraftingRecipe()
        {

        }

        public UnlockableCraftingRecipe(string WhichTab, Recipe recipe, bool HasUnlocked = false)
        {
            this.recipe = recipe;
            this.hasUnlocked = HasUnlocked;
            this.whichTab = WhichTab;
        }

        public void unlock()
        {
            this.hasUnlocked = true;
        }

    }
}
