using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Crafting
{
    /// <summary>
    /// Returns results on crafting recipes on whether or not they were successfula s well as information on the actual crafting recipe.
    /// </summary>
    public class CraftingRecipeResult
    {

        public bool successful;
        public Recipe recipe;

        public CraftingRecipeResult()
        {

        }

        public CraftingRecipeResult(Recipe recipe, bool Success)
        {
            this.recipe = recipe;
            this.successful = Success;
        }

    }
}
