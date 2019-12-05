using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace StardustCore.Events
{
    public class EventManager
    {
        /// <summary>
        /// The list of events that this event manager is holding.
        /// </summary>
        public Dictionary<string, EventHelper> events;

        /// <summary>
        /// Event logic that occurs when the specialized command appears.
        /// </summary>
        public Dictionary<string, Action<EventManager,string>> customEventLogic;


        public Dictionary<string, Action<EventManager,string>> concurrentEventActions;

        public bool eventStarted;

        public EventManager()
        {
            this.events = new Dictionary<string, EventHelper>();
            this.customEventLogic = new Dictionary<string, Action<EventManager,string>>();
            this.concurrentEventActions = new Dictionary<string, Action<EventManager,string>>();

            this.customEventLogic.Add("Omegasis.EventFramework.AddObjectToPlayersInventory", ExtraEventActions.addObjectToPlayerInventory);
            this.customEventLogic.Add("Omegasis.EventFramework.ViewportLerp", ExtraEventActions.ViewportLerp);
        }

        /// <summary>
        /// Adds an event to the dictionary of events.
        /// </summary>
        /// <param name="Event"></param>
        public void addEvent(EventHelper Event)
        {
            this.events.Add(Event.eventName, Event);
        }

        /// <summary>
        /// Adds in custom code that happens when the game's current event sees the given command name.
        /// </summary>
        /// <param name="CommandName"></param>
        /// <param name="Function"></param>
        public void addCustomEventLogic(string CommandName,Action<EventManager,string> Function)
        {
            this.customEventLogic.Add(CommandName, Function);
        }

        public void addConcurrentEvent(string EventData,Action<EventManager,string> Function)
        {

            this.concurrentEventActions.Add(EventData, Function);
        }

        /// <summary>
        /// Hooked into the game's update tick.
        /// </summary>
        public virtual void update()
        {
            if (Game1.CurrentEvent == null) return;
            string commandName = this.getGameCurrentEventCommandStringName();
            //HappyBirthday.ModMonitor.Log("Current event command name is: " + commandName, StardewModdingAPI.LogLevel.Info);

            foreach(KeyValuePair<string,Action<EventManager,string>> pair in this.concurrentEventActions)
            {
                pair.Value.Invoke(this,pair.Key);
            }

            if (string.IsNullOrEmpty(commandName)) return;
            if (this.customEventLogic.ContainsKey(commandName)){
                this.customEventLogic[commandName].Invoke(this,this.getGameCurrentEventCommandString());
            }
        }

        public virtual void finishConcurrentEvent(string Key)
        {
            if (this.concurrentEventActions.ContainsKey(Key))
            {
                this.concurrentEventActions.Remove(Key);
            }
        }

        /// <summary>
        /// Gets the event from this list of events.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Starts the event with the given id if possible.
        /// </summary>
        /// <param name="EventName"></param>
        public virtual void startEventAtLocationIfPossible(string EventName)
        {
            if (this.events.ContainsKey(EventName))
            {
                this.events[EventName].startEventAtLocationifPossible();
            }
        }

        /// <summary>
        /// Clears the event from the farmer.
        /// </summary>
        /// <param name="EventName"></param>
        public void clearEventFromFarmer(string EventName)
        {

            this.events.TryGetValue(EventName, out EventHelper e);
            if (e == null) return;
            Game1.player.eventsSeen.Remove(e.getEventID());
        }

        /// <summary>
        /// Gets the current command and all of it's data.
        /// </summary>
        /// <returns></returns>
        public virtual string getGameCurrentEventCommandString()
        {
            if (Game1.CurrentEvent != null)
            {
                return Game1.CurrentEvent.currentCommandString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the name of the action for the current command in the string of event commands.
        /// </summary>
        /// <returns></returns>
        public virtual string getGameCurrentEventCommandStringName()
        {
            if (Game1.CurrentEvent != null)
            {
                return Game1.CurrentEvent.currentCommandStringName();
            }
            else
            {
                return "";
            }
        }
    }
}
