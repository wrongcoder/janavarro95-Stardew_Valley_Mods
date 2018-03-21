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
using StardustCore.UIUtilities.MenuComponents.Delegates;
using StardustCore.UIUtilities.MenuComponents.Delegates.Functionality;

namespace StardewSymphonyRemastered.Framework.Menus
{
    public class MusicManagerMenu : IClickableMenuExtended
    {

        public MusicManagerMenu(float width, float height)
        {
            this.width = (int)width;
            this.height = (int)height;
            this.texturedStrings = new List<StardustCore.UIUtilities.SpriteFonts.Components.TexturedString>();
            this.texturedStrings.Add(StardustCore.UIUtilities.SpriteFonts.SpriteFont.vanillaFont.ParseString("Hello", new Microsoft.Xna.Framework.Vector2(100, 100),StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.LightColorsList.Blue)));
            this.buttons = new List<StardustCore.UIUtilities.MenuComponents.Button>();
            //this.buttons.Add(new Button("myButton", new Rectangle(100, 100, 64, 64), StardewSymphony.textureManager.getTexture("MusicNote").Copy(StardewSymphony.ModHelper), "mynote", new Rectangle(0, 0, 16, 16), 4f, new StardustCore.Animations.Animation(new Rectangle(0, 0, 16, 16)), Color.White, Color.White,new ButtonFunctionality(new DelegatePairing(hello,null),null,null),false)); //A button that does nothing on the left click.  

            int numOfButtons = 0;
            int rows = 0;
            foreach(var v in StardewSymphony.musicManager.musicPacks)
            {
                if (v.Value.musicPackInformation.Icon == null)
                {
                    this.buttons.Add(new Button(v.Key, new Rectangle(100 + (numOfButtons * 100), 125 + (rows * 100), 64, 64), StardewSymphony.textureManager.getTexture("MusicDisk"), "", new Rectangle(0, 0, 16, 16), 4f, new StardustCore.Animations.Animation(new Rectangle(0, 0, 16, 16)), StardustCore.IlluminateFramework.Colors.randomColor(), Color.White, new ButtonFunctionality(new DelegatePairing(displayMusicPack, new List<object>
                    {
                        (object)v
                    }
                    ), null, new DelegatePairing(null, new List<object>(){
                    (object)v
                    }
                    )), false));
                }
                else
                {
                    this.buttons.Add(new Button(v.Key, new Rectangle(100 + (numOfButtons * 100), 100 + (rows * 100), 64, 64), v.Value.musicPackInformation.Icon, "", new Rectangle(0, 0, 16, 16), 4f, new StardustCore.Animations.Animation(new Rectangle(0, 0, 16, 16)), StardustCore.IlluminateFramework.LightColorsList.Black, Color.White, new ButtonFunctionality(new DelegatePairing(displayMusicPack, new List<object>
                    {
                        (object)v
                    }
                    ), null, new DelegatePairing(null, new List<object>(){
                    (object)v
                    }
                    )), false));
                }

                numOfButtons++;
                if (numOfButtons > 8)
                {
                    numOfButtons = 0;
                    rows++;
                }
            }
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            foreach (var v in this.buttons)
            {
                if (v.containsPoint(x, y)) v.onRightClick();
            }
        }

        public override void performHoverAction(int x, int y)
        {
            foreach(var v in this.buttons)
            {
                if (v.containsPoint(x, y))
                {
                    var pair = (KeyValuePair<string, MusicPack>)v.buttonFunctionality.hover.paramaters[0];
                    v.hoverText = (string)pair.Key;
                    v.onHover();
                    //StardewSymphony.ModMonitor.Log(pair.Key);
                }
                else
                {
                    v.hoverText = "";
                }
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            foreach (var v in this.buttons)
            {
                if (v.containsPoint(x, y)) v.onLeftClick();
            }
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

        //Button Functionality
        #region
        private void hello(List<object> param)
        {
            StardewSymphony.ModMonitor.Log("Hello");
        }

        public void displayMusicPack(List<object> param)
        {
            var info=(KeyValuePair<string, MusicPack>)param[0];
            StardewSymphony.ModMonitor.Log(info.ToString());
            StardewSymphony.musicManager.swapMusicPacks(info.Key);
            StardewSymphony.musicManager.playRandomSongFromPack(info.Key);
            //info.Value.playRandomSong();
        }

        #endregion
    }
}
