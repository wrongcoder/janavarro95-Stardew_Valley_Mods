using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize
{
    class Util
    {


     

        public static Microsoft.Xna.Framework.Rectangle parseRectFromJson(string s){

           

            s = s.Replace('{', ' ');
            s = s.Replace('}', ' ');
            s = s.Replace('^', ' ');
            s = s.Replace(':', ' ');
            string[] parsed = s.Split(' ');
            foreach (var v in parsed)
            {
                //Log.AsyncY(v);
            }
            return new Microsoft.Xna.Framework.Rectangle(Convert.ToInt32(parsed[2]), Convert.ToInt32(parsed[4]), Convert.ToInt32(parsed[6]), Convert.ToInt32(parsed[8]));
            }

        public static bool addItemToInventoryElseDrop(Item I)
        {
            if (Game1.player.isInventoryFull() == false)
            {
                Game1.player.addItemToInventoryBool(I, false);
                return true;
            }
            else {
                Random random = new Random(129);
                int i = random.Next();
                i = i % 4;
                Vector2 v2 = new Vector2(Game1.player.getTileX() * Game1.tileSize, Game1.player.getTileY() * Game1.tileSize);
                Game1.createItemDebris(I, v2, i);
                return false;
            }
        }

        public static Color invertColor(Color c)
        {
            int r;
            int g;
            int b;
            int a=255;

            r = 255-c.R;
            g = 255-c.G;
            b = 255 - c.B;
           // a = 255 - c.A;

            return new Color(r, g, b, a);

        }
    }
}
