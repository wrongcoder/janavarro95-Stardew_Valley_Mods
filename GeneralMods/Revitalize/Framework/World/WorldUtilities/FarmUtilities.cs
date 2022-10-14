using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities
{
    /// <summary>
    /// Utilities that deals with the player's farm.
    /// </summary>
    public static class FarmUtilities
    {

        /// <summary>
        /// Gets how much more hay the silos on the farm can store.
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfHayPiecesUntilFullSilos()
        {
            return Utility.numSilos() * 240 - (int)Game1.getFarm().piecesOfHay.Value;
        }

        /// <summary>
        /// Gets the maximum number of hay pieces that can be bought to fill the silos.
        /// </summary>
        /// <param name="PricePerHayPiece"></param>
        /// <returns></returns>
        public static int GetNumberOfHayPiecesUntilFullSilosLimitByPlayersMoney(int PricePerHayPiece)
        {
            int maxHay = GetNumberOfHayPiecesUntilFullSilos();
            int money = Game1.player.Money;

            int maxTotalHayPrice = maxHay * PricePerHayPiece;
            if (maxTotalHayPrice > money)
            {
                maxHay = money / PricePerHayPiece;
            }

            return maxHay;
        }

        /// <summary>
        /// Actually refils the silos on the farm from the silo refil item.
        /// </summary>
        /// <param name="PricePerHayPiece"></param>
        public static void FillSilosFromSiloReillItem(int PricePerHayPiece)
        {
            Game1.getFarm().tryToAddHay(GetNumberOfHayPiecesUntilFullSilosLimitByPlayersMoney(PricePerHayPiece));
        }

    }
}
