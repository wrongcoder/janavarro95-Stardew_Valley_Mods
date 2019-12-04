using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.HappyBirthday.Framework.Events.Preconditions;
using Omegasis.HappyBirthday.Framework.Events.Preconditions.TimeSpecific;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Events
{
    /// <summary>
    /// Helps in creating events based in code for stardew valley.
    /// https://stardewvalleywiki.com/Modding:Event_data
    /// </summary>
    public class EventHelper
    {

        /// <summary>
        /// Nexus user id for Omegasis (aka me).
        /// </summary>
        protected int nexusUserId = 32171640;

        /// <summary>
        /// Wraps SDV facing direction.
        /// </summary>
        public enum FacingDirection
        {
            Up,
            Right,
            Down,
            Left
        }


        protected StringBuilder eventData;
        protected StringBuilder eventPreconditionData;
        protected List<EventPrecondition> eventPreconditions;
        protected int eventID;

        public EventHelper()
        {
            this.eventData = new StringBuilder();
            this.eventPreconditionData = new StringBuilder();
            this.eventPreconditions = new List<EventPrecondition>();
        }

        public EventHelper(int ID,LocationPrecondition Location ,TimePrecondition Time, EventDayExclusionPrecondition NotTheseDays, EventStartData StartData)
        {
            this.eventData = new StringBuilder();
            this.eventPreconditionData = new StringBuilder();
            this.eventPreconditions = new List<EventPrecondition>();
            this.eventID = ID;
            this.add(Location);
            this.add(Time);
            this.add(NotTheseDays);
            this.add(StartData.ToString());
        }

        public EventHelper(List<EventPrecondition> Conditions, EventStartData StartData)
        {
            this.eventData = new StringBuilder();
            this.eventPreconditions = new List<EventPrecondition>();
            this.eventPreconditionData = new StringBuilder();
            foreach (var v in Conditions)
            {
                this.add(v);
            }
            this.add(StartData.ToString());
        }

        /// <summary>
        /// Adds in the event precondition data to the string builder and appends seperators as necessary.
        /// </summary>
        /// <param name="Data"></param>
        public virtual void add(EventPrecondition Data)
        {
            this.eventPreconditions.Add(Data);
            if (this.eventPreconditionData.Length > 0)
            {
                this.eventPreconditionData.Append(this.getSeperator());
            }
            this.eventPreconditionData.Append(Data.ToString());
        }

        /// <summary>
        /// Adds in the data to the event data.Aka what happens during the event.
        /// </summary>
        /// <param name="Data"></param>
        protected virtual void add(string Data)
        {

            if (this.eventData.Length > 0)
            {
                this.eventData.Append(this.getSeperator());
            }
            this.eventData.Append(Data);
        }

        /// <summary>
        /// Adds in the data to the event data. Aka what happens during the event.
        /// </summary>
        /// <param name="Builder"></param>
        protected virtual void add(StringBuilder Builder)
        {
            this.add(Builder.ToString());
        }


        /// <summary>
        /// Converts the direction to enum.
        /// </summary>
        /// <param name="Dir"></param>
        /// <returns></returns>
        protected virtual int getFacingDirectionNumber(FacingDirection Dir)
        {
            return (int)Dir;
        }

        /// <summary>
        /// Gets the even parsing seperator.
        /// </summary>
        /// <returns></returns>
        protected virtual string getSeperator()
        {
            return "/";
        }

        /// <summary>
        /// Gets the starting event numbers based off of my nexus user id.
        /// </summary>
        /// <returns></returns>
        protected virtual string getUniqueEventStartID()
        {
            string s = this.nexusUserId.ToString();
            return s.Substring(0, 4);
        }

        protected virtual string getEventID()
        {
            return this.getUniqueEventStartID() + this.eventID.ToString();
        }

        /// <summary>
        /// Checks to ensure I don't create a id value that is too big for nexus.
        /// </summary>
        /// <param name="IDToCheck"></param>
        /// <returns></returns>
        protected virtual bool isIdValid(int IDToCheck)
        {
            if (IDToCheck > 2147483647 || IDToCheck < 0) return false;
            else return true;
        }

        /// <summary>
        /// Checks to ensure I don't create a id value that is too big for nexus.
        /// </summary>
        /// <param name="IDToCheck"></param>
        /// <returns></returns>
        protected virtual bool isIdValid(string IDToCheck)
        {
            if (Convert.ToInt32(IDToCheck) > 2147483647 || Convert.ToInt32(IDToCheck) < 0) return false;
            else return true;
        }

        protected virtual string getEventString()
        {
            return this.eventData.ToString();
        }

        protected virtual StardewValley.Event getEvent(Farmer PlayerActor = null)
        {
            return new StardewValley.Event(this.getEventString(), Convert.ToInt32(this.getEventID()), PlayerActor);
        }

        /// <summary>
        /// Checks to see if all of the event preconditions have been met and starts the event if so.
        /// </summary>
        protected virtual void startEventAtLocationifPossible()
        {
            if (this.canEventOccur())
            {
                Game1.player.currentLocation.currentEvent = this.getEvent();
            }
        }

        //~~~~~~~~~~~~~~~~//
        //   Validation   //
        //~~~~~~~~~~~~~~~~//

            /// <summary>
            /// Checks to see if the event can occur.
            /// </summary>
            /// <returns></returns>
        protected virtual bool canEventOccur()
        {
            foreach(EventPrecondition eve in this.eventPreconditions)
            {
                if (eve.meetsCondition() == false) return false;
            }

            return true;
        }

        //~~~~~~~~~~~~~~~~//
        //      Actions   //
        //~~~~~~~~~~~~~~~~//

        /// <summary>
        /// Adds an object at the specified tile from the TileSheets\Craftables.png sprite sheet
        /// </summary>
        /// <param name="xTile"></param>
        /// <param name="yTile"></param>
        /// <param name="ID"></param>
        protected virtual void addBigProp(int xTile, int yTile, int ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addBigProp ");
            b.Append(xTile.ToString());
            b.Append(" ");
            b.Append(yTile.ToString());
            b.Append(" ");
            b.Append(ID.ToString());
            this.add(b);
        }

        /// <summary>
        /// Starts an active dialogue event with the given ID and a length of 4 days.
        /// </summary>
        /// <param name="ID"></param>
        protected virtual void addConversationTopic(string ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addBigProp ");
            b.Append(ID);
            this.add(b);
        }

        /// <summary>
        /// Adds the specified cooking recipe to the player.
        /// </summary>
        /// <param name="Recipe"></param>
        protected virtual void addCookingRecipe(string Recipe)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addCookingRecipe ");
            b.Append(Recipe);
            this.add(b);
        }

        /// <summary>
        /// Adds the specified crafting recipe to the player.
        /// </summary>
        /// <param name="Recipe"></param>
        protected virtual void addCraftingRecipe(string Recipe)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addCraftingRecipe ");
            b.Append(Recipe);
            this.add(b);
        }

        /// <summary>
        /// Add a non-solid prop from the current festival texture. Default solid width/height is 1. Default display height is solid height.
        /// </summary>
        protected virtual void addFloorProp(int PropIndex, int XTile, int YTile, int SolidWidth, int SolidHeight, int DisplayHeight)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addFloorProp ");
            b.Append(PropIndex.ToString());
            b.Append(" ");
            b.Append(XTile.ToString());
            b.Append(" ");
            b.Append(YTile.ToString());
            b.Append(" ");
            b.Append(SolidWidth.ToString());
            b.Append(" ");
            b.Append(SolidHeight.ToString());
            b.Append(" ");
            b.Append(DisplayHeight.ToString());
            this.add(b);
        }

        /// <summary>
        /// Adds a glowing temporary sprite at the specified tile from the Maps\springobjects.png sprite sheet. A light radius of 0 just places the sprite.
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="XPosition"></param>
        /// <param name="YPosition"></param>
        /// <param name="LightRadius"></param>
        protected virtual void addLantern(int ItemID, int XPosition, int YPosition, float LightRadius)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addLantern ");
            b.Append(ItemID.ToString());
            b.Append(" ");
            b.Append(XPosition.ToString());
            b.Append(" ");
            b.Append(YPosition.ToString());
            b.Append(" ");
            b.Append(LightRadius.ToString());
            this.add(b);
        }

        /// <summary>
        /// 	Set a letter as received.
        /// </summary>
        /// <param name="ID"></param>
        protected virtual void addMailReceived(string ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addMailReceived  ");
            b.Append(ID);
            this.add(b);
        }



    }
}
