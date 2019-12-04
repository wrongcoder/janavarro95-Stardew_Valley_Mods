using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.TimeSpecific
{
    public class SeasonExclusion:EventPrecondition
    {
        public bool spring;
        public bool summer;
        public bool fall;
        public bool winter;

        public SeasonExclusion()
        {

        }

        public SeasonExclusion(bool Spring, bool Summer, bool Fall, bool Winter)
        {
            this.spring = Spring;
            this.summer = Summer;
            this.fall = Fall;
            this.winter = Winter;
        }

        public override string ToString()
        {
            return this.precondition_NotThisSeason();
        }

        /// <summary>
        /// This event will occur in the seasons that aren't these seasons.
        /// </summary>
        /// <param name="Spring">If true this event won't occur in the spring.</param>
        /// <param name="Summer">If true this event won't occur in the summer.</param>
        /// <param name="Fall">If true this event won't occur in the fall.</param>
        /// <param name="Winter">If true this event won't occur in the winter.</param>
        /// <returns></returns>
        public string precondition_NotThisSeason()
        {
            StringBuilder b = new StringBuilder();
            b.Append("d ");

            List<string> words = new List<string>();

            if (this.spring)
            {
                words.Add("spring");
            }
            if (this.summer)
            {
                words.Add("summer");
            }
            if (this.fall)
            {
                words.Add("fall");
            }
            if (this.winter)
            {
                words.Add("winter");
            }

            for (int i = 0; i < words.Count; i++)
            {
                b.Append(words[i]);
                if (i != words.Count - 1)
                {
                    b.Append(" ");
                }
            }
            return b.ToString();
        }

    }
}
