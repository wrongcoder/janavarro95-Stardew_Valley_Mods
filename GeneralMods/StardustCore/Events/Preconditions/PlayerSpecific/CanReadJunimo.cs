using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace StardustCore.Events.Preconditions.PlayerSpecific
{
    public class CanReadJunimo:EventPrecondition
    {

        public CanReadJunimo()
        {

        }

        public override string ToString()
        {
            return "StardustCore.Events.Preconditions.Player.CanReadJunimo";
        }

        public override bool meetsCondition()
        {
            return Game1.player.mailReceived.Contains("canReadJunimoText");
        }


    }
}
