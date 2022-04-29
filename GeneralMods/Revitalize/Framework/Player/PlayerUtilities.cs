using System.Linq;
using StardewValley;

namespace Omegasis.Revitalize.Framework.Player
{
    public static class PlayerUtilities
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

        /// <summary>
        /// Adds an item to the player's inventory by a new slot, or stackable equivalent.
        /// </summary>
        /// <param name="Who"></param>
        /// <param name="I"></param>
        public static bool AddItemToInventory(this Farmer Who, Item I)
        {
            if (Who != null)
            {
                int emptyIndex = -1;
                for (int i = 0; i < Who.MaxItems; i++)
                {

                    //Find the first empty index in the player's inventory.
                    if (Who.items[i] == null && emptyIndex == -1)
                    {
                        emptyIndex = i;
                        continue;
                    }
                    //Check to see if the items can stack. If they can simply add them together and then continue on.
                    if (Who.items[i]!=null && Who.items[i].canStackWith(I))
                    {
                        Who.items[i].Stack += I.Stack;
                        return true;
                    }
                }

                if (emptyIndex != -1)
                {
                    Who.items[emptyIndex] = I;

                    //Set as active toolbar item.
                    if (emptyIndex < 12)
                    {
                        Who.CurrentToolIndex = emptyIndex;
                    }
                    return true;
                }
                return false;
            }
            return false;
        }

    }
}
