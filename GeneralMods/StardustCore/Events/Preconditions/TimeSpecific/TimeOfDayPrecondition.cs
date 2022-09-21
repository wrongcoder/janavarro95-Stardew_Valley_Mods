using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.StardustCore.Events.Preconditions.TimeSpecific
{
    /// <summary>
    /// The event will only happen between the given times.
    /// </summary>
    /// <param name="StartTime"></param>
    /// <param name="EndTime"></param>
    /// <returns></returns>
    public class TimeOfDayPrecondition:EventPrecondition
    {
        public const string EventPreconditionId = "StardustCore.Events.Preconditions.TimeSpecific.TimeOfDayPrecondition";

        public int startTimeOfDay;
        public int endTimeOfDay;

        public TimeOfDayPrecondition()
        {

        }

        public TimeOfDayPrecondition(int Start, int End)
        {
            this.startTimeOfDay = Start;
            this.endTimeOfDay = End;
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append(EventPreconditionId);
            b.Append(" ");
            b.Append(this.startTimeOfDay.ToString());
            b.Append(" ");
            b.Append(this.endTimeOfDay.ToString());
            return b.ToString();

        }

        public override bool meetsCondition()
        {
            if (Game1.timeOfDay >= this.startTimeOfDay && Game1.timeOfDay <= this.endTimeOfDay) return true;
            else return false;
        }

    }
}
