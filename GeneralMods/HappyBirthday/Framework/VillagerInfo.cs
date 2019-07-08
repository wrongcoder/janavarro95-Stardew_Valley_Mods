using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework
{
    public class VillagerInfo
    {
        public bool hasGivenBirthdayWish;
        public bool hasGivenBirthdayGift;

        public VillagerInfo()
        {
            this.hasGivenBirthdayGift = false;
            this.hasGivenBirthdayWish = false;
        }

        public void reset()
        {
            this.hasGivenBirthdayGift = false;
            this.hasGivenBirthdayWish = false;
        }

    }
}
