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

        public string npc;
        public DatingNPC()
        {

        }
        public DatingNPC(NPC npc)
        {
            this.npc = npc.Name;
        }

        public override string ToString()
        {
            return this.precondition_DatingNPC();
        }
        /// <summary>
        /// Creates a precondition that the current player must be dating the current npc.
        /// </summary>
        /// <returns></returns>
        public string precondition_DatingNPC()
        {
            StringBuilder b = new StringBuilder();
            b.Append("D ");
            b.Append(this.npc);
            return b.ToString();
        }

        public override bool meetsCondition()
        {
            if (Game1.player.friendshipData.ContainsKey(this.npc)){
                return Game1.player.friendshipData[this.npc].IsDating();
            }
            else
            {
                return false;
            }
        }
    }
}
