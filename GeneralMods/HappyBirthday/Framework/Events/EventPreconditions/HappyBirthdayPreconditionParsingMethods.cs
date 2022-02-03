using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Events.EventPreconditions
{
    public static class HappyBirthdayPreconditionParsingMethods
    {
        public static FarmerBirthdayPrecondition ParseFarmerBirthdayPrecondition(string[] preconditionData)
        {
            return new FarmerBirthdayPrecondition();
        }

        public static SpouseBirthdayPrecondition ParseSpouseBirthdayPrecondition(string[] preconditionData)
        {
            return new SpouseBirthdayPrecondition();
        }

        public static HasChosenBirthdayPrecondition ParseHasChosenBirthdayPrecondition(string[] preconditionData)
        {
            return new HasChosenBirthdayPrecondition(Convert.ToBoolean(preconditionData[1]));
        }
    }
}
