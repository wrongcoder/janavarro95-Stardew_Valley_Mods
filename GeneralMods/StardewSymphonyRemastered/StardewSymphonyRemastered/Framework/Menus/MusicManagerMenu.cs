using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace StardewSymphonyRemastered.Framework.Menus
{
    class MusicManagerMenu : StardewValley.Menus.IClickableMenu
    {
        public List<StardustCore.UIUtilities.SpriteFonts.Components.TexturedString> texturedStrings;
        public MusicManagerMenu(float width, float height)
        {
            this.width = (int)width;
            this.height = (int)height;
            this.texturedStrings = new List<StardustCore.UIUtilities.SpriteFonts.Components.TexturedString>();
            this.texturedStrings.Add(StardustCore.UIUtilities.SpriteFonts.SpriteFont.vanillaFont.ParseString("Hello", new Microsoft.Xna.Framework.Vector2(100, 100),StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.LightColorsList.Blue)));
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            throw new NotImplementedException();
        }

        public override void drawBackground(SpriteBatch b)
        {
            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);
            //base.drawBackground(b);
        }

        public override void draw(SpriteBatch b)
        {
            this.drawBackground(b);

            foreach(var v in texturedStrings)
            {
                v.draw(b);
            }
        }
    }
}
