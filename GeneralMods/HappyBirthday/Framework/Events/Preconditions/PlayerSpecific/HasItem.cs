using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.PlayerSpecific
{
    public class HasItem:EventPrecondition
    {

        public int id;

        public HasItem()
        {

        }

        public HasItem(int ID)
        {
            this.id = ID;
        }


        public override string ToString()
        {
            return this.precondition_playerHasItem();
        }


        /// <summary>
        /// Player has the item with the given id. Parent sheet index?
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string precondition_playerHasItem()
        {
            StringBuilder b = new StringBuilder();
            b.Append("i ");
            b.Append(this.id.ToString());
            return b.ToString();
        }
    }
}
