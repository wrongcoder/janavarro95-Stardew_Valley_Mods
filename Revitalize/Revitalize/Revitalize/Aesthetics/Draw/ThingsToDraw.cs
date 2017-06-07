using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Draw
{
    class ThingsToDraw
    {

        public static void drawAllHuds()
        {
            Magic.MagicMonitor.drawMagicMeter();
        }


    }
}
