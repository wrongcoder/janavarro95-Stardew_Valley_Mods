using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Gifts
{
    public class GiftInformation
    {


        public int minRequiredHearts;
        public string objectID;
        public int minAmount;
        public int maxAmount;

        public GiftInformation()
        {

        }

        public GiftInformation(GiftIDS.SDVObject ObjectID, int RequiredHearts, int MinAmount, int MaxAmount)
        {
            this.objectID = "StardewValley.Object." + Enum.GetName(typeof(GiftIDS.SDVObject), (int)ObjectID);
            this.minRequiredHearts = RequiredHearts;
            this.maxAmount = 20;
            this.minAmount = MinAmount;
            this.maxAmount = MaxAmount;
        }

        public GiftInformation(string ObjectID, int RequiredHearts, int MinAmount, int MaxAmount)
        {
            this.objectID = ObjectID;
            this.minRequiredHearts = RequiredHearts;
            this.maxAmount = 20;
            this.minAmount = MinAmount;
            this.maxAmount = MaxAmount;
        }

        public GiftInformation(string ObjectID, int RequiredHearts,int MaxRequiredHearts ,int MinAmount, int MaxAmount)
        {
            this.objectID = ObjectID;
            this.minRequiredHearts = RequiredHearts;
            this.maxAmount = MaxRequiredHearts;
            this.minAmount = MinAmount;
            this.maxAmount = MaxAmount;
        }


        public Item getOne()
        {
            Item I = GiftIDS.RegisteredGifts[this.objectID].getOne();
            if (this.minAmount != this.maxAmount)
            {
                I.Stack = StardewValley.Game1.random.Next(this.minAmount, this.maxAmount);
            }
            else
            {
                I.Stack = this.minAmount;
            }
            return I;
        }

    }
}
