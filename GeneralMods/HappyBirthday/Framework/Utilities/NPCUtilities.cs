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
        public static NPC LastSpeaker;

        public static List<NPC> GetAllNpcs()
        {
            List<NPC> npcs = new List<NPC>();
            foreach (GameLocation location in Game1.locations)
            {
                foreach (NPC npc in location.getCharacters())
                {
                    npcs.Add(npc);
                }
            }
            return npcs;
        }

        /// <summary>
        /// Checks to see if an npc should wish the player happy birthday or not.
        /// </summary>
        /// <param name="NpcName"></param>
        /// <returns></returns>
        public static bool ShouldWishPlayerHappyBirthday(string NpcName)
        {
            if (HappyBirthday.Instance.birthdayManager.isVillagerInQueue(NpcName) == false) return false;
            if (HappyBirthday.Instance.birthdayManager.hasGivenBirthdayWish(NpcName) == true) return false;
            if (Game1.player.getFriendshipHeartLevelForNPC(NpcName) < HappyBirthday.Configs.modConfig.minimumFriendshipLevelForBirthdayWish)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks to see if an npc should give the player a gift or not.
        /// </summary>
        /// <param name="NpcName"></param>
        /// <returns></returns>
        public static bool ShouldGivePlayerBirthdayGift(string NpcName)
        {
            if (HappyBirthday.Instance.birthdayManager.isVillagerInQueue(NpcName) == false) return false;
            if (HappyBirthday.Instance.birthdayManager.hasGivenBirthdayGift(NpcName) == true) return false;
            if (Game1.player.getFriendshipHeartLevelForNPC(NpcName) < HappyBirthday.Configs.modConfig.minimumFriendshipLevelForBirthdayWish)
            {
                return false;
            }
            return true;
        }

    }
}
