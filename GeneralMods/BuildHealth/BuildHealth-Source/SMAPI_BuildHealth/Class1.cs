using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;
using Microsoft.Xna.Framework;

namespace BuildHealth
{

    public class BuildHealth : Mod
    {
        public double BuildHealth_data_xp_nextlvl=20;
        public double BuildHealth_data_xp_current=0;

        public int BuildHealth_data_current_lvl=0;

        public int BuildHealth_data_health_bonus_acumulated=0;

        public int BuildHealth_data_ini_health_bonus=0;

        public bool BuildHealth_data_clear_mod_effects = false;

        public int BuildHealth_data_old_health = 0;

        public bool tool_cleaner = false;

        public bool fed = false;


        public int old_health;

        public int new_health;



        public Config ModConfig { get; set; }

		public static bool upon_loading = false;

        public bool collapse_check;

        //Credit goes to Zoryn for pieces of this config generation that I kinda repurposed.
        public override void Entry(IModHelper helper)
        {

            StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += SleepCallback;
            StardewModdingAPI.Events.GameEvents.UpdateTick += EatingCallBack; //sloppy again but it'll do.

            StardewModdingAPI.Events.GameEvents.OneSecondTick += Tool_Cleanup;
            StardewModdingAPI.Events.GameEvents.UpdateTick += ToolCallBack;
            StardewModdingAPI.Events.SaveEvents.AfterLoad += LoadingCallBack;
            StardewModdingAPI.Events.GameEvents.UpdateTick += Collapse_Callback;

            StardewModdingAPI.Events.GameEvents.UpdateTick += damage_check;

            var configLocation = Path.Combine(helper.DirectoryPath, "BuildHealthConfig.json");
            if (!File.Exists(configLocation))
            {
                Monitor.Log("The config file for BuildHealth was not found, guess I'll create it...");
                ModConfig = new Config();

                ModConfig.BuildHealth_current_lvl = 0;
                ModConfig.BuildHealth_max_lvl = 100;

                ModConfig.BuildHealth_Health_increase_upon_lvl_up = 1;

                ModConfig.BuildHealth_xp_current = 0;
                ModConfig.BuildHealth_xp_nextlvl = 20;
                ModConfig.BuildHealth_xp_curve = 1.15;

                ModConfig.BuildHealth_xp_eating = 2;
                ModConfig.BuildHealth_xp_sleeping = 10;
                ModConfig.BuildHealth_xp_tooluse = 1;

                ModConfig.BuildHealth_ini_Health_boost = 0;

                ModConfig.BuildHealth_Health_accumulated = 0;

                File.WriteAllBytes(configLocation, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ModConfig)));
            }
            else
            {
                ModConfig = JsonConvert.DeserializeObject<Config>(Encoding.UTF8.GetString(File.ReadAllBytes(configLocation)));
                Monitor.Log("Found BuildHealth config file.");
            }

         //   DataLoader();
         //   MyWritter();
            Monitor.Log("BuildHealth Initialization Completed");
        }



        public void ToolCallBack(object sender, EventArgs e) //ultra quick response for checking if a tool is used.
        {
            if (tool_cleaner == true) return;


            if (StardewValley.Game1.player.usingTool == true)
            {
                //Monitor.Log("Tool is being used");
                BuildHealth_data_xp_current += ModConfig.BuildHealth_xp_tooluse;
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
                // Monitor.Log("NOM NOM NOM");
                fed = true;

                //this code will run when the player eats an object. I.E. increases their eating skills.
            }
            //I'm going to assume they ate the food.
            if ((StardewValley.Game1.isEating == false) && fed == true)
            {
                // Monitor.Log("NOM NOM NOM");
                BuildHealth_data_xp_current += ModConfig.BuildHealth_xp_eating;
                fed = false;
            }


            return;
        }



        public void damage_check(object sender, EventArgs e)
        {
            var player = StardewValley.Game1.player;

            if (old_health > player.health)
            {
                BuildHealth_data_xp_current += (old_health - player.health);
                //Log.Info(old_health - player.health);
                old_health = (player.health);
                
            }
            if (old_health < player.health)
            {
                old_health = player.health;
            }
           


            return;
        }




        public void SleepCallback(object sender, EventArgs e)
        {
            collapse_check = false;
            if (upon_loading ==true){

                Clear_Checker();

            var player = StardewValley.Game1.player;

            BuildHealth_data_xp_current += ModConfig.BuildHealth_xp_sleeping;

            if (BuildHealth_data_old_health == 0)
            {
                BuildHealth_data_old_health = player.maxHealth; //grab the initial Health value
            }

            if (BuildHealth_data_clear_mod_effects == true)
            {
                    Clear_DataLoader();
                    //This will run when the character goes to sleep. It will increase their sleeping skill.
                    player.maxHealth = BuildHealth_data_old_health;
                BuildHealth_data_xp_nextlvl = ModConfig.BuildHealth_xp_nextlvl;
                BuildHealth_data_xp_current = ModConfig.BuildHealth_xp_current;
                BuildHealth_data_health_bonus_acumulated = 0;
                BuildHealth_data_old_health = player.maxHealth;
                BuildHealth_data_ini_health_bonus = 0;
                BuildHealth_data_current_lvl = 0;
               Monitor.Log("BuildHealth Reset!");
            }


            if (BuildHealth_data_clear_mod_effects == false)
            {
                if (BuildHealth_data_current_lvl < ModConfig.BuildHealth_max_lvl)
                {
                    while (BuildHealth_data_xp_current >= BuildHealth_data_xp_nextlvl)
                    {
                        BuildHealth_data_current_lvl += 1;
                        BuildHealth_data_xp_current = BuildHealth_data_xp_current - BuildHealth_data_xp_nextlvl;
                        BuildHealth_data_xp_nextlvl = (ModConfig.BuildHealth_xp_curve * BuildHealth_data_xp_nextlvl);
                        player.maxHealth += ModConfig.BuildHealth_Health_increase_upon_lvl_up;
                        BuildHealth_data_health_bonus_acumulated += ModConfig.BuildHealth_Health_increase_upon_lvl_up;
                    }


                }
            }
            BuildHealth_data_clear_mod_effects = false;

            MyWritter();
        }

            old_health = StardewValley.Game1.player.maxHealth;


        }


        public void LoadingCallBack(object sender, EventArgs e)
        {
                DataLoader();
                MyWritter();
				upon_loading=true;
                //runs when the player is loaded.
                var player = StardewValley.Game1.player;

                if (BuildHealth_data_old_health == 0)
                {
                    BuildHealth_data_old_health = player.maxHealth; //grab the initial health value
                }

                player.maxHealth = BuildHealth_data_ini_health_bonus + BuildHealth_data_health_bonus_acumulated + BuildHealth_data_old_health; //incase the ini stam bonus is loaded in. 

                if (BuildHealth_data_clear_mod_effects == true)
                {
                    player.maxHealth = BuildHealth_data_old_health;
                    Monitor.Log("BuildHealth Reset!");
                }

                DataLoader();
                MyWritter();
            
            old_health = StardewValley.Game1.player.maxHealth;
        }


        public void Collapse_Callback(object sender, EventArgs e) //if the player stays up too late add some xp.
        {
            if (collapse_check == false)
            {

                if (StardewValley.Game1.farmerShouldPassOut == true)
                {

                    BuildHealth_data_xp_current += ModConfig.BuildHealth_Pass_Out_XP;
                    collapse_check = true;
                    Monitor.Log("The player has collapsed!");
                    return;
                }
            }
        }


        //Mod config data.
        public class Config
        {
            public double BuildHealth_xp_nextlvl { get; set; }
            public double BuildHealth_xp_current { get; set; }
            public double BuildHealth_xp_curve { get; set; }

            public int BuildHealth_current_lvl { get; set; }
            public int BuildHealth_max_lvl { get; set; }

            public int BuildHealth_Health_increase_upon_lvl_up { get; set; }

            public int BuildHealth_xp_tooluse { get; set; }
            public int BuildHealth_xp_eating { get; set; }
            public int BuildHealth_xp_sleeping { get; set; }

            public int BuildHealth_ini_Health_boost { get; set; }

            public int BuildHealth_Health_accumulated { get; set; }

            public int BuildHealth_Pass_Out_XP { get; set; }

        }


        void Clear_DataLoader()
        {
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData"))) Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Helper.DirectoryPath, "PlayerData", "BuildHealth_data_");
            string mylocation2 = mylocation+myname;
           string mylocation3 = mylocation2+".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                Monitor.Log("The config file for BuildHealth was not found, guess I'll create it...");


                BuildHealth_data_clear_mod_effects = false;
                BuildHealth_data_old_health = 0;
                BuildHealth_data_ini_health_bonus = 0;
            }

            else
            {
                //loads the BuildHealth_data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation3);
                BuildHealth_data_ini_health_bonus = Convert.ToInt32(readtext[9]);
                BuildHealth_data_clear_mod_effects = Convert.ToBoolean(readtext[14]);
                BuildHealth_data_old_health = Convert.ToInt32(readtext[16]);

            }
        }

        void Clear_Checker()
        {
            //loads the data to the variables upon loading the game.
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData"))) Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Helper.DirectoryPath, "PlayerData", "BuildHealth_data_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                Monitor.Log("The config file for BuildHealth was not found, guess I'll create it...");


                BuildHealth_data_clear_mod_effects = false;
                BuildHealth_data_old_health = 0;
                BuildHealth_data_ini_health_bonus = 0;
            }

            else
            {
                //loads the BuildHealth_data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation3);
                BuildHealth_data_clear_mod_effects = Convert.ToBoolean(readtext[14]);

            }
        }


        void DataLoader()
        {
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData"))) Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Helper.DirectoryPath, "PlayerData", "BuildHealth_data_");
            string mylocation2 = mylocation+myname;
           string mylocation3 = mylocation2+".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                Monitor.Log("The config file for BuildHealth was not found, guess I'll create it...");
                BuildHealth_data_xp_nextlvl = ModConfig.BuildHealth_xp_nextlvl;
                BuildHealth_data_xp_current = ModConfig.BuildHealth_xp_current;
                BuildHealth_data_current_lvl = ModConfig.BuildHealth_current_lvl;
                BuildHealth_data_ini_health_bonus = ModConfig.BuildHealth_ini_Health_boost;
                BuildHealth_data_health_bonus_acumulated = ModConfig.BuildHealth_Health_accumulated;
                BuildHealth_data_clear_mod_effects = false;
                BuildHealth_data_old_health = 0;

            }

            else
            {
                //        Monitor.Log("HEY THERE IM LOADING DATA");

                //loads the BuildHealth_data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation3);
                BuildHealth_data_current_lvl = Convert.ToInt32(readtext[3]);
                BuildHealth_data_xp_nextlvl = Convert.ToDouble(readtext[7]);  //these array locations refer to the lines in BuildHealth_data.json
                BuildHealth_data_xp_current = Convert.ToDouble(readtext[5]);
                BuildHealth_data_ini_health_bonus = Convert.ToInt32(readtext[9]);
                BuildHealth_data_health_bonus_acumulated = Convert.ToInt32(readtext[11]);
                BuildHealth_data_clear_mod_effects = Convert.ToBoolean(readtext[14]);
                BuildHealth_data_old_health = Convert.ToInt32(readtext[16]);

            }
        }

        void MyWritter()
        {
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData"))) Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Helper.DirectoryPath, "PlayerData", "BuildHealth_data_");
            string mylocation2 = mylocation+myname;
           string mylocation3 = mylocation2+".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
                Monitor.Log("The data file for BuildHealth was not found, guess I'll create it when you sleep.");

                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Player: Build Health Data. Modification can cause errors. Edit at your own risk.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player Current Level:";
                mystring3[3] = BuildHealth_data_current_lvl.ToString();

                mystring3[4] = "Player Current XP:";
                mystring3[5] = BuildHealth_data_xp_current.ToString();

                mystring3[6] = "Xp to next Level:";
                mystring3[7] = BuildHealth_data_xp_nextlvl.ToString();

                mystring3[8] = "Initial Health Bonus:";
                mystring3[9] = BuildHealth_data_ini_health_bonus.ToString();

                mystring3[10] = "Additional Health Bonus:";
                mystring3[11] = BuildHealth_data_health_bonus_acumulated.ToString();

                mystring3[12] = "=======================================================================================";
                mystring3[13] = "RESET ALL MOD EFFECTS? This will effective start you back at square 1. Also good if you want to remove this mod.";
                mystring3[14] = BuildHealth_data_clear_mod_effects.ToString();
                mystring3[15] = "OLD Health AMOUNT: This is the initial value of the Player's Health before this mod took over.";
                mystring3[16] = BuildHealth_data_old_health.ToString();


                File.WriteAllLines(mylocation3, mystring3);
            }

            else
            {
                //    Monitor.Log("HEY IM SAVING DATA");

                //write out the info to a text file at the end of a day.
                mystring3[0] = "Player: Build Health Data. Modification can cause errors. Edit at your own risk.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player Current Level:";
                mystring3[3] = BuildHealth_data_current_lvl.ToString();

                mystring3[4] = "Player Current XP:";
                mystring3[5] = BuildHealth_data_xp_current.ToString();

                mystring3[6] = "Xp to next Level:";
                mystring3[7] = BuildHealth_data_xp_nextlvl.ToString();

                mystring3[8] = "Initial Health Bonus:";
                mystring3[9] = BuildHealth_data_ini_health_bonus.ToString();

                mystring3[10] = "Additional Health Bonus:";
                mystring3[11] = BuildHealth_data_health_bonus_acumulated.ToString();

                mystring3[12] = "=======================================================================================";
                mystring3[13] = "RESET ALL MOD EFFECTS? This will effective start you back at square 1. Also good if you want to remove this mod.";
                mystring3[14] = BuildHealth_data_clear_mod_effects.ToString();
                mystring3[15] = "OLD Health AMOUNT: This is the initial value of the Player's Health before this mod took over.";
                mystring3[16] = BuildHealth_data_old_health.ToString();


                File.WriteAllLines(mylocation3, mystring3);
            }
        }

    } //end my function
}