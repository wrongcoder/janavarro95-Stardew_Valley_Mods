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
            foreach(var v in Game1.player.currentLocation.terrainFeatures)
            {
                
                if(v.Value is StardewValley.TerrainFeatures.HoeDirt)
                {
                    if((v.Value as StardewValley.TerrainFeatures.HoeDirt).crop != null)
                    {
                        //cropsToWater.Add(v.Key);
                        //If my dirt needs to be watered and the crop isn't fully grown.
                        if ((v.Value as StardewValley.TerrainFeatures.HoeDirt).needsWatering() == true && (v.Value as StardewValley.TerrainFeatures.HoeDirt).crop.fullyGrown == false)
                        {
                            TileNode t = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.LightSkyBlue));
                            t.placementAction(Game1.currentLocation, (int)v.Key.X * Game1.tileSize, (int)v.Key.Y * Game1.tileSize);
                            Utilities.tileExceptionList.Add(new TileExceptionMetaData(t,"Water"));
                            cropsToWater.Add(t);
                        }
                    }
                }
            }



            foreach(var v in cropsToWater)
            {
                WindowsInput.InputSimulator.SimulateKeyUp(WindowsInput.VirtualKeyCode.VK_C);
                int xMin = -1;
                int yMin = -1;
                int xMax = 1;
                int yMax = 1;
                List<TileNode> miniGoals = new List<TileNode>();
                List<List<TileNode>> paths = new List<List<TileNode>>();
                //try to set children to tiles where children haven't been before
                for (int x = xMin; x <= xMax; x++)
                {
                    for (int y = yMin; y <= yMax; y++)
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
                if (v.tileLocation.X > Game1.player.getTileX())
                {
                    Game1.player.faceDirection(1);
                }
                if (v.tileLocation.Y < Game1.player.getTileY())
                {
                    Game1.player.faceDirection(0);
                }
                if (v.tileLocation.Y > Game1.player.getTileY())
                {
                    Game1.player.faceDirection(2);
                }
                foreach(var item in Game1.player.items)
                {
                    if(item is StardewValley.Tools.WateringCan)
                    {
                        Game1.player.CurrentToolIndex = Game1.player.getIndexOfInventoryItem(item);
                    }
                }
                WindowsInput.InputSimulator.SimulateKeyDown(WindowsInput.VirtualKeyCode.VK_C);
            }

        }
        
         

    }
}
