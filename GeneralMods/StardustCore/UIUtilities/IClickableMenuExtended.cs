using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.UIUtilities
{
    public class IClickableMenuExtended : StardewValley.Menus.IClickableMenu
    {
        public List<StardustCore.UIUtilities.SpriteFonts.Components.TexturedString> texturedStrings;
        public List<StardustCore.UIUtilities.MenuComponents.Button> buttons;

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            
        }
    }
}
