using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.StardustCore.Events.Preconditions.PlayerSpecific
{
    public class CanReadJunimoEventPrecondition:EventPrecondition
    {
        public const string EventPreconditionId = "StardustCore.Events.Preconditions.Player.CanReadJunimo";
        public CanReadJunimoEventPrecondition()
        {

        }

        public override string ToString()
        {
            return EventPreconditionId;
        }

        public override bool meetsCondition()
        {
            return Game1.player.mailReceived.Contains("canReadJunimoText");
        }


    }
}
