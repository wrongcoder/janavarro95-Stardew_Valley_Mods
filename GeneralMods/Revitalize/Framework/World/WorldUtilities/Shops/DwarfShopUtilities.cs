using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Objects;
using Omegasis.Revitalize.Framework.Player;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Shops
{
    public static class DwarfShopUtilities
    {

        public static int DwarfShop_NormalGeodesRemainingToday;
        public static int DwarfShop_FrozenGeodesRemainingToday;
        public static int DwarfShop_MagmaGeodesRemainingToday;
        public static int DwarfShop_OmniGeodesRemainingToday;


        public static int CoalBushesForSaleToday;
        public static int CopperBushesForSaleToday;
        public static int IronBushesForSaleToday;
        public static int GoldBushesForSaleToday;
        public static int IridiumBushesForSaleToday;
        public static int RadioactiveBushesForSaleToday;


        public static Func<ISalable, Farmer, int, bool> DwarfShop_DefaultOnPurchaseMethod;

        /// <summary>
        /// Determine's this shop's stock at the beginning of the day.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void OnNewDay(object sender, StardewModdingAPI.Events.DayStartedEventArgs args)
        {
            DwarfShop_NormalGeodesRemainingToday = RevitalizeModCore.Configs.shopsConfigManager.dwarfShopConfig.NumberOfNormalGeodesToSell;
            if (Game1.player.deepestMineLevel >= 40)
            {
                DwarfShop_FrozenGeodesRemainingToday = RevitalizeModCore.Configs.shopsConfigManager.dwarfShopConfig.NumberOfFrozenGeodesToSell;
            }
            if (Game1.player.deepestMineLevel >= 80)
            {
                DwarfShop_MagmaGeodesRemainingToday = RevitalizeModCore.Configs.shopsConfigManager.dwarfShopConfig.NumberOfMagmaGeodesToSell;
            }
            if (Game1.player.hasSkullKey && (Game1.dayOfMonth % 7 == 0 || RevitalizeModCore.Configs.shopsConfigManager.dwarfShopConfig.SellOmniGeodesEveryDayInsteadOnJustSundays))
            {
                //Add 1 omni geode on sundays.
                DwarfShop_OmniGeodesRemainingToday = RevitalizeModCore.Configs.shopsConfigManager.dwarfShopConfig.NumberOfOmniGeodesToSell;
            }
            else
            {
                DwarfShop_OmniGeodesRemainingToday = 0;
            }


            if (Game1.player.MiningLevel >= 10)
            {

                int coalChance = Game1.random.Next(101);
                //25% chance
                if (coalChance >= 75)
                {
                    CoalBushesForSaleToday = 1;
                }

                int copperChance = Game1.random.Next(101);
                //25% chance.
                if (copperChance >= 75)
                {
                    CopperBushesForSaleToday = 1;
                }

                if (Game1.player.deepestMineLevel >= 40)
                {

                    int ironChance = Game1.random.Next(101);
                    //20% chance
                    if (ironChance >= 80)
                    {
                        IronBushesForSaleToday = 1;
                    }
                }

                if (Game1.player.deepestMineLevel >= 80)
                {
                    int goldChance = Game1.random.Next(101);
                    //20% chance
                    if (goldChance >= 80)
                    {
                        GoldBushesForSaleToday = 1;
                    }
                }

                if (Game1.player.hasSkullKey)
                {
                    int iridiumChance = Game1.random.Next(101);
                    //15% chance.
                    if (iridiumChance >= 85)
                    {
                        IridiumBushesForSaleToday = 1;
                    }
                }

                //Only sell the radioactive bush if the player has a chance of getting the 'Danger in the Deep' quest.
                if (PlayerUtilities.GetNumberOfGoldenWalnutsFound() >= 100)
                {
                    int radioactiveChance = Game1.random.Next(101);
                    if (radioactiveChance >= 90)
                    {
                        RadioactiveBushesForSaleToday = 1;
                    }
                }

            }
        }

        /// <summary>
        /// Adds items to be sold by the dwarf.
        /// </summary>
        /// <param name="Menu"></param>
        public static void AddStockToDwarfShop(ShopMenu Menu)
        {
            DwarfShop_DefaultOnPurchaseMethod = Menu.onPurchase;
            Menu.onPurchase = OnPurchaseFromDwarfShop;

            if (DwarfShop_NormalGeodesRemainingToday > 0)
            {
               ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(Enums.SDVObject.Geode, DwarfShop_NormalGeodesRemainingToday), RevitalizeModCore.Configs.shopsConfigManager.dwarfShopConfig.NormalGeodePrice, DwarfShop_NormalGeodesRemainingToday);
            }
            if (DwarfShop_FrozenGeodesRemainingToday > 0)
            {
                ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(Enums.SDVObject.FrozenGeode, DwarfShop_FrozenGeodesRemainingToday), RevitalizeModCore.Configs.shopsConfigManager.dwarfShopConfig.FrozenGeodePrice, DwarfShop_FrozenGeodesRemainingToday);
            }
            if (DwarfShop_MagmaGeodesRemainingToday > 0)
            {
                ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(Enums.SDVObject.MagmaGeode, DwarfShop_MagmaGeodesRemainingToday), RevitalizeModCore.Configs.shopsConfigManager.dwarfShopConfig.MagmaGeodePrice, DwarfShop_MagmaGeodesRemainingToday);
            }
            if (DwarfShop_OmniGeodesRemainingToday > 0)
            {
                ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(Enums.SDVObject.OmniGeode, DwarfShop_OmniGeodesRemainingToday), RevitalizeModCore.Configs.shopsConfigManager.dwarfShopConfig.OmniGeodePrice, DwarfShop_OmniGeodesRemainingToday);
            }

            if (CoalBushesForSaleToday > 0)
            {
                ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(ResourceObjectIds.CoalBush), 10_000, CoalBushesForSaleToday);
            }
            if (CopperBushesForSaleToday > 0)
            {
                ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(ResourceObjectIds.CopperOreBush), 5_000, CopperBushesForSaleToday);
            }
            if (IronBushesForSaleToday > 0)
            {
                ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(ResourceObjectIds.IronOreBush), 15_000, IronBushesForSaleToday);
            }
            if (GoldBushesForSaleToday > 0)
            {
                ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(ResourceObjectIds.GoldOreBush), 20_000, GoldBushesForSaleToday);
            }
            if (IridiumBushesForSaleToday > 0)
            {
                ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(ResourceObjectIds.IridiumOreBush), 25_000, IridiumBushesForSaleToday);
            }
            if (RadioactiveBushesForSaleToday > 0)
            {
                ShopUtilities.AddItemToShop(Menu, RevitalizeModCore.ModContentManager.objectManager.getItem(ResourceObjectIds.RadioactiveOreBush), 100_000, RadioactiveBushesForSaleToday);
            }

        }

        /// <summary>
        /// What happens when an item is bought from this shop.
        /// </summary>
        /// <param name="purchasedItem"></param>
        /// <param name="who"></param>
        /// <param name="AmountPurchased"></param>
        /// <returns>True closes the shop menu, false does not.</returns>
        private static bool OnPurchaseFromDwarfShop(ISalable purchasedItem, Farmer who, int AmountPurchased)
        {

            ItemReference item = new ItemReference(purchasedItem);
            if (item.SdvObjectId == Enums.SDVObject.Geode)
            {
                DwarfShop_NormalGeodesRemainingToday -= AmountPurchased;
                return false;
            }
            if (item.SdvObjectId == Enums.SDVObject.FrozenGeode)
            {
                DwarfShop_FrozenGeodesRemainingToday -= AmountPurchased;
                return false;
            }
            if (item.SdvObjectId == Enums.SDVObject.MagmaGeode)
            {
                DwarfShop_MagmaGeodesRemainingToday -= AmountPurchased;
                return false;
            }
            if (item.SdvObjectId == Enums.SDVObject.OmniGeode)
            {
                DwarfShop_OmniGeodesRemainingToday -= AmountPurchased;
                return false;
            }

            if (item.ObjectManagerId.Equals(ResourceObjectIds.CoalBush))
            {
                CoalBushesForSaleToday--;
                return false;
            }
            if (item.ObjectManagerId.Equals(ResourceObjectIds.CopperOreBush))
            {
                CopperBushesForSaleToday--;
                return false;
            }
            if (item.ObjectManagerId.Equals(ResourceObjectIds.IronOreBush))
            {
                IronBushesForSaleToday--;
                return false;
            }
            if (item.ObjectManagerId.Equals(ResourceObjectIds.GoldOreBush))
            {
                GoldBushesForSaleToday--;
                return false;
            }
            if (item.ObjectManagerId.Equals(ResourceObjectIds.IridiumOreBush))
            {
                IridiumBushesForSaleToday--;
                return false;
            }
            if (item.ObjectManagerId.Equals(ResourceObjectIds.RadioactiveOreBush))
            {
                RadioactiveBushesForSaleToday--;
                return false;
            }

            if (DwarfShop_DefaultOnPurchaseMethod != null)
            {
                return DwarfShop_DefaultOnPurchaseMethod.Invoke(purchasedItem, who, AmountPurchased);
            }

            return false;
        }

    }
}
