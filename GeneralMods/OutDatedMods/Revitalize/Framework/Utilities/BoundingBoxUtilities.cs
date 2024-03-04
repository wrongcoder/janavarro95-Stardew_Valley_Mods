using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Omegasis.Revitalize.Framework.Utilities
{
    public class BoundingBoxUtilities
    {
        /// <summary>
        /// Generates a vector2 from the input paramaters for tilesize and pixels for mapping texture pixel cordinates to game tile sizes.
        /// </summary>
        /// <param name="TilesX"></param>
        /// <param name="PixelsX"></param>
        /// <param name="TilesY"></param>
        /// <param name="PixelsY"></param>
        /// <param name="TileSize"></param>
        /// <returns></returns>
        public static Vector2 GenerateVector2FromTextureDimensions(int TilesX, int PixelsX, int TilesY, int PixelsY, int TileSize)
        {
            return new Vector2(TilesX + ((float)PixelsX / TileSize), TilesY + ((float)PixelsY / TileSize));
        }
    }
}
