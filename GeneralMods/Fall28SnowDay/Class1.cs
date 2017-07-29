using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Omegasis.Fall28SnowDay
{
    /// <summary>The mod entry point.</summary>
    public class Fall28SnowDay : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            TimeEvents.DayOfMonthChanged += this.TimeEvents_DayOfMonthChanged;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked when <see cref="Game1.dayOfMonth"/> changes.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void TimeEvents_DayOfMonthChanged(object sender, EventArgsIntChanged e)
        {
            if (Game1.dayOfMonth == 27 && Game1.IsFall)
                Game1.weatherForTomorrow = Game1.weather_snow;
        }
    }
}
