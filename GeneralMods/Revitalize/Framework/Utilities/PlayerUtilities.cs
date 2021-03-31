using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Revitalize.Framework.Utilities
{
    public class PlayerUtilities
    {
        /// <summary>
        /// Gets the unique id for the character.
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueCharacterString()
        {
            return Game1.player.Name + "_" + Game1.player.UniqueMultiplayerID;
        }


        public static bool CanPlayerInventoryReceiveThisItem(Item I)
        {
            for(int i = 0; i < Game1.player.Items.Count; i++)
            {
                if (I == Game1.player.Items[i]) return true;
                if (Game1.player.Items[i] == null) return true;
                if (I.canStackWith(Game1.player.Items[i])) return true;
            }
            return false;

        }
    }
}
