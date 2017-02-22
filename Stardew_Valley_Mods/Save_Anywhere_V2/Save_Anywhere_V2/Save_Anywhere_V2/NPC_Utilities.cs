using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Save_Anywhere_V2.Save_Utilities
{
    class NPC_Utilities
    {
        public static string npc_name;
        public static int npc_tile_x;
        public static int npc_tile_y;
        public static string npc_current_map_name;
     public static System.Collections.Generic.List<List<string>> routesFromLocationToLocation = new List<List<string>>();
        public static Microsoft.Xna.Framework.Point npc_point;

        public static void Save_NPC_Info()
        {
            Save_Anywhere_V2.Mod_Core.npc_path = Path.Combine(Save_Anywhere_V2.Mod_Core.player_path, "NPC_Save_Info");
            if (!Directory.Exists(Save_Anywhere_V2.Mod_Core.npc_path))
            {
                Directory.CreateDirectory(Save_Anywhere_V2.Mod_Core.npc_path);
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
                    string mylocation = Path.Combine(Save_Anywhere_V2.Mod_Core.npc_path, npc.name);
                    string mylocation2 = mylocation;
                    string mylocation3 = mylocation2 + ".txt";
                    string[] mystring3 = new string[20];
                    if (!File.Exists(mylocation3))
                    {

                        Log.Info("Save Anywhere: The NPC save info for " + npc_name + " doesn't exist. It will be created when the custom saving method is run. Which is now.");
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
            foreach(var npc in npc_list) { 
                    if (npc.IsMonster == true) continue;
                    if (npc is StardewValley.Monsters.Bat || npc is StardewValley.Monsters.BigSlime || npc is StardewValley.Monsters.Bug || npc is StardewValley.Monsters.Cat || npc is StardewValley.Monsters.Crow || npc is StardewValley.Monsters.Duggy || npc is StardewValley.Monsters.DustSpirit || npc is StardewValley.Monsters.Fireball || npc is StardewValley.Monsters.Fly || npc is StardewValley.Monsters.Ghost || npc is StardewValley.Monsters.GoblinPeasant || npc is StardewValley.Monsters.GoblinWizard || npc is StardewValley.Monsters.GreenSlime || npc is StardewValley.Monsters.Grub || npc is StardewValley.Monsters.LavaCrab || npc is StardewValley.Monsters.MetalHead || npc is StardewValley.Monsters.Monster || npc is StardewValley.Monsters.Mummy || npc is StardewValley.Monsters.RockCrab || npc is StardewValley.Monsters.RockGolem || npc is StardewValley.Monsters.Serpent || npc is StardewValley.Monsters.ShadowBrute || npc is StardewValley.Monsters.ShadowGirl || npc is StardewValley.Monsters.ShadowGuy || npc is StardewValley.Monsters.ShadowShaman || npc is StardewValley.Monsters.Skeleton || npc is StardewValley.Monsters.SkeletonMage || npc is StardewValley.Monsters.SkeletonWarrior || npc is StardewValley.Monsters.Spiker || npc is StardewValley.Monsters.SquidKid) continue;


                Save_Anywhere_V2.Mod_Core.npc_path = Path.Combine(Save_Anywhere_V2.Mod_Core.player_path, "NPC_Save_Info");
                if (!Directory.Exists(Save_Anywhere_V2.Mod_Core.npc_path))
                {
                    Directory.CreateDirectory(Save_Anywhere_V2.Mod_Core.npc_path);
                }
                string mylocation = Path.Combine(Save_Anywhere_V2.Mod_Core.npc_path, npc.name);
                    string mylocation2 = mylocation;
                    string mylocation3 = mylocation2 + ".txt";
                    string[] mystring3 = new string[20];
                    if (!File.Exists(mylocation3))
                    {
                    Log.Info("Missing character file?!?");
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
                    Log.Info("Warped NPC" +npc_name);
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
            Save_Anywhere_V2.Mod_Core.npc_warp = true;
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
        private static SchedulePathDescription pathfindToNextScheduleLocation(NPC npc,string startingLocation, int startingX, int startingY, string endingLocation, int endingX, int endingY, int finalFacingDirection, string endBehavior, string endMessage)
        {
            Stack<Point> stack = new Stack<Point>();
            Point startPoint = new Point(startingX, startingY);
            List<string> list = startingLocation.Equals(endingLocation) ? (List<string>)null : getLocationRoute(npc,startingLocation, endingLocation);
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
    }
    }
