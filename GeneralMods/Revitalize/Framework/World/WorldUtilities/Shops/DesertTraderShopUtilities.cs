using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants.Ids.Items.BlueprintIds;
using Omegasis.Revitalize.Framework.Constants;
using StardewValley.Menus;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Shops
{
    public static class DesertTraderShopUtilities
    {

        public static void AddStockToShop(ShopMenu menu)
        {
            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, Constants.CraftingIds.CraftingRecipeBooks.WorkbenchCraftingRecipies, Constants.CraftingIds.RecipeIds.WorkbenchRecipeIds.DarkwoodCask ,RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.DarkwoodCask), 20, Enums.SDVObject.Wine);
        }
    }
}
