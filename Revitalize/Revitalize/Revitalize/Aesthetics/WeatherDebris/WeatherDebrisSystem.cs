using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Aesthetics.WeatherDebris
{

    public class weatherNode
    {
        public WeatherDebrisPlus weatherDebris;
        public int TimesToAdd;

        public weatherNode(WeatherDebrisPlus w, int addThisMany)
        {
            weatherDebris = w;
            TimesToAdd = addThisMany;
        }
    }

    class WeatherDebrisSystem
    {
        public static List<WeatherDebrisPlus> thisWeatherDebris;
        public static void update()
        {
            foreach (WeatherDebrisPlus w in WeatherDebrisSystem.thisWeatherDebris)
            {
                // Log.AsyncM("COUNT" + Lists.thisWeatherDebris.Count);
                w.update();
            }
        }

        public static void draw()
        {
            if (Game1.player.currentLocation.ignoreDebrisWeather == false)
            {
                foreach (WeatherDebrisPlus w in WeatherDebrisSystem.thisWeatherDebris)
                {
                    w.draw(Game1.spriteBatch);
                }
            }

        }

        public static void cleanWeatherDebris()
        {
            thisWeatherDebris.Clear();
        }

        public static void addMultipleDebrisWithVaryingCounts(List<weatherNode> listToAdd)
        {
            foreach(var v in listToAdd)
            {
                if (v.TimesToAdd == 0 || v.weatherDebris==null) continue;
                for(int i=1; i <= v.TimesToAdd; i++)
                {
                    thisWeatherDebris.Add(v.weatherDebris);
                }
            }
        }

        public static void addMultipleDebrisFromSingleType(weatherNode w)
        {
            if (w.TimesToAdd == 0 || w.weatherDebris == null) return;
            for(int i = 1; i <= w.TimesToAdd; i++)
            {
              var v=  new WeatherDebrisPlus(new Vector2((float)Game1.random.Next(0, Game1.graphics.GraphicsDevice.Viewport.Width), (float)Game1.random.Next(0, Game1.graphics.GraphicsDevice.Viewport.Height)), w.weatherDebris.sourceRect, w.weatherDebris.animationIntervalOffset, w.weatherDebris.which, (float)Game1.random.Next(15) / 500f, (float)Game1.random.Next(-10, 0) / 50f, (float)Game1.random.Next(10) / 50f, true);
                thisWeatherDebris.Add(v);
            }
        }

    }
}
