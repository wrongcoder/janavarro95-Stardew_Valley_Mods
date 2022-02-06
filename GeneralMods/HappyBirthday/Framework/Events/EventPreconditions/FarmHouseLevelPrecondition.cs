using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.StardustCore.Events.Preconditions;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Events.EventPreconditions
{
    public class FarmHouseLevelPrecondition:EventPrecondition
    {

        public const string EventPreconditionId = "Omegasis.HappyBirthday.Framework.EventPreconditions.FarmHouseLevelPrecondition";

        public int farmHouseLevel;

        public FarmHouseLevelPrecondition()
        {

        }

        public FarmHouseLevelPrecondition(int Level)
        {

        }


        public override string ToString()
        {
            return EventPreconditionId + " " + this.farmHouseLevel.ToString();
        }

        public override bool meetsCondition()
        {
            return Game1.player.HouseUpgradeLevel == this.farmHouseLevel;
        }
    }
}
