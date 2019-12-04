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
        private const int nexusUserId = 32171640;

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


        private bool _precondition_snowWeather;
        private bool _precondition_debrisWeather;
        private bool _precondition_weddingDayWeather;
        private bool _precondition_stormyWeather;
        private bool _precondition_festivalWeather;


        private StringBuilder eventData;

        public int eventID;

        public EventHelper()
        {

        }

        public EventHelper(int ID,TimePrecondition Time, EventDayExclusionPrecondition NotTheseDays)
        {
            this.eventData = new StringBuilder();
            this.eventID = ID;
            this.add(Time);
            this.add(NotTheseDays);

        }

        public EventHelper(List<EventPrecondition> Conditions)
        {
            this.eventData = new StringBuilder();
            foreach(var v in Conditions)
            {
                if(v is WeatherPrecondition)
                {
                    WeatherPrecondition w = (v as WeatherPrecondition);
                    if(w.weather== WeatherPrecondition.Weather.Sunny || w.weather== WeatherPrecondition.Weather.Rainy)
                    {
                        this.add(v);
                    }
                    else if(w.weather== WeatherPrecondition.Weather.Debris)
                    {
                        this._precondition_debrisWeather = true;
                    }
                    else if(w.weather== WeatherPrecondition.Weather.Festival)
                    {
                        this._precondition_festivalWeather = true;
                    }
                    else if(w.weather== WeatherPrecondition.Weather.Snow)
                    {
                        this._precondition_snowWeather = true;
                    }
                    else if(w.weather== WeatherPrecondition.Weather.Storm)
                    {
                        this._precondition_stormyWeather = true;
                    }
                    else if(w.weather== WeatherPrecondition.Weather.Wedding)
                    {
                        this._precondition_weddingDayWeather = true;
                    }
                    continue;
                }


                this.add(v);
            }
        }

        /// <summary>
        /// Adds in the event data to the string builder and appends seperators as necessary.
        /// </summary>
        /// <param name="Data"></param>
        public virtual void add(EventPrecondition Data)
        {
            if (this.eventData.Length > 0)
            {
                this.eventData.Append(this.getSeperator());
            }
            this.eventData.Append(Data.ToString());
        }

        public virtual void add(string Data)
        {

            if (this.eventData.Length > 0)
            {
                this.eventData.Append(this.getSeperator());
            }
            this.eventData.Append(Data);
        }


        /// <summary>
        /// Converts the direction to enum.
        /// </summary>
        /// <param name="Dir"></param>
        /// <returns></returns>
        public virtual int getFacingDirectionNumber(FacingDirection Dir)
        {
            return (int)Dir;
        }

        /// <summary>
        /// Gets the even parsing seperator.
        /// </summary>
        /// <returns></returns>
        public virtual string getSeperator()
        {
            return "/";
        }

        /// <summary>
        /// Gets the starting event numbers based off of my nexus user id.
        /// </summary>
        /// <returns></returns>
        private string getUniqueEventStartID()
        {
            string s = nexusUserId.ToString();
            return s.Substring(0, 4);
        }

        private string getEventID()
        {
            return this.getUniqueEventStartID() + this.eventID.ToString();
        }

        /// <summary>
        /// Checks to ensure I don't create a id value that is too big for nexus.
        /// </summary>
        /// <param name="IDToCheck"></param>
        /// <returns></returns>
        public virtual bool isIdValid(int IDToCheck)
        {
            if (IDToCheck > 2147483647 || IDToCheck < 0) return false;
            else return true;
        }

        /// <summary>
        /// Checks to ensure I don't create a id value that is too big for nexus.
        /// </summary>
        /// <param name="IDToCheck"></param>
        /// <returns></returns>
        public virtual bool isIdValid(string IDToCheck)
        {
            if (Convert.ToInt32(IDToCheck) > 2147483647 ||Convert.ToInt32(IDToCheck) < 0) return false;
            else return true;
        }

        public virtual string getEventString()
        {
            return this.eventData.ToString();
        }

        public virtual StardewValley.Event getEvent(Farmer PlayerActor=null)
        {
            return new StardewValley.Event(this.getEventString(), Convert.ToInt32(this.getEventID()), PlayerActor);
        }


        //~~~~~~~~~~~~~~~~//
        //      Actions   //
        //~~~~~~~~~~~~~~~~//

        public virtual void addBigProp(int xTile, int yTile, int ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addBigProp ");
            b.Append(xTile.ToString());
            b.Append(" ");
            b.Append(yTile.ToString());
            b.Append(" ");
            b.Append(ID.ToString());
            this.add(b.ToString());
        }


    }
}
