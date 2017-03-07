using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using Compatability;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MenuControllerCompatability
{
    public class Class1 : Mod
    {

        public override void Entry(IModHelper helper)
        {
            StardewModdingAPI.Events.GameEvents.UpdateTick += MenuCompatability;
            StardewModdingAPI.Events.GraphicsEvents.Resize += GraphicsEvents_Resize;
        }

        private void GraphicsEvents_Resize(object sender, EventArgs e)
        {
            if (CompatabilityManager.compatabilityMenu != null)
            {
                CompatabilityManager.compatabilityMenu.resize();
            }
        }

        private void MenuCompatability(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu == null) return;
            //Log.AsyncC(Game1.activeClickableMenu.GetType());
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            if (currentState.IsConnected == false||Game1.options.gamepadControls==false) return;
            // if (Game1.options.gamepadControls == false && useMenuFocus==false) return;
            if (CompatabilityManager.doUpdate == true)
            {
                CompatabilityManager.compatabilityMenu.Compatability();
                CompatabilityManager.compatabilityMenu.Update();
                return;
            }
            if (Game1.activeClickableMenu is StardewValley.Menus.TitleMenu && CompatabilityManager.characterCustomizer == false)
            {
                if (CompatabilityManager.doUpdate == false)
                {
                    CompatabilityManager.compatabilityMenu = new Compatability.Vanilla.TitleMenu();
                    CompatabilityManager.doUpdate = true;
                }

            }
            if (Game1.activeClickableMenu is StardewValley.Menus.TitleMenu && CompatabilityManager.characterCustomizer == true)
            {
                // compatabilityMenu = new Menus.Compatability.Vanilla.TitleMenu();
                //  compatabilityMenu.Compatability();
            }
            if (Game1.activeClickableMenu is StardewValley.Menus.TitleMenu && CompatabilityManager.loadMenu == true)
            {
                // compatabilityMenu = new Menus.Compatability.Vanilla.TitleMenu();
                //  compatabilityMenu.Compatability();
            }
            if (Game1.activeClickableMenu is StardewValley.Menus.TitleMenu && CompatabilityManager.aboutMenu == true)
            {
          //      Log.AsyncO("BOOOO");
                CompatabilityManager.compatabilityMenu = new Compatability.Vanilla.AboutMenu();
                CompatabilityManager.doUpdate = true;
                //  compatabilityMenu.Compatability();
            }
            else
            {
                // compatabilityMenu = null;
            }

        }


    }
}
