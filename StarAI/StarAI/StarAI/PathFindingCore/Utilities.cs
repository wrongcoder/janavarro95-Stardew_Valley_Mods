using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace StarAI.PathFindingCore
{
    public class Utilities
    {
        

        public static  List<TileExceptionMetaData> tileExceptionList = new List<TileExceptionMetaData>();

        public static List<TileExceptionNode> ignoreCheckTiles = new List<TileExceptionNode>();
       public static string folderForExceptionTiles="ExceptionTilesData";




        public static Vector2 parseCenterFromTile(int tileX, int tileY)
        {
            //int x = (tileX * Game1.tileSize) + Game1.tileSize / 2;
            //int y = (tileY * Game1.tileSize) + Game1.tileSize / 2;
            int x = (tileX * Game1.tileSize);
            int y = (tileY * Game1.tileSize);
            return new Vector2(x, y);
        }

        public static void initializeTileExceptionList()
        {
            
        }
        

        public static void cleanExceptionList(TileNode t)
        {
            TileExceptionMetaData err= new TileExceptionMetaData(null,"");
            foreach (var v in tileExceptionList)
            {
                if (v.tile == t) err = v;
            }
            if(err.tile != null)
            {
                tileExceptionList.Remove(err);
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


        public static int calculatePathCost(TileNode v, bool Placement = true)
        {
            object[] obj = new object[2];
            obj[0] = v;
            obj[1] = Placement;
            return calculatePathCost(obj);
        }

        public static int calculatePathCost(object obj)
        {
            object[] objArr = (object[])obj;
            TileNode v = (TileNode)objArr[0];
            bool placement;
            try
            {
                 placement = (bool)objArr[1];
            }
            catch(Exception err)
            {
                 placement = true;
            }
            foreach (var q in objArr)
            {
                ModCore.CoreMonitor.Log("OK THIS IS THE RESULT !: " + q, LogLevel.Alert);
            }
            if (v == null) ModCore.CoreMonitor.Log("WTF MARK!!!!!!: ", LogLevel.Alert);
            bool moveOn = false;

            WindowsInput.InputSimulator.SimulateKeyUp(WindowsInput.VirtualKeyCode.VK_X);
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
                    //ModCore.CoreMonitor.Log("AHHHHHHH POSITION: " + pos.ToString(), LogLevel.Alert);
                    bool f = PathFindingCore.TileNode.checkIfICanPlaceHere(v, pos * Game1.tileSize, v.thisLocation, true);
                    ModCore.CoreMonitor.Log("OK THIS IS THE RESULT F: " + f, LogLevel.Alert);
                    if (f == true)
                    {

                        TileNode t = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.RosyBrown));
                        if(placement)t.placementAction(Game1.currentLocation, (int)pos.X * Game1.tileSize, (int)pos.Y * Game1.tileSize);
                        else t.fakePlacementAction(v.thisLocation, (int)pos.X, (int)pos.Y);
                        //StardustCore.Utilities.masterAdditionList.Add(new StardustCore.DataNodes.PlacementNode(t, Game1.currentLocation, (int)pos.X * Game1.tileSize, (int)pos.Y * Game1.tileSize));
                        miniGoals.Add(t);
                        Utilities.tileExceptionList.Add(new TileExceptionMetaData(t, "CropNavigation"));
                    }
                }
            }
            List<TileNode> removalList = new List<TileNode>();
            foreach (var nav in miniGoals)
            {
                TileNode tempSource = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.RosyBrown));
                if(placement)tempSource.placementAction(Game1.player.currentLocation, Game1.player.getTileX() * Game1.tileSize, Game1.player.getTileY() * Game1.tileSize);
                else tempSource.fakePlacementAction(Game1.player.currentLocation, Game1.player.getTileX(), Game1.player.getTileY());
               
                List<TileNode> path = PathFindingCore.PathFindingLogic.pathFindToSingleGoalReturnPath(tempSource, nav, new List<TileNode>(), placement);

                ModCore.CoreMonitor.Log(tempSource.tileLocation.ToString()+tempSource.tileLocation.ToString());
                ModCore.CoreMonitor.Log(nav.tileLocation.ToString() + nav.tileLocation.ToString());

                if (path.Count != 0)
                {
                    ModCore.CoreMonitor.Log("PATH WAS NOT NULL", LogLevel.Warn);
                    paths.Add(path);
                    foreach (var someTile in path)
                    {
                        if (someTile == nav) removalList.Add(someTile);
                        StardustCore.ModCore.SerializationManager.trackedObjectList.Remove(someTile);
                        if(placement)someTile.thisLocation.objects.Remove(someTile.tileLocation);
                        //someTile.performRemoveAction(someTile.tileLocation, someTile.thisLocation);    
                        //StardustCore.Utilities.masterRemovalList.Add(v);
                    }
                }

            }
            foreach (var q in removalList)
            {
                StardustCore.ModCore.SerializationManager.trackedObjectList.Remove(q);
                if(placement)q.thisLocation.objects.Remove(q.tileLocation);
            }
            removalList.Clear();
            int pathCost = 999999999;
            List<TileNode> correctPath = new List<TileNode>();
            foreach (var potentialPath in paths)
            {
                if (potentialPath.Count == 0) continue;
                if (potentialPath.Count < pathCost)
                {
                    pathCost = potentialPath.Count;
                    correctPath = potentialPath;
                }
            }

            foreach (var goodTile in correctPath)
            {
                StardustCore.ModCore.SerializationManager.trackedObjectList.Add(goodTile);
                //goodTile.placementAction(goodTile.thisLocation, (int)goodTile.tileLocation.X * Game1.tileSize, (int)goodTile.tileLocation.Y * Game1.tileSize);
                //StardustCore.Utilities.masterAdditionList.Add(new StardustCore.DataNodes.PlacementNode(goodTile, Game1.currentLocation, (int)goodTile.tileLocation.X * Game1.tileSize, (int)goodTile.tileLocation.Y * Game1.tileSize));
            }
            //END HERE FOR JUST CALCULATING PATH COST
            if (paths.Count == 0) return Int32.MaxValue;
            return correctPath.Count;
        }

    }
}
