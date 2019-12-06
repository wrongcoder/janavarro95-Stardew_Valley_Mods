using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace StardustCore.Events.Preconditions.PlayerSpecific
{
    public class HasUnlockedCommunityCenter:EventPrecondition
    {


        public HasUnlockedCommunityCenter()
        {

        }

        public override bool meetsCondition()
        {
            return Game1.player.eventsSeen.Contains(611439);
        }
    }
}
