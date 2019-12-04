using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.TimeSpecific
{
    public class WeatherPrecondition:EventPrecondition
    {
        public enum Weather
        {
            Sunny,
            Rainy,
            Debris,
            Storm,
            Festival,
            Snow,
            Wedding
        }

        public Weather weather;

        public WeatherPrecondition()
        {
            
        }

        /// <summary>
        /// Creates the prconiditon that it must be sunny for the event to occur.
        /// </summary>
        /// <returns></returns>
        public string precondition_sunnyWeather()
        {
            StringBuilder b = new StringBuilder();
            b.Append("w sunny");
            return b.ToString();
        }

        /// <summary>
        /// Creates the precondition that it must be rainy for the event to occur.
        /// </summary>
        /// <returns></returns>
        public string precondition_rainyWeather()
        {
            StringBuilder b = new StringBuilder();
            b.Append("w rainy");
            return b.ToString();
        }

        //Experimental weather checks. May or may not be used when checking for when to use an event.

    }
}
