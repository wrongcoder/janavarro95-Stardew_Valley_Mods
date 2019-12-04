using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.TimeSpecific
{
    public class EventDayExclusionPrecondition:EventPrecondition
    {

        public bool sunday;
        public bool monday;
        public bool tuesday;
        public bool wednesday;
        public bool thursday;
        public bool friday;
        public bool saturday;

        public EventDayExclusionPrecondition()
        {

        }

        public EventDayExclusionPrecondition(bool Sunday, bool Monday, bool Tuesday, bool Wednesday, bool Thursday, bool Friday, bool Saturday)
        {
            this.sunday = Sunday;
            this.monday = Monday;
            this.tuesday = Tuesday;
            this.wednesday = Wednesday;
            this.thursday = Thursday;
            this.friday = Friday;
            this.saturday = Saturday;
        }

        /// <summary>
        /// Gets the event precondition data.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append("d ");

            List<string> words = new List<string>();

            if (this.monday)
            {
                words.Add("Mon");
            }
            if (this.tuesday)
            {
                words.Add("Tue");
            }
            if (this.wednesday)
            {
                words.Add("Wed");
            }
            if (this.thursday)
            {
                words.Add("Thu");
            }
            if (this.friday)
            {
                words.Add("Fri");
            }
            if (this.saturday)
            {
                words.Add("Sat");
            }
            if (this.sunday)
            {
                words.Add("Sun");
            }

            for (int i = 0; i < words.Count; i++)
            {
                b.Append(words[i]);
                if (i != words.Count - 1)
                {
                    b.Append(" ");
                }
            }
            return b.ToString();
        }
    }
}
