using System;
using System.IO;
using StardewModdingAPI;

namespace Omegasis.AutoSpeed
{

    public class AutoSpeed : Mod
    {
        int speed_int = 5;
        public override void Entry(IModHelper helper)
        {
            //StardewModdingAPI.Events.GameEvents.UpdateTick += Events_UpdateTick;
            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
            var configLocation = Path.Combine(helper.DirectoryPath, "AutoSpeed_Data.txt");
            if (!File.Exists(configLocation))
            {
                 speed_int = 1;
            }
            DataLoader();
           // MyWritter();
            Monitor.Log("AutoSpeed Initialization Completed",LogLevel.Info);           
        }

        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            StardewValley.Game1.player.addedSpeed = speed_int;
        }

        void DataLoader()
        {
            //loads the data to the variables upon loading the game.
            var mylocation = Path.Combine(Helper.DirectoryPath, "AutoSpeed_data.txt");
            if (!File.Exists(mylocation)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
               Monitor.Log("The config file for AutoSpeed was not found, guess I'll create it...",LogLevel.Warn);
                MyWritter();
            }
            else
            {
                string[] readtext = File.ReadAllLines(mylocation);
                speed_int = Convert.ToInt32(readtext[3]);
            }
        }
  
        void MyWritter()
        {
            //saves the BuildEndurance_data at the end of a new day;
            var mylocation = Path.Combine(Helper.DirectoryPath, "AutoSpeed_data.txt");
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation))
            {
                Monitor.Log("The data file for AutoSpeed was not found, guess I'll create it when you sleep.",LogLevel.Info);

                mystring3[0] = "Player: AutoSpeed Config:";
                mystring3[1] = "====================================================================================";
                mystring3[2] = "Player Added Speed:";
                mystring3[3] = speed_int.ToString();
                File.WriteAllLines(mylocation, mystring3);
            }
            else
            {
                mystring3[0] = "Player: AutoSpeed Config:";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player Added Speed:";
                mystring3[3] = speed_int.ToString();

                File.WriteAllLines(mylocation, mystring3);
            }
        }

    } //end my function
}