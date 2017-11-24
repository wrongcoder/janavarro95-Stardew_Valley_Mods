using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardustCore;
using StardewValley;
using Microsoft.Xna.Framework;
using System.IO;

namespace StarAI.PathFindingCore.CropLogic
{
    

    class CropLogic
    {
        public static List<TileNode> cropsToWater = new List<TileNode>();

        public static void getAllCropsNeededToBeWatered()
        {
            foreach (var v in Game1.player.currentLocation.terrainFeatures)
            {

                if (v.Value is StardewValley.TerrainFeatures.HoeDirt)
                {
                    if ((v.Value as StardewValley.TerrainFeatures.HoeDirt).crop != null)
                    {
                        //cropsToWater.Add(v.Key);
                        //If my dirt needs to be watered and the crop isn't fully grown.
                        if ((v.Value as StardewValley.TerrainFeatures.HoeDirt).needsWatering() == true && (v.Value as StardewValley.TerrainFeatures.HoeDirt).crop.fullyGrown == false)
                        {
                            TileNode t = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.LightSkyBlue));
                            t.placementAction(Game1.currentLocation, (int)v.Key.X * Game1.tileSize, (int)v.Key.Y * Game1.tileSize);
                            Utilities.tileExceptionList.Add(new TileExceptionMetaData(t, "Water"));
                            cropsToWater.Add(t);
                        }
                    }
                }
            }

            //Instead of just running this function I should add it to my execution queue.
            foreach(var v in cropsToWater)
            {
                object[] obj = new object[1];
                obj[0] = v;
                ExecutionCore.TaskList.taskList.Add(new Task(new Action<object>(waterSingleCrop), obj));
             //   waterSingleCrop(v);
            }
        }

        public static void waterSingleCrop(TileNode v)
        {
            object[] obj = new object[1];
            obj[0] = v;
            waterSingleCrop(obj);
        }


        public static void waterSingleCrop(object obj) {
            object[] objArr =(object[]) obj;
            TileNode v =(TileNode) objArr[0];
            bool moveOn = false;
            foreach (var q in Utilities.tileExceptionList)
            {
                if(q.tile==v && q.actionType=="Water")
                {
                    moveOn = true;
                }
            }
            if (moveOn == false) return;

                WindowsInput.InputSimulator.SimulateKeyUp(WindowsInput.VirtualKeyCode.VK_C);
                int xMin = -1;
                int yMin = -1;
                int xMax = 1;
                int yMax = 1;
                List<TileNode> miniGoals = new List<TileNode>();
                List<List<TileNode>> paths = new List<List<TileNode>>();
                //try to set children to tiles where children haven't been before
                for (int x = xMin; x <= xMax; x++)
                {                    for (int y = yMin; y <= yMax; y++)
                    {
                        if (x == 0 && y == 0) continue;

                        //Include these 4 checks for just left right up down movement. Remove them to enable 8 direction path finding
                        if (x == -1 && y == -1) continue; //upper left
                        if (x == -1 && y == 1) continue; //bottom left
                        if (x == 1 && y == -1) continue; //upper right
                        if (x == 1 && y == 1) continue; //bottom right

                        Vector2 pos = new Vector2(v.tileLocation.X + x, v.tileLocation.Y + y);
                        ModCore.CoreMonitor.Log("AHHHHHHH POSITION: " + pos.ToString(), LogLevel.Alert);
                        bool f=  PathFindingCore.TileNode.checkIfICanPlaceHere(v, pos*Game1.tileSize, v.thisLocation,true);
                        ModCore.CoreMonitor.Log("OK THIS IS THE RESULT F: " + f, LogLevel.Alert);
                        if (f == true)
                        {
                         
                            TileNode t = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.RosyBrown));
                            t.placementAction(Game1.currentLocation,(int)pos.X * Game1.tileSize, (int)pos.Y * Game1.tileSize);
                            miniGoals.Add(t);
                            Utilities.tileExceptionList.Add(new TileExceptionMetaData(t,"CropNavigation"));
                        }
                    }
                }
                foreach(var nav in miniGoals)
                {
                    TileNode tempSource= new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.RosyBrown));
                    tempSource.placementAction(Game1.player.currentLocation, Game1.player.getTileX()*Game1.tileSize, Game1.player.getTileY()*Game1.tileSize);
                    List<TileNode> path=  PathFindingCore.PathFindingLogic.pathFindToSingleGoalReturnPath(tempSource,nav,new List<TileNode>());
                    if (path != null)
                    {
                        ModCore.CoreMonitor.Log("PATH WAS NOT NULL", LogLevel.Warn);
                        paths.Add(path);
                        foreach(var someTile in path)
                        {
                            StardustCore.ModCore.SerializationManager.trackedObjectList.Remove(someTile);
                            someTile.thisLocation.objects.Remove(someTile.tileLocation);
                        }
                    }
                    
                }
                int pathCost = 999999999;
                List<TileNode> correctPath = new List<TileNode>();
                foreach(var potentialPath in paths)
                {
                    if (potentialPath.Count < pathCost)
                    {
                        pathCost = potentialPath.Count;
                        correctPath = potentialPath;
                    }
                }
                foreach (var goodTile in correctPath) {
                    StardustCore.ModCore.SerializationManager.trackedObjectList.Add(goodTile);
                    goodTile.placementAction(goodTile.thisLocation,(int) goodTile.tileLocation.X*Game1.tileSize, (int)goodTile.tileLocation.Y*Game1.tileSize);
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
                    if(item is StardewValley.Tools.WateringCan)
                    {
                        Game1.player.CurrentToolIndex = Game1.player.getIndexOfInventoryItem(item);
                    }
                }
                bool move = false;
                while ((v.thisLocation.terrainFeatures[v.tileLocation] as StardewValley.TerrainFeatures.HoeDirt).state==0)
                {
                  if(WindowsInput.InputSimulator.IsKeyDown(WindowsInput.VirtualKeyCode.VK_C)==false)  WindowsInput.InputSimulator.SimulateKeyDown(WindowsInput.VirtualKeyCode.VK_C);

                    Vector2 center=new Vector2();
                    if (Game1.player.facingDirection == 2)
                    {
                         center = Utilities.parseCenterFromTile((int)v.tileLocation.X+1, (int)v.tileLocation.Y);
                        continue;
                    }
                    if (Game1.player.facingDirection == 1)
                    {
                        center = Utilities.parseCenterFromTile((int)v.tileLocation.X-1, (int)v.tileLocation.Y);
                        continue;
                    }
                    if (Game1.player.facingDirection == 0)
                    {
                        center = Utilities.parseCenterFromTile((int)v.tileLocation.X, (int)v.tileLocation.Y+1);
                        continue;

                    }
                    if (Game1.player.facingDirection == 3)
                    {
                        center = Utilities.parseCenterFromTile((int)v.tileLocation.X, (int)v.tileLocation.Y-1);
                        continue;
                    }
                    //Get center of destination tile and move player there.
                    /*
                    if (v.position.X < Game1.player.position.X)
                    {
                        Game1.player.position = new Vector2(Game1.player.position.X + .1f, Game1.player.position.Y);
                        move = false;
                        continue;
                    }
                    if (v.position.X > Game1.player.position.X)
                    {
                        Game1.player.position = new Vector2(Game1.player.position.X - .1f, Game1.player.position.Y);
                        move = false;
                        continue;
                    }
                    if (v.position.Y < Game1.player.position.Y)
                    {
                        Game1.player.position = new Vector2(Game1.player.position.X, Game1.player.position.Y + .1f);
                        move = false;
                        continue;
                    }
                    if (v.position.Y > Game1.player.position.Y)
                    {
                        Game1.player.position = new Vector2(Game1.player.position.X, Game1.player.position.Y - .1f);
                        move = false;
                        continue;
                    }
                    */
                    Game1.player.position = center;
                  
                 
                    //Game1.setMousePosition((int)v.tileLocation.X*Game1.tileSize/2,(int)v.tileLocation.Y*Game1.tileSize/2);
                    ModCore.CoreMonitor.Log("DOESNT WATER LIKE YOU THINK IT SHOULD");
                    ModCore.CoreMonitor.Log("player pos: "+Game1.player.position.ToString(),LogLevel.Warn);
                    ModCore.CoreMonitor.Log("TilePos: "+v.position.ToString(), LogLevel.Error);
                }
                Utilities.cleanExceptionList(v);
                StardustCore.ModCore.SerializationManager.trackedObjectList.Remove(v);
                v.thisLocation.objects.Remove(v.tileLocation);

                WindowsInput.InputSimulator.SimulateKeyUp(WindowsInput.VirtualKeyCode.VK_C);
            }

        
        
         

    }
}
