using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace StarAI.PathFindingCore
{
    public class Utilities
    {

       public static  List<TileExceptionMetaData> tileExceptionList = new List<TileExceptionMetaData>();

        public static List<TileExceptionNode> ignoreCheckTiles = new List<TileExceptionNode>();
       public static string folderForExceptionTiles="ExceptionTilesData";

        public static Vector2 parseCenterFromTile(int tileX, int tileY)
        {
            //int x = (tileX * Game1.tileSize) + Game1.tileSize / 2;
            //int y = (tileY * Game1.tileSize) + Game1.tileSize / 2;
            int x = (tileX * Game1.tileSize);
            int y = (tileY * Game1.tileSize);
            return new Vector2(x, y);
        }

        public static void initializeTileExceptionList()
        {
            
        }
        

        public static void cleanExceptionList(TileNode t)
        {
            TileExceptionMetaData err= new TileExceptionMetaData(null,"");
            foreach (var v in tileExceptionList)
            {
                if (v.tile == t) err = v;
            }
            if(err.tile != null)
            {
                tileExceptionList.Remove(err);
            }
        }
        /// <summary>
        /// Used to calculate center of a tile with varience.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="goal"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool isWithinRange(float position, float goal, int tolerance)
        {
            if (position >= goal - tolerance && position <= goal + tolerance) return true;
            return false;
        }

    }
}
