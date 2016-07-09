using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI;
using StardewValley.Menus;
using System.IO;
using System.Timers;
using System.Windows.Input;
using StardewValley.Characters;
using Microsoft.Xna.Framework;


/*
TO DO:

*/
namespace Stardew_Save_Anywhere_Mod
{
    public class Class1 : Mod
    {
        // string save_path="";

        bool initialize = false;


        string key_binding="K";

        bool game_loaded = false;
        string player_map_name;
        int player_tile_x;
        int player_tile_Y;
        bool player_flop = false;
        
        int game_time;
        double timer_interval = 3500;

        bool timer = true;

        bool game_updated_and_loaded;
        bool time_updated;

        bool warped = false;

        bool simulate_time;
        bool warp_character;


        Timer aTimer = new Timer(3500); //fires every X miliseconds. 3500 is 4 times faster than the game's normal speed //originally this was 2000


        string npc_name;
        string npc_map_name;
        bool npc_map_outside;
        Microsoft.Xna.Framework.Point npc_point;
        int npc_x;
        int npc_y;

        public override void Entry(params object[] objects)
        {
            //set up all of my events here
            StardewModdingAPI.Events.PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
          //  StardewModdingAPI.Events.TimeEvents.TimeOfDayChanged += TimeEvents_TimeOfDayChanged;  Not used as of version 1.0.1
            StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += TimeEvents_DayOfMonthChanged;
            StardewModdingAPI.Events.GameEvents.OneSecondTick += GameEvents_OneSecondTick;
            StardewModdingAPI.Events.ControlEvents.KeyPressed += ControlEvents_KeyPressed;
        }

        public void TimeEvents_DayOfMonthChanged(object sender, StardewModdingAPI.Events.EventArgsIntChanged e)
        {
            if (game_loaded == true)
            {
               file_clean_up();
                game_time = 600; //resets the game time so that simulation doesn't happen every day.
                game_updated_and_loaded = true; //prevents the next day from being updated
                StardewValley.Game1.player.canMove = true;  //do I even use this?
            }
        }

        public void ControlEvents_KeyPressed(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            //DataLoader_Settings(); //update the key if players changed it while playing.
            if (e.KeyPressed.ToString() ==key_binding) //if the key is pressed, load my cusom save function
            {
                my_save();
            }
        }

        public void GameEvents_OneSecondTick(object sender, EventArgs e)
        { //updates the info every second
           
            if (game_loaded == true)
            {

                if (initialize == false)
                {
                    
                 //   DataLoader_Settings(); //load up the mod config file.
                    DataLoader_Horse();
                    DataLoader_NPC(false); //loads the NPC's with original location info
                    initialize = true;


                    if (simulate_time == true)
                    {
                        timer = false;
                       // simulate_time = false;
                    }
                    else timer = true;

                    if (warp_character == true && game_updated_and_loaded == false)
                    {
                        warped = false;
                        //warp_character = false;
                    }

                }

               
                else warped = true;

                DataLoader_Player(); //warps the character and changes the game time. WTF HOW DID THIS BREAK???

                if (warped == false)
                {

                    game_updated_and_loaded = false;
                    if (StardewValley.Game1.player.currentLocation.name != player_map_name  && warped==false)
                    {
                        MyWritter_NPC(true); //writes in the NPC's info. May be redundant?
                        warped = true; 
                        StardewValley.Game1.warpFarmer(player_map_name, player_tile_x, player_tile_Y, player_flop); //player flop is always false. //Just incase I run this a couple of times.
                        StardewValley.Game1.warpFarmer(player_map_name, player_tile_x, player_tile_Y, player_flop); //player flop is always false. //probably bad programming practice.
                       StardewValley.Game1.warpFarmer(player_map_name, player_tile_x, player_tile_Y, player_flop); //player flop is always false. 
                        Log.Success("WARPED");
                        //timer = false; //activate my timer. False means that it hasn't been initialized.
                        game_updated_and_loaded = true;

                      }

                }
               



                // DataLoader_NPC(false);
                if (warped == true && timer == false)
                {
                    //load config info for the timer here.

                    aTimer.Interval = timer_interval; //this should update the timer to run at the config amount of seconds.

                    timer = true; //timer is now running

                    aTimer.AutoReset = true; 
                    aTimer.Enabled = true;

                    aTimer.Elapsed += ATimer_Elapsed;

                    //simulate time here

                    if (Game1.timeOfDay >= game_time) aTimer.Enabled = false; //disable my timer

                    //aTimer.Enabled = false;
                }


            }

        }

        public void ATimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (time_updated == true) return;
            if (Game1.timeOfDay < game_time)
            {
                Game1.performTenMinuteClockUpdate();
                /*
                if (create_original == false)
                {
                    MyWritter_NPC(true);
                    create_original = true;
                }
                */
                Log.Info("This is the current time: Keep simulating :" + Game1.timeOfDay);


                foreach (StardewValley.GameLocation asdf in Game1.locations)
                {
                    GameLocation NPClocation = (GameLocation)asdf;
                    foreach (StardewValley.NPC obj in NPClocation.characters)
                    {
                        obj.addedSpeed = 7; //changes how fast npc's move in the world. Default added speed I put was 9 when timer_interval was 2000 miliseconds
                    }
                }

            }

            if (Game1.timeOfDay >= game_time)
            {
                time_updated = true;
                Game1.player.canMove = true;
                game_time = 600; //reset the game_time so that oversimulation doesn't happen again while playing.
            }
        }
    
        /*
        public void TimeEvents_TimeOfDayChanged(object sender, StardewModdingAPI.Events.EventArgsIntChanged e)
        {
            if (Game1.timeOfDay < game_time)
            {

                if (game_loaded == true)
                {
                    //do nothing right now. I might need this later though.
                }

                
                GameLocation farm = StardewValley.Game1.getLocationFromName("Farm");
                StardewValley.TerrainFeatures.HoeDirt mydirt;
                foreach (StardewValley.TerrainFeatures.TerrainFeature dirty in farm.terrainFeatures.Values)
                {
                 //   Log.Error("IS THIS WRONG?");
                    if (dirty is StardewValley.TerrainFeatures.HoeDirt)
                    {
                       // Log.Error("JELLO SAUCE!!!");
                        mydirt =(StardewValley.TerrainFeatures.HoeDirt)dirty;
                        Log.Info("CONVERSION");
                        if (mydirt.state.Equals(0)) //0 is dry, 1 is watered
                        {

                            Log.Error("NEED SOME WATER");
                        }

                        if (mydirt.state.Equals(1)) //0 is dry, 1 is watered
                        {

                            Log.Success("NICE AND WET");
                        }

                    }

                }
                 //This originally was the code to preserve soil, but Ape apparently covered that in the save functionality. Still I'll keep it here incase I need to update the farm later.

            } //end game loaded if
        }
*/
        public void PlayerEvents_LoadedGame(object sender, StardewModdingAPI.Events.EventArgsLoadedGameChanged e)
        {
            game_loaded = true;
            DataLoader_Settings();
            MyWritter_Settings();
            if (simulate_time == true) aTimer = new Timer(timer_interval);
        }


    

        void DataLoader_Settings()
        {
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(PathOnDisk, "Save_Anywhere_Config");
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                //  Console.WriteLine("Can't load custom save info since the file doesn't exist.");

                key_binding = "K";
                timer_interval = 3500;
                warp_character = true;
                simulate_time = true;
              //  Log.Info("KEY TIME");
             }

            else
            {
                //        Console.WriteLine("HEY THERE IM LOADING DATA");
                string[] readtext = File.ReadAllLines(mylocation3);
                key_binding = Convert.ToString(readtext[3]);
               timer_interval = Convert.ToDouble(readtext[5]);
                simulate_time = Convert.ToBoolean(readtext[7]);
                warp_character = Convert.ToBoolean(readtext[9]);


                // Log.Info(key_binding);
                // Log.Info(Convert.ToString(readtext[3]));

            }
        }

        void MyWritter_Settings()
        {

            //write all of my info to a text file.
            string myname = StardewValley.Game1.player.name;

            string mylocation = Path.Combine(PathOnDisk, "Save_Anywhere_Config");
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

                mystring3[4] = "Timer interval for NPCs. Default 3500 which simulates game time at 4x speed.";
                mystring3[5] = timer_interval.ToString();

                mystring3[6] = "Simulate game time? Game time will be sped up until restored before saving";
                mystring3[7] = simulate_time.ToString();

                mystring3[8] = "Warp player when loading?";
                mystring3[9] = warp_character.ToString();


                File.WriteAllLines(mylocation3, mystring3);

            }

            else
            {

                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Config: Save_Anywhere Info. Feel free to mess with these settings.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Key binding for saving anywhere. Press this key to save anywhere!";
                mystring3[3] = key_binding.ToString();

                mystring3[4] = "Timer interval for NPCs. Default 3500 which simulates game time at 4x speed.";
                mystring3[5] = timer_interval.ToString();

                mystring3[6] = "Simulate game time? Game time will be sped up until restored before saving";
                mystring3[7] = simulate_time.ToString();

                mystring3[8] = "Warp player when loading?";
                mystring3[9] = warp_character.ToString();

                File.WriteAllLines(mylocation3, mystring3);
            }
        }

        void MyWritter_Player()
        {

            MyWritter_Settings();
            string myname = StardewValley.Game1.player.name;

            string mylocation = Path.Combine(PathOnDisk, "Player_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
                Console.WriteLine("The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.");
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Player: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player Current Game Time";
                mystring3[3] = Game1.timeOfDay.ToString();

                mystring3[4] = "Player Current Map Name";
                mystring3[5] = player_map_name.ToString();

                mystring3[6] = "Player X Position";
                mystring3[7] = player_tile_x.ToString();

                mystring3[8] = "Player Y Position";
                mystring3[9] = player_tile_Y.ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }

            else
            {

                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Player: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player Current Game Time";
                mystring3[3] = Game1.timeOfDay.ToString();

                mystring3[4] = "Player Current Map Name";
                mystring3[5] = player_map_name.ToString();

                mystring3[6] = "Player X Position";
                mystring3[7] = player_tile_x.ToString();

                mystring3[8] = "Player Y Position";
                mystring3[9] = player_tile_Y.ToString();

                File.WriteAllLines(mylocation3, mystring3);
            }
        }

        void DataLoader_Player()
        {
            //   DataLoader_Settings();
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(PathOnDisk, "Player_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";

            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                //  Console.WriteLine("Can't load custom save info since the file doesn't exist.");
                warped = true;
            }

            else
            {
                //        Console.WriteLine("HEY THERE IM LOADING DATA");

                string[] readtext = File.ReadAllLines(mylocation3);
                game_time = Convert.ToInt32(readtext[3]);
                player_map_name = Convert.ToString(readtext[5]);
                player_tile_x = Convert.ToInt32(readtext[7]);
                player_tile_Y = Convert.ToInt32(readtext[9]);


            }
        }


        void MyWritter_Horse()
        {

            Horse horse = Utility.findHorse();


            if (horse == null)
            {
                //Game1.getFarm().characters.Add((NPC)new Horse(this.player_tile_x + 1, this.player_tile_Y + 1));
                Log.Info("NEIGH: No horse exists");
                return;
            }
           // else
             //   Game1.warpCharacter((NPC)horse, Game1.player.currentLocation.name, StardewValley.Game1.player.getTileLocationPoint(), false, true);




            string myname = StardewValley.Game1.player.name;

            string mylocation = Path.Combine(PathOnDisk, "Horse_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
 

                Console.WriteLine("The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.");
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Horse: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player Current Map Name";
                mystring3[3] = horse.currentLocation.name.ToString();

                mystring3[4] = "Player X Position";
                mystring3[5] = horse.getTileX().ToString();

                mystring3[6] = "Player Y Position";
                mystring3[7] = horse.getTileY().ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }

            else
            {
             //   Console.WriteLine("The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.");
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Horse: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player Current Map Name";
                mystring3[3] = horse.currentLocation.name.ToString();

                mystring3[4] = "Player X Position";
                mystring3[5] = horse.getTileX().ToString();

                mystring3[6] = "Player Y Position";
                mystring3[7] = horse.getTileY().ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }
        }

        void DataLoader_Horse()
        {
            Horse horse = Utility.findHorse();
            if (horse == null)
            {
                //Game1.getFarm().characters.Add((NPC)new Horse(this.player_tile_x + 1, this.player_tile_Y + 1));
                Log.Info("NEIGH: No horse exists");
                return;
            }
            // else
               //Game1.warpCharacter((NPC)horse, Game1.player.currentLocation.name, StardewValley.Game1.player.getTileLocationPoint(), false, true);





            //   DataLoader_Settings();
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(PathOnDisk, "Horse_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                //  Console.WriteLine("Can't load custom save info since the file doesn't exist.");

            }

            else
            {
                string horse_map_name = "";
                int horse_x;
                int horse_y;

                Point horse_point;

                //        Console.WriteLine("HEY THERE IM LOADING DATA");
                string[] readtext = File.ReadAllLines(mylocation3);
                horse_map_name = Convert.ToString(readtext[3]);
               horse_x = Convert.ToInt32(readtext[5]);
               horse_y = Convert.ToInt32(readtext[7]);

                horse_point.X = horse_x;
                horse_point.Y = horse_y;

                Game1.warpCharacter((NPC)horse, horse_map_name, horse_point, false, true);

            }
        }


        void my_save()
        {
            if (Game1.player.currentLocation.name == "CommunityCenter")
            {
                Log.Error("There is an issue saving in the community center. Blame the Junimos not being saved to the player's save file.");
                Log.Error("Your data has not been saved. Sorry for the issue.");
               return;
            }

            if (Game1.player.currentLocation.name == "Sewer")
            {
                Log.Error("There is an issue saving in the Sewer. Blame the animals for not being saved to the player's save file.");
                Log.Error("Your data has not been saved. Sorry for the issue.");
                return;
            }


            //if a player has shipped an item, run this code.
            if (Enumerable.Count<Item>((IEnumerable<Item>)Game1.getFarm().shippingBin) > 0)
            {
                Game1.endOfNightMenus.Push((IClickableMenu)new ShippingMenu(Game1.getFarm().shippingBin));
                Game1.showEndOfNightStuff(); //shows the nightly shipping menu.
                Game1.getFarm().shippingBin.Clear(); //clears out the shipping bin to prevent exploits
            }

            Game1.activeClickableMenu = new StardewValley.Menus.SaveGameMenu(); //This command is what allows the player to save anywhere as it calls the saving function.


            //grab the player's info
            player_map_name = StardewValley.Game1.player.currentLocation.name;
            player_tile_x = StardewValley.Game1.player.getTileX();
            player_tile_Y = StardewValley.Game1.player.getTileY();
            player_flop = false;

            MyWritter_Player(); //write my info to a text file


            MyWritter_Horse();

            DataLoader_Settings();  //load settings. Prevents acidental overwrite.
            MyWritter_Settings(); //save settings. 

            //Game1.warpFarmer(player_map_name, player_tile_x, player_tile_Y, player_flop); //refresh the player's location just incase. That will prove that they character's info was valid.

            //so this is essentially the basics of the code...
           // Log.Error("IS THIS BREAKING?");
        }


        void file_clean_up()
        {
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(PathOnDisk, "Player_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";

            if (File.Exists(mylocation3)) //delete the custom save when going to bed.
            {
                File.Delete(mylocation3); //clean up the player's save info
            }


             mylocation = Path.Combine(PathOnDisk, "Horse_Save_Info_");
             mylocation2 = mylocation + myname;
             mylocation3 = mylocation2 + ".txt";

            if (File.Exists(mylocation3)) //delete the custom save when going to bed.
            {
                File.Delete(mylocation3); //clean up the player's horse save info
            }

        }


        void DataLoader_NPC(bool sleep)
        {
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(PathOnDisk, "NPC_Original_Save_Info_"+myname);
            string mylocation_save = Path.Combine(PathOnDisk, "NPC_Original_Save_Info_"+myname);

            if (sleep == true)
            {
                mylocation = Path.Combine(PathOnDisk, "NPC_Save_Info_"+myname);
            }

            string myloc2B = mylocation_save;
            string mylocation2 = mylocation;

            string mylocation3 = mylocation2 + ".txt";
            string myloc3B = myloc2B + ".txt";

            if (!File.Exists(myloc3B)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {

              //  Log.Error(" I CAN NOT LOAD Can't load original save info since the file doesn't exist.");
                return;
            }



            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {

             //   Log.Error(" I CAN NOT LOAD Can't load custom save info since the file doesn't exist.");
                return;
            }

            int i = 3;


            if (sleep == true)
            {
                mylocation = Path.Combine(PathOnDisk, "NPC_Save_Info_"+myname);
            }

            if (!File.Exists(myloc3B)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {

//                Log.Error(" I CAN NOT LOAD Can't load original save info since the file doesn't exist.");

            }



            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {

              //  Log.Error(" I CAN NOT LOAD Can't load custom save info since the file doesn't exist.");

            }

            else
            {

                //        Console.WriteLine("HEY THERE IM LOADING DATA");

                //Saves all of the info for NPC's into a file.
                string[] readtext = File.ReadAllLines(mylocation3);
                string[] readtexty = File.ReadAllLines(myloc3B);

                int j = 0;
                List<NPC> list = new List<NPC>(); //This will collect all of the info for the NPC's and save it into a list

                foreach (StardewValley.GameLocation asdf in Game1.locations)
                {
                    var NPClocationd = (GameLocation)asdf;
                    Log.Error(asdf.name); //show the loaded location's name.
                    System.Threading.Thread.Sleep(50); //prevent the game from loading characters too quickly by delaying time 10 miliseconds.
                    if (asdf.name == "Farm") continue;
                    if (asdf.name == "CommunityCenter") continue;
                    foreach (StardewValley.NPC obj in NPClocationd.characters)
                    {
                        Log.Success(obj.name);

                        list.Add(obj); //add the character to the list. Warping them inside of this loop here breaks things.

                    }

                    foreach (NPC item in list) //iterate across my NPC list
                    {
                        i = 3;
                        while (item.name != readtexty[i]) //look across the NPC_Origina_Info file
                        {
                            i += 11;
                        }

                        Log.Info(i); //tell me where I am at. Line # = i+1

                        //write out the info to a text file at the end of a day. This will run if it doesnt exist.
                        if (readtext[i] == "") break;
                        if (readtext[i] == "\n") break;

                        //npc_name = Convert.ToString(readtexty[i]);
                        Log.Info(npc_name);
                       // Log.Error("WHAT IS THIS?");
                        //System.Threading.Thread.Sleep(1000);
                        i += 2;

                        npc_map_name = Convert.ToString(readtexty[i]);
                        Log.Info(npc_map_name);
                        //System.Threading.Thread.Sleep(1000);
                        i += 2;


                        npc_map_outside = Convert.ToBoolean(readtexty[i]);
                        Log.Info(npc_map_outside);
                        //System.Threading.Thread.Sleep(1000);
                        i += 2;


                        npc_x = Convert.ToInt32(readtexty[i]);
                        Log.Info(npc_x);
                        //System.Threading.Thread.Sleep(1000);
                        i += 2;


                        npc_y = Convert.ToInt32(readtexty[i]);
                        Log.Info(npc_y);
                        //System.Threading.Thread.Sleep(3000);
                        i += 3;

                        Log.Info(i);
                        // System.Threading.Thread.Sleep(100);

                        npc_point.X = npc_x;
                        npc_point.Y = npc_y;

                        Microsoft.Xna.Framework.Vector2 myvector2;

                        myvector2.X = npc_x;
                        myvector2.Y = npc_y;

                        Log.Info("character warp!");

                        //basically I just filled the console with debugging information. You are welcome.


                        Game1.warpCharacter(item, npc_map_name, npc_point, true, true); //warp my npc back to original location
                        //list.Remove(item);

                        i = 3;
                    }
                    list.Clear(); //clean up my list

                }

            
                    //timer = false;
                } //end for eaches
            }

        void MyWritter_NPC(bool sleep)
        {
            //basically grabs all of the Npc's info and saves it to a text document. Takes a second or so.


            string myname = StardewValley.Game1.player.name;
            string mylocation;
            string mylocation2;
            if (sleep == false)
            {
                 mylocation = Path.Combine(PathOnDisk, "NPC_Save_Info_"+myname);
                mylocation2 = mylocation;
            }
            else
            {
                mylocation = Path.Combine(PathOnDisk, "NPC_Original_Save_Info_"+myname);
                mylocation2 = mylocation;
            }
            



            string mylocation3 = mylocation2 + ".txt";
            int counter = 1;
            GameLocation NPClocation;
            foreach (StardewValley.GameLocation asdf in Game1.locations)
            {
                NPClocation = (GameLocation)asdf;


                foreach (StardewValley.NPC obj in NPClocation.characters)
                {
                    counter += 11;
                }
            }




                    string[] mystring3 = new string[counter];
            if (!File.Exists(mylocation3))
            {
                
                int i = 0;
                mystring3[i] = "NPC : SAVE ANYWHERE INFO. DON'T TOUCH THIS";
                i++;
                foreach (StardewValley.GameLocation asdf in Game1.locations)
                {
                    NPClocation = (GameLocation)asdf;

                    if (NPClocation.name == "CommunityCenter")
                    {
                        continue;
                    }

                  
                    foreach (StardewValley.NPC obj in NPClocation.characters)
                    {
                        //grab all of the NPC INFO.
                     //   Log.Info(obj.getName());
                        npc_name = obj.getName();
                        System.Threading.Thread.Sleep(5);

                   //     Log.Info(obj.currentLocation.name);
                        npc_map_name = obj.currentLocation.name;
                        System.Threading.Thread.Sleep(5);


                   //     Log.Info(NPClocation.isOutdoors);
                        npc_map_outside = NPClocation.isOutdoors;
                        System.Threading.Thread.Sleep(5);

                        npc_point.X = obj.getTileX();
                        npc_point.Y = obj.getTileY();

                        //Log.Info(obj.getTileLocationPoint());
                        //npc_point = obj.getTileLocationPoint();
                        //System.Threading.Thread.Sleep(5);

                        Log.Success("Let's save all of the NPCS in the world!");
                        //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                        
                
                        mystring3[i] = "====================================================================================";
                        i++;
                        mystring3[i] = "NPC NAME";
                        i++;
                        mystring3[i] = obj.getName().ToString();
                        i++;

                        mystring3[i] = "NPC Current Map Name";
                        i++;
                        mystring3[i] = obj.currentLocation.name.ToString();
                        i++;

                        mystring3[i] = "NPC IS OUTSIDE?";
                        i++;
                        mystring3[i] = NPClocation.isOutdoors.ToString();
                        i++;



                        mystring3[i] = "NPC TILE LOCATION : X";
                        i++;
                        mystring3[i] = npc_point.X.ToString();
                        i++;

                        mystring3[i] = "NPC TILE LOCATION : Y";
                        i++;
                        mystring3[i] = npc_point.Y.ToString();
                        i++;


                        //NPCS WILL FIGURE OUT MOVEMENT ON THEIR OWN.
                        //I CAN SIMPLY (SIMULATE TIME) OR (WARP THEM??)
                    }
                }

                File.WriteAllLines(mylocation3, mystring3);
            }

            else
            {
                int i = 0;
                mystring3[i] = "NPC : SAVE ANYWHERE INFO. DON'T TOUCH THIS";
                i++;
                foreach (StardewValley.GameLocation asdf in Game1.locations)
                {
                    NPClocation = (GameLocation)asdf;


                    foreach (StardewValley.NPC obj in NPClocation.characters)
                    {
                        //grab all of the NPC INFO.
                        Log.Info(obj.getName());
                        npc_name = obj.getName();
                        System.Threading.Thread.Sleep(5);

                        Log.Info(obj.currentLocation.name);
                        npc_map_name = obj.currentLocation.name;
                        System.Threading.Thread.Sleep(5);


                        Log.Info(NPClocation.isOutdoors);
                        npc_map_outside = NPClocation.isOutdoors;
                        System.Threading.Thread.Sleep(5);

                        npc_point.X = obj.getTileX();
                        npc_point.Y = obj.getTileY();

                        //Log.Info(obj.getTileLocationPoint());
                        //npc_point = obj.getTileLocationPoint();
                        //System.Threading.Thread.Sleep(5);

                        Log.Success("Let's save all of the NPCS in the world!");
                        //write out the info to a text file at the end of a day. This will run if it doesnt exist.



                        mystring3[i] = "====================================================================================";
                        i++;
                        mystring3[i] = "NPC NAME";
                        i++;
                        mystring3[i] = obj.getName().ToString();
                        i++;

                        mystring3[i] = "NPC Current Map Name";
                        i++;
                        mystring3[i] = obj.currentLocation.name.ToString();
                        i++;

                        mystring3[i] = "NPC IS OUTSIDE?";
                        i++;
                        mystring3[i] = NPClocation.isOutdoors.ToString();
                        i++;



                        mystring3[i] = "NPC TILE LOCATION : X";
                        i++;
                        mystring3[i] = npc_point.X.ToString();
                        i++;

                        mystring3[i] = "NPC TILE LOCATION : Y";
                        i++;
                        mystring3[i] = npc_point.Y.ToString();
                        i++;


                        //NPCS WILL FIGURE OUT MOVEMENT ON THEIR OWN.
                        //I CAN SIMPLY (SIMULATE TIME) OR (WARP THEM??)
                    }
                }

                File.WriteAllLines(mylocation3, mystring3);
            }
                }
            }
        }
     //end class