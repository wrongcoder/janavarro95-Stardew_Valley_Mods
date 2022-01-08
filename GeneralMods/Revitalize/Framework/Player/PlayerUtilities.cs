using System.Linq;
using StardewValley;

namespace Revitalize.Framework.Player
{
    public class PlayerUtilities
    {
        /// <summary>
        /// Checks to see if the player being played is the farmhand.
        /// </summary>
        /// <returns></returns>
        public static bool PlayerIsFarmHand()
        {
            return Game1.getAllFarmhands().Select(farmer => farmer.uniqueMultiplayerID.Value.Equals(Game1.player.uniqueMultiplayerID.Value)).Any(); 
        }

    }
}
