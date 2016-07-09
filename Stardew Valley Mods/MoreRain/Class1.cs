using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using System.IO;
using StardewValley;


namespace MoreRain
{

    public class MoreRain : Mod
    { 
        int rainint = 0;
        int thunderint = 0;

        bool gameloaded;

        bool suppress_log;

        public override void Entry(params object[] objects)
        {
           // set_up();
            StardewModdingAPI.Events.PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
            StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += TimeEvents_DayOfMonthChanged;
            DataLoader();
            MyWritter();
        }

        public void TimeEvents_DayOfMonthChanged(object sender, StardewModdingAPI.Events.EventArgsIntChanged e)
        {
            if (gameloaded == false) return;
            New_day_Update();
        }

        public void PlayerEvents_LoadedGame(object sender, StardewModdingAPI.Events.EventArgsLoadedGameChanged e)
        {
            gameloaded = true;
            
            New_day_Update();
        }

        void New_day_Update() //updates all info whenever I call this.
        {

            if (Game1.weatherForTomorrow == Game1.weather_festival )
            {
                if(suppress_log==false)Log.Info("There is a festival tomorrow, therefore it will not rain.");
                return;
            }

            if(Game1.weatherForTomorrow== Game1.weather_wedding)
            {
                if(suppress_log==false)Log.Info("There is a wedding tomorrow and rain on your wedding day will not happen.");
                return;
            }


            Random random = new Random();
            int randomNumber = random.Next(0, 100); //sets ran variable to some num between 0 and 100
            Random thunder_random = new Random();
            int thunder_randomNumber = random.Next(0, 100);

            if (StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_sunny || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_rain || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_lightning || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_debris)
            { //if my weather isn't something special. This is to prevent something from going wierd.
                if (randomNumber <= rainint) //if the random variable is less than or equal to the chance for rain.
                {
                    StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_rain; //sets rainy weather tomorrow
                    if(suppress_log==false)Log.Info("It will rain tomorrow.");
                }
                else {
                    StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_sunny;//sets sunny weather tomorrow
                    if(suppress_log==false)Log.Info("It will not rain tomorrow.");
                }

                if (StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_rain)
                {
                    if (randomNumber <= thunderint) //if the random variable is less than or equal to the chance for rain.
                    {
                        StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_lightning; //sets rainy weather tomorrow
                        if(suppress_log==false)Log.Info("It will be stormy tomorrow.");
                    }
                    else
                    {
                        StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_rain;//sets sunny weather tomorrow
                        if(suppress_log==false)Log.Info("There will be no lightning tomorrow.");
                    }
                }
            }
            else
            {
              //  if(suppress_log==false)Log.Info("The weather for tomorrow is not rainy, stormy, or sunny. Must be something special.");
            }
            if(suppress_log==false)Log.Info("More Rain has updated.");
        }


        void MyWritter()
        {
            //saves the BuildEndurance_data at the end of a new day;
            string mylocation = Path.Combine(PathOnDisk, "More_Rain_Config");
            //string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation + ".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
                Log.Info("The data file for More Rain wasn't found. Time to create it!");
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.
                mystring3[0] = "Player: More Rain Config. Feel free to edit.";
                mystring3[1] = "====================================================================================";
                mystring3[2] = "Rain chance: The chance out of 100 that it will rain tomorrow.";
                mystring3[3] = rainint.ToString();
                mystring3[4] = "Storm chance: The chance out of 100 that it will be stormy tomorrow.";
                mystring3[5] = thunderint.ToString();

                mystring3[6] = "Supress Log: If true, the mod won't output any messages to the console.";
                mystring3[7] = suppress_log.ToString();


                File.WriteAllLines(mylocation3, mystring3);
            }
            else
            {
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.
                mystring3[0] = "Player: More Rain Config. Feel free to edit.";
                mystring3[1] = "====================================================================================";
                mystring3[2] = "Rain chance: The chance out of 100 that it will rain tomorrow.";
                mystring3[3] = rainint.ToString();
                mystring3[4] = "Storm chance: The chance out of 100 that it will be stormy tomorrow.";
                mystring3[5] = thunderint.ToString();
                mystring3[6] = "Supress Log: If true, the mod won't output any messages to the console.";
                mystring3[7] = suppress_log.ToString();

                File.WriteAllLines(mylocation3, mystring3);
            }
        }
        void DataLoader()
        {
            //loads the data to the variables upon loading the game.
            string mylocation = Path.Combine(PathOnDisk, "More_Rain_Config");
            //string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                rainint = 15;
                thunderint = 5;
                suppress_log = true;
            }
            else
            {
                string[] readtext = File.ReadAllLines(mylocation3);
                rainint = Convert.ToInt32(readtext[3]);
                thunderint = Convert.ToInt32(readtext[5]);  //these array locations refer to the lines in BuildEndurance_data.json
                suppress_log = Convert.ToBoolean(readtext[7]);
            }
        }
    }
}
