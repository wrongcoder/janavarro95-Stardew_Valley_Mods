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


//how to simulate npc movement?

/*
TO DO:

    Can't seem to restore NPC location upon custom loading...


SAVE NPC INFO TO EXTERNAL TXT DATA FILE
read in npc data and warp them upon loading
SIMULATE TIME TO THAT POINT IN THE DAY TO MAKE SURE NPCS GET PLACED PROPPERLY  //alternative if warping breaks


??? ANYTHING ELSE THAT COMES UP???


Save the player's map and tile location and warp them there upon loading the game.
*/
namespace Stardew_Save_Anywhere_Mod
{
    public class Class1 : Mod
    {
        string save_path="";

        string key_binding="K";

        int name_count = 0;

        bool game_loaded = false;
        string player_map_name = "false";
        int player_tile_x;
        int player_tile_Y;
        bool player_flop = false;
        bool warped = false;
        int game_time;

        bool timer = true;

        bool game_updated_and_loaded = true;


        Timer aTimer = new Timer(2000); //fires every X miliseconds


        string npc_name;
        string npc_map_name;
        bool npc_map_outside;
        Microsoft.Xna.Framework.Point npc_point;
        int npc_x;
        int npc_y;
        int r = 3;

        public override void Entry(params object[] objects)
        {
           
            StardewModdingAPI.Events.PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
            StardewModdingAPI.Events.TimeEvents.TimeOfDayChanged += TimeEvents_TimeOfDayChanged;
            StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += TimeEvents_DayOfMonthChanged;
            StardewModdingAPI.Events.GameEvents.OneSecondTick += GameEvents_OneSecondTick;
            StardewModdingAPI.Events.ControlEvents.KeyPressed += ControlEvents_KeyPressed;
        }

        public void TimeEvents_DayOfMonthChanged(object sender, StardewModdingAPI.Events.EventArgsIntChanged e)
        {
            if (game_loaded == true)
            {
                if (save_path != "")
                {
                    File.Delete(save_path);
                    save_path = "";
                }
                game_updated_and_loaded = true; //prevents the next day from being updated
                StardewValley.Game1.player.canMove = true;
            }
        }

        public void ControlEvents_KeyPressed(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            DataLoader_Settings();
          //  key_binding=key_binding.ToUpper();
            if (e.KeyPressed.ToString() ==key_binding)
            {
                //  Log.Info("POOOOO");
                my_save();
            }
            /*
            if (e.KeyPressed.ToString() == "P")
            {
                //  Log.Info("POOOOO");
                MyWritter_NPC(true);
            }
        */
            /*
            if (e.KeyPressed.ToString() == "Z")
            {
                DataLoader_NPC(false);
            }
           */
        }

        public void GameEvents_OneSecondTick(object sender, EventArgs e)
        { //warps the farmer!!! =D
            
            // StardewValley.Game1.player.canMove = true;
            if (game_loaded == true)
            {

                if (save_path == "")
                {
                    warped = true; //prevent premature warping
                }

            
                if (warped == false)
                {
                    DataLoader_Player(); //warps the character and changes the game time.
                    DataLoader_Settings();



                    game_updated_and_loaded = false;
                    if (StardewValley.Game1.player.currentLocation.name != player_map_name)
                    {

                        DataLoader_NPC(false);
                        MyWritter_NPC(true);
                        warped = true;
                        StardewValley.Game1.warpFarmer(player_map_name, player_tile_x, player_tile_Y, player_flop); //player flop is always false. 
                        StardewValley.Game1.warpFarmer(player_map_name, player_tile_x, player_tile_Y, player_flop); //player flop is always false. 
                        StardewValley.Game1.warpFarmer(player_map_name, player_tile_x, player_tile_Y, player_flop); //player flop is always false. 
                        Log.Success("WARPED");
                        timer = false;

                    }

                }
                



                // DataLoader_NPC(false);
                if (warped == true && timer == false)
                {
                    timer = true;

                    aTimer.AutoReset = true;
                    aTimer.Enabled = true;

                    aTimer.Elapsed += ATimer_Elapsed;

                    //simulate time here

                    if (Game1.timeOfDay >= game_time) aTimer.Enabled = false;

                    //aTimer.Enabled = false;
                }


            }

        }

        public void ATimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (game_updated_and_loaded == true) return;
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
                        obj.addedSpeed = 9;


                    }
                }

            }

            if (Game1.timeOfDay >= game_time)
            {
                game_updated_and_loaded = true;
                Game1.player.canMove = true;
            }
        }

        public void TimeEvents_TimeOfDayChanged(object sender, StardewModdingAPI.Events.EventArgsIntChanged e)
        {
            if (Game1.timeOfDay < game_time)
            {
                if (game_loaded == true)
                {



                }

                /*
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
                */ //This originally was the code to preserve soil, but Ape apparently covered that in the save functionality.

            } //end game loaded if
        }

        public void PlayerEvents_LoadedGame(object sender, StardewModdingAPI.Events.EventArgsLoadedGameChanged e)
        {
            game_loaded = true;
            MyWritter_Settings();
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

            }

            else
            {
                //        Console.WriteLine("HEY THERE IM LOADING DATA");

                //loads the BuildEndurance_data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation3);
                game_time = Convert.ToInt32(readtext[3]);
                player_map_name = Convert.ToString(readtext[5]);
                player_tile_x = Convert.ToInt32(readtext[7]);
                player_tile_Y = Convert.ToInt32(readtext[9]);

            }
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
              //  Log.Info("KEY TIME");
             }

            else
            {
                //        Console.WriteLine("HEY THERE IM LOADING DATA");

                //loads the BuildEndurance_data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation3);
                key_binding = Convert.ToString(readtext[3]);
               // Log.Info(key_binding);
               // Log.Info(Convert.ToString(readtext[3]));

            }
        }

        void MyWritter_Settings()
        {
            string myname = StardewValley.Game1.player.name;

            string mylocation = Path.Combine(PathOnDisk, "Save_Anywhere_Config");
            string mylocation2 = mylocation;
            string mylocation3 = mylocation2 + ".txt";
            save_path = mylocation3;
            string[] mystring3 = new string[4];
            if (!File.Exists(mylocation3))
            {
                Console.WriteLine("The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.");
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

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

        void MyWritter_Player()
        {

            MyWritter_Settings();
            string myname = StardewValley.Game1.player.name;

            string mylocation = Path.Combine(PathOnDisk, "Player_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            save_path = mylocation3;
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

        void my_save()
        {
            if (Enumerable.Count<Item>((IEnumerable<Item>)Game1.getFarm().shippingBin) > 0)
            {
                Game1.endOfNightMenus.Push((IClickableMenu)new ShippingMenu(Game1.getFarm().shippingBin));
                Game1.showEndOfNightStuff(); //pushes the game forward sadly


                Game1.getFarm().shippingBin.Clear();
            }

            Game1.activeClickableMenu = new StardewValley.Menus.SaveGameMenu();
            //NOT SURE IF THIS IS REDUNDANT BUT BEST BE SAFE

            //GRAB THE CHARACTER VALUES BEFORE HAND

            //AND WARP THEM OFFSET A BIT


            player_map_name = StardewValley.Game1.player.currentLocation.name;
            player_tile_x = StardewValley.Game1.player.getTileX();
            player_tile_Y = StardewValley.Game1.player.getTileY();
            player_flop = false;

            MyWritter_Player();

            MyWritter_NPC(false);


            DataLoader_Settings();
            MyWritter_Settings();
            
            //player_tile_x += 5;
            //player_tile_Y += 5;

            Game1.warpFarmer(player_map_name, player_tile_x, player_tile_Y, player_flop);






            //so this is essentially the basics of the code...
            Log.Error("IS THIS BREAKING?");
        }


        void DataLoader_NPC(bool sleep)
        {
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(PathOnDisk, "NPC_Original_Save_Info_");
            string mylocation_save = Path.Combine(PathOnDisk, "NPC_Original_Save_Info_");

            if (sleep == true)
            {
                mylocation = Path.Combine(PathOnDisk, "NPC_Save_Info_");
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


            name_count++;

            int i = 3;


            if (sleep == true)
            {
                mylocation = Path.Combine(PathOnDisk, "NPC_Save_Info_");
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

                //loads the BuildEndurance_data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation3);
                string[] readtexty = File.ReadAllLines(myloc3B);

                int j = 0;
                List<NPC> list = new List<NPC>();

                foreach (StardewValley.GameLocation asdf in Game1.locations)
                {
                    var NPClocationd = (GameLocation)asdf;
                    Log.Error(asdf.name);
                    System.Threading.Thread.Sleep(10);
                    foreach (StardewValley.NPC obj in NPClocationd.characters)
                    {
                        Log.Success(obj.name);
                        //  System.Threading.Thread.Sleep(1000);

                        list.Add(obj);

                    }
                    foreach (NPC item in list)
                    {
                        i = 3;
                        while (item.name != readtexty[i])
                        {
                            i += 11;
                        }

                        Log.Info(i);


                        //Log.Info(obj.getTileLocationPoint());
                        //npc_point = obj.getTileLocationPoint();
                        //System.Threading.Thread.Sleep(5);

                    //    Console.WriteLine("LOADER ELSE FUUUU");
                        //  System.Threading.Thread.Sleep(100);
                        //write out the info to a text file at the end of a day. This will run if it doesnt exist.
                        if (readtext[i] == "") break;
                        if (readtext[i] == "\n") break;

                        //npc_name = Convert.ToString(readtexty[i]);
                        Log.Info(npc_name);
                        Log.Error("WHAT IS THIS?");
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




                        Game1.warpCharacter(item, npc_map_name, npc_point, true, true);
                        //list.Remove(item);

                        i = 3;
                    }
                    list.Clear();

                }

            
                    //timer = false;
                } //end for eaches
            }

        void MyWritter_NPC(bool sleep)
        {
            //saves the BuildEndurance_data at the end of a new day;
            string myname = StardewValley.Game1.player.name;
            string mylocation;
            string mylocation2;
            if (sleep == false)
            {
                 mylocation = Path.Combine(PathOnDisk, "NPC_Save_Info_");
                mylocation2 = mylocation;
            }
            else
            {
                mylocation = Path.Combine(PathOnDisk, "NPC_Original_Save_Info_");
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