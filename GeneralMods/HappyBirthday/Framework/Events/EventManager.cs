using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Events
{
    public class EventManager
    {


        public Dictionary<string, EventHelper> events;


        public EventManager()
        {
            this.events = new Dictionary<string, EventHelper>();
        }

        public void addEvent(EventHelper Event)
        {
            this.events.Add(Event.eventName, Event);
        }

        public EventHelper getEvent(string Name)
        {
            if (this.events.ContainsKey(Name))
            {
                return this.events[Name];
            }
            else
            {
                return null;
            }
        }


        public void clearEventFromFarmer(string EventName)
        {

            this.events.TryGetValue(EventName, out EventHelper e);
            if (e == null) return;
            Game1.player.eventsSeen.Remove(e.getEventID());
        }
    }
}
