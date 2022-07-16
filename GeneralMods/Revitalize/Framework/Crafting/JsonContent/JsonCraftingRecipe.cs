using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Crafting.JsonContent
{
    /// <summary>
    /// TODO: Fill this out.
    /// </summary>
    public class JsonCraftingRecipe
    {

        public List<JsonCraftingComponent> inputs;
        public List<JsonCraftingComponent> outputs;

        public JsonCraftingRecipe()
        {
            this.inputs = new List<JsonCraftingComponent>();
            this.outputs = new List<JsonCraftingComponent>();
        }

        

    }
}
