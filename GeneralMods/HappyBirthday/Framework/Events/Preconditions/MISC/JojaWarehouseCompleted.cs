using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.MISC
{
    public class JojaWarehouseCompleted:EventPrecondition
    {
        public JojaWarehouseCompleted()
        {

        }

        public override string ToString()
        {
            return this.precondition_JojaWarehouseCompleted();
        }

        /// <summary>
        /// Adds in the precondition that the joja warehouse has been completed.
        /// </summary>
        /// <returns></returns>
        public string precondition_JojaWarehouseCompleted()
        {
            StringBuilder b = new StringBuilder();
            b.Append("J");
            return b.ToString();
        }
    }
}
