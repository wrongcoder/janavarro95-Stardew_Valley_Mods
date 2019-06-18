using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewSymphonyRemastered.Framework.V2;
using StardewValley;
using StardewValley.Menus;
using StardustCore.Animations;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents;
using StardustCore.UIUtilities.MenuComponents.Delegates;
using StardustCore.UIUtilities.MenuComponents.Delegates.Functionality;
using StardustCore.UIUtilities.SpriteFonts;

namespace StardewSymphonyRemastered.Framework.Menus
{
    public class MusicManagerMenuV2 : IClickableMenuExtended
    {    /// <summary>Interface for the menu for selection music.</summary>
         /// <summary>The different displays for this menu.</summary>
         ///


            ///Weather, time, day
        public enum DrawMode
        {
            AlbumSelection,
            AlbumFancySelection,
            SongSelectionMode,

            DifferentSelectionTypesModePage1, //Used for locations, events, festivals,  menus (house, exclamation mark, star, and list/book icons respectively)
            DifferentSelectionTypesModePage2, //Used for seasons
            WeatherSelection,
            FestivalSelection,
            EventSelection,
            MenuSelection,

            TimeSelection,
            LocationSelection,
            DaySelection,
            NothingElseToDisplay,

            SelectedEvent,
            SelectedFestival,
            SelectedMenu,

            SeasonSelection,
        }

        public List<Button> musicAlbumButtons;
        public Button currentMusicPackAlbum;
        public Button currentSelectedSong;
        public Button currentlySelectedOption; //The big button for season, menu, event, and festivals
        public Button currentlySelectedWeather; //Used to display what weather the user selected
        public Button currentlySelectedTime;
        public Button currentlySelectedLocation;
        public Button currentlySelectedDay;

        public Button currentlySelectedFestival;
        public Button currentlySelectedEvent;
        public Button currentlySelectedMenu;

        public Button addButton;
        public Button deleteButton;
        public Button playButton;
        public Button stopButton;
        public Button backButton;


        public bool selectedJustLocation;

        public DrawMode drawMode;
        public int currentAlbumIndex;
        public int currentSongPageIndex;
        public int locationPageIndex;
        public int festivalPageIndex;
        public int eventPageIndex;
        public int menuPageIndex;
        public int timePageIndex;

        public List<Button> fancyButtons; //List that holds all of the buttons for the fancy album menu.
        public int framesSinceLastUpdate; //Used to control how fast we can cycle through the menu.

        public bool searchBoxSelected;

        public MusicManagerMenuV2(float width, float height)
        {
            this.width = (int)width;
            this.height = (int)height;
            this.texturedStrings = new List<StardustCore.UIUtilities.SpriteFonts.Components.TexturedString>();
            this.musicAlbumButtons = new List<Button>();
            //thismusicAlbumButtons.Add(new Button("myButton", new Rectangle(100, 100, 64, 64), StardewSymphony.textureManager.getTexture("MusicNote").Copy(StardewSymphony.ModHelper), "mynote", new Rectangle(0, 0, 16, 16), 4f, new Animation(new Rectangle(0, 0, 16, 16)), Color.White, Color.White,new ButtonFunctionality(new DelegatePairing(hello,null),null,null),false)); //A button that does nothing on the left click.  

            this.fancyButtons = new List<Button>();

            //Initialize music album icons.
            int numOfButtons = 0;
            int rows = 0;
            foreach (MusicPackV2 musicPack in StardewSymphony.musicManager.MusicPacks.Values)
            {
                var sortedQuery = musicPack.SongInformation.songs.OrderBy(name => name);
                //musicPack.SongInformation.listOfSongsWithoutTriggers = sortedQuery.ToList(); //Alphabetize.
                if (musicPack.Icon == null)
                {
                    Texture2DExtended texture = StardewSymphony.textureManager.getTexture("MusicDisk");
                    float scale = 1.00f / (texture.Width / 64f);

                    this.musicAlbumButtons.Add(new Button(musicPack.Name, new Rectangle(100 + (numOfButtons * 100), 125 + (rows * 100), 64, 64), texture, "", new Rectangle(0, 0, 16, 16), scale, new Animation(new Rectangle(0, 0, 16, 16)), StardustCore.IlluminateFramework.Colors.randomColor(), Color.White, new ButtonFunctionality(new DelegatePairing(null, new List<object> { musicPack }), null, new DelegatePairing(null, new List<object> { musicPack })), false));
                }
                else
                {
                    float scale = 1.00f / (musicPack.Icon.Width / 64f);
                    this.musicAlbumButtons.Add(new Button(musicPack.Name, new Rectangle(100 + (numOfButtons * 100), 125 + (rows * 100), 64, 64), musicPack.Icon, "", new Rectangle(0, 0, musicPack.Icon.Width, musicPack.Icon.Height), scale, new Animation(new Rectangle(0, 0, 16, 16)), StardustCore.IlluminateFramework.LightColorsList.Black, StardustCore.IlluminateFramework.LightColorsList.Black, new ButtonFunctionality(new DelegatePairing(null, new List<object> { musicPack }), null, new DelegatePairing(null, new List<object> { musicPack })), false));
                }

                numOfButtons++;
                if (numOfButtons > 8)
                {
                    numOfButtons = 0;
                    rows++;
                }
            }

            //determine background color
            if (Game1.timeOfDay < 1200) this.dialogueBoxBackgroundColor = Color.SpringGreen;
            if (Game1.timeOfDay >= 1200 && Game1.timeOfDay < 1800) this.dialogueBoxBackgroundColor = Color.White;
            if (Game1.timeOfDay >= 1800) this.dialogueBoxBackgroundColor = Color.DarkViolet;


            this.currentAlbumIndex = 0;
            this.locationPageIndex = 0;
            this.menuPageIndex = 0;
            this.festivalPageIndex = 0;
            this.eventPageIndex = 0;
            this.timePageIndex = 0;
            this.drawMode = DrawMode.AlbumFancySelection;

            this.updateFancyButtons();
            this.framesSinceLastUpdate = 0;

            this.searchBoxSelected = false;
            this.menuTextures = new List<Texture2DExtended>();

            Vector2 playPos = new Vector2(this.width * .1f + 128 + 32, this.height * .05f + 128); //Put it to the right of the music disk
            this.playButton = new Button("PlayButton", new Rectangle((int)playPos.X, (int)playPos.Y, 64, 64), StardewSymphony.textureManager.getTexture("PlayButton"), "", new Rectangle(0, 0, 16, 16), 4f, new Animation(new Rectangle(0, 0, 16, 16)), Color.White, Color.White, new ButtonFunctionality(null, null, null));

            Vector2 stopPos = new Vector2(this.width * .1f + 192 + 32, this.height * .05f + 128); //Put it to the right of the music disk
            this.stopButton = new Button("StopButton", new Rectangle((int)stopPos.X, (int)stopPos.Y, 64, 64), StardewSymphony.textureManager.getTexture("StopButton"), "", new Rectangle(0, 0, 16, 16), 4f, new Animation(new Rectangle(0, 0, 16, 16)), Color.White, Color.White, new ButtonFunctionality(null, null, null));

            Vector2 addPos = new Vector2(this.width * .1f + 256 + 32, this.height * .05f + 128); //Put it to the right of the music disk
            this.addButton = new Button("AddIcon", new Rectangle((int)addPos.X, (int)addPos.Y, 64, 64), StardewSymphony.textureManager.getTexture("AddIcon"), "", new Rectangle(0, 0, 32, 32), 2f, new Animation(new Rectangle(0, 0, 32, 32)), Color.White, Color.White, new ButtonFunctionality(null, null, null));

            Vector2 delPos = new Vector2(this.width * .1f + 320 + 32, this.height * .05f + 128); //Put it to the right of the music disk
            this.deleteButton = new Button("DeleteIcon", new Rectangle((int)delPos.X, (int)delPos.Y, 64, 64), StardewSymphony.textureManager.getTexture("DeleteIcon"), "", new Rectangle(0, 0, 32, 32), 2f, new Animation(new Rectangle(0, 0, 32, 32)), Color.White, Color.White, new ButtonFunctionality(null, null, null));

            Vector2 backPos = new Vector2(this.width * .1f + 64, this.height * .05f); //Put it to the right of the music disk
            this.backButton = new Button("BackButton", new Rectangle((int)backPos.X, (int)backPos.Y, 64, 64), StardewSymphony.textureManager.getTexture("BackButton"), "", new Rectangle(0, 0, 16, 16), 4f, new Animation(new Rectangle(0, 0, 16, 16)), Color.White, Color.White, new ButtonFunctionality(null, null, null));

        }


        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            bool buttonSelected = false;

            if (this.currentSelectedSong != null && this.currentMusicPackAlbum != null && this.playButton.containsPoint(x, y))
            {
                Game1.playSound("coin");
                this.playSong();
                return;
            }

            if (this.currentSelectedSong != null && this.currentMusicPackAlbum != null && this.stopButton.containsPoint(x, y))
            {
                Game1.playSound("coin");
                this.stopSong();
                return;
            }

            if (this.currentSelectedSong != null && this.currentMusicPackAlbum != null && this.addButton.containsPoint(x, y))
            {
                Game1.playSound("coin");
                this.addSong();
                return;
            }

            if (this.currentSelectedSong != null && this.currentMusicPackAlbum != null && this.deleteButton.containsPoint(x, y))
            {
                Game1.playSound("coin");
                this.deleteSong();
                return;
            }

            if (this.backButton.containsPoint(x, y))
            {
                Game1.playSound("coin");
                this.goBack();
                return;
            }

            if (this.drawMode == DrawMode.AlbumFancySelection)
            {
                int count = 0;
                Button ok = Button.Empty();
                foreach (var button in this.fancyButtons)
                {
                    if (button.containsPoint(x, y) && button.buttonFunctionality.leftClick != null)
                    {
                        Game1.playSound("coin");
                        button.onLeftClick();
                        this.currentAlbumIndex += count - 3;
                        while (this.currentAlbumIndex < 0)
                            this.currentAlbumIndex = (this.musicAlbumButtons.Count - (this.currentAlbumIndex * -1));
                        ok = button;
                    }
                    if (button.buttonFunctionality.leftClick != null)
                        count++;
                }
                this.selectAlbum(ok);
                this.updateFancyButtons();
                return;
            }

            if (this.drawMode == DrawMode.SongSelectionMode)
            {
                int amountToShow = 6;
                this.updateFancyButtons();

                int amount;
                if (0 + ((this.currentSongPageIndex + 1) * amountToShow) >= this.fancyButtons.Count)
                {
                    amount = (0 + ((this.currentSongPageIndex + 1) * (amountToShow)) - this.fancyButtons.Count);
                    amount = amountToShow - amount;
                    if (amount < 0) amount = 0;
                }
                else if (this.fancyButtons.Count < amountToShow)
                    amount = this.fancyButtons.Count;
                else
                    amount = amountToShow;

                if (amount == 0 && this.currentSongPageIndex > 1)
                    this.currentSongPageIndex--;

                var drawList = this.fancyButtons.GetRange(0 + (this.currentSongPageIndex * (amountToShow)), amount);

                bool songSelected = false;
                //Get a list of components to draw. And if I click one select the song.
                foreach (var component in drawList)
                {
                    if (component.containsPoint(x, y))
                    {
                        Game1.playSound("coin");
                        this.selectSong(component);
                        songSelected = true;
                    }
                }
                if (songSelected)
                    this.updateFancyButtons();
                return;
            }
        }


        /// <summary>
        /// Update all of the buttons for the menu.
        /// </summary>
        public void updateFancyButtons()
        {
            //Album selection mode.
            if (this.drawMode == DrawMode.AlbumFancySelection)
            {
                this.fancyButtons.Clear();
                Vector4 placement = new Vector4((Game1.viewport.Width / 3), (Game1.viewport.Height / 4) + 128, this.width, this.height / 2);
                //generate buttons
                int offsetX = 200;
                if (this.musicAlbumButtons.Count > 0)
                {
                    for (int i = -3; i < 4; i++)
                    {
                        try
                        {
                            Button button = this.musicAlbumButtons.ElementAt(Math.Abs((this.currentAlbumIndex + i) % this.musicAlbumButtons.Count)).clone();
                            button.bounds = new Rectangle((int)placement.X + (i * 100) + offsetX, (int)placement.Y, 64, 64);
                            this.fancyButtons.Add(button);
                        }
                        catch
                        {
                            if (this.currentAlbumIndex + i == 0)
                            {
                                Button button = this.musicAlbumButtons.ElementAt(Math.Abs(0 % this.musicAlbumButtons.Count)).clone();
                                button.bounds = new Rectangle((int)placement.X + (i * 100) + offsetX, (int)placement.Y, 64, 64);
                                this.fancyButtons.Add(button);
                            }
                            else
                            {
                                try
                                {
                                    Button button = this.musicAlbumButtons.ElementAt(Math.Abs(((this.currentAlbumIndex + i) - this.musicAlbumButtons.Count) % this.musicAlbumButtons.Count)).clone();
                                    button.bounds = new Rectangle((int)placement.X + (i * 100) + offsetX, (int)placement.Y, 64, 64);
                                    this.fancyButtons.Add(button);
                                }
                                catch
                                {
                                    Button button = this.musicAlbumButtons.ElementAt(Math.Abs((this.currentAlbumIndex + i) + this.musicAlbumButtons.Count) % this.musicAlbumButtons.Count).clone();
                                    button.bounds = new Rectangle((int)placement.X + (i * 100) + offsetX, (int)placement.Y, 64, 64);
                                    this.fancyButtons.Add(button);
                                }
                            }
                        }
                    }
                    this.fancyButtons.Add(new Button("Outline", new Rectangle((int)placement.X + offsetX - 16, (int)placement.Y - 16, 64, 64), StardewSymphony.textureManager.getTexture("OutlineBox"), "", new Rectangle(0, 0, 16, 16), 6f, new Animation(new Rectangle(0, 0, 16, 16)), Color.White, Color.White, new ButtonFunctionality(null, null, new DelegatePairing(null, new List<object>())), false));
                    int count = 0;
                    foreach (var button in this.fancyButtons)
                    {

                        if (count == 3)
                        {
                            MusicPackV2 musicPack = (MusicPackV2)this.fancyButtons.ElementAt(count).buttonFunctionality.hover.paramaters[0];
                            this.texturedStrings.Clear();
                            this.texturedStrings.Add(SpriteFonts.vanillaFont.ParseString($"Current Album Name: {musicPack.Name}", new Vector2(button.bounds.X / 2, button.bounds.Y + 128), button.textColor));
                            button.hoverText = "";
                        }
                        count++;
                    }
                }
            }

            //Song selection mode.
            if (this.drawMode == DrawMode.SongSelectionMode)
            {
                this.fancyButtons.Clear();
                //Vector4 placement = new Vector4((Game1.viewport.Width / 3), (Game1.viewport.Height / 4) + 128, this.width, this.height / 2);
                MusicPackV2 musicPack = (MusicPackV2)this.currentMusicPackAlbum.buttonFunctionality.leftClick.paramaters[0];
                

                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 100, this.height * .9f);
                for (int i = 0; i < musicPack.SongInformation.songs.Count; i++)
                {
                    //Allow 8 songs to be displayed per page.
                    Texture2DExtended texture = StardewSymphony.textureManager.getTexture("MusicNote");
                    float scale = 1.00f / (texture.getTexture().Width / 64f);
                    string songName = musicPack.SongInformation.songs.ElementAt(i).Key;
                    Rectangle srcRect = new Rectangle(0, 0, texture.getTexture().Width, texture.getTexture().Height);
                    this.fancyButtons.Add(new Button(songName, new Rectangle((int)placement2.X + 25, (int)placement2.Y + ((i % 6) * 100) + 100, 64, 64), texture, songName, srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));
                }
            }

            if(this.drawMode== DrawMode.DifferentSelectionTypesModePage1)
            {
                this.fancyButtons.Clear();

                int buttonXPosition = 450;

                //Season Icon placement.
                Vector4 seasonPlacement = new Vector4(this.width * .2f + buttonXPosition, this.getFixedPositionFromMenu(0,64*2).Y, 5 * 100, this.height * .9f);
                switch (Game1.currentSeason)
                {
                    case "spring":
                        {
                            Texture2DExtended springTexture = StardewSymphony.textureManager.getTexture("SpringIcon");
                            if (springTexture == null)
                            {
                                if (StardewSymphony.Config.EnableDebugLog)
                                    StardewSymphony.ModMonitor.Log("SPRING TEXTURE NULL!");
                                return;
                            }
                            float scale = 1.00f / (springTexture.getTexture().Width / 64f);
                            Rectangle srcRect = new Rectangle(0, 0, springTexture.getTexture().Width, springTexture.getTexture().Height);
                            this.fancyButtons.Add(new Button("SeasonIcon", new Rectangle((int)seasonPlacement.X, (int)seasonPlacement.Y, 64, 64), springTexture, "Seasonal Music", srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));
                        }
                        break;

                    case "summer":
                        {
                            Texture2DExtended summerTexture = StardewSymphony.textureManager.getTexture("SummerIcon");
                            if (summerTexture == null)
                            {
                                if (StardewSymphony.Config.EnableDebugLog)
                                    StardewSymphony.ModMonitor.Log("SUMMER TEXTURE NULL!");
                                return;
                            }
                            float scale = 1.00f / (summerTexture.getTexture().Width / 64f);
                            Rectangle srcRect = new Rectangle(0, 0, summerTexture.getTexture().Width, summerTexture.getTexture().Height);
                            this.fancyButtons.Add(new Button("SeasonIcon", new Rectangle((int)seasonPlacement.X, (int)seasonPlacement.Y, 64, 64), summerTexture, "Seasonal Music", srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));
                        }
                        break;

                    case "fall":
                        {
                            Texture2DExtended fallTexture = StardewSymphony.textureManager.getTexture("FallIcon");
                            if (fallTexture == null)
                            {
                                if (StardewSymphony.Config.EnableDebugLog)
                                    StardewSymphony.ModMonitor.Log("FALL TEXTURE NULL!");
                                return;
                            }
                            float scale = 1.00f / (fallTexture.getTexture().Width / 64f);
                            Rectangle srcRect = new Rectangle(0, 0, fallTexture.getTexture().Width, fallTexture.getTexture().Height);
                            this.fancyButtons.Add(new Button("SeasonIcon", new Rectangle((int)seasonPlacement.X, (int)seasonPlacement.Y, 64, 64), fallTexture, "Seasonal Music", srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));
                        }
                        break;

                    case "winter":
                        {
                            Texture2DExtended winterTexture = StardewSymphony.textureManager.getTexture("WinterIcon");
                            if (winterTexture == null)
                            {
                                if (StardewSymphony.Config.EnableDebugLog)
                                    StardewSymphony.ModMonitor.Log("WINTER TEXTURE NULL!");
                                return;
                            }
                            float scale = 1.00f / (winterTexture.getTexture().Width / 64f);
                            Rectangle srcRect = new Rectangle(0, 0, winterTexture.getTexture().Width, winterTexture.getTexture().Height);
                            this.fancyButtons.Add(new Button("SeasonIcon", new Rectangle((int)seasonPlacement.X, (int)seasonPlacement.Y, 64, 64), winterTexture, "Seasonal Music", srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));
                        }
                        break;
                }

                Vector4 festivalPlacement = new Vector4(this.width * .2f + buttonXPosition, this.getFixedPositionFromMenu(0,64*3+16).Y, 6 * 100, this.height * .9f);
                Vector4 eventPlacement = new Vector4(this.width * .2f + buttonXPosition, this.getFixedPositionFromMenu(0, 64*4+(16*2)).Y, 7 * 100, this.height * .9f);
                Vector4 menuPlacement = new Vector4(this.width * .2f + buttonXPosition, this.getFixedPositionFromMenu(0, 64*5+(16*3)).Y, 8 * 100, this.height * .9f);
                Vector4 locationPlacement = new Vector4(this.width * .2f + buttonXPosition, this.getFixedPositionFromMenu(0, 64*6+(16*4)).Y, 9 * 100, this.height * .9f);
                Vector4 weatherPlacement = new Vector4(this.width * .2f + buttonXPosition, this.getFixedPositionFromMenu(0, 64 * 7 + (16 * 5)).Y, 9 * 100, this.height * .9f);

                //Festival Icon placement.
                Texture2DExtended festivalTexture = StardewSymphony.textureManager.getTexture("FestivalIcon");
                float festivalScale = 1.00f / (festivalTexture.getTexture().Width / 64f);
                Rectangle festivalSrcRect = new Rectangle(0, 0, festivalTexture.getTexture().Width, festivalTexture.getTexture().Height);
                this.fancyButtons.Add(new Button("FestivalIcon", new Rectangle((int)festivalPlacement.X, (int)festivalPlacement.Y, 64, 64), festivalTexture, "Festival Music", festivalSrcRect, festivalScale, new Animation(festivalSrcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));

                //Event Icon placement.
                Texture2DExtended eventTexture = StardewSymphony.textureManager.getTexture("EventIcon");
                float eventScale = 1.00f / (eventTexture.getTexture().Width / 64f);
                Rectangle eventSrcRectangle = new Rectangle(0, 0, eventTexture.getTexture().Width, eventTexture.getTexture().Height);
                this.fancyButtons.Add(new Button("EventIcon", new Rectangle((int)eventPlacement.X, (int)eventPlacement.Y, 64, 64), eventTexture, "Event Music", eventSrcRectangle, eventScale, new Animation(eventSrcRectangle), Color.White, Color.White, new ButtonFunctionality(null, null, null)));

                //Menu Icon placement.
                Texture2DExtended menuTexture = StardewSymphony.textureManager.getTexture("MenuIcon");
                float menuScale = 1.00f / (menuTexture.getTexture().Width / 64f);
                Rectangle menuSrcRectangle = new Rectangle(0, 0, menuTexture.getTexture().Width, menuTexture.getTexture().Height);
                this.fancyButtons.Add(new Button("MenuIcon", new Rectangle((int)menuPlacement.X, (int)menuPlacement.Y, 64, 64), menuTexture, "Menu Music", menuSrcRectangle, menuScale, new Animation(menuSrcRectangle), Color.White, Color.White, new ButtonFunctionality(null, null, null)));

                //Location Icon placement.
                Texture2DExtended locationTexture = StardewSymphony.textureManager.getTexture("HouseIcon");
                float locationScale = 1.00f / (locationTexture.getTexture().Width / 64f);
                Rectangle locationRect = new Rectangle(0, 0, locationTexture.getTexture().Width, locationTexture.getTexture().Height);
                this.fancyButtons.Add(new Button("LocationButton", new Rectangle((int)locationPlacement.X, (int)locationPlacement.Y, 64, 64), locationTexture, "Location Music", locationRect, locationScale, new Animation(locationRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));

                Texture2DExtended weatherTexture = StardewSymphony.textureManager.getTexture("WeatherIcon");
                float weatherScale = 1.00f / (weatherTexture.getTexture().Width / 64f);
                Rectangle weatherRect = new Rectangle(0, 0, weatherTexture.getTexture().Width, weatherTexture.getTexture().Height);
                this.fancyButtons.Add(new Button("WeatherButton", new Rectangle((int)weatherPlacement.X, (int)weatherPlacement.Y, 64, 64), weatherTexture, "Weather Music", weatherRect, weatherScale, new Animation(weatherRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));
            }

            
        }

        public override void update(GameTime time)
        {
            int updateNumber = 20;
            //Used for updating the album select screen.
            if (this.drawMode == DrawMode.AlbumFancySelection)
            {
                if (this.framesSinceLastUpdate == updateNumber)
                {
                    var state = Keyboard.GetState();
                    if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
                    {
                        this.currentAlbumIndex--;
                        if (this.currentAlbumIndex < 0) this.currentAlbumIndex = this.musicAlbumButtons.Count - 1;
                        this.updateFancyButtons();
                        this.framesSinceLastUpdate = 0;
                        Game1.playSound("shwip");
                    }

                    if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
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
            //Used for updating the song selection screen.
            if (this.drawMode == DrawMode.SongSelectionMode)
            {
                if (this.framesSinceLastUpdate == updateNumber)
                {
                    var state = Keyboard.GetState();
                    if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
                    {
                        if (this.currentSongPageIndex > 0)
                            this.currentSongPageIndex--;
                        this.updateFancyButtons();
                        this.framesSinceLastUpdate = 0;
                        Game1.playSound("shwip");
                    }

                    if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                    {
                        this.currentSongPageIndex++;
                        this.updateFancyButtons();
                        this.framesSinceLastUpdate = 0;
                        Game1.playSound("shwip");
                    }
                }
                else
                    this.framesSinceLastUpdate++;
            }
        }

        public void playSong()
        {

        }

        public void stopSong()
        {

        }

        public void addSong()
        {

        }

        public void deleteSong()
        {

        }
        public void goBack()
        {

        }
        public void selectAlbum(Button b)
        {
            if (b.label == "Null")
                return;

            this.currentMusicPackAlbum = b.clone(new Vector2(this.width * .1f + 64, this.height * .05f + 128));
            this.texturedStrings.Clear();
            this.texturedStrings.Add(SpriteFonts.vanillaFont.ParseString("Name:" + b.name, new Vector2(this.width * .1f, this.height * .05f + 256), b.textColor, false));
            this.drawMode = DrawMode.SongSelectionMode;
        }

        /// <summary>Select a given song from the menu.</summary>
        public void selectSong(Button b)
        {
            if (b.label == "Null")
                return;

            this.currentSelectedSong = b.clone(new Vector2(this.width * .1f + 64, this.height * .05f + 256));
            this.drawMode = DrawMode.DifferentSelectionTypesModePage1;
        }

        /// <summary>
        /// Draw the menu.
        /// </summary>
        /// <param name="b"></param>
        public override void draw(SpriteBatch b)
        {
            Vector4 placement = new Vector4(this.width * .1f, this.height * .05f - 96, 4 * 100 + 50, this.height + 32);
            Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f - 96, 5 * 100, this.height + 32);

            if (this.drawMode == DrawMode.AlbumSelection)
            {
                this.drawDialogueBoxBackground();
                foreach (var button in this.musicAlbumButtons)
                    button.draw(b);
            }

            if (this.drawMode == DrawMode.AlbumFancySelection)
            {
                Vector4 placement3 = new Vector4(Game1.viewport.Width / 4 - 50, Game1.viewport.Height / 4, 8 * 100, 128 * 2);
                this.drawDialogueBoxBackground((int)placement3.X, (int)placement3.Y, (int)placement3.Z, (int)placement3.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 0)));

                foreach (var button in this.fancyButtons)
                    button.draw(b);
                foreach (var str in this.texturedStrings)
                    str.draw(b);
            }

            if (this.drawMode == DrawMode.SongSelectionMode)
            {
                this.drawDialogueBoxBackground((int)placement.X, (int)placement.Y, (int)placement.Z, (int)placement.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));
                this.drawDialogueBoxBackground((int)placement2.X, (int)placement2.Y, (int)placement2.Z, (int)placement2.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));

                int amountToShow = 6;
                this.currentMusicPackAlbum.draw(b);

                int amount;
                if (0 + ((this.currentSongPageIndex + 1) * amountToShow) >= this.fancyButtons.Count)
                {
                    amount = (0 + ((this.currentSongPageIndex + 1) * (amountToShow)) - this.fancyButtons.Count);
                    amount = amountToShow - amount;
                    if (amount < 0) amount = 0;
                }
                else if (this.fancyButtons.Count < amountToShow)
                    amount = this.fancyButtons.Count;
                else
                    amount = amountToShow;

                if (amount == 0 && this.currentSongPageIndex > 1)
                    this.currentSongPageIndex--;

                var drawList = this.fancyButtons.GetRange(0 + (this.currentSongPageIndex * (amountToShow)), amount);

                foreach (var button in drawList)
                    button.draw(b);

                foreach (var str in this.texturedStrings)
                    str.draw(b);
            }

            if (this.drawMode == DrawMode.DifferentSelectionTypesModePage1)
            {
                this.drawDialogueBoxBackground((int)placement.X, (int)placement.Y, (int)placement.Z, (int)placement.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));
                this.drawDialogueBoxBackground((int)placement2.X, (int)placement2.Y, (int)placement2.Z, (int)placement2.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));

                this.currentMusicPackAlbum.draw(b);
                this.currentSelectedSong.draw(b);

                foreach (Button button in this.fancyButtons)
                    button.draw(b);

                foreach (var str in this.texturedStrings)
                    str.draw(b);

                //draw election buttons here???
            }

            this.drawMouse(b);
        }
    }
}
