using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocalization.Framework.Menus
{
    public class VocalizationMenu: IClickableMenuExtended
    {
        SliderButton sliderButton;


        public VocalizationMenu(int xPos, int yPos, int width, int height, bool showCloseButton = false) : base(xPos, yPos, width, height,showCloseButton)
        {
            this.xPositionOnScreen = xPos;
            this.yPositionOnScreen = yPos;
            this.width = width;
            this.height = height;
            this.showRightCloseButton = showCloseButton;
            setUpButtons();

        }

        public void setUpButtons()
        {
            Texture2DExtended buttonTexture = new Texture2DExtended(Vocalization.ModHelper, Vocalization.Manifest, Path.Combine("Content", "Graphics", "SliderButton.png"));
            Button bar = new Button(new Rectangle(this.xPositionOnScreen + 100, this.yPositionOnScreen + 220, 100, 20), new Texture2DExtended(Vocalization.ModHelper, Vocalization.Manifest, Path.Combine("Content", "Graphics", "SliderBar.png")),new Rectangle(0,0,100,10),2f);
            //Texture2DExtended barTexture = new Texture2DExtended(Vocalization.ModHelper, Vocalization.Manifest, Path.Combine("Content", "Graphics", "SliderBar.png"));
            Rectangle sourceRect = new Rectangle(0, 0, 8, 16);
            this.sliderButton = new SliderButton("Slider", "Volume", new Rectangle(this.xPositionOnScreen+100, this.yPositionOnScreen+220, 8, 16), buttonTexture, bar, sourceRect, 2f, new SliderInformation(SliderStyle.Horizontal, 100, 1), new StardustCore.Animations.Animation(sourceRect), Color.White, Color.Black, new StardustCore.UIUtilities.MenuComponents.Delegates.Functionality.ButtonFunctionality(null, null, null), false, null, true);
        }


        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            this.sliderButton.onLeftClick(x, y);
            base.receiveLeftClick(x, y, playSound);
        }

        public override void leftClickHeld(int x, int y)
        {
            this.sliderButton.onLeftClickHeld(x, y);
            base.leftClickHeld(x, y);
        }

        public override IClickableMenuExtended clone()
        {
            Vocalization.ModMonitor.Log("Cloning with position: " + xPositionOnScreen.ToString());
            return new VocalizationMenu(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, this.showRightCloseButton);
        }

        public override void draw(SpriteBatch b)
        {
            this.drawDialogueBoxBackground(this.xPositionOnScreen,this.yPositionOnScreen,this.width,this.height);

            sliderButton.draw(b,Color.White,Vector2.Zero,0.5f);

            Vocalization.ModMonitor.Log(this.xPositionOnScreen.ToString()+" "+this.yPositionOnScreen.ToString());
        }


    }
}
