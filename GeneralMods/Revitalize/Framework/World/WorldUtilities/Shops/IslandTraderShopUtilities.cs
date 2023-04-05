using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.Ids.Items.BlueprintIds;
using StardewValley.Menus;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Shops
{
    public static class IslandTraderShopUtilities
    {
        public static void AddStockToShop(ShopMenu menu)
        {
            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, Constants.CraftingIds.CraftingRecipeBooks.WorkbenchCraftingRecipies, Constants.CraftingIds.RecipeIds.WorkbenchRecipeIds.MidnightCask, RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.MidnightCask), 50, Enums.SDVObject.Wine);
            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, Constants.CraftingIds.CraftingRecipeBooks.WorkbenchCraftingRecipies, Constants.CraftingIds.RecipeIds.WorkbenchRecipeIds.AdvancedSolarPanel, RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.AdvancedSolarPanel), 250, Enums.SDVObject.SolarEssence);
        }

    }
}
