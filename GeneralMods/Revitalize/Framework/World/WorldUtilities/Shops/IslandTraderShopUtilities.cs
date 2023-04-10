using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.CraftingIds;
using Omegasis.Revitalize.Framework.Constants.CraftingIds.RecipeIds;
using Omegasis.Revitalize.Framework.Constants.Ids.Items.BlueprintIds;
using StardewValley.Menus;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Shops
{
    public static class IslandTraderShopUtilities
    {
        public static void AddStockToShop(ShopMenu menu)
        {
            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.MidnightCask, RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.MidnightCask), 50, Enums.SDVObject.Wine);
            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.AdvancedSolarPanel, RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.AdvancedSolarPanel), 250, Enums.SDVObject.SolarEssence);

            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, Constants.CraftingIds.CraftingRecipeBooks.WorkbenchCraftingRecipies, Constants.CraftingIds.RecipeIds.WorkbenchRecipeIds.HardwoodPreservesJar, RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.HardwoodPreservesJar), 10, Enums.SDVObject.Jelly);
            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, Constants.CraftingIds.CraftingRecipeBooks.WorkbenchCraftingRecipies, Constants.CraftingIds.RecipeIds.WorkbenchRecipeIds.HardwoodPreservesJar, RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.HardwoodPreservesJar), 10, Enums.SDVObject.Pickles);
            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.AncientPreservesJar, RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.AncientPreservesJar), 40, Enums.SDVObject.Jelly);
            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.AncientPreservesJar, RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.AncientPreservesJar), 40, Enums.SDVObject.Pickles);
        }

    }
}
