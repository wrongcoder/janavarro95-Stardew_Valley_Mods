using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Objects;
using Omegasis.Revitalize.Framework.World.Objects.Machines;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities
{
    public class Utilities
    {

        public static void InitializeGameWorld()
        {
            AddModdedMachinesToGameWorld();
        }

        /// <summary>
        /// Adds various machines and stuff to the game world.
        /// </summary>
        private static void AddModdedMachinesToGameWorld()
        {
            GameLocation cinderSapForestLocation = GameLocationUtilities.GetGameLocation(Enums.StardewLocation.Forest);
            HayMaker hayMaker = (RevitalizeModCore.ObjectManager.getObject<HayMaker>(Machines.HayMaker, 1).getOne(true) as HayMaker);
            if (RevitalizeModCore.Configs.shopsConfigManager.hayMakerShopConfig.IsHayMakerShopSetUpOutsideOfMarniesRanch &&
                cinderSapForestLocation.isObjectAtTile((int)RevitalizeModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.X, (int)RevitalizeModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.Y) == false)
            {
                hayMaker.placementActionAtTile(cinderSapForestLocation, (int)RevitalizeModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.X, (int)RevitalizeModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.Y);
            }

        }

    }
}
