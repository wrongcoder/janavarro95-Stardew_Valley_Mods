using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netcode;
using Revitalize.Framework.World.Objects.Items;
using StardewValley;

namespace Revitalize.Framework.Objects.InformationFiles
{
    /// <summary>
    /// Deals with information reguarding resources.
    /// </summary>
    public class ResourceInformation
    {
        /// <summary>
        /// The item to drop.
        /// </summary>
        public ItemReference droppedItem;

        /// <summary>
        /// The min amount of resources to drop given the getNumberOfDrops function.
        /// </summary>
        public readonly NetInt minResourcePerDrop = new NetInt();
        /// <summary>
        /// The max amount of resources to drop given the getNumberOfDrops function.
        /// </summary>
        public readonly NetInt maxResourcePerDrop = new NetInt();
        /// <summary>
        /// The min amount of nodes that would be spawned given the getNumberOfNodesToSpawn function.
        /// </summary>
        public readonly NetInt minNumberOfNodesSpawned = new NetInt();
        /// <summary>
        /// The max amount of nodes that would be spawned given the getNumberOfNodesToSpawn function.
        /// </summary>
        public readonly NetInt maxNumberOfNodesSpawned = new NetInt();
        /// <summary>
        /// The influence multiplier that luck has on how many nodes of this resource spawn.
        /// </summary>
        public readonly NetDouble spawnAmountLuckFactor = new NetDouble();
        /// <summary>
        /// The influence multiplier that luck has on ensuring the resource spawns.
        /// </summary>
        public readonly NetDouble spawnChanceLuckFactor = new NetDouble();

        public readonly NetDouble dropAmountLuckFactor = new NetDouble();
        /// <summary>
        /// The influence multiplier that luck has on ensuring the resource drops.
        /// </summary>
        public readonly NetDouble dropChanceLuckFactor = new NetDouble();

        /// <summary>
        /// The chance for the resource to spawn from 0.0 ~ 1.0
        /// </summary>
        public readonly NetDouble chanceToSpawn = new NetDouble();
        /// <summary>
        /// The chance for the resource to drop from 0.0 ~ 1.0
        /// </summary>
        public readonly NetDouble chanceToDrop = new NetDouble();

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public ResourceInformation()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ItemReference">The reference to the item to get from this resource.</param>
        /// <param name="MinDropAmount"></param>
        /// <param name="MaxDropAmount"></param>
        /// <param name="MinNumberOfNodes"></param>
        /// <param name="MaxNumberOfNodes"></param>
        /// <param name="ChanceToSpawn"></param>
        /// <param name="ChanceToDrop"></param>
        /// <param name="SpawnChanceLuckFactor"></param>
        /// <param name="SpawnAmountLuckFactor"></param>
        /// <param name="DropChanceLuckFactor"></param>
        /// <param name="DropAmountLuckFactor"></param>
        public ResourceInformation(ItemReference ItemReference, int MinDropAmount, int MaxDropAmount, int MinNumberOfNodes, int MaxNumberOfNodes,double ChanceToSpawn=1f,double ChanceToDrop=1f, double SpawnChanceLuckFactor = 0f, double SpawnAmountLuckFactor = 0f,double DropChanceLuckFactor=0f, double DropAmountLuckFactor = 0f)
        {
            this.droppedItem = ItemReference;
            this.minResourcePerDrop.Value = MinDropAmount;
            this.maxResourcePerDrop.Value = MaxDropAmount;
            this.minNumberOfNodesSpawned.Value = MinNumberOfNodes;
            this.maxNumberOfNodesSpawned.Value = MaxNumberOfNodes;
            this.spawnAmountLuckFactor.Value = SpawnAmountLuckFactor;
            this.dropAmountLuckFactor.Value = DropAmountLuckFactor;
            this.chanceToSpawn.Value = ChanceToSpawn;
            this.chanceToDrop.Value = ChanceToDrop;
            this.spawnChanceLuckFactor.Value = SpawnChanceLuckFactor;
            this.dropChanceLuckFactor.Value = DropChanceLuckFactor;
        }

        public virtual List<INetSerializable> getNetFields()
        {
            List<INetSerializable> netFields = new List<INetSerializable>()
            {
                this.minResourcePerDrop,
                this.maxResourcePerDrop,
                this.minNumberOfNodesSpawned,
                this.maxNumberOfNodesSpawned,
                this.spawnAmountLuckFactor,
                this.dropAmountLuckFactor,
                this.chanceToSpawn,
                this.chanceToDrop,
                this.spawnChanceLuckFactor,
                this.dropChanceLuckFactor
            };
            netFields.AddRange(this.droppedItem.getNetFields());
            return netFields;
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
                amount = (int)Math.Min(amount + (this.dropAmountLuckFactor * (Game1.player.LuckLevel + Game1.player.addedLuckLevel.Value)), this.maxResourcePerDrop);
            }
            else
            {
                amount = (int)(amount + (this.dropAmountLuckFactor * (Game1.player.LuckLevel + Game1.player.addedLuckLevel.Value)));
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
                amount = (int)Math.Min(amount + (this.spawnAmountLuckFactor * (Game1.player.LuckLevel + Game1.player.addedLuckLevel.Value)), this.maxNumberOfNodesSpawned);
            }
            else
            {
                amount = (int)(amount + (this.spawnAmountLuckFactor * (Game1.player.LuckLevel + Game1.player.addedLuckLevel.Value)));
            }
            return amount;
        }

        /// <summary>
        /// Checks to see if the resource can spawn at the player's location.
        /// </summary>
        /// <returns></returns>
        public virtual bool canSpawnAtLocation()
        {
            return true;
        }
        /// <summary>
        /// Checks to see if the resource can spawn at the given location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public virtual bool canSpawnAtLocation(GameLocation location)
        {
            return true;
        }

        /// <summary>
        /// Checks to see if this resource's spawn chance is greater than the spawn chance it is checked against.
        /// </summary>
        /// <returns></returns>
        public virtual bool shouldSpawn()
        {
            double chance = Game1.random.NextDouble();
            chance = (chance - (this.spawnChanceLuckFactor * (Game1.player.LuckLevel + Game1.player.addedLuckLevel.Value)));
            if (this.chanceToSpawn >= chance) return true;
            else return false;
        }

        /// <summary>
        /// Checks to see if this resource's drop chance is greater than the spawn chance it is checked against.
        /// </summary>
        /// <returns></returns>
        public virtual bool shouldDropResource()
        {
            double chance = Game1.random.NextDouble();
            chance= (chance - (this.dropChanceLuckFactor * (Game1.player.LuckLevel + Game1.player.addedLuckLevel.Value)));

            if (this.chanceToDrop >= chance) return true;
            else return false;
        }

        /// <summary>
        /// Gets an item that should be dropped from this resource with the appropriate drop amount;
        /// </summary>
        /// <returns></returns>
        public Item getItemDrops()
        {
            return this.droppedItem.getItem(this.getNumberOfDropsToSpawn());
        }
    }
}
