using Microsoft.Xna.Framework.Audio;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stardew_Music_Expansion_API
{
    //also known as the music_pack
    public class MusicManager
    {
        public string wave_bank_name;
        public string sound_bank_name;

        public List<Cue> spring_song_list;
        public int num_of_spring_songs;
        public List<Cue> summer_song_list;
        public int num_of_summer_songs;
        public List<Cue> fall_song_list;
        public int num_of_fall_songs;
        public List<Cue> winter_song_list;
        public int num_of_winter_songs;


        public List<Cue> spring_night_song_list;
        public int num_of_spring_night_songs;
        public List<Cue> summer_night_song_list;
        public int num_of_summer_night_songs;
        public List<Cue> fall_night_song_list;
        public int num_of_fall_night_songs;
        public List<Cue> winter_night_song_list;
        public int num_of_winter_night_songs;


        public List<Cue> spring_rain_song_list;
        public int num_of_spring_rain_songs;
        public List<Cue> summer_rain_song_list;
        public int num_of_summer_rain_songs;
        public List<Cue> fall_rain_song_list;
        public int num_of_fall_rain_songs;
        public List<Cue> winter_snow_song_list;
        public int num_of_winter_snow_songs;


        public List<Cue> spring_rain_night_song_list;
        public int num_of_spring_rain_night_songs;
        public List<Cue> summer_rain_night_song_list;
        public int num_of_summer_rain_night_songs;
        public List<Cue> fall_rain_night_song_list;
        public int num_of_fall_rain_night_songs;
        public List<Cue> winter_snow_night_song_list;
        public int num_of_winter_snow_night_songs;

        public List<Cue> locational_cues;
        public Dictionary<string, List<Cue>> locational_songs;


        public Dictionary<string, List<Cue>> locational_rain_songs;
        public Dictionary<string, List<Cue>> locational_night_songs;
        public Dictionary<string, List<Cue>> locational_rain_night_songs;


        public WaveBank newwave;
        public SoundBank new_sound_bank;
        public string path_loc;

        public MusicManager(string wb, string sb, string directory)
        {
            wave_bank_name = wb;
            sound_bank_name = sb;
            wave_bank_name += ".xwb";
            sound_bank_name += ".xsb";
            path_loc = directory;

            Console.WriteLine(Path.Combine(path_loc, wave_bank_name));
            Console.WriteLine(Path.Combine(path_loc, sound_bank_name));


            if (File.Exists(Path.Combine(path_loc, wave_bank_name)))
            {
                newwave = new WaveBank(Game1.audioEngine, Path.Combine(path_loc, wave_bank_name)); //look for wave bank in sound_pack root directory.
            }
            if (File.Exists(Path.Combine(path_loc, sound_bank_name)))
            {
                new_sound_bank = new SoundBank(Game1.audioEngine, Path.Combine(path_loc, sound_bank_name)); //look for sound bank in sound_pack root directory.
            }

            Game1.audioEngine.Update();

            spring_song_list = new List<Cue>();
            num_of_spring_songs = 0;
            summer_song_list = new List<Cue>();
            num_of_summer_songs = 0;
            fall_song_list = new List<Cue>();
            num_of_fall_songs = 0;
            winter_song_list = new List<Cue>();
            num_of_winter_songs = 0;

            spring_night_song_list = new List<Cue>();
            num_of_spring_night_songs = 0;
            summer_night_song_list = new List<Cue>();
            num_of_summer_night_songs = 0;
            fall_night_song_list = new List<Cue>();
            num_of_fall_night_songs = 0;
            winter_night_song_list = new List<Cue>();
            num_of_winter_night_songs = 0;


            //rainy initialization
            spring_rain_song_list = new List<Cue>();
            num_of_spring_rain_songs = 0;
            summer_rain_song_list = new List<Cue>();
            num_of_summer_rain_songs = 0;
            fall_rain_song_list = new List<Cue>();
            num_of_fall_rain_songs = 0;
            winter_snow_song_list = new List<Cue>();
            num_of_winter_snow_songs = 0;

            spring_rain_night_song_list = new List<Cue>();
            num_of_spring_rain_night_songs = 0;
            summer_rain_night_song_list = new List<Cue>();
            num_of_summer_rain_night_songs = 0;
            fall_rain_night_song_list = new List<Cue>();
            num_of_fall_rain_night_songs = 0;
            winter_snow_night_song_list = new List<Cue>();
            num_of_winter_snow_night_songs = 0;

            locational_songs = new Dictionary<string, List<Cue>>();
            locational_rain_songs = new Dictionary<string, List<Cue>>();
            locational_night_songs = new Dictionary<string, List<Cue>>();
            locational_rain_night_songs = new Dictionary<string, List<Cue>>();
        }

        public void Music_Loader_Seasons(string conditional_name, Dictionary<string, MusicManager> reference_dic) //reads in cue names from a text file and adds them to a specific list. Morphs with specific conditional name. Conditionals are hardcoded.
        {
            //loads the data to the variables upon loading the game.
            var music_path = path_loc;
            string mylocation = Path.Combine(music_path, "Music_Files", "Seasons", conditional_name);
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";


            if (!File.Exists(mylocation3)) //check to make sure the file actually exists
            {

                string error_message = "Stardew Symohony:The specified music file could not be found. That music file is " + conditional_name + " which should be located at " + mylocation3 + " but don't worry I'll create it for you right now. It's going to be blank though";
                Console.WriteLine(error_message);

                string[] mystring3 = new string[3];//seems legit.
                mystring3[0] = conditional_name + " music file. This file holds all of the music that will play when there is no music for this game location, or simply put this is default music. Simply type the name of the song below the wall of equal signs.";
                mystring3[1] = "========================================================================================";

                File.WriteAllLines(mylocation3, mystring3);
            }

            else
            {
                Console.WriteLine("Stardew Symphony:The music pack located at: " + path_loc + " is processing the song info for the game location: " + conditional_name);
                //System.Threading.Thread.Sleep(1000);
                // add in data here

                string[] readtext = File.ReadAllLines(mylocation3);
                string cue_name;
                int i = 2;
                var lineCount = File.ReadLines(mylocation3).Count();

                while (i < lineCount) //the ordering seems bad, but it works.
                {
                    if (Convert.ToString(readtext[i]) == "")
                    {
                        //  Monitor.Log("Blank space detected.");
                        break;

                    }
                    if (Convert.ToString(readtext[i]) == "\n")
                    {
                        break;

                    }


                    if (conditional_name == "spring")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;


                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            spring_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_spring_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            spring_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_spring_songs++;
                        }
                        //  Monitor.Log(cue_name);
                    }
                    if (conditional_name == "summer")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;

                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            summer_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_summer_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            summer_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_summer_songs++;
                        }
                    }
                    if (conditional_name == "fall")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;

                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            fall_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_fall_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            fall_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_fall_songs++;
                        }
                    }
                    if (conditional_name == "winter")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;

                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            winter_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_winter_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            winter_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_winter_songs++;
                        }
                    }
                    //add in other stuff here
                    //========================================================================================================================================================================================
                    //NIGHTLY SEASONAL LOADERS
                    if (conditional_name == "spring_night")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;


                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            spring_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_spring_night_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            spring_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_spring_night_songs++;
                        }
                        //  Monitor.Log(cue_name);
                    }
                    if (conditional_name == "summer_night")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;

                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            summer_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_summer_night_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            summer_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_summer_night_songs++;
                        }
                    }
                    if (conditional_name == "fall_night")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;

                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            fall_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_fall_night_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            fall_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_fall_night_songs++;
                        }
                    }
                    if (conditional_name == "winter_night")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;

                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            winter_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_winter_night_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            winter_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_winter_night_songs++;
                        }
                    }
                    ////////NOW I"M ADDING THE PART THAT WILL READ IN RAINY SEASONAL SONGS FOR DAY AND NIGHT
                    if (conditional_name == "spring_rain")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;


                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            spring_rain_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_spring_rain_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            spring_rain_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_spring_rain_songs++;
                        }
                        //  Monitor.Log(cue_name);
                    }
                    if (conditional_name == "summer_rain")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;

                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            summer_rain_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_summer_rain_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            summer_rain_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_summer_rain_songs++;
                        }
                    }
                    if (conditional_name == "fall_rain")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;

                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            fall_rain_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_fall_rain_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            fall_rain_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_fall_rain_songs++;
                        }
                    }
                    if (conditional_name == "winter_snow")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;

                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            winter_snow_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_winter_snow_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            winter_snow_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_winter_snow_songs++;
                        }
                    }
                    //add in other stuff here
                    //========================================================================================================================================================================================
                    //NIGHTLY SEASONAL RAIN LOADERS
                    if (conditional_name == "spring_rain_night")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;


                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            spring_rain_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_spring_rain_night_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            spring_rain_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_spring_rain_night_songs++;
                        }
                        //  Monitor.Log(cue_name);
                    }
                    if (conditional_name == "summer_rain_night")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;

                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            summer_rain_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_summer_rain_night_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            summer_rain_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_summer_rain_night_songs++;
                        }
                    }
                    if (conditional_name == "fall_rain_night")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;

                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            fall_rain_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_fall_rain_night_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            fall_rain_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_fall_rain_night_songs++;
                        }
                    }
                    if (conditional_name == "winter_snow_night")
                    {
                        cue_name = Convert.ToString(readtext[i]);
                        i++;

                        if (!reference_dic.Keys.Contains(cue_name))
                        {
                            winter_snow_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_winter_snow_night_songs++;
                            reference_dic.Add(cue_name, this);

                        }
                        else
                        {
                            winter_snow_night_song_list.Add(new_sound_bank.GetCue(cue_name));

                            num_of_winter_snow_night_songs++;
                        }
                    }
                }
                if (i == 2)
                {
                    //  Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 +" this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");
                    // System.Threading.Thread.Sleep(10);
                    return;
                }
                Console.WriteLine("StardewSymohony:The music pack located at: " + path_loc + " has successfully processed the song info for the game location: " + conditional_name);
            }
        }
        public void Music_Loader_Locations(string conditional_name, Dictionary<string, MusicManager> reference_dic) //reads in cue names from a text file and adds them to a specific list. Morphs with specific conditional name. Conditionals are hardcoded.
        {
            locational_cues = new List<Cue>();
            //loads the data to the variables upon loading the game.
            var music_path = path_loc;
            string mylocation = Path.Combine(music_path, "Music_Files", "Locations", conditional_name);
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //check to make sure the file actually exists
            {
                Console.WriteLine("StardewSymohony:A music list for the location " + conditional_name + " does not exist for the music pack located at " + mylocation3 + " which isn't a problem, I just thought I'd let you know since this may have been intentional. Also I'm creating it for you just incase. Cheers.");

                //Console.WriteLine("Creating the Config file");
                string[] mystring3 = new string[3];//seems legit.
                mystring3[0] = conditional_name + " music file. This file holds all of the music that will play when at this game location. Simply type the name of the song below the wall of equal signs.";
                mystring3[1] = "========================================================================================";

                File.WriteAllLines(mylocation3, mystring3);
                return;
            }

            else
            {
                Console.WriteLine("Stardew Symphony:The music pack located at: " + path_loc + " is processing the song info for the game location: " + conditional_name);
                //System.Threading.Thread.Sleep(1000);
                string[] readtext = File.ReadAllLines(mylocation3);
                string cue_name;
                int i = 2;
                var lineCount = File.ReadLines(mylocation3).Count();
                while (i < lineCount) //the ordering seems bad, but it works.
                {
                    if (Convert.ToString(readtext[i]) == "")
                    {
                        break;
                    }
                    if (Convert.ToString(readtext[i]) == "\n")
                    {
                        break;
                    }
                    cue_name = Convert.ToString(readtext[i]);
                    i++;
                    if (!reference_dic.Keys.Contains(cue_name))
                    {
                        locational_cues.Add(new_sound_bank.GetCue(cue_name));
                        reference_dic.Add(cue_name, this);
                    }
                    else
                    {
                        locational_cues.Add(new_sound_bank.GetCue(cue_name));
                    }
                }
                if (i == 2)
                {
                    //  Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 + " this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");
                    return;
                }
                if (locational_cues.Count > 0)
                {
                    locational_songs.Add(conditional_name, locational_cues);
                    Console.WriteLine("StardewSymhony:The music pack located at: " + path_loc + " has successfully processed the song info for the game location: " + conditional_name);

                    return;
                }
                else
                {
                    // Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 + " this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");
                    return;
                }
            }
        }

        public void Music_Loader_Locations_Rain(string conditional_name, Dictionary<string, MusicManager> reference_dic) //reads in cue names from a text file and adds them to a specific list. Morphs with specific conditional name. Conditionals are hardcoded.
        {
            locational_cues = new List<Cue>();
            var music_path = path_loc;
            string mylocation = Path.Combine(music_path, "Music_Files", "Locations", conditional_name);
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //check to make sure the file actually exists
            {
                Console.WriteLine("StardewSymphony:A music list for the location " + conditional_name + " does not exist for the music pack located at " + mylocation3 + " which isn't a problem, I just thought I'd let you know since this may have been intentional. Also I'm creating it for you just incase. Cheers.");
                string[] mystring3 = new string[3];//seems legit.
                mystring3[0] = conditional_name + " music file. This file holds all of the music that will play when at this game location. Simply type the name of the song below the wall of equal signs.";
                mystring3[1] = "========================================================================================";
                File.WriteAllLines(mylocation3, mystring3);
                return;
            }

            else
            {
                // add in data here
                string[] readtext = File.ReadAllLines(mylocation3);
                string cue_name;
                int i = 2;
                var lineCount = File.ReadLines(mylocation3).Count();
                while (i < lineCount) //the ordering seems bad, but it works.
                {
                    if (Convert.ToString(readtext[i]) == "")
                    {
                        // Monitor.Log("Blank space detected.");
                        break;
                    }
                    if (Convert.ToString(readtext[i]) == "\n")
                    {
                        // Monitor.Log("end line reached");
                        break;
                    }
                    cue_name = Convert.ToString(readtext[i]);
                    i++;
                    if (!reference_dic.Keys.Contains(cue_name))
                    {
                        locational_cues.Add(new_sound_bank.GetCue(cue_name));
                        reference_dic.Add(cue_name, this);
                    }
                    else
                    {
                        locational_cues.Add(new_sound_bank.GetCue(cue_name));
                    }
                }
                if (i == 2)
                {
                    // Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 + " this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");
                    return;
                }
                if (locational_cues.Count > 0)
                {
                    locational_rain_songs.Add(conditional_name, locational_cues);
                    Console.WriteLine("StardewSymohony:The music pack located at: " + path_loc + " has successfully processed the song info for the game location: " + conditional_name);
                    return;
                }
                else
                {
                    //  Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 + " this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");
                    return;
                }
            }
        }
        public void Music_Loader_Locations_Night(string conditional_name, Dictionary<string, MusicManager> reference_dic) //reads in cue names from a text file and adds them to a specific list. Morphs with specific conditional name. Conditionals are hardcoded.
        {
            locational_cues = new List<Cue>();
            //loads the data to the variables upon loading the game.
            var music_path = path_loc;
            string mylocation = Path.Combine(music_path, "Music_Files", "Locations", conditional_name);
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //check to make sure the file actually exists
            {
                Console.WriteLine("StardewSymphony:A music list for the location " + conditional_name + " does not exist for the music pack located at " + mylocation3 + " which isn't a problem, I just thought I'd let you know since this may have been intentional. Also I'm creating it for you just incase. Cheers.");
                //Console.WriteLine("Creating the Config file");
                string[] mystring3 = new string[3];//seems legit.
                mystring3[0] = conditional_name + " music file. This file holds all of the music that will play when at this game location. Simply type the name of the song below the wall of equal signs.";
                mystring3[1] = "========================================================================================";
                File.WriteAllLines(mylocation3, mystring3);
                return;
            }

            else
            {
                // add in data here
                string[] readtext = File.ReadAllLines(mylocation3);
                string cue_name;
                int i = 2;
                var lineCount = File.ReadLines(mylocation3).Count();

                while (i < lineCount) //the ordering seems bad, but it works.
                {
                    if (Convert.ToString(readtext[i]) == "")
                    {
                        //    Monitor.Log("Blank space detected.");
                        break;

                    }
                    if (Convert.ToString(readtext[i]) == "\n")
                    {
                        //Monitor.Log("end line reached");
                        break;

                    }
                    cue_name = Convert.ToString(readtext[i]);
                    i++;
                    if (!reference_dic.Keys.Contains(cue_name))
                    {
                        locational_cues.Add(new_sound_bank.GetCue(cue_name));
                        reference_dic.Add(cue_name, this);
                    }
                    else
                    {
                        locational_cues.Add(new_sound_bank.GetCue(cue_name));
                    }

                }
                if (i == 2)
                {
                    //  Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 + " this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");
                    return;
                }
                if (locational_cues.Count > 0)
                {
                    locational_night_songs.Add(conditional_name, locational_cues);
                    Console.WriteLine("StardewSymphonyLThe music pack located at: " + path_loc + " has successfully processed the song info for the game location: " + conditional_name);
                    return;
                }
                else
                {
                    //  Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 + " this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");
                    return;
                }
            }
        }
        public void Music_Loader_Locations_Rain_Night(string conditional_name, Dictionary<string, MusicManager> reference_dic) //reads in cue names from a text file and adds them to a specific list. Morphs with specific conditional name. Conditionals are hardcoded.
        {
            locational_cues = new List<Cue>();
            var music_path = path_loc;

            string mylocation = Path.Combine(music_path, "Music_Files", "Locations", conditional_name);
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //check to make sure the file actually exists
            {
                Console.WriteLine("StardewSymphony:A music list for the location " + conditional_name + " does not exist for the music pack located at " + mylocation3 + " which isn't a problem, I just thought I'd let you know since this may have been intentional. Also I'm creating it for you just incase. Cheers.");
                string[] mystring3 = new string[3];//seems legit.
                mystring3[0] = conditional_name + " music file. This file holds all of the music that will play when at this game location. Simply type the name of the song below the wall of equal signs.";
                mystring3[1] = "========================================================================================";

                File.WriteAllLines(mylocation3, mystring3);


                return;
            }

            else
            {
                //load in music stuff from the text files using the code below.
                string[] readtext = File.ReadAllLines(mylocation3);
                string cue_name;
                int i = 2;
                var lineCount = File.ReadLines(mylocation3).Count();

                while (i < lineCount) //the ordering seems bad, but it works.
                {
                    if (Convert.ToString(readtext[i]) == "") //if there is ever an empty line, stop processing the music file
                    {
                        break;
                    }
                    if (Convert.ToString(readtext[i]) == "\n")
                    {
                        break;
                    }
                    cue_name = Convert.ToString(readtext[i]);
                    i++;
                    if (!reference_dic.Keys.Contains(cue_name))
                    {
                        locational_cues.Add(new_sound_bank.GetCue(cue_name));
                        reference_dic.Add(cue_name, this);
                    }
                    else
                    {
                        locational_cues.Add(new_sound_bank.GetCue(cue_name));
                    }
                }
                if (i == 2)
                {
                    //   Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 + " this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");

                    return;
                }

                if (locational_cues.Count > 0)
                {
                    locational_rain_night_songs.Add(conditional_name, locational_cues);

                    Console.WriteLine("StardewSymohony:The music pack located at: " + path_loc + " has successfully processed the song info for the game location: " + conditional_name);
                    return;
                }
                else
                {
                    //  Monitor.Log("Just thought that I'd let you know that there are no songs associated with the music file located at " + mylocation3 + " this may be intentional, but just incase you were wanted music, now you knew which ones were blank.");
                    return;
                }

            }
        }

    };
}
