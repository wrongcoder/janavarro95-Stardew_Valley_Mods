using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace StardewSymphonyRemastered.Framework.V2
{
    public class SongConditionals
    {
        string season;
        string weather;
        string time;
        string location;
        string dayOfWeek;

        string eventKey;
        string festival;
        string menu;

        private readonly string[] seasons;
        private readonly string[] weathers;
        private readonly string[] daysOfWeek;
        private readonly string[] timesOfDay;
        public static char seperator = '_';

        public SongConditionals()
        {
            this.seasons = new[]
{
                "spring",
                "summer",
                "fall",
                "winter"
            };

            this.weathers = new[]
            {
                "sunny",
                "rain",
                "debris",
                "lightning",
                "snow",
                "festival",
                "wedding"
            };
            this.daysOfWeek = new[]
            {
                "sunday",
                "monday",
                "tuesday",
                "wednesday",
                "thursday",
                "friday",
                "saturday"
            };
            this.timesOfDay = new[]
            {
                "day",
                "night",
                "12A.M.",
                "1A.M.",
                "2A.M.",
                "3A.M.",
                "4A.M.",
                "5A.M.",
                "6A.M.",
                "7A.M.",
                "8A.M.",
                "9A.M.",
                "10A.M.",
                "11A.M.",
                "12P.M.",
                "1P.M.",
                "2P.M.",
                "3P.M.",
                "4P.M.",
                "5P.M.",
                "6P.M.",
                "7P.M.",
                "8P.M.",
                "9P.M.",
                "10P.M.",
                "11P.M.",
            };
        }

        public SongConditionals(string key) : this()
        {
            this.setConditionalsFromKey(key);
        }

        /// <summary>
        /// Parse a given key and figure out when a song can play.
        /// </summary>
        /// <param name="key"></param>
        public void setConditionalsFromKey(string key)
        {
            string[] splits = key.Split(seperator);
            foreach (string split in splits)
            {
                //Parse key into conditionals.
                if (this.seasons.Contains(split))
                {
                    this.season = split;
                }
                else if (this.weathers.Contains(split))
                {
                    this.weather = split;
                }
                else if (this.daysOfWeek.Contains(split))
                {
                    this.dayOfWeek = split;
                }
                else if (this.timesOfDay.Contains(split))
                {
                    this.time = split;
                }
                else if (SongSpecificsV2.menus.Contains(split))
                {
                    this.menu = split;
                }
                else if (SongSpecificsV2.locations.Contains(split))
                {
                    this.location = split;
                }
                else if (SongSpecificsV2.events.Contains(split))
                {
                    this.eventKey = split;
                }
                else if (SongSpecificsV2.festivals.Contains(split))
                {
                    this.festival = split;
                }
            }
        }

        /// <summary>
        /// Checks if a song can be played provided a given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool canBePlayed(string key)
        {

            string tempSeason = "";
            string tempWeather = "";
            string tempTime = "";
            string tempLocation = "";
            string tempDayOfWeek = "";
            string tempMenu = "";
            string tempFestival = "";
            string tempEvent = "";

            string[] splits = key.Split(seperator);
            foreach (string split in splits)
            {
                //Parse key into conditionals.
                if (this.seasons.Contains(split))
                {
                    tempSeason = split;
                }
                else if (this.weathers.Contains(split))
                {
                    tempWeather = split;
                }
                else if (this.daysOfWeek.Contains(split))
                {
                    tempDayOfWeek = split;
                }
                else if (this.timesOfDay.Contains(split))
                {
                    tempTime = split;
                }
                else if (SongSpecificsV2.menus.Contains(split))
                {
                    tempMenu = split;
                }
                else if (SongSpecificsV2.locations.Contains(split))
                {
                    tempLocation = split;
                }
                else if (SongSpecificsV2.events.Contains(split))
                {
                    tempEvent = split;
                }
                else if (SongSpecificsV2.festivals.Contains(split))
                {
                    tempFestival = split;
                }
            }


            if (this.isLocationSpecific())
            {
                if (!string.IsNullOrEmpty(tempLocation))
                {
                    if (this.dayOfWeek != tempDayOfWeek) return false; //If there is a check but not the right location return false
                }
                else
                {
                    return false; //If there is no check against this then return false;
                }
            }
            if (this.isTimeSpecific())
            {
                if (!string.IsNullOrEmpty(tempTime))
                {
                    if (this.time != tempTime) return false; //If the two times don't match return false
                }
                else
                {
                    return false; //If there is no check against this and this is time specific don't allow it.
                }
            }


            if (this.isDaySpecific())
            {
                //condition specific check
                if (!string.IsNullOrEmpty(tempDayOfWeek))
                {
                    if (this.dayOfWeek != tempDayOfWeek) return false;
                }
                else
                {
                    return false; //There is no check against this day of the week. Don't allow it.
                }
            }

            //Check for season.
            if (this.isSeasonSpecific())
            {
                if (!string.IsNullOrEmpty(tempSeason))
                {
                    if (this.season != tempSeason) return false;
                }
                else
                {
                    return false;
                }
            }

            //Check for weather.
            if (this.isWeatherSpecific())
            {
                if (!string.IsNullOrEmpty(tempWeather))
                {
                    if (this.weather != tempWeather) return false;
                }
                else
                {
                    return false;
                }
            }


            if (!string.IsNullOrEmpty(this.menu))
            {
                if (!string.IsNullOrEmpty(tempMenu))
                {
                    if (this.menu != tempMenu) return false;
                }
            }

            if (!string.IsNullOrEmpty(this.festival))
            {
                if (!string.IsNullOrEmpty(tempFestival))
                {
                    if (this.festival != tempFestival) return false;
                }
            }

            if (!string.IsNullOrEmpty(this.eventKey))
            {
                if (!string.IsNullOrEmpty(tempEvent))
                {
                    if (this.eventKey != tempEvent) return false;
                }
            }
            return true;
        }


        public bool isLocationSpecific()
        {
            return !string.IsNullOrEmpty(this.location);
        }

        public bool isTimeSpecific()
        {
            return !string.IsNullOrEmpty(this.time);
        }

        public bool isSeasonSpecific()
        {
            return !string.IsNullOrEmpty(this.season);
        }
        public bool isWeatherSpecific()
        {
            return !string.IsNullOrEmpty(this.weather);
        }
        public bool isDaySpecific()
        {
            return !string.IsNullOrEmpty(this.dayOfWeek);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(SongConditionals)) return false;
            SongConditionals other = (SongConditionals)obj;
            if (this.season != other.season) return false;
            if (this.weather != other.weather) return false;
            if (this.dayOfWeek != other.dayOfWeek) return false;
            if (this.location != other.location) return false;
            if (this.time != other.time) return false;
            if (this.menu != other.menu) return false;
            if (this.eventKey != other.eventKey) return false;
            if (this.festival != other.festival) return false;

            return true;
        }
    }
}
