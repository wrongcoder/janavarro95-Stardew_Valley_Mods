using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace StardustCore.Events.Preconditions.NPCSpecific
{
    public class DatingNPC:EventPrecondition
    {

        public string datingNpc;
        public DatingNPC()
        {

        }
        public DatingNPC(NPC npc)
        {
            this.datingNpc = npc.Name;
        }

        public override string ToString()
        {
            return "StardustCore.Events.Preconditions.NPCSpecific.DatingNPC " + this.datingNpc;
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
