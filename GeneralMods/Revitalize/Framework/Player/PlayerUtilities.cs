using System.Linq;
using StardewValley;

namespace Omegasis.Revitalize.Framework.Player
{
    public class PlayerUtilities
    {

        /// <summary>
        /// Checks to see if the farmer team has completed a given special order.
        /// See Data/SpecialOrders and Strings/SpecialOrderStrings in the StardewValley Content folder for more info.
        /// </summary>
        /// <param name="SpecialOrderName">This is the name of the special order entry object, not the display name of the special order.</param>
        /// <returns></returns>
        public static bool HasCompletedSpecialOrder(string SpecialOrderName)
        {
            bool hasCompletedSpecialRequest = Game1.player.team.completedSpecialOrders.ContainsKey(SpecialOrderName);
            return Game1.player.team.completedSpecialOrders.ContainsKey(SpecialOrderName);
        }

        /// <summary>
        /// Checks to see if the special order for Robin's hardwood donation has been completed.
        /// </summary>
        /// <returns></returns>
        public static bool HasCompletedHardwoodDonationSpecialOrderForRobin()
        {
            return HasCompletedSpecialOrder("Robin");
        }

    }
}
