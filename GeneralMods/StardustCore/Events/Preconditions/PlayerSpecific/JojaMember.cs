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

        public bool isMember;

        public JojaMember()
        {

        }

        public JojaMember(bool IsMember)
        {
            this.isMember = IsMember;
        }

        public override bool meetsCondition()
        {
            if (this.isMember)
            {
                return Game1.player.mailReceived.Contains("JojaMember") == true;
            }
            else
            {
                return Game1.player.mailReceived.Contains("JojaMember") == false;
            }
        }
    }
}
