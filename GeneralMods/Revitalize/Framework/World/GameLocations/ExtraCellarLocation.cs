using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Omegasis.Revitalize.Framework.Constants.Ids.GameLocations;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;

namespace Omegasis.Revitalize.Framework.World.GameLocations
{
    [XmlType("Mods_Revitalize.GameLocations.Buildings.ExtraCellarBuilding")]
    public class ExtraCellarLocation : Cellar
    {
        public ExtraCellarLocation()
    : base(BuildingAssetLoader.GetMapsAssetName(GameLocationIds.ExtraCellar), GameLocationIds.ExtraCellar)
        {
        }

        /// <summary>
        /// Reset the local state. Since this derives from cellars and that seems to only be for farmhouses and such, we need to fix them again...
        /// </summary>
        protected override void resetLocalState()
        {
            base.resetLocalState();
            this.map.Properties.TryGetValue("Warp", out var warpsUnparsed);

            string targetName = warpsUnparsed.ToString().Split(" ")[2];

            foreach (Warp warp in base.warps)
            {
                warp.TargetName = targetName;
                if (warp.flipFarmer.Value == false)
                {
                    warp.flipFarmer.Value = true;
                }
            }
        }
    }
}
