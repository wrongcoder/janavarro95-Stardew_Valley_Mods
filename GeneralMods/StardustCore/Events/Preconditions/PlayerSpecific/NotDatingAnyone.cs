using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace StardustCore.Events.Preconditions.PlayerSpecific
{
    public class NotDatingAnyone: EventPrecondition
    {
        public NotDatingAnyone()
        {

        }

        public override string ToString()
        {
            return "Omegasis.EventFramework.Precondition.NotDatingAnyone";
        }


        public override bool meetsCondition()
        {
            foreach(GameLocation loc in Game1.locations)
            {
                foreach (NPC npc in loc.getCharacters())
                {
                    if (Game1.player.friendshipData[npc.Name].IsDating()) return false;
                }
            }
            return true;
        }

    }
}
