using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Objects;
using Omegasis.Revitalize.Framework.World.Objects;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using Omegasis.Revitalize.Framework.World.Objects.Machines.Furnaces;
using Omegasis.Revitalize.Framework.World.Objects.Machines.Misc;
using Omegasis.RevitalizeAutomateCompatibility.Objects;
using Omegasis.RevitalizeAutomateCompatibility.Objects.Machines;
using Pathoschild.Stardew.Automate;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.TerrainFeatures;

namespace Omegasis.RevitalizeAutomateCompatibility
{
    public class RevitalizeAutomationFactory : IAutomationFactory
    {
        /// <summary>Get a machine, container, or connector instance for a given object.</summary>
        /// <param name="obj">The in-game object.</param>
        /// <param name="location">The location to check.</param>
        /// <param name="tile">The tile position to check.</param>
        /// <returns>Returns an instance or <c>null</c>.</returns>
        public IAutomatable GetFor(StardewValley.Object obj, GameLocation location, in Vector2 tile)
        {
            if (obj is ICustomModObject)
            {
                ICustomModObject modObj = (obj as ICustomModObject);
                if (modObj.Id.Equals(MachineIds.ElectricFurnace) || modObj.Id.Equals(MachineIds.NuclearFurnace) || modObj.Id.Equals(MachineIds.MagicalFurnace))
                {
                    return new PoweredMachineWrapper<ElectricFurnace>((ElectricFurnace)obj, location, tile);
                }

                if(modObj.Id.Equals(MachineIds.CoalAdvancedGeodeCrusher) || modObj.Id.Equals(MachineIds.ElectricAdvancedGeodeCrusher) || modObj.Id.Equals(MachineIds.NuclearAdvancedGeodeCrusher) || modObj.Id.Equals(MachineIds.MagicalAdvancedGeodeCrusher))
                {
                    return new PoweredMachineWrapper<AdvancedGeodeCrusher>((AdvancedGeodeCrusher)obj, location, tile);
                }

                if (modObj.Id.Equals(MachineIds.AdvancedCharcoalKiln) || modObj.Id.Equals(MachineIds.DeluxCharcoalKiln) || modObj.Id.Equals(MachineIds.SuperiorCharcoalKiln))
                {
                    return new MachineWrapper<AdvancedCharcoalKiln>((AdvancedCharcoalKiln)obj, location, tile);
                }
            }

            return null;
        }

        /// <summary>Get a machine, container, or connector instance for a given terrain feature.</summary>
        /// <param name="feature">The terrain feature.</param>
        /// <param name="location">The location to check.</param>
        /// <param name="tile">The tile position to check.</param>
        /// <returns>Returns an instance or <c>null</c>.</returns>
        public IAutomatable GetFor(TerrainFeature feature, GameLocation location, in Vector2 tile)
        {
            return null;
        }

        /// <summary>Get a machine, container, or connector instance for a given building.</summary>
        /// <param name="building">The building.</param>
        /// <param name="location">The location to check.</param>
        /// <param name="tile">The tile position to check.</param>
        /// <returns>Returns an instance or <c>null</c>.</returns>
        public IAutomatable GetFor(Building building, BuildableGameLocation location, in Vector2 tile)
        {
            return null;
        }

        /// <summary>Get a machine, container, or connector instance for a given tile position.</summary>
        /// <param name="location">The location to check.</param>
        /// <param name="tile">The tile position to check.</param>
        /// <returns>Returns an instance or <c>null</c>.</returns>
        /// <remarks>Shipping bin logic from <see cref="Farm.leftClick"/>, garbage can logic from <see cref="Town.checkAction"/>.</remarks>
        public IAutomatable GetForTile(GameLocation location, in Vector2 tile)
        {
            return null;
        }
    }
}
