using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.Revitalize.Framework.HUD
{
    public static class HudUtilities
    {
        /// <summary>
        /// Shows the error message for the game's hud to say that the player's inventory is full.
        /// </summary>
        public static void ShowInventoryFullErrorMessage()
        {
            Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
        }
    }
}
