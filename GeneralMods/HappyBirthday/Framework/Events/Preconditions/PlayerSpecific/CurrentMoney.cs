using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.PlayerSpecific
{
    public class CurrentMoney:EventPrecondition
    {

        public int amount;

        public CurrentMoney()
        {

        }

        public CurrentMoney(int Amount)
        {
            this.amount = Amount;
        }

        public override string ToString()
        {
            return this.precondition_playerHasThisMuchMoney();
        }

        /// <summary>
        /// Adds in the precondition that the player has atleast this much money.
        /// </summary>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public string precondition_playerHasThisMuchMoney()
        {
            StringBuilder b = new StringBuilder();
            b.Append("M ");
            b.Append(this.amount.ToString());
            return b.ToString();
        }
    }
}
