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



        public List<IntRange> floorsToSpawnOn;
        /// <summary>
        /// The list of floors to exclude spawning on in the mine.
        /// </summary>
        public List<IntRange> floorsToExclude;

        /// <summary>
        /// 
        /// </summary>
        public List<string> minesToSpawnIn;

        public bool spawnInRegularMine;
        public bool spawnInSkullCavern;
        public bool spawnsOnFarm;
        public bool spawnsInQuarry;

        public OreResourceInformation() : base()
        {

        }

        public OreResourceInformation(Item I, int MinDropAmount, int MaxDropAmount, int MinNumberOfNodes, int MaxNumberOfNodes,bool SpawnsOnFarm, bool SpawnsInQuarry,List<IntRange> FloorsToSpawnOn, List<IntRange> FloorsToExclude,bool SpawnInRegularMine,bool SpawnInSkullCave,float SpawnLuckFactor=0f,float DropLuckFactor=0f) : base(I, MinDropAmount, MaxDropAmount, MinNumberOfNodes, MaxNumberOfNodes,SpawnLuckFactor,DropLuckFactor)
        {
            this.spawnsOnFarm = SpawnsOnFarm;
            this.spawnsInQuarry = SpawnsInQuarry;
            this.floorsToSpawnOn = FloorsToSpawnOn;
            this.floorsToExclude = FloorsToExclude!=null? FloorsToExclude: new List<IntRange>();
            this.spawnInRegularMine = SpawnInRegularMine;
            this.spawnInSkullCavern = SpawnInSkullCave;
        }

        public override int getNumberOfDropsToSpawn(bool limitToMax = true)
        {
            return base.getNumberOfDropsToSpawn(limitToMax);
        }

        public override int getNumberOfNodesToSpawn(bool limitToMax = true)
        {
            return base.getNumberOfNodesToSpawn(limitToMax);
        }

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
