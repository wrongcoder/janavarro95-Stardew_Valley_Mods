using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants.CraftingIds;
using Omegasis.Revitalize.Framework.Constants.CraftingIds.RecipeIds;
using Omegasis.Revitalize.Framework.Player;
using Omegasis.Revitalize.Framework.World.Objects;
using StardewValley.Menus;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Shops
{
    public static class WalnutRoomShopUtilities
    {

        /// <summary>
        /// What happens if a new day starts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void OnNewDay(object sender, StardewModdingAPI.Events.DayStartedEventArgs args)
        {

        }

        public static void AddItemsToShop(ShopMenu Menu)
        {
            if (!PlayerUtilities.KnowsCraftingRecipe(CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.NuclearFurnaceRecipeId))
            {
                ShopUtilities.AddItemToWalnutRoomShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(Constants.ItemIds.Items.BlueprintIds.WorkbenchBlueprintIds.Workbench_NuclearFurnaceCraftingRecipeBlueprint), 20, 1);
            }
            if (!PlayerUtilities.KnowsCraftingRecipe(CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.MagicalFurnaceRecipeId))
            {
                ShopUtilities.AddItemToWalnutRoomShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(Constants.ItemIds.Items.BlueprintIds.WorkbenchBlueprintIds.Workbench_MagicalFurnaceCraftingRecipeBlueprint), 40, 1);
            }
        }

    }
}
