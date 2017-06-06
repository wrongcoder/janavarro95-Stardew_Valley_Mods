using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile;

namespace Revitalize.Utilities
{
    class MapUtilities
    {

        public static void loadCustomFarmMap(GameLocation loc, Vector2 oldLoc, bool[,] oldWaterTiles)
        {
            //GameLocation location= Game1.getLocationFromName("Farm");
            transferWaterTiles(loc, oldLoc, oldWaterTiles);
            parseWarpsFromFile(loc);
        }

        public static void removeAllWaterTilesFromMap(GameLocation c)
        {
            Log.AsyncM(c.map.Layers[0].LayerWidth);
            Log.AsyncM(c.map.Layers[0].LayerHeight);
            for (int i = 0; i < c.map.Layers[0].LayerWidth; i++)
            {
                for (int j = 0; j < c.map.Layers[0].LayerHeight; j++)
                {
                    try
                    {
                        
                        c.waterTiles[i, j] = false;
                    }
                    catch (Exception e)
                    {

                    }
                }
                Log.AsyncY("Position: " + i +" of "+ c.map.Layers[0].LayerWidth);
            }
            Log.AsyncC("Removed All Water Tiles from " + c.name);
        }

        public static Vector2 getMapDimensions(GameLocation loc)
        {
            return new Vector2(loc.map.Layers[0].LayerWidth, loc.map.Layers[0].LayerHeight);
        }

        /// <summary>
        /// Attempts to take the information for waterTile array dimensions from a smaller map, and parse it into a much larger map.
        /// </summary>
        /// <param name="oldLoc"> Dimensions of old game location to transfer waterTile information from.</param>
        /// <param name="newLoc">New game location to transfer waterTile information from.</param>
        public static void transferWaterTiles(GameLocation newLoc, Vector2 oldLoc, bool[,] oldWaterTiles)
        {
            if (newLoc.map.Layers[0].LayerWidth < oldLoc.X && newLoc.map.Layers[0].LayerHeight < oldLoc.Y) return;
            bool[,] newWaterTiles = new bool[(int)Utilities.MapUtilities.getMapDimensions(newLoc).X, (int)Utilities.MapUtilities.getMapDimensions(newLoc).Y];
            for (int i = 0; i < oldLoc.X; i++)
            {
                for (int j = 0; j < oldLoc.Y; j++)
                {
                    try
                    {
                        
                        newLoc.waterTiles[i, j] = oldWaterTiles[i,j];
                    }
                    catch (Exception e)
                    {

                    }
                   
                }
                Log.AsyncY("Position: " + i + " of " + newLoc.map.Layers[0].LayerWidth);
            }
            newLoc.waterTiles = newWaterTiles;
            return;
        }

        public static void parseWarpsFromFile(GameLocation loc)
        {
            string[] s;
            loc.warps.Clear();
            try
            {
                s = Directory.GetFiles(Path.Combine(Class1.persistentMapSwap.folderPath, "Warps"));
            }
            catch(Exception e)
            {
                 s = Directory.GetFiles(Path.Combine(Class1.persistentMapSwap.folderPath, "warps"));
            }
            foreach (string file in s)
            {
                Log.AsyncO(file);
                string[] reader = File.ReadAllLines(file);
                try
                {
                    Warp w = new Warp(Convert.ToInt32(reader[1]), Convert.ToInt32(reader[3]), reader[5], Convert.ToInt32(reader[7]), Convert.ToInt32(reader[9]), Convert.ToBoolean(reader[11]));
                    loc.warps.Add(w);
                }
                catch (Exception err)
                {
                    Log.AsyncR(err);
                }
           }
        }
    }
}
