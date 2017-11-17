using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI;
using WindowsInput;
using Microsoft.Xna.Framework;
using StarAI.PathFindingCore;
using System.IO;
using StardustCore;

namespace StarAI
{
    //TODO: Clean up initial code
    //Make sure pathos doesn't update this once since it's a homework assignment. Sorry Pathos!
    //Work on dijakstra's algorithm for path finding on this one? Make sure obstacles are included.
    //Question how this is all going to work.
    public class ModCore : Mod
    {
        public static StardewModdingAPI.IMonitor CoreMonitor;
        public static StardewModdingAPI.IModHelper CoreHelper;
        public static List<Warp> warpGoals= new List<Warp>();
        public static object[] obj = new object[3];
        
        public static Task fun = new Task(new Action<object>(PathFindingLogic.pathFindToSingleGoal),obj);
        public override void Entry(IModHelper helper)
        {
            obj[0] = PathFindingLogic.source;
            obj[1] = PathFindingLogic.currentGoal;
            obj[2] = PathFindingLogic.queue;
            CoreHelper = helper;
            helper.ConsoleCommands.Add("hello", "Ok?", new Action<string, string[]>(hello));
            helper.ConsoleCommands.Add("pathfind", "pathy?", new Action<string, string[]>(pathfind));
            string[] s = new string[10];
            
            CoreMonitor = this.Monitor;
            CoreMonitor.Log("Hello AI WORLD!", LogLevel.Info);
            //throw new NotImplementedException();
            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_AfterLoad;
            StardewModdingAPI.Events.LocationEvents.CurrentLocationChanged += LocationEvents_CurrentLocationChanged;
            StardewModdingAPI.Events.GameEvents.SecondUpdateTick += GameEvents_SecondUpdateTick;

            StardewModdingAPI.Events.ControlEvents.KeyPressed += ControlEvents_KeyPressed;

            StardustCore.ModCore.SerializationManager.acceptedTypes.Add("StarAI.PathFindingCore.TileNode", new StardustCore.Serialization.SerializerDataNode(new StardustCore.Serialization.SerializerDataNode.SerializingFunction(TileNode.Serialize), new StardustCore.Serialization.SerializerDataNode.ParsingFunction(TileNode.ParseIntoInventory), new StardustCore.Serialization.SerializerDataNode.WorldParsingFunction(TileNode.SerializeFromWorld), new StardustCore.Serialization.SerializerDataNode.SerializingToContainerFunction(TileNode.Serialize)));
        }

        public void pathfind(string s, string[] args)
        {
            if (args.Length < 1)
            {
                CoreMonitor.Log("No args passed into path finding function",LogLevel.Error);
            }
            if (args[0] == "addGoal")
            {

                TileNode t = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"),StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Green));
                Vector2 pos=new Vector2((int)(Convert.ToInt32(args[1]) * Game1.tileSize), Convert.ToInt32(args[2]) * Game1.tileSize);
                bool ok = t.checkIfICanPlaceHere(t, new Vector2(pos.X,pos.Y), Game1.player.currentLocation);
                if (ok == false)
                {
                    CoreMonitor.Log("Can't place a goal point here!!!", LogLevel.Error);
                    return;
                }
                t.placementAction(Game1.currentLocation,(int) pos.X,(int) pos.Y);
                PathFindingLogic.currentGoal = t;
                PathFindingLogic.goals.Add(t);
            }

            if (args[0] == "addStart")
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
                    CoreMonitor.Log("Can't place a start point here!!!", LogLevel.Error);
                    return;
                }
                t.placementAction(Game1.currentLocation, (int)pos.X, (int)pos.Y);
                PathFindingLogic.source = t;
            }

            if (args[0] == "restart")
            {
                List<CoreObject> removalList = new List<CoreObject>();
                foreach(var v in StardustCore.ModCore.SerializationManager.trackedObjectList)
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

            if (args[0] == "start")
            {
                if (Game1.player == null) return;
                if (Game1.hasLoadedGame == false) return;
                // ModCore.CoreMonitor.Log(Game1.player.currentLocation.isTileLocationOpen(new xTile.Dimensions.Location((int)(Game1.player.getTileX() + 1)*Game1.tileSize, (int)(Game1.player.getTileY())*Game1.tileSize)).ToString());
                //CoreMonitor.Log(Convert.ToString(warpGoals.Count));
                if (PathFindingCore.PathFindingLogic.currentGoal == null)
                {
                    CoreMonitor.Log("NO VALID GOAL SET FOR PATH FINDING!",LogLevel.Error);
                }
                if (PathFindingCore.PathFindingLogic.source == null)
                {
                    CoreMonitor.Log("NO VALID START SET FOR PATH FINDING!", LogLevel.Error);
                }

                if (fun.Status == TaskStatus.Running)
                {
                    CoreMonitor.Log("TASK IS RUNNING CAN'T PATHFIND AT THE MOMENT", LogLevel.Alert);
                    return;
                }
                if (fun.Status == TaskStatus.RanToCompletion)
                {

                    CoreMonitor.Log("TASK IS Finished PATHFINDING", LogLevel.Warn);
                    obj[0] = PathFindingLogic.source;
                    obj[1] = PathFindingLogic.currentGoal;
                    obj[2] = PathFindingLogic.queue;
                    fun = new Task(new Action<object>(PathFindingLogic.pathFindToSingleGoal), obj);
                    return;
                }

                if (fun.Status == TaskStatus.Created)
                {
                    CoreMonitor.Log("CREATE AND RUN A TASK!!! PATHFINDING!");
                    obj[0] = PathFindingLogic.source;
                    obj[1] = PathFindingLogic.currentGoal;
                    obj[2] = PathFindingLogic.queue;
                    fun = new Task(new Action<object>(PathFindingLogic.pathFindToSingleGoal), obj);
                    
                    fun.Start();
                    return;
                }
                CoreMonitor.Log(fun.Status.ToString());
                if (fun.Status == TaskStatus.Faulted)
                {
                    CoreMonitor.Log(fun.Exception.ToString());
                    CoreMonitor.Log("CREATE AND RUN A TASK!!! PATHFINDING!");
                    obj[0] = PathFindingLogic.source;
                    obj[1] = PathFindingLogic.currentGoal;
                    obj[2] = PathFindingLogic.queue;
                    fun = new Task(new Action<object>(PathFindingLogic.pathFindToSingleGoal), obj);
                }

            }
        }

        private void ControlEvents_KeyPressed(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Microsoft.Xna.Framework.Input.Keys.J)
            {
                CoreMonitor.Log("OK THE J KEY WAS PRESSED!");
                List<Item> shoppingList = new List<Item>();
                TileNode t = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"),StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Aqua));
                if (t == null)
                {
                    CoreMonitor.Log("WTF?????");
                }
                try
                {
                    if (t == null)
                    {
                        return;
                    }
                    shoppingList.Add((Item)t);
                    Game1.activeClickableMenu = new StardewValley.Menus.ShopMenu(shoppingList);
                }catch(Exception err)
                {
                    CoreMonitor.Log(Convert.ToString(err));
                }
             }

            if (e.KeyPressed == Microsoft.Xna.Framework.Input.Keys.K)
            {
                CoreMonitor.Log("OK THE K KEY WAS PRESSED!");
                
                TileNode t = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"),StardustCore.IlluminateFramework.Colors.randomColor());
                if (t == null)
                {
                    CoreMonitor.Log("WTF?????");
                }
                try
                {
                    if (t == null)
                    {
                        return;
                    }
                    CoreMonitor.Log(new Vector2(Game1.player.getTileX()*Game1.tileSize,Game1.player.getTileY()*Game1.tileSize).ToString());

                    int xPos = (int)(Game1.player.getTileX()) * Game1.tileSize;
                    int yPos = (int)(Game1.player.getTileY()) * Game1.tileSize;
                    Rectangle r = new Rectangle(xPos, yPos, Game1.tileSize, Game1.tileSize);
                    Vector2 pos = new Vector2(r.X, r.Y);
                    bool ok = t.checkIfICanPlaceHere(t,pos,Game1.player.currentLocation);
                    if (ok == false) return;
                    t.placementAction(Game1.currentLocation, Game1.player.getTileX()*Game1.tileSize, Game1.player.getTileY()*Game1.tileSize);
                    t.setAdjacentTiles(true);
                }
                catch (Exception err)
                {
                    CoreMonitor.Log(Convert.ToString(err));
                }
            }
        }

        public static Vector2 parseCenterFromTile(int tileX, int tileY)
        {
            int x = (tileX * Game1.tileSize) + Game1.tileSize / 2;
            int y = (tileY * Game1.tileSize) + Game1.tileSize / 2;
            return new Vector2(x, y);
        }

        public static void calculateMovement()
        {
            bool xTargetReached = false;
            bool yTargetReached = false;
            while (warpGoals.Count > 0)
            {
                Warp w = warpGoals[0];
                if (Game1.player.getTileX() == w.X && Game1.player.getTileY() == w.Y)
                {
                    warpGoals.Remove(w);
                    CoreMonitor.Log("LOOOP", LogLevel.Debug);
                    // return;
                    continue;
                }
                else
                {
                    Vector2 center = parseCenterFromTile(w.X, w.Y);
                    while (Game1.player.position.X > center.X && xTargetReached==false)
                    {
                        if (isWithinRange(Game1.player.position.X, center.X, 12) == true)
                        {
                            ModCore.CoreMonitor.Log("XXXXXXXtargetReached");
                            xTargetReached = true;
                            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
                            //InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
                            //InputSimulator.SimulateKeyUp(VirtualKeyCode.UP);
                            //InputSimulator.SimulateKeyUp(VirtualKeyCode.DOWN);
                            //break;
                            continue;
                        }
                        //CoreMonitor.Log(Convert.ToString(Game1.player.position.X), LogLevel.Debug);
                        //CoreMonitor.Log(Convert.ToString(center.X), LogLevel.Warn);
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_A);
                        //InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
                        //InputSimulator.SimulateKeyUp(VirtualKeyCode.UP);
                        //InputSimulator.SimulateKeyUp(VirtualKeyCode.DOWN);
                    }
                    while (Game1.player.position.X < center.X && xTargetReached==false)
                    {
                        if (isWithinRange(Game1.player.position.X, center.X, 6) == true)
                        {
                            xTargetReached = true;
                            ModCore.CoreMonitor.Log("XXXXXXXtargetReached");
                            //InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
                            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
                            //InputSimulator.SimulateKeyUp(VirtualKeyCode.UP);
                            //InputSimulator.SimulateKeyUp(VirtualKeyCode.DOWN);
                            //break;
                            continue;
                        }
                        //CoreMonitor.Log(Convert.ToString(Game1.player.position.X), LogLevel.Debug);
                        //CoreMonitor.Log(Convert.ToString(center.X), LogLevel.Warn);
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_D);
                        //InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
                        //InputSimulator.SimulateKeyUp(VirtualKeyCode.UP);
                        //InputSimulator.SimulateKeyUp(VirtualKeyCode.DOWN);
                    }
                    ModCore.CoreMonitor.Log("Run???");
                    while (Game1.player.position.Y < center.Y && yTargetReached==false)
                    {
                        ModCore.CoreMonitor.Log("banana");
                        if (isWithinRange(Game1.player.position.Y, center.Y, 6) == true)
                        {
                            yTargetReached = true;
                            ModCore.CoreMonitor.Log("YYYYYYYYYtargetReached");
                            //InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
                            //InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
                            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_S);
                            //InputSimulator.SimulateKeyUp(VirtualKeyCode.DOWN);
                            //break;
                            continue;
                        }
                        //CoreMonitor.Log(Convert.ToString(Game1.player.position.X), LogLevel.Debug);
                        //CoreMonitor.Log(Convert.ToString(center.X), LogLevel.Warn);
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_S);
                        //InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
                        //InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
                        //InputSimulator.SimulateKeyUp(VirtualKeyCode.DOWN);
                    }
                    ModCore.CoreMonitor.Log("Or no???");
                    while (Game1.player.position.Y > center.Y&& yTargetReached==false)
                    {
                        ModCore.CoreMonitor.Log("potato");
                        if (isWithinRange(Game1.player.position.Y, center.Y, 6) == true)
                        {
                            yTargetReached = true;
                            ModCore.CoreMonitor.Log("YYYYYYYYYtargetReached");
                            //InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
                            //InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
                            //InputSimulator.SimulateKeyUp(VirtualKeyCode.UP);
                            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
                            //break;
                            continue;
                        }
                        //CoreMonitor.Log(Convert.ToString(Game1.player.position.X), LogLevel.Debug);
                        //CoreMonitor.Log(Convert.ToString(center.X), LogLevel.Warn);
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_W);
                        //InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
                        //InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
                        //InputSimulator.SimulateKeyUp(VirtualKeyCode.UP);
                    }
                    /*
                    while (Game1.player.getTileX() < w.X)
                    {
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_D);
                    }

                    if (Game1.player.getTileX() == w.X)
                    {
                        warpGoals.Remove(w);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
                        return;
                    }
                    */

                    if (xTargetReached == true && yTargetReached == true)
                    {
                        warpGoals.Remove(w);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_S);
                        //return;
                        CoreMonitor.Log("Reached goal!", LogLevel.Error);
                        continue;
                    }


                    CoreMonitor.Log("UNCAUGHT EXCEPTION", LogLevel.Error);
                }
            }
        }

        /// <summary>
        /// Used to calculate center of a tile with varience.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="goal"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool isWithinRange(float position,float goal, int tolerance)
        {
            if (position >= goal - tolerance && position <= goal + tolerance) return true;
            return false;
        }


        private void GameEvents_SecondUpdateTick(object sender, EventArgs e)
        {
            if (Game1.player == null) return;
            if (Game1.hasLoadedGame == false) return;
           // ModCore.CoreMonitor.Log(Game1.player.currentLocation.isTileLocationOpen(new xTile.Dimensions.Location((int)(Game1.player.getTileX() + 1)*Game1.tileSize, (int)(Game1.player.getTileY())*Game1.tileSize)).ToString());
            //CoreMonitor.Log(Convert.ToString(warpGoals.Count));
            if (warpGoals.Count == 0) return;
            if (fun.Status == TaskStatus.Running)
            {
                //CoreMonitor.Log("TASK IS RUNNING", LogLevel.Alert);
                return;
            }
            if (fun.Status == TaskStatus.RanToCompletion)
            {

                //CoreMonitor.Log("TASK IS Finished", LogLevel.Warn);
                fun=new Task(new Action(calculateMovement));
                return;
            }

            if (fun.Status == TaskStatus.Created)
            {
                //CoreMonitor.Log("CREATE AND RUN A TASK!!!");
                fun.Start();
                return;
            }
            
        }

        private void LocationEvents_CurrentLocationChanged(object sender, StardewModdingAPI.Events.EventArgsCurrentLocationChanged e)
        {
            CoreMonitor.Log("LOCATION CHANGED!");
            CoreMonitor.Log(Game1.player.currentLocation.name);
            foreach(var v in Game1.player.currentLocation.warps)
            {
                string s ="X: " +Convert.ToString(v.X) + " Y: " + Convert.ToString(v.Y) + " TargetX: " + Convert.ToString(v.TargetX) + " TargetY: " + Convert.ToString(v.TargetY) + " TargetLocationName: " + Convert.ToString(v.TargetName);
                CoreMonitor.Log(s);
                //warpGoals.Add(v); Disabled for now
            }
            //GameLocation loc=Game1.getLocationFromName("location name")
            //
        }


        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">This is the command's name</param>
        /// <param name="sarray">This is the parameters that follow.</param>
        public void hello(string s,string[] sarray)
        {
            CoreMonitor.Log(s, LogLevel.Info);

            foreach(var word in sarray)
            {
                CoreMonitor.Log(word, LogLevel.Info);
            }
            CoreMonitor.Log("FUDGE");
           // Game1.player.tryToMoveInDirection(2, true, 0, false);

        }
    }
}
