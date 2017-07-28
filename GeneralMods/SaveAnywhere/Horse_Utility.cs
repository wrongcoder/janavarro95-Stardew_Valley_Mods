using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Save_Anywhere_V2.Save_Utilities.Animals
{
    class Horse_Utility
    {

      public static  void Save_Horse_Info()
        {
            
           
            Horse horse = Utility.findHorse();


            if (horse == null)
            {
                //Game1.getFarm().characters.Add((NPC)new Horse(this.player_tile_x + 1, this.player_tile_Y + 1));
                Mod_Core.thisMonitor.Log("NEIGH: No horse exists", LogLevel.Debug);
                return;
            }
            // else
            //   Game1.warpCharacter((NPC)horse, Game1.player.currentLocation.name, StardewValley.Game1.player.getTileLocationPoint(), false, true);




            string myname = StardewValley.Game1.player.name;

            string mylocation = Path.Combine(Save_Anywhere_V2.Mod_Core.animal_path, "Horse_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {

                Mod_Core.thisMonitor.Log("The horse save info doesn't exist. It will be created when the custom saving method is run. Which is now.", LogLevel.Debug);
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Horse: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Horse Current Map Name";
                mystring3[3] = horse.currentLocation.name.ToString();

                mystring3[4] = "Horse X Position";
                mystring3[5] = horse.getTileX().ToString();

                mystring3[6] = "Horse Y Position";
                mystring3[7] = horse.getTileY().ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }

            else
            {
                //   Console.WriteLine("The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.");
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Horse: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Horse Current Map Name";
                mystring3[3] = horse.currentLocation.name.ToString();

                mystring3[4] = "Horse X Position";
                mystring3[5] = horse.getTileX().ToString();

                mystring3[6] = "Horse Y Position";
                mystring3[7] = horse.getTileY().ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }
        }

      public static void Load_Horse_Info()
        {
            Horse horse = Utility.findHorse();
            if (horse == null)
            {
                Mod_Core.thisMonitor.Log("NEIGH: No horse exists", LogLevel.Debug);
                return;
            }
            //   DataLoader_Settings();
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Save_Anywhere_V2.Mod_Core.animal_path, "Horse_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
            }

            else
            {
                string horse_map_name = "";
                int horse_x;
                int horse_y;
                Point horse_point;
                string[] readtext = File.ReadAllLines(mylocation3);
                horse_map_name = Convert.ToString(readtext[3]);
                horse_x = Convert.ToInt32(readtext[5]);
                horse_y = Convert.ToInt32(readtext[7]);
                horse_point.X = horse_x;
                horse_point.Y = horse_y;
                Game1.warpCharacter((NPC)horse, horse_map_name, horse_point, false, true);
            }
        }
    }
}
