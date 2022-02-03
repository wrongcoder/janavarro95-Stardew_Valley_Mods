using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.StardustCore.Events.Preconditions;

namespace Omegasis.HappyBirthday.Framework.EventPreconditions
{
    public class FarmerBirthdayPrecondition:EventPrecondition
    {
        public const string EventPreconditionId = "Omegasis.HappyBirthday.Framework.EventPreconditions.FarmerBirthdayPrecondition";

        public FarmerBirthdayPrecondition()
        {

        }

        public override string ToString()
        {
            return EventPreconditionId;
        }

        public override bool meetsCondition()
        {
            return HappyBirthdayModCore.Instance.birthdayManager.isBirthday();
        }

    }
}
