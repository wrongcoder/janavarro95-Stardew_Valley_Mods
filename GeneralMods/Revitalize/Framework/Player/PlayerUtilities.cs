using System.Linq;
using Omegasis.Revitalize.Framework.Constants;
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

        public static bool InventoryContainsItem(this Farmer Who, Enums.SDVObject obj)
        {
            return InventoryContainsItem(Who, (int)obj);
        }
        public static bool InventoryContainsEnoughOfAnItem(this Farmer Who, Enums.SDVObject obj, int MinStackSize=1)
        {
            return InventoryContainsEnoughOfAnItem(Who, (int)obj,MinStackSize);
        }

        public static bool ReduceInventoryItemIfEnoughFound(this Farmer Who, Enums.SDVObject obj, int MinStackSize)
        {
            if (InventoryContainsEnoughOfAnItem(Who, obj, MinStackSize))
            {
                return ReduceInventoryItemStackSize(Who, obj, MinStackSize);
            }
            return false;
        }

        public static bool InventoryContainsItem(this Farmer Who,int ParentSheetIndex)
        {
            if (Who != null)
            {
                for (int i = 0; i < Who.MaxItems; i++)
                {

                    //Find the first empty index in the player's inventory.
                    if (Who.items[i] == null)
                    {
                        continue;
                    }
                    //Check to see if the items can stack. If they can simply add them together and then continue on.
                    if (Who.items[i].ParentSheetIndex==ParentSheetIndex)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool ReduceInventoryItemStackSize(this Farmer Who, Enums.SDVObject ParentSheetIndex, int StackSize=1)
        {
            return ReduceInventoryItemStackSize(Who, (int)ParentSheetIndex, StackSize);
        }

        public static bool ReduceInventoryItemStackSize(this Farmer Who, int ParentSheetIndex, int StackSizeToReduce=1)
        {
            if (Who != null)
            {
                for (int i = 0; i < Who.MaxItems; i++)
                {

                    //Find the first empty index in the player's inventory.
                    if (Who.items[i] == null)
                    {
                        continue;
                    }
                    //Check to see if the items can stack. If they can simply add them together and then continue on.
                    if (Who.items[i].ParentSheetIndex == ParentSheetIndex)
                    {
                        Who.items[i].Stack -= StackSizeToReduce;

                        if (Who.items[i].Stack <= 0) Who.items[i] = null;
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool InventoryContainsEnoughOfAnItem(this Farmer Who, int ParentSheetIndex, int MinStackSize=1)
        {
            if (Who != null)
            {
                for (int i = 0; i < Who.MaxItems; i++)
                {

                    //Find the first empty index in the player's inventory.
                    if (Who.items[i] == null)
                    {
                        continue;
                    }
                    //Check to see if the items can stack. If they can simply add them together and then continue on.
                    if (Who.items[i].ParentSheetIndex == ParentSheetIndex && Who.items[i].Stack>=MinStackSize)
                    {
                        return true;
                    }
                }
            }
            return false;
        }



    }
}
