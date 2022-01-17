using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Utilities
{
    public static class NPCUtilities
    {

        public static List<NPC> GetAllNpcs()
        {
            List<NPC> npcs = new List<NPC>();
            foreach(GameLocation location in Game1.locations)
            {
                foreach(NPC npc in location.getCharacters())
                {
                    npcs.Add(npc);
                }
            }
            return npcs;
        }

    }
}
