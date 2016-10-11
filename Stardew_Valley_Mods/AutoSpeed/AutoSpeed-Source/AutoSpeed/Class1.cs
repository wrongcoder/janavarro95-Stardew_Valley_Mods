using System;
using StardewModdingAPI;
using System.IO;


namespace ClassLibrary1
{

    public class AutoSpeed : Mod
    {
        int speed_int = 5;
        public override void Entry(params object[] objects)
        {
            //StardewModdingAPI.Events.GameEvents.UpdateTick += Events_UpdateTick;
            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
            var configLocation = Path.Combine(PathOnDisk, "AutoSpeed_Data.txt");
            if (!File.Exists(configLocation))
            {
                 speed_int = 1;
            }

            DataLoader();
            MyWritter();
            Console.WriteLine("AutoSpeed Initialization Completed");

        }

        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            StardewValley.Game1.player.addedSpeed = speed_int;
        }

        public void Events_UpdateTick(object sender, EventArgs e)
        {
            StardewValley.Game1.player.addedSpeed = speed_int;

        }

        void DataLoader()
        {
            //loads the data to the variables upon loading the game.
            var mylocation = Path.Combine(PathOnDisk, "AutoSpeed_data.txt");
            if (!File.Exists(mylocation)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                Console.WriteLine("The config file for AutoSpeed was not found, guess I'll create it...");

            }
            else
            {
                //        Console.WriteLine("HEY THERE IM LOADING DATA");

                //loads the BuildEndurance_data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation);
                speed_int = Convert.ToInt32(readtext[3]);

            }
        }
  
        void MyWritter()
        {
            //saves the BuildEndurance_data at the end of a new day;
            var mylocation = Path.Combine(PathOnDisk, "AutoSpeed_data.txt");
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation))
            {
                Console.WriteLine("The data file for AutoSpeed was not found, guess I'll create it when you sleep.");

                mystring3[0] = "Player: AutoSpeed Config:";
                mystring3[1] = "====================================================================================";
                mystring3[2] = "Player Added Speed:";
                mystring3[3] = speed_int.ToString();
                File.WriteAllLines(mylocation, mystring3);
            }
            else
            {
                //    Console.WriteLine("HEY IM SAVING DATA");

                //write out the info to a text file upon loading
                mystring3[0] = "Player: AutoSpeed Config:";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player Added Speed:";
                mystring3[3] = speed_int.ToString();

                File.WriteAllLines(mylocation, mystring3);
            }
        }

    } //end my function
}