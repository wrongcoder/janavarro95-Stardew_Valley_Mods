using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.MISC
{
    public class ChanceToOccur:EventPrecondition
    {

        public float chance;

        public ChanceToOccur()
        {

        }

        public ChanceToOccur(float Chance)
        {
            if (Chance < 0) throw new Exception("Chance amount can't be less than 0!");
            if (Chance > 1) Chance = 1;
            this.chance = Chance;
        }

        public override string ToString()
        {
            return this.precondition_chanceToOccur();
        }

        /// <summary>
        /// Creates a precondition where the event has a specific amount chance to occur.
        /// </summary>
        /// <param name="Amount">The chance to occur between 0 and 1. .45 would be a 45% chance to occur.</param>
        /// <returns></returns>
        public string precondition_chanceToOccur()
        {
            StringBuilder b = new StringBuilder();
            b.Append("r ");
            b.Append(this.chance.ToString());
            return b.ToString();
        }
    }
}
