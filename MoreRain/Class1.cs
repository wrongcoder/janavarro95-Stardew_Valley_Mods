using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace MoreRain
{

    public class MoreRain : Mod
    {
        string rainstring = "15";
        bool auto_update = false;
        string mypath = "";
        int rainint = 0;
        string thunderstring = "5";
        int thunderint = 0;
        public override string Name
        {
            get { return "MoreRain"; }
        }

        public override string Authour
        {
            get { return "Alpha_Omegasis"; }
        }

        public override string Version
        {
            get { return "0.0.2a"; }
        }

        public override string Description
        {
            get { return "Overrides normal rain patterns using a int in MoreRain.txt. Won't take place if the next day doesn't have a chance to be rainy or sunny. I.E a festival or a wedding."; }
        }

        public override void Entry(params object[] objects)
        {
            set_up();
            StardewModdingAPI.Events.GameEvents.UpdateTick += Events_UpdateTick;

        }

        void set_up()
        {
            string line;
            int counter = 0;
            mypath = Path.GetFullPath("Mods/MoreRain.txt");

            Console.WriteLine("Found MoreRain.txt at " + mypath);
            // Open the text file using a stream reader.
            //Reads in the rain variable from MoreRain.txt

            System.IO.StreamReader file = new System.IO.StreamReader(mypath);
            while((line = file.ReadLine()) != null)
    {
    System.Console.WriteLine (line);
    if (counter == 0) { rainstring = line; }
    if (counter == 1) { thunderstring = line; }
    counter++;
                

    }
            rainint = Convert.ToInt32(rainstring);
            thunderint = Convert.ToInt32(thunderstring);

            Console.WriteLine(rainint);
            Console.WriteLine("MoreRain mod has loaded.");
            Program.LogColour(ConsoleColor.Blue, "MAKE IT RAIN");
        }

        void Events_UpdateTick(object sender, EventArgs e)
        {

            if (StardewModdingAPI.Inheritance.SGame.newDay)
            {
                auto_update = false; //resets upon a new day
            }
            if (StardewModdingAPI.Inheritance.SGame.hasLoadedGame) //makes sure a game file is loaded up
            {
                if (StardewModdingAPI.Inheritance.SGame.player.isMoving()) //waits for the character to move to update speed
                {
                    if (auto_update == false)
                    {
                        New_day_Update(); //updates the info upon a new day.
                    }
                }
            }
        }

        void New_day_Update() //updates all info whenever I call this.
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 100); //sets ran variable to some num between 0 and 100
            Random thunder_random = new Random();
            int thunder_randomNumber = random.Next(0, 100);

            if (StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_sunny || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_rain || StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_lightning)
            { //if my weather isn't something special. This is to prevent something from going wierd.
                if (randomNumber <= rainint) //if the random variable is less than or equal to the chance for rain.
                {
                    StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_rain; //sets rainy weather tomorrow
                    Console.WriteLine("It will rain tomorrow."); 
                }
                else {
                    StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_sunny;//sets sunny weather tomorrow
                    Console.WriteLine("It will not rain tomorrow.");
                }
                /*
                Console.WriteLine(randomNumber);
                Console.WriteLine(rainint);
                 */ 
               

                if (StardewValley.Game1.weatherForTomorrow == StardewValley.Game1.weather_rain)
                {
                    if (randomNumber <= thunderint) //if the random variable is less than or equal to the chance for rain.
                    {
                        StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_lightning; //sets rainy weather tomorrow
                        Console.WriteLine("It will be stormy tomorrow.");
                    }
                    else
                    {
                        StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_rain;//sets sunny weather tomorrow
                        Console.WriteLine("There will be no lightning tomorrow.");
                    }
                }
                
            }
            else
            {
                Console.WriteLine("The weather for tomorrow is not rainy, stormy, or sunny. Must be something special.");
            }
            Console.WriteLine("RainMod has updated.");
            auto_update = true;
        }
    }
}
