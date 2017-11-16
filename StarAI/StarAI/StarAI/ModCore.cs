using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI;
using WindowsInput;
using Microsoft.Xna.Framework;

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

        public static Task fun = new Task(new Action(calculateMovement));
        public override void Entry(IModHelper helper)
        {
            CoreHelper = helper;
            helper.ConsoleCommands.Add("hello", "Ok?", new Action<string, string[]>(hello));
            string[] s = new string[10];
            
            CoreMonitor = this.Monitor;
            CoreMonitor.Log("Hello AI WORLD!", LogLevel.Info);
            //throw new NotImplementedException();
            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_AfterLoad;
            StardewModdingAPI.Events.LocationEvents.CurrentLocationChanged += LocationEvents_CurrentLocationChanged;
            StardewModdingAPI.Events.GameEvents.SecondUpdateTick += GameEvents_SecondUpdateTick;
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
                warpGoals.Add(v);
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
