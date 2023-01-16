using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Utilities
{
    public class TimeUtilities
    {
        /// <summary>
        /// Gets the minutes for the time passed in.
        /// </summary>
        /// <param name="Days"></param>
        /// <param name="Hours"></param>
        /// <param name="Minutes"></param>
        /// <returns></returns>
        public static int GetMinutesFromTime(int Days, int Hours, int Minutes)
        {
            int amount = 0;
            amount += Days * 24 * 60;
            amount += Hours * 60;
            amount += Minutes;
            return amount;
        }
    }
}
