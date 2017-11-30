using Microsoft.Xna.Framework;
using StarAI.ExecutionCore.TaskPrerequisites;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarAI.PathFindingCore.DebrisLogic
{
    class DebrisLogic
    {
        public static List<TileNode> sticksToChop = new List<TileNode>();


        public static void getAllSticksToChop(GameLocation location)
        {
            object[] arr = new object[1];
            arr[0] = location;
            getAllSticksToChop(arr);
        }

        /// <summary>
        /// DO NOT USE THIS UNLESS YOU WANT LAG UP THE WAZOO
        /// </summary>
        /// <param name="obj"></param>
        public static void getAllSticksToChop(object obj)
        {
            int twingCount = 0;
            object[] objArr = (object[])obj;
            GameLocation location = (GameLocation)objArr[0];
            foreach (var v in location.objects)
            {
                ModCore.CoreMonitor.Log(v.Value.name);

                if (v.Value.name == "Twig")
                {
                    ModCore.CoreMonitor.Log(v.Value.name,LogLevel.Warn);
                    TileNode t = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Brown));
                    //t.placementAction(Game1.currentLocation, (int)v.Key.X * Game1.tileSize, (int)v.Key.Y * Game1.tileSize);
                    t.fakePlacementAction(Game1.currentLocation, (int)v.Key.X, (int)v.Key.Y);
                    //t.tileLocation = new Vector2((int)v.Key.X, (int)v.Key.Y);
                    //t.position = new Vector2(v.Key.X * Game1.tileSize, v.Key.Y * Game1.tileSize);
                    //t.thisLocation = location;
                    //StardustCore.Utilities.masterAdditionList.Add(new StardustCore.DataNodes.PlacementNode(t, Game1.currentLocation, (int)v.Key.X * Game1.tileSize, (int)v.Key.Y * Game1.tileSize));
                    Utilities.tileExceptionList.Add(new TileExceptionMetaData(t, "ChopStick"));
                    sticksToChop.Add(t);
                    twingCount++;
                }
                }


            int ok = 0;
            foreach (var v in sticksToChop)
            {

                object[] objList = new object[1];
                objList[0] = v;
                // ExecutionCore.TaskList.taskList.Add(new Task(new Action<object>(waterSingleCrop), obj));
                StardewValley.Tools.Axe w = new StardewValley.Tools.Axe();
                ModCore.CoreMonitor.Log("Processing twig:" + ok.ToString() + " / " + twingCount.ToString());
                ok++;
                ExecutionCore.CustomTask task = new ExecutionCore.CustomTask(chopSingleStick, objList, new ExecutionCore.TaskMetaData("Chop Single Stick", new StaminaPrerequisite(true, 3), new ToolPrerequisite(true, w.GetType(), 1)));
                if (task.taskMetaData.cost == Int32.MaxValue)
                {
                    System.Threading.Thread.Sleep(1000);
                    Utilities.clearExceptionListWithNames(true);
                    continue;
                }

                ModCore.CoreMonitor.Log("TASK COST:" + task.taskMetaData.cost.ToString());
                if (task.taskMetaData.cost.ToString()=="2.147484E+09")
                {
                    ModCore.CoreMonitor.Log("OHH THAT's BAD");
                    System.Threading.Thread.Sleep(2000);
                }
                    ExecutionCore.TaskList.taskList.Add(task);
                ModCore.CoreMonitor.Log("TASK LIST COUNT:"+ExecutionCore.TaskList.taskList.Count.ToString());
                Utilities.clearExceptionListWithName(true, "Child");
                Utilities.clearExceptionListWithName("Child");
                //   waterSingleCrop(v);
            }
        }


        public static void getAllSticksToChopRadius(GameLocation location)
        {
            object[] arr = new object[1];
            arr[0] = location;
            getAllSticksToChopRadius(arr);
        }


        public static StardewValley.Object checkRadiusForObject(int radius,string name)
        {
            for(int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    bool f= Game1.player.currentLocation.isObjectAt((Game1.player.getTileX() + x) * Game1.tileSize, (Game1.player.getTileY() + y) * Game1.tileSize);
                    if (f == false) continue;
                    StardewValley.Object obj = Game1.player.currentLocation.getObjectAt((Game1.player.getTileX() + x)*Game1.tileSize, (Game1.player.getTileY() + y)*Game1.tileSize);
                    if (obj == null) continue;
                    if (obj.name==name)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }

        public static bool doesLocationContainObject(GameLocation location, string name)
        {
           foreach(var v in location.objects)
            {
                if (name == v.Value.name) return true;
            }
            return false;
        }

        public static void getAllSticksToChopRadius(object obj)
        {
            int radius = 1;
            int twingCount = 0;
            object[] objArr = (object[])obj;
            GameLocation location = (GameLocation)objArr[0];
            if (doesLocationContainObject(location, "Twig")) {
                StardewValley.Object twig = checkRadiusForObject(radius, "Twig");
                while (twig == null)
                {
                    radius++;
                    twig = checkRadiusForObject(radius, "Twig");
                }


                ModCore.CoreMonitor.Log(twig.name);

                if (twig.name == "Twig")
                {
                    ModCore.CoreMonitor.Log(twig.name, LogLevel.Warn);
                    TileNode t = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Brown));
                    //t.placementAction(Game1.currentLocation, (int)v.Key.X * Game1.tileSize, (int)v.Key.Y * Game1.tileSize);
                    t.fakePlacementAction(Game1.currentLocation, (int)twig.tileLocation.X, (int)twig.tileLocation.Y);
                    //t.tileLocation = new Vector2((int)v.Key.X, (int)v.Key.Y);
                    //t.position = new Vector2(v.Key.X * Game1.tileSize, v.Key.Y * Game1.tileSize);
                    //t.thisLocation = location;
                    //StardustCore.Utilities.masterAdditionList.Add(new StardustCore.DataNodes.PlacementNode(t, Game1.currentLocation, (int)v.Key.X * Game1.tileSize, (int)v.Key.Y * Game1.tileSize));
                    Utilities.tileExceptionList.Add(new TileExceptionMetaData(t, "ChopStick"));
                    sticksToChop.Add(t);
                    twingCount++;
                }
            }
            else
            {
                ModCore.CoreMonitor.Log("Twig is not found at location.");
            }
        


            int ok = 0;
            foreach (var v in sticksToChop)
            {

                object[] objList = new object[2];
                objList[0] = v;
                // ExecutionCore.TaskList.taskList.Add(new Task(new Action<object>(waterSingleCrop), obj));
                StardewValley.Tools.Axe w = new StardewValley.Tools.Axe();
                ModCore.CoreMonitor.Log("Processing twig:" + ok.ToString() + " / " + twingCount.ToString());
                ok++;
                ExecutionCore.CustomTask task = new ExecutionCore.CustomTask(chopSingleStick, objList, new ExecutionCore.TaskMetaData("Chop Single Stick", new StaminaPrerequisite(true, 3), new ToolPrerequisite(true, w.GetType(), 1)));
                objList[1] = task.taskMetaData.path;
                task.objectParameterDataArray = objList;


                if (task.taskMetaData.cost == Int32.MaxValue)
                {
                    System.Threading.Thread.Sleep(1000);
                    Utilities.clearExceptionListWithNames(true);
                    continue;
                }
                if (task.taskMetaData.cost.ToString() == "2.147484E+09")
                {
                    ModCore.CoreMonitor.Log("OHH THAT's BAD");
                    System.Threading.Thread.Sleep(2000);
                }

                ModCore.CoreMonitor.Log("TASK COST:" + task.taskMetaData.cost.ToString());

                ExecutionCore.TaskList.taskList.Add(task);
                Utilities.clearExceptionListWithName("Child");
                ModCore.CoreMonitor.Log("TASK LIST COUNT:" + ExecutionCore.TaskList.taskList.Count.ToString());
                //Utilities.clearExceptionListWithName(true, "Child");
                //   waterSingleCrop(v);
            }
            sticksToChop.Clear();
        }


        public static void chopSingleStick(TileNode v,List<TileNode> path)
        {
            object[] obj = new object[1];
            obj[0] = v;
            obj[1] = path;
            chopSingleStick(obj);
        }



        public static void chopSingleStick(object obj)
        {
            
            object[] objArray=(object[])obj;
           
            TileNode v = (TileNode)objArray[0];
            //List<TileNode> correctPath = Utilities.pathStuff(v);//(List<TileNode>)objArray[1];
            List<TileNode> correctPath =(List<TileNode>)objArray[1];
            foreach (var goodTile in correctPath)
            {
                StardustCore.ModCore.SerializationManager.trackedObjectList.Add(goodTile);
                //StardustCore.Utilities.masterAdditionList.Add(new StardustCore.DataNodes.PlacementNode(goodTile, Game1.currentLocation, (int)goodTile.tileLocation.X * Game1.tileSize, (int)goodTile.tileLocation.Y * Game1.tileSize));
                goodTile.placementAction(goodTile.thisLocation, (int)goodTile.tileLocation.X * Game1.tileSize, (int)goodTile.tileLocation.Y * Game1.tileSize);

            }
            PathFindingLogic.calculateMovement(correctPath);



            if (v.tileLocation.X < Game1.player.getTileX())
            {
                Game1.player.faceDirection(3);
            }
            else if (v.tileLocation.X > Game1.player.getTileX())
            {
                Game1.player.faceDirection(1);
            }
            else if (v.tileLocation.Y < Game1.player.getTileY())
            {
                Game1.player.faceDirection(0);
            }
            else if (v.tileLocation.Y > Game1.player.getTileY())
            {
                Game1.player.faceDirection(2);
            }
            foreach (var item in Game1.player.items)
            {
                if (item is StardewValley.Tools.Axe)
                {
                    Game1.player.CurrentToolIndex = Game1.player.getIndexOfInventoryItem(item);
                }
            }
            bool move = false;
            StardewValley.Object twig = v.thisLocation.objects[v.tileLocation];
            while ((twig.name=="Twig"))
            {
                if (!v.thisLocation.isObjectAt((int)v.tileLocation.X*Game1.tileSize, (int)v.tileLocation.Y*Game1.tileSize)) break;
                if (WindowsInput.InputSimulator.IsKeyDown(WindowsInput.VirtualKeyCode.VK_C) == false) WindowsInput.InputSimulator.SimulateKeyDown(WindowsInput.VirtualKeyCode.VK_C);

                Vector2 center = new Vector2();
                if (Game1.player.facingDirection == 2)
                {
                    center = Utilities.parseCenterFromTile((int)v.tileLocation.X + 1, (int)v.tileLocation.Y);
                    continue;
                }
                if (Game1.player.facingDirection == 1)
                {
                    center = Utilities.parseCenterFromTile((int)v.tileLocation.X - 1, (int)v.tileLocation.Y);
                    continue;
                }
                if (Game1.player.facingDirection == 0)
                {
                    center = Utilities.parseCenterFromTile((int)v.tileLocation.X, (int)v.tileLocation.Y + 1);
                    continue;

                }
                if (Game1.player.facingDirection == 3)
                {
                    center = Utilities.parseCenterFromTile((int)v.tileLocation.X, (int)v.tileLocation.Y - 1);
                    continue;
                }
                Game1.player.position = center;


                //Game1.setMousePosition((int)v.tileLocation.X*Game1.tileSize/2,(int)v.tileLocation.Y*Game1.tileSize/2);
                ModCore.CoreMonitor.Log("DOESNT Axe LIKE YOU THINK IT SHOULD");
                ModCore.CoreMonitor.Log("player pos: " + Game1.player.position.ToString(), LogLevel.Warn);
                ModCore.CoreMonitor.Log("TilePos: " + v.position.ToString(), LogLevel.Error);
            }
            Utilities.cleanExceptionList(v);
            StardustCore.ModCore.SerializationManager.trackedObjectList.Remove(v);
            // StardustCore.Utilities.masterRemovalList.Add(v);
            //v.performRemoveAction(v.tileLocation, v.thisLocation);
           // v.thisLocation.objects.Remove(v.tileLocation);
            foreach (var goodTile in correctPath)
            {
                StardustCore.ModCore.SerializationManager.trackedObjectList.Remove(goodTile);
                //StardustCore.Utilities.masterRemovalList.Add(v);
                goodTile.performRemoveAction(goodTile.tileLocation, goodTile.thisLocation);
                //goodTile.placementAction(goodTile.thisLocation, (int)goodTile.tileLocation.X * Game1.tileSize, (int)goodTile.tileLocation.Y * Game1.tileSize);
            }
            WindowsInput.InputSimulator.SimulateKeyUp(WindowsInput.VirtualKeyCode.VK_C);
        }


    }
}
