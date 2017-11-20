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

            pathfind("Initialize Delay 0", new string[] {
                "setDelay",
                "0"
                });
        }



        public static void pathfind(string s, string[] args)
        {
            
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
                pathfind(s,new string[]{

                    "setStart",
                    "currentPosition"
                });
                int currentX = Game1.player.getTileX();
                int currentY = Game1.player.getTileY();
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

                pathfind("pathfind restart", new string[]
                {
                        "start"
                });
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
                if (ok == false)
                {
                    ModCore.CoreMonitor.Log("Can't place a start point here!!!", LogLevel.Error);
                    return;
                }
                t.placementAction(Game1.currentLocation, (int)pos.X, (int)pos.Y);
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
                    ModCore.obj[0] = PathFindingLogic.source;
                    ModCore.obj[1] = PathFindingLogic.currentGoal;
                    ModCore.obj[2] = PathFindingLogic.queue;
                    ModCore.fun = new Task(new Action<object>(PathFindingLogic.pathFindToSingleGoal), ModCore.obj);
                    return;
                }

                if (ModCore.fun.Status == TaskStatus.Created)
                {
                    ModCore.CoreMonitor.Log("CREATE AND RUN A TASK!!! PATHFINDING!");
                    ModCore.obj[0] = PathFindingLogic.source;
                    ModCore.obj[1] = PathFindingLogic.currentGoal;
                    ModCore.obj[2] = PathFindingLogic.queue;
                    ModCore.fun = new Task(new Action<object>(PathFindingLogic.pathFindToSingleGoal), ModCore.obj);

                    ModCore.fun.Start();
                    return;
                }
                ModCore.CoreMonitor.Log(ModCore.fun.Status.ToString());
                if (ModCore.fun.Status == TaskStatus.Faulted)
                {
                    ModCore.CoreMonitor.Log(ModCore.fun.Exception.ToString());
                    ModCore.CoreMonitor.Log("CREATE AND RUN A TASK!!! PATHFINDING!");
                    ModCore.obj[0] = PathFindingLogic.source;
                    ModCore.obj[1] = PathFindingLogic.currentGoal;
                    ModCore.obj[2] = PathFindingLogic.queue;
                    ModCore.fun = new Task(new Action<object>(PathFindingLogic.pathFindToSingleGoal), ModCore.obj);
                }

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

