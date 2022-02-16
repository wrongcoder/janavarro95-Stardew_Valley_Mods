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

        public static HasChosenFavoriteGiftPrecondition ParseHasChosenFavoriteGiftPrecondition(string[] preconditionData)
        {
            return new HasChosenFavoriteGiftPrecondition(Convert.ToBoolean(preconditionData[1]));
        }

        public static IsMarriedToPrecondition ParseIsMarriedToPrecondition(string[] preconditionData)
        {
            return new IsMarriedToPrecondition(preconditionData[1]);
        }

        public static GameLocationIsHomePrecondition ParseGameLocationIsHomePrecondition(string[] preconditionData)
        {
            return new GameLocationIsHomePrecondition();
        }

        public static FarmHouseLevelPrecondition ParseFarmHouseLevelPrecondition(string[] preconditionData)
        {
            return new FarmHouseLevelPrecondition(Convert.ToInt32(preconditionData[1]));
        }

        public static YearPrecondition ParseYearGreaterThanOrEqualToPrecondition(string[] preconditionData)
        {
            return new YearPrecondition(Convert.ToInt32(preconditionData[1]), Enum.Parse<YearPrecondition.YearPreconditionType>(preconditionData[2]));
        }

        public static VillagersHaveEnoughFriendshipBirthdayPrecondition ParseVillagersHaveEnoughFriendshipBirthdayPrecondition(string[] preconditionData)
        {
            return new VillagersHaveEnoughFriendshipBirthdayPrecondition();
        }
    }
}
