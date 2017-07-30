using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Menus;

namespace Omegasis.SaveAnywhere
{
    /// <summary>Provides methods for saving and loading game data.</summary>
    internal class SaveUtilities
    {
        public static bool passiveSave;
        public static bool should_ship;
        
        public static string npc_name;
        public static int npc_tile_x;
        public static int npc_tile_y;
        public static string npc_current_map_name;
        public static System.Collections.Generic.List<List<string>> routesFromLocationToLocation = new List<List<string>>();
        public static Microsoft.Xna.Framework.Point npc_point;

        public static string pet_name;
        public static StardewValley.Character my_pet;
        public static string pet_map_name;
        public static int pet_tile_x;
        public static int pet_tile_y;
        public static bool is_pet_outside;
        public static Microsoft.Xna.Framework.Point pet_point;

        public static int player_x_tile;
        public static int player_y_tile;
        public static string players_current_map_name;
        public static int player_game_time;
        public static int player_facing_direction;
        public static bool has_player_warped_yet;

        public static void shipping_check()
        {

            if (Game1.activeClickableMenu != null) return;
            if (SaveUtilities.should_ship == true)
            {
                Game1.activeClickableMenu = new NewShippingMenu(Game1.getFarm().shippingBin);
                SaveUtilities.should_ship = false;
                Game1.getFarm().shippingBin.Clear();
                Game1.getFarm().lastItemShipped = null;
                SaveUtilities.passiveSave = true;
            }
            else
            {
                Game1.activeClickableMenu = new StardewValley.Menus.SaveGameMenu();
            }
        }

        public static void save_game()
        {
            /*

            if (Game1.player.currentLocation.name == "Sewer")
            {
                Log.Error("There is an issue saving in the Sewer. Blame the animals for not being saved to the player's save file.");
                Log.Error("Your data has not been saved. Sorry for the issue.");
                return;
            }
            */

            //if a player has shipped an item, run this code.
            if (Enumerable.Count<Item>((IEnumerable<Item>)Game1.getFarm().shippingBin) > 0)
            {
                should_ship = true;
                //   Game1.endOfNightMenus.Push((IClickableMenu)new ShippingMenu(Game1.getFarm().shippingBin));
                //   Game1.showEndOfNightStuff(); //shows the nightly shipping menu.
                //    Game1.getFarm().shippingBin.Clear(); //clears out the shipping bin to prevent exploits
            }

            try
            {
                shipping_check();
                //  Game1.activeClickableMenu = new StardewValley.Menus.SaveGameMenu();
            }
            catch (Exception rrr)
            {
                Game1.showRedMessage("Can't save here. See log for error.");
                SaveAnywhere.thisMonitor.Log(rrr.ToString(), LogLevel.Error);
            }

            // Game1.activeClickableMenu = new StardewValley.Menus.SaveGameMenu(); //This command is what allows the player to save anywhere as it calls the saving function.

            SaveUtilities.save_player_info();
            SaveUtilities.save_animal_info();
            SaveUtilities.Save_NPC_Info();

            //grab the player's info
            //  player_map_name = StardewValley.Game1.player.currentLocation.name;
            //  player_tile_x = StardewValley.Game1.player.getTileX();
            //  player_tile_Y = StardewValley.Game1.player.getTileY();
            //  player_flop = false;

            //   MyWritter_Player(); //write my info to a text file


            //   MyWritter_Horse();

            //   DataLoader_Settings();  //load settings. Prevents acidental overwrite.
            //   MyWritter_Settings(); //save settings. 

            //Game1.warpFarmer(player_map_name, player_tile_x, player_tile_Y, player_flop); //refresh the player's location just incase. That will prove that they character's info was valid.

            //so this is essentially the basics of the code...
            // Log.Error("IS THIS BREAKING?");
        }

        public static void Save_Horse_Info()
        {


            Horse horse = Utility.findHorse();


            if (horse == null)
            {
                //Game1.getFarm().characters.Add((NPC)new Horse(this.player_tile_x + 1, this.player_tile_Y + 1));
                SaveAnywhere.thisMonitor.Log("NEIGH: No horse exists", LogLevel.Debug);
                return;
            }
            // else
            //   Game1.warpCharacter((NPC)horse, Game1.player.currentLocation.name, StardewValley.Game1.player.getTileLocationPoint(), false, true);




            string myname = StardewValley.Game1.player.name;

            string mylocation = Path.Combine(SaveAnywhere.animal_path, "Horse_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {

                SaveAnywhere.thisMonitor.Log("The horse save info doesn't exist. It will be created when the custom saving method is run. Which is now.", LogLevel.Debug);
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Horse: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Horse Current Map Name";
                mystring3[3] = horse.currentLocation.name.ToString();

                mystring3[4] = "Horse X Position";
                mystring3[5] = horse.getTileX().ToString();

                mystring3[6] = "Horse Y Position";
                mystring3[7] = horse.getTileY().ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }

            else
            {
                //   Console.WriteLine("The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.");
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Horse: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Horse Current Map Name";
                mystring3[3] = horse.currentLocation.name.ToString();

                mystring3[4] = "Horse X Position";
                mystring3[5] = horse.getTileX().ToString();

                mystring3[6] = "Horse Y Position";
                mystring3[7] = horse.getTileY().ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }
        }

        public static void Load_Horse_Info()
        {
            Horse horse = Utility.findHorse();
            if (horse == null)
            {
                SaveAnywhere.thisMonitor.Log("NEIGH: No horse exists", LogLevel.Debug);
                return;
            }
            //   DataLoader_Settings();
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(SaveAnywhere.animal_path, "Horse_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
            }

            else
            {
                string horse_map_name = "";
                int horse_x;
                int horse_y;
                Point horse_point;
                string[] readtext = File.ReadAllLines(mylocation3);
                horse_map_name = Convert.ToString(readtext[3]);
                horse_x = Convert.ToInt32(readtext[5]);
                horse_y = Convert.ToInt32(readtext[7]);
                horse_point.X = horse_x;
                horse_point.Y = horse_y;
                Game1.warpCharacter((NPC)horse, horse_map_name, horse_point, false, true);
            }
        }

        public static void save_animal_info()
        {
            SaveAnywhere.animal_path = Path.Combine(SaveAnywhere.player_path, "Animals");
            if (!Directory.Exists(SaveAnywhere.animal_path))
            {
                Directory.CreateDirectory(SaveAnywhere.animal_path);

            }
            SaveUtilities.Save_Horse_Info();
            SaveUtilities.save_pet_info();
        }

        public static void load_animal_info()
        {
            SaveAnywhere.animal_path = Path.Combine(SaveAnywhere.player_path, "Animals");
            if (!Directory.Exists(SaveAnywhere.animal_path))
            {
                Directory.CreateDirectory(SaveAnywhere.animal_path);

            }
            SaveUtilities.Load_Horse_Info();
            SaveUtilities.Load_pet_Info();
        }

        public static void Save_NPC_Info()
        {
            SaveAnywhere.npc_path = Path.Combine(SaveAnywhere.player_path, "NPC_Save_Info");
            if (!Directory.Exists(SaveAnywhere.npc_path))
            {
                Directory.CreateDirectory(SaveAnywhere.npc_path);
            }
            foreach (var location in Game1.locations)
            {
                foreach (var npc in location.characters)
                {
                    if (npc.IsMonster == true) continue;
                    if (npc is StardewValley.Monsters.Bat || npc is StardewValley.Monsters.BigSlime || npc is StardewValley.Monsters.Bug || npc is StardewValley.Monsters.Cat || npc is StardewValley.Monsters.Crow || npc is StardewValley.Monsters.Duggy || npc is StardewValley.Monsters.DustSpirit || npc is StardewValley.Monsters.Fireball || npc is StardewValley.Monsters.Fly || npc is StardewValley.Monsters.Ghost || npc is StardewValley.Monsters.GoblinPeasant || npc is StardewValley.Monsters.GoblinWizard || npc is StardewValley.Monsters.GreenSlime || npc is StardewValley.Monsters.Grub || npc is StardewValley.Monsters.LavaCrab || npc is StardewValley.Monsters.MetalHead || npc is StardewValley.Monsters.Monster || npc is StardewValley.Monsters.Mummy || npc is StardewValley.Monsters.RockCrab || npc is StardewValley.Monsters.RockGolem || npc is StardewValley.Monsters.Serpent || npc is StardewValley.Monsters.ShadowBrute || npc is StardewValley.Monsters.ShadowGirl || npc is StardewValley.Monsters.ShadowGuy || npc is StardewValley.Monsters.ShadowShaman || npc is StardewValley.Monsters.Skeleton || npc is StardewValley.Monsters.SkeletonMage || npc is StardewValley.Monsters.SkeletonWarrior || npc is StardewValley.Monsters.Spiker || npc is StardewValley.Monsters.SquidKid) continue;
                    npc_name = npc.name;
                    npc_current_map_name = location.name;
                    npc_tile_x = npc.getTileX();
                    npc_tile_y = npc.getTileY();
                    string mylocation = Path.Combine(SaveAnywhere.npc_path, npc.name);
                    string mylocation2 = mylocation;
                    string mylocation3 = mylocation2 + ".txt";
                    string[] mystring3 = new string[20];
                    if (!File.Exists(mylocation3))
                    {
                        SaveAnywhere.thisMonitor.Log("Save Anywhere: The NPC save info for " + npc_name + " doesn't exist. It will be created when the custom saving method is run. Which is now.");
                        //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                        mystring3[0] = "NPC: Save_Anywhere Info. Editing this might break some things.";
                        mystring3[1] = "====================================================================================";

                        mystring3[2] = "NPC Name";
                        mystring3[3] = npc_name.ToString();

                        mystring3[4] = "NPC Current Map Name";
                        mystring3[5] = npc_current_map_name.ToString();

                        mystring3[6] = "NPC X Position";
                        mystring3[7] = npc_tile_x.ToString();

                        mystring3[8] = "NPC Y Position";
                        mystring3[9] = npc_tile_y.ToString();

                        File.WriteAllLines(mylocation3, mystring3);
                    }

                    else
                    {
                        //   Console.WriteLine("The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.");
                        //write out the info to a text file at the end of a day. This will run if it doesnt exist.
                        mystring3[0] = "NPC: Save_Anywhere Info. Editing this might break some things.";
                        mystring3[1] = "====================================================================================";

                        mystring3[2] = "NPC Current Map Name";
                        mystring3[3] = npc_name.ToString();

                        mystring3[4] = "NPC Current Map Name";
                        mystring3[5] = npc_current_map_name.ToString();

                        mystring3[6] = "NPC X Position";
                        mystring3[7] = npc_tile_x.ToString();

                        mystring3[8] = "NPC Y Position";
                        mystring3[9] = npc_tile_y.ToString();
                        File.WriteAllLines(mylocation3, mystring3);
                    }
                }

            }


        }

        public static void Load_NPC_Info()
        {
            List<NPC> npc_list = new List<NPC>();
            foreach (var location in Game1.locations)
            {
                foreach (var npc in location.characters)
                {
                    if (npc.IsMonster == true) continue;
                    if (npc is StardewValley.Monsters.Bat || npc is StardewValley.Monsters.BigSlime || npc is StardewValley.Monsters.Bug || npc is StardewValley.Monsters.Cat || npc is StardewValley.Monsters.Crow || npc is StardewValley.Monsters.Duggy || npc is StardewValley.Monsters.DustSpirit || npc is StardewValley.Monsters.Fireball || npc is StardewValley.Monsters.Fly || npc is StardewValley.Monsters.Ghost || npc is StardewValley.Monsters.GoblinPeasant || npc is StardewValley.Monsters.GoblinWizard || npc is StardewValley.Monsters.GreenSlime || npc is StardewValley.Monsters.Grub || npc is StardewValley.Monsters.LavaCrab || npc is StardewValley.Monsters.MetalHead || npc is StardewValley.Monsters.Monster || npc is StardewValley.Monsters.Mummy || npc is StardewValley.Monsters.RockCrab || npc is StardewValley.Monsters.RockGolem || npc is StardewValley.Monsters.Serpent || npc is StardewValley.Monsters.ShadowBrute || npc is StardewValley.Monsters.ShadowGirl || npc is StardewValley.Monsters.ShadowGuy || npc is StardewValley.Monsters.ShadowShaman || npc is StardewValley.Monsters.Skeleton || npc is StardewValley.Monsters.SkeletonMage || npc is StardewValley.Monsters.SkeletonWarrior || npc is StardewValley.Monsters.Spiker || npc is StardewValley.Monsters.SquidKid) continue;
                    if (npc is StardewValley.NPC || npc is StardewValley.Characters.Cat || npc is StardewValley.Characters.Dog) npc_list.Add(npc);
                }
            }
            foreach (var npc in npc_list)
            {
                if (npc.IsMonster == true) continue;
                if (npc is StardewValley.Monsters.Bat || npc is StardewValley.Monsters.BigSlime || npc is StardewValley.Monsters.Bug || npc is StardewValley.Monsters.Cat || npc is StardewValley.Monsters.Crow || npc is StardewValley.Monsters.Duggy || npc is StardewValley.Monsters.DustSpirit || npc is StardewValley.Monsters.Fireball || npc is StardewValley.Monsters.Fly || npc is StardewValley.Monsters.Ghost || npc is StardewValley.Monsters.GoblinPeasant || npc is StardewValley.Monsters.GoblinWizard || npc is StardewValley.Monsters.GreenSlime || npc is StardewValley.Monsters.Grub || npc is StardewValley.Monsters.LavaCrab || npc is StardewValley.Monsters.MetalHead || npc is StardewValley.Monsters.Monster || npc is StardewValley.Monsters.Mummy || npc is StardewValley.Monsters.RockCrab || npc is StardewValley.Monsters.RockGolem || npc is StardewValley.Monsters.Serpent || npc is StardewValley.Monsters.ShadowBrute || npc is StardewValley.Monsters.ShadowGirl || npc is StardewValley.Monsters.ShadowGuy || npc is StardewValley.Monsters.ShadowShaman || npc is StardewValley.Monsters.Skeleton || npc is StardewValley.Monsters.SkeletonMage || npc is StardewValley.Monsters.SkeletonWarrior || npc is StardewValley.Monsters.Spiker || npc is StardewValley.Monsters.SquidKid) continue;


                SaveAnywhere.npc_path = Path.Combine(SaveAnywhere.player_path, "NPC_Save_Info");
                if (!Directory.Exists(SaveAnywhere.npc_path))
                {
                    Directory.CreateDirectory(SaveAnywhere.npc_path);
                }
                string mylocation = Path.Combine(SaveAnywhere.npc_path, npc.name);
                string mylocation2 = mylocation;
                string mylocation3 = mylocation2 + ".txt";
                string[] mystring3 = new string[20];
                if (!File.Exists(mylocation3))
                {
                    SaveAnywhere.thisMonitor.Log("Missing character file?!?", LogLevel.Error);
                    continue;
                }

                else
                {
                    string[] readtext = File.ReadAllLines(mylocation3);
                    npc_name = Convert.ToString(readtext[3]);
                    npc_current_map_name = Convert.ToString(readtext[5]);
                    npc_tile_x = Convert.ToInt32(readtext[7]);
                    npc_tile_y = Convert.ToInt32(readtext[9]);
                    npc_point = new Microsoft.Xna.Framework.Point();
                    npc_point.X = npc_tile_x;
                    npc_point.Y = npc_tile_y;
                    if (npc_current_map_name == "" || npc_current_map_name == null) continue;
                    //Log.Info("Warped NPC" +npc_name);
                    Game1.warpCharacter((StardewValley.NPC)npc, npc_current_map_name, npc_point, false, true);

                    // npc.updateMovement(Game1.getLocationFromName(npc_current_map_name), Game1.currentGameTime);
                    //npc.moveCharacterOnSchedulePath();
                    //  npc.dayUpdate(Game1.dayOfMonth);
                    //npc_update(npc, Game1.dayOfMonth);

                    //  npc.DirectionsToNewLocation = pathfindToNextScheduleLocation(npc, npc.currentLocation.name, npc.getTileX(), npc.getTileY(), npc.currentLocation.name, 52, 99, 3, "", "");
                    //  npc.updateMovement(npc.currentLocation,Game1.currentGameTime);
                    //npc.Schedule = npc.getSchedule(Game1.dayOfMonth);
                    //npc.moveCharacterOnSchedulePath();


                }
            }
            SaveAnywhere.npc_warp = true;
        }
        private static Stack<Point> addToStackForSchedule(Stack<Point> original, Stack<Point> toAdd)
        {
            if (toAdd == null)
                return original;
            original = new Stack<Point>((IEnumerable<Point>)original);
            while (original.Count > 0)
                toAdd.Push(original.Pop());
            return toAdd;
        }
        private static List<string> getLocationRoute(NPC npc, string startingLocation, string endingLocation)
        {
            foreach (List<string> list in routesFromLocationToLocation)
            {
                if (Enumerable.First<string>((IEnumerable<string>)list).Equals(startingLocation) && Enumerable.Last<string>((IEnumerable<string>)list).Equals(endingLocation) && (npc.gender == 0 || !list.Contains("BathHouse_MensLocker")) && (npc.gender != 0 || !list.Contains("BathHouse_WomensLocker")))
                    return list;
            }
            return (List<string>)null;
        }
        private static SchedulePathDescription pathfindToNextScheduleLocation(NPC npc, string startingLocation, int startingX, int startingY, string endingLocation, int endingX, int endingY, int finalFacingDirection, string endBehavior, string endMessage)
        {
            Stack<Point> stack = new Stack<Point>();
            Point startPoint = new Point(startingX, startingY);
            List<string> list = startingLocation.Equals(endingLocation) ? (List<string>)null : getLocationRoute(npc, startingLocation, endingLocation);
            if (list != null)
            {
                for (int index = 0; index < Enumerable.Count<string>((IEnumerable<string>)list); ++index)
                {
                    GameLocation locationFromName = Game1.getLocationFromName(list[index]);
                    if (index < Enumerable.Count<string>((IEnumerable<string>)list) - 1)
                    {
                        Point warpPointTo = locationFromName.getWarpPointTo(list[index + 1]);
                        if (warpPointTo.Equals(Point.Zero) || startPoint.Equals(Point.Zero))
                            throw new Exception("schedule pathing tried to find a warp point that doesn't exist.");
                        stack = addToStackForSchedule(stack, PathFindController.findPathForNPCSchedules(startPoint, warpPointTo, locationFromName, 30000));
                        startPoint = locationFromName.getWarpPointTarget(warpPointTo);
                    }
                    else
                        stack = addToStackForSchedule(stack, PathFindController.findPathForNPCSchedules(startPoint, new Point(endingX, endingY), locationFromName, 30000));
                }
            }
            else if (startingLocation.Equals(endingLocation))
                stack = PathFindController.findPathForNPCSchedules(startPoint, new Point(endingX, endingY), Game1.getLocationFromName(startingLocation), 30000);
            return new SchedulePathDescription(stack, finalFacingDirection, endBehavior, endMessage);

        }

        public static void save_pet_info()
        {
            if (Game1.player.hasPet() == false) return;
            pet_name = Game1.player.getPetName();
            foreach (var location in Game1.locations)
            {
                foreach (var npc in location.characters)
                {
                    if (npc is StardewValley.Characters.Dog || npc is StardewValley.Characters.Cat)
                    {
                        pet_map_name = location.name;
                        pet_tile_x = npc.getTileX();
                        pet_tile_y = npc.getTileY();
                        is_pet_outside = location.isOutdoors;

                    }

                }

            }
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(SaveAnywhere.animal_path, "Pet_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {

                SaveAnywhere.thisMonitor.Log("Save Anywhere: The pet save info doesn't exist. It will be created when the custom saving method is run. Which is now.", LogLevel.Debug);
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Pet: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Pet Current Map Name";
                mystring3[3] = pet_map_name.ToString();

                mystring3[4] = "Pet X Position";
                mystring3[5] = pet_tile_x.ToString();

                mystring3[6] = "Pet Y Position";
                mystring3[7] = pet_tile_y.ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }

            else
            {
                //   Console.WriteLine("The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.");
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.
                mystring3[0] = "Pet: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Pet Current Map Name";
                mystring3[3] = pet_map_name.ToString();

                mystring3[4] = "Pet X Position";
                mystring3[5] = pet_tile_x.ToString();

                mystring3[6] = "Pet Y Position";
                mystring3[7] = pet_tile_y.ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }

        }

        public static void Load_pet_Info()
        {
            if (Game1.player.hasPet() == false) return;
            //   DataLoader_Settings();
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(SaveAnywhere.animal_path, "Pet_Save_Info_");
            string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation2 + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
            }

            else
            {
                string[] readtext = File.ReadAllLines(mylocation3);
                pet_map_name = Convert.ToString(readtext[3]);
                pet_tile_x = Convert.ToInt32(readtext[5]);
                pet_tile_y = Convert.ToInt32(readtext[7]);
                get_pet();
                pet_point = new Microsoft.Xna.Framework.Point();
                pet_point.X = pet_tile_x;
                pet_point.Y = pet_tile_y;
                Game1.warpCharacter((StardewValley.NPC)my_pet, pet_map_name, pet_point, false, true);
            }
        }

        public static void get_pet()
        {
            if (Game1.player.hasPet() == false) return;
            foreach (var location in Game1.locations)
            {
                foreach (var npc in location.characters)
                {
                    if (npc is StardewValley.Characters.Dog || npc is StardewValley.Characters.Cat)
                    {
                        pet_map_name = location.name;
                        pet_tile_x = npc.getTileX();
                        pet_tile_y = npc.getTileY();
                        is_pet_outside = location.isOutdoors;
                        my_pet = npc;
                    }

                }

            }

        }

        public static void get_player_info()
        {
            get_x();
            get_y();
            get_current_map_name();
            get_facing_direction();
        }

        public static void get_x()
        {
            player_x_tile = Game1.player.getTileX();
        }
        public static void get_y()
        {
            player_y_tile = Game1.player.getTileY();
        }
        public static void get_current_map_name()
        {
            players_current_map_name = Game1.player.currentLocation.name;
        }
        public static void get_facing_direction()
        {
            player_facing_direction = Game1.player.facingDirection;
        }

        public static void save_player_info()
        {
            get_player_info();
            string name = StardewValley.Game1.player.name;
            SaveAnywhere.player_path = Path.Combine(SaveAnywhere.mod_path, "Save_Data", name);
            if (!Directory.Exists(SaveAnywhere.player_path))
            {
                Directory.CreateDirectory(SaveAnywhere.player_path);
            }



            string mylocation = Path.Combine(SaveAnywhere.player_path, "Player_Save_Info_");
            string mylocation2 = mylocation + name;
            string mylocation3 = mylocation2 + ".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
                SaveAnywhere.thisMonitor.Log("Save Anywhere: The custom character save info doesn't exist. It will be created when the custom saving method is run. Which is now.", LogLevel.Info);
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.

                mystring3[0] = "Player: Save_Anywhere Info. Editing this might break some things.";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Player Current Game Time";
                mystring3[3] = Game1.timeOfDay.ToString();

                mystring3[4] = "Player Current Map Name";
                mystring3[5] = players_current_map_name.ToString();

                mystring3[6] = "Player X Position";
                mystring3[7] = player_x_tile.ToString();

                mystring3[8] = "Player Y Position";
                mystring3[9] = player_y_tile.ToString();



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
                mystring3[5] = players_current_map_name.ToString();

                mystring3[6] = "Player X Position";
                mystring3[7] = player_x_tile.ToString();

                mystring3[8] = "Player Y Position";
                mystring3[9] = player_y_tile.ToString();



                File.WriteAllLines(mylocation3, mystring3);
            }

        }

        public static void load_player_info()
        {
            string name = StardewValley.Game1.player.name;
            SaveAnywhere.player_path = Path.Combine(SaveAnywhere.mod_path, "Save_Data", name);
            if (!Directory.Exists(SaveAnywhere.player_path))
            {
                Directory.CreateDirectory(SaveAnywhere.player_path);
            }



            string mylocation = Path.Combine(SaveAnywhere.player_path, "Player_Save_Info_");
            string mylocation2 = mylocation + name;
            string mylocation3 = mylocation2 + ".txt";
            string[] mystring3 = new string[20];

            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                //  Console.WriteLine("Can't load custom save info since the file doesn't exist.");
                SaveUtilities.has_player_warped_yet = true;
            }

            else
            {
                //        Console.WriteLine("HEY THERE IM LOADING DATA");

                string[] readtext = File.ReadAllLines(mylocation3);
                player_game_time = Convert.ToInt32(readtext[3]);
                players_current_map_name = Convert.ToString(readtext[5]);
                player_x_tile = Convert.ToInt32(readtext[7]);
                player_y_tile = Convert.ToInt32(readtext[9]);
                Game1.timeOfDay = player_game_time;

            }
        }

        public static void warp_player()
        {
            GameLocation new_location = Game1.getLocationFromName(players_current_map_name);
            Game1.player.previousLocationName = Game1.player.currentLocation.name;
            Game1.locationAfterWarp = new_location;
            Game1.xLocationAfterWarp = player_x_tile;
            Game1.yLocationAfterWarp = player_y_tile;
            Game1.facingDirectionAfterWarp = player_facing_direction;
            Game1.fadeScreenToBlack();

            Game1.warpFarmer(players_current_map_name, player_x_tile, player_y_tile, false);
            SaveAnywhere.thisMonitor.Log("WARP THE PLAYER");
            Game1.player.faceDirection(player_facing_direction);

            if (Directory.Exists(SaveAnywhere.player_path))
            {
                // Directory.Delete(player_path, true);
            }
        }
    }
}
