
using System.Collections.Generic;
using Newtonsoft.Json;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.World.Objects.Crafting;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;

namespace Omegasis.Revitalize.Framework.World.Objects.InformationFiles.Json.Crafting
{
    /// <summary>
    /// Holds information for blueprint items in json format.
    /// </summary>
    public class JsonCraftingBlueprint:JsonBasicItemInformation
    {
        public ItemReference itemToDraw;

        public List<CraftingBookIdToRecipeId> recipesToUnlock;

        public JsonCraftingBlueprint()
        {
            this.itemToDraw = new ItemReference();
            this.recipesToUnlock = new List<CraftingBookIdToRecipeId>();
        }

        public virtual Blueprint toBlueprint()
        {
            return new Blueprint(this);
        }

    }
}
