using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.PlayerSpecific
{
    public class Gender:EventPrecondition
    {
        public bool isMale;


        public Gender()
        {

        }

        public Gender(bool IsMale)
        {
            this.isMale = IsMale;
        }

        public override string ToString()
        {
            if (this.isMale)
            {
                return this.precondition_playerIsMale();
            }
            else
            {
                return this.precondition_playerIsFemale();
            }
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

    }
}
