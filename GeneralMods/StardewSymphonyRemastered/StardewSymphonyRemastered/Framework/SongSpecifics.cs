using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewSymphonyRemastered.Framework
{
    /// <summary>
    /// Stores information about what songs play when.
    /// 
    /// TODO: Festivals and events
    /// </summary>
    public class SongSpecifics
    {
        Dictionary<string, List<Song>> listOfSongsWithTriggers; //triggerName, <songs>

        Dictionary<string, List<Song>> eventSongs;

        Dictionary<string, List<Song>> festivalSongs;

        public List<Song> listOfSongsWithoutTriggers; 

        public static List<string> locations = new List<string>();
        public static List<string> festivals = new List<string>();
        public static List<string> events = new List<string>();

        string[] seasons;
        string[] weather;
        string[] daysOfWeek;
        string[] timesOfDay;
        public static char seperator = '_';

        /// <summary>
        /// Constructor.
        /// </summary>
        public SongSpecifics()
        {
            seasons = new string[]
            {
                "spring",
                "summer",
                "fall",
                "winter"
            };

            weather = new string[]
            {
                "sunny",
                "rainy",
                "debris",
                "lightning",
                "festival",
                "snow",
                "wedding"
            };
            daysOfWeek = new string[]
            {
                "sunday",
                "monday",
                "tuesday",
                "wednesday",
                "thursday",
                "friday",
                "saturday"
            };
            timesOfDay = new string[]
            {
                "day",
                "night"
            };

            listOfSongsWithTriggers = new Dictionary<string, List<Song>>();
            eventSongs = new Dictionary<string, List<Song>>();
            festivalSongs = new Dictionary<string, List<Song>>();

            this.listOfSongsWithoutTriggers = new List<Song>();

            this.addSongLists();

        }



        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //                         Static Methods                       //
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        #region

        /// <summary>
        /// TODO: Add functionality for events and festivals
        /// Sum up some conditionals to parse the correct string key to access the songs list.
        /// </summary>
        /// <returns></returns>
        public static string getCurrentConditionalString()
        {
            string key = "";
            if (Game1.eventUp == true)
            {
                //Get the event id an hijack it with some different music
                //String key="Event_EventName";
            }
            else if (Game1.isFestival())
            {
                //hijack the name of the festival and load some different songs
                // string s="Festival_FestivalName";
            }
            else
            {
                key = getLocationString() + seperator + getSeasonNameString() + seperator + getWeatherString() + seperator + getDayOfWeekString() + seperator + getTimeOfDayString();
            }
            return key;
        }



        /// <summary>
        /// Initialize the location lists with the names of all of the major locations in the game.
        /// </summary>
        public static void addLocations()
        {
            foreach (var v in Game1.locations)
            {
                locations.Add(v.name);
            }
        }

        /// <summary>
        /// TODO: Find a way to get all of the festivals in the game for this list. Perhapse have a way to check the season and day of the month and equivilate it to something.
        /// Initialize list of festivals for the game.
        /// </summary>
        public static void addFestivals()
        {
            //Do some logic for festivals here.
        }

        /// <summary>
        /// Add a specific new festival to the list
        /// </summary>
        public static void addFestival(string name)
        {
            festivals.Add(name);
        }

        /// <summary>
        /// TODO: Get a list of all of the vanilla events in the game. But how to determine what event is playing is the question.
        /// </summary>
        /// <param name="name"></param>
        public static void addEvents()
        {
            //Do some logic here
        }

        /// <summary>
        /// TODO: Custom way to add in event to hijack music.
        /// </summary>
        /// <param name="name"></param>
        public static void addEvent(string name)
        {
            //Do some logic here
        }

        /// <summary>
        /// Add a location to the loctaion list.
        /// </summary>
        /// <param name="name"></param>
        public static void addLocation(string name)
        {
            locations.Add(name);
        }

        /// <summary>
        /// Get the name of the day of the week from what game day it is.
        /// </summary>
        /// <returns></returns>
        public static string getDayOfWeekString()
        {
            int day = Game1.dayOfMonth;
            int dayOfWeek = day % 7;
            if (dayOfWeek == 0)
            {
                return "sunday";
            }
            if (dayOfWeek == 1)
            {
                return "monday";
            }
            if (dayOfWeek == 2)
            {
                return "tuesday";
            }
            if (dayOfWeek == 3)
            {
                return "wednesday";
            }
            if (dayOfWeek == 4)
            {
                return "thursday";
            }
            if (dayOfWeek == 5)
            {
                return "friday";
            }
            if (dayOfWeek == 6)
            {
                return "saturday";
            }
            return "";
        }

        /// <summary>
        /// Get the name of the current season
        /// </summary>
        /// <returns></returns>
        public static string getSeasonNameString()
        {
            return Game1.currentSeason.ToLower();
        }

        /// <summary>
        /// Get the name for the current weather outside.
        /// </summary>
        /// <returns></returns>
        public static string getWeatherString()
        {
            if (Game1.weatherIcon == Game1.weather_sunny) return "sunny";
            if (Game1.weatherIcon == Game1.weather_rain) return "rainy";
            if (Game1.weatherIcon == Game1.weather_debris) return "debris";
            if (Game1.weatherIcon == Game1.weather_lightning) return "lightning";
            if (Game1.weatherIcon == Game1.weather_festival) return "festival";
            if (Game1.weatherIcon == Game1.weather_snow) return "snow";
            if (Game1.weatherIcon == Game1.weather_wedding) return "wedding";
            return "";
        }

        /// <summary>
        /// Get the name for the time of day that it currently is.
        /// </summary>
        /// <returns></returns>
        public static string getTimeOfDayString()
        {
            if (Game1.timeOfDay < Game1.getModeratelyDarkTime()) return "day";
            else return "night";
        }

        /// <summary>
        /// Get the name of the location of where I am at.
        /// </summary>
        /// <returns></returns>
        public static string getLocationString()
        {
            return Game1.currentLocation.name;
        }
        #endregion

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //                         Non-Static Methods                   //
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        #region
        /// <summary>
        /// Adds the song's reference to a music pack.
        /// </summary>
        /// <param name="songName"></param>
        public void addSongToMusicPack(Song song)
        {
            this.listOfSongsWithoutTriggers.Add(song);
        }

        /// <summary>
        /// Checks if the song exists at all in this music pack.
        /// </summary>
        /// <param name="songName"></param>
        /// <returns></returns>
        public bool isSongInList(string songName)
        {
            Song s = getSongFromList(listOfSongsWithoutTriggers, songName);
            return listOfSongsWithoutTriggers.Contains(s);
        }

        /// <summary>
        /// A pretty big function to add in all of the specific songs that play at certain locations_seasons_weather_dayOfWeek_times. 
        /// </summary>
        public void addSongLists()
        {
            foreach (var loc in locations)
            {
                foreach (var season in seasons)
                {
                    listOfSongsWithTriggers.Add(loc + seperator + season, new List<Song>());
                    foreach(var Weather in weather)
                    {
                        listOfSongsWithTriggers.Add(loc + seperator + season + seperator + Weather, new List<Song>());
                        foreach(var day in daysOfWeek)
                        {
                            listOfSongsWithTriggers.Add(loc + seperator + season + seperator + Weather + seperator + day, new List<Song>());
                            foreach(var time in timesOfDay)
                            {
                                listOfSongsWithTriggers.Add(loc + seperator + season + seperator + Weather + seperator + day + seperator + time, new List<Song>());
                            }
                        }
                    }
                }
            }

            //Add in some default seasonal music because maybe a location doesn't have some music?
            foreach (var season in seasons)
            {
                listOfSongsWithTriggers.Add(season, new List<Song>());
                foreach (var Weather in weather)
                {
                    listOfSongsWithTriggers.Add( season + seperator + Weather, new List<Song>());
                    foreach (var day in daysOfWeek)
                    {
                        listOfSongsWithTriggers.Add(season + seperator + Weather + seperator + day, new List<Song>());
                        foreach (var time in timesOfDay)
                        {
                            listOfSongsWithTriggers.Add(season + seperator + Weather + seperator + day + seperator + time, new List<Song>());
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Used to access the master list of songs this music pack contains.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KeyValuePair<string,List<Song>>getSongList(string key)
        {
            string keyPhrase = key.Split(seperator).ElementAt(0);

            if (keyPhrase == "event")
            {
                
                foreach (KeyValuePair<string, List<Song>> pair in eventSongs)
                {
                    if (pair.Key == key) return pair;
                }
                
                //return new KeyValuePair<string, List<Song>>(key, eventSongs[key]);
            }
            else if (keyPhrase == "festival")
            {
                
                foreach (KeyValuePair<string, List<Song>> pair in festivalSongs)
                {
                    if (pair.Key == key) return pair;
                }
                
                //return new KeyValuePair<string, List<string>>(key, festivalSongs[key]);
            }
            else
            {
                
                foreach(KeyValuePair<string,List<Song>> pair in listOfSongsWithTriggers)
                {
                    if (pair.Key == key) return pair;
                }
                
                //return new KeyValuePair<string, List<string>>(key, listOfSongsWithTriggers[key]);
            }
            return new KeyValuePair<string, List<Song>>("",null);
        }

        /// <summary>
        /// Add a song name to a specific list of songs to play that will play under certain conditions.
        /// </summary>
        /// <param name="songListKey"></param>
        /// <param name="songName"></param>
        public void addSongToList(string songListKey,string songName)
        {
            var songKeyPair = getSongList(songListKey);
            var song = getSongFromList(songKeyPair.Value, songName);
            songKeyPair.Value.Add(song);
        }

        /// <summary>
        /// Remove a song name from a specific list of songs to play that will play under certain conditions.
        /// </summary>
        /// <param name="songListKey"></param>
        /// <param name="songName"></param>
        public void removeSongFromList(string songListKey,string songName)
        {
            var songKeyPair = getSongList(songListKey);
            var song = getSongFromList(songKeyPair.Value, songName);
            songKeyPair.Value.Remove(song);
        }

        /// <summary>
        /// Get the Song instance that is referenced with the song's name.
        /// </summary>
        /// <param name="songList"></param>
        /// <param name="songName"></param>
        /// <returns></returns>
        public Song getSongFromList(List<Song> songList,string songName)
        {
            foreach(var song in songList)
            {
                if (song.name == songName) return song;
            }
            return null;
        }
        #endregion
    }
}
