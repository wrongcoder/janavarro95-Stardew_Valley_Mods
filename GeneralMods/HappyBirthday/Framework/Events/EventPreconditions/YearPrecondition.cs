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
    public class YearPrecondition : EventPrecondition
    {
        public const string EventPreconditionId = "Omegasis.HappyBirthday.Framework.EventPreconditions.YearPrecondition";

        public Enums.ComparisonType yearPreconditionType;

        public int yearValue;

        public YearPrecondition()
        {

        }

        public YearPrecondition(int year, Enums.ComparisonType yearPreconditionType)
        {
            this.yearValue = year;
            this.yearPreconditionType = yearPreconditionType;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", EventPreconditionId, this.yearValue.ToString(), this.yearPreconditionType);
        }

        public override bool meetsCondition()
        {
            switch (this.yearPreconditionType)
            {
                case Enums.ComparisonType.LessThan:
                    return Game1.year < this.yearValue;
                case Enums.ComparisonType.LessThanOrEqualTo:
                    return Game1.year <= this.yearValue;
                case Enums.ComparisonType.EqualTo:
                    return Game1.year == this.yearValue;
                case Enums.ComparisonType.GreaterTo:
                    return Game1.year > this.yearValue;
                case Enums.ComparisonType.GreaterThanOrEqualTo:
                    return Game1.year >= this.yearValue;
                default:
                    return false;
            }
        }

    }
}
