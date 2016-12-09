using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;

namespace Fall28_Snow_Day
{
    public class Class1:Mod
    {
        public override void Entry(params object[] objects)
        {
            StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += TimeEvents_DayOfMonthChanged;
        }

        public void TimeEvents_DayOfMonthChanged(object sender, StardewModdingAPI.Events.EventArgsIntChanged e)
        {
            if (StardewValley.Game1.dayOfMonth == 27 && Game1.IsFall == true)
            {
                Log.Success("Weather checker now!!!");
                Game1.weatherForTomorrow = Game1.weather_snow;
            }
        }
    }
}
