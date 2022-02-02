using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.Events.Preconditions
{
    public class EventPrecondition
    {

        public virtual string SerializedPrecondition
        {
            get
            {
                return this.ToString();
            }
        }

        public virtual bool meetsCondition()
        {
            return false;
        }

        public override string ToString()
        {
            return "StarducstCore.Events.Preconditions.EventPrecondition";
        }
    }
}
