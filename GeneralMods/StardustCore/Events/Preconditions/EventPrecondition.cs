using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.Events.Preconditions
{
    public class EventPrecondition
    {


        public virtual bool meetsCondition()
        {
            return false;
        }
    }
}
