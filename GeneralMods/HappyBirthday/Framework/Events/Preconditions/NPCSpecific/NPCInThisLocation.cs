using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Events.Preconditions.NPCSpecific
{
    public class NPCInThisLocation
    {

        public NPC npc;

        public NPCInThisLocation()
        {

        }

        public NPCInThisLocation(NPC NPC) 
        {
            this.npc = NPC;
        }

        public override string ToString()
        {
            return this.precondition_npcInPlayersLocation();
        }


        /// <summary>
        /// The given npc must be in the same game location as the player.
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public string precondition_npcInPlayersLocation()
        {
            StringBuilder b = new StringBuilder();
            b.Append("p ");
            b.Append(this.npc.Name);
            return b.ToString();
        }

    }
}
