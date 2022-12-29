using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.Ids.Buildings;
using Omegasis.Revitalize.Framework.World.GameLocations;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewValley;
using StardewValley.Buildings;

namespace Omegasis.Revitalize.Framework.World.Buildings.Structures
{
    [XmlType("Mods_Revitalize.Buildings.ExtraCellarBuilding")]
    public class ExtraCellarBuilding : Building
    {
        private static readonly BluePrint Blueprint = new(BuildingIds.ExtraCellar);

        public ExtraCellarBuilding()
            : base(ExtraCellarBuilding.Blueprint, Vector2.Zero) { }

        protected override GameLocation getIndoors(string nameOfIndoorsWithoutUnique)
        {
            return new ExtraCellarLocation();
        }



        public override bool doAction(Vector2 tileLocation, Farmer who)
        {
            if (this.isInteractingWithBuilding(tileLocation, who))
            {
                who.currentLocation.playSoundAt("doorClose", tileLocation);
                SoundUtilities.PlaySoundAt(Enums.StardewSound.doorClose, this.indoors.Value, Game1.player.getTileLocation());
                bool isStructure = this.indoors.Value != null;
                Game1.warpFarmer(this.indoors.Value.NameOrUniqueName, this.indoors.Value.warps[0].X, this.indoors.Value.warps[0].Y + 1, (int)Enums.Direction.Down, isStructure);
                return true;
            }
            return false;
        }

        /*
        public override void performToolAction(Tool t, int tileX, int tileY)
        {
            base.performToolAction(t, tileX, tileY);
        }
        */

        public virtual bool isInteractingWithBuilding(Vector2 tileLocation, Farmer who)
        {
            Rectangle rect = new Rectangle(this.tileX.Value, this.tileY.Value, this.tilesWide.Value, this.tilesHigh.Value);
            return rect.Contains(tileLocation);
        }

        public override void updateInteriorWarps(GameLocation interior = null)
        {
            base.updateInteriorWarps(interior);
        }
    }
}
