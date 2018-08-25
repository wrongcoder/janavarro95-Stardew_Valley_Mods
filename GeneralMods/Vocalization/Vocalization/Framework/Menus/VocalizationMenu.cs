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

    /// <summary>
    /// TODO:
    /// Make Ok button that saves settings and closes menu instead  of readyToCloseFunction()
    /// Make Cyclic buttons(aka a button that holds a ton of buttons)
    /// Make cyclic translation button. (english, spanish, etc)
    /// Make cyclic audio modes button. (full, simple, etc)
    /// </summary>
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
            Button bar = new Button(new Rectangle(this.xPositionOnScreen + 100, this.yPositionOnScreen + 220, 200, 40), new Texture2DExtended(Vocalization.ModHelper, Vocalization.Manifest, Path.Combine("Content", "Graphics", "SliderBar.png")),new Rectangle(0,0,100,10),2f);
            //Texture2DExtended barTexture = new Texture2DExtended(Vocalization.ModHelper, Vocalization.Manifest, Path.Combine("Content", "Graphics", "SliderBar.png"));
            Rectangle sourceRect = new Rectangle(0, 0, 4, 16);
            this.sliderButton = new SliderButton("Slider", "Volume", new Rectangle(this.xPositionOnScreen+100, this.yPositionOnScreen+220, 4, 16), buttonTexture, bar, sourceRect, 2f, new SliderInformation(SliderStyle.Horizontal, (int)(Vocalization.config.voiceVolume*100), 1), new StardustCore.Animations.Animation(sourceRect), Color.White, Color.Black, new StardustCore.UIUtilities.MenuComponents.Delegates.Functionality.ButtonFunctionality(null, null, null), false, null, true);
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
            this.drawOnlyDialogueBoxBackground(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, new Color(255, 255, 255, 255),0.4f);
            sliderButton.draw(b,Color.White,Vector2.Zero,0.5f);
        }

        /// <summary>
        /// Save the menu information upon menu being closed.
        /// </summary>
        /// <returns></returns>
        public override bool readyToClose()
        {
            Vocalization.ModMonitor.Log(sliderButton.sliderInformation.xPos.ToString());
            decimal xPos = sliderButton.sliderInformation.xPos;
            Vocalization.config.voiceVolume = (decimal)(xPos/ 100.0M);
            Vocalization.ModMonitor.Log(Vocalization.config.voiceVolume.ToString());
            Vocalization.ModHelper.WriteConfig<ModConfig>(Vocalization.config);
            Vocalization.soundManager.volume =(float) Vocalization.config.voiceVolume;

            if (Vocalization.config.translationInfo.currentTranslation != getTranslationInfo())
            {
                //Change the mod config translation info
                //Clear out the Sound manager sounds and Vocalization Dialogue Cues
                //Reload all of the dialogue files
            }

            if (Vocalization.config.currentMode != getAudioMode())
            {
                Vocalization.config.currentMode = getAudioMode();
            }


            return true;
        }

        public string getTranslationInfo()
        {
            //Return the name of the button which will have the translation stuff here!
            return "English";
        }

        public string getAudioMode()
        {
            //Return the name of the mode that the current mode button is selected on.
            return "Full";
        }


    }
}
