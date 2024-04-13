using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.HappyBirthday.Framework.Utilities;
using Omegasis.StardustCore.Events.Preconditions;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Events.EventPreconditions
{
    /// <summary>
    /// A condition that checks the current farmhouse level for the player.
    /// </summary>
    public class FarmHouseLevelPrecondition:EventPrecondition
    {

        public const string EventPreconditionId = "Omegasis.HappyBirthday.Framework.EventPreconditions.FarmHouseLevelPrecondition";

        /// <summary>
        /// The level requirement for the player's home.
        /// </summary>
        public int farmHouseLevel;

        /// <summary>
        /// The comparison type to the actual value that the Player's home value is.
        /// </summary>
        public Enums.ComparisonType comparisonType = Enums.ComparisonType.EqualTo;



        public FarmHouseLevelPrecondition()
        {

        }

        public FarmHouseLevelPrecondition(int Level,Enums.ComparisonType ComparisonType= Enums.ComparisonType.EqualTo)
        {
            this.farmHouseLevel = Level;
            this.comparisonType = ComparisonType;
        }


        public override string ToString()
        {
            return string.Format("{0} {1} {2}", EventPreconditionId, this.farmHouseLevel.ToString(), this.comparisonType);
        }

        public override bool meetsCondition()
        {
            switch (this.comparisonType)
            {
                case Enums.ComparisonType.LessThan:
                    return Game1.player.HouseUpgradeLevel < this.farmHouseLevel;
                case Enums.ComparisonType.LessThanOrEqualTo:
                    return Game1.player.HouseUpgradeLevel <= this.farmHouseLevel;
                case Enums.ComparisonType.EqualTo:
                    return Game1.player.HouseUpgradeLevel == this.farmHouseLevel;
                case Enums.ComparisonType.GreaterTo:
                    return Game1.player.HouseUpgradeLevel > this.farmHouseLevel;
                case Enums.ComparisonType.GreaterThanOrEqualTo:
                    return Game1.player.HouseUpgradeLevel >= this.farmHouseLevel;
                default:
                    return false;
            }
        }
    }
}
