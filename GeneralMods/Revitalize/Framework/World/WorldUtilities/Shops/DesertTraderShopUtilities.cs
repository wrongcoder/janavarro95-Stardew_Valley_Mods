using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants.Ids.Items.BlueprintIds;
using Omegasis.Revitalize.Framework.Constants;
using StardewValley.Menus;
using Omegasis.Revitalize.Framework.Constants.CraftingIds.RecipeIds;
using Omegasis.Revitalize.Framework.Constants.CraftingIds;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Shops
{
    public static class DesertTraderShopUtilities
    {

        public static void AddStockToShop(ShopMenu menu)
        {
            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.DarkwoodCask ,RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.DarkwoodCask), 20, Enums.SDVObject.Wine);
            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.HardwoodPreservesJar, RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.HardwoodPreservesJar), 20, Enums.SDVObject.Jelly);
            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.HardwoodPreservesJar, RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.HardwoodPreservesJar), 20, Enums.SDVObject.Pickles);
            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.AncientPreservesJar, RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.AncientPreservesJar), 50, Enums.SDVObject.Jelly);
            ShopUtilities.AddItemToShopIfCraftingRecipeNotKnown(menu, CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.AncientPreservesJar, RevitalizeModCore.ModContentManager.objectManager.getItem(WorkbenchBlueprintIds.AncientPreservesJar), 50, Enums.SDVObject.Pickles);
        }
    }
}
