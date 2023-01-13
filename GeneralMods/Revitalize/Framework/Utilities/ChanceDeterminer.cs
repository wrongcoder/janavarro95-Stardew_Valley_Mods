using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Utilities
{
    /// <summary>
    /// A utility class for determining if a value is within a valid range. If so, return the value stored by this object.
    /// </summary>
    public class IntOutcomeChanceDeterminer
    {
        /// <summary>
        /// The chance range for if this value should occur. For example a range of 1-20 means there is a 20% chance a range check of 1-100 (inclusive) passes and retuns the value determined by <see cref="getValueIfInInclusiveBounds(int)"/>
        /// </summary>
        public IntRange validRangeForChance;
        /// <summary>
        /// The returned value if the <see cref="getValueIfInInclusiveBounds(int)"/> returns true.
        /// </summary>
        public IntRange outcomeValue;

        public IntOutcomeChanceDeterminer()
        {

        }

        public IntOutcomeChanceDeterminer(IntRange validRangeForChance, int outcomeValue)
        {
            this.validRangeForChance = validRangeForChance;
            this.outcomeValue = new IntRange(outcomeValue,outcomeValue);
        }

        public IntOutcomeChanceDeterminer(IntRange validRangeForChance, IntRange outcomeValue)
        {
            this.validRangeForChance = validRangeForChance;
            this.outcomeValue = outcomeValue;
        }

        public virtual int getValueIfInInclusiveBounds(int Determiner)
        {
            if (this.validRangeForChance.ContainsExclusive(Determiner))
            {
                return this.outcomeValue.getRandomInclusive();
            }
            return 0;
        }

        public virtual bool containsInclusive(int Determiner)
        {
            return this.validRangeForChance.ContainsExclusive(Determiner);
        }
    }
}
