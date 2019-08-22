using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using SObject = StardewValley.Object;
namespace Revitalize.Framework.Hacks
{
    public class ObjectInteractionHacks
    {

        /// <summary>
        /// Returns the object underneath the mouse's position.
        /// </summary>
        /// <returns></returns>
        public static SObject GetItemAtMouseTile()
        {
            Vector2 mouseTilePosition = Game1.currentCursorTile;
            if (Game1.player.currentLocation.isObjectAtTile((int)mouseTilePosition.X, (int)mouseTilePosition.Y))
            {
                return Game1.player.currentLocation.getObjectAtTile((int)mouseTilePosition.X, (int)mouseTilePosition.Y);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Checks to see if the given object is a SDV vanilla furnace.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsObjectFurnace(SObject obj)
        {
            if (obj.ParentSheetIndex == 13 && obj.bigCraftable.Value && obj.Category == -9 && obj.Name == "Furnace")
            {
                return true;
            }
            else return false;
        }

        public static void Input_CheckForObjectInteraction(object sender, StardewModdingAPI.Events.ButtonPressedEventArgs e)
        {
            if(e.Button== StardewModdingAPI.SButton.MouseRight)
            {
                SObject obj= GetItemAtMouseTile();
            }
        }

    }
}
