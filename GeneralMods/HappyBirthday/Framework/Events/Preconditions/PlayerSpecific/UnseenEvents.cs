using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.PlayerSpecific
{
    public class UnseenEvents:EventPrecondition
    {
        public List<string> unseenEvents;

        public UnseenEvents()
        {
            this.unseenEvents = new List<string>();
        }

        public UnseenEvents(string ID)
        {
            this.unseenEvents.Add(ID);
        }

        public UnseenEvents(List<string> IDS)
        {
            this.unseenEvents = IDS.ToList();
        }

        public override string ToString()
        {
            return this.precondition_playerHasNotSeenEvents();
        }

        /// <summary>
        /// Current player has seen the specified events.
        /// </summary>
        /// <param name="IDS"></param>
        /// <returns></returns>
        public string precondition_playerHasNotSeenEvents()
        {
            StringBuilder b = new StringBuilder();
            b.Append("k ");
            for (int i = 0; i < this.unseenEvents.Count; i++)
            {
                b.Append(this.unseenEvents[i]);
                if (i != this.unseenEvents.Count - 1)
                {
                    b.Append(" ");
                }
            }
            return b.ToString();
        }
    }
}
