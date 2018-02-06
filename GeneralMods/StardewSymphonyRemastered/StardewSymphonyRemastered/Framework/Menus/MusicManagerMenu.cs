using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace StardewSymphonyRemastered.Framework.Menus
{
    class MusicManagerMenu : StardewValley.Menus.IClickableMenu
    {
        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            throw new NotImplementedException();
        }

        public override void drawBackground(SpriteBatch b)
        {
            base.drawBackground(b);
        }
    }
}
