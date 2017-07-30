using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using StardewValley;

namespace Omegasis.StardewSymphony
{
    //also known as the music_pack
    internal class MusicManager
    {
        /*********
        ** Properties
        *********/
        /// <summary>The directory path containing the music.</summary>
        public string Directory { get; }

        /// <summary>The name of the wavebank file.</summary>
        public string WavebankName { get; }

        /// <summary>The name of the soundbank file.</summary>
        public string SoundbankName { get; }

        /// <summary>The loaded wavebank (if any).</summary>
        public WaveBank Wavebank { get; }

        /// <summary>The loaded soundbank (if any).</summary>
        public SoundBank Soundbank { get; }

        /// <summary>Songs that play in spring.</summary>
        public List<Cue> SpringSongs { get; } = new List<Cue>();

        /// <summary>Songs that play in summer.</summary>
        public List<Cue> SummerSongs { get; } = new List<Cue>();

        /// <summary>Songs that play in fall.</summary>
        public List<Cue> FallSongs { get; } = new List<Cue>();

        /// <summary>Songs that play in winter.</summary>
        public List<Cue> WinterSongs { get; } = new List<Cue>();

        /// <summary>Songs that play on spring nights.</summary>
        public List<Cue> SpringNightSongs { get; } = new List<Cue>();

        /// <summary>Songs that play on summer nights.</summary>
        public List<Cue> SummerNightSongs { get; } = new List<Cue>();

        /// <summary>Songs that play on fall nights.</summary>
        public List<Cue> FallNightSongs { get; } = new List<Cue>();

        /// <summary>Songs that play on winter nights.</summary>
        public List<Cue> WinterNightSongs { get; } = new List<Cue>();

        /// <summary>Songs that play on rainy spring days.</summary>
        public List<Cue> SpringRainSongs { get; } = new List<Cue>();

        /// <summary>Songs that play on rainy summer days.</summary>
        public List<Cue> SummerRainSongs { get; } = new List<Cue>();

        /// <summary>Songs that play on rainy fall days.</summary>
        public List<Cue> FallRainSongs { get; } = new List<Cue>();

        /// <summary>Songs that play on rainy winter days.</summary>
        public List<Cue> WinterSnowSongs { get; } = new List<Cue>();

        /// <summary>Songs that play on rainy spring nights.</summary>
        public List<Cue> SpringRainNightSongs { get; } = new List<Cue>();

        /// <summary>Songs that play on rainy summer nights.</summary>
        public List<Cue> SummerRainNightSongs { get; } = new List<Cue>();

        /// <summary>Songs that play on rainy fall nights.</summary>
        public List<Cue> FallRainNightSongs { get; } = new List<Cue>();

        /// <summary>Songs that play on rainy winter nights.</summary>
        public List<Cue> WinterSnowNightSongs { get; } = new List<Cue>();

        /// <summary>Songs that play in specific locations.</summary>
        public Dictionary<string, List<Cue>> LocationSongs { get; } = new Dictionary<string, List<Cue>>();

        /// <summary>Songs that play in specific locations on rainy days.</summary>
        public Dictionary<string, List<Cue>> LocationRainSongs { get; } = new Dictionary<string, List<Cue>>();

        /// <summary>Songs that play in specific locations at night.</summary>
        public Dictionary<string, List<Cue>> LocationNightSongs { get; } = new Dictionary<string, List<Cue>>();

        /// <summary>Songs that play in specific locations on rainy nights.</summary>
        public Dictionary<string, List<Cue>> LocationRainNightSongs { get; } = new Dictionary<string, List<Cue>>();


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="wavebank">The name of the wavebank file.</param>
        /// <param name="soundbank">The name of the soundbank file.</param>
        /// <param name="directory">The directory path containing the music.</param>
        public MusicManager(string wavebank, string soundbank, string directory)
        {
            // init data
            this.Directory = directory;
            this.WavebankName = wavebank + ".xwb";
            this.SoundbankName = soundbank + ".xsb";

            // init banks
            string wavePath = Path.Combine(this.Directory, this.WavebankName);
            string soundPath = Path.Combine(this.Directory, this.SoundbankName);

            Console.WriteLine(wavePath);
            Console.WriteLine(soundPath);

            if (File.Exists(wavePath))
                this.Wavebank = new WaveBank(Game1.audioEngine, wavePath);
            if (File.Exists(Path.Combine(this.Directory, this.SoundbankName)))
                this.Soundbank = new SoundBank(Game1.audioEngine, soundPath);

            // update audio
            Game1.audioEngine.Update();
        }

        /// <summary>Read cue names from a text file and adds them to a specific list. Morphs with specific conditional name. Conditionals are hardcoded.</summary>
        /// <param name="conditionalName">The conditional file name to read.</param>
        /// <param name="cues">The music list to update.</param>
        public void Music_Loader_Seasons(string conditionalName, IDictionary<string, MusicManager> cues)
        {
            string path = Path.Combine(this.Directory, "Music_Files", "Seasons", conditionalName + ".txt");
            if (!File.Exists(path))
            {
                Console.WriteLine($"Stardew Symohony:The specified music file could not be found. That music file is {conditionalName} which should be located at {path} but don't worry I'll create it for you right now. It's going to be blank though");
                string[] text = new string[3];
                text[0] = conditionalName + " music file. This file holds all of the music that will play when there is no music for this game location, or simply put this is default music. Simply type the name of the song below the wall of equal signs.";
                text[1] = "========================================================================================";

                File.WriteAllLines(path, text);
            }
            else
            {
                Console.WriteLine($"Stardew Symphony:The music pack located at: {this.Directory} is processing the song info for the game location: {conditionalName}");

                string[] text = File.ReadAllLines(path);
                int i = 2;
                var lineCount = File.ReadLines(path).Count();

                while (i < lineCount) //the ordering seems bad, but it works.
                {
                    if (Convert.ToString(text[i]) == "")
                        break;
                    if (Convert.ToString(text[i]) == "\n")
                        break;

                    string cueName;
                    if (conditionalName == "spring")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.SpringSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.SpringSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    if (conditionalName == "summer")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.SummerSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.SummerSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    if (conditionalName == "fall")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.FallSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.FallSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    if (conditionalName == "winter")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.WinterSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);

                        }
                        else
                            this.WinterSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    //add in other stuff here
                    //========================================================================================================================================================================================
                    //NIGHTLY SEASONAL LOADERS
                    if (conditionalName == "spring_night")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.SpringNightSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.SpringNightSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    if (conditionalName == "summer_night")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.SummerNightSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.SummerNightSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    if (conditionalName == "fall_night")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.FallNightSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.FallNightSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    if (conditionalName == "winter_night")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.WinterNightSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.WinterNightSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    ////////NOW I"M ADDING THE PART THAT WILL READ IN RAINY SEASONAL SONGS FOR DAY AND NIGHT
                    if (conditionalName == "spring_rain")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.SpringRainSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.SpringRainSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    if (conditionalName == "summer_rain")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.SummerRainSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.SummerRainSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    if (conditionalName == "fall_rain")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.FallRainSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.FallRainSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    if (conditionalName == "winter_snow")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.WinterSnowSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.WinterSnowSongs.Add(this.Soundbank.GetCue(cueName));
                    }

                    //add in other stuff here
                    //========================================================================================================================================================================================
                    //NIGHTLY SEASONAL RAIN LOADERS
                    if (conditionalName == "spring_rain_night")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.SpringRainNightSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);

                        }
                        else
                            this.SpringRainNightSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    if (conditionalName == "summer_rain_night")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.SummerRainNightSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.SummerRainNightSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    if (conditionalName == "fall_rain_night")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.FallRainNightSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.FallRainNightSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                    if (conditionalName == "winter_snow_night")
                    {
                        cueName = Convert.ToString(text[i]);
                        i++;

                        if (!cues.Keys.Contains(cueName))
                        {
                            this.WinterSnowNightSongs.Add(this.Soundbank.GetCue(cueName));
                            cues.Add(cueName, this);
                        }
                        else
                            this.WinterSnowNightSongs.Add(this.Soundbank.GetCue(cueName));
                    }
                }
                if (i == 2)
                {
                    //  Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 +" this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");
                    // System.Threading.Thread.Sleep(10);
                    return;
                }
                Console.WriteLine("StardewSymohony:The music pack located at: " + this.Directory + " has successfully processed the song info for the game location: " + conditionalName);
            }
        }

        /// <summary>Read cue names from a text file and adds them to a specific list. Morphs with specific conditional name. Conditionals are hardcoded.</summary>
        /// <param name="conditionalName">The conditional file name to read.</param>
        /// <param name="cues">The music list to update.</param>
        public void Music_Loader_Locations(string conditionalName, IDictionary<string, MusicManager> cues)
        {
            List<Cue> locationSongs = new List<Cue>();
            //loads the data to the variables upon loading the game.
            var musicPath = this.Directory;
            string mylocation = Path.Combine(musicPath, "Music_Files", "Locations", conditionalName);
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //check to make sure the file actually exists
            {
                Console.WriteLine("StardewSymohony:A music list for the location " + conditionalName + " does not exist for the music pack located at " + mylocation3 + " which isn't a problem, I just thought I'd let you know since this may have been intentional. Also I'm creating it for you just incase. Cheers.");

                //Console.WriteLine("Creating the Config file");
                string[] mystring3 = new string[3];//seems legit.
                mystring3[0] = conditionalName + " music file. This file holds all of the music that will play when at this game location. Simply type the name of the song below the wall of equal signs.";
                mystring3[1] = "========================================================================================";

                File.WriteAllLines(mylocation3, mystring3);
            }
            else
            {
                Console.WriteLine("Stardew Symphony:The music pack located at: " + this.Directory + " is processing the song info for the game location: " + conditionalName);
                string[] readtext = File.ReadAllLines(mylocation3);
                int i = 2;
                var lineCount = File.ReadLines(mylocation3).Count();
                while (i < lineCount) //the ordering seems bad, but it works.
                {
                    if (Convert.ToString(readtext[i]) == "")
                        break;
                    if (Convert.ToString(readtext[i]) == "\n")
                        break;
                    string cueName = Convert.ToString(readtext[i]);
                    i++;
                    if (!cues.Keys.Contains(cueName))
                    {
                        locationSongs.Add(this.Soundbank.GetCue(cueName));
                        cues.Add(cueName, this);
                    }
                    else
                        locationSongs.Add(this.Soundbank.GetCue(cueName));
                }
                if (i == 2)
                {
                    //  Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 + " this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");
                    return;
                }
                if (locationSongs.Count > 0)
                {
                    this.LocationSongs.Add(conditionalName, locationSongs);
                    Console.WriteLine("StardewSymhony:The music pack located at: " + this.Directory + " has successfully processed the song info for the game location: " + conditionalName);
                }
            }
        }

        /// <summary>Read cue names from a text file and adds them to a specific list. Morphs with specific conditional name. Conditionals are hardcoded.</summary>
        /// <param name="conditionalName">The conditional file name to read.</param>
        /// <param name="cues">The music list to update.</param>
        public void Music_Loader_Locations_Rain(string conditionalName, IDictionary<string, MusicManager> cues)
        {
            List<Cue> locationSongs = new List<Cue>();
            var musicPath = this.Directory;
            string mylocation = Path.Combine(musicPath, "Music_Files", "Locations", conditionalName);
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //check to make sure the file actually exists
            {
                Console.WriteLine("StardewSymphony:A music list for the location " + conditionalName + " does not exist for the music pack located at " + mylocation3 + " which isn't a problem, I just thought I'd let you know since this may have been intentional. Also I'm creating it for you just incase. Cheers.");
                string[] mystring3 = new string[3];//seems legit.
                mystring3[0] = conditionalName + " music file. This file holds all of the music that will play when at this game location. Simply type the name of the song below the wall of equal signs.";
                mystring3[1] = "========================================================================================";
                File.WriteAllLines(mylocation3, mystring3);
            }
            else
            {
                // add in data here
                string[] readtext = File.ReadAllLines(mylocation3);
                int i = 2;
                var lineCount = File.ReadLines(mylocation3).Count();
                while (i < lineCount) //the ordering seems bad, but it works.
                {
                    if (Convert.ToString(readtext[i]) == "")
                        break;
                    if (Convert.ToString(readtext[i]) == "\n")
                        break;
                    string cueName = Convert.ToString(readtext[i]);
                    i++;
                    if (!cues.Keys.Contains(cueName))
                    {
                        locationSongs.Add(this.Soundbank.GetCue(cueName));
                        cues.Add(cueName, this);
                    }
                    else
                        locationSongs.Add(this.Soundbank.GetCue(cueName));
                }
                if (i == 2)
                {
                    // Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 + " this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");
                    return;
                }
                if (locationSongs.Count > 0)
                {
                    this.LocationRainSongs.Add(conditionalName, locationSongs);
                    Console.WriteLine("StardewSymohony:The music pack located at: " + this.Directory + " has successfully processed the song info for the game location: " + conditionalName);
                }
            }
        }

        /// <summary>Read cue names from a text file and adds them to a specific list. Morphs with specific conditional name. Conditionals are hardcoded.</summary>
        /// <param name="conditionalName">The conditional file name to read.</param>
        /// <param name="cues">The music list to update.</param>
        public void Music_Loader_Locations_Night(string conditionalName, IDictionary<string, MusicManager> cues)
        {
            List<Cue> locationSongs = new List<Cue>();
            //loads the data to the variables upon loading the game.
            var musicPath = this.Directory;
            string mylocation = Path.Combine(musicPath, "Music_Files", "Locations", conditionalName);
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //check to make sure the file actually exists
            {
                Console.WriteLine("StardewSymphony:A music list for the location " + conditionalName + " does not exist for the music pack located at " + mylocation3 + " which isn't a problem, I just thought I'd let you know since this may have been intentional. Also I'm creating it for you just incase. Cheers.");
                string[] mystring3 = new string[3];//seems legit.
                mystring3[0] = conditionalName + " music file. This file holds all of the music that will play when at this game location. Simply type the name of the song below the wall of equal signs.";
                mystring3[1] = "========================================================================================";
                File.WriteAllLines(mylocation3, mystring3);
            }
            else
            {
                // add in data here
                string[] readtext = File.ReadAllLines(mylocation3);
                int i = 2;
                var lineCount = File.ReadLines(mylocation3).Count();

                while (i < lineCount) //the ordering seems bad, but it works.
                {
                    if (Convert.ToString(readtext[i]) == "")
                        break;
                    if (Convert.ToString(readtext[i]) == "\n")
                        break;
                    string cueName = Convert.ToString(readtext[i]);
                    i++;
                    if (!cues.Keys.Contains(cueName))
                    {
                        locationSongs.Add(this.Soundbank.GetCue(cueName));
                        cues.Add(cueName, this);
                    }
                    else
                        locationSongs.Add(this.Soundbank.GetCue(cueName));
                }
                if (i == 2)
                {
                    //  Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 + " this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");
                    return;
                }
                if (locationSongs.Count > 0)
                {
                    this.LocationNightSongs.Add(conditionalName, locationSongs);
                    Console.WriteLine("StardewSymphonyLThe music pack located at: " + this.Directory + " has successfully processed the song info for the game location: " + conditionalName);
                }
            }
        }

        /// <summary>Read cue names from a text file and adds them to a specific list. Morphs with specific conditional name. Conditionals are hardcoded.</summary>
        /// <param name="conditionalName">The conditional file name to read.</param>
        /// <param name="cues">The music list to update.</param>
        public void Music_Loader_Locations_Rain_Night(string conditionalName, IDictionary<string, MusicManager> cues)
        {
            List<Cue> locationSongs = new List<Cue>();
            var musicPath = this.Directory;

            string path = Path.Combine(musicPath, "Music_Files", "Locations", conditionalName + ".txt");
            if (!File.Exists(path)) //check to make sure the file actually exists
            {
                Console.WriteLine("StardewSymphony:A music list for the location " + conditionalName + " does not exist for the music pack located at " + path + " which isn't a problem, I just thought I'd let you know since this may have been intentional. Also I'm creating it for you just incase. Cheers.");
                string[] text = new string[3];//seems legit.
                text[0] = conditionalName + " music file. This file holds all of the music that will play when at this game location. Simply type the name of the song below the wall of equal signs.";
                text[1] = "========================================================================================";
                File.WriteAllLines(path, text);
            }
            else
            {
                //load in music stuff from the text files using the code below.
                string[] text = File.ReadAllLines(path);
                int i = 2;
                var lineCount = File.ReadLines(path).Count();

                while (i < lineCount) //the ordering seems bad, but it works.
                {
                    if (Convert.ToString(text[i]) == "") //if there is ever an empty line, stop processing the music file
                        break;
                    if (Convert.ToString(text[i]) == "\n")
                        break;
                    string cueName = Convert.ToString(text[i]);
                    i++;
                    if (!cues.Keys.Contains(cueName))
                    {
                        locationSongs.Add(this.Soundbank.GetCue(cueName));
                        cues.Add(cueName, this);
                    }
                    else
                        locationSongs.Add(this.Soundbank.GetCue(cueName));
                }
                if (i == 2)
                {
                    //  Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 + " this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");
                    return;
                }

                if (locationSongs.Count > 0)
                {
                    this.LocationRainNightSongs.Add(conditionalName, locationSongs);
                    Console.WriteLine("StardewSymohony:The music pack located at: " + this.Directory + " has successfully processed the song info for the game location: " + conditionalName);
                }
            }
        }
    }
}
