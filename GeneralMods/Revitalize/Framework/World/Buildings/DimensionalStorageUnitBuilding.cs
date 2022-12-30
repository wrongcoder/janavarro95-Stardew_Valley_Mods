using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.Ids.Buildings;
using Omegasis.Revitalize.Framework.HUD;
using Omegasis.Revitalize.Framework.Menus;
using Omegasis.Revitalize.Framework.Menus.Items;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
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

        public static int DimensionalStorageUnitMaxItems = 9;

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

                if (who.ActiveObject != null)
                {
                    SoundUtilities.PlaySoundAt(Enums.StardewSound.throwDownITem, GetDimensionalStorageUnitBuildingGameLocation(), new Vector2(this.tileX + this.tilesWide / 2, this.tileY + this.tilesHigh / 2));
                    AddItemToDimensionalStorageUnit(who.ActiveObject);
                    who.removeItemFromInventory(who.ActiveObject);
                    return true;
                }

                SoundUtilities.PlaySound(Constants.Enums.StardewSound.qi_shop);
                Game1.activeClickableMenu = new DimensionalStorageUnitMenu();
                //Game1.activeClickableMenu = new InventoryTransferMenu(0,0,400,400,this.items,36);
                return true;
            }
            return false;

        }

        public override void BeforeDemolish()
        {
            BuildableGameLocation loc = GetDimensionalStorageUnitBuildingGameLocation();
            foreach (Item i in this.items)
            {
                //building middle is about 40 pixels.
                int randX = Game1.random.Next(40, this.tilesWide * Game1.tileSize + 1 - 40);
                int randY = Game1.random.Next(40, this.tilesHigh * Game1.tileSize + 1 - 40);
                Game1.createItemDebris(i, new Vector2(this.tileX * Game1.tileSize + randX, this.tileY * Game1.tileSize + randY), Game1.random.Next(1, 5), loc);
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

        public static bool AddItemToDimensionalStorageUnit(Item item)
        {
            for (int i = 0; i < UniversalItems.Count; i++)
            {
                //Check to see if the items can stack. If they can simply add them together and then continue on.
                if (UniversalItems[i] != null && UniversalItems[i].canStackWith(item))
                {
                    UniversalItems[i].Stack += item.Stack;
                    return true;
                }
            }

            if (UniversalItems.Count < DimensionalStorageUnitMaxItems)
            {
                UniversalItems.Add(item);
                return true;
            }
            else
            {
                HudUtilities.ShowInventoryFullErrorMessage();
            }
            return false;
        }



    }
}
