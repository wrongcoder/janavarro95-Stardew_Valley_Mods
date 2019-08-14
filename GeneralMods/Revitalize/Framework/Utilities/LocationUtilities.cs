using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Revitalize.Framework.Objects;
using StardewValley;

namespace Revitalize.Framework.Utilities
{
    public class LocationUtilities
    {
        /// <summary>
        /// Checks to see if the player is in the regular mine.
        /// </summary>
        /// <returns></returns>
        public static bool IsPlayerInMine()
        {
            if (Game1.player.currentLocation.Name.StartsWith("UndergroundMine"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Checks to see if the player is in skull cave.
        /// </summary>
        /// <returns></returns>
        public static bool IsPlayerInSkullCave()
        {
            if (Game1.player.currentLocation.Name == "SkullCave" || Game1.player.currentLocation.Name.StartsWith("SkullCave"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the current mine level for the player. If the player is not in the mine this is -1.
        /// </summary>
        /// <returns></returns>
        public static int CurrentMineLevel()
        {
            if (IsPlayerInMine())
            {
                return (Game1.player.currentLocation as StardewValley.Locations.MineShaft).mineLevel;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets the tile width and height for the map.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Vector2 GetLocationTileDimensions(GameLocation location)
        {
            Vector2 dimensions = new Vector2(location.Map.GetLayer("Back").LayerWidth, location.Map.GetLayer("Back").LayerHeight);
            //ModCore.log("Dimensions of map is: " + dimensions);
            return dimensions;
        }

        /// <summary>
        /// Gets all open positions for this location for this object.
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="TestObject"></param>
        /// <returns></returns>
        public static List<Vector2> GetOpenObjectTiles(GameLocation Location,MultiTiledObject TestObject)
        {
            Vector2 dimensions = GetLocationTileDimensions(Location);
            List<Vector2> openTiles = new List<Vector2>();
            for(int i = 0; i < dimensions.X; i++)
            {
                for(int j = 0; j < dimensions.Y; j++)
                {
                    Vector2 tile = new Vector2(i, j);
                    if (TestObject.canBePlacedHere(Location, tile))
                    {
                        openTiles.Add(tile);
                    }
                }
            }
            return openTiles;
        }
    }
}
