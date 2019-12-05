using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.HappyBirthday.Framework.Events;
using Omegasis.HappyBirthday.Framework.Events.Preconditions;
using Omegasis.HappyBirthday.Framework.Events.Preconditions.TimeSpecific;
using Omegasis.HappyBirthday.Framework.Events.SpecialPreconditions;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework
{
    public class BirthdayEvents
    {

        public static EventHelper CommunityCenterBirthday()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new LocationPrecondition(Game1.getLocationFromName("CommunityCenter")));
            conditions.Add(new TimePrecondition(600, 2600));
            EventHelper e = new EventHelper("CommunityCenterBirthday",19950, conditions, new EventStartData(EventStartData.MusicToPlayType.Continue, 32, 22, new EventStartData.FarmerData(32, 22, EventHelper.FacingDirection.Up),new List<EventStartData.NPCData>()));
            e.globalFadeIn();
            e.moveFarmerUp(6, EventHelper.FacingDirection.Up, false);
            e.showMessage("Community center birthday here.");
            e.end();

            return e;
        }
    }
}
