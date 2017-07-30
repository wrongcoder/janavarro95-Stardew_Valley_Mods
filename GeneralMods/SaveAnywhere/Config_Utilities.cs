using System;
using System.IO;

namespace Omegasis.SaveAnywhere
{
    class Config_Utilities
    {
        public static string key_binding = "K";
        public static bool warp_character;
       public static void DataLoader_Settings()
        {
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(SaveAnywhere.mod_path, "Save_Anywhere_Config");
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
              key_binding = "K";
                warp_character = true;
            }

            else
            {
                string[] readtext = File.ReadAllLines(mylocation3);
                key_binding = Convert.ToString(readtext[3]);
            }
        }

       public static void MyWritter_Settings()
        {

            //write all of my info to a text file.
            string myname = StardewValley.Game1.player.name;

            string mylocation = Path.Combine(SaveAnywhere.mod_path, "Save_Anywhere_Config");
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";

            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
                Console.WriteLine("The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.");

                mystring3[0] = "Config: Save_Anywhere Info. Feel free to mess with these settings.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Key binding for saving anywhere. Press this key to save anywhere!";
                mystring3[3] = key_binding.ToString();


                File.WriteAllLines(mylocation3, mystring3);

            }

            else
            {

                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Config: Save_Anywhere Info. Feel free to mess with these settings.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Key binding for saving anywhere. Press this key to save anywhere!";
                mystring3[3] = key_binding.ToString();

                File.WriteAllLines(mylocation3, mystring3);
            }
        }
    }
}
