using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardustCore.Events.Preconditions;

namespace Omegasis.HappyBirthday.Framework.EventPreconditions
{
    public class FarmerBirthdayPrecondition:EventPrecondition
    {


        public FarmerBirthdayPrecondition()
        {

        }

        public override string ToString()
        {
            return "Omegasis.HappyBirthday";
        }

        public override bool meetsCondition()
        {
            return HappyBirthday.Instance.birthdayManager.isBirthday();
        }

    }
}
