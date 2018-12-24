using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Illuminate;
using System.IO;

namespace Revitalize.Framework.Environment
{
    public class DarkerNight
    {
        
        public static float IncrediblyDark = 0.9f;
        public static float VeryDark = 0.75f;
        public static float SomewhatDark = 0.50f;

        public static DarkerNightConfig Config;

        public static void InitializeConfig()
        {
            if (File.Exists(Path.Combine(Revitalize.ModCore.ModHelper.DirectoryPath, "Configs", "DarkerNightConfig.json")))
            {
                Config = Revitalize.ModCore.ModHelper.Data.ReadJsonFile<DarkerNightConfig>(Path.Combine("Configs", "DarkerNightConfig.json"));
            }
            else
            {
                Config = new DarkerNightConfig();
                Revitalize.ModCore.ModHelper.Data.WriteJsonFile<DarkerNightConfig>(Path.Combine("Configs", "DarkerNightConfig.json"),Config);
            }
        }

        public static void setDarkerNightColor()
        {
            if (Game1.player == null) return;
            if (Game1.player.currentLocation.IsOutdoors && Game1.timeOfDay>= Game1.getTrulyDarkTime())
            {
                //Game1.ambientLight = Game1.ambientLight.GreyScaleAverage();
                Game1.outdoorLight = Game1.ambientLight*Config.DarknessIntensity;

                Revitalize.ModCore.log("OUT: " + Game1.outdoorLight);
                Revitalize.ModCore.log("Ambient"+Game1.ambientLight);
            }
        }


    }
}
