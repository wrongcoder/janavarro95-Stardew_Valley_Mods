using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace StarAI.PathFindingCore
{
    public class Utilities
    {

       public static  List<TileExceptionNode> tileExceptionList = new List<TileExceptionNode>();
       public static string folderForExceptionTiles="ExceptionTilesData";

        public static Vector2 parseCenterFromTile(int tileX, int tileY)
        {
            int x = (tileX * Game1.tileSize) + Game1.tileSize / 2;
            int y = (tileY * Game1.tileSize) + Game1.tileSize / 2;
            return new Vector2(x, y);
        }

        public static void initializeTileExceptionList()
        {
            
        }



        public static void calculateMovement()
        {
            bool xTargetReached = false;
            bool yTargetReached = false;
            while (ModCore.warpGoals.Count > 0)
            {
                Warp w = ModCore.warpGoals[0];
                if (Game1.player.getTileX() == w.X && Game1.player.getTileY() == w.Y)
                {
                    ModCore.warpGoals.Remove(w);
                    ModCore.CoreMonitor.Log("LOOOP", LogLevel.Debug);
                    // return;
                    continue;
                }
                else
                {
                    Vector2 center = parseCenterFromTile(w.X, w.Y);
                    while (Game1.player.position.X > center.X && xTargetReached == false)
                    {
                        if (isWithinRange(Game1.player.position.X, center.X, 12) == true)
                        {
                            ModCore.CoreMonitor.Log("XXXXXXXtargetReached");
                            xTargetReached = true;
                            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
                            //break;
                            continue;
                        }
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_A);
                    }
                    while (Game1.player.position.X < center.X && xTargetReached == false)
                    {
                        if (isWithinRange(Game1.player.position.X, center.X, 6) == true)
                        {
                            xTargetReached = true;
                            ModCore.CoreMonitor.Log("XXXXXXXtargetReached");
                            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
                            continue;
                        }
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_D);
                    }
                    ModCore.CoreMonitor.Log("Run???");
                    while (Game1.player.position.Y < center.Y && yTargetReached == false)
                    {
                        ModCore.CoreMonitor.Log("banana");
                        if (isWithinRange(Game1.player.position.Y, center.Y, 6) == true)
                        {
                            yTargetReached = true;
                            ModCore.CoreMonitor.Log("YYYYYYYYYtargetReached");
                            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_S);
                            continue;
                        }
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_S);
                    }
                    ModCore.CoreMonitor.Log("Or no???");
                    while (Game1.player.position.Y > center.Y && yTargetReached == false)
                    {
                        ModCore.CoreMonitor.Log("potato");
                        if (isWithinRange(Game1.player.position.Y, center.Y, 6) == true)
                        {
                            yTargetReached = true;
                            ModCore.CoreMonitor.Log("YYYYYYYYYtargetReached");
                            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
                            continue;
                        }
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_W);
                    }


                    if (xTargetReached == true && yTargetReached == true)
                    {
                        ModCore.warpGoals.Remove(w);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_S);
                        //return;
                        ModCore.CoreMonitor.Log("Reached goal!", LogLevel.Error);
                        continue;
                    }


                    ModCore.CoreMonitor.Log("UNCAUGHT EXCEPTION", LogLevel.Error);
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
        public static bool isWithinRange(float position, float goal, int tolerance)
        {
            if (position >= goal - tolerance && position <= goal + tolerance) return true;
            return false;
        }

    }
}
