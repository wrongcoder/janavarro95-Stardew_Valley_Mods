using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents;

namespace StardewSymphonyRemastered.Framework.Menus
{
    public class MusicManagerMenu : IClickableMenuExtended
    {
        private string musicNotePath;

        public MusicManagerMenu(float width, float height)
        {
            


            this.width = (int)width;
            this.height = (int)height;
            this.texturedStrings = new List<StardustCore.UIUtilities.SpriteFonts.Components.TexturedString>();
            this.texturedStrings.Add(StardustCore.UIUtilities.SpriteFonts.SpriteFont.vanillaFont.ParseString("Hello", new Microsoft.Xna.Framework.Vector2(100, 100),StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.LightColorsList.Blue)));
            this.buttons = new List<StardustCore.UIUtilities.MenuComponents.Button>();
            this.buttons.Add(new Button("myButton", new Rectangle(100, 100, 200, 100), StardewSymphony.textureManager.getTexture("MusicNote").Copy(StardewSymphony.ModHelper), "mynote", new Rectangle(0, 0, 16, 16), 4f, new StardustCore.Animations.Animation(new Rectangle(0, 0, 16, 16)), Color.White, Color.White, false));
           
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {

        }

        public override void drawBackground(SpriteBatch b)
        {
            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);
        }

        public override void draw(SpriteBatch b)
        {
            this.drawBackground(b);

            foreach(var v in texturedStrings)
            {
                v.draw(b);
            }

            foreach (var v in buttons)
            {
                v.draw(b);
            }
            this.drawMouse(b);
        }
    }
}
