using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Netcode;
using Omegasis.Revitalize.Framework.Constants.Ids.Buildings;
using Omegasis.Revitalize.Framework.Menus;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;

namespace Omegasis.Revitalize.Framework.World.Buildings
{
    /// <summary>
    /// TODO: New menu for dimensional storage unit where it can manage upgrades. Maybe recycle happy birthday one? 
    /// TODO: Need better inventory management here: Capacity, upgrading it etc.
    /// TODO: Need Automate Compatibility
    /// </summary>
    [XmlType("Mods_Revitalize.Buildings.DimensionalStorageUnitBuilding")]
    public class DimensionalStorageUnitBuilding : CustomBuilding
    {
        /// <summary>
        /// Used for optimizations.
        /// </summary>
        public static DimensionalStorageUnitBuilding CachedDimensionalStorageUnitBuilding;

        public static NetObjectList<Item> UniversalItems
        {
            get
            {
                DimensionalStorageUnitBuilding b = GetDimensionalStorageUnitBuilding();
                if (b != null)
                {
                    return b.items;
                }
                return new NetObjectList<Item>();
            }
        }

        private static readonly BluePrint Blueprint = new(BuildingIds.DimensionalStorageUnit);


        public NetObjectList<Item> items = new NetObjectList<Item>();

        public DimensionalStorageUnitBuilding()
    : base(Blueprint, Vector2.Zero) { }

        protected override void initNetFields()
        {
            base.initNetFields();
            this.NetFields.AddFields(this.items);
        }

        public override bool doAction(Vector2 tileLocation, Farmer who)
        {
            if (this.isInteractingWithBuilding(tileLocation, who))
            {
                //TODO: Make a different menu for this too.
                Game1.activeClickableMenu = new InventoryTransferMenu(0,0,400,400,this.items,36);
                return true;
            }
            return false;
            
        }

        public override bool performActiveObjectDropInAction(Farmer who, bool probe)
        {
            //Maybe use void essences here to upgrade the capacity?

            return base.performActiveObjectDropInAction(who, probe);
        }

        public override void BeforeDemolish()
        {
            BuildableGameLocation loc = GetDimensionalStorageUnitBuildingGameLocation();
            foreach (Item i in this.items)
            {
                loc.debris.Add(new StardewValley.Debris(i, new Vector2(this.tileX * Game1.tileSize, this.tileY * Game1.tileSize)));
            }
            this.items.Clear();
        }

        public static DimensionalStorageUnitBuilding GetDimensionalStorageUnitBuilding()
        {
            if (CachedDimensionalStorageUnitBuilding != null)
            {
                return CachedDimensionalStorageUnitBuilding;
            }

            foreach (BuildableGameLocation loc in Game1.locations)
            {
                foreach (Building b in loc.buildings)
                {
                    if (b is DimensionalStorageUnitBuilding)
                    {
                        CachedDimensionalStorageUnitBuilding = (b as DimensionalStorageUnitBuilding);
                        return (b as DimensionalStorageUnitBuilding);
                    }
                }
            }
            return null;
        }

        public static BuildableGameLocation GetDimensionalStorageUnitBuildingGameLocation()
        {
            foreach (BuildableGameLocation loc in Game1.locations)
            {
                foreach (Building b in loc.buildings)
                {
                    if (b is DimensionalStorageUnitBuilding)
                    {
                        CachedDimensionalStorageUnitBuilding = (b as DimensionalStorageUnitBuilding);
                        return loc;
                    }
                }
            }
            return null;
        }
    }
}
