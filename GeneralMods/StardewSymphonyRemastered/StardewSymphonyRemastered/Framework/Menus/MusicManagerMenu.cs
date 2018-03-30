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
using StardustCore.UIUtilities.SpriteFonts;
using StardustCore.UIUtilities.SpriteFonts.Components;

namespace StardewSymphonyRemastered.Framework.Menus
{

    public class MusicManagerMenu : IClickableMenuExtended
    {
        public enum DrawMode
        {
            AlbumSelection,
            SongSelection,
            AlbumFancySelection,
        }

        public List<Button> musicAlbumButtons;
        public MusicPack currentMusicPack;
        public DrawMode drawMode;
        public int currentAlbumIndex;

        public List<Button> fancyButtons;
        public int framesSinceLastUpdate;

        public bool searchBoxSelected;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public MusicManagerMenu()
        {

        }

        public MusicManagerMenu(float width, float height)
        {
            this.width = (int)width;
            this.height = (int)height;
            this.texturedStrings = new List<StardustCore.UIUtilities.SpriteFonts.Components.TexturedString>();
            this.musicAlbumButtons = new List<StardustCore.UIUtilities.MenuComponents.Button>();
            //thismusicAlbumButtons.Add(new Button("myButton", new Rectangle(100, 100, 64, 64), StardewSymphony.textureManager.getTexture("MusicNote").Copy(StardewSymphony.ModHelper), "mynote", new Rectangle(0, 0, 16, 16), 4f, new StardustCore.Animations.Animation(new Rectangle(0, 0, 16, 16)), Color.White, Color.White,new ButtonFunctionality(new DelegatePairing(hello,null),null,null),false)); //A button that does nothing on the left click.  

            fancyButtons = new List<Button>();
            


            int numOfButtons = 0;
            int rows = 0;
            foreach(var v in StardewSymphony.musicManager.musicPacks)
            {
                if (v.Value.musicPackInformation.Icon == null)
                {
                    Texture2DExtended texture = StardewSymphony.textureManager.getTexture("MusicDisk");
                    float scale = 1.00f / ((float)texture.texture.Width / 64f);
       
                    this.musicAlbumButtons.Add(new Button(v.Key, new Rectangle(100 + (numOfButtons * 100), 125 + (rows * 100), 64, 64),texture, "", new Rectangle(0, 0, 16, 16), scale, new StardustCore.Animations.Animation(new Rectangle(0, 0, 16, 16)), StardustCore.IlluminateFramework.Colors.randomColor(), Color.White,new ButtonFunctionality(new DelegatePairing(PlayRandomSongFromSelectedMusicPack, new List<object>
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
                    float scale = 1.00f / ((float)v.Value.musicPackInformation.Icon.texture.Width / 64f);
                    this.musicAlbumButtons.Add(new Button(v.Key, new Rectangle(100 + (numOfButtons * 100), 125 + (rows * 100), 64, 64), v.Value.musicPackInformation.Icon, "", new Rectangle(0, 0, v.Value.musicPackInformation.Icon.texture.Width, v.Value.musicPackInformation.Icon.texture.Height), scale, new StardustCore.Animations.Animation(new Rectangle(0, 0, 16, 16)), StardustCore.IlluminateFramework.LightColorsList.Black, StardustCore.IlluminateFramework.LightColorsList.Black, new ButtonFunctionality(new DelegatePairing(PlayRandomSongFromSelectedMusicPack, new List<object>
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

            if (Game1.timeOfDay < 1200) this.dialogueBoxBackgroundColor = Color.SpringGreen;
            if (Game1.timeOfDay >= 1200&& Game1.timeOfDay < 1800) this.dialogueBoxBackgroundColor = Color.White;
            if (Game1.timeOfDay >= 1800) this.dialogueBoxBackgroundColor = Color.DarkViolet;


            this.currentAlbumIndex = 0;
            this.drawMode = DrawMode.AlbumFancySelection;

            this.updateFancyButtons();
            this.framesSinceLastUpdate = 0;

            this.searchBoxSelected = false;
            this.menuTextures = new List<Texture2DExtended>();


    }

        public override void update(GameTime time)
        {

            if (framesSinceLastUpdate == 20)
            {
                var state = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left) || state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                {
                    this.currentAlbumIndex--;
                    if (this.currentAlbumIndex < 0) this.currentAlbumIndex = this.musicAlbumButtons.Count - 1;
                    this.updateFancyButtons();
                    this.framesSinceLastUpdate = 0;
                    Game1.playSound("shwip");
                }

                if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right) || state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                {
                    this.currentAlbumIndex++;
                    if (this.currentAlbumIndex == this.musicAlbumButtons.Count) this.currentAlbumIndex = 0;
                    this.updateFancyButtons();
                    this.framesSinceLastUpdate = 0;
                    Game1.playSound("shwip");
                }
            }
            else
            {
                this.framesSinceLastUpdate++;
            }
        }

        public virtual void updateFancyButtons()
        {
            this.fancyButtons.Clear();
            Vector4 placement = new Vector4((Game1.viewport.Width / 3), (Game1.viewport.Height / 4)+128, this.width, this.height / 2);
            //generate buttons
            int offsetX = 200;
            if (this.musicAlbumButtons.Count > 0)
            {
                for (int i = -3; i < 4; i++)
                {
                    try
                    {
                        Button button = this.musicAlbumButtons.ElementAt(this.currentAlbumIndex + i).clone();
                        button.bounds = new Rectangle((int)placement.X + (i * 100)+offsetX, (int)placement.Y, 64, 64);
                        fancyButtons.Add(button);
                    }
                    catch (Exception err)
                    {
                        if (this.currentAlbumIndex + i == 0)
                        {
                            Button button = this.musicAlbumButtons.ElementAt(0).clone();
                            button.bounds = new Rectangle((int)placement.X + (i * 100) + offsetX, (int)placement.Y,64, 64);
                            fancyButtons.Add(button);
                        }
                        else {

                            try
                            {
                                Button button = this.musicAlbumButtons.ElementAt(((this.currentAlbumIndex + i) - this.musicAlbumButtons.Count)%this.musicAlbumButtons.Count).clone();
                                button.bounds = new Rectangle((int)placement.X + (i * 100) + offsetX, (int)placement.Y, 64, 64);
                                fancyButtons.Add(button);
                            }
                            catch (Exception err2)
                            {
                                
                                    Button button = this.musicAlbumButtons.ElementAt(((this.currentAlbumIndex + i) + this.musicAlbumButtons.Count) % this.musicAlbumButtons.Count).clone();
                                    button.bounds = new Rectangle((int)placement.X + (i * 100) + offsetX, (int)placement.Y, 64, 64);
                                    fancyButtons.Add(button);
                                
                            }
                        }
                    }
                }
                this.fancyButtons.Add(new Button("Outline", new Rectangle((int)placement.X + offsetX-16, (int)placement.Y-16, 64, 64), StardewSymphony.textureManager.getTexture("OutlineBox"), "", new Rectangle(0, 0, 16, 16), 6f, new StardustCore.Animations.Animation(new Rectangle(0, 0, 16, 16)), Color.White, Color.White, new ButtonFunctionality(null, null, new DelegatePairing(null,new List<object>())), false));
                int count = 0;
                foreach (var v in fancyButtons)
                {

                    if (count == 3)
                    {
                        var pair = (KeyValuePair<string, MusicPack>)fancyButtons.ElementAt(count).buttonFunctionality.hover.paramaters[0];
                        //v.hoverText = (string)pair.Key;
                        //Do something like current album name =
                        this.texturedStrings.Clear();
                        this.texturedStrings.Add(SpriteFonts.vanillaFont.ParseString("Current Album Name:" + (string)pair.Key, new Microsoft.Xna.Framework.Vector2(v.bounds.X / 2, v.bounds.Y + 128), v.textColor));
                        v.hoverText = "";
                    }
                    count++;
                }
            }
        }


        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            if (this.drawMode == DrawMode.AlbumSelection)
            {
                foreach (var v in this.musicAlbumButtons)
                {
                    if (v.containsPoint(x, y)) v.onRightClick();
                }
            }

            if (this.drawMode == DrawMode.AlbumFancySelection)
            {
                int count = 0;
                foreach (var v in this.fancyButtons)
                {
                    count++;
                    //if (v.containsPoint(x, y)) v.onRightClick();
                    //this.currentAlbumIndex += count;
                    //if (this.currentAlbumIndex >= this.musicAlbumButtons.Count) this.currentAlbumIndex -= this.musicAlbumButtons.Count;
                    //this.updateFancyButtons();
                }
            }
        }

        public override void performHoverAction(int x, int y)
        {
            if (this.drawMode == DrawMode.AlbumSelection)
            {
                foreach (var v in this.musicAlbumButtons)
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

            if (this.drawMode == DrawMode.AlbumFancySelection)
            {
                int count = 0;
                foreach (var v in this.fancyButtons)
                {
                    if (v.containsPoint(x, y))
                    {
                        if (v == null)
                        {
                          //  StardewSymphony.ModMonitor.Log("v is null at count: " + count);
                            continue;
                        }
                        if (v.buttonFunctionality == null)
                        {
                           // StardewSymphony.ModMonitor.Log("button functionality is null at count: " + count);
                            continue;
                        }
                        if (v.buttonFunctionality.hover == null)
                        {
                           // StardewSymphony.ModMonitor.Log("hover is null at count: " + count);
                            continue;
                        }
                        if (v.buttonFunctionality.hover.paramaters == null)
                        {
                           // StardewSymphony.ModMonitor.Log("Params are null at count: " + count);
                            continue;
                        }
                        if (v.buttonFunctionality.hover.paramaters.Count==0)
                        {
                            //StardewSymphony.ModMonitor.Log("Params are 0 at count: " + count);
                            continue;
                        }
                        var pair = (KeyValuePair<string, MusicPack>)v.buttonFunctionality.hover.paramaters[0];
                        v.hoverText = (string)pair.Key;
                        //if (v.buttonFunctionality.hover != null) v.buttonFunctionality.hover.run();
                        //StardewSymphony.ModMonitor.Log(pair.Key);
                        v.onHover();
                        //StardewSymphony.ModMonitor.Log(pair.Key);
                    }
                    else
                    {

                            v.hoverText = "";
                    }
                    count++;
                }

                
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (this.drawMode == DrawMode.AlbumSelection) {
                foreach (var v in this.musicAlbumButtons)
                {
                    if (v.containsPoint(x, y)) v.onLeftClick();
                }
            }

            if (this.drawMode == DrawMode.AlbumFancySelection)
            {
                int count = 0;
                foreach (var v in this.fancyButtons)
                {
                    if (v.containsPoint(x, y) && v.buttonFunctionality.leftClick != null)
                    {
                        v.onLeftClick();
                        this.currentAlbumIndex += count - 3;
                        //if (this.currentAlbumIndex >= this.musicAlbumButtons.Count) this.currentAlbumIndex -= (this.musicAlbumButtons.Count);
                        StardewSymphony.ModMonitor.Log(this.currentAlbumIndex.ToString());
                    }
                    if (v.buttonFunctionality.leftClick != null)
                    {
                        count++;
                    }
                }
                while(currentAlbumIndex < 0) { 
                        this.currentAlbumIndex = (this.musicAlbumButtons.Count - (this.currentAlbumIndex*-1));
            }
                this.updateFancyButtons();
                }
        }


        public override void draw(SpriteBatch b)
        {
            if (this.drawMode == DrawMode.AlbumSelection)
            {
                this.drawDialogueBoxBackground();
                foreach (var v in this.musicAlbumButtons)
                {
                    v.draw(b);
                }
            }

            if (this.drawMode == DrawMode.AlbumFancySelection)
            {
                Vector4 placement = new Vector4(Game1.viewport.Width/4-50, Game1.viewport.Height/4, 8 * 100, 128*2);
                this.drawDialogueBoxBackground((int)placement.X, (int)placement.Y, (int)placement.Z, (int)placement.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 0)));
               
                foreach(var v in fancyButtons)
                {
                    v.draw(b);
                }
                foreach (var v in this.texturedStrings)
                {
                    v.draw(b);
                }
            }
            this.drawMouse(b);
        }

        //Button Functionality
        #region
        private void hello(List<object> param)
        {
            StardewSymphony.ModMonitor.Log("Hello");
        }

        public void PlayRandomSongFromSelectedMusicPack(List<object> param)
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
