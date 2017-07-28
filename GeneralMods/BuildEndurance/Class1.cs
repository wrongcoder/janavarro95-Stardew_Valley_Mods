using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using StardewModdingAPI;

namespace Omegasis.BuildEndurance
{

    public class BuildEndurance : Mod
    {
        static bool exhausted_check = false;
        static bool collapse_check = false;
        static double BuildEndurance_data_xp_nextlvl = 20;
        static double BuildEndurance_data_xp_current = 0;
        static int BuildEndurance_data_current_lvl = 0;
        static int BuildEndurance_data_stam_bonus_acumulated = 0;
  
         static int BuildEndurance_data_ini_stam_bonus = 0;

        static bool BuildEndurance_data_clear_mod_effects = false;

       static int BuildEndurance_data_old_stamina = 0;

       static bool tool_cleaner = false;

       static bool fed = false;

        public Config ModConfig { get; set; }

        static bool upon_loading = false;


        static int nightly_stamina_value = 0;


        //Credit goes to Zoryn for pieces of this config generation that I kinda repurposed.
        public override void Entry(IModHelper helper)
        {
            StardewModdingAPI.Events.GameEvents.UpdateTick += EatingCallBack; //sloppy again but it'll do.

            StardewModdingAPI.Events.GameEvents.OneSecondTick += Tool_Cleanup;
            StardewModdingAPI.Events.GameEvents.UpdateTick += ToolCallBack;
            StardewModdingAPI.Events.SaveEvents.AfterLoad += LoadingCallBack;
            StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += SleepCallback;

            StardewModdingAPI.Events.GameEvents.UpdateTick += Exhaustion_callback;
            StardewModdingAPI.Events.GameEvents.UpdateTick += Collapse_Callback;


            var configLocation = Path.Combine(helper.DirectoryPath, "BuildEnduranceConfig.json");
            if (!File.Exists(configLocation))
            {
                Monitor.Log("Initial configuration file setup.");
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

                ModConfig.BuildEndurance_Exhaustion_XP = 25;
                ModConfig.BuildEndurance_Pass_Out_XP = 50;


                File.WriteAllBytes(configLocation, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ModConfig)));
            }
            else
            {
                ModConfig = JsonConvert.DeserializeObject<Config>(Encoding.UTF8.GetString(File.ReadAllBytes(configLocation)));
                Monitor.Log("Found BuildEndurance config file.");
            }

            // DataLoader();
            // MyWritter();  //hopefully loading these after the game is loaded will prevent wierd issues.

            Monitor.Log("BuildEndurance Initialization Completed");
        }


        public void ToolCallBack(object sender, EventArgs e) //ultra quick response for checking if a tool is used.
        {
            if (tool_cleaner == true) return;


            if (StardewValley.Game1.player.usingTool == true)
            {
             //   Console.WriteLine("Tool is being used");
                BuildEndurance_data_xp_current += ModConfig.BuildEndurance_xp_tooluse;
                //BuildEndurance_data_xp_current += 1000; For testing purposes
               // Log.Info(BuildEndurance_data_xp_current);

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
                 //Console.WriteLine("NOM NOM NOM22222222222");
                BuildEndurance_data_xp_current += ModConfig.BuildEndurance_xp_eating;
                fed = false;
            }


            return;
        }


        public void Exhaustion_callback(object sender, EventArgs e) //if the player is exhausted add xp.
        {

            if (exhausted_check == false)
            {
                if (StardewValley.Game1.player.exhausted)
                {
                    BuildEndurance_data_xp_current += ModConfig.BuildEndurance_Exhaustion_XP;
                    exhausted_check = true;
                    Monitor.Log("The player is exhausted");
                }
            }
            

        }
        


        public void Collapse_Callback(object sender, EventArgs e) //if the player stays up too late add some xp.
        {
            if (collapse_check == false)
            {

                if (StardewValley.Game1.farmerShouldPassOut == true)
                {

                    BuildEndurance_data_xp_current += ModConfig.BuildEndurance_Pass_Out_XP;
                    collapse_check = true;
                    Monitor.Log("The player has collapsed!");
                    return;
                }
            }
        }


        public void LoadingCallBack(object sender, EventArgs e)
        {
            
                DataLoader();
                MyWritter();
                upon_loading = true;
                //runs when the player is loaded.

                var player = StardewValley.Game1.player;

                if (BuildEndurance_data_old_stamina == 0)
                {
                    BuildEndurance_data_old_stamina = player.MaxStamina; //grab the initial stamina value
                }


                player.MaxStamina = nightly_stamina_value;

                if (nightly_stamina_value == 0)
                {
                    player.MaxStamina = BuildEndurance_data_ini_stam_bonus + BuildEndurance_data_stam_bonus_acumulated + BuildEndurance_data_old_stamina; //incase the ini stam bonus is loaded in. 
                }
                if (BuildEndurance_data_clear_mod_effects == true)
                {
                    player.MaxStamina = BuildEndurance_data_old_stamina;
                   // Console.WriteLine("BuildEndurance Reset!");
                }

                DataLoader();
                MyWritter();
        }

        public void SleepCallback(object sender, EventArgs e)
        {
            //This will run when the character goes to sleep. It will increase their sleeping skill.
            exhausted_check = false;
            collapse_check = false;
            if (upon_loading == true)
            {

                //Log.Info("THIS IS MY NEW DAY CALL BACK XP version 1");
                Monitor.Log(BuildEndurance_data_xp_current.ToString());

                Clear_Checker();
                Monitor.Log(BuildEndurance_data_clear_mod_effects.ToString());

                var player = StardewValley.Game1.player;

                BuildEndurance_data_xp_current += ModConfig.BuildEndurance_xp_sleeping;

                if (BuildEndurance_data_old_stamina == 0)
                {
                    BuildEndurance_data_old_stamina = player.MaxStamina; //grab the initial stamina value
                }

                if (BuildEndurance_data_clear_mod_effects == true)
                {
                    Clear_DataLoader();
                    player.MaxStamina = BuildEndurance_data_old_stamina;
                    BuildEndurance_data_xp_nextlvl = ModConfig.BuildEndurance_xp_nextlvl;
                    BuildEndurance_data_xp_current = ModConfig.BuildEndurance_xp_current;
                    BuildEndurance_data_stam_bonus_acumulated = 0;
                    BuildEndurance_data_old_stamina = player.MaxStamina;
                    BuildEndurance_data_ini_stam_bonus = 0;
                    BuildEndurance_data_current_lvl = 0;
                   
                    //because this doesn't work propperly at first anyways.
                  //  Console.WriteLine("BuildEndurance Reset!");
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

                        }
                    }
                }
                BuildEndurance_data_clear_mod_effects = false;
                nightly_stamina_value = StardewValley.Game1.player.maxStamina;
                MyWritter();
            }
            //else Log.Info("Safely Loading.");
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

         public int   BuildEndurance_Exhaustion_XP { get; set; }
         public int       BuildEndurance_Pass_Out_XP { get; set; }

        }


        void Clear_DataLoader()
        {
            DataLoader();
            MyWritter();
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData"))) Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Helper.DirectoryPath, "PlayerData", "BuildEndurance_data_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
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

        void Clear_Checker()
        {

            //loads the data to the variables upon loading the game.
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData"))) Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Helper.DirectoryPath, "PlayerData", "BuildEndurance_data_"); ;
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
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
                BuildEndurance_data_clear_mod_effects = Convert.ToBoolean(readtext[14]);


            }
        }



        void DataLoader()
        {
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData"))) Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Helper.DirectoryPath,"PlayerData", "BuildEndurance_data_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                Console.WriteLine("DataLoading");
                BuildEndurance_data_xp_nextlvl = ModConfig.BuildEndurance_xp_nextlvl;
                BuildEndurance_data_xp_current = ModConfig.BuildEndurance_xp_current;
                BuildEndurance_data_current_lvl = ModConfig.BuildEndurance_current_lvl;
                BuildEndurance_data_ini_stam_bonus = ModConfig.BuildEndurance_ini_stam_boost;
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
                nightly_stamina_value = Convert.ToInt32(readtext[18]); //this should grab the nightly stamina values
            }
        }

        void MyWritter()
        {
            //saves the BuildEndurance_data at the end of a new day;
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData"))) Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Helper.DirectoryPath,"PlayerData", "BuildEndurance_data_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            string[] mystring3 = new string[20];
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

                mystring3[17] = "Nightly Stamina Value: This is the value of the player's stamina that was saved when the player slept.";
                mystring3[18] = nightly_stamina_value.ToString(); //this should save the player's stamina upon sleeping.


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

                mystring3[17] = "Nightly Stamina Value: This is the value of the player's stamina that was saved when the player slept.";
                mystring3[18] = nightly_stamina_value.ToString();

                File.WriteAllLines(mylocation3, mystring3);
            }
        }

    } //end my function
}