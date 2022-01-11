using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Revitalize.Framework.World.WorldUtilities
{
    /// <summary>
    /// Utilities pertaining to the game locations in Stardew Valley.
    /// </summary>
    public class GameLocationUtilities
    {
        /// <summary>
        /// Gets a game location from a StardewLocation enum value.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static GameLocation GetGameLocation(Enums.StardewLocation location)
        {
            return Game1.getLocationFromName(Enum.GetName<Enums.StardewLocation>(location));
        }

    }
}
