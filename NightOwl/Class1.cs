using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI;
using System.IO;

/*TODO:
*/
/*
Issues:
-Mail can't be wiped without destroying all mail.
-Lighting transition does not work if it is raining.
    -set the weather to clear if you are stayig up late.
        -transition still doesnt work. However atleast it is dark now.
*/



namespace Stardew_NightOwl
{
    public class Class1 : Mod
    {
        int player_x;
        int player_y;
        string prior_map;

        bool was_raining;
        bool time_reset;
        int prior_money;
        int post_money;
        float pre_stam;
        int pre_health;


        bool lighting_transition; //if true transition happens. If false, game starts out bright at 2AM. Good to remove that awkward change from dark to bright.
        bool warp;
        bool stay_up;

       // bool wipe_mail;
        bool protect_money;
        bool persistant_health;
        bool persistant_stamina;

        bool game_loaded;

        public override void Entry(params object[] objects)
        {
            StardewModdingAPI.Events.TimeEvents.TimeOfDayChanged += TimeEvents_TimeOfDayChanged;
            StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += TimeEvents_DayOfMonthChanged;
            StardewModdingAPI.Events.PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
        }

        public void PlayerEvents_LoadedGame(object sender, StardewModdingAPI.Events.EventArgsLoadedGameChanged e)
        {
            DataLoader();
            MyWritter();
            game_loaded = true;
        }

        public void TimeEvents_DayOfMonthChanged(object sender, StardewModdingAPI.Events.EventArgsIntChanged e)
        {
            if (game_loaded == false) return;
            DataLoader();
            MyWritter();
            List<string> newmail = new List<string>();
            if (time_reset == true)
            {
                was_raining = false;
                if (persistant_stamina==true) Game1.player.stamina = pre_stam; //reset health and stam upon collapsing
                if (persistant_health == true) Game1.player.health = pre_health;
                post_money = Game1.player.money;
                if (protect_money == true) Game1.player.money += (prior_money - post_money); //add the money back from colapsing.
                time_reset = false; //reset my bool.
                if(warp==true)Game1.warpFarmer(prior_map, player_x, player_y, false);
            }
            /*
            if (wipe_mail == true)
            {
                foreach (var i in Game1.mailbox) //delete those annoying charge messages. If only I could do this with mail IRL.
                { 
                    Log.Info(i);

                    if (i.Contains("passedOut"))
                    {
                        Log.Info("Found bad mail");
                    }
                    else {
                        newmail.Add(i);
                    }
                }
                Game1.mailbox.Clear();
                foreach (string mail in newmail)
                {
                    Game1.player.mailReceived.Remove(mail);
                    Game1.player.mailForTomorrow.Add(mail);
                    //Game1.addMailForTomorrow(mail, false, false);
                }
            }
            */


        }


        public void TimeEvents_TimeOfDayChanged(object sender, StardewModdingAPI.Events.EventArgsIntChanged e)
        {
            if (game_loaded == false) return;
            float color_mod;
            //transition the lighting for more "realistic" lighting from 2am to 11am. There is an awkward screen shuffle that happens because of this update.

            if (lighting_transition == true)
            {
                if (Game1.timeOfDay > 200 && Game1.timeOfDay < 1100)
                {
                        color_mod = (1100 - Game1.timeOfDay) / 1000f; //.7f
                        Game1.outdoorLight = Game1.ambientLight * color_mod;
                    
                }
            }


            if (stay_up == true)
            {
                if (Game1.timeOfDay == 2550)
                {
                    if (StardewValley.Game1.isRaining == true)
                    {
                        was_raining = true;
                        StardewValley.Game1.isRaining = false; //regardless make sure I change the weather. Otherwise lighting gets screwy.
                    }
                        StardewValley.Game1.updateWeatherIcon();
                    Game1.timeOfDay = 150; //change it from 1:50 am late, to 1:50 am early
                }
            }

            if(Game1.timeOfDay == 600)
            {
                //if (time_reset == false) return;
                time_reset = true;
                Game1.farmerShouldPassOut = true; //make the farmer collapse.
                player_x = Game1.player.getTileX();
                player_y = Game1.player.getTileY();
                prior_map = Game1.player.currentLocation.name;
                pre_stam = Game1.player.stamina;
                pre_health = Game1.player.health;
                prior_money = Game1.player.money;
            }

        }

        

        void MyWritter()
        {
            //saves the BuildEndurance_data at the end of a new day;
            string myname = Game1.player.name;
            string mylocation = Path.Combine(PathOnDisk, "Night_Owl_Config_");
            //string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation + ".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
                Console.WriteLine("The data file for Night Owl wasn't found. Time to create it!");

                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Player: Night Owl Config. Feel free to edit.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Stay up to 6 AM?: Do you wan't to stay up till 6 AM the next morning?";
                mystring3[3] = stay_up.ToString();

                mystring3[4] = "Custom Lighting Transition?: Do you want to have a lighting transition from 2AM to 5:50 AM? Setting this to false will keep the world dark until the player passes out or goes to bed.";
                mystring3[5] = lighting_transition.ToString();

                mystring3[6] = "Warp to collapse position?: True: Stay in the same place. False: Warp back home.";
                mystring3[7] = warp.ToString();

                
               // mystring3[8] = "Clean out charges mail?: Get rid of the annoying We charged X gold for your health fees, etc.";
              //  mystring3[9] = wipe_mail.ToString();

                mystring3[8] = "Keep Money?: True: Don't be charged for passing out.";
                mystring3[9] = protect_money.ToString();
                
                mystring3[10] = "Keep stamina when staying up? When the farmer passes out at 6, should their stamina be their stamina before collapsing?";
                mystring3[11] = persistant_stamina.ToString();
                mystring3[12] = "Keep health when staying up? When the farmer passes out at 6, should their health be their health before collapsing?";
                mystring3[13] = persistant_health.ToString();

                /*
                mystring3[17] = "Nightly Stamina Value: This is the value of the player's stamina that was saved when the player slept.";
                mystring3[18] = nightly_stamina_value.ToString(); //this should save the player's stamina upon sleeping.
                */

                File.WriteAllLines(mylocation3, mystring3);
            }

            else
            {
                //    Console.WriteLine("HEY IM SAVING DATA");

                //write out the info to a text file at the end of a day.
                mystring3[0] = "Player: Night Owl Config. Feel free to edit.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Stay up to 6 AM?: Do you wan't to stay up till 6 AM the next morning?";
                mystring3[3] = stay_up.ToString();

                mystring3[4] = "Custom Lighting Transition?: Do you want to have a lighting transition from 2AM to 5:50 AM? Setting this to false will keep the world dark until the player passes out or goes to bed.";
                mystring3[5] = lighting_transition.ToString();

                mystring3[6] = "Warp to collapse position?: True: Stay in the same place. False: Warp back home.";
                mystring3[7] = warp.ToString();


                // mystring3[8] = "Clean out charges mail?: Get rid of the annoying We charged X gold for your health fees, etc.";
                //mystring3[9] = wipe_mail.ToString();

                mystring3[8] = "Keep Money?: True: Don't be charged for passing out.";
                mystring3[9] = protect_money.ToString();

                mystring3[10] = "Keep stamina when staying up? When the farmer passes out at 6, should their stamina be their stamina before collapsing?";
                mystring3[11] = persistant_stamina.ToString();
                mystring3[12] = "Keep health when staying up? When the farmer passes out at 6, should their health be their health before collapsing?";
                mystring3[13] = persistant_health.ToString();

                File.WriteAllLines(mylocation3, mystring3);
            }
        }
        void DataLoader()
        {
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(PathOnDisk, "Night_Owl_Config_");
            //string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                Console.WriteLine("Loading Night_Owl_Config");
                lighting_transition = true;
                warp = true;
                stay_up = true;

                persistant_health = true;
                persistant_stamina = true;
                //wipe_mail = true;
                protect_money = true;

            }

            else
            {
                //        Console.WriteLine("HEY THERE IM LOADING DATA");

                //loads the BuildEndurance_data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation3);
                stay_up = Convert.ToBoolean(readtext[3]);
                lighting_transition = Convert.ToBoolean(readtext[5]);  //these array locations refer to the lines in BuildEndurance_data.json
                warp = Convert.ToBoolean(readtext[7]);


                //wipe_mail = Convert.ToBoolean(readtext[9]);
                protect_money = Convert.ToBoolean(readtext[9]);
                persistant_stamina = Convert.ToBoolean(readtext[11]);
                persistant_health = Convert.ToBoolean(readtext[13]);


            }
        }

    }
}
