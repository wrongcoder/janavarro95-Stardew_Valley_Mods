using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.StardustCore.Events.Preconditions;

namespace Omegasis.HappyBirthday.Framework.Events.EventPreconditions
{
    public class HasChosenBirthdayPrecondition : EventPrecondition
    {
        public const string EventPreconditionId = "Omegasis.HappyBirthday.Framework.EventPreconditions.HasChosenBirthdayPrecondition";

        public bool shouldHaveChosenbirthday;

        public HasChosenBirthdayPrecondition()
        {
            this.shouldHaveChosenbirthday = true;
        }

        public HasChosenBirthdayPrecondition(bool ShouldHaveChosenBirthday)
        {
            this.shouldHaveChosenbirthday = ShouldHaveChosenBirthday;
        }


        public override string ToString()
        {
            return EventPreconditionId + " " + this.shouldHaveChosenbirthday.ToString();
        }

        public override bool meetsCondition()
        {
            return HappyBirthdayModCore.Instance.birthdayManager.hasChosenBirthday()==this.shouldHaveChosenbirthday;
        }

    }
}
