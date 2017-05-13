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
        int springRainInt;
        int springThunderInt;
        int summerRainInt;
        int summerThunderInt;
        int fallRainInt;
        int fallThunderInt;
        int winterSnowInt;

        bool gameloaded;

        bool suppress_log;

        public override void Entry(IModHelper helper)
        {
           // set_up();
            StardewModdingAPI.Events.SaveEvents.AfterLoad += PlayerEvents_LoadedGame;
            StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += TimeEvents_DayOfMonthChanged;
            DataLoader();
        }

        public void TimeEvents_DayOfMonthChanged(object sender, StardewModdingAPI.Events.EventArgsIntChanged e)
        {
            if (gameloaded == false) return;
            New_day_Update();
        }

        public void PlayerEvents_LoadedGame(object sender, EventArgs e)
        {
            gameloaded = true;
            
            New_day_Update();
        }

        void New_day_Update() //updates all info whenever I call this.
        {

            

            if (Game1.weatherForTomorrow == Game1.weather_festival )
            {
                if(suppress_log==false)Monitor.Log("There is a festival tomorrow, therefore it will not rain.");
                return;
            }

            if(Game1.weatherForTomorrow== Game1.weather_wedding)
            {
                if(suppress_log==false)Monitor.Log("There is a wedding tomorrow and rain on your wedding day will not happen.");
                return;
            }


            Random random = new Random();
            int randomNumber = random.Next(0, 100); //sets ran variable to some num between 0 and 100
            Random thunder_random = new Random();
            int thunder_randomNumber = random.Next(0, 100);

            if (Game1.currentSeason == "spring")
            {
                if (StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_sunny || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_rain || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_lightning || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_debris)
                { //if my weather isn't something special. This is to prevent something from going wierd.
                    if (randomNumber <= springRainInt) //if the random variable is less than or equal to the chance for rain.
                    {
                        StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_rain; //sets rainy weather tomorrow
                        if (suppress_log == false) Monitor.Log("It will rain tomorrow.");
                    }
                    else
                    {
                        StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_sunny;//sets sunny weather tomorrow
                        if (suppress_log == false) Monitor.Log("It will not rain tomorrow.");
                    }

                    if (StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_rain)
                    {
                        if (randomNumber <= springThunderInt) //if the random variable is less than or equal to the chance for rain.
                        {
                            StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_lightning; //sets rainy weather tomorrow
                            if (suppress_log == false) Monitor.Log("It will be stormy tomorrow.");
                        }
                        else
                        {
                            StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_rain;//sets sunny weather tomorrow
                            if (suppress_log == false) Monitor.Log("There will be no lightning tomorrow.");
                        }
                    }
                }
            }
            else if (Game1.currentSeason == "summer")
            {
                if (StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_sunny || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_rain || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_lightning || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_debris)
                { //if my weather isn't something special. This is to prevent something from going wierd.
                    if (randomNumber <= summerRainInt) //if the random variable is less than or equal to the chance for rain.
                    {
                        StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_rain; //sets rainy weather tomorrow
                        if (suppress_log == false) Monitor.Log("It will rain tomorrow.");
                    }
                    else
                    {
                        StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_sunny;//sets sunny weather tomorrow
                        if (suppress_log == false) Monitor.Log("It will not rain tomorrow.");
                    }

                    if (StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_rain)
                    {
                        if (randomNumber <= summerThunderInt) //if the random variable is less than or equal to the chance for rain.
                        {
                            StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_lightning; //sets rainy weather tomorrow
                            if (suppress_log == false) Monitor.Log("It will be stormy tomorrow.");
                        }
                        else
                        {
                            StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_rain;//sets sunny weather tomorrow
                            if (suppress_log == false) Monitor.Log("There will be no lightning tomorrow.");
                        }
                    }
                }
            }
            else if (Game1.currentSeason=="fall"|| Game1.currentSeason == "autumn")
            {
                if (StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_sunny || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_rain || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_lightning || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_debris)
                { //if my weather isn't something special. This is to prevent something from going wierd.
                    if (randomNumber <= fallRainInt) //if the random variable is less than or equal to the chance for rain.
                    {
                        StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_rain; //sets rainy weather tomorrow
                        if (suppress_log == false) Monitor.Log("It will rain tomorrow.");
                    }
                    else
                    {
                        StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_sunny;//sets sunny weather tomorrow
                        if (suppress_log == false) Monitor.Log("It will not rain tomorrow.");
                    }

                    if (StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_rain)
                    {
                        if (randomNumber <= fallThunderInt) //if the random variable is less than or equal to the chance for rain.
                        {
                            StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_lightning; //sets rainy weather tomorrow
                            if (suppress_log == false) Monitor.Log("It will be stormy tomorrow.");
                        }
                        else
                        {
                            StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_rain;//sets sunny weather tomorrow
                            if (suppress_log == false) Monitor.Log("There will be no lightning tomorrow.");
                        }
                    }
                }
            }
            else if (Game1.currentSeason == "winter")
            {
                if (StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_sunny || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_rain || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_lightning || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_debris || Game1.weatherForTomorrow==StardewValley.Game1.weather_snow)
                { //if my weather isn't something special. This is to prevent something from going wierd.
                    if (randomNumber <= winterSnowInt) //if the random variable is less than or equal to the chance for rain.
                    {
                        StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_snow; //sets rainy weather tomorrow
                        if (suppress_log == false) Monitor.Log("It will snow tomorrow.");
                    }
                    else
                    {
                        //StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_sunny;//sets sunny weather tomorrow
                        if (suppress_log == false) Monitor.Log("It will not snow tomorrow.");
                    }
                }
            }
        }


        void MyWritter()
        {
            //saves the BuildEndurance_data at the end of a new day;
            string mylocation = Path.Combine(Helper.DirectoryPath, "More_Rain_Config");
            //string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation + ".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
                Monitor.Log("The data file for More Rain wasn't found. Time to create it!");
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.
                mystring3[0] = "Player: More Rain Config. Feel free to edit.";
                mystring3[1] = "====================================================================================";
                mystring3[2] = "Spring Rain chance: The chance out of 100 that it will rain tomorrow.";
                mystring3[3] = springRainInt.ToString();
                mystring3[4] = "Spring Storm chance: The chance out of 100 that it will be stormy tomorrow.";
                mystring3[5] = springThunderInt.ToString();

                mystring3[6] = "Summer Rain chance: The chance out of 100 that it will rain tomorrow.";
                mystring3[7] = summerRainInt.ToString();
                mystring3[8] = "Summer Storm chance: The chance out of 100 that it will be stormy tomorrow.";
                mystring3[9] = summerThunderInt.ToString();

                mystring3[10] = "Fall Rain chance: The chance out of 100 that it will rain tomorrow.";
                mystring3[11] = fallRainInt.ToString();
                mystring3[12] = "Fall Storm chance: The chance out of 100 that it will be stormy tomorrow.";
                mystring3[13] = fallThunderInt.ToString();

                mystring3[14] = "Winter Snow chance: The chance out of 100 that it will rain tomorrow.";
                mystring3[15] = winterSnowInt.ToString();


                mystring3[16] = "Supress Log: If true, the mod won't output any messages to the console.";
                mystring3[17] = suppress_log.ToString();


                File.WriteAllLines(mylocation3, mystring3);
            }
            else
            {
                Monitor.Log("The data file for More Rain wasn't found. Time to create it!");
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.
                mystring3[0] = "Player: More Rain Config. Feel free to edit.";
                mystring3[1] = "====================================================================================";
                mystring3[2] = "Spring Rain chance: The chance out of 100 that it will rain tomorrow.";
                mystring3[3] = springRainInt.ToString();
                mystring3[4] = "Spring Storm chance: The chance out of 100 that it will be stormy tomorrow.";
                mystring3[5] = springThunderInt.ToString();

                mystring3[6] = "Summer Rain chance: The chance out of 100 that it will rain tomorrow.";
                mystring3[7] = summerRainInt.ToString();
                mystring3[8] = "Summer Storm chance: The chance out of 100 that it will be stormy tomorrow.";
                mystring3[9] = summerThunderInt.ToString();

                mystring3[10] = "Fall Rain chance: The chance out of 100 that it will rain tomorrow.";
                mystring3[11] = fallRainInt.ToString();
                mystring3[12] = "Fall Storm chance: The chance out of 100 that it will be stormy tomorrow.";
                mystring3[13] = fallThunderInt.ToString();

                mystring3[14] = "Winter Snow chance: The chance out of 100 that it will rain tomorrow.";
                mystring3[15] = winterSnowInt.ToString();


                mystring3[16] = "Supress Log: If true, the mod won't output any messages to the console.";
                mystring3[17] = suppress_log.ToString();


                File.WriteAllLines(mylocation3, mystring3);
            }
        }
        void DataLoader()
        {
            //loads the data to the variables upon loading the game.
            string mylocation = Path.Combine(Helper.DirectoryPath, "More_Rain_Config");
            //string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
               springRainInt = 15;
                summerRainInt = 5;
                fallRainInt = 15;
                winterSnowInt = 15;


                springThunderInt = 5;
                summerThunderInt = 10;
                fallThunderInt = 5;

                suppress_log = true;
                MyWritter();
            }
            else
            {
                try
                {
                    string[] readtext = File.ReadAllLines(mylocation3);
                    springRainInt = Convert.ToInt32(readtext[3]);
                    springThunderInt = Convert.ToInt32(readtext[5]);
                    summerRainInt = Convert.ToInt32(readtext[7]);
                    summerThunderInt = Convert.ToInt32(readtext[9]);
                    fallRainInt = Convert.ToInt32(readtext[11]);
                    fallThunderInt = Convert.ToInt32(readtext[13]);
                    winterSnowInt = Convert.ToInt32(readtext[15]);
                    suppress_log = Convert.ToBoolean(readtext[17]);
                }
                catch (Exception e) //something dun goofed
                {
                    springRainInt = 15;
                    summerRainInt = 5;
                    fallRainInt = 15;
                    winterSnowInt = 15;


                    springThunderInt = 5;
                    summerThunderInt = 10;
                    fallThunderInt = 5;

                    suppress_log = true;
                    MyWritter();
                }
            }
        }
    }
}
