using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.PlayerSpecific
{
    public class SeenEvents:EventPrecondition
    {

        public List<string> seenEvents;

        public SeenEvents()
        {
            this.seenEvents = new List<string>();
        }

        public SeenEvents(string ID)
        {
            this.seenEvents.Add(ID);
        }

        public SeenEvents(List<string> IDS)
        {
            this.seenEvents = IDS.ToList();
        }

        public override string ToString()
        {
            return this.precondition_playerHasSeenEvents();
        }

        /// <summary>
        /// Current player has seen the specified events.
        /// </summary>
        /// <param name="IDS"></param>
        /// <returns></returns>
        public string precondition_playerHasSeenEvents()
        {
            StringBuilder b = new StringBuilder();
            b.Append("e ");
            for (int i = 0; i < this.seenEvents.Count; i++)
            {
                b.Append(this.seenEvents[i]);
                if (i != this.seenEvents.Count - 1)
                {
                    b.Append(" ");
                }
            }
            return b.ToString();
        }

    }
}
