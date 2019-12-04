using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.PlayerSpecific
{
    public class HasNotRecievedLetter:EventPrecondition
    {

        public string id;

        public HasNotRecievedLetter()
        {

        }

        public HasNotRecievedLetter(string ID)
        {
            this.id = ID;
        }

        public override string ToString()
        {
            return this.precondition_playerHasNotRecievedLetter();
        }

        /// <summary>
        /// The player has not seen the letter with the given id.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string precondition_playerHasNotRecievedLetter()
        {
            StringBuilder b = new StringBuilder();
            b.Append("l ");
            b.Append(this.id.ToString());
            return b.ToString();
        }
    }
}
