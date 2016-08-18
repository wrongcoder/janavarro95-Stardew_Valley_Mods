using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Minigames;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
    public class Birthday_Menu : IClickableMenu
    {
        public const int colorPickerTimerDelay = 100;

        public List<int> shirtOptions;

        public List<int> hairStyleOptions;

        public List<int> accessoryOptions;

        public int currentShirt;

        public int currentHair;

        public int currentAccessory;

        public int colorPickerTimer;

        public ColorPicker pantsColorPicker;

        public ColorPicker hairColorPicker;

        public ColorPicker eyeColorPicker;

        public List<ClickableComponent> labels = new List<ClickableComponent>();


        public List<ClickableComponent> seasonButtons = new List<ClickableComponent>();

        public ClickableTextureComponent okButton;

        public ClickableTextureComponent skipIntroButton;

       // public ClickableTextureComponent randomButton;

        public TextBox nameBox;

        public TextBox farmnameBox;

        public TextBox favThingBox;

        public bool skipIntro;

        public bool wizardSource;

        public ColorPicker lastHeldColorPicker;

        public int timesRandom;

        public Birthday_Menu(bool wizardSource = false) : base(Game1.viewport.Width / 2 - (632 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize, 632 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2 + Game1.tileSize, false)
        {
           
            this.wizardSource = wizardSource;
            this.setUpPositions();
            Game1.player.faceDirection(2);
            Game1.player.FarmerSprite.StopAnimation();
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            this.xPositionOnScreen = Game1.viewport.Width / 2 - (632 + IClickableMenu.borderWidth * 2) / 2;
            this.yPositionOnScreen = Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize;
            this.setUpPositions();
        }

        public virtual void setUpPositions()
        {
            this.labels.Clear();
            this.seasonButtons.Clear();
            this.okButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), "OK", null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false, false);
           

        
           // this.randomButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 12, this.yPositionOnScreen + Game1.tileSize + Game1.pixelZoom * 14, Game1.pixelZoom * 10, Game1.pixelZoom * 10), "", "", Game1.mouseCursors, new Rectangle(381, 361, 10, 10), (float)Game1.pixelZoom);
            int num = Game1.tileSize * 2;
            //new ClickableComponent(new Rectangle(x2, y, Game1.tileSize * 3 / 4 - 4, Game1.tileSize * 3 / 4 - 4), string.Concat((object) (index * 5))) //allows you to make numbers into buttons: Taken from elevator menu
            this.labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 8, 1, 1),"Birthday Season: "+ HappyBirthday.Class1.player_birthday_season));
            this.labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize, Game1.tileSize * 2, Game1.tileSize), "Birthday Date: " + HappyBirthday.Class1.player_birthday_date));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 1 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + (int)(Game1.tileSize * 3.25) - Game1.tileSize / 4, Game1.tileSize*2, Game1.tileSize), "Spring", "Spring", Game1.mouseCursors, new Rectangle(188, 438, 32, 9), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + (int)(Game1.tileSize * 3.25) - Game1.tileSize / 4, Game1.tileSize*2, Game1.tileSize), "Summer", "Summer", Game1.mouseCursors, new Rectangle(220, 438, 32, 8), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + (int)(Game1.tileSize * 3.25) - Game1.tileSize / 4, Game1.tileSize*2, Game1.tileSize), "Fall", "Fall", Game1.mouseCursors, new Rectangle(188, 447, 32, 10), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 7 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + (int)(Game1.tileSize * 3.25) - Game1.tileSize / 4, Game1.tileSize*2, Game1.tileSize), "Winter", "Winter", Game1.mouseCursors, new Rectangle(220, 448, 32, 8), (float)Game1.pixelZoom, false, false));


            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 1 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "1", "1", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 2 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "2", "2", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "3", "3", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(24, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "4", "4", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(32, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "5", "5", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(40, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 6 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "6", "6", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(48, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 7 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "7", "7", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(56, 16, 8, 12), (float)Game1.pixelZoom, false, false));

            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 1 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "8", "8", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(64, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 2 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "9", "9", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(72, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize *2.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize /2, Game1.tileSize), "10", "10", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 3.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize/2, Game1.tileSize), "10", "10", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(0, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 3.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "11", "11", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 4.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "11", "11", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 4.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "12", "12", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 5.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "12", "12", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 5.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "13", "13", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 6.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "13", "13", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(24, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 6.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "14", "14", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 7.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "14", "14", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(32, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            // this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4 - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "11", "11", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(32, 16, 8, 12), (float)Game1.pixelZoom, false, false));


            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 0.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "15", "15", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 1.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "15", "15", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(40, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 1.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "16", "16", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 2.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "16", "16", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(48, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 2.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "17", "17", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 3.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "17", "17", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(56, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 3.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "18", "18", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 4.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "18", "18", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(64, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 4.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "19", "19", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 5.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "19", "19", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(72, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 5.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "20", "20", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 6.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "20", "20", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(0, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 6.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "21", "21", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 7.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "21", "21", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), (float)Game1.pixelZoom, false, false));


            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 0.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "22", "22", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 1.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "22", "22", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 1.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "23", "23", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 2.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "23", "23", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(24, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 2.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "24", "24", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 3.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "24", "24", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(32, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 3.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "25", "25", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 4.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "25", "25", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(40, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 4.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "26", "26", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 5.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "26", "26", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(48, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 5.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "27", "27", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 6.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "27", "27", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(56, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 6.75) - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "28", "28", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            this.seasonButtons.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + (int)(Game1.tileSize * 7.25) - Game1.tileSize / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "28", "28", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(64, 16, 8, 12), (float)Game1.pixelZoom, false, false));
            num = Game1.tileSize * 4 + 8;
          
        }

        public virtual void optionButtonClick(string name)
        {
            if (name != null)
            {
                if (name == "Spring")
                {
                    HappyBirthday.Class1.player_birthday_season = "spring";
                    Game1.activeClickableMenu = new Birthday_Menu();
                }
             else   if (name == "Summer")
                {
                    HappyBirthday.Class1.player_birthday_season = "summer";
                    Game1.activeClickableMenu = new Birthday_Menu();
                }
            else    if (name == "Fall")
                {
                    HappyBirthday.Class1.player_birthday_season = "fall";
                    Game1.activeClickableMenu = new Birthday_Menu();
                }
             else   if (name == "Winter")
                {
                    HappyBirthday.Class1.player_birthday_season = "winter";
                    Game1.activeClickableMenu = new Birthday_Menu();
                }
         

              else  if (name == "OK")
                {
                    if (!this.canLeaveMenu())
                    {
                        return;
                    }
                    //Game1.player.Name = this.nameBox.Text.Trim();
                    //  Game1.player.favoriteThing = this.favThingBox.Text.Trim();
                    if (Game1.activeClickableMenu is TitleMenu)
                    {
                        //   (Game1.activeClickableMenu as TitleMenu).createdNewCharacter(this.skipIntro);
                    }
                    else
                    {
                        Game1.exitActiveMenu();
                        if (Game1.currentMinigame != null && Game1.currentMinigame is Intro)
                        {
                            (Game1.currentMinigame as Intro).doneCreatingCharacter();
                        }
                        else if (this.wizardSource)
                        {
                            Game1.flashAlpha = 1f;
                            Game1.playSound("yoba");
                        }
                    }
                }
                else
                {
                    HappyBirthday.Class1.player_birthday_date = Convert.ToInt32(name);
                    Game1.activeClickableMenu = new Birthday_Menu();
                }

            }
            Game1.playSound("coin");
        }

        public virtual void selectionClick(string name, int change)
        {
            if (name != null)
            {
                if (name == "Skin")
                {
                    Game1.player.changeSkinColor(Game1.player.skin + change);
                    Game1.playSound("skeletonStep");
                    return;
                }
                if (name == "Hair")
                {
                    Game1.player.changeHairStyle(Game1.player.hair + change);
                    Game1.playSound("grassyStep");
                    return;
                }
                if (name == "Shirt")
                {
                    Game1.player.changeShirt(Game1.player.shirt + change);
                    Game1.playSound("coin");
                    return;
                }
                if (name == "Acc")
                {
                    Game1.player.changeAccessory(Game1.player.accessory + change);
                    Game1.playSound("purchase");
                    return;
                }
                if (!(name == "Direction"))
                {
                    return;
                }
                Game1.player.faceDirection((Game1.player.facingDirection - change + 4) % 4);
                Game1.player.FarmerSprite.StopAnimation();
                Game1.player.completelyStopAnimatingOrDoingAction();
                Game1.playSound("pickUpItem");
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
        
            foreach (ClickableComponent current2 in this.seasonButtons)
            {
                if (current2.containsPoint(x, y))
                {
                    this.optionButtonClick(current2.name);
                    current2.scale -= 0.5f;
                    current2.scale = Math.Max(3.5f, current2.scale);
                }
            }


            if (this.okButton.containsPoint(x, y) && this.canLeaveMenu())
            {
                this.optionButtonClick(this.okButton.name);
                this.okButton.scale -= 0.25f;
                this.okButton.scale = Math.Max(0.75f, this.okButton.scale);
            }

            if (!this.wizardSource)
            {
            }
            
           
        }

        public override void leftClickHeld(int x, int y)
        {

        }

        public override void releaseLeftClick(int x, int y)
        {

        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
        }

        public override void receiveKeyPress(Keys key)
        {
          
        }

        public override void performHoverAction(int x, int y)
        {
        
     
            if (!this.wizardSource)
            {
                
                using (List<ClickableComponent>.Enumerator enumerator4 = this.seasonButtons.GetEnumerator())
                {
                    while (enumerator4.MoveNext())
                    {
                        ClickableTextureComponent clickableTextureComponent4 = (ClickableTextureComponent)enumerator4.Current;
                        if (clickableTextureComponent4.containsPoint(x, y))
                        {
                            clickableTextureComponent4.scale = Math.Min(clickableTextureComponent4.scale + 0.02f, clickableTextureComponent4.baseScale + 0.1f);
                        }
                        else
                        {
                            clickableTextureComponent4.scale = Math.Max(clickableTextureComponent4.scale - 0.02f, clickableTextureComponent4.baseScale);
                        }
                    }
                }
            }
            if (this.okButton.containsPoint(x, y) && this.canLeaveMenu())
            {
                this.okButton.scale = Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.1f);
            }
            else
            {
                this.okButton.scale = Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);
            }
       
        }

        public virtual bool canLeaveMenu()
        {
            return this.wizardSource || (Game1.player.name.Length > 0 && Game1.player.farmName.Length > 0 && Game1.player.favoriteThing.Length > 0);
        }

        public override void draw(SpriteBatch b)
        {
            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, (string)null, false);
            b.Draw(Game1.daybg, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize + Game1.tileSize * 2 / 3 - 2), (float)(this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4)), Color.White);
            Game1.player.FarmerRenderer.draw(b, Game1.player.FarmerSprite.CurrentAnimationFrame, Game1.player.FarmerSprite.CurrentFrame, Game1.player.FarmerSprite.SourceRect, new Vector2((float)(this.xPositionOnScreen - 2 + Game1.tileSize * 2 / 3 + Game1.tileSize * 2 - Game1.tileSize / 2), (float)(this.yPositionOnScreen + IClickableMenu.borderWidth - Game1.tileSize / 4 + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 2)), Vector2.Zero, 0.8f, Color.White, 0.0f, 1f, Game1.player);
            if (!this.wizardSource)
            {
               
                foreach (ClickableTextureComponent textureComponent in this.seasonButtons)
                {
                    textureComponent.draw(b);
                       // b.Draw(Game1.mouseCursors, textureComponent.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 34, -1, -1)), Color.White);
                }
                foreach (ClickableComponent clickableComponent in this.labels)
                {
                    Color color = Color.Violet;
                    Utility.drawTextWithShadow(b, clickableComponent.name, Game1.smallFont, new Vector2((float)clickableComponent.bounds.X, (float)clickableComponent.bounds.Y), color, 1f, -1f, -1, -1, 1f, 3);
                    // b.Draw(Game1.mouseCursors, textureComponent.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 34, -1, -1)), Color.White);
                }
               
            }
            foreach (ClickableComponent clickableComponent in this.labels)
            {
                string text = "";
                Color color = Game1.textColor;
                switch (clickableComponent.name.Substring(0, 4))
                {
                    case "Name":
                        color = Game1.player.name.Length < 1 ? Color.Red : Game1.textColor;
                        if (!this.wizardSource)
                            break;
                        continue;
                    case "Farm":
                        color = Game1.player.farmName.Length < 1 ? Color.Red : Game1.textColor;
                        if (!this.wizardSource)
                            break;
                        continue;
                    case "Favo":
                        color = Game1.player.favoriteThing.Length < 1 ? Color.Red : Game1.textColor;
                        if (!this.wizardSource)
                            break;
                        continue;
                    case "Shir":
                        text = string.Concat((object)(Game1.player.shirt + 1));
                        break;
                    case "Skin":
                        text = string.Concat((object)(Game1.player.skin + 1));
                        break;
                    case "Hair":
                        if (!clickableComponent.name.Contains("Color"))
                        {
                            text = string.Concat((object)(Game1.player.hair + 1));
                            break;
                        }
                        break;
                    case "Acc.":
                        text = string.Concat((object)(Game1.player.accessory + 2));
                        break;
                    default:
                        color = Game1.textColor;
                        break;
                }
                Utility.drawTextWithShadow(b, clickableComponent.name, Game1.smallFont, new Vector2((float)clickableComponent.bounds.X, (float)clickableComponent.bounds.Y), color, 1f, -1f, -1, -1, 1f, 3);
                if (Enumerable.Count<char>((IEnumerable<char>)text) > 0)
                    Utility.drawTextWithShadow(b, text, Game1.smallFont, new Vector2((float)(clickableComponent.bounds.X + Game1.tileSize / 3) - Game1.smallFont.MeasureString(text).X / 2f, (float)(clickableComponent.bounds.Y + Game1.tileSize / 2)), color, 1f, -1f, -1, -1, 1f, 3);
            }
            
            if (this.canLeaveMenu())
            {
                this.okButton.draw(b);
            }
            else
            {
                this.okButton.draw(b);
                this.okButton.draw(b, Color.Black * 0.5f, 0.97f);
            }
            if (!this.wizardSource)
            {
              
                if (this.skipIntroButton != null)
                {
                    
                }
               
            }
            this.drawMouse(b);
        }
    }
}
