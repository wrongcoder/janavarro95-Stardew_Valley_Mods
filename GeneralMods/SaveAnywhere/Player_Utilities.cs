using System;
using System.IO;
using StardewModdingAPI;
using StardewValley;

namespace Omegasis.SaveAnywhere
{
    class Player_Utilities
    {
        public static int player_x_tile;
        public static int player_y_tile;
        public static string players_current_map_name;
        public static int player_game_time;
        public static int player_facing_direction;
        public static bool has_player_warped_yet;
        public static void get_player_info()
        {
            get_x();
            get_y();
            get_current_map_name();
            get_facing_direction();
        }

        public static void get_x()
        {
           player_x_tile=Game1.player.getTileX();
        }
        public static void get_y()
        {
            player_y_tile = Game1.player.getTileY();
        }
        public static void get_current_map_name()
        {
            players_current_map_name = Game1.player.currentLocation.name;
        }
        public static void get_facing_direction()
        {
            player_facing_direction = Game1.player.facingDirection;
        }

        public static void save_player_info()
        {
            get_player_info();
            string name = StardewValley.Game1.player.name;
            Mod_Core.player_path= Path.Combine(Mod_Core.mod_path, "Save_Data", name);
            if (!Directory.Exists(Mod_Core.player_path)){
                Directory.CreateDirectory(Mod_Core.player_path);
            }



            string mylocation = Path.Combine(Mod_Core.player_path, "Player_Save_Info_");
            string mylocation2 = mylocation + name;
            string mylocation3 = mylocation2 + ".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
                Mod_Core.thisMonitor.Log("Save Anywhere: The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.", LogLevel.Info);
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Player: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player Current Game Time";
                mystring3[3] = Game1.timeOfDay.ToString();

                mystring3[4] = "Player Current Map Name";
                mystring3[5] = players_current_map_name.ToString();

                mystring3[6] = "Player X Position";
                mystring3[7] = player_x_tile.ToString();

                mystring3[8] = "Player Y Position";
                mystring3[9] = player_y_tile.ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }

            else
            {

                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Player: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player Current Game Time";
                mystring3[3] = Game1.timeOfDay.ToString();

                mystring3[4] = "Player Current Map Name";
                mystring3[5] = players_current_map_name.ToString();

                mystring3[6] = "Player X Position";
                mystring3[7] = player_x_tile.ToString();

                mystring3[8] = "Player Y Position";
                mystring3[9] = player_y_tile.ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }

        }

        public static void load_player_info()
        {
            string name = StardewValley.Game1.player.name;
            Mod_Core.player_path = Path.Combine(Mod_Core.mod_path, "Save_Data", name);
            if (!Directory.Exists(Mod_Core.player_path))
            {
                Directory.CreateDirectory(Mod_Core.player_path);
            }



            string mylocation = Path.Combine(Mod_Core.player_path, "Player_Save_Info_");
            string mylocation2 = mylocation + name;
            string mylocation3 = mylocation2 + ".txt";
            string[] mystring3 = new string[20];

            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                //  Console.WriteLine("Can't load custom save info since the file doesn't exist.");
                Player_Utilities.has_player_warped_yet = true;
            }

            else
            {
                //        Console.WriteLine("HEY THERE IM LOADING DATA");

                string[] readtext = File.ReadAllLines(mylocation3);
                player_game_time = Convert.ToInt32(readtext[3]);
                players_current_map_name = Convert.ToString(readtext[5]);
                player_x_tile = Convert.ToInt32(readtext[7]);
                player_y_tile = Convert.ToInt32(readtext[9]);
                Game1.timeOfDay = player_game_time;

            }
        }

        public static void warp_player()
        {
            GameLocation new_location = Game1.getLocationFromName(players_current_map_name);
            Game1.player.previousLocationName = Game1.player.currentLocation.name;
            Game1.locationAfterWarp = new_location;
            Game1.xLocationAfterWarp = player_x_tile;
            Game1.yLocationAfterWarp = player_y_tile;
            Game1.facingDirectionAfterWarp = player_facing_direction;
            Game1.fadeScreenToBlack();

            Game1.warpFarmer(players_current_map_name, player_x_tile, player_y_tile, false);
            Mod_Core.thisMonitor.Log("WARP THE PLAYER");
            Game1.player.faceDirection(player_facing_direction);

            if (Directory.Exists(Mod_Core.player_path))
            {
                // Directory.Delete(player_path, true);
            }
        }

    }
}
