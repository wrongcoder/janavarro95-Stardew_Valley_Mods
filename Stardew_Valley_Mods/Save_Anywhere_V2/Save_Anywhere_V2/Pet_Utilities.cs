using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Save_Anywhere_V2.Save_Utilities.Animals
{
    class Pet_Utilities
    {
        public static string pet_name;
        public static StardewValley.Character my_pet;
        public static string pet_map_name;
        public static int pet_tile_x;
        public static int pet_tile_y;
        public static bool is_pet_outside;
        public static bool has_pet_warped_yet;
       public static  Microsoft.Xna.Framework.Point pet_point;

        public static void save_pet_info()
        {
            if (Game1.player.hasPet() == false) return;
            pet_name = Game1.player.getPetName();
            foreach (var location in Game1.locations)
            {
                foreach (var npc in location.characters)
                {
                    if (npc is StardewValley.Characters.Dog || npc is StardewValley.Characters.Cat)
                    {
                        pet_map_name = location.name;
                        pet_tile_x = npc.getTileX();
                        pet_tile_y = npc.getTileY();
                        is_pet_outside = location.isOutdoors;

                    }

                }

            }
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Save_Anywhere_V2.Mod_Core.animal_path, "Pet_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {


                Log.Info("Save Anywhere: The pet save info doesn't exist. It will be created when the custom saving method is run. Which is now.");
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Pet: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Pet Current Map Name";
                mystring3[3] = pet_map_name.ToString();

                mystring3[4] = "Pet X Position";
                mystring3[5] = pet_tile_x.ToString();

                mystring3[6] = "Pet Y Position";
                mystring3[7] = pet_tile_y.ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }

            else
            {
                //   Console.WriteLine("The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.");
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.
                mystring3[0] = "Pet: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Pet Current Map Name";
                mystring3[3] = pet_map_name.ToString();

                mystring3[4] = "Pet X Position";
                mystring3[5] = pet_tile_x.ToString();

                mystring3[6] = "Pet Y Position";
                mystring3[7] = pet_tile_y.ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }

        }

        public static void Load_pet_Info()
        {
            if (Game1.player.hasPet() == false) return;
            //   DataLoader_Settings();
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Save_Anywhere_V2.Mod_Core.animal_path, "Pet_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
            }

            else
            {
                string[] readtext = File.ReadAllLines(mylocation3);
                pet_map_name = Convert.ToString(readtext[3]);
                pet_tile_x = Convert.ToInt32(readtext[5]);
                pet_tile_y = Convert.ToInt32(readtext[7]);
                get_pet();
               pet_point = new Microsoft.Xna.Framework.Point();
                pet_point.X = pet_tile_x;
                pet_point.Y = pet_tile_y;
                Game1.warpCharacter((StardewValley.NPC)my_pet, pet_map_name, pet_point, false, true);
            }
        }

        public static void get_pet()
        {
            if (Game1.player.hasPet() == false) return;
            foreach (var location in Game1.locations)
            {
                foreach (var npc in location.characters)
                {
                    if (npc is StardewValley.Characters.Dog || npc is StardewValley.Characters.Cat)
                    {
                        pet_map_name = location.name;
                        pet_tile_x = npc.getTileX();
                        pet_tile_y = npc.getTileY();
                        is_pet_outside = location.isOutdoors;
                        my_pet = npc;
                    }

                }

            }

        }
    }
}
