using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Utilities
{
    public class Enums
    {
        /// <summary>
        /// Determines how the <see cref="Game1.year"/> value should be in terms of <see cref="yearValue"/>'s value.
        /// </summary>
        public enum ComparisonType
        {
            LessThan,
            LessThanOrEqualTo,
            EqualTo,
            GreaterTo,
            GreaterThanOrEqualTo
        }
    }
}
