using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Microsoft.Xna.Framework.Audio;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Omegasis.StardewSymphony
{
    /*
    TODO:
    0. Add in event handling so that I don't mute a heart event or wedding music.
    6. add in Stardew songs again to music selection
    7. add in more tracks.
    11. Tutorial for adding more music into the game?
    15. add in blank templates for users to make their own wave/sound banks
    */
    /// <summary>The mod entry point.</summary>
    public class StardewSymphony : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>All of the music/soundbanks and their locations.</summary>
        private IList<MusicManager> MasterList = new List<MusicManager>();

        /// <summary>All of the cue names that I ever add in.</summary>
        private IDictionary<string, MusicManager> SongWaveReference;

        /// <summary>The game locations.</summary>
        private IList<GameLocation> GameLocations;

        /// <summary>The number generator for randomisation.</summary>
        private Random Random;

        /// <summary>The game's original soundbank.</summary>
        private SoundBank DefaultSoundbank;

        /// <summary>The game's original wavebank.</summary>
        private WaveBank DefaultWavebank;

        private MusicHexProcessor HexProcessor;

        /****
        ** Context
        ****/
        /// <summary>Whether the player loaded a save.</summary>
        private bool IsGameLoaded;

        /// <summary>Whether no music pack was loaded for the current location.</summary>
        private bool HasNoMusic;

        /// <summary>The song that's currently playing.</summary>
        private Cue CurrentSong;

        /// <summary>The current sound info.</summary>
        private MusicManager CurrentSoundInfo;

        /// <summary>A timer used to create random pauses between songs.</summary>
        private Timer SongDelayTimer = new Timer();

        /****
        ** Config
        ****/
        /// <summary>The minimum delay (in milliseconds) to pass before playing the next song, or 0 for no delay.</summary>
        private int MinSongDelay;

        /// <summary>The maximum delay (in milliseconds) to pass before playing the next song, or 0 for no delay.</summary>
        private int MaxSongDelay;

        /// <summary>Whether to disable ambient rain audio when music is playing. If false, plays ambient rain audio alongside whatever songs are set in rain music.</summary>
        private bool SilentRain;

        /// <summary>Whether to play seasonal music from the music packs, instead of defaulting to the Stardew Valley Soundtrack.</summary>
        private bool PlaySeasonalMusic;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.HexProcessor = new MusicHexProcessor(this.MasterList, this.Reset);

            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            TimeEvents.DayOfMonthChanged += this.TimeEvents_DayOfMonthChanged;
            GameEvents.UpdateTick += this.GameEvents_UpdateTick;
            LocationEvents.CurrentLocationChanged += this.LocationEvents_CurrentLocationChanged;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked when the game updates (roughly 60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (!this.IsGameLoaded || !this.MasterList.Any())
                return; //basically if absolutly no music is loaded into the game for locations/festivals/seasons, don't override the game's default music player.

            if (this.CurrentSong == null)
            {
                this.HasNoMusic = true;
                return; //if there wasn't any music at loaded at all for the area, just play the default stardew soundtrack.
            }
            if (this.HasNoMusic && !this.CurrentSong.IsPlaying)
                this.CurrentSong = null; //if there was no music loaded for the area and the last song has finished playing, default to the Stardew Soundtrack.

            if (this.CurrentSong != null)
            {
                this.HasNoMusic = false;
                if (!this.CurrentSong.IsPlaying && !this.SongDelayTimer.Enabled)
                    this.StartMusicDelay();
            }

            if (Game1.isFestival())
                return; // replace with festival
            if (Game1.eventUp)
                return; // replace with event music
            if (Game1.isRaining && !this.SilentRain)
                return; // play the rain ambience soundtrack

            Game1.currentSong.Stop(AudioStopOptions.Immediate); //stop the normal songs from playing over the new songs
            Game1.nextMusicTrack = "";  //same as above line
        }

        /// <summary>The method invoked when <see cref="Game1.dayOfMonth"/> changes.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void TimeEvents_DayOfMonthChanged(object sender, EventArgsIntChanged e)
        {
            if (!this.IsGameLoaded)
                return;
            this.StopSound(); //if my music player is called and I forget to clean up sound before hand, kill the old sound.
            this.LoadConfig();
            this.WriteConfig();

            this.SelectMusic();
        }

        /// <summary>The method invoked after the player loads a save.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            // init config
            this.LoadConfig();
            this.WriteConfig();

            // init context
            this.Random = new Random();
            this.MasterList = new List<MusicManager>();
            this.SongWaveReference = new Dictionary<string, MusicManager>();
            this.GameLocations = Game1.locations;
            this.HasNoMusic = true;

            // keep a copy of the original banks
            this.DefaultSoundbank = Game1.soundBank;
            this.DefaultWavebank = Game1.waveBank;

            // load music packs
            {
                string musicPacksPath = Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "Music_Packs")).FullName;
                var musicPacks = new Dictionary<string, string>();
                ProcessDirectory(musicPacksPath, musicPacks);
                this.SongDelayTimer.Enabled = false;
                foreach (var pack in musicPacks)
                    this.LoadMusicInfo(pack.Key, pack.Value);
            }

            // init sound
            this.HexProcessor.ProcessHex();
            this.IsGameLoaded = true;
            this.SelectMusic();
        }

        /// <summary>The method invoked after the player warps to a new area.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void LocationEvents_CurrentLocationChanged(object sender, EventArgsCurrentLocationChanged e)
        {
            if (!this.IsGameLoaded)
                return;

            this.SelectMusic();
        }

        /// <summary>Create a random delay and then choose the next song.</summary>
        private void StartMusicDelay()
        {
            // reset timer
            this.SongDelayTimer.Dispose();
            this.SongDelayTimer = new Timer(this.Random.Next(this.MinSongDelay, this.MaxSongDelay));

            // start timer
            this.SongDelayTimer.Elapsed += (sender, args) =>
            {
                this.SelectMusic();
                this.SongDelayTimer.Enabled = false;
            };
            this.SongDelayTimer.Start();
        }

        /// <summary>Reads cue names from a text file and adds them to a specific list. Morphs with specific conditional name.</summary>
        /// <param name="rootDir">The root directory for music files.</param>
        /// <param name="configPath">The full path to the config file to read.</param>
        private void LoadMusicInfo(string rootDir, string configPath)
        {
            // make sure file exists
            if (!File.Exists(configPath))
            {
                Console.WriteLine("StardewSymphony:This music pack lacks a Config.txt. Without one, I can't load in the music.");
                return;
            }

            // parse config file
            string[] text = File.ReadAllLines(configPath);
            string wave = Convert.ToString(text[3]);
            string sound = Convert.ToString(text[5]);

            // load all of the info files here. This is some deep magic I worked at 4 AM. I almost forgot how the heck this worked when I woke up.
            MusicManager manager = new MusicManager(wave, sound, rootDir);
            manager.Music_Loader_Seasons("spring", this.SongWaveReference);
            manager.Music_Loader_Seasons("summer", this.SongWaveReference);
            manager.Music_Loader_Seasons("fall", this.SongWaveReference);
            manager.Music_Loader_Seasons("winter", this.SongWaveReference);
            manager.Music_Loader_Seasons("spring_night", this.SongWaveReference);
            manager.Music_Loader_Seasons("summer_night", this.SongWaveReference);
            manager.Music_Loader_Seasons("fall_night", this.SongWaveReference);
            manager.Music_Loader_Seasons("winter_night", this.SongWaveReference);
            manager.Music_Loader_Seasons("spring_rain", this.SongWaveReference);
            manager.Music_Loader_Seasons("summer_rain", this.SongWaveReference);
            manager.Music_Loader_Seasons("fall_rain", this.SongWaveReference);
            manager.Music_Loader_Seasons("winter_snow", this.SongWaveReference);
            manager.Music_Loader_Seasons("spring_rain_night", this.SongWaveReference);
            manager.Music_Loader_Seasons("summer_rain_night", this.SongWaveReference);
            manager.Music_Loader_Seasons("fall_rain_night", this.SongWaveReference);
            manager.Music_Loader_Seasons("winter_snow_night", this.SongWaveReference);

            // load location music
            foreach (GameLocation location in this.GameLocations)
            {
                manager.Music_Loader_Locations(location.name, this.SongWaveReference);
                manager.Music_Loader_Locations_Night(location.name + "_night", this.SongWaveReference);
                manager.Music_Loader_Locations_Rain(location.name + "_rain", this.SongWaveReference);
                manager.Music_Loader_Locations_Rain_Night(location.name + "_rain_night", this.SongWaveReference);
            }

            // add everything to master song list
            this.MasterList.Add(manager);
        }

        /// <summary>Recursively load music packs from the given directory.</summary>
        /// <param name="dirPath">The directory path to search for music packs.</param>
        /// <param name="musicPacks">The dictionary to update with music packs.</param>
        private void ProcessDirectory(string dirPath, IDictionary<string, string> musicPacks)
        {
            // load music files
            foreach (string filePath in Directory.GetFiles(dirPath))
            {
                string extension = Path.GetExtension(filePath);
                if (extension == ".xsb")
                {
                    Log.AsyncG(filePath);
                    this.HexProcessor.AddSoundBank(filePath);
                }
                //if (extension == "xwb")
                //{
                //    Log.AsyncC(path);
                //    MusicHexProcessor.allWaveBanks.Add(path);
                //}
            }

            // read config file
            if (File.Exists(Path.Combine(dirPath, "Config.txt")))
            {
                string temp = Path.Combine(dirPath, "Config.txt");
                musicPacks.Add(dirPath, temp);
            }

            // check subdirectories
            foreach (string childDir in Directory.GetDirectories(dirPath))
                this.ProcessDirectory(childDir, musicPacks);
        }

        /// <summary>Select music for the current location.</summary>
        private void SelectMusic()
        {
            if (!this.IsGameLoaded)
                return;

            //  no_music = false;
            //if at any time the music for an area can't be played for some unknown reason, the game should default to playing the Stardew Valley Soundtrack.
            bool isRaining = Game1.isRaining;

            if (Game1.player.currentLocation is Farm)
            {
                farm_music_selector();
                return;
            }
            if (Game1.isFestival())
            {
                this.StopSound();
                return; //replace with festival music if I decide to support it.
            }
            if (Game1.eventUp)
            {
                this.StopSound();
                return; //replace with event music if I decide to support it/people request it.
            }


            bool isNight = (Game1.timeOfDay < 600 || Game1.timeOfDay > Game1.getModeratelyDarkTime());
            if (isRaining)
            {
                if (isNight)
                {
                    music_player_rain_night(); //some really awful heirarchy type thing I made up to help ensure that music plays all the time
                    if (this.HasNoMusic)
                    {
                        music_player_rain();
                        if (this.HasNoMusic)
                        {
                            music_player_night();
                            if (this.HasNoMusic)
                                music_player_location();
                        }
                    }
                }
                else
                {
                    music_player_rain();
                    if (this.HasNoMusic)
                    {
                        music_player_night();
                        if (this.HasNoMusic)
                            music_player_location();
                    }
                }
            }
            else
            {
                if (isNight)
                {
                    music_player_night();
                    if (this.HasNoMusic) //if there is no music playing right now play some music.
                        music_player_location();
                }
                else
                    music_player_location();
            }

            if (this.HasNoMusic) //if there is valid music playing
            {
                if (!this.PlaySeasonalMusic)
                    return;

                if (this.CurrentSong != null && this.CurrentSong.IsPlaying)
                    return;

                this.Monitor.Log("Loading Default Seasonal Music");

                if (!this.MasterList.Any())
                {
                    Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                    this.Reset();
                    return;
                }

                //add in seasonal stuff here
                if (this.HasNoMusic)
                {
                    if (Game1.IsSpring)
                    {
                        if (isRaining)
                            spring_rain_songs();
                        else
                            spring_songs();
                    }
                    else if (Game1.IsSummer)
                    {
                        if (isRaining)
                            summer_rain_songs();
                        else
                            summer_songs();
                    }
                    else if (Game1.IsFall)
                    {
                        if (isRaining)
                            fall_rain_songs();
                        else
                            fall_songs();
                    }
                    else if (Game1.IsWinter)
                    {
                        if (Game1.isSnowing)
                            winter_snow_songs();
                        else
                            winter_songs();
                    }
                }
            }
        }


        public void farm_music_selector()
        {
            if (!this.IsGameLoaded)
                return;

            //  no_music = false;
            //if at any time the music for an area can't be played for some unknown reason, the game should default to playing the Stardew Valley Soundtrack.
            bool night_time = false;
            bool rainy = Game1.isRaining;

            Monitor.Log("Loading farm music.");
            if (Game1.isFestival())
            {
                this.StopSound();
                return; //replace with festival music if I decide to support it.
            }
            if (Game1.eventUp)
            {
                this.StopSound();
                return; //replace with event music if I decide to support it/people request it.
            }


            if (Game1.timeOfDay < 600 || Game1.timeOfDay > Game1.getModeratelyDarkTime())
            {
                night_time = true;
            }
            else
            {
                night_time = false;
            }

            Monitor.Log("Loading Default Seasonal Music");

            if (!this.MasterList.Any())
            {
                Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                this.Reset();
                return;

            }

            //add in seasonal stuff here
            if (Game1.IsSpring)
            {
                if (rainy)
                {
                    spring_rain_songs();
                }
                else
                {
                    spring_songs();
                }
            }
            if (Game1.IsSummer)
            {
                if (rainy)
                {
                    summer_rain_songs();
                }
                else
                {
                    summer_songs();
                }
            }
            if (Game1.IsFall)
            {
                if (rainy)
                {
                    fall_rain_songs();
                }
                else
                {
                    fall_songs();
                }
            }
            if (Game1.IsWinter)
            {
                if (Game1.isSnowing)
                {
                    winter_snow_songs();
                }
                else
                {
                    winter_songs();
                }
            }
            //end seasonal songs
            if (this.CurrentSong != null)
            {
                if (this.CurrentSong.IsPlaying)
                {
                    return;
                }
            }
            //start locational songs
            if (rainy && night_time)
            {
                music_player_rain_night(); //some really awful heirarchy type thing I made up to help ensure that music plays all the time
                if (this.HasNoMusic)
                {
                    music_player_rain();
                    if (this.HasNoMusic)
                    {
                        music_player_night();
                        if (this.HasNoMusic)
                        {
                            music_player_location();

                        }
                    }
                }

            }
            if (rainy && night_time == false)
            {
                music_player_rain();
                if (this.HasNoMusic)
                {
                    music_player_night();
                    if (this.HasNoMusic)
                    {
                        music_player_location();

                    }
                }

            }
            if (rainy == false && night_time)
            {
                music_player_night();
                if (this.HasNoMusic)
                {
                    music_player_location();

                }

            }
            if (rainy == false && night_time == false)
            {
                music_player_location();
            }

            //end of function. Natural return;
            return;
        }

        public void music_player_location()
        {
            if (!this.IsGameLoaded)
            {
                this.StartMusicDelay();
                return;
            }
            this.Random.Next();
            int randomNumber = this.Random.Next(0, this.MasterList.Count); //random number between 0 and n. 0 not included

            if (!this.MasterList.Any())
            {
                Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                this.Reset();
                return;

            }

            this.CurrentSoundInfo = this.MasterList.ElementAt(randomNumber); //grab a random wave bank/song bank/music pack/ from all available music packs.
            if (Game1.player.currentLocation != null)
            {
                int helper1 = 0;
                int master_helper = 0;
                bool found = false;

                //this mess of a while loop iterates across all of my music packs looking for a valid music pack to play music from.
                while (true)
                {
                    if (this.CurrentSoundInfo.LocationSongs.Keys.Contains(Game1.player.currentLocation.name))
                    {

                        foreach (var entry in this.CurrentSoundInfo.LocationSongs)
                        {
                            if (entry.Key == Game1.player.currentLocation.name)
                            {
                                if (entry.Value.Count > 0)
                                {
                                    //Monitor.Log("FOUND THE RIGHT POSITIONING OF THE CLASS");
                                    found = true;
                                    break;
                                }
                                else
                                {
                                    //this section tells me if it is valid and is less than or equal to 0
                                    //Monitor.Log("Count is less than for this class zero. Switching music packs");
                                    found = false;
                                    master_helper++; //iterate across the classes
                                    break;
                                }

                            }
                            else
                            {//this section iterates through the keys
                                Monitor.Log("Not there");
                                found = false;
                                helper1++;
                                continue;
                            }

                        } //itterate through all of the valid locations that were stored in this class

                    }
                    else
                    {
                        Monitor.Log("No data could be loaded on this area. Swaping music packs");
                        found = false;
                    }
                    if (found == false) //if I didnt find the music.
                    {
                        master_helper++;

                        if (master_helper > this.MasterList.Count)
                        {
                            Monitor.Log("I've gone though every music pack with no success. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                            this.HasNoMusic = true;
                            return;

                        }
                        int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                        this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.
                        continue;
                    }
                    else
                    {
                        break;
                    }

                }

                List<Cue> cues = this.CurrentSoundInfo.LocationSongs.Values.ElementAt(helper1); //set a list of songs to a "random" list of songs from a music pack
                int pointer = 0;
                while (!cues.Any()) //yet another circular array
                {
                    pointer++;
                    int motzy = (pointer + randomNumber) % this.MasterList.Count; //why do I name my variables pointless names?

                    this.CurrentSoundInfo = this.MasterList.ElementAt(motzy);
                    if (pointer > this.MasterList.Count)
                    {
                        Monitor.Log("No music packs have any valid music for this area. AKA all music packs are empty;");
                        this.HasNoMusic = true;
                        return;
                    }

                }

                Monitor.Log("loading music for this area");
                this.StopSound();
                int random3 = this.Random.Next(0, cues.Count);
                Game1.soundBank = this.CurrentSoundInfo.Soundbank; //change the game's soundbank temporarily
                Game1.waveBank = this.CurrentSoundInfo.Wavebank;//dito but wave bank

                this.CurrentSong = cues.ElementAt(random3); //grab a random song from the winter song list
                this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                if (this.CurrentSong != null)
                {
                    Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + "for the location " + Game1.player.currentLocation);
                    this.HasNoMusic = false;
                    this.CurrentSong.Play(); //play some music
                    this.Reset();
                    return;
                }
            }
            else
            {
                Monitor.Log("Location is null");
                this.HasNoMusic = true;
            }
        }//end music player
        public void music_player_rain()
        {
            if (!this.IsGameLoaded)
            {
                this.StartMusicDelay();
                return;
            }
            this.Random.Next();
            int randomNumber = this.Random.Next(0, this.MasterList.Count); //random number between 0 and n. 0 not included

            if (!this.MasterList.Any())
            {
                Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                this.Reset();
                return;

            }

            this.CurrentSoundInfo = this.MasterList.ElementAt(randomNumber); //grab a random wave bank/song bank/music pack/ from all available music packs.            

            if (Game1.player.currentLocation != null)
            {
                int helper1 = 0;
                int master_helper = 0;
                bool found = false;

                while (true)
                {
                    if (this.CurrentSoundInfo.LocationRainSongs.Keys.Contains(Game1.player.currentLocation.name + "_rain"))
                    {

                        foreach (var entry in this.CurrentSoundInfo.LocationRainSongs)
                        {
                            if (entry.Key == Game1.player.currentLocation.name + "_rain")
                            {
                                if (entry.Value.Count > 0)
                                {
                                    //Monitor.Log("FOUND THE RIGHT POSITIONING OF THE CLASS");
                                    found = true;
                                    break;
                                }
                                else
                                {
                                    //this section tells me if it is valid and is less than or equal to 0
                                    //Monitor.Log("Count is less than for this class zero. Switching music packs");
                                    found = false;
                                    master_helper++; //iterate across the classes
                                    break;
                                }

                            }
                            else
                            {//this section iterates through the keys
                                Monitor.Log("Not there");
                                found = false;
                                helper1++;
                                continue;
                            }

                        } //itterate through all of the svalid locations that were stored in this class

                    }
                    else
                    {
                        Monitor.Log("No data could be loaded on this area. Swaping music packs");
                        found = false;
                    }
                    if (found == false) //if I didnt find the music.
                    {
                        master_helper++;

                        if (master_helper > this.MasterList.Count)
                        {
                            Monitor.Log("I've gone though every music pack with no success. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                            this.HasNoMusic = true;
                            return;

                        }
                        int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                        this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                List<Cue> cues = this.CurrentSoundInfo.LocationRainSongs.Values.ElementAt(helper1);


                int pointer = 0;
                while (!cues.Any())
                {
                    pointer++;
                    int motzy = (pointer + randomNumber) % this.MasterList.Count;

                    this.CurrentSoundInfo = this.MasterList.ElementAt(motzy);
                    if (pointer > this.MasterList.Count)
                    {
                        Monitor.Log("No music packs have any valid music for this area. AKA all music packs are empty;");
                        this.HasNoMusic = true;
                        return;
                    }

                }



                Monitor.Log("loading music for this area");
                this.StopSound();
                int random3 = this.Random.Next(0, cues.Count);
                Game1.soundBank = this.CurrentSoundInfo.Soundbank;
                Game1.waveBank = this.CurrentSoundInfo.Wavebank;

                this.CurrentSong = cues.ElementAt(random3); //grab a random song from the winter song list
                this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                if (this.CurrentSong != null)
                {
                    this.HasNoMusic = false;
                    Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + "for the location " + Game1.player.currentLocation + " while it is raining");
                    this.CurrentSong.Play();
                    this.Reset();
                    return;
                }



            }
            else
            {
                Monitor.Log("Location is null");
            }
        }//end music player
        public void music_player_night()
        {
            if (!this.IsGameLoaded)
            {
                this.StartMusicDelay();
                return;
            }
            this.Random.Next();
            int randomNumber = this.Random.Next(0, this.MasterList.Count); //random number between 0 and n. 0 not included

            if (!this.MasterList.Any())
            {
                Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                this.Reset();
                return;

            }

            this.CurrentSoundInfo = this.MasterList.ElementAt(randomNumber); //grab a random wave bank/song bank/music pack/ from all available music packs.            

            if (Game1.player.currentLocation != null)
            {
                int helper1 = 0;
                int master_helper = 0;
                bool found = false;

                while (true)
                {
                    if (this.CurrentSoundInfo.LocationNightSongs.Keys.Contains(Game1.player.currentLocation.name + "_night"))
                    {

                        foreach (var entry in this.CurrentSoundInfo.LocationNightSongs)
                        {
                            if (entry.Key == Game1.player.currentLocation.name + "_night")
                            {
                                if (entry.Value.Count > 0)
                                {
                                    //Monitor.Log("FOUND THE RIGHT POSITIONING OF THE CLASS");
                                    found = true;
                                    break;
                                }
                                else
                                {
                                    //this section tells me if it is valid and is less than or equal to 0
                                    //Monitor.Log("Count is less than for this class zero. Switching music packs");
                                    found = false;
                                    master_helper++; //iterate across the classes
                                    break;
                                }

                            }
                            else
                            {//this section iterates through the keys
                                Monitor.Log("Not there");
                                found = false;
                                helper1++;
                                continue;
                            }

                        } //itterate through all of the svalid locations that were stored in this class

                    }
                    else
                    {
                        Monitor.Log("No data could be loaded on this area. Swaping music packs");
                        found = false;
                    }
                    if (found == false) //if I didnt find the music.
                    {
                        master_helper++;

                        if (master_helper > this.MasterList.Count)
                        {
                            Monitor.Log("I've gone though every music pack with no success. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                            this.HasNoMusic = true;
                            return;

                        }
                        int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                        this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                List<Cue> cues = this.CurrentSoundInfo.LocationNightSongs.Values.ElementAt(helper1);
                int pointer = 0;
                int motzy = 0;
                while (!cues.Any())
                {
                    pointer++;
                    motzy = (pointer + randomNumber) % this.MasterList.Count;

                    this.CurrentSoundInfo = this.MasterList.ElementAt(motzy);
                    if (pointer > this.MasterList.Count)
                    {
                        Monitor.Log("No music packs have any valid music for this area. AKA all music packs are empty;");
                        this.HasNoMusic = true;
                        return;
                    }

                }


                Monitor.Log("loading music for this area");
                this.StopSound();
                int random3 = this.Random.Next(0, cues.Count);
                Game1.soundBank = this.CurrentSoundInfo.Soundbank;
                Game1.waveBank = this.CurrentSoundInfo.Wavebank;

                this.CurrentSong = cues.ElementAt(random3); //grab a random song from the winter song list
                this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                if (this.CurrentSong != null)
                {
                    this.HasNoMusic = false;
                    Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + "for the location " + Game1.player.currentLocation + " while it is night time.");
                    this.CurrentSong.Play();
                    this.Reset();
                    return;
                }



            }
            else
            {
                Monitor.Log("Location is null");
            }
        }//end music player
        public void music_player_rain_night()
        {
            if (!this.IsGameLoaded)
            {
                this.StartMusicDelay();
                return;
            }
            this.Random.Next();
            int randomNumber = this.Random.Next(0, this.MasterList.Count); //random number between 0 and n. 0 not included

            if (!this.MasterList.Any())
            {
                Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                this.Reset();
                return;

            }

            this.CurrentSoundInfo = this.MasterList.ElementAt(randomNumber); //grab a random wave bank/song bank/music pack/ from all available music packs.            

            if (Game1.player.currentLocation != null)
            {

                int helper1 = 0;
                int master_helper = 0;
                bool found = false;

                while (true)
                {
                    if (this.CurrentSoundInfo.LocationRainNightSongs.Keys.Contains(Game1.player.currentLocation.name + "_rain_night"))
                    {

                        foreach (var entry in this.CurrentSoundInfo.LocationRainNightSongs)
                        {
                            if (entry.Key == Game1.player.currentLocation.name + "_rain_night")
                            {
                                if (entry.Value.Count > 0)
                                {
                                    //Monitor.Log("FOUND THE RIGHT POSITIONING OF THE CLASS");
                                    found = true;
                                    break;
                                }
                                else
                                {
                                    //this section tells me if it is valid and is less than or equal to 0
                                    //Monitor.Log("Count is less than for this class zero. Switching music packs");
                                    found = false;
                                    master_helper++; //iterate across the classes
                                    break;
                                }

                            }
                            else
                            {//this section iterates through the keys
                                Monitor.Log("Not there");
                                found = false;
                                helper1++;
                                continue;
                            }

                        } //itterate through all of the svalid locations that were stored in this class

                    }
                    else
                    {
                        Monitor.Log("No data could be loaded on this area. Swaping music packs");
                        found = false;
                    }
                    if (found == false) //if I didnt find the music.
                    {
                        master_helper++;

                        if (master_helper > this.MasterList.Count)
                        {
                            Monitor.Log("I've gone though every music pack with no success. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                            this.HasNoMusic = true;
                            return;

                        }
                        int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                        this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                List<Cue> cues = this.CurrentSoundInfo.LocationRainNightSongs.Values.ElementAt(helper1);

                int pointer = 0;
                while (!cues.Any())
                {
                    pointer++;
                    int motzy = (pointer + randomNumber) % this.MasterList.Count;

                    this.CurrentSoundInfo = this.MasterList.ElementAt(motzy);
                    if (pointer > this.MasterList.Count)
                    {
                        Monitor.Log("No music packs have any valid music for this area. AKA all music packs are empty;");
                        this.HasNoMusic = true;
                        return;
                    }

                }
                Monitor.Log("loading music for this area");
                this.StopSound();
                int random3 = this.Random.Next(0, cues.Count);
                Game1.soundBank = this.CurrentSoundInfo.Soundbank;
                Game1.waveBank = this.CurrentSoundInfo.Wavebank;

                this.CurrentSong = cues.ElementAt(random3); //grab a random song from the winter song list
                this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                if (this.CurrentSong != null)
                {
                    this.HasNoMusic = false;
                    Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + "for the location " + Game1.player.currentLocation + " while it is raining at night.");
                    this.CurrentSong.Play();
                    this.Reset();
                    return;
                }



            }
            else
            {
                Monitor.Log("Location is null");
            }
        }//end music player

        public void spring_songs()
        {
            if (!this.IsGameLoaded)
            {
                this.StartMusicDelay();
                return;
            }
            this.Random.Next();
            int randomNumber = this.Random.Next(0, this.MasterList.Count); //random number between 0 and n. 0 not included

            if (!this.MasterList.Any())
            {
                Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                this.Reset();
                return;

            }

            this.CurrentSoundInfo = this.MasterList.ElementAt(randomNumber); //grab a random wave bank/song bank/music pack/ from all available music packs.            


            if (Game1.timeOfDay < 600 || Game1.timeOfDay >= Game1.getModeratelyDarkTime())  //expanded upon, just incase my night owl mod is installed.
            {
                randomNumber = this.Random.Next(0, this.CurrentSoundInfo.SpringNightSongs.Count); //random number between 0 and n. 0 not includes

                if (this.CurrentSoundInfo.SpringNightSongs.Count == 0) //nightly spring songs
                {
                    Monitor.Log("The spring night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                    int master_helper = 0;
                    while (master_helper != this.MasterList.Count)
                    {
                        if (this.CurrentSoundInfo.SpringNightSongs.Count > 0)
                        {
                            this.StopSound();
                            Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                            Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                            this.CurrentSong = this.CurrentSoundInfo.SpringNightSongs.ElementAt(randomNumber); //grab a random song from the spring song list
                            this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                            break;

                        } //itterate through all of the svalid locations that were stored in this class
                        else
                        {
                            master_helper++;
                        }
                        if (master_helper > this.MasterList.Count)
                        {
                            Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                            this.HasNoMusic = true;

                            return;

                            //break;
                        }
                        int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                        this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                        continue;
                    }
                }


                else
                {
                    this.StopSound();
                    this.CurrentSong = this.CurrentSoundInfo.SpringNightSongs.ElementAt(randomNumber); //grab a random song from the spring song list
                    Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                    Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                    this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                }
                if (this.CurrentSong != null)
                {
                    this.HasNoMusic = false;
                    Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + " while it is a Spring Night. Check the seasons folder for more info");
                    this.CurrentSong.Play();
                    this.Reset();
                    return;
                } //if cueballs is null, aka the song list either wasn't initialized, or it is empty, default to playing the normal songs.


            }
            //not nightly spring songs. AKA default songs

            randomNumber = this.Random.Next(0, this.CurrentSoundInfo.SpringSongs.Count); //random number between 0 and n. 0 not includes
            if (this.CurrentSoundInfo.SpringSongs.Count == 0)
            {
                Monitor.Log("The spring night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                int master_helper = 0;
                while (master_helper != this.MasterList.Count)
                {
                    if (this.CurrentSoundInfo.SpringNightSongs.Count > 0)
                    {
                        this.StopSound();
                        Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                        Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                        this.CurrentSong = this.CurrentSoundInfo.SpringSongs.ElementAt(randomNumber); //grab a random song from the spring song list
                        this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                        break;

                    } //itterate through all of the svalid locations that were stored in this class
                    else
                    {
                        master_helper++;
                    }
                    if (master_helper > this.MasterList.Count)
                    {
                        Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                        this.HasNoMusic = true;
                        return;
                        //            cueball = null;
                    }
                    int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                    this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                    continue;
                }
            }
            else
            {
                this.StopSound();
                this.CurrentSong = this.CurrentSoundInfo.SpringSongs.ElementAt(randomNumber); //grab a random song from the spring song list
                Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
            }
            if (this.CurrentSong == null) return;
            this.HasNoMusic = false;
            Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + "while it is Spring Time. Check the seasons folder for more info");
            this.CurrentSong.Play();
            this.Reset();
            return;

        } //plays the songs associated with spring time
        public void spring_rain_songs()
        {
            if (!this.IsGameLoaded)
            {
                this.StartMusicDelay();
                return;
            }
            this.Random.Next();
            int randomNumber = this.Random.Next(0, this.MasterList.Count); //random number between 0 and n. 0 not included

            if (!this.MasterList.Any())
            {
                Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                this.Reset();
                return;

            }

            this.CurrentSoundInfo = this.MasterList.ElementAt(randomNumber); //grab a random wave bank/song bank/music pack/ from all available music packs.            


            if (Game1.timeOfDay < 600 || Game1.timeOfDay >= Game1.getModeratelyDarkTime())  //expanded upon, just incase my night owl mod is installed.
            {
                randomNumber = this.Random.Next(0, this.CurrentSoundInfo.SpringRainNightSongs.Count); //random number between 0 and n. 0 not includes

                if (this.CurrentSoundInfo.SpringRainNightSongs.Count == 0) //nightly spring_rain songs
                {
                    Monitor.Log("The spring_rain night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                    int master_helper = 0;
                    while (master_helper != this.MasterList.Count)
                    {
                        if (this.CurrentSoundInfo.SpringRainNightSongs.Count > 0)
                        {
                            this.StopSound();
                            Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                            Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                            this.CurrentSong = this.CurrentSoundInfo.SpringRainNightSongs.ElementAt(randomNumber); //grab a random song from the spring_rain song list
                            this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                            break;

                        } //itterate through all of the svalid locations that were stored in this class
                        else
                        {
                            master_helper++;
                        }
                        if (master_helper > this.MasterList.Count)
                        {
                            Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                            this.HasNoMusic = true;
                            return;
                        }
                        int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                        this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                        continue;
                    }
                }
                else
                {
                    this.StopSound();
                    this.CurrentSong = this.CurrentSoundInfo.SpringRainNightSongs.ElementAt(randomNumber); //grab a random song from the fall song list
                    Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                    Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                    this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                }


                if (this.CurrentSong != null)
                {
                    this.HasNoMusic = false;
                    Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + "while it is a rainy Spring night. Check the Seasons folder for more info");
                    this.CurrentSong.Play();
                    this.Reset();
                    return;
                } //if cueballs is null, aka the song list either wasn't initialized, or it is empty, default to playing the normal songs.


            }
            //not nightly spring_rain songs. AKA default songs

            randomNumber = this.Random.Next(0, this.CurrentSoundInfo.SpringRainSongs.Count); //random number between 0 and n. 0 not includes
            if (this.CurrentSoundInfo.SpringRainSongs.Count == 0)
            {
                Monitor.Log("The spring_rain night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                int master_helper = 0;
                while (master_helper != this.MasterList.Count)
                {
                    if (this.CurrentSoundInfo.SpringRainSongs.Count > 0)
                    {
                        this.StopSound();
                        Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                        Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                        this.CurrentSong = this.CurrentSoundInfo.SpringRainSongs.ElementAt(randomNumber); //grab a random song from the spring_rain song list
                        this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                        break;

                    } //itterate through all of the svalid locations that were stored in this class
                    else
                    {
                        master_helper++;
                    }
                    if (master_helper > this.MasterList.Count)
                    {
                        Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                        this.HasNoMusic = true;
                        return;
                        //            cueball = null;
                    }
                    int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                    this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                    continue;
                }
            }
            else
            {
                this.StopSound();
                this.CurrentSong = this.CurrentSoundInfo.SpringRainSongs.ElementAt(randomNumber); //grab a random song from the fall song list
                Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
            }
            if (this.CurrentSong == null) return;
            this.HasNoMusic = false;
            Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + "while it is a rainy Spring Day. Check the seasons folder for more info");
            this.CurrentSong.Play();
            this.Reset();
        }

        public void summer_songs()
        {
            if (!this.IsGameLoaded)
            {
                this.StartMusicDelay();
                return;
            }
            this.Random.Next();
            int randomNumber = this.Random.Next(0, this.MasterList.Count); //random number between 0 and n. 0 not included

            if (this.MasterList.Count == 0)
            {
                Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                this.Reset();
                return;

            }

            this.CurrentSoundInfo = this.MasterList.ElementAt(randomNumber); //grab a random wave bank/song bank/music pack/ from all available music packs.            


            if (Game1.timeOfDay < 600 || Game1.timeOfDay >= Game1.getModeratelyDarkTime())  //expanded upon, just incase my night owl mod is installed.
            {
                randomNumber = this.Random.Next(0, this.CurrentSoundInfo.SummerNightSongs.Count); //random number between 0 and n. 0 not includes

                if (this.CurrentSoundInfo.SummerNightSongs.Count == 0) //nightly summer songs
                {
                    Monitor.Log("The summer night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                    int master_helper = 0;
                    while (master_helper != this.MasterList.Count)
                    {
                        if (this.CurrentSoundInfo.SummerNightSongs.Count > 0)
                        {
                            this.StopSound();
                            Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                            Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                            this.CurrentSong = this.CurrentSoundInfo.SummerNightSongs.ElementAt(randomNumber); //grab a random song from the summer song list
                            this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                            break;

                        } //itterate through all of the svalid locations that were stored in this class
                        else
                        {
                            master_helper++;
                        }
                        if (master_helper > this.MasterList.Count)
                        {
                            Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                            this.HasNoMusic = true;
                            return;
                        }
                        int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                        this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                        continue;
                    }
                }
                else
                {
                    this.StopSound();
                    Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                    Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                    this.CurrentSong = this.CurrentSoundInfo.SummerNightSongs.ElementAt(randomNumber); //grab a random song from the summer song list
                    this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);

                }



                if (this.CurrentSong != null)
                {
                    Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + " while it is a Summer Night. Check the Seasons folder for more info.");
                    this.HasNoMusic = false;
                    this.CurrentSong.Play();
                    this.Reset();
                    return;
                } //if cueballs is null, aka the song list either wasn't initialized, or it is empty, default to playing the normal songs.


            }
            //not nightly summer songs. AKA default songs

            randomNumber = this.Random.Next(0, this.CurrentSoundInfo.SummerSongs.Count); //random number between 0 and n. 0 not includes
            if (this.CurrentSoundInfo.SummerSongs.Count == 0)
            {
                Monitor.Log("The summer night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                int master_helper = 0;
                while (master_helper != this.MasterList.Count)
                {
                    if (this.CurrentSoundInfo.SummerNightSongs.Count > 0)
                    {
                        this.StopSound();
                        Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                        Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                        this.CurrentSong = this.CurrentSoundInfo.SummerSongs.ElementAt(randomNumber); //grab a random song from the summer song list
                        this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                        break;

                    } //itterate through all of the svalid locations that were stored in this class
                    else
                    {
                        master_helper++;
                    }
                    if (master_helper > this.MasterList.Count)
                    {
                        Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                        this.HasNoMusic = true;
                        return;
                        //            cueball = null;
                    }
                    int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                    this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                    continue;
                }
            }
            if (this.CurrentSong == null) return;
            this.StopSound();
            Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
            Game1.waveBank = this.CurrentSoundInfo.Wavebank;
            this.CurrentSong = this.CurrentSoundInfo.SummerSongs.ElementAt(randomNumber); //grab a random song from the summer song list
            this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
            if (this.CurrentSong != null)
            {
                Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + " while it is a Fall day. Check the Seasons folder for more info.");
                // System.Threading.Thread.Sleep(30000);
                this.HasNoMusic = false;
                this.CurrentSong.Play();
                this.Reset();
            }
            return;

        } //plays the songs associated with summer time
        public void summer_rain_songs()
        {
            if (!this.IsGameLoaded)
            {
                this.StartMusicDelay();
                return;
            }
            this.Random.Next();
            int randomNumber = this.Random.Next(0, this.MasterList.Count); //random number between 0 and n. 0 not included

            if (this.MasterList.Count == 0)
            {
                Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                this.Reset();
                return;

            }

            this.CurrentSoundInfo = this.MasterList.ElementAt(randomNumber); //grab a random wave bank/song bank/music pack/ from all available music packs.            


            if (Game1.timeOfDay < 600 || Game1.timeOfDay >= Game1.getModeratelyDarkTime())  //expanded upon, just incase my night owl mod is installed.
            {
                randomNumber = this.Random.Next(0, this.CurrentSoundInfo.SummerRainNightSongs.Count); //random number between 0 and n. 0 not includes

                if (this.CurrentSoundInfo.SummerRainNightSongs.Count == 0) //nightly summer_rain songs
                {
                    Monitor.Log("The summer_rain night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                    int master_helper = 0;
                    while (master_helper != this.MasterList.Count)
                    {
                        if (this.CurrentSoundInfo.SummerRainNightSongs.Count > 0)
                        {
                            this.StopSound();
                            Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                            Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                            this.CurrentSong = this.CurrentSoundInfo.SummerRainNightSongs.ElementAt(randomNumber); //grab a random song from the summer_rain song list
                            this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                            break;

                        } //itterate through all of the svalid locations that were stored in this class
                        else
                        {
                            master_helper++;
                        }
                        if (master_helper > this.MasterList.Count)
                        {
                            Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                            this.HasNoMusic = true;
                            return;
                        }
                        int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                        this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                        continue;
                    }
                }

                else
                {
                    this.StopSound();
                    this.CurrentSong = this.CurrentSoundInfo.SummerRainNightSongs.ElementAt(randomNumber); //grab a random song from the summer song list
                    Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                    Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                    this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                }

                if (this.CurrentSong != null)
                {
                    Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + " while it is a rainy Summer Night. Check the Seasons folder for more info.");
                    this.HasNoMusic = false;
                    this.CurrentSong.Play();
                    this.Reset();
                    return;
                } //if cueballs is null, aka the song list either wasn't initialized, or it is empty, default to playing the normal songs.


            }
            //not nightly summer_rain songs. AKA default songs

            randomNumber = this.Random.Next(0, this.CurrentSoundInfo.SummerRainSongs.Count); //random number between 0 and n. 0 not includes
            if (this.CurrentSoundInfo.SummerRainSongs.Count == 0)
            {
                Monitor.Log("The summer_rain night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                int master_helper = 0;
                while (master_helper != this.MasterList.Count)
                {
                    if (this.CurrentSoundInfo.SummerRainSongs.Count > 0)
                    {
                        this.StopSound();
                        Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                        Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                        this.CurrentSong = this.CurrentSoundInfo.SummerRainSongs.ElementAt(randomNumber); //grab a random song from the summer_rain song list
                        this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                        break;

                    } //itterate through all of the svalid locations that were stored in this class
                    else
                    {
                        master_helper++;
                    }
                    if (master_helper > this.MasterList.Count)
                    {
                        Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                        this.HasNoMusic = true;
                        return;
                        //            cueball = null;
                    }

                    int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                    this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                    continue;
                }
            }

            else
            {
                this.StopSound();
                this.CurrentSong = this.CurrentSoundInfo.SummerRainSongs.ElementAt(randomNumber); //grab a random song from the summer song list
                Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
            }

            if (this.CurrentSong == null) return;
            Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + " while it is a rainy Summer day. Check the Seasons folder for more info.");
            this.HasNoMusic = false;
            this.CurrentSong.Play();
            this.Reset();

        } //plays the songs associated with summer time
        public void fall_songs()
        {
            if (!this.IsGameLoaded)
                return;

            this.Random.Next();
            int randomNumber = this.Random.Next(0, this.MasterList.Count); //random number between 0 and n. 0 not included

            if (this.MasterList.Count == 0)
            {
                Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                // System.Threading.Thread.Sleep(3000);
                this.Reset();
                return;

            }

            this.CurrentSoundInfo = this.MasterList.ElementAt(randomNumber); //grab a random wave bank/song bank/music pack/ from all available music packs.            


            if (Game1.timeOfDay < 600 || Game1.timeOfDay >= Game1.getModeratelyDarkTime())  //expanded upon, just incase my night owl mod is installed.
            {
                randomNumber = this.Random.Next(0, this.CurrentSoundInfo.FallNightSongs.Count); //random number between 0 and n. 0 not includes

                if (this.CurrentSoundInfo.FallNightSongs.Count == 0) //nightly fall songs
                {
                    Monitor.Log("The fall night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                                                                                                      //   System.Threading.Thread.Sleep(3000);
                    int master_helper = 0;
                    while (master_helper != this.MasterList.Count)
                    {
                        if (this.CurrentSoundInfo.FallNightSongs.Count > 0)
                        {
                            this.StopSound();
                            this.CurrentSong = this.CurrentSoundInfo.FallNightSongs.ElementAt(randomNumber); //grab a random song from the fall song list
                            Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                            Game1.waveBank = this.CurrentSoundInfo.Wavebank;

                            this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                            break;

                        } //itterate through all of the svalid locations that were stored in this class
                        else
                        {
                            master_helper++;
                        }
                        if (master_helper >= this.MasterList.Count)
                        {
                            Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                            //         System.Threading.Thread.Sleep(3000);
                            this.HasNoMusic = true;
                            return;
                            // cueball = null;
                        }
                        int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                        this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                        continue;
                    }
                }
                else
                {
                    this.StopSound();

                    this.CurrentSong = this.CurrentSoundInfo.FallNightSongs.ElementAt(randomNumber); //grab a random song from the fall song list
                    Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                    Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                    this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                }


                if (this.CurrentSong != null)
                {
                    Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + " while it is a Fall Night. Check the Seasons folder for more info.");

                    //System.Threading.Thread.Sleep(30000);
                    this.HasNoMusic = false;
                    this.CurrentSong.Play();
                    this.Reset();
                    return;
                } //if cueballs is null, aka the song list either wasn't initialized, or it is empty, default to playing the normal songs.


            }
            //not nightly fall songs. AKA default songs

            randomNumber = this.Random.Next(0, this.CurrentSoundInfo.FallSongs.Count); //random number between 0 and n. 0 not includes
            if (this.CurrentSoundInfo.FallSongs.Count == 0)
            {
                Monitor.Log("The fall night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                                                                                                  // System.Threading.Thread.Sleep(3000);
                int master_helper = 0;
                while (master_helper != this.MasterList.Count)
                {
                    if (this.CurrentSoundInfo.FallSongs.Count > 0)
                    {
                        this.StopSound();
                        this.CurrentSong = this.CurrentSoundInfo.FallSongs.ElementAt(randomNumber); //grab a random song from the fall song list
                        Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                        Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                        this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                        break;

                    } //itterate through all of the svalid locations that were stored in this class
                    else
                    {
                        master_helper++;
                    }

                    if (master_helper >= this.MasterList.Count)
                    {
                        Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                        //       System.Thr1eading.Thread.Sleep(3000);
                        this.HasNoMusic = true;
                        return;
                        //            cueball = null;
                    }
                    int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                    this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                    continue;
                }
            }
            else
            {
                this.StopSound();
                this.CurrentSong = this.CurrentSoundInfo.FallSongs.ElementAt(randomNumber); //grab a random song from the fall song list
                Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
            }
            if (this.CurrentSong != null)
            {
                Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + " while it is a Fall day. Check the Seasons folder for more info.");
                // System.Threading.Thread.Sleep(30000);
                this.HasNoMusic = false;
                this.CurrentSong.Play();
                this.Reset();
            }
            return;

        } //plays the songs associated with fall time
        public void fall_rain_songs()
        {
            if (!this.IsGameLoaded)
            {
                this.StartMusicDelay();
                return;
            }
            this.Random.Next();
            int randomNumber = this.Random.Next(0, this.MasterList.Count); //random number between 0 and n. 0 not included

            if (this.MasterList.Count == 0)
            {
                Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                this.Reset();
                return;

            }

            this.CurrentSoundInfo = this.MasterList.ElementAt(randomNumber); //grab a random wave bank/song bank/music pack/ from all available music packs.            


            if (Game1.timeOfDay < 600 || Game1.timeOfDay >= Game1.getModeratelyDarkTime())  //expanded upon, just incase my night owl mod is installed.
            {
                randomNumber = this.Random.Next(0, this.CurrentSoundInfo.FallRainNightSongs.Count); //random number between 0 and n. 0 not includes

                if (this.CurrentSoundInfo.FallRainNightSongs.Count == 0) //nightly fall_rain songs
                {
                    Monitor.Log("The fall_rain night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                    int master_helper = 0;
                    while (master_helper != this.MasterList.Count)
                    {
                        if (this.CurrentSoundInfo.FallRainNightSongs.Count > 0)
                        {
                            this.StopSound();
                            Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                            Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                            this.CurrentSong = this.CurrentSoundInfo.FallRainNightSongs.ElementAt(randomNumber); //grab a random song from the fall_rain song list
                            this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);

                            break;

                        } //itterate through all of the svalid locations that were stored in this class
                        else
                        {
                            master_helper++;
                        }
                        if (master_helper > this.MasterList.Count)
                        {
                            Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                            this.HasNoMusic = true;
                            return;
                        }
                        int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                        this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                        continue;
                    }
                }
                else
                {
                    this.StopSound();
                    this.CurrentSong = this.CurrentSoundInfo.FallRainNightSongs.ElementAt(randomNumber); //grab a random song from the fall song list
                    Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                    Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                    this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                }


                if (this.CurrentSong != null)
                {
                    this.HasNoMusic = false;
                    Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + " while it is a rainy Fall Night. Check the Seasons folder for more info.");
                    this.CurrentSong.Play();
                    this.Reset();
                    return;
                } //if cueballs is null, aka the song list either wasn't initialized, or it is empty, default to playing the normal songs.


            }
            //not nightly fall_rain songs. AKA default songs

            randomNumber = this.Random.Next(0, this.CurrentSoundInfo.FallRainSongs.Count); //random number between 0 and n. 0 not includes
            if (this.CurrentSoundInfo.FallRainSongs.Count == 0)
            {
                Monitor.Log("The fall_rain night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                int master_helper = 0;
                while (master_helper != this.MasterList.Count)
                {
                    if (this.CurrentSoundInfo.FallRainSongs.Count > 0)
                    {
                        this.StopSound();
                        Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                        Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                        this.CurrentSong = this.CurrentSoundInfo.FallRainSongs.ElementAt(randomNumber); //grab a random song from the fall_rain song list
                        this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                        break;

                    } //itterate through all of the svalid locations that were stored in this class
                    else
                    {
                        master_helper++;
                    }
                    if (master_helper > this.MasterList.Count)
                    {
                        Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                        this.HasNoMusic = true;
                        return;
                        //            cueball = null;
                    }
                    int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                    this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                    continue;
                }
            }
            else
            {
                this.StopSound();
                this.CurrentSong = this.CurrentSoundInfo.FallRainSongs.ElementAt(randomNumber); //grab a random song from the fall song list
                Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
            }

            if (this.CurrentSong == null) return;
            Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + " while it is a rainy Fall day. Check the Seasons folder for more info.");
            this.HasNoMusic = false;
            this.CurrentSong.Play();
            this.Reset();

        } //plays the songs associated with fall time
        public void winter_songs()
        {
            if (!this.IsGameLoaded)
            {
                this.StartMusicDelay();
                return;
            }
            this.Random.Next();
            int randomNumber = this.Random.Next(0, this.MasterList.Count); //random number between 0 and n. 0 not included

            if (this.MasterList.Count == 0)
            {
                Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                this.Reset();
                return;

            }

            this.CurrentSoundInfo = this.MasterList.ElementAt(randomNumber); //grab a random wave bank/song bank/music pack/ from all available music packs.            


            if (Game1.timeOfDay < 600 || Game1.timeOfDay >= Game1.getModeratelyDarkTime())  //expanded upon, just incase my night owl mod is installed.
            {
                randomNumber = this.Random.Next(0, this.CurrentSoundInfo.WinterNightSongs.Count); //random number between 0 and n. 0 not includes

                if (this.CurrentSoundInfo.WinterNightSongs.Count == 0) //nightly winter songs
                {

                    Monitor.Log("The winter night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                    int master_helper = 0;
                    while (master_helper != this.MasterList.Count)
                    {
                        if (this.CurrentSoundInfo.WinterNightSongs.Count > 0)
                        {
                            this.StopSound();
                            Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                            Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                            this.CurrentSong = this.CurrentSoundInfo.WinterNightSongs.ElementAt(randomNumber); //grab a random song from the winter song list
                            this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                            break;

                        } //itterate through all of the svalid locations that were stored in this class
                        else
                        {
                            master_helper++;
                        }
                        if (master_helper > this.MasterList.Count)
                        {
                            Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                            this.HasNoMusic = true;
                            return;
                        }
                        int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                        this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                        continue;
                    }
                }

                else
                {
                    this.StopSound();
                    this.CurrentSong = this.CurrentSoundInfo.WinterNightSongs.ElementAt(randomNumber); //grab a random song from the fall song list
                    Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                    Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                    this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                }

                if (this.CurrentSong != null)
                {
                    this.HasNoMusic = false;
                    Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + " while it is a Winter Night. Check the Seasons folder for more info.");
                    this.CurrentSong.Play();
                    this.Reset();
                    return;
                } //if cueballs is null, aka the song list either wasn't initialized, or it is empty, default to playing the normal songs.


            }
            //not nightly winter songs. AKA default songs

            randomNumber = this.Random.Next(0, this.CurrentSoundInfo.WinterSongs.Count); //random number between 0 and n. 0 not includes
            if (this.CurrentSoundInfo.WinterSongs.Count == 0)
            {
                Monitor.Log("The winter night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                int master_helper = 0;
                while (master_helper != this.MasterList.Count)
                {
                    if (this.CurrentSoundInfo.WinterNightSongs.Count > 0)
                    {
                        this.StopSound();
                        Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                        Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                        this.CurrentSong = this.CurrentSoundInfo.WinterSongs.ElementAt(randomNumber); //grab a random song from the winter song list
                        this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                        break;

                    } //itterate through all of the svalid locations that were stored in this class
                    else
                    {
                        master_helper++;
                    }
                    if (master_helper > this.MasterList.Count)
                    {
                        Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                        this.HasNoMusic = true;
                        return;
                        //            cueball = null;
                    }
                    int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                    this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                    continue;
                }
            }
            else
            {
                this.StopSound();
                this.CurrentSong = this.CurrentSoundInfo.WinterSongs.ElementAt(randomNumber); //grab a random song from the fall song list
                Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
            }
            if (this.CurrentSong == null) return;
            Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + " while it is a Winter Day. Check the Seasons folder for more info.");
            this.HasNoMusic = false;
            this.CurrentSong.Play();
            this.Reset();

        } //plays the songs associated with winter time

        public void winter_snow_songs()
        {
            if (!this.IsGameLoaded)
            {
                this.StartMusicDelay();
                return;
            }
            this.Random.Next();
            int randomNumber = this.Random.Next(0, this.MasterList.Count); //random number between 0 and n. 0 not included

            if (this.MasterList.Count == 0)
            {
                Monitor.Log("The Wave Bank list is empty. Something went wrong, or you don't have any music packs installed, or you didn't have any songs in the list files.");
                this.Reset();
                return;

            }

            this.CurrentSoundInfo = this.MasterList.ElementAt(randomNumber); //grab a random wave bank/song bank/music pack/ from all available music packs.            


            if (Game1.timeOfDay < 600 || Game1.timeOfDay >= Game1.getModeratelyDarkTime())  //expanded upon, just incase my night owl mod is installed.
            {
                randomNumber = this.Random.Next(0, this.CurrentSoundInfo.WinterSnowNightSongs.Count); //random number between 0 and n. 0 not includes

                if (this.CurrentSoundInfo.WinterSnowNightSongs.Count == 0) //nightly winter_snow songs
                {
                    Monitor.Log("The winter_snow night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                    int master_helper = 0;
                    while (master_helper != this.MasterList.Count)
                    {
                        if (this.CurrentSoundInfo.WinterSnowNightSongs.Count > 0)
                        {
                            this.StopSound();
                            Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                            Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                            this.CurrentSong = this.CurrentSoundInfo.WinterSnowNightSongs.ElementAt(randomNumber); //grab a random song from the winter_snow song list
                            this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                            break;

                        } //itterate through all of the svalid locations that were stored in this class
                        else
                        {
                            master_helper++;
                        }
                        if (master_helper > this.MasterList.Count)
                        {
                            Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                            this.HasNoMusic = true;
                            return;
                        }
                        int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                        this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                        continue;
                    }
                }

                else
                {
                    this.StopSound();
                    this.CurrentSong = this.CurrentSoundInfo.WinterSnowNightSongs.ElementAt(randomNumber); //grab a random song from the fall song list
                    Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                    Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                    this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                }

                if (this.CurrentSong != null)
                {
                    this.HasNoMusic = false;
                    Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + " while it is a snowy Winter night. Check the Seasons folder for more info.");
                    this.CurrentSong.Play();
                    this.Reset();
                    return;
                } //if cueballs is null, aka the song list either wasn't initialized, or it is empty, default to playing the normal songs.


            }
            //not nightly winter_snow songs. AKA default songs

            randomNumber = this.Random.Next(0, this.CurrentSoundInfo.WinterSnowSongs.Count); //random number between 0 and n. 0 not includes
            if (this.CurrentSoundInfo.WinterSnowSongs.Count == 0)
            {
                Monitor.Log("The winter_snow night song list is empty. Trying to look for more songs."); //or should I default where if there aren't any nightly songs to play a song from a different play list?
                int master_helper = 0;
                while (master_helper != this.MasterList.Count)
                {
                    if (this.CurrentSoundInfo.WinterSnowSongs.Count > 0)
                    {
                        this.StopSound();
                        Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                        Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                        this.CurrentSong = this.CurrentSoundInfo.WinterSnowSongs.ElementAt(randomNumber); //grab a random song from the winter_snow song list
                        this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
                        break;

                    } //itterate through all of the svalid locations that were stored in this class
                    else
                    {
                        master_helper++;
                    }
                    if (master_helper > this.MasterList.Count)
                    {
                        Monitor.Log("I've gone though every music pack with no success for default music. There is no music to load for this area so it will be silent once this song finishes playing. Sorry!");
                        this.HasNoMusic = true;
                        return;
                        //            cueball = null;
                    }
                    int randomIndex = (master_helper + randomNumber) % this.MasterList.Count;
                    this.CurrentSoundInfo = this.MasterList.ElementAt(randomIndex); //grab a random wave bank/song bank/music pack/ from all available music packs.            
                    continue;
                }
            }
            else
            {
                this.StopSound();
                this.CurrentSong = this.CurrentSoundInfo.WinterSnowSongs.ElementAt(randomNumber); //grab a random song from the fall song list
                Game1.soundBank = this.CurrentSoundInfo.Soundbank; //access my new sound table
                Game1.waveBank = this.CurrentSoundInfo.Wavebank;
                this.CurrentSong = Game1.soundBank.GetCue(this.CurrentSong.Name);
            }
            if (this.CurrentSong == null) return;
            this.HasNoMusic = false;
            Monitor.Log("Now listening to: " + this.CurrentSong.Name + " from the music pack located at: " + this.CurrentSoundInfo.Directory + " while it is a snowy winter day. Check the Seasons folder for more info.");
            this.CurrentSong.Play();
            this.Reset();
            return;

        } //plays the songs associated with spring time


        /// <summary>Stop the currently playing sound, if any.</summary>
        public void StopSound()
        {
            if (this.CurrentSong == null)
            {
                //trying to stop a song that doesn't "exist" crashes the game. This prevents that.
                return;
            }

            if (this.CurrentSoundInfo == null)
            {
                //if my info class is null, return. Should only be null if the game starts. Pretty sure my code should prevent this.
                return;
            }
            Game1.soundBank = this.CurrentSoundInfo.Soundbank; //reset the wave/sound banks back to the music pack's
            Game1.waveBank = this.CurrentSoundInfo.Wavebank;
            this.CurrentSong.Stop(AudioStopOptions.Immediate); //redundant stopping code
            this.CurrentSong.Stop(AudioStopOptions.AsAuthored);
            Game1.soundBank = this.DefaultSoundbank; //reset the wave/sound to the game's original
            Game1.waveBank = this.DefaultWavebank;
            this.CurrentSong = null;
        }

        /// <summary>Reset the game audio to the original settings.</summary>
        private void Reset()
        {
            Game1.waveBank = this.DefaultWavebank;
            Game1.soundBank = this.DefaultSoundbank;
        }

        /// <summary>Save the configuration settings.</summary>
        private void WriteConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, "Music_Expansion_Config.txt");
            string[] text = new string[20];
            text[0] = "Player: Stardew Valley Music Expansion Config. Feel free to edit.";
            text[1] = "====================================================================================";

            text[2] = "Minimum delay time: This is the minimal amout of time(in miliseconds!!!) that will pass before another song will play. 0 means a song will play immediately, 1000 means a second will pass, etc. Used in RNG to determine a random delay between songs.";
            text[3] = this.MinSongDelay.ToString();

            text[4] = "Maximum delay time: This is the maximum amout of time(in miliseconds!!!) that will pass before another song will play. 0 means a song will play immediately, 1000 means a second will pass, etc. Used in RNG to determine a random delay between songs.";
            text[5] = this.MaxSongDelay.ToString();

            text[6] = "Silent rain? Setting this value to false plays the default ambient rain music along side whatever songs are set in rain music. Setting this to true will disable the ambient rain music. It's up to the soundpack creators wither or not they want to mix their music with rain prior to loading it in here.";
            text[7] = this.SilentRain.ToString();

            text[8] = "Seasonal_Music? Setting this value to true will play the seasonal music from the music packs instead of defaulting to the Stardew Valley Soundtrack.";
            text[9] = this.PlaySeasonalMusic.ToString();

            File.WriteAllLines(path, text);
        }

        /// <summary>Load the configuration settings.</summary>
        void LoadConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, "Music_Expansion_Config.txt");
            if (!File.Exists(path))
            {
                this.MinSongDelay = 10000;
                this.MaxSongDelay = 30000;
                this.SilentRain = false;
                this.PlaySeasonalMusic = true;
            }
            else
            {
                string[] text = File.ReadAllLines(path);
                this.MinSongDelay = Convert.ToInt32(text[3]);
                this.MaxSongDelay = Convert.ToInt32(text[5]);
                this.SilentRain = Convert.ToBoolean(text[7]);
                this.PlaySeasonalMusic = Convert.ToBoolean(text[9]);
            }
        }
    }
}
