using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Revitalize.Framework.Objects.InformationFiles;
using Revitalize.Framework.Objects.Resources.OreVeins;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.Animations;
using StardustCore.UIUtilities;

namespace Revitalize.Framework.Objects
{
    public class ResourceManager
    {
        /// <summary>
        /// A static reference to the resource manager for quicker access.
        /// </summary>
        public static ResourceManager self;

        /// <summary>
        /// A list of all of the ores held by the resource manager.
        /// </summary>
        public Dictionary<string, OreVeinObj> ores;
        public List<int> visitedFloors;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ResourceManager()
        {
            self = this;
            this.ores = new Dictionary<string, OreVeinObj>();
            this.visitedFloors = new List<int>();
            this.loadOreVeins();
        }

        /// <summary>
        /// Loads in all of the ore veins for the game.
        /// </summary>
        private void loadOreVeins()
        {
            //The pancake ore.


            OreVeinObj testOre = new OreVeinObj(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Resources.Ore.Test", TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), typeof(OreVeinTile), Color.White), new BasicItemInformation("Test Ore Vein", "Omegasis.Revitalize.Resources.Ore.Test", "A ore vein that is used for testing purposes.", "Revitalize.Ore", Color.Black, -300, 0, false, 350, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Resources.Ore", "Test"), new Animation(0, 0, 16, 16)), Color.White, false, null, null));
            /*
            testOre.addComponent(new Vector2(0, 0), new OreVeinTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Resources.Ore.Test", TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), typeof(OreVeinTile), Color.White), new BasicItemInformation("Test Ore Vein", "Omegasis.Revitalize.Resources.Ore.Test", "A ore vein that is used for testing purposes.", "Revitalize.Ore", Color.Black, -300, 0, false, 350, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Resources.Ore", "Test"), new Animation(0, 0, 16, 16)), Color.White, false, null, null), new InformationFiles.OreResourceInformation(new StardewValley.Object(211, 1), false, true, true, false, new List<IntRange>()
            {
                new IntRange(1,9)
            }, new List<IntRange>(), 1, 5, 1, 10, 1d, 1d, 0, 0, 0, 0), new List<ResourceInformaton>()));
            this.ores.Add("Omegasis.Revitalize.Resources.Ore.Test", testOre);
            */
        }

        /// <summary>
        /// Spawns an ore vein at the given location if possible.
        /// </summary>
        /// <param name="name"></param>
        public bool spawnOreVein(string name, GameLocation Location, Vector2 TilePosition)
        {
            if (this.ores.ContainsKey(name))
            {
                OreVeinObj spawn;
                this.ores.TryGetValue(name, out spawn);
                if (spawn != null)
                {
                    spawn = (OreVeinObj)spawn.getOne();
                    bool spawnable = this.canResourceBeSpawnedHere(spawn, Location, TilePosition);
                    if (spawnable)
                    {
                        //ModCore.log("Location is: " + Location.Name);
                        spawn.placementAction(Location, (int)TilePosition.X * Game1.tileSize, (int)TilePosition.Y * Game1.tileSize, Game1.player);
                    }
                    else
                    {
                        ModCore.log("Can't spawn ore: " + name + "at tile location: " + TilePosition);
                    }
                    return spawnable;
                }
                ModCore.log("Key doesn't exist. Weird.");
                return false;
            }
            else
            {
                throw new Exception("The ore dictionary doesn't contain they key for resource: " + name);
            }
        }
        /// <summary>
        /// Spawns an orevein at the tile position at the same location as the player.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="TilePosition"></param>
        /// <returns></returns>
        public bool spawnOreVein(string name, Vector2 TilePosition)
        {
            return this.spawnOreVein(name, Game1.player.currentLocation, TilePosition);
        }

        /// <summary>
        /// Checks to see if a resource can be spawned here.
        /// </summary>
        /// <param name="OBJ"></param>
        /// <param name="Location"></param>
        /// <param name="TilePosition"></param>
        /// <returns></returns>
        public bool canResourceBeSpawnedHere(MultiTiledObject OBJ, GameLocation Location, Vector2 TilePosition)
        {
            return OBJ.canBePlacedHere(Location, TilePosition) && Location.isTileLocationTotallyClearAndPlaceable(TilePosition);
        }


        //~~~~~~~~~~~~~~~~~~~~~~~//
        //  Mine ore spawn code  //
        //~~~~~~~~~~~~~~~~~~~~~~~//

        #region
        public void spawnOreInMine()
        {
            int floorLevel = LocationUtilities.CurrentMineLevel();
            if (this.hasVisitedFloor(floorLevel))
            {
                //Already has spawned ores for this visit.
                return;
            }
            else
            {
                this.visitedFloors.Add(floorLevel);
            }
            List<OreVeinObj> spawnableOreVeins = new List<OreVeinObj>();
            //Get a list of all of the ores that can spawn on this mine level.
            foreach (KeyValuePair<string, OreVeinObj> pair in this.ores)
            {
                if (pair.Value.resourceInfo.canSpawnAtLocation() && (pair.Value.resourceInfo as OreResourceInformation).canSpawnOnCurrentMineLevel())
                {
                    spawnableOreVeins.Add(pair.Value);
                }
            }

            foreach (OreVeinObj ore in spawnableOreVeins)
            {
                if (ore.resourceInfo.shouldSpawn())
                {
                    int amount = ore.resourceInfo.getNumberOfNodesToSpawn();
                    List<Vector2> openTiles = LocationUtilities.GetOpenObjectTiles(Game1.player.currentLocation, (OreVeinObj)ore.getOne());
                    amount = Math.Min(amount, openTiles.Count); //Only spawn for as many open tiles or the amount of nodes to spawn.
                    for (int i = 0; i < amount; i++)
                    {
                        int position = Game1.random.Next(openTiles.Count);
                        bool didSpawn = this.spawnOreVein(ore.info.id, openTiles[position]);
                        if (didSpawn == false)
                        {
                            i--; //If the tile didn't spawn due to some odd reason ensure that the amount is spawned.
                            openTiles.Remove(openTiles[position]);
                        }
                        else
                        {
                            openTiles.Remove(openTiles[position]); //Remove that tile from the list of open tiles.
                        }
                    }
                }
                else
                {
                    //Ore doesn't meet spawn chance.
                }
                //ModCore.log("Spawned :" + amount + " pancake test ores!");
            }

        }

        /// <summary>
        /// Checks to see if the player has visited the given floor.
        /// </summary>
        /// <param name="Floor"></param>
        /// <returns></returns>
        public bool hasVisitedFloor(int Floor)
        {
            return this.visitedFloors.Contains(Floor);
        }

        /// <summary>
        /// Source: SDV. 
        /// </summary>
        /// <param name="tileX"></param>
        /// <param name="tileY"></param>
        /// <returns></returns>
        private bool isTileOpenForQuarryStone(int tileX, int tileY)
        {
            GameLocation loc = Game1.getLocationFromName("Mountain");
            if (loc.doesTileHaveProperty(tileX, tileY, "Diggable", "Back") != null)
                return loc.isTileLocationTotallyClearAndPlaceable(new Vector2((float)tileX, (float)tileY));
            return false;
        }

        /// <summary>
        /// Update the quarry every day with new ores to spawn.
        /// </summary>
        private void quarryDayUpdate()
        {
            List<OreVeinObj> spawnableOreVeins = new List<OreVeinObj>();
            //Get a list of all of the ores that can spawn on this mine level.
            foreach (KeyValuePair<string, OreVeinObj> pair in this.ores)
            {
                if ((pair.Value.resourceInfo as OreResourceInformation).spawnsInQuarry)
                {
                    spawnableOreVeins.Add(pair.Value);
                    //ModCore.log("Found an ore that spawns in the quarry");
                }
            }
            foreach (OreVeinObj ore in spawnableOreVeins)
            {
                if (ore.resourceInfo.shouldSpawn())
                {
                    int amount = ore.resourceInfo.getNumberOfNodesToSpawn();
                    List<Vector2> openTiles = this.getOpenQuarryTiles(ore);
                    //ModCore.log("Number of open tiles is: " + openTiles.Count);
                    amount = Math.Min(amount, openTiles.Count); //Only spawn for as many open tiles or the amount of nodes to spawn.
                    for (int i = 0; i < amount; i++)
                    {
                        int position = Game1.random.Next(openTiles.Count);
                        bool didSpawn = this.spawnOreVein(ore.info.id,Game1.getLocationFromName("Mountain"),openTiles[position]);
                        if (didSpawn == false)
                        {
                            i--; //If the tile didn't spawn due to some odd reason ensure that the amount is spawned.
                            openTiles.Remove(openTiles[position]);
                            //amount = Math.Min(amount, openTiles.Count); //Only spawn for as many open tiles or the amount of nodes to spawn.
                        }
                        else
                        {
                            //ModCore.log("Spawned ore in the quarry!");
                            openTiles.Remove(openTiles[position]); //Remove that tile from the list of open tiles.
                        }
                    }
                }
                else
                {
                    //Ore doesn't meet spawn chance.
                }
                //ModCore.log("Spawned :" + amount + " pancake test ores!");
            }

        }

        /// <summary>
        /// Gets a list of all of the open quarry tiles.
        /// </summary>
        /// <returns></returns>
        private List<Vector2> getOpenQuarryTiles(MultiTiledObject obj)
        {
            List<Vector2> tiles = new List<Vector2>();
            Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(106, 13, 21, 21);
            for(int i = r.X; i <= r.X + r.Width; i++)
            {
                for(int j=r.Y;j<= r.Y + r.Height; j++)
                {
                    if (this.isTileOpenForQuarryStone(i, j) && this.canResourceBeSpawnedHere(obj,Game1.getLocationFromName("Mountain"),new Vector2(i,j)))
                    {
                        tiles.Add(new Vector2(i, j));
                    }
                }
            }
            if (tiles.Count == 0)
            {
                //ModCore.log("Quarry is full! Can't spawn more resources!");
            }
            return tiles;
        }

        #endregion



        /// <summary>
        /// What happens when the player warps maps.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="playerWarped"></param>
        public void OnPlayerLocationChanged(object o, EventArgs playerWarped)
        {
            this.spawnOreInMine();
            if (LocationUtilities.IsPlayerInMine() == false && LocationUtilities.IsPlayerInSkullCave() == false && LocationUtilities.IsPlayerInMineEnterance() == false)
            {
                this.visitedFloors.Clear();
            }
        }

        public void DailyResourceSpawn(object o, EventArgs NewDay)
        {
            this.quarryDayUpdate();
        }


    }
}
