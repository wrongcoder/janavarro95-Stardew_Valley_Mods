using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.StardustCore.Events
{
    public class ConcurrentEventInformation
    {
        public string id;
        public string[] commandInfo;
        public Action<Event, string[], EventContext> functionToRun;
        public bool finished;

        public ConcurrentEventInformation()
        {

        }

        public ConcurrentEventInformation(string ID, string[] CommandInfo,Action<Event, string[], EventContext> Function)
        {
            this.id = ID;
            this.commandInfo = CommandInfo;
            this.functionToRun = Function;
        }

        public void finish()
        {
            this.finished = true;
        }


        public void invokeIfNotFinished()
        {
            if (this.finished) return;
            this.functionToRun.Invoke(Game1.CurrentEvent, this.commandInfo,null);
        }

        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj as ConcurrentEventInformation).id.Equals(this.id);
        }

    }
}
