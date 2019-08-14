using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Utilities;
using StardewValley;

namespace Revitalize.Framework.Objects.InformationFiles
{
    public class OreResourceInformation:ResourceInformaton
    {


        /// <summary>
        /// The floors of the mine that this resource should spawn in.
        /// </summary>
        public List<IntRange> floorsToSpawnOn;
        /// <summary>
        /// The list of floors to exclude spawning on in the mine.
        /// </summary>
        public List<IntRange> floorsToExclude;

        /// <summary>
        /// Should this resource spawn in the mine in the mountains?
        /// </summary>
        public bool spawnInRegularMine;
        /// <summary>
        /// Should this resource spawn in Skull Cavern?
        /// </summary>
        public bool spawnInSkullCavern;
        /// <summary>
        /// Should this resource spawn on farms. Notably the hiltop farm?
        /// </summary>
        public bool spawnsOnFarm;
        /// <summary>
        /// Should this resource spawn in the quarry?
        /// </summary>
        public bool spawnsInQuarry;


        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public OreResourceInformation() : base()
        {

        }

        public OreResourceInformation(Item I,bool SpawnsOnFarm, bool SpawnsInQuarry, bool SpawnInRegularMine, bool SpawnInSkullCave,List<IntRange> FloorsToSpawnOn,List<IntRange>FloorsToExclude ,int MinDropAmount, int MaxDropAmount, int MinNumberOfNodes, int MaxNumberOfNodes, double ChanceToSpawn = 1f, double ChanceToDrop = 1f, double SpawnChanceLuckFactor = 0f, double SpawnAmountLuckFactor = 0f, double DropChanceLuckFactor = 0f, double DropAmountLuckFactor = 0f) : base(I, MinDropAmount, MaxDropAmount, MinNumberOfNodes, MaxNumberOfNodes,ChanceToSpawn,ChanceToDrop,SpawnChanceLuckFactor,SpawnAmountLuckFactor,DropChanceLuckFactor,DropAmountLuckFactor)
        {
            this.spawnsOnFarm = SpawnsOnFarm;
            this.spawnsInQuarry = SpawnsInQuarry;
            this.floorsToSpawnOn = FloorsToSpawnOn;
            this.floorsToExclude = FloorsToExclude!=null? FloorsToExclude: new List<IntRange>();
            this.spawnInRegularMine = SpawnInRegularMine;
            this.spawnInSkullCavern = SpawnInSkullCave;
        }

        /// <summary>
        /// Gets the number of drops that should spawn for this ore.
        /// </summary>
        /// <param name="limitToMax"></param>
        /// <returns></returns>
        public override int getNumberOfDropsToSpawn(bool limitToMax = true)
        {
            return base.getNumberOfDropsToSpawn(limitToMax);
        }

        /// <summary>
        /// Gets the number of nodes that should spawn for this ore.
        /// </summary>
        /// <param name="limitToMax"></param>
        /// <returns></returns>
        public override int getNumberOfNodesToSpawn(bool limitToMax = true)
        {
            return base.getNumberOfNodesToSpawn(limitToMax);
        }

        /// <summary>
        /// Can this ore spawn at the given location?
        /// </summary>
        /// <returns></returns>
        public override bool canSpawnAtLocation()
        {
            if (this.spawnsOnFarm && Game1.player.currentLocation is StardewValley.Farm)
            {
                return true;
            }
            if (this.spawnsInQuarry && Game1.player.currentLocation is StardewValley.Locations.Mountain)
            {
                return true;
            }
            if (this.spawnInRegularMine && LocationUtilities.IsPlayerInMine())
            {
                return true;
            }
            if (this.spawnInSkullCavern && LocationUtilities.IsPlayerInSkullCave())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the resource can spawn at the given game location.
        /// </summary>
        /// <param name="Location"></param>
        /// <returns></returns>
        public override bool canSpawnAtLocation(GameLocation Location)
        {
            if(this.spawnsOnFarm && Location is StardewValley.Farm)
            {
                return true;
            }
            if(this.spawnsInQuarry && Location is StardewValley.Locations.Mountain)
            {
                return true;
            }
            if(this.spawnInRegularMine && LocationUtilities.IsPlayerInMine())
            {
                return true;
            }
            if(this.spawnInSkullCavern && LocationUtilities.IsPlayerInSkullCave())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks to see if this ore can be spawned on the current mine level.
        /// </summary>
        /// <returns></returns>
        public bool canSpawnOnCurrentMineLevel()
        {
            int level=LocationUtilities.CurrentMineLevel();
            foreach(IntRange range in this.floorsToSpawnOn)
            {
                if (range.ContainsInclusive(level))
                {
                    foreach(IntRange exclude in this.floorsToExclude)
                    {
                        if (exclude.ContainsInclusive(level)) return false;
                    }
                    return true;
                }
            }
            return false;
        }
        
    }
}
