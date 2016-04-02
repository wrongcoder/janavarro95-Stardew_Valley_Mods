using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;
using Microsoft.Xna.Framework;
using System.Windows;
namespace BuildEndurance
{

    public class BuildEndurance : Mod
    {
        public static double BuildEndurance_data_xp_nextlvl=20;
        public static double BuildEndurance_data_xp_current=0;

        public static int BuildEndurance_data_current_lvl=0;

        public static int BuildEndurance_data_stam_bonus_acumulated=0;

        public static int BuildEndurance_data_ini_stam_bonus=0;

        public static bool BuildEndurance_data_clear_mod_effects = false;

        public static int BuildEndurance_data_old_stamina = 0;

        public static bool tool_cleaner = false;

        public static bool fed = false;

        public Config ModConfig { get; set; }

        public static bool upon_loading = false;

        //Credit goes to Zoryn for pieces of this config generation that I kinda repurposed.
        public override void Entry(params object[] objects)
        {
            Log.Info("HEYO WORLD");
            
            StardewModdingAPI.Events.GameEvents.UpdateTick += EatingCallBack; //sloppy again but it'll do.

            StardewModdingAPI.Events.GameEvents.OneSecondTick += Tool_Cleanup;
              StardewModdingAPI.Events.GameEvents.UpdateTick += ToolCallBack;
            StardewModdingAPI.Events.PlayerEvents.LoadedGame += LoadingCallBack;
            StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += SleepCallback;

            var configLocation = Path.Combine(PathOnDisk, "BuildEnduranceConfig.json");
            if (!File.Exists(configLocation))
            {
                Console.WriteLine("Initial configuration file setup.");
                ModConfig = new Config();

                ModConfig.BuildEndurance_current_lvl = 0;
                ModConfig.BuildEndurance_max_lvl = 100;

                ModConfig.BuildEndurance_stam_increase_upon_lvl_up = 1;

                ModConfig.BuildEndurance_xp_current = 0;
                ModConfig.BuildEndurance_xp_nextlvl = 20;
                ModConfig.BuildEndurance_xp_curve = 1.15;

                ModConfig.BuildEndurance_xp_eating = 2;
                ModConfig.BuildEndurance_xp_sleeping = 10;
                ModConfig.BuildEndurance_xp_tooluse = 1;

                ModConfig.BuildEndurance_ini_stam_boost = 0;

                ModConfig.BuildEndurance_stam_accumulated = 0;

                File.WriteAllBytes(configLocation, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ModConfig)));
            }
            else
            {
                ModConfig = JsonConvert.DeserializeObject<Config>(Encoding.UTF8.GetString(File.ReadAllBytes(configLocation)));
                Console.WriteLine("Found BuildEndurance config file.");
            }
            
           // DataLoader();
           // MyWritter();  //hopefully loading these after the game is loaded will prevent wierd issues.
            
            Console.WriteLine("BuildEndurance Initialization Completed");
        }
    


        public void ToolCallBack(object sender, EventArgs e) //ultra quick response for checking if a tool is used.
        {
            if (tool_cleaner == true) return;


            if (StardewModdingAPI.Entities.SPlayer.CurrentFarmer.usingTool == true)
            {
                //Console.WriteLine("Tool is being used");
                BuildEndurance_data_xp_current += ModConfig.BuildEndurance_xp_tooluse;
                tool_cleaner = true;
            }
            else return;
        }

        public void Tool_Cleanup(object sender, EventArgs e) //nerfs how quickly xp is actually gained. I hope.
        {

            if (tool_cleaner == true) tool_cleaner = false;
            else return;
        }

        public void EatingCallBack(object sender, EventArgs e)
        {
           

            if (StardewValley.Game1.isEating == true)
            {               
               // Console.WriteLine("NOM NOM NOM");
                fed = true;

                //this code will run when the player eats an object. I.E. increases their eating skills.
            }
            //I'm going to assume they ate the food.
            if ((StardewValley.Game1.isEating == false) && fed == true)
            {
               // Console.WriteLine("NOM NOM NOM");
                BuildEndurance_data_xp_current += ModConfig.BuildEndurance_xp_eating;
                fed = false;
            }
            

            return;
        }

       
    

        public void LoadingCallBack(object sender, EventArgs e)
        {

         //   Log.Info("GamessssssssLoaded");
           // Console.WriteLine("entering loading callback");
            if (StardewModdingAPI.Inheritance.SGame.hasLoadedGame == true)
            {
                Log.Info("CharacterLoaded");
                //   Console.WriteLine("Penetrated loading callback");
                //Log.Info(StardewValley.Game1.player.name);
                DataLoader();
                MyWritter();
                upon_loading = true;
                Log.Info("writers passed");
                //runs when the player is loaded.


                var player = StardewValley.Game1.player;
                
                if (BuildEndurance_data_old_stamina == 0)
                {
                    BuildEndurance_data_old_stamina = player.MaxStamina; //grab the initial stamina value
                }

                player.MaxStamina = BuildEndurance_data_ini_stam_bonus + BuildEndurance_data_stam_bonus_acumulated + BuildEndurance_data_old_stamina; //incase the ini stam bonus is loaded in. 

                if (BuildEndurance_data_clear_mod_effects == true)
                {
                    player.MaxStamina = BuildEndurance_data_old_stamina;
                    Console.WriteLine("BuildEndurance Reset!");
                }

                DataLoader();
                MyWritter();
            }

        }

        public void SleepCallback(object sender, EventArgs e)
        {
            Log.Info("SLEEP CALLBACK");
           
            Log.Info("CLEAR DATA PASSED");
            //This will run when the character goes to sleep. It will increase their sleeping skill.
            //Console.WriteLine("Is this being hit?");

            if (upon_loading == true)
            {
                Clear_DataLoader();
                //because this doesn't work propperly at first anyways.

                //return;



                var player = StardewValley.Game1.player;

                BuildEndurance_data_xp_current += ModConfig.BuildEndurance_xp_sleeping;

                if (BuildEndurance_data_old_stamina == 0)
                {
                    BuildEndurance_data_old_stamina = player.MaxStamina; //grab the initial stamina value
                }

                if (BuildEndurance_data_clear_mod_effects == true)
                {
                    player.MaxStamina = BuildEndurance_data_old_stamina;
                    BuildEndurance_data_xp_nextlvl = ModConfig.BuildEndurance_xp_nextlvl;
                    BuildEndurance_data_xp_current = ModConfig.BuildEndurance_xp_current;
                    BuildEndurance_data_stam_bonus_acumulated = 0;
                    BuildEndurance_data_old_stamina = player.MaxStamina;
                    BuildEndurance_data_ini_stam_bonus = 0;
                    BuildEndurance_data_current_lvl = 0;
                    Console.WriteLine("BuildEndurance Reset!");
                }


                if (BuildEndurance_data_clear_mod_effects == false)
                {
                    if (BuildEndurance_data_current_lvl < ModConfig.BuildEndurance_max_lvl)
                    {
                        while (BuildEndurance_data_xp_current >= BuildEndurance_data_xp_nextlvl)
                        {
                            BuildEndurance_data_current_lvl += 1;
                            BuildEndurance_data_xp_current = BuildEndurance_data_xp_current - BuildEndurance_data_xp_nextlvl;
                            BuildEndurance_data_xp_nextlvl = (ModConfig.BuildEndurance_xp_curve * BuildEndurance_data_xp_nextlvl);
                            player.MaxStamina += ModConfig.BuildEndurance_stam_increase_upon_lvl_up;
                            BuildEndurance_data_stam_bonus_acumulated += ModConfig.BuildEndurance_stam_increase_upon_lvl_up;
                            Log.Info("IF YOU SEE THIS TOO MUCH THIS IS AN INFINITE LOOP. CRAP");
                        }

                        /*
                            if (player.MaxStamina != BuildEndurance_data_old_stamina + BuildEndurance_data_stam_bonus_acumulated + BuildEndurance_data_ini_stam_bonus)
                        {
                            player.MaxStamina = BuildEndurance_data_old_stamina + BuildEndurance_data_stam_bonus_acumulated + BuildEndurance_data_ini_stam_bonus;
                        }
                        */


                    }
                }
                BuildEndurance_data_clear_mod_effects = false;

                MyWritter();
            }
            else Log.Info("Lazy programming");
        }


        //Mod config data.
        public class Config
        {
            public double BuildEndurance_xp_nextlvl { get; set; }
            public double BuildEndurance_xp_current { get; set; }
            public double BuildEndurance_xp_curve { get; set; }

            public int BuildEndurance_current_lvl { get; set; }
            public int BuildEndurance_max_lvl { get; set; }

            public int BuildEndurance_stam_increase_upon_lvl_up { get; set; }

            public int BuildEndurance_xp_tooluse { get; set; }
            public int BuildEndurance_xp_eating { get; set; }
            public int BuildEndurance_xp_sleeping { get; set; }

            public int BuildEndurance_ini_stam_boost { get; set; }

            public int BuildEndurance_stam_accumulated { get; set; }
          
        }


        void Clear_DataLoader()
        {
            DataLoader();
            MyWritter();
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(PathOnDisk, "BuildEndurance_data_");
           string mylocation2 = mylocation+myname;
           string mylocation3 = mylocation2+".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                Console.WriteLine("Clear Data Loaded could not find the correct file.");


                BuildEndurance_data_clear_mod_effects = false;
                BuildEndurance_data_old_stamina = 0;
                BuildEndurance_data_ini_stam_bonus = 0;
                //return;
            }

            else
            {
                //loads the BuildEndurance_data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation3);
                BuildEndurance_data_ini_stam_bonus = Convert.ToInt32(readtext[9]);
                BuildEndurance_data_clear_mod_effects = Convert.ToBoolean(readtext[14]);
                BuildEndurance_data_old_stamina = Convert.ToInt32(readtext[16]);

            }
        }




        void DataLoader()
        {
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(PathOnDisk, "BuildEndurance_data_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                Console.WriteLine("DataLoading");
                BuildEndurance_data_xp_nextlvl = ModConfig.BuildEndurance_xp_nextlvl;
                BuildEndurance_data_xp_current = ModConfig.BuildEndurance_xp_current;
                BuildEndurance_data_current_lvl = ModConfig.BuildEndurance_current_lvl;
                BuildEndurance_data_ini_stam_bonus =  ModConfig.BuildEndurance_ini_stam_boost;
                BuildEndurance_data_stam_bonus_acumulated = ModConfig.BuildEndurance_stam_accumulated;
                BuildEndurance_data_clear_mod_effects = false;
                BuildEndurance_data_old_stamina = 0;

            }

            else
            {
        //        Console.WriteLine("HEY THERE IM LOADING DATA");

                //loads the BuildEndurance_data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation3);
                BuildEndurance_data_current_lvl = Convert.ToInt32(readtext[3]);
                BuildEndurance_data_xp_nextlvl = Convert.ToDouble(readtext[7]);  //these array locations refer to the lines in BuildEndurance_data.json
                BuildEndurance_data_xp_current = Convert.ToDouble(readtext[5]);
                BuildEndurance_data_ini_stam_bonus = Convert.ToInt32(readtext[9]);
               BuildEndurance_data_stam_bonus_acumulated = Convert.ToInt32(readtext[11]);
                BuildEndurance_data_clear_mod_effects = Convert.ToBoolean(readtext[14]);
                BuildEndurance_data_old_stamina = Convert.ToInt32(readtext[16]);
                
            }
        }

        void MyWritter()
        {
            //saves the BuildEndurance_data at the end of a new day;
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(PathOnDisk, "BuildEndurance_data_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            string[] mystring3= new string[20];
            if (!File.Exists(mylocation3))
            {
                Console.WriteLine("The data file for BuildEndurance was not found, guess I'll create it when you sleep.");

                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Player: Build Endurance Data. Modification can cause errors. Edit at your own risk.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player Current Level:";
                mystring3[3] = BuildEndurance_data_current_lvl.ToString();

                mystring3[4] = "Player Current XP:";
                mystring3[5] = BuildEndurance_data_xp_current.ToString();

                mystring3[6] = "Xp to next Level:";
                mystring3[7] = BuildEndurance_data_xp_nextlvl.ToString();

                mystring3[8] = "Initial Stam Bonus:";
                mystring3[9] = BuildEndurance_data_ini_stam_bonus.ToString();

                mystring3[10] = "Additional Stam Bonus:";
                mystring3[11] = BuildEndurance_data_stam_bonus_acumulated.ToString();

                mystring3[12] = "=======================================================================================";
                mystring3[13] = "RESET ALL MOD EFFECTS? This will effective start you back at square 1. Also good if you want to remove this mod.";
                mystring3[14] = BuildEndurance_data_clear_mod_effects.ToString();
                mystring3[15] = "OLD STAMINA AMOUNT: This is the initial value of the Player's Stamina before this mod took over.";
                mystring3[16] = BuildEndurance_data_old_stamina.ToString();

                File.WriteAllLines(mylocation3, mystring3);
            }
        
            else
            {
            //    Console.WriteLine("HEY IM SAVING DATA");

                //write out the info to a text file at the end of a day.
                mystring3[0] = "Player: Build Endurance Data. Modification can cause errors. Edit at your own risk.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player Current Level:";
                mystring3[3] = BuildEndurance_data_current_lvl.ToString();

                mystring3[4] = "Player Current XP:";
                mystring3[5] = BuildEndurance_data_xp_current.ToString();

                mystring3[6] = "Xp to next Level:";
                mystring3[7] = BuildEndurance_data_xp_nextlvl.ToString();

                mystring3[8] = "Initial Stam Bonus:";
                mystring3[9] = BuildEndurance_data_ini_stam_bonus.ToString();

                mystring3[10] = "Additional Stam Bonus:";
                mystring3[11] = BuildEndurance_data_stam_bonus_acumulated.ToString();

                mystring3[12] = "=======================================================================================";
                mystring3[13] = "RESET ALL MOD EFFECTS? This will effective start you back at square 1. Also good if you want to remove this mod.";
                mystring3[14] = BuildEndurance_data_clear_mod_effects.ToString();
                mystring3[15] = "OLD STAMINA AMOUNT: This is the initial value of the Player's Stamina before this mod took over.";
                mystring3[16] = BuildEndurance_data_old_stamina.ToString();


                File.WriteAllLines(mylocation3, mystring3);
            }
        }

    } //end my function
}