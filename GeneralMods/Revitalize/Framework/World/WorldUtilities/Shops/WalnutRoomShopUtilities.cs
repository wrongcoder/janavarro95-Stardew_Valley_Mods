using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Configs.ShopConfigs;
using Omegasis.Revitalize.Framework.Constants.CraftingIds;
using Omegasis.Revitalize.Framework.Constants.CraftingIds.RecipeIds;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Items.BlueprintIds;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Objects;
using Omegasis.Revitalize.Framework.Player;
using Omegasis.Revitalize.Framework.World.Objects;
using StardewValley;
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
            WalnutRoomShopConfig shopConfig = RevitalizeModCore.Configs.shopsConfigManager.walnutRoomShopConfig;
            ObjectManager objectManager = RevitalizeModCore.ModContentManager.objectManager;

            if (!PlayerUtilities.KnowsCraftingRecipe(CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.NuclearFurnaceRecipeId))
            {
                ShopUtilities.AddItemToWalnutRoomShop(Menu, objectManager.getItem(WorkbenchBlueprintIds.NuclearFurnaceBlueprint), shopConfig.NuclearFurnaceBlueprintQiGemPrice, 1);
            }
            if (!PlayerUtilities.KnowsCraftingRecipe(CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.MagicalFurnaceRecipeId))
            {
                ShopUtilities.AddItemToWalnutRoomShop(Menu, objectManager.getItem(WorkbenchBlueprintIds.MagicalFurnaceBlueprint), shopConfig.MagicalFurnaceBlueprintQigemPrice, 1);
            }
            if (!PlayerUtilities.KnowsCraftingRecipe(CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.RadioactiveFuelCellRecipeId))
            {
                ShopUtilities.AddItemToWalnutRoomShop(Menu, objectManager.getItem(WorkbenchBlueprintIds.RadioactiveFuelCellBlueprint),shopConfig.RadioactiveFuelBlueprintQiGemPrice,1);
            }

            if (!PlayerUtilities.KnowsCraftingRecipe(CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.NuclearMiningDrillRecipeId) && Game1.player.hasSkullKey)
            {
                ShopUtilities.AddItemToWalnutRoomShop(Menu, objectManager.getItem(WorkbenchBlueprintIds.NuclearMiningDrillBlueprint), shopConfig.NuclearMiningDrillBlueprintPrice, 1);
            }
            if (!PlayerUtilities.KnowsCraftingRecipe(CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipeIds.MagicalMiningDrillRecipeId) && Game1.player.hasSkullKey)
            {
                ShopUtilities.AddItemToWalnutRoomShop(Menu, objectManager.getItem(WorkbenchBlueprintIds.MagicalMiningDrillBlueprint), shopConfig.MagicalMiningDrillBlueprintPrice, 1);
            }

            if (Game1.player.MiningLevel >= 10 && Game1.player.hasSkullKey)
            {
                ShopUtilities.AddItemToWalnutRoomShop(Menu, objectManager.getItem(ResourceObjectIds.CoalBush), shopConfig.CoalResourceBushQiGemPrice);
                ShopUtilities.AddItemToWalnutRoomShop(Menu, objectManager.getItem(ResourceObjectIds.CopperOreBush), shopConfig.CopperOreResourceBushQiGemPrice);
                ShopUtilities.AddItemToWalnutRoomShop(Menu, objectManager.getItem(ResourceObjectIds.IronOreBush), shopConfig.IronOreResourceBushQiGemPrice);
                ShopUtilities.AddItemToWalnutRoomShop(Menu, objectManager.getItem(ResourceObjectIds.GoldOreBush), shopConfig.GoldOreResourceBushQiGemPrice);
                ShopUtilities.AddItemToWalnutRoomShop(Menu, objectManager.getItem(ResourceObjectIds.IridiumOreBush), shopConfig.IridiumOreResoureceBushQiGemPrice);
                ShopUtilities.AddItemToWalnutRoomShop(Menu, objectManager.getItem(ResourceObjectIds.RadioactiveOreBush), shopConfig.RadioactiveOreResoureceBushQiGemPrice);
            }
        }

    }
}
