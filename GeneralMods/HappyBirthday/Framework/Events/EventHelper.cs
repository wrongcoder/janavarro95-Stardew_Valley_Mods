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

        public EventHelper()
        {

        }

        public EventHelper(TimePrecondition Time, EventDayExclusionPrecondition NotTheseDays)
        {
            this.eventData = new StringBuilder();

            this.add(Time.ToString());
            this.add(NotTheseDays.ToString());

        }

        public EventHelper(List<EventPrecondition> Conditions)
        {
            this.eventData = new StringBuilder();
            foreach(var v in Conditions)
            {
                if(v is WeatherPrecondition)
                {
                    WeatherPrecondition w = (v as WeatherPrecondition);
                    if(w.weather== WeatherPrecondition.Weather.Sunny)
                    {
                        this.add(w.precondition_sunnyWeather());
                    }
                    else if(w.weather== WeatherPrecondition.Weather.Rainy)
                    {
                        this.add(w.precondition_rainyWeather());
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


                this.add(v.ToString());
            }
        }

        /// <summary>
        /// Adds in the event data to the string builder and appends seperators as necessary.
        /// </summary>
        /// <param name="Data"></param>
        public void add(string Data)
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
        public int getFacingDirectionNumber(FacingDirection Dir)
        {
            return (int)Dir;
        }

        /// <summary>
        /// Gets the even parsing seperator.
        /// </summary>
        /// <returns></returns>
        public string getSeperator()
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

        /// <summary>
        /// Checks to ensure I don't create a id value that is too big for nexus.
        /// </summary>
        /// <param name="IDToCheck"></param>
        /// <returns></returns>
        public bool isIdValid(int IDToCheck)
        {
            if (IDToCheck > 2147483647 || IDToCheck < 0) return false;
            else return true;
        }

        /// <summary>
        /// Checks to ensure I don't create a id value that is too big for nexus.
        /// </summary>
        /// <param name="IDToCheck"></param>
        /// <returns></returns>
        public bool isIdValid(string IDToCheck)
        {
            if (Convert.ToInt32(IDToCheck) > 2147483647 ||Convert.ToInt32(IDToCheck) < 0) return false;
            else return true;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~//
        //      Preconditions      //
        //~~~~~~~~~~~~~~~~~~~~~~~~~//

        #region

        /// <summary>
        /// Quote from SDV wiki.
        /// "The special dialogue event with the given ID is not in progress.
        /// This can be a custom event ID, but these are the in-game IDs:
        /// cc_Begin, cc_Boulder, cc_Bridge, cc_Bus, cc_Complete, cc_Greenhouse, cc_Minecart, dumped_Girls, dumped_Guys, Introduction, joja_Begin, pamHouseUpgrade, pamHouseUpgradeAnonymous, secondChance_Girls, secondChance_Guys, willyCrabs."
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string precondition_EventNotInProgress(string ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("A ");
            b.Append(ID.ToString());
            return b.ToString();
        }



        /// <summary>
        /// Creates a precondition where the event has a specific amount chance to occur.
        /// </summary>
        /// <param name="Amount">The chance to occur between 0 and 1. .45 would be a 45% chance to occur.</param>
        /// <returns></returns>
        public string precondition_chanceToOccur(float Amount)
        {
            StringBuilder b = new StringBuilder();
            b.Append("r ");
            if (Amount < 0) throw new Exception("Chance amount can't be less than 0!");
            if (Amount > 1) Amount=1;
            b.Append(Amount.ToString());
            return b.ToString();
        }

        /// <summary>
        /// Creates a precondition where the npc is not invisible. (Probably that you can find them in the game world.
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public string precondition_npcNotInvisible(NPC npc)
        {
            StringBuilder b = new StringBuilder();
            b.Append("v ");
            b.Append(npc.Name);
            return b.ToString();
        }




        /// <summary>
        /// Creates a precondition that the current player must be dating the current npc.
        /// </summary>
        /// <param name="NPC"></param>
        /// <returns></returns>
        public string precondition_DatingNPC(NPC NPC)
        {
            StringBuilder b = new StringBuilder();
            b.Append("D ");
            b.Append(NPC.Name);
            return b.ToString();
        }

        /// <summary>
        /// Adds in the precondition that the joja warehouse has been completed.
        /// </summary>
        /// <returns></returns>
        public string precondition_JojaWarehouseCompleted()
        {
            StringBuilder b = new StringBuilder();
            b.Append("J");
            return b.ToString();
        }


        /// <summary>
        /// Adds in the precondition that the player has atleast this much money.
        /// </summary>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public string precondition_playerHasThisMuchMoney(int Amount)
        {
            StringBuilder b = new StringBuilder();
            b.Append("M ");
            b.Append(Amount.ToString());
            return b.ToString();
        }

        /// <summary>
        /// Adds in the precondition that the player has this secret note.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string precondition_playerHasThisSecretNote(int ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("S ");
            b.Append(ID.ToString());
            return b.ToString();
        }

        /// <summary>
        /// Creates the precondition that the player must be standing on this tile.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public string precondition_playerOnThisTile(int x,int y)
        {
            StringBuilder b = new StringBuilder();
            b.Append("a ");
            b.Append(x.ToString());
            b.Append(" ");
            b.Append(y.ToString());
            return b.ToString();
        }

        /// <summary>
        /// Creates the precondition that the player must be standing on this tile.
        /// </summary>
        /// <param name="Position"></param>
        /// <returns></returns>
        public string precondition_playerOnThisTile(Vector2 Position)
        {
            return this.precondition_playerOnThisTile((int)Position.X, (int)Position.Y);
        }

        /// <summary>
        /// Creates the precondition that the player must be standing on this tile.
        /// </summary>
        /// <param name="Position"></param>
        /// <returns></returns>
        public string precondition_playerOnThisTile(Point Position)
        {
            return this.precondition_playerOnThisTile(Position.X, Position.Y);
        }

        /// <summary>
        /// Creates the precondition that the player has reached the bottom of the mines this many times.
        /// </summary>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public string precondition_playerHasReachedMineBottomXTimes(int Amount)
        {
            StringBuilder b = new StringBuilder();
            b.Append("b ");
            b.Append(Amount.ToString());
            return b.ToString();
        }

        /// <summary>
        /// Creates the precondition that the player has atleast this many inventory slots free for the event.
        /// </summary>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public string precondition_playerHasInventorySlotsFree(int Amount)
        {
            StringBuilder b = new StringBuilder();
            b.Append("c ");
            b.Append(Amount.ToString());
            return b.ToString();
        }

        /// <summary>
        /// Current player has seen the specified event .
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string precondition_playerHasSeenEvent(string ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("e ");
            b.Append(ID.ToString());
            return b.ToString();
        }

        /// <summary>
        /// Current player has seen the specified events.
        /// </summary>
        /// <param name="IDS"></param>
        /// <returns></returns>
        public string precondition_playerHasSeenEvents(List<string> IDS)
        {
            StringBuilder b = new StringBuilder();
            b.Append("e ");
            for (int i = 0; i < IDS.Count; i++)
            {
                b.Append(IDS[i]);
                if (i != IDS.Count - 1)
                {
                    b.Append(" ");
                }
            }
            return b.ToString();
        }

        /// <summary>
        /// Gets the amount of friedship points required for this event to occur.
        /// </summary>
        /// <param name="Npc"></param>
        /// <param name="Points"></param>
        /// <returns></returns>
        public string precondition_FriendshipRequired(NPC Npc, int Points)
        {
            StringBuilder b = new StringBuilder();
            b.Append("f ");
            b.Append(Npc.Name);
            b.Append(" ");
            b.Append(Points.ToString());
            return b.ToString();
        }

        /// <summary>
        /// Gets the amount of hearts required for this event to occur.
        /// </summary>
        /// <param name="Points"></param>
        /// <param name="Npc"></param>
        /// <returns></returns>
        public string precondition_FriendshipHeartsRequired(NPC Npc, int Hearts)
        {
            StringBuilder b = new StringBuilder();
            b.Append("f ");
            b.Append(Npc.Name);
            b.Append(" ");
            int points = Hearts * 250;
            b.Append(points.ToString());
            return b.ToString();
        }

        /// <summary>
        /// The player must be male to view this event.
        /// </summary>
        /// <returns></returns>
        public string precondition_playerIsMale()
        {
            StringBuilder b = new StringBuilder();
            b.Append("g ");
            b.Append("0");
            return b.ToString();
        }

        /// <summary>
        /// The player must be female to view this event.
        /// </summary>
        /// <returns></returns>
        public string precondition_playerIsFemale()
        {
            StringBuilder b = new StringBuilder();
            b.Append("g ");
            b.Append("1");
            return b.ToString();
        }

        /// <summary>
        /// Condition: The player has no pet and wants a cat.
        /// </summary>
        /// <returns></returns>
        public string precondition_playerWantsCat()
        {
            StringBuilder b = new StringBuilder();
            b.Append("h ");
            b.Append("cat");
            return b.ToString();
        }
        /// <summary>
        /// Condition: The player has no pet and wants a dog.
        /// </summary>
        /// <returns></returns>
        public string precondition_playerWantsDog()
        {
            StringBuilder b = new StringBuilder();
            b.Append("h ");
            b.Append("dog");
            return b.ToString();
        }

        /// <summary>
        /// Player has the item with the given id. Parent sheet index?
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string precondition_playerHasItem(int ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("i ");
            b.Append(ID.ToString());
            return b.ToString();
        }

        /// <summary>
        /// Player has played for atleast this many days.
        /// </summary>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public string precondition_playerHasPlayedForXDays(int Amount)
        {
            StringBuilder b = new StringBuilder();
            b.Append("j ");
            b.Append(Amount.ToString());
            return b.ToString();
        }

        /// <summary>
        /// The player has not seen the event.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string precondition_playerHasNotSeenEvent(int ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("k ");
            b.Append(ID.ToString());
            return b.ToString();
        }

        /// <summary>
        /// The player has not seen these events.
        /// </summary>
        /// <param name="IDS"></param>
        /// <returns></returns>
        public string precondition_playerHasNotSeenEvents(List<string> IDS)
        {
            StringBuilder b = new StringBuilder();
            b.Append("k ");
            for (int i = 0; i < IDS.Count; i++)
            {
                b.Append(IDS[i]);
                if (i != IDS.Count - 1)
                {
                    b.Append(" ");
                }
            }
            return b.ToString();
        }

        /// <summary>
        /// The player has not seen the letter with the given id.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string precondition_playerHasNotRecievedLetter(string ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("l ");
            b.Append(ID.ToString());
            return b.ToString();
        }

        /// <summary>
        /// Current player has earned at least this much money (regardless of how much they currently have).
        /// </summary>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public string precondition_playerEarnedMoneyTotal(int Amount)
        {
            StringBuilder b = new StringBuilder();
            b.Append("m ");
            b.Append(Amount.ToString());
            return b.ToString();
        }
        /// <summary>
        /// The player has seen the letter with the given id.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string precondition_playerHasRecievedLetter(string ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("n ");
            b.Append(ID.ToString());
            return b.ToString();
        }

        /// <summary>
        /// Current player is not married to that NPC.
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public string precondition_playerNotMarriedToThisNPC(NPC npc)
        {
            StringBuilder b = new StringBuilder();
            b.Append("o ");
            b.Append(npc.Name);
            return b.ToString();
        }

        /// <summary>
        /// The given npc must be in the same game location as the player.
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public string precondition_npcInPlayersLocation(NPC npc)
        {
            StringBuilder b = new StringBuilder();
            b.Append("p ");
            b.Append(npc.Name);
            return b.ToString();
        }
        /// <summary>
        /// The player has answered with the dialogue option of this choice.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string precondition_answeredDialogueOption(string ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("q ");
            b.Append(ID);
            return b.ToString();
        }

        /// <summary>
        /// The player has answered with the dialogue options of these choices.
        /// </summary>
        /// <param name="IDS"></param>
        /// <returns></returns>
        public string precondition_answeredDialogueOptions(List<string> IDS)
        {
            StringBuilder b = new StringBuilder();
            b.Append("q ");
            for (int i = 0; i < IDS.Count; i++)
            {
                b.Append(IDS[i]);
                if (i != IDS.Count - 1)
                {
                    b.Append(" ");
                }
            }
            return b.ToString();
        }

        /// <summary>
        /// Current player has shipped at least <Amount> of the specified item. Can specify multiple item and number pairs, in which case all of them must be met.
        /// </summary>
        /// <param name="ID">The id of the item. Parent sheet index?</param>
        /// <param name="Amount">The amount shipped.</param>
        /// <returns></returns>
        public string precondition_playerHasShippedItem(int ID, int Amount)
        {
            StringBuilder b = new StringBuilder();
            b.Append("s ");
            b.Append(ID);
            b.Append(" ");
            b.Append(Amount.ToString());
            return b.ToString();
        }

        /// <summary>
        /// Current player has shipped at least <Amount> of the specified item. Can specify multiple item and number pairs, in which case all of them must be met.
        /// </summary>
        /// <param name="Pairs"></param>
        /// <returns></returns>
        public string precondition_playerHasShippedTheseItems(List<KeyValuePair<int,int>> Pairs)
        {
            StringBuilder b = new StringBuilder();
            b.Append("s ");
            for(int i = 0; i < Pairs.Count; i++)
            {

                int ID = Pairs[i].Key;
                int Amount = Pairs[i].Value;
                b.Append(ID);
                b.Append(" ");
                b.Append(Amount.ToString());

                if (i != Pairs.Count - 1)
                {
                    b.Append(" ");
                }
            }


            return b.ToString();
        }

        #endregion




    }
}
