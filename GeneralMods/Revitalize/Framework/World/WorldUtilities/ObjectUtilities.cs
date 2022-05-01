using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities
{
    public static class ObjectUtilities
    {
        /// <summary>
        /// Gets the objects that are at adjacent tile positions (and potentially including this one!).
        /// </summary>
        /// <param name="location"></param>
        /// <param name="StartingPosition"></param>
        /// <param name="IncludeStartingTilePosition"></param>
        /// <returns></returns>
        public static Dictionary<Vector2,StardewValley.Object> GetAllAdjacentObjectsOrNull(GameLocation location, Vector2 StartingPosition, bool IncludeStartingTilePosition=true)
        {
            Dictionary<Vector2, StardewValley.Object> adjacentObjects = new Dictionary<Vector2, StardewValley.Object>();

            //Can return null if no object at position.
            for (int x = (int)StartingPosition.X - 1; x <= (int)StartingPosition.X + 1; x++)
            {
                for (int y = (int)StartingPosition.Y - 1; y <= StartingPosition.Y + 1; y++)
                {

                    StardewValley.Object obj = location.getObjectAtTile(x, y);
                    Vector2 tilePosition = new Vector2(x, y);
                    if (tilePosition.X == StartingPosition.X && tilePosition.Y == StartingPosition.Y && IncludeStartingTilePosition)
                    {
                        adjacentObjects.Add(tilePosition, obj);
                    }
                    else
                    {
                        adjacentObjects.Add(tilePosition, obj);
                    }

                }
            }
            return adjacentObjects;

        }

        public static Dictionary<Vector2, StardewValley.Object> GetAllConnectedObjectsStartingAtTilePosition(GameLocation location,Vector2 TilePosition, bool IncludeStartingTilePosition)
        {

            HashSet<Vector2> exploredTiles = new HashSet<Vector2>();
            Dictionary<Vector2, StardewValley.Object> connectedObjects = new Dictionary<Vector2, StardewValley.Object>();

            //get starting object,
            //get list of connected adjacent objects.

            Queue<Vector2> tilesToExplore = new Queue<Vector2>();
            tilesToExplore.Enqueue(TilePosition);

            while (tilesToExplore.Count > 0)
            {
                Vector2 tileToExplore = tilesToExplore.Dequeue();

                if (exploredTiles.Contains(tileToExplore))
                {
                    continue;
                }

                Dictionary<Vector2, StardewValley.Object> objects = GetAllAdjacentObjectsOrNull(location, tileToExplore, true);
                foreach(KeyValuePair<Vector2,StardewValley.Object> tileToObject in objects)
                {

                    if (exploredTiles.Contains(tileToExplore))
                    {
                        continue;
                    }
                    if (tileToObject.Value == null)
                    {
                        continue;
                    }

                    exploredTiles.Add(tileToObject.Key);
                    connectedObjects.Add(tileToObject.Key,tileToObject.Value);
                    tilesToExplore.Enqueue(tileToObject.Key);

                }

            }
            return connectedObjects;


        }


    }
}
