using System;
using StardewValley;
using StardewModdingAPI;
using System.IO;
using StardewValley.Menus;
using System.Collections.Generic;

namespace Buy_Back_Collectables
{
    public class Class1 : Mod
    {
        string key_binding = "B";
       public static double cost = 3.0;
        bool game_loaded = false;


        public static List<String> debugList;

        public override void Entry(IModHelper helper)
        {
            //set up all of my events here
            StardewModdingAPI.Events.PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
            StardewModdingAPI.Events.ControlEvents.KeyPressed += ControlEvents_KeyPressed;
            StardewModdingAPI.Events.GameEvents.OneSecondTick += GameEvents_OneSecondTick;
            debugList = new List<string>();
        }

        private void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            if (debugList.Count == 0) return;
           foreach(var v in debugList)
            {
                this.Monitor.Log(v);
            }
            debugList.Clear();
        }

        public void ControlEvents_KeyPressed(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            if (Game1.player == null) return;
            if (Game1.player.currentLocation == null) return;
            if (game_loaded == false) return;

            if (e.KeyPressed.ToString() == key_binding) //if the key is pressed, load my cusom save function
            {
                if (Game1.activeClickableMenu != null) return;
                else
                {
                    Game1.activeClickableMenu = new StardewValley.Menus.UpdatedCollectionsPage(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2);
                }
            }
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
            string mylocation = Path.Combine(Helper.DirectoryPath, "BuyBack_Config");
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                key_binding = "B";
                cost = 3.0;
            }

            else
            {
                string[] readtext = File.ReadAllLines(mylocation3);
                key_binding = Convert.ToString(readtext[3]);
                cost = Convert.ToDouble(readtext[5]);
            }
        }

        void MyWritter_Settings()
        {
            //write all of my info to a text file.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Helper.DirectoryPath, "BuyBack_Config");
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
                Monitor.Log("BuyBack Collections: Config not found. Creating it now.");

                mystring3[0] = "Config: Buy Back Collections. Feel free to mess with these settings.";
                mystring3[1] = "====================================================================================";
                mystring3[2] = "Key binding";
                mystring3[3] = key_binding.ToString();
                mystring3[4] = "Collectables Multiplier Cost: Sell Value * value listed below";
                mystring3[5] = cost.ToString();
                File.WriteAllLines(mylocation3, mystring3);
            }
            else
            {
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.
                mystring3[0] = "Config: Buy Back Collections. Feel free to mess with these settings.";
                mystring3[1] = "====================================================================================";
                mystring3[2] = "Key binding";
                mystring3[3] = key_binding.ToString();
                mystring3[4] = "Collectables Multiplier Cost: Sell Value * value listed below";
                mystring3[5] = cost.ToString();
                File.WriteAllLines(mylocation3, mystring3);
            }
        }

        public void debugMessage(string s)
        {
            this.Monitor.Log(s);
        }

    }
}
//end class