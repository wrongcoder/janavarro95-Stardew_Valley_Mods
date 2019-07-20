using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Revitalize.Framework.Utilities
{
    public static class Vector2Utilities
    {
        public static double Magnitude(this Vector2 vec)
        {
            return Math.Sqrt(Math.Pow(vec.X, 2) + Math.Pow(vec.Y, 2));
        }

        /// <summary>
        /// Returns the unit vector.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Vector2 UnitVector(this Vector2 vec)
        {
            double mag = Magnitude(vec);
            return new Vector2((float)(vec.X / mag),(float)(vec.Y / mag));

        }

    }
}
