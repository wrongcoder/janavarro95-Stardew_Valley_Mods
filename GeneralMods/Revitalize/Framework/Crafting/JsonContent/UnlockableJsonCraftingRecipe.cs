using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Crafting.JsonContent
{
    public class UnlockableJsonCraftingRecipe
    {
        public string whichTab;
        public bool isUnlocked;
        public JsonCraftingRecipe recipe;


        public UnlockableJsonCraftingRecipe()
        {
            this.whichTab = "";
            this.isUnlocked = true;
            this.recipe = new JsonCraftingRecipe();
        }

        public UnlockableJsonCraftingRecipe(string WhichTab, JsonCraftingRecipe recipe, bool HasUnlocked = false)
        {
            this.recipe = recipe;
            this.isUnlocked = HasUnlocked;
            this.whichTab = WhichTab;
        }

        public virtual UnlockableCraftingRecipe toUnlockableCraftingRecipe()
        {
            return new UnlockableCraftingRecipe(this.whichTab, this.recipe.toCraftingRecipe(), this.isUnlocked);
        }
    }
}
