using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame.SSCMenus
{
    public class TitleScreen: StardustCore.UIUtilities.IClickableMenuExtended
    {
        StardustCore.UIUtilities.Texture2DExtended background;
        StardustCore.UIUtilities.MenuComponents.BlinkingText menuText;
        public TitleScreen(int x, int y, int width, int height):base(x,y,width,height,false)
        {
            this.background = SeasideScramble.self.textureUtils.getExtendedTexture("SSCMaps", "TitleScreenBackground");
            this.menuText = new StardustCore.UIUtilities.MenuComponents.BlinkingText("Sea Side Scramble: Lite Edition" + System.Environment.NewLine + "Click to start.",1000);
        }

        public TitleScreen(xTile.Dimensions.Rectangle viewport) : this(0, 0, viewport.Width, viewport.Height)
        {

        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            this.xPositionOnScreen = newBounds.X;
            this.yPositionOnScreen = newBounds.Y;
            this.width = newBounds.Width;
            this.height = newBounds.Height;
            base.gameWindowSizeChanged(oldBounds, newBounds);
        }



        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            //Start the game!
        }

        public override bool readyToClose()
        {
            //When menu is closed!
            return false;
        }

        public override void update(GameTime time)
        {
            this.menuText.update(time);   
        }

        public override void draw(SpriteBatch b)
        {
            b.GraphicsDevice.Clear(Color.Black);
            this.drawTitleBackground(b);
            this.drawTitleText(b);
            this.drawMouse(b);
        }

        public void drawTitleBackground(SpriteBatch b)
        {
            b.Draw(this.background.texture,new Vector2(this.xPositionOnScreen,this.yPositionOnScreen),SeasideScramble.self.camera.getXNARect() ,Color.White);
            //this.drawDialogueBoxBackground(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, Color.Black);
        }

        public void drawTitleText(SpriteBatch b)
        {
            Vector2 offset=StardewValley.Game1.dialogueFont.MeasureString(this.menuText.displayText);
            this.menuText.draw(b, StardewValley.Game1.dialogueFont, new Vector2((this.width / 2) - (offset.X / 2), this.height / 2), Color.White);
           
        }

    }
}
