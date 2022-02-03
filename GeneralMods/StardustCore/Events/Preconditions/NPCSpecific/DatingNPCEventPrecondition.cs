using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.StardustCore.Events.Preconditions.NPCSpecific
{
    public class DatingNPCEventPrecondition:EventPrecondition
    {
        public const string EventPreconditionId = "StardustCore.Events.Preconditions.NPCSpecific.DatingNPC";

        public string datingNpc;
        public DatingNPCEventPrecondition()
        {

        }

        public DatingNPCEventPrecondition(string datingNpcName)
        {
            this.datingNpc = datingNpcName;
        }

        public DatingNPCEventPrecondition(NPC npc)
        {
            this.datingNpc = npc.Name;
        }

        public override string ToString()
        {
            return EventPreconditionId + " " + this.datingNpc;
        }

        public override bool meetsCondition()
        {
            if (Game1.player.friendshipData.ContainsKey(this.datingNpc)){
                return Game1.player.friendshipData[this.datingNpc].IsDating();
            }
            else
            {
                return false;
            }
        }
    }
}
