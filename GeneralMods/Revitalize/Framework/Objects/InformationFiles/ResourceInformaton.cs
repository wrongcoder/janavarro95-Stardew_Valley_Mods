using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Revitalize.Framework.Objects.InformationFiles
{
    /// <summary>
    /// Deals with information reguarding resources.
    /// </summary>
    public class ResourceInformaton
    {
        /// <summary>
        /// The item to drop.
        /// </summary>
        public Item droppedItem;

        public int minResourcePerDrop;
        public int maxResourcePerDrop;
        public int minNumberOfNodesSpawned;
        public int maxNumberOfNodesSpawned;
        public float spawnLuckFactor;
        public float dropLuckFactor;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public ResourceInformaton()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="I">The item to drop.</param>
        /// <param name="MinDropAmount">The min amount to drop.</param>
        /// <param name="MaxDropAmount">The max amount to drop.</param>
        public ResourceInformaton(Item I, int MinDropAmount, int MaxDropAmount, int MinNumberOfNodes, int MaxNumberOfNodes, float SpawnLuckFactor = 0f, float DropLuckFactor=0f)
        {
            this.droppedItem = I;
            this.minResourcePerDrop = MinDropAmount;
            this.maxResourcePerDrop = MaxDropAmount;
            this.minNumberOfNodesSpawned = MinNumberOfNodes;
            this.maxNumberOfNodesSpawned = MaxNumberOfNodes;
            this.spawnLuckFactor = SpawnLuckFactor;
            this.dropLuckFactor = DropLuckFactor;
        }


        /// <summary>
        /// Gets the number of drops to spawn for the given resource;
        /// </summary>
        /// <returns></returns>
        public virtual int getNumberOfDropsToSpawn(bool limitToMax = true)
        {
            int amount = Game1.random.Next(this.minResourcePerDrop, this.maxResourcePerDrop + 1);

            if (limitToMax)
            {
                amount = (int)Math.Min(amount + (this.dropLuckFactor * (Game1.player.LuckLevel + Game1.player.addedLuckLevel.Value)), this.maxResourcePerDrop);
            }
            else
            {
                amount = (int)(amount + (this.dropLuckFactor * (Game1.player.LuckLevel + Game1.player.addedLuckLevel.Value)));
            }
            return amount;
        }

        /// <summary>
        /// Gets the number of resource nodes to spawn when spawning multiple clusters.
        /// </summary>
        /// <returns></returns>
        public virtual int getNumberOfNodesToSpawn(bool limitToMax = true)
        {
            int amount = Game1.random.Next(this.minNumberOfNodesSpawned, this.maxNumberOfNodesSpawned + 1);
            if (limitToMax)
            {
                amount = (int)Math.Min(amount + (this.spawnLuckFactor * (Game1.player.LuckLevel + Game1.player.addedLuckLevel.Value)), this.maxNumberOfNodesSpawned);
            }
            else
            {
                amount = (int)(amount + (this.spawnLuckFactor * (Game1.player.LuckLevel + Game1.player.addedLuckLevel.Value)));
            }
            return amount;
        }

        public virtual bool canSpawnAtLocation()
        {
            return true;
        }
        public virtual bool canSpawnAtLocation(GameLocation location)
        {
            return true;
        }
    }
}
