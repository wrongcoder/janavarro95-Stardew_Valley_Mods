using Microsoft.Xna.Framework;
using StarAI.PathFindingCore;
using StardewModdingAPI;
using StardewValley;
using StardustCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarAI;
using StarAI.PathFindingCore.DebrisLogic;
using StarAI.PathFindingCore.WaterLogic;
using StarAI.PathFindingCore.CropLogic;

namespace StarAI
{
    class Commands
    {

        public static void initializeCommands()
        {
            ModCore.CoreHelper.ConsoleCommands.Add("hello", "Ok?", new Action<string, string[]>(hello));
            ModCore.CoreHelper.ConsoleCommands.Add("pathfind", "pathy?", new Action<string, string[]>(Commands.pathfind));
            ModCore.CoreHelper.ConsoleCommands.Add("pathfinding", "pathy?", new Action<string, string[]>(Commands.pathfind));
            ModCore.CoreHelper.ConsoleCommands.Add("Execute", "Run tasks", new Action<string,string[]>(Commands.runTasks));
            //ModCore.CoreHelper.ConsoleCommands.Add("execute", "Run tasks", new Action<string, string[]>(Commands.runTasks));
            ModCore.CoreHelper.ConsoleCommands.Add("runTasks", "Run tasks", new Action<string, string[]>(Commands.runTasks));
            ModCore.CoreHelper.ConsoleCommands.Add("Water", "Water the crops", new Action<string, string[]>(Commands.waterCrops));
            ModCore.CoreHelper.ConsoleCommands.Add("Harvest", "Harvest the crops", new Action<string, string[]>(Commands.harvestCrops));
            ModCore.CoreHelper.ConsoleCommands.Add("getseeds", "Get Seeds From chests.", new Action<string, string[]>(Commands.getSeedsFromChests));

            ModCore.CoreHelper.ConsoleCommands.Add("choptwigs", "Chop twigs.", new Action<string, string[]>(Commands.chopAllTwigs));
            ModCore.CoreHelper.ConsoleCommands.Add("chopsticks", "Chop twigs.", new Action<string, string[]>(Commands.chopAllTwigs));

            ModCore.CoreHelper.ConsoleCommands.Add("choptrees", "Chop trees down.", new Action<string, string[]>(Commands.chopAllTrees));
            ModCore.CoreHelper.ConsoleCommands.Add("cuttrees",  "Chop trees down.", new Action<string, string[]>(Commands.chopAllTrees));

            ModCore.CoreHelper.ConsoleCommands.Add("breakstones", "Break small stones with pickaxe.", new Action<string, string[]>(Commands.breakAllStones));

            ModCore.CoreHelper.ConsoleCommands.Add("cutweed", "Cut weeds with a tool.", new Action<string, string[]>(Commands.cutAllWeeds));
            ModCore.CoreHelper.ConsoleCommands.Add("cutweeds", "Cut weeds with a tool", new Action<string, string[]>(Commands.cutAllWeeds));

            ModCore.CoreHelper.ConsoleCommands.Add("watercan", "Fill my watering can.", new Action<string, string[]>(Commands.fillWateringCan));
            ModCore.CoreHelper.ConsoleCommands.Add("fillcan", "Fill my watering can.", new Action<string, string[]>(Commands.fillWateringCan));

            ModCore.CoreHelper.ConsoleCommands.Add("shippingbin", "Goto shipping bin", new Action<string, string[]>(Commands.goToShippingBin));
            ModCore.CoreHelper.ConsoleCommands.Add("shipItem", "Fill my watering can.", new Action<string, string[]>(Commands.shipItem));
            // ModCore.CoreHelper.ConsoleCommands.Add("chopsticks", "Chop twigs.", new Action<string, string[]>(Commands.chopAllTwigs));
            pathfind("Initialize Delay 0", new string[] {
                "setDelay",
                "0"
                });
        }

        public static void getSeedsFromChests(string s, string[] args)
        {
            ChestLogic.getAllSeasonalSeedsFromAllChestsAtLocation(Game1.player.currentLocation);
        }

        public static void shipItem(string s, string[] args)
        {
            if (args.Length < 2)
            {
                ModCore.CoreMonitor.Log("NOT ENOUGH PARAMETERS. NEED 2 ARGS. ItemIndex,Amount");
                return;
            }
            StardewValley.Object ok =new StardewValley.Object(Convert.ToInt32(args[0]),Convert.ToInt32(args[1]));

            if (ok == null) {
                ModCore.CoreMonitor.Log("ITEM IS NULL????");
                return;
            }
            ExecutionCore.TaskPrerequisites.ItemPrerequisite pre = new ExecutionCore.TaskPrerequisites.ItemPrerequisite(ok, ok.stack);
            if (pre.doesPlayerHaveEnoughOfMe())
            {
                ShippingLogic.goToShippingBinShipItem(ok);
            }
            else
            {
                ModCore.CoreMonitor.Log("Player does not have: " + ok.name + ": amount: " + ok.stack.ToString());
            }
        }

        public static void goToShippingBin(string s, string[] args)
        {
            ShippingLogic.goToShippingBinSetUp();
        }

        public static void fillWateringCan(string s, string[] args)
        {
            WaterLogic.getAllWaterTiles(Game1.player.currentLocation);
        }

        public static void chopAllTrees(string s, string[] args)
        {
            if (args.Length == 1)
            {
                if (args[0] == "All" || args[0] == "all")
                {
                    ModCore.CoreMonitor.Log("CHOP ALL TREES");
                    DebrisLogic.getAllTreesToChop(Game1.player.currentLocation);
                    return;
                }
            }
            DebrisLogic.getAllTreesToChopRadius(Game1.player.currentLocation);
        }

        public static void chopAllTwigs(string s, string[] args)
        {
            if (args.Length == 1)
            {
                if (args[0] == "All" || args[0] == "all")
                {
                    DebrisLogic.getAllSticksToChop(Game1.player.currentLocation);
                    return;
                }
            }
            DebrisLogic.getAllSticksToChopRadius(Game1.player.currentLocation);
        }



        public static void breakAllStones(string s, string[] args)
        {
            if (args.Length == 1)
            {
                if (args[0] == "All" || args[0] == "all")
                {
                    DebrisLogic.getAllStonesToBreak(Game1.player.currentLocation);
                    return;
                }
            }
            DebrisLogic.getAllStonestoBreakRadius(Game1.player.currentLocation);
        }

        public static void cutAllWeeds(string s, string[] args)
        {
            if (args.Length == 1)
            {
                if (args[0] == "All" || args[0] == "all")
                {
                    DebrisLogic.getAllWeedsToCut(Game1.player.currentLocation);
                    return;
                }
            }
            DebrisLogic.getAllWeedsToCutRadius(Game1.player.currentLocation);
        }

        public static void runTasks(string s, string[] args)
        {
            ExecutionCore.TaskList.runTaskList();
          
        }



        public static void waterCrops(string s, string[] args)
        {
            PathFindingCore.CropLogic.CropLogic.getAllCropsNeededToBeWatered();
        }

        public static void harvestCrops(string s,string[] args)
        {
            PathFindingCore.CropLogic.CropLogic.getAllCropsNeededToBeHarvested();
        }

        /// <summary>
        /// 1.Set start position
        /// 2.set goal
        /// 3.queue up the task
        /// </summary>
        /// <param name="s"></param>
        /// <param name="args"></param>
        public static void pathfind(string s, string[] args)
        {
            if (PathFindingCore.PathFindingLogic.source != null)
            {
                ModCore.CoreMonitor.Log("THIS IS OUR SOURCE POINT: as " + s + " : " + PathFindingCore.PathFindingLogic.source.tileLocation.ToString());
            }
            if (args.Length < 1)
            {
                ModCore.CoreMonitor.Log("No args passed into path finding function", LogLevel.Error);
            }

            //Set delay code
            #region
            if (args[0]=="setDelay"|| args[0]=="delay" || args[0]=="setdelay"|| args[0] == "SetDelay")
            {
                PathFindingLogic.delay = Convert.ToInt32(args[1]);
                ModCore.CoreMonitor.Log("Pathfinding node delay set to: " + Convert.ToString(PathFindingLogic.delay) + " milliseconds.");
            }
            #endregion

            //PathTo Code
            #region
            if (args[0]=="pathTo"|| args[0]=="pathto"|| args[0]=="PathTo"|| args[0] == "Pathto")
            {
                if (PathFindingLogic.currentGoal == null)
                {
                    pathfind(s, new string[]{

                    "setStart",
                    "currentPosition"
                });
                }
                else
                {
                    pathfind(s, new string[]{

                    "setStart",
                    PathFindingLogic.currentGoal.tileLocation.X.ToString(),
                    PathFindingLogic.currentGoal.tileLocation.Y.ToString(),
                });
                }
              

                int currentX;
                int currentY;
                if (PathFindingLogic.currentGoal == null)
                {
                     currentX = Game1.player.getTileX();
                     currentY = Game1.player.getTileY();
                }
                else
                {
                     currentX = (int)PathFindingLogic.currentGoal.tileLocation.X;
                     currentY = (int)PathFindingLogic.currentGoal.tileLocation.Y;
                }

                int xOffset = Convert.ToInt32(args[1]);
                int yOffset = Convert.ToInt32(args[2]);
                int destX = currentX + xOffset;
                int destY = currentY + yOffset;
                pathfind(s, new string[]
                {
                    "addGoal",
                    destX.ToString(),
                    destY.ToString()

                });

                pathfind("pathfind pathto", new string[]
                {
                        "queue"
                });
                //PathFindingLogic.currentGoal = null;
                //PathFindingLogic.source = null;
            }
            #endregion

            //Add goal code
            #region
            if (args[0] == "addGoal" || args[0] == "setGoal")
            {

                TileNode t = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Green));
                Vector2 pos = new Vector2((int)(Convert.ToInt32(args[1]) * Game1.tileSize), Convert.ToInt32(args[2]) * Game1.tileSize);
                bool ok =TileNode.checkIfICanPlaceHere(t, new Vector2(pos.X, pos.Y), Game1.player.currentLocation);
                if (ok == false)
                {
                    ModCore.CoreMonitor.Log("Can't place a goal point here!!!", LogLevel.Error);
                    return;
                }
                t.placementAction(Game1.currentLocation, (int)pos.X, (int)pos.Y);
                ModCore.CoreMonitor.Log("Placing start at: " + pos.ToString(), LogLevel.Warn);

                if (PathFindingLogic.currentGoal != null)
                {
                    PathFindingLogic.source = PathFindingLogic.currentGoal;
                }
                PathFindingLogic.currentGoal = t;
                PathFindingLogic.goals.Add(t);
            }
            #endregion
            
            //Add start
            #region
            if (args[0] == "addStart" || args[0] == "setStart")
            {

                TileNode t = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Magenta));
                Vector2 pos;
                if (args[1] == "currentPosition")
                {
                    pos = new Vector2((int)(Game1.player.getTileX() * Game1.tileSize), Game1.player.getTileY() * Game1.tileSize);
                }
                else
                {
                    pos = new Vector2((int)(Convert.ToInt32(args[1]) * Game1.tileSize), Convert.ToInt32(args[2]) * Game1.tileSize);
                }
              
                bool ok = TileNode.checkIfICanPlaceHere(t, new Vector2(pos.X, pos.Y), Game1.player.currentLocation);
                bool cry = false;
                if (t.thisLocation == null)
                {
                    cry = true;
                    t.thisLocation=Game1.player.currentLocation;
                }

                if (t.thisLocation.isObjectAt((int)pos.X, (int)pos.Y))
                {
                    StardewValley.Object maybe = t.thisLocation.getObjectAt((int)pos.X, (int)pos.Y);
                    if (maybe is TileNode)
                    {
                        
                            PathFindingLogic.source = (TileNode)maybe;
                            ModCore.CoreMonitor.Log("Changed the source point!!!!:"+PathFindingLogic.source.tileLocation, LogLevel.Warn);
                           // ok = true;
                        
                    }
                }
                if (ok == false)
                {
                    ModCore.CoreMonitor.Log("Can't place a start point here!!!", LogLevel.Error);
                    return;
                }
                t.placementAction(Game1.currentLocation, (int)pos.X, (int)pos.Y);
                ModCore.CoreMonitor.Log("Placing start at: "+pos.ToString(), LogLevel.Warn);
                PathFindingLogic.source = t;
            }
            #endregion
            
            //Restart Code
            #region
            if (args[0] == "restart")
            {
                List<CoreObject> removalList = new List<CoreObject>();
                foreach (var v in StardustCore.ModCore.SerializationManager.trackedObjectList)
                {
                    removalList.Add(v);
                }
                foreach (var v in removalList)
                {
                    StardustCore.ModCore.SerializationManager.trackedObjectList.Remove(v);
                    Game1.player.currentLocation.objects.Remove(v.TileLocation);
                    pathfind("pathfind restart", new string[]
                    {
                        "addGoal",
                        PathFindingLogic.currentGoal.tileLocation.X.ToString(),
                        PathFindingLogic.currentGoal.tileLocation.Y.ToString(),
                    }
                    );
                }
                removalList.Clear();
                pathfind("pathfind restart", new string[]
                {
                        "addStart",
                        PathFindingLogic.source.tileLocation.X.ToString(),
                        PathFindingLogic.source.tileLocation.Y.ToString(),
                }
                );
                pathfind("pathfind restart", new string[]
               {
                        "start"
               }
               );

            }
            #endregion
            
            //start code
            #region
            if (args[0] == "start")
            {
                if (Game1.player == null) return;
                if (Game1.hasLoadedGame == false) return;
                // ModCore.CoreMonitor.Log(Game1.player.currentLocation.isTileLocationOpen(new xTile.Dimensions.Location((int)(Game1.player.getTileX() + 1)*Game1.tileSize, (int)(Game1.player.getTileY())*Game1.tileSize)).ToString());
                //CoreMonitor.Log(Convert.ToString(warpGoals.Count));
                if (PathFindingCore.PathFindingLogic.currentGoal == null)
                {
                    ModCore.CoreMonitor.Log("NO VALID GOAL SET FOR PATH FINDING!", LogLevel.Error);
                }
                if (PathFindingCore.PathFindingLogic.source == null)
                {
                    ModCore.CoreMonitor.Log("NO VALID START SET FOR PATH FINDING!", LogLevel.Error);
                }

                PathFindingLogic.pathFindToAllGoals();

            }
            #endregion

            //Queue Code
            #region
            if (args[0]=="queue" || args[0] == "Queue")
            {
                if (Game1.player == null) return;
                if (Game1.hasLoadedGame == false) return;
                // ModCore.CoreMonitor.Log(Game1.player.currentLocation.isTileLocationOpen(new xTile.Dimensions.Location((int)(Game1.player.getTileX() + 1)*Game1.tileSize, (int)(Game1.player.getTileY())*Game1.tileSize)).ToString());
                //CoreMonitor.Log(Convert.ToString(warpGoals.Count));
                if (PathFindingCore.PathFindingLogic.currentGoal == null)
                {
                    ModCore.CoreMonitor.Log("NO VALID GOAL SET FOR PATH FINDING!", LogLevel.Error);
                    return;
                }
                if (PathFindingCore.PathFindingLogic.source == null)
                {
                    ModCore.CoreMonitor.Log("NO VALID START SET FOR PATH FINDING!", LogLevel.Error);
                    return;
                }
                object[] obj = new object[3];
                obj[0] = PathFindingLogic.source;
                obj[1] = PathFindingLogic.currentGoal;
                PathFindingLogic.queue = new List<TileNode>();
                obj[2] = PathFindingLogic.queue;
               // ExecutionCore.TaskList.taskList.Add(new ExecutionCore.CustomTask(PathFindingLogic.pathFindToSingleGoal, obj,new ExecutionCore.TaskMetaData("Pathfind Command",PathFindingCore.Utilities.calculatePathCost(PathFindingLogic.source,false))));
                //ExecutionCore.TaskList.taskList.Add(new Task(new Action<object>(PathFindingLogic.pathFindToSingleGoal),obj));
            }
            #endregion


            //AddTask one liner
            #region
            if (args[0] == "addTask" || args[0] == "addtask" || args[0] == "AddTask" || args[0] == "Addtask")
            {
                pathfind("add a task",new string[]{
                    "addStart",
                    args[1],
                    args[2],
                });
                pathfind("add a task", new string[] {
                    "addGoal",
                    args[3],
                    args[4]

                });
                pathfind("add a task", new string[]
                {
                    "queue"
                });
            }
            #endregion
        }

        /// <summary>
        /// A test function.
        /// </summary>
        /// <param name="s">This is the command's name</param>
        /// <param name="sarray">This is the parameters that follow.</param>
        public static void hello(string s, string[] sarray)
        {
            ModCore.CoreMonitor.Log(s, LogLevel.Info);

            foreach (var word in sarray)
            {
                ModCore.CoreMonitor.Log(word, LogLevel.Info);
            }
            ModCore.CoreMonitor.Log("FUDGE");
            // Game1.player.tryToMoveInDirection(2, true, 0, false);

        }
    }

}

