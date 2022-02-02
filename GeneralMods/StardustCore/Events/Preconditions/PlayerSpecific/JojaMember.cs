using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace StardustCore.Events.Preconditions.PlayerSpecific
{
    public class JojaMember:EventPrecondition
    {

        public bool isJojaMember;

        public JojaMember()
        {

        }

        public JojaMember(bool IsMember)
        {
            this.isJojaMember = IsMember;
        }

        public override bool meetsCondition()
        {
            if (this.isJojaMember)
            {
                return Game1.player.mailReceived.Contains("JojaMember") == true;
            }
            else
            {
                return Game1.player.mailReceived.Contains("JojaMember") == false;
            }
        }

        public override string ToString()
        {
            return "StardustCore.Events.Preconditions.PlayerSpecific.JojaMemeber " + this.isJojaMember.ToString(); 
        }
    }
}
