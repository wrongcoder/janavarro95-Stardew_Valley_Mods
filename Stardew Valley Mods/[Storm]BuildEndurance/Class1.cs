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
namespace BuildEndurance
{
    [Mod]
    public class BuildEndurance : DiskResource
    {
        public double BuildEndurance_data_xp_nextlvl;
        public double BuildEndurance_data_xp_current;

        public int BuildEndurance_data_current_lvl;

        public int BuildEndurance_data_stam_bonus_acumulated;

        public int BuildEndurance_data_ini_stam_bonus;

        public bool BuildEndurance_data_clear_mod_effects = false;

        public int BuildEndurance_data_old_stamina = 0;

        public Config ModConfig { get; set; }

        [Subscribe]
        //Credit goes to Zoryn for pieces of this config generation that I kinda repurposed.
        public void InitializeCallback(InitializeEvent @event)
        {
            var configLocation = Path.Combine(PathOnDisk, "BuildEnduranceConfig.json");
            if (!File.Exists(configLocation))
            {
                Logging.LogToFile("The config file for BuildEndurance was not found, guess I'll create it...");
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
                // Console.WriteLine("The config file for MoreRain has been loaded.\n\t RainChance: {0}, ThunderChance: {1}",
                //     ModConfig.RainChance, ModConfig.ThunderChance);
            }
            else
            {
                ModConfig = JsonConvert.DeserializeObject<Config>(Encoding.UTF8.GetString(File.ReadAllBytes(configLocation)));
                Logging.LogToFile("Found BuildEndurance config file.");
            }

            DataLoader();
            MyWritter();
            Logging.LogToFile("BuildEndurance Initialization Completed");
        }

        [Subscribe]

        public void ToolCallBack(Storm.StardewValley.Event.ReleaseUseToolButtonEvent @event)
        {
            //this will be the code that runs when a tool is used
            //Logging.LogToFile("A TOOL IS BEING USED");
            BuildEndurance_data_xp_current += ModConfig.BuildEndurance_xp_tooluse;  
        }
        [Subscribe]
        public void EatingCallBack(Storm.StardewValley.Event.PlayerEatObjectEvent @event)
        {
            //this code will run when the player eats an object. I.E. increases their eating skills.
            //Logging.LogToFile("A FOOD IS BEING CONSUMED");
            BuildEndurance_data_xp_current += ModConfig.BuildEndurance_xp_eating;
        }

        [Subscribe]
        public void SleepCallback(Storm.StardewValley.Event.PreNewDayEvent @event)
        {
            Clear_DataLoader();
            //This will run when the character goes to sleep. It will increase their sleeping skill.
            var player = @event.Root.Player;

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
                Logging.LogToFile("BuildEndurance Reset!");
            }


            if (BuildEndurance_data_clear_mod_effects == false)
            {
                if (BuildEndurance_data_current_lvl < ModConfig.BuildEndurance_max_lvl) { 
                while (BuildEndurance_data_xp_current >= BuildEndurance_data_xp_nextlvl)
                {
                        BuildEndurance_data_current_lvl += 1;
                        BuildEndurance_data_xp_current = BuildEndurance_data_xp_current - BuildEndurance_data_xp_nextlvl;
                        BuildEndurance_data_xp_nextlvl = (ModConfig.BuildEndurance_xp_curve * BuildEndurance_data_xp_nextlvl);
                    player.MaxStamina += ModConfig.BuildEndurance_stam_increase_upon_lvl_up;
                    BuildEndurance_data_stam_bonus_acumulated += ModConfig.BuildEndurance_stam_increase_upon_lvl_up;
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

        [Subscribe]
        public void LoadingCallBack(Storm.StardewValley.Event.PostGameLoadedEvent @event)
        {
            DataLoader();
            MyWritter();
            //runs when the player is loaded.
            var player = @event.Root.Player;

            if (BuildEndurance_data_old_stamina == 0)
            {
                BuildEndurance_data_old_stamina = player.MaxStamina; //grab the initial stamina value
            }

        player.MaxStamina = BuildEndurance_data_ini_stam_bonus +BuildEndurance_data_stam_bonus_acumulated +BuildEndurance_data_old_stamina; //incase the ini stam bonus is loaded in. 

            if (BuildEndurance_data_clear_mod_effects == true)
            {
                player.MaxStamina = BuildEndurance_data_old_stamina;
                Logging.LogToFile("BuildEndurance Reset!");
            }

            DataLoader();
            MyWritter();
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
            //loads the data to the variables upon loading the game.
            var mylocation = Path.Combine(PathOnDisk, "BuildEndurance_data.txt");
           // string[] mystring = new string[20];
            if (!File.Exists(mylocation)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                Logging.LogToFile("The config file for BuildEndurance was not found, guess I'll create it...");


                BuildEndurance_data_clear_mod_effects = false;
                BuildEndurance_data_old_stamina = 0;
                BuildEndurance_data_ini_stam_bonus = 0;
            }

            else
            {
                //loads the BuildEndurance_data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation);
                BuildEndurance_data_ini_stam_bonus = Convert.ToInt32(readtext[9]);
                BuildEndurance_data_clear_mod_effects = Convert.ToBoolean(readtext[14]);
                BuildEndurance_data_old_stamina = Convert.ToInt32(readtext[16]);

            }
        }




        void DataLoader()
        {
            //loads the data to the variables upon loading the game.
            var mylocation = Path.Combine(PathOnDisk, "BuildEndurance_data.txt");
            //string[] mystring = new string[20];
            if (!File.Exists(mylocation)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                Logging.LogToFile("The config file for BuildEndurance was not found, guess I'll create it...");
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
                //loads the BuildEndurance_data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation);
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
            var mylocation = Path.Combine(PathOnDisk, "BuildEndurance_data.txt");
            string[] mystring3= new string[20];
            if (!File.Exists(mylocation))
            {
                Logging.LogToFile("The data file for BuildEndurance was not found, guess I'll create it when you sleep.");

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


                File.WriteAllLines(mylocation, mystring3);
            }
        
            else
            {
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


                File.WriteAllLines(mylocation, mystring3);
            }
        }

    } //end my function
}