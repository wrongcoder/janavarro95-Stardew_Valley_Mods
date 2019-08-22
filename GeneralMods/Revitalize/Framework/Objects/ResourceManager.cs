using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Revitalize.Framework.Factories.Objects.Resources;
using Revitalize.Framework.Objects.InformationFiles;
using Revitalize.Framework.Objects.Items.Resources;
using Revitalize.Framework.Objects.Resources.OreVeins;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.Animations;
using StardustCore.UIUtilities;

namespace Revitalize.Framework.Objects
{
    public class ResourceManager
    {

        private string oreResourceDataPath= Path.Combine("Data", "Objects", "Resources","Ore");

        /// <summary>
        /// A static reference to the resource manager for quicker access.
        /// </summary>
        public static ResourceManager self;

        /// <summary>
        /// A list of all of the ores held by the resource manager.
        /// </summary>
        public Dictionary<string, OreVeinObj> oreVeins;
        public Dictionary<string, OreResourceInformation> oreResourceInformationTable;
        public Dictionary<string, Ore> ores;

        /// <summary>
        /// A list of all visited floors on the current visit to the mines.
        /// </summary>
        public List<int> visitedFloors;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ResourceManager()
        {
            self = this;
            this.oreVeins = new Dictionary<string, OreVeinObj>();
            this.oreResourceInformationTable = new Dictionary<string, OreResourceInformation>();
            this.ores = new Dictionary<string, Ore>();
            this.visitedFloors = new List<int>();

            this.loadInOreItems();
            this.serializeOreVeins();
            this.loadOreVeins();

        }

        /// <summary>
        /// Loads in all of the ore veins for the game.
        /// </summary>
        private void loadOreVeins()
        {
            //The pancake ore.

            /*
            OreVeinObj testOre = new OreVeinObj(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Resources.Ore.Test", TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), typeof(OreVeinTile), Color.White), new BasicItemInformation("Test Ore Vein", "Omegasis.Revitalize.Resources.Ore.Test", "A ore vein that is used for testing purposes.", "Revitalize.Ore", Color.Black, -300, 0, false, 350, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Resources.Ore", "Test"), new Animation(0, 0, 16, 16)), Color.White, false, null, null));
            testOre.addComponent(new Vector2(0, 0), new OreVeinTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Resources.Ore.Test", TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), typeof(OreVeinTile), Color.White), new BasicItemInformation("Test Ore Vein", "Omegasis.Revitalize.Resources.Ore.Test", "A ore vein that is used for testing purposes.", "Revitalize.Ore", Color.Black, -300, 0, false, 350, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Resources.Ore", "Test"), new Animation(0, 0, 16, 16)), Color.White, false, null, null),
                new InformationFiles.OreResourceInformation(new StardewValley.Object(211, 1), true, true, true, false, new List<IntRange>()
            {
                new IntRange(1,9)
            }, new List<IntRange>(), (i => i == 1), (i => i % 10 == 0), 1, 5, 1, 10, new IntRange(1, 3), new IntRange(1, 3), new IntRange(0, 0), new List<IntRange>()
            {
                new IntRange(0,0)
            }, new List<IntRange>()
            {
                new IntRange(0,9999)
            }, (i => i == -1), (i => i == -1), 1d, 1.0d, 0.10d, 1d, 1d, 0, 0, 0, 0), new List<ResourceInformaton>(),4));

            this.ores.Add("Omegasis.Revitalize.Resources.Ore.Test", testOre);
            */

            if (!Directory.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", this.oreResourceDataPath))) Directory.CreateDirectory(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", this.oreResourceDataPath));
            List<string> directories = Directory.GetDirectories(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", this.oreResourceDataPath)).ToList();
            directories.Add(Path.Combine(ModCore.ModHelper.DirectoryPath, "Content", this.oreResourceDataPath));
            foreach (string directory in directories)
            {
                string[] files = Directory.GetFiles(directory);

                Dictionary<string, OreVeinObj> objs = new Dictionary<string, OreVeinObj>();

                //Deserialize container.
                foreach (string file in files)
                {
                    if ((Path.GetFileName(file)).Contains("_") == true) continue;
                    else
                    {
                        OreFactoryInfo factoryInfo = ModCore.Serializer.DeserializeContentFile<OreFactoryInfo>(file);
                        objs.Add(Path.GetFileNameWithoutExtension(file), new OreVeinObj(factoryInfo.PyTkData, factoryInfo.info));
                    }
                }
                //Deseralize components
                foreach (string file in files)
                {
                    if ((Path.GetFileName(file)).Contains("_") == false) continue;
                    else
                    {

                        string[] splits = Path.GetFileNameWithoutExtension(file).Split('_');
                        string name = splits[0];
                        Vector2 offset = new Vector2(Convert.ToInt32(splits[1]), Convert.ToInt32(splits[2]));
                        OreFactoryInfo info = ModCore.Serializer.DeserializeContentFile<OreFactoryInfo>(file);

                        OreVeinTile orePiece = new OreVeinTile(info.PyTkData, info.info,info.OreSpawnInfo,info.ExtraDrops,info.Health);
                        objs[name].addComponent(offset, orePiece);
                    }
                }
                foreach (var v in objs)
                {
                    this.oreVeins.Add(v.Value.info.id, v.Value);
                    //ModCore.ObjectManager.lamps.Add(v.Value.info.id, v.Value);
                }
            }

        }

        /// <summary>
        /// Serializes an example ore to eb
        /// </summary>
        private void serializeOreVeins() {
            OreVeinObj testOre = new OreVeinObj(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Resources.Ore.Test", TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), typeof(OreVeinTile), Color.White), new BasicItemInformation("Test Ore Vein", "Omegasis.Revitalize.Resources.Ore.Test", "A ore vein that is used for testing purposes.", "Revitalize.Ore", Color.Black, -300, 0, false, 350, true, true, TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Resources.Ore", "Test"), new Animation(0, 0, 16, 16)), Color.White, false, null, null));
            OreVeinTile testOre_0_0= new OreVeinTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Resources.Ore.Test", TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), typeof(OreVeinTile), Color.White), new BasicItemInformation("Test Ore Vein", "Omegasis.Revitalize.Resources.Ore.Test", "A ore vein that is used for testing purposes.", "Revitalize.Ore", Color.Black, -300, 0, false, 350, true, true, TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Resources.Ore", "Test"), new Animation(0, 0, 16, 16)), Color.White, false, null, null),
                new InformationFiles.OreResourceInformation(this.getOre("Tin"), true, true, true, false, new List<IntRange>()
            {
                new IntRange(1,9)
            }, new List<IntRange>(), (i => i == 1), (i => i % 10 == 0), 1, 5, 1, 10, new IntRange(1, 3), new IntRange(1, 3), new IntRange(0, 0), new List<IntRange>()
            {
                new IntRange(0,0)
            }, new List<IntRange>()
            {
                new IntRange(0,9999)
            }, null,null, 1d, 1.0d, 0.10d, 1d, 1d, 0, 0, 0, 0), new List<ResourceInformaton>(), 4);

            OreFactoryInfo testOre_0_0_file = new OreFactoryInfo(testOre_0_0);
            OreFactoryInfo testOre_file = new OreFactoryInfo(testOre);

            ModCore.Serializer.SerializeContentFile("TestOre_0_0", testOre_0_0_file,Path.Combine(this.oreResourceDataPath,"TestOre"));
            ModCore.Serializer.SerializeContentFile("TestOre", testOre_file, Path.Combine(this.oreResourceDataPath, "TestOre"));

        }

        /// <summary>
        /// Loads in all of the ore items into the game.
        /// </summary>
        private void loadInOreItems()
        {
            Ore tinOre = new Ore(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Items.Resources.Ore.TinOre", TextureManager.GetTexture(ModCore.Manifest, "Items.Resources.Ore", "TinOre"), typeof(Ore), Color.White, true), new BasicItemInformation("Tin Ore", "Omegasis.Revitalize.Items.Resources.Ore.TinOre", "Tin ore that can be smelted into tin ingots for further use.", "Ore", Color.Silver, -300, 0, false, 85, false, false, TextureManager.GetTexture(ModCore.Manifest, "Items.Resources.Ore", "TinOre"), new AnimationManager(), Color.White, true, null, null), 1);
            this.ores.Add("Tin", tinOre);
        }

        /// <summary>
        /// Gets an ore from the list of stored ores in this mod.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Stack"></param>
        /// <returns></returns>
        public Ore getOre(string name,int Stack=1)
        {
            if (this.ores.ContainsKey(name))
            {
                Ore o = (Ore)this.ores[name].getOne();
                o.Stack = Stack;
                return o;
            }
            else
            {
                return null;
            }
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
        //  World Ore Spawn Code //
        //~~~~~~~~~~~~~~~~~~~~~~~//

        #region


        /// <summary>
        /// Spawns an ore vein at the given location if possible.
        /// </summary>
        /// <param name="name"></param>
        public bool spawnOreVein(string name, GameLocation Location, Vector2 TilePosition)
        {
            if (this.oreVeins.ContainsKey(name))
            {
                OreVeinObj spawn;
                this.oreVeins.TryGetValue(name, out spawn);
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
        /// Spawns ore in the mine depending on a lot of given variables such as floor level and spawn chance.
        /// </summary>
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
            foreach (KeyValuePair<string, OreVeinObj> pair in this.oreVeins)
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
            foreach (KeyValuePair<string, OreVeinObj> pair in this.oreVeins)
            {
                if ((pair.Value.resourceInfo as OreResourceInformation).spawnsInQuarry)
                {
                    spawnableOreVeins.Add(pair.Value);
                    //ModCore.log("Found an ore that spawns in the quarry");
                }
            }
            foreach (OreVeinObj ore in spawnableOreVeins)
            {
                if ((ore.resourceInfo as OreResourceInformation).shouldSpawnInQuarry())
                {
                    int amount = (ore.resourceInfo as OreResourceInformation).getNumberOfNodesToSpawnQuarry();
                    List<Vector2> openTiles = this.getOpenQuarryTiles(ore);
                    amount = Math.Min(amount, openTiles.Count); //Only spawn for as many open tiles or the amount of nodes to spawn.
                    for (int i = 0; i < amount; i++)
                    {
                        int position = Game1.random.Next(openTiles.Count);
                        bool didSpawn = this.spawnOreVein(ore.info.id, Game1.getLocationFromName("Mountain"), openTiles[position]);
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
            }

        }

        /// <summary>
        /// Spawns ore in the mountain farm every day.
        /// </summary>
        public void mountainFarmDayUpdate()
        {
            if (LocationUtilities.Farm_IsFarmHiltopFarm() == false)
            {
                ModCore.log("Farm is not hiltop farm!");
                return;
            }
            GameLocation farm = Game1.getFarm();

            List<OreVeinObj> spawnableOreVeins = new List<OreVeinObj>();
            //Get a list of all of the ores that can spawn on this mine level.
            foreach (KeyValuePair<string, OreVeinObj> pair in this.oreVeins)
            {
                if ((pair.Value.resourceInfo as OreResourceInformation).spawnsOnFarm)
                {
                    spawnableOreVeins.Add(pair.Value);
                    ModCore.log("Found an ore that spawns on the farm!");
                }
            }
            foreach (OreVeinObj ore in spawnableOreVeins)
            {
                if ((ore.resourceInfo as OreResourceInformation).shouldSpawnOnFarm())
                {
                    int amount = (ore.resourceInfo as OreResourceInformation).getNumberOfNodesToSpawnFarm();
                    List<Vector2> openTiles = this.getFarmQuarryOpenTiles(ore);
                    if (openTiles.Count == 0)
                    {
                        ModCore.log("No open farm tiles!");
                    }
                    amount = Math.Min(amount, openTiles.Count); //Only spawn for as many open tiles or the amount of nodes to spawn.
                    for (int i = 0; i < amount; i++)
                    {
                        int position = Game1.random.Next(openTiles.Count);
                        bool didSpawn = this.spawnOreVein(ore.info.id, farm, openTiles[position]);
                        if (didSpawn == false)
                        {
                            i--; //If the tile didn't spawn due to some odd reason ensure that the amount is spawned.
                            openTiles.Remove(openTiles[position]);
                            //amount = Math.Min(amount, openTiles.Count); //Only spawn for as many open tiles or the amount of nodes to spawn.
                            ModCore.log("Did not spawn ore in the farm quarry!");
                        }
                        else
                        {
                            ModCore.log("Spawned ore in the farm quarry!");
                            openTiles.Remove(openTiles[position]); //Remove that tile from the list of open tiles.
                        }
                    }
                }
                else
                {
                    //Ore doesn't meet spawn chance.
                }
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
            for (int i = r.X; i <= r.X + r.Width; i++)
            {
                for (int j = r.Y; j <= r.Y + r.Height; j++)
                {
                    if (this.isTileOpenForQuarryStone(i, j) && this.canResourceBeSpawnedHere(obj, Game1.getLocationFromName("Mountain"), new Vector2(i, j)))
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

        /// <summary>
        /// Gets all of the open tiles in the farm quarry.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private List<Vector2> getFarmQuarryOpenTiles(MultiTiledObject obj)
        {
            List<Vector2> tiles = new List<Vector2>();
            Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(5, 37, 22, 8);
            GameLocation farm = Game1.getFarm();
            for (int i = r.X; i <= r.X + r.Width; i++)
            {
                for (int j = r.Y; j <= r.Y + r.Height; j++)
                {
                    Vector2 pos = new Vector2(i, j);
                    if (farm.doesTileHavePropertyNoNull((int)pos.X, (int)pos.Y, "Type", "Back").Equals("Dirt") && this.canResourceBeSpawnedHere(obj, farm, new Vector2(i, j)))
                    {
                        tiles.Add(pos);
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


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //          SMAPI Events       //
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        #region
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

        /// <summary>
        /// Triggers at the start of every new day to populate the world full of ores.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="NewDay"></param>
        public void DailyResourceSpawn(object o, EventArgs NewDay)
        {
            this.mountainFarmDayUpdate();
            this.quarryDayUpdate();
        }
        #endregion

    }
}
