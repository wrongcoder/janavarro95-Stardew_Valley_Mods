using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.StardustCore.Events.Preconditions;

namespace Omegasis.HappyBirthday.Framework.Events.EventPreconditions
{
    public class HasChosenFavoriteGiftPrecondition : EventPrecondition
    {

        public const string EventPreconditionId = "Omegasis.HappyBirthday.Framework.EventPreconditions.HasChosenFavoriteGiftPrecondition";

        public bool hasChosenFavoriteGift;

        public HasChosenFavoriteGiftPrecondition()
        {
            this.hasChosenFavoriteGift = true;
        }

        public HasChosenFavoriteGiftPrecondition(bool ShouldHaveChosenBirthday)
        {
            this.hasChosenFavoriteGift = ShouldHaveChosenBirthday;
        }


        public override string ToString()
        {
            return EventPreconditionId + " " + this.hasChosenFavoriteGift.ToString();
        }

        public override bool meetsCondition()
        {
            return HappyBirthdayModCore.Instance.birthdayManager.hasChoosenFavoriteGift() == this.hasChosenFavoriteGift;
        }
    }
}
