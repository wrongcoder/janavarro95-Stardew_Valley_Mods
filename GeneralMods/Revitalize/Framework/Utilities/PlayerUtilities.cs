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
    }
}
