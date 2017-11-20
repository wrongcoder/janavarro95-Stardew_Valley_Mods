using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarAI;
using StardewModdingAPI;
using StardewValley;
using WindowsInput;

namespace StarAI.PathFindingCore
{
    class PathFindingLogic
    {
        public static TileNode source;
        public static List<TileNode> goals = new List<TileNode>();
        public static List<TileNode> queue = new List<TileNode>();
        public static int totalPathCost;
        public static TileNode currentGoal;
        public static int delay;

        public static List<TileNode> path = new List<TileNode>();
        public static int index = 0;


        public static void pathFindToAllGoals()
        {
            List<TileNode> cleanseGoals = new List<TileNode>();
            foreach (var v in goals)
            {
                Commands.pathfind("path to all goals", new string[]{
               "addStart",
               "currentPosition"
            });
                queue = new List<TileNode>();
                currentGoal = v;
                pathFindToSingleGoal(source, v, queue); //v is a goal in my goal list, queue is my queue to work with,and I always set my start to where I am at.
                List<TileNode> removalList = new List<TileNode>();
                foreach(var tile in path)
                {
                    removalList.Add(tile);
                }
                foreach (var tile in removalList)
                {
                    StardustCore.ModCore.SerializationManager.trackedObjectList.Remove(tile);
                    (tile as TileNode).thisLocation.objects.Remove(tile.tileLocation);
                }
                cleanseGoals.Add(v);
            }
            foreach(var v in cleanseGoals)
            {
                goals.Remove(v);
            }

            
        }


        public static void pathFindToSingleGoal(TileNode Source, TileNode Goal, List<TileNode> Queue)
        {
         object[] obj = new object[3];
            obj[0] = Source;
            obj[1] = Goal;
            obj[2] = Queue;
            pathFindToSingleGoal(obj);
         }

        public static void pathFindToSingleGoal(object data)
        {
            ModCore.CoreMonitor.Log("LET'S GO!!!!", LogLevel.Error);
            object[] obj = (object[])data;

            foreach(var v in obj)
            {
                ModCore.CoreMonitor.Log(v.ToString(), LogLevel.Warn);
            }

            TileNode Source =(TileNode) obj[0];
            TileNode Goal = (TileNode)obj[1];
            List<TileNode> Queue = (List<TileNode>)obj[2];
            totalPathCost = 0;
            TileNode currentNode = Source;
            queue.Add(currentNode);
            index++;
            bool goalFound = false;
            while (currentNode.tileLocation != Goal.tileLocation && queue.Count != 0)
            {

                //Add children to current node
                int xMin = -1;
                int yMin = -1;
                int xMax = 1;
                int yMax = 1;

                //try to set children to tiles where children haven't been before
                for (int x = xMin; x <= xMax; x++)
                {
                    for (int y = yMin; y <= yMax; y++)
                    {
                        if (x == 0 && y == 0) continue;

                        //Include these 4 checks for just left right up down movement. Remove them to enable 8 direction path finding
                        if (x == -1 && y == -1) continue; //upper left
                        if (x == -1 && y ==  1) continue; //bottom left
                        if (x == 1 && y == -1) continue; //upper right
                        if (x == 1 && y == 1) continue; //bottom right
                        //TileNode t = new TileNode(1, Vector2.Zero, Souce.texturePath,source.dataPath, source.drawColor);
                        TileNode.setSingleTileAsChild(currentNode, (int)currentNode.tileLocation.X + x, (int)currentNode.tileLocation.Y + y);
                        Vector2 check = new Vector2((int)currentNode.tileLocation.X + x, (int)currentNode.tileLocation.Y + y);
                        if(check.X==Goal.tileLocation.X && check.Y == Goal.tileLocation.Y)
                        {
                            Goal.parent = currentNode;
                            currentNode.drawColor = StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.DarkOrange);
                            currentNode = Goal;
                           // ModCore.CoreMonitor.Log("SNAGED THE GOAL!!!!!!");
                            //System.Threading.Thread.Sleep(2000);
                            goalFound = true;
                        }
                    }
                }
                if (goalFound == true)
                {
                    currentNode = Goal;
                    //ModCore.CoreMonitor.Log("FOUND YOU!!!");
                    //System.Threading.Thread.Sleep(2000);
                    break;
                }
                List<TileNode> adjList = new List<TileNode>();
                foreach (var node in currentNode.children) {
                    //TileNode t = new TileNode(1, Vector2.Zero, Souce.texturePath,source.dataPath, source.drawColor);
                    //TileNode.setSingleTileAsChild(source, (int)source.tileLocation.X + x, (int)source.tileLocation.Y + y);
                    if (node.parent == null)
                    {
                        ModCore.CoreMonitor.Log("I DONT UNDERSTAND!");
                        System.Threading.Thread.Sleep(delay);
                    }
                    //ModCore.CoreMonitor.Log("ok checking adj:" + node.tileLocation.ToString());


                    if (node.seenState == 0)
                    {
                        node.drawColor = StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.LightPink); //Seen
                        adjList.Add(node);
                    }
                    if (node.seenState == 1)
                    {
                        node.drawColor = StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Blue);
                    }
                    if (node.seenState == 2)
                    {
                        node.drawColor = StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.DarkOrange);
                    }
                }




                foreach (var v in adjList)
                {
                    if (queue.Contains(v)) continue;
                    else queue.Add(v);
                }
                currentNode.seenState = 2;
                
                currentNode.drawColor = StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.DarkOrange); //Finished
                try
                {
                    currentNode = queue.ElementAt(index);
                }
                catch(Exception err)
                {
                    break;
                }
                currentNode.drawColor = StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Blue); //Working
                index++;
          
            }

            if (currentNode.tileLocation != Goal.tileLocation)
            {
                ModCore.CoreMonitor.Log("NO PATH FOUND", LogLevel.Error);
                return;
            }

            if (currentNode.tileLocation == Goal.tileLocation)
            {
                ModCore.CoreMonitor.Log("SWEET BEANS!!!!!!", LogLevel.Error);
                queue.Clear();
                index = 0;
                //ModCore.CoreMonitor.Log(currentNode.parent.ToString(), LogLevel.Error);
                currentNode.drawColor = StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.LightGreen);
                //currentGoal.drawColor=
            }

            while (currentNode.parent != null)
            {
                currentNode.drawColor= StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Red); //Working
                path.Add(currentNode);
                if (currentNode.parent.tileLocation.X<currentNode.tileLocation.X)
                {
                    currentNode.parent.animationManager.setAnimation("Right", 0);
                }
                if (currentNode.parent.tileLocation.X > currentNode.tileLocation.X)
                {
                    currentNode.parent.animationManager.setAnimation("Left", 0);
                }
                if (currentNode.parent.tileLocation.Y < currentNode.tileLocation.Y)
                {
                    currentNode.parent.animationManager.setAnimation("Down", 0);
                }
                if (currentNode.parent.tileLocation.Y > currentNode.tileLocation.Y)
                {
                    currentNode.parent.animationManager.setAnimation("Up", 0);
                }
                currentNode.parent.animationManager.enableAnimation();
                currentNode = currentNode.parent;
                System.Threading.Thread.Sleep(delay);
                if (currentNode.parent == null)
                {
                    currentNode.drawColor = StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Red); //Working
                    path.Add(currentNode);
                }
            }
            List<TileNode> removalList = new List<TileNode>();
            foreach(var v in StardustCore.ModCore.SerializationManager.trackedObjectList)
            {
                if(v is TileNode)
                {
                    if (path.Contains(v) || goals.Contains(v))
                    {
                        continue;
                    }
                    else
                    {
                        removalList.Add((TileNode)v);
                    }
                }
            }
            foreach(var v in removalList)
            {
                StardustCore.ModCore.SerializationManager.trackedObjectList.Remove(v);
                (v as TileNode).thisLocation.objects.Remove(v.tileLocation);
            }

            calculateMovement(path);
           // goals.Remove(Goal);
            
        }

        public static void calculateMovement(List<TileNode> path)
        {
            path.Reverse();
            bool xTargetReached = false;
            bool yTargetReached = false;
            while (path.Count > 0)
            {
                TileNode w = path[0];
                ModCore.CoreMonitor.Log("Goto: " + w.tileLocation.ToString());
                ModCore.CoreMonitor.Log("My position now: " + Game1.player.getTileLocation());
                //ModCore.CoreMonitor.Log("My Point position now: " + Game1.player.getTileLocationPoint());
                if (Game1.player.getTileX() == w.tileLocation.X && Game1.player.getTileY() == w.tileLocation.Y)
                {
                    path.Remove(w);
                    w.thisLocation.objects.Remove(w.tileLocation);
                    xTargetReached = false;
                    yTargetReached = false;
                    StardustCore.ModCore.SerializationManager.trackedObjectList.Remove(w);
                    ModCore.CoreMonitor.Log("LOOOP", LogLevel.Debug);
                    // return;
                    continue;
                }
                else
                {
                    Vector2 center = Utilities.parseCenterFromTile((int)w.tileLocation.X, (int)w.tileLocation.Y);
                    while (Game1.player.position.X > center.X && xTargetReached == false)
                    {
                        if (Utilities.isWithinRange(Game1.player.position.X, center.X, 6) == true)
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
                        ModCore.CoreMonitor.Log("Player x: " + Game1.player.position.X);
                        ModCore.CoreMonitor.Log("center x: " + center.X);
                        if (Utilities.isWithinRange(Game1.player.position.X, center.X, 6) == true)
                        {
                            xTargetReached = true;
                            ModCore.CoreMonitor.Log("XXXXXXXtargetReached");
                            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
                            continue;
                        }
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_D);
                    }
                    while (Game1.player.position.Y < center.Y && yTargetReached == false)
                    {
                        ModCore.CoreMonitor.Log("banana");
                        if (Utilities.isWithinRange(Game1.player.position.Y, center.Y, 6) == true)
                        {
                            yTargetReached = true;
                            ModCore.CoreMonitor.Log("YYYYYYYYYtargetReached");
                            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_S);
                            continue;
                        }
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_S);
                    }
                    while (Game1.player.position.Y > center.Y && yTargetReached == false)
                    {
                        ModCore.CoreMonitor.Log("potato");
                        if (Utilities.isWithinRange(Game1.player.position.Y, center.Y, 6) == true)
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
                        path.Remove(w);



                        xTargetReached = false;
                        yTargetReached = false;
                        StardustCore.ModCore.SerializationManager.trackedObjectList.Remove(w);
                        w.thisLocation.objects.Remove(w.tileLocation);

                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_S);
                        //return;
                        ModCore.CoreMonitor.Log("Reached goal!", LogLevel.Error);
                        //Game1.player.position = new Vector2(center.X, center.Y);
                        continue;
                    }


                    ModCore.CoreMonitor.Log("UNCAUGHT EXCEPTION", LogLevel.Error);
                }
            }
            
        }


    }
}
