using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.EventPreconditions
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
    }
}
