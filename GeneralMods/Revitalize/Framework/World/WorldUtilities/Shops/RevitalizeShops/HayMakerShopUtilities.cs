using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Items;
using Omegasis.Revitalize.Framework.World.Objects;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using StardewValley;
using StardewValley.Menus;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Shops.RevitalizeShops
{
    /// <summary>
    /// Utilities for the Hay Maker Shop. 
    /// </summary>
    public static class HayMakerShopUtilities
    {
        public static string StoreContext = "Revitalize.Shops.HayMaker";

        public static void AddItemsToShop(ShopMenu Menu)
        {
            Menu.onPurchase = OnPurchaseFromShop;

            ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(Enums.SDVObject.Hay, -1), HaySellPrice(), -1);
            if(Game1.player.Money>= HaySellPrice())
            {
                ShopUtilities.AddItemToShop(Menu,RevitalizeModCore.ModContentManager.objectManager.getItem(FarmingItems.RefillSilosFakeItem), HaySellPrice() * FarmUtilities.GetNumberOfHayPiecesUntilFullSilosLimitByPlayersMoney(HaySellPrice()), -1);
            }
        }

        /// <summary>
        /// Called when purchasing an item from the hay maker shop.
        /// </summary>
        /// <param name="purchasedItem"></param>
        /// <param name="who"></param>
        /// <param name="AmountPurchased"></param>
        /// <returns>A bool representing if the menu should be closed or not.</returns>
        private static bool OnPurchaseFromShop(ISalable purchasedItem, Farmer who, int AmountPurchased)
        {
            if (purchasedItem is IBasicItemInfoProvider)
            {
                if((purchasedItem as IBasicItemInfoProvider).Id.Equals(FarmingItems.RefillSilosFakeItem))
                {
                    int addedHay = FarmUtilities.GetNumberOfHayPiecesUntilFullSilosLimitByPlayersMoney(HaySellPrice());
                    FarmUtilities.FillSilosFromSiloReillItem(RevitalizeModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerShopHaySellPrice);
                    SoundUtilities.PlaySound(Enums.StardewSound.Ship);
                    Game1.drawDialogueBox(string.Format("Added: {0} pieces of {1} to the silos on the farm.", addedHay, RevitalizeModCore.ModContentManager.objectManager.getItem(Enums.SDVObject.Hay).DisplayName));
                    return true;
                }
            }
            return false;
        }

        private static int HaySellPrice()
        {
            return RevitalizeModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerShopHaySellPrice;
        }
    }
}
