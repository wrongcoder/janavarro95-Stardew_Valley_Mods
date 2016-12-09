using System;
using StardewValley;
using StardewModdingAPI;
using System.IO;

/*
TO DO:

*/
namespace Billboard_Anywhere
{
    public class Class1 : Mod
    {
        string key_binding = "B";

        bool game_loaded = false;

        public override void Entry(IModHelper helper)
        {
            //set up all of my events here
            StardewModdingAPI.Events.PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
            StardewModdingAPI.Events.ControlEvents.KeyPressed += ControlEvents_KeyPressed;
        }



        public void ControlEvents_KeyPressed(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            if (Game1.player == null) return;
            if (Game1.player.currentLocation == null) return;
            if (game_loaded == false) return;

            if (e.KeyPressed.ToString() == key_binding) //if the key is pressed, load my cusom save function
            {
                if (Game1.activeClickableMenu != null) return;
                    my_menu();
            }
            //DataLoader_Settings(); //update the key if players changed it while playing.
        }

        public void PlayerEvents_LoadedGame(object sender, StardewModdingAPI.Events.EventArgsLoadedGameChanged e)
        {
            game_loaded = true;
            DataLoader_Settings();
            MyWritter_Settings();
        }




        void DataLoader_Settings()
        {
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Helper.DirectoryPath, "Billboard_Anywhere_Config");
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                //  Console.WriteLine("Can't load custom save info since the file doesn't exist.");

                key_binding = "B";
                //  Log.Info("KEY TIME");
            }

            else
            {
                //        Console.WriteLine("HEY THERE IM LOADING DATA");
                string[] readtext = File.ReadAllLines(mylocation3);
                key_binding = Convert.ToString(readtext[3]);


                // Log.Info(key_binding);
                // Log.Info(Convert.ToString(readtext[3]));

            }
        }

        void MyWritter_Settings()
        {

            //write all of my info to a text file.
            string myname = StardewValley.Game1.player.name;

            string mylocation = Path.Combine(Helper.DirectoryPath, "Billboard_Anywhere_Config");
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";

            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
                Monitor.Log("Billboard_Anywhere: The Billboard Anywhere Config doesn't exist. Creating it now.");

                mystring3[0] = "Config: Billboard_Anywhere. Feel free to mess with these settings.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Key binding for opening the billboard anywhere. Press this key to do so";
                mystring3[3] = key_binding.ToString();



                File.WriteAllLines(mylocation3, mystring3);

            }

            else
            {

                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Config: Billboard_Anywhere Info. Feel free to mess with these settings.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Key binding for opening the billboard anywhere. Press this key to do so";
                mystring3[3] = key_binding.ToString();

                File.WriteAllLines(mylocation3, mystring3);
            }
        }




        void my_menu()
        {

            //   Game1.activeClickableMenu = new StardewValley.Menus.SaveGameMenu(); //This command is what allows the player to save anywhere as it calls the saving function.


            Game1.activeClickableMenu = new StardewValley.Menus.Billboard();
        }

    }
}
//end class