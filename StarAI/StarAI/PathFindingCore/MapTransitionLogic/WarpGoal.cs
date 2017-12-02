using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarAI.PathFindingCore.MapTransitionLogic
{
    public class WarpGoal
    {
        public WarpGoal parentWarpGoal;
        public Warp warp;
        public List<WarpGoal> childrenWarps;

        public WarpGoal(WarpGoal Parent, Warp CurrentWarp)
        {
            this.parentWarpGoal = Parent;
            this.warp = CurrentWarp;
            this.childrenWarps = new List<WarpGoal>();
        }

        public static List<WarpGoal> getWarpChain(GameLocation location,string mapName)
        {
            GameLocation check = Game1.getLocationFromName(mapName);
            if (check == null)
            {
                ModCore.CoreMonitor.Log("INVALID LOCATION");
                return null;
            }
            //init
            List<WarpGoal> startinggoals = new List<WarpGoal>();
            foreach(var Warp in location.warps)
            {
                WarpGoal child = new WarpGoal(null, Warp);
                startinggoals.Add(child);
                if (Warp.TargetName == mapName)
                {
                    List<WarpGoal> listOfOne = new List<WarpGoal>();
                    listOfOne.Add(child);
                    return listOfOne;
                }
            }

            //keep chaining children

           List<WarpGoal> warpChain= okBye(startinggoals, mapName);
            if (warpChain == null)
            {
                ModCore.CoreMonitor.Log("NULL WARP CHAIN");
                return null;
            }
            if (warpChain.Count == 0)
            {
                ModCore.CoreMonitor.Log("NULL WARP CHAIN OR CAN't FIND PATH TO LOCATION");
                return null;
            }
            
          

            foreach(var v in warpChain)
            {
                if (v.parentWarpGoal != null)
                {
                    ModCore.CoreMonitor.Log("Take this warp from location to destination:" +v.parentWarpGoal.warp.TargetName +" To " + v.warp.TargetName);
                }
                else
                {
                    ModCore.CoreMonitor.Log("Take this warp from location to destination:" + Game1.player.currentLocation.name + " To " + v.warp.TargetName);
                }
            }

            List<List<TileNode>> pathMaster = new List<List<TileNode>>();
            warpChain.Reverse();

            foreach (var v in startinggoals)
            {
                if (v.warp.TargetName == warpChain.ElementAt(0).warp.TargetName)
                {
                    //v.parentWarpGoal = warpChain.ElementAt(warpChain.Count - 1);
                    warpChain.Insert(0,v);
                    ModCore.CoreMonitor.Log("Insert from" + Game1.player.currentLocation.name + " To " + v.warp.TargetName);
                    break;
                }
            }
          
            for (int i=0;i<warpChain.Count;i++)
            {
                WarpGoal v = warpChain[i];
                ModCore.CoreMonitor.Log("Processing:" +v.warp.TargetName);
                if (i == 0)
                {

                    TileNode s = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Brown));
                    s.fakePlacementAction(Game1.player.currentLocation, Game1.player.getTileX(), Game1.player.getTileY());
                    Utilities.tileExceptionList.Add(new TileExceptionMetaData(s, "WarpGoal"));

                    TileNode t = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Brown));
                    t.fakePlacementAction(Game1.currentLocation, v.warp.X, v.warp.Y);
                    Utilities.tileExceptionList.Add(new TileExceptionMetaData(t, "WarpGoal"));
                    pathMaster.Add(Utilities.getIdealPath(t,s));
                    Utilities.clearExceptionListWithName("Child");
                    Utilities.tileExceptionList.Clear();

                    ModCore.CoreMonitor.Log("OK COUNT:"+pathMaster.Count.ToString());

                    ModCore.CoreMonitor.Log(("Name: " + Game1.currentLocation + " X " + warpChain[i].warp.X + " Y " + warpChain[i].warp.Y));
                   // List<TileNode> miniPath = pathMaster.ElementAt(pathMaster.Count - 1);

                    continue;
                }
                else
                {
                   if (i == warpChain.Count - 1) continue;
                    ModCore.CoreMonitor.Log("Count:" +warpChain.Count.ToString());
                    ModCore.CoreMonitor.Log("I:" + i.ToString());
                    int index = i + 1;
                    ModCore.CoreMonitor.Log(("Name Source: " + warpChain[i].warp.TargetName + " X " + warpChain[index-1].warp.TargetX + " Y " + warpChain[index-1].warp.TargetY));
                    ModCore.CoreMonitor.Log(("Name Destination: " + warpChain[i].warp.TargetName + " X " + warpChain[index].warp.X + " Y " + warpChain[index].warp.Y));
                    try
                    {
                        TileNode tears = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Brown));
                        tears.fakePlacementAction(Game1.getLocationFromName(warpChain[i].warp.TargetName), warpChain[index].warp.X, warpChain[index].warp.Y);
                        Utilities.tileExceptionList.Add(new TileExceptionMetaData(tears, "WarpGoal"));

                        TileNode source = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.Brown));
                        source.fakePlacementAction(Game1.getLocationFromName(warpChain[i].warp.TargetName), warpChain[index-1].warp.TargetX, warpChain[index-1].warp.TargetY);
                        Utilities.tileExceptionList.Add(new TileExceptionMetaData(source, "WarpGoal"));


                        pathMaster.Add(Utilities.getIdealPath(tears,source));
                        Utilities.clearExceptionListWithName("Child");
                        Utilities.tileExceptionList.Clear();
                        continue;
                    }
                    catch (Exception err)
                    {
                        ModCore.CoreMonitor.Log("WTF ME I GUESS");
                        ModCore.CoreMonitor.Log(err.ToString());
                    }
                }
            }
            bool once = false;


            foreach(var path in pathMaster)
            {
                foreach(var v in path)
                {
                    ModCore.CoreMonitor.Log("This is my path LOL:" + v.thisLocation.ToString() + v.tileLocation.ToString(),StardewModdingAPI.LogLevel.Warn);
                }
            }



            while (pathMaster.Count != 0)
            {
                pathMaster.ElementAt(0).Remove(pathMaster.ElementAt(0).ElementAt( (pathMaster.ElementAt(0).Count-1) ) ); //get first path and remove first element from it because it will force me to warp back.
                ModCore.CoreMonitor.Log("Pathing to:" + pathMaster.ElementAt(0).ElementAt(0).thisLocation.ToString() + pathMaster.ElementAt(0).ElementAt(0).tileLocation.ToString());
                ModCore.CoreMonitor.Log("Pathing from:" + pathMaster.ElementAt(0).ElementAt(pathMaster.ElementAt(0).Count - 1).thisLocation.ToString() + pathMaster.ElementAt(0).ElementAt(pathMaster.ElementAt(0).Count - 1).tileLocation.ToString());

                if (once == false)
                {

                    foreach(var v in pathMaster.ElementAt(0))
                    {
                        ModCore.CoreMonitor.Log("This is my path:" + v.thisLocation.ToString() + v.tileLocation.ToString());
                    }
                    //pathMaster.ElementAt(0).Remove(pathMaster.ElementAt(0).ElementAt(0));
                    PathFindingLogic.calculateMovement(pathMaster.ElementAt(0));
                    ModCore.CoreMonitor.Log("WTF???");
                    once = true;
                    //warped = false;
                }
                else if (once == true)
                {
                    List<TileNode> temp = new List<TileNode>();
                    for(int i=0;i< pathMaster.ElementAt(0).Count; i++)
                    {
                       
                            temp.Add(pathMaster.ElementAt(0).ElementAt(i));
                        
                    }
                    ModCore.CoreMonitor.Log("Pathing from FIX:"+temp.ElementAt(temp.Count-1).thisLocation.ToString()+temp.ElementAt(temp.Count-1).tileLocation.ToString());

                    foreach (var v in temp)
                    {
                        ModCore.CoreMonitor.Log("This is my path modified:" + v.thisLocation.ToString() + v.tileLocation.ToString()+ v.position.ToString());
                    }
                   // temp.Remove(temp.ElementAt(0));
                    Game1.player.position = temp.ElementAt(temp.Count-1).position;
                    PathFindingLogic.calculateMovement(temp);
                }

                bool warped = false;
                for (int i = -1; i <= 1; i++)
                {

                    for (int j = -1; j <= 1; j++)
                    {
                        foreach (var warp in Game1.player.currentLocation.warps) //get location of tiles.
                        {
                            if (warp.X == Game1.player.getTileX() + i && warp.Y == Game1.player.getTileY() + j)
                            {
                                Game1.warpFarmer(warp.TargetName, warp.TargetX, warp.TargetY, false);
                                ModCore.CoreMonitor.Log("WARP:" + warped.ToString());
                                warped = true;
                                break;
                            }
                        }
                        if (warped == true) break;
                    }
                    if (warped == true) break;
                }
                warped = false;
                pathMaster.Remove(pathMaster.ElementAt(0));
                once = true;
            }


            //Do final location walk to stuff here.
            return warpChain;
            
        }

        public static List<WarpGoal> okBye(List<WarpGoal> param,string targetMapName)
        {
            bool found = false;
            WarpGoal theOne= new WarpGoal(null,null);
            List<WarpGoal> warpChain = new List<WarpGoal>();
            foreach (WarpGoal w in param)
            {
                GameLocation loc = Game1.getLocationFromName(w.warp.TargetName);
                foreach (var v in loc.warps)
                {
                    WarpGoal ok = new WarpGoal(w, v);
                    w.childrenWarps.Add(ok);
                    if (v.TargetName == targetMapName)
                    {
                        found = true;
                        theOne = ok;
                        break;
                    }
                }
                if (found == false)
                {
                  return okBye(w.childrenWarps,targetMapName);
                }
                if (found == true)
                {
                    while (theOne.parentWarpGoal != null)
                    {
                        warpChain.Add(theOne);
                        theOne = theOne.parentWarpGoal;
                    }
                    warpChain.Add(theOne);
                }
                return warpChain; 
                //recursively call this logic???
            }
            return new List<WarpGoal>();
        }

    }
}
