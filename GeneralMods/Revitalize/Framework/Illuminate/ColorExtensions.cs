using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Illuminate
{
    public static class ColorExtensions
    {
        public static Color GreyScaleAverage(this Color color)
        {
            int value = (color.R + color.G + color.B) / 3;
            return new Color(new Vector3(value));
        }

    }
}
