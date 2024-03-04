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
using StardewValley.Network;
using StardewValley.Objects;

namespace Omegasis.Revitalize.Framework.World.GameLocations
{
    [XmlType("Mods_Omegasis.Revitalize.GameLocations.Buildings.ExtraCellarBuilding")]
    public class ExtraCellarLocation : Cellar
    {
        public ExtraCellarLocation()
    : base(BuildingAssetLoader.GetMapsAssetName(GameLocationIds.ExtraCellar), GameLocationIds.ExtraCellar)
        {
            //Since the original base cellar class adds in more casks, we need to clear this so that the save can load properly. Why? Honestly not sure past the first sentence, but this fixed one of my saves and properly preserved the objects upon creation...
            this.Objects.Clear();
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
