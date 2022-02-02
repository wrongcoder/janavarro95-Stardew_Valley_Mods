using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace StardustCore.Events.Preconditions
{
    public class LocationPrecondition:EventPrecondition
    {
        public string locationName;


        public LocationPrecondition()
        {

        }

        public LocationPrecondition(GameLocation Location)
        {
            this.locationName = Location.NameOrUniqueName;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Location">The name of the location.</param>
        /// <param name="IsStructure">The location is a building on the farm.</param>
        public LocationPrecondition(string Location, bool IsStructure=false)
        {
            this.locationName = Location;
        }

        public override bool meetsCondition()
        {
            return Game1.player.currentLocation == Game1.getLocationFromName(this.locationName);
        }

        public override string ToString()
        {
            return "StardustCore.Events.Preconditions.GameLocationPrecondition " + this.locationName;
        }
    }
}
