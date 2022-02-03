using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.StardustCore.Events.Preconditions.PlayerSpecific
{
    public class IsJojaMemberEventPrecondition:EventPrecondition
    {

        public const string EventPreconditionId = "StardustCore.Events.Preconditions.PlayerSpecific.JojaMemeber";

        public bool isJojaMember;

        public IsJojaMemberEventPrecondition()
        {

        }

        public IsJojaMemberEventPrecondition(bool IsMember)
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
            return EventPreconditionId +" " + this.isJojaMember.ToString(); 
        }
    }
}
