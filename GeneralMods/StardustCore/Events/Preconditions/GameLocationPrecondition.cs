using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.StardustCore.Events.Preconditions
{
    public class GameLocationPrecondition:EventPrecondition
    {
        public const string EventPreconditionId = "StardustCore.Events.Preconditions.GameLocationPrecondition";

        public string locationName;


        public GameLocationPrecondition()
        {

        }

        public GameLocationPrecondition(GameLocation Location)
        {
            this.locationName = Location.NameOrUniqueName;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Location">The name of the location.</param>
        /// <param name="IsStructure">The location is a building on the farm.</param>
        public GameLocationPrecondition(string Location, bool IsStructure=false)
        {
            this.locationName = Location;
        }

        public override bool meetsCondition()
        {
            return Game1.player.currentLocation == Game1.getLocationFromName(this.locationName);
        }

        public override string ToString()
        {
            return EventPreconditionId+ " " + this.locationName;
        }
    }
}
