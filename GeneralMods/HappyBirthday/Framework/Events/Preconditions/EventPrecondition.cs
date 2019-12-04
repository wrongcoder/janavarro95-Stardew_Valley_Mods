using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions
{
    public class EventPrecondition
    {


        public virtual bool meetsCondition()
        {
            return false;
        }
    }
}
