using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Storm.ExternalEvent;
using Storm.StardewValley;
using Storm.StardewValley.Event;
using Storm.StardewValley.Wrapper;
using Storm;
using Microsoft.Xna.Framework;
namespace BuildHealth
{
    [Mod]
    public class BuildHealth : DiskResource
    {
        public double data_xp_nextlvl;
        public double data_xp_current;

        public int data_current_lvl;

        public int data_health_bonus_acumulated;

        public int data_ini_health_bonus;

        public bool data_clear_mod_effects = false;

        public int data_old_health = 0;

        public Config ModConfig { get; set; }

        [Subscribe]
        //Credit goes to Zoryn for pieces of this config generation that I kinda repurposed.
        public void InitializeCallback(InitializeEvent @event)
        {
            var configLocation = Path.Combine(PathOnDisk, "BuildHealth_Config.json");
            if (!File.Exists(configLocation))
            {
                Logging.LogToFile("The config file for BuildHealth was not found, guess I'll create it...");
                ModConfig = new Config();

                ModConfig.current_lvl = 0;
                ModConfig.max_lvl = 100;

                ModConfig.health_increase_upon_lvl_up = 1;

                ModConfig.xp_current = 0;
                ModConfig.xp_nextlvl = 20;
                ModConfig.xp_curve = 1.15;

                ModConfig.xp_eating = 2;
                ModConfig.xp_sleeping = 10;
                ModConfig.xp_tooluse = 1;

                ModConfig.ini_health_boost = 0;

                ModConfig.health_accumulated = 0;

                File.WriteAllBytes(configLocation, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ModConfig)));
                // Console.WriteLine("The config file for MoreRain has been loaded.\n\t RainChance: {0}, ThunderChance: {1}",
                //     ModConfig.RainChance, ModConfig.ThunderChance);
            }
            else
            {
                ModConfig = JsonConvert.DeserializeObject<Config>(Encoding.UTF8.GetString(File.ReadAllBytes(configLocation)));
                Logging.LogToFile("Found BuildHealth config file.");
            }

            DataLoader();
            MyWritter();
            Logging.LogToFile("BuildHealth Initialization Completed");
        }

        [Subscribe]

        public void ToolCallBack(Storm.StardewValley.Event.ReleaseUseToolButtonEvent @event)
        {
            //this will be the code that runs when a tool is used
            //Logging.LogToFile("A TOOL IS BEING USED");
            data_xp_current += ModConfig.xp_tooluse;
        }
        [Subscribe]
        public void EatingCallBack(Storm.StardewValley.Event.PlayerEatObjectEvent @event)
        {
            //this code will run when the player eats an object. I.E. increases their eating skills.
            //Logging.LogToFile("A FOOD IS BEING CONSUMED");
            data_xp_current += ModConfig.xp_eating;
        }

        [Subscribe]
        public void SleepCallback(Storm.StardewValley.Event.BeforeNewDayEvent @event)
        {
            Clear_DataLoader();
            //This will run when the character goes to sleep. It will increase their sleeping skill.
            var player = @event.Root.Player;

            data_xp_current += ModConfig.xp_sleeping;

            if (data_old_health == 0)
            {
                data_old_health = player.MaxHealth; //grab the initial health value
            }

            if (data_clear_mod_effects == true)
            {
                player.MaxHealth = data_old_health;
                data_xp_nextlvl = 0;
                data_xp_current = 0;
                data_health_bonus_acumulated = 0;
                data_old_health = 0;
                data_ini_health_bonus = 0;
                data_current_lvl = 0;
                Logging.LogToFile("BuildHealth Reset!");
            }


            if (data_clear_mod_effects == false)
            {
                if (data_current_lvl < ModConfig.max_lvl)
                {
                    while (data_xp_current >= data_xp_nextlvl)
                    {
                        data_current_lvl += 1;
                        data_xp_current = data_xp_current - data_xp_nextlvl;
                        data_xp_nextlvl = (ModConfig.xp_curve * data_xp_nextlvl);
                        player.MaxHealth += ModConfig.health_increase_upon_lvl_up;
                        data_health_bonus_acumulated += ModConfig.health_increase_upon_lvl_up;
                    }
                    if (player.MaxHealth != data_old_health + data_health_bonus_acumulated + data_ini_health_bonus)
                    {
                        player.MaxHealth = data_old_health + data_health_bonus_acumulated + data_ini_health_bonus;
                    }


                }
            }
            data_clear_mod_effects = false;

            MyWritter();
        }

        [Subscribe]
        public void LoadingCallBack(Storm.StardewValley.Event.AfterGameLoadedEvent @event)
        {
            DataLoader();
            MyWritter();
            //runs when the player is loaded.
            var player = @event.Root.Player;

            if (data_old_health == 0)
            {
                data_old_health = player.MaxHealth; //grab the initial health value
            }

            player.MaxHealth = data_ini_health_bonus + data_health_bonus_acumulated + data_old_health; //incase the ini health bonus is loaded in. 

            if (data_clear_mod_effects == true)
            {
                player.MaxHealth = data_old_health;
                Logging.LogToFile("BuildHealth Reset!");
            }

            DataLoader();
            MyWritter();
        }
        //Mod config data.
        public class Config
        {
            public double xp_nextlvl { get; set; }
            public double xp_current { get; set; }
            public double xp_curve { get; set; }

            public int current_lvl { get; set; }
            public int max_lvl { get; set; }

            public int health_increase_upon_lvl_up { get; set; }

            public int xp_tooluse { get; set; }
            public int xp_eating { get; set; }
            public int xp_sleeping { get; set; }

            public int ini_health_boost { get; set; }

            public int health_accumulated { get; set; }

        }


        void Clear_DataLoader()
        {
            //loads the data to the variables upon loading the game.
            var mylocation = Path.Combine(PathOnDisk, "BuildHealth_data.txt");
            string[] mystring = new string[20];
            if (!File.Exists(mylocation)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                Logging.LogToFile("The config file for BuildHealth was not found, guess I'll create it...");

                data_clear_mod_effects = false;
                data_old_health = 0;
                data_ini_health_bonus = 0;
            }

            else
            {
                //loads the data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation);
                data_ini_health_bonus = Convert.ToInt32(readtext[9]);
                data_clear_mod_effects = Convert.ToBoolean(readtext[14]);
                data_old_health = Convert.ToInt32(readtext[16]);

            }
        }




        void DataLoader()
        {
            //loads the data to the variables upon loading the game.
            var mylocation = Path.Combine(PathOnDisk, "BuildHealth_data.txt");
            //string[] mystring = new string[20];
            if (!File.Exists(mylocation)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                Logging.LogToFile("The config file for BuildHealth was not found, guess I'll create it...");
                data_xp_nextlvl = ModConfig.xp_nextlvl;
                data_xp_current = ModConfig.xp_current;
                data_current_lvl = ModConfig.current_lvl;
                data_ini_health_bonus = ModConfig.ini_health_boost;
                data_health_bonus_acumulated = ModConfig.health_accumulated;
                data_clear_mod_effects = false;
                data_old_health = 0;

            }

            else
            {
                //loads the data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation);
                data_current_lvl = Convert.ToInt32(readtext[3]);
                data_xp_nextlvl = Convert.ToDouble(readtext[7]);  //these array locations refer to the lines in data.json
                data_xp_current = Convert.ToDouble(readtext[5]);
                data_ini_health_bonus = Convert.ToInt32(readtext[9]);
                data_health_bonus_acumulated = Convert.ToInt32(readtext[11]);
                data_clear_mod_effects = Convert.ToBoolean(readtext[14]);
                data_old_health = Convert.ToInt32(readtext[16]);

            }
        }

        void MyWritter()
        {
            //saves the data at the end of a new day;
            var mylocation = Path.Combine(PathOnDisk, "BuildHealth_data.txt");
            string[] mystring2 = new string[20];
            if (!File.Exists(mylocation))
            {
                Logging.LogToFile("The data file for BuildHealth was not found, guess I'll create it when you sleep.");

                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring2[0] = "Player: Build Health Data. Modification can cause errors. Edit at your own risk.";
                mystring2[1] = "====================================================================================";

                mystring2[2] = "Player Current Level:";
                mystring2[3] = data_current_lvl.ToString();

                mystring2[4] = "Player Current XP:";
                mystring2[5] = data_xp_current.ToString();

                mystring2[6] = "Xp to next Level:";
                mystring2[7] = data_xp_nextlvl.ToString();

                mystring2[8] = "Initial health Bonus:";
                mystring2[9] = data_ini_health_bonus.ToString();

                mystring2[10] = "Additional health Bonus:";
                mystring2[11] = data_health_bonus_acumulated.ToString();

                mystring2[12] = "=======================================================================================";
                mystring2[13] = "RESET ALL MOD EFFECTS? This will effective start you back at square 1. Also good if you want to remove this mod.";
                mystring2[14] = data_clear_mod_effects.ToString();
                mystring2[15] = "OLD health AMOUNT: This is the initial value of the Player's health before this mod took over.";
                mystring2[16] = data_old_health.ToString();


                File.WriteAllLines(mylocation, mystring2);
            }

            else
            {
                //write out the info to a text file at the end of a day.
                mystring2[0] = "Player: Build Health Data. Modification can cause errors. Edit at your own risk.";
                mystring2[1] = "====================================================================================";

                mystring2[2] = "Player Current Level:";
                mystring2[3] = data_current_lvl.ToString();

                mystring2[4] = "Player Current XP:";
                mystring2[5] = data_xp_current.ToString();

                mystring2[6] = "Xp to next Level:";
                mystring2[7] = data_xp_nextlvl.ToString();

                mystring2[8] = "Initial health Bonus:";
                mystring2[9] = data_ini_health_bonus.ToString();

                mystring2[10] = "Additional health Bonus:";
                mystring2[11] = data_health_bonus_acumulated.ToString();

                mystring2[12] = "=======================================================================================";
                mystring2[13] = "RESET ALL MOD EFFECTS? This will effective start you back at square 1. Also good if you want to remove this mod.";
                mystring2[14] = data_clear_mod_effects.ToString();
                mystring2[15] = "OLD health AMOUNT: This is the initial value of the Player's health before this mod took over.";
                mystring2[16] = data_old_health.ToString();


                File.WriteAllLines(mylocation, mystring2);
            }
        }

    } //end my function
}