using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Utilities
{
    public class TimeUtilities
    {
        public static int GetMinutesFromTime(int Days, int Hours, int Minutes)
        {
            int amount=0;
            amount += Days * 24 * 60;
            amount += Hours * 60;
            amount += Minutes;
            return amount;
        }
    }
}
