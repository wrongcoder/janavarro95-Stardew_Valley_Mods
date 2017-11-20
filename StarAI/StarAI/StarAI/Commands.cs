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

            pathfind("Initialize Delay 0", new string[] {
                "setDelay",
                "0"
                });
        }

        public static void runTasks(string s, string[] args)
        {
            ModCore.CoreMonitor.Log("EXECUTE TASKS");
            PathFindingLogic.source = null;
            PathFindingLogic.currentGoal = null;
            if (ExecutionCore.TaskList.executioner.Status == TaskStatus.Running)
            {
                ModCore.CoreMonitor.Log("Tasklist is already executing. Just going to return.");
                return;
            }
            if (ExecutionCore.TaskList.executioner.Status == TaskStatus.RanToCompletion)
            {
                ModCore.CoreMonitor.Log("Tasklist is finished. Going to restart");

                List<Task> removalList = new List<Task>();
                foreach(var v in ExecutionCore.TaskList.taskList)
                {
                    if (v.IsCompleted) removalList.Add(v);
                }
                foreach(var v in removalList)
                {
                    ExecutionCore.TaskList.taskList.Remove(v);
                }

                ExecutionCore.TaskList.executioner = new Task(new Action(ExecutionCore.TaskList.runTaskList));
                ExecutionCore.TaskList.executioner.Start();
               // ExecutionCore.TaskList.taskList.Clear();
                return;
                //ExecutionCore.TaskList.runTaskList();

            }

            if (ExecutionCore.TaskList.executioner.Status == TaskStatus.Faulted)
            {
                ModCore.CoreMonitor.Log(ExecutionCore.TaskList.executioner.Exception.ToString());
            }

            if (ExecutionCore.TaskList.executioner.Status == TaskStatus.Created)
            {
                //ExecutionCore.TaskList.runTaskList();
                List<Task> removalList = new List<Task>();
                ExecutionCore.TaskList.executioner.Start();
  
                return;
            }
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
                bool ok = t.checkIfICanPlaceHere(t, new Vector2(pos.X, pos.Y), Game1.player.currentLocation);
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
              
                bool ok = t.checkIfICanPlaceHere(t, new Vector2(pos.X, pos.Y), Game1.player.currentLocation);
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

                if (ModCore.fun.Status == TaskStatus.Running)
                {
                    ModCore.CoreMonitor.Log("TASK IS RUNNING CAN'T PATHFIND AT THE MOMENT", LogLevel.Alert);
                    return;
                }
                if (ModCore.fun.Status == TaskStatus.RanToCompletion)
                {

                    ModCore.CoreMonitor.Log("TASK IS Finished PATHFINDING", LogLevel.Warn);
                    ModCore.fun = new Task(new Action(PathFindingLogic.pathFindToAllGoals));
                   // return;
                }

                if (ModCore.fun.Status == TaskStatus.Created)
                {
                    ModCore.CoreMonitor.Log("CREATE AND RUN A TASK!!! PATHFINDING!");
                    ModCore.fun = new Task(new Action(PathFindingLogic.pathFindToAllGoals));

                    ModCore.fun.Start();
                    return;
                }
                ModCore.CoreMonitor.Log(ModCore.fun.Status.ToString());
                if (ModCore.fun.Status == TaskStatus.Faulted)
                {
                    ModCore.CoreMonitor.Log(ModCore.fun.Exception.ToString());
                    ModCore.CoreMonitor.Log("CREATE AND RUN A TASK!!! PATHFINDING!");
                    ModCore.fun = new Task(new Action(PathFindingLogic.pathFindToAllGoals));
                }

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
                ExecutionCore.TaskList.taskList.Add(new Task(new Action<object>(PathFindingLogic.pathFindToSingleGoal),obj));
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

