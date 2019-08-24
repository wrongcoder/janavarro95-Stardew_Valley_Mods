using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Revitalize.Framework.Crafting;
using Revitalize.Framework.Utilities;
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
            if (Game1.player == null) return null;
            if (Game1.player.currentLocation == null) return null;
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
        /// Checks for right mouse button iteraction with the world.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Input_CheckForObjectInteraction(object sender, StardewModdingAPI.Events.ButtonPressedEventArgs e)
        {
            if(e.Button== StardewModdingAPI.SButton.MouseRight)
            {
                SObject obj= GetItemAtMouseTile();
                if (obj == null) return;
                if (ObjectUtilities.IsObjectFurnace(obj) && ObjectUtilities.IsObjectHoldingItem(obj)==false)
                {
                    bool crafted=VanillaRecipeBook.VanillaRecipes.TryToCraftRecipe(obj);
                    if (crafted == false) return;
                    obj.initializeLightSource((Vector2)(obj.TileLocation), false);
                    obj.showNextIndex.Value = true;
                    /*
                    Game1.multiplayer.broadcastSprites(who.currentLocation, new TemporaryAnimatedSprite(30, this.tileLocation.Value * 64f + new Vector2(0.0f, -16f), Color.White, 4, false, 50f, 10, 64, (float)(((double)this.tileLocation.Y + 1.0) * 64.0 / 10000.0 + 9.99999974737875E-05), -1, 0)
                    {
                        alphaFade = 0.005f
                    });
                    */
                    (Game1.player.currentLocation).TemporarySprites.Add(new TemporaryAnimatedSprite(30, obj.TileLocation * 64f + new Vector2(0.0f, -16f), Color.White, 4, false, 50f, 10, 64, (float)(((double)obj.TileLocation.Y + 1.0) * 64.0 / 10000.0 + 9.99999974737875E-05), -1, 0));
                    obj.addWorkingAnimation(Game1.player.currentLocation);
                }
            }
        }

    }
}
