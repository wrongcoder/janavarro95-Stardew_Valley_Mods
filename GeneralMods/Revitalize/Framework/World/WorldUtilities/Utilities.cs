using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Constants.ItemIds.Objects;
using Revitalize.Framework.Objects;
using Revitalize.Framework.World.Objects.Machines;
using StardewValley;

namespace Revitalize.Framework.World.WorldUtilities
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
            GameLocation cinderSapForestLocation = Game1.getLocationFromName("Forest");
            HayMaker hayMaker = (ModCore.ObjectManager.GetObject<HayMaker>(Machines.HayMaker, 1).getOne(true) as HayMaker);
            if (ModCore.Configs.shopsConfigManager.hayMakerShopConfig.IsHayMakerShopSetUpOutsideOfMarniesRanch &&
                cinderSapForestLocation.isObjectAtTile((int)ModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.X, (int)ModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.Y) == false)
            {
                hayMaker.placementActionAtTile(cinderSapForestLocation, (int)ModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.X, (int)ModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.Y);
            }

        }

    }
}
