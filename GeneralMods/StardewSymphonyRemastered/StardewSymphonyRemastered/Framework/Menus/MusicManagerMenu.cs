using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardustCore.Animations;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents;
using StardustCore.UIUtilities.MenuComponents.Delegates;
using StardustCore.UIUtilities.MenuComponents.Delegates.Functionality;
using StardustCore.UIUtilities.SpriteFonts;
using StardustCore.UIUtilities.SpriteFonts.Components;

namespace StardewSymphonyRemastered.Framework.Menus
{
    /* TODO: Make the different menus for the conditional keys
     * Normal (season, weather,time, location,date)
     * Festival
     * Event
     *
     * once song is selected also have play and stop button to see how song plays.
     * 
     * 
     * Figure out positioning for icons on fancy icons
     * 
     */

    /// <summary>
    /// Interface for the menu for selection music.
    /// </summary>
    public class MusicManagerMenu : IClickableMenuExtended
    {
        /// <summary>
        /// The different displays for this menu.
        /// </summary>
        public enum DrawMode
        {
            AlbumSelection,
            AlbumFancySelection,
            SongSelectionMode,

            DifferntSelectionTypesMode, //Used for locations, events, festivals,  menus (house, exclamation mark, star, and list/book icons respectively)
            WeatherSelection,
            FestivalSelection,
            EventSelection,
            MenuSelection,

            TimeSelection,
            LocationSelection,
            DaySelection,
            NothingElseToDisplay
        }

        public List<Button> musicAlbumButtons; 
        public Button currentMusicPackAlbum;
        public Button currentSelectedSong;
        public Button currentlySelectedOption; //The big button for season, menu, event, and festivals
        public Button currentlySelectedWeather; //Used to display what weather the user selected
        public Button currentlySelectedTime;
        public Button currentlySelectedLocation;
        public Button currentlySelectedDay;

        public DrawMode drawMode;
        public int currentAlbumIndex;
        public int currentSongPageIndex;
        public int locationPageIndex;

        public List<Button> fancyButtons; //List that holds all of the buttons for the fancy album menu.
        public int framesSinceLastUpdate; //Used to control how fast we can cycle through the menu.

        public bool searchBoxSelected;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public MusicManagerMenu()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public MusicManagerMenu(float width, float height)
        {
            this.width = (int)width;
            this.height = (int)height;
            this.texturedStrings = new List<StardustCore.UIUtilities.SpriteFonts.Components.TexturedString>();
            this.musicAlbumButtons = new List<StardustCore.UIUtilities.MenuComponents.Button>();
            //thismusicAlbumButtons.Add(new Button("myButton", new Rectangle(100, 100, 64, 64), StardewSymphony.textureManager.getTexture("MusicNote").Copy(StardewSymphony.ModHelper), "mynote", new Rectangle(0, 0, 16, 16), 4f, new StardustCore.Animations.Animation(new Rectangle(0, 0, 16, 16)), Color.White, Color.White,new ButtonFunctionality(new DelegatePairing(hello,null),null,null),false)); //A button that does nothing on the left click.  

            fancyButtons = new List<Button>();
            

            //Initialize music album icons.
            int numOfButtons = 0;
            int rows = 0;
            foreach(var v in StardewSymphony.musicManager.musicPacks)
            {
                var sortedQuery = v.Value.songInformation.listOfSongsWithoutTriggers.OrderBy(x => x.name);
                v.Value.songInformation.listOfSongsWithoutTriggers=sortedQuery.ToList(); //Alphabetize.
                if (v.Value.musicPackInformation.getTexture() == null)
                {
                    Texture2DExtended texture = StardewSymphony.textureManager.getTexture("MusicDisk");
                    float scale = 1.00f / ((float)texture.texture.Width / 64f);
       
                    this.musicAlbumButtons.Add(new Button(v.Key, new Rectangle(100 + (numOfButtons * 100), 125 + (rows * 100), 64, 64),texture, "", new Rectangle(0, 0, 16, 16), scale, new StardustCore.Animations.Animation(new Rectangle(0, 0, 16, 16)), StardustCore.IlluminateFramework.Colors.randomColor(), Color.White,new ButtonFunctionality(new DelegatePairing(null, new List<object>
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
                    float scale = 1.00f / ((float)v.Value.musicPackInformation.getTexture().texture.Width / 64f);
                    this.musicAlbumButtons.Add(new Button(v.Key, new Rectangle(100 + (numOfButtons * 100), 125 + (rows * 100), 64, 64), v.Value.musicPackInformation.getTexture(), "", new Rectangle(0, 0, v.Value.musicPackInformation.getTexture().texture.Width, v.Value.musicPackInformation.getTexture().texture.Height), scale, new StardustCore.Animations.Animation(new Rectangle(0, 0, 16, 16)), StardustCore.IlluminateFramework.LightColorsList.Black, StardustCore.IlluminateFramework.LightColorsList.Black, new ButtonFunctionality(new DelegatePairing(null, new List<object>
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

            //determine background color
            if (Game1.timeOfDay < 1200) this.dialogueBoxBackgroundColor = Color.SpringGreen;
            if (Game1.timeOfDay >= 1200&& Game1.timeOfDay < 1800) this.dialogueBoxBackgroundColor = Color.White;
            if (Game1.timeOfDay >= 1800) this.dialogueBoxBackgroundColor = Color.DarkViolet;


            this.currentAlbumIndex = 0;
            this.locationPageIndex = 0;
            this.drawMode = DrawMode.AlbumFancySelection;

            this.updateFancyButtons();
            this.framesSinceLastUpdate = 0;

            this.searchBoxSelected = false;
            this.menuTextures = new List<Texture2DExtended>();


    }

        /// <summary>
        /// Runs every game tick to check for stuff.
        /// </summary>
        /// <param name="time"></param>
        public override void update(GameTime time)
        {
            int updateNumber = 20;
            if (this.drawMode == DrawMode.AlbumFancySelection)
            {
                if (framesSinceLastUpdate == updateNumber)
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

            if (this.drawMode == DrawMode.SongSelectionMode)
            {
                if (framesSinceLastUpdate == updateNumber)
                {
                    var state = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                    if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left) || state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                    {
                        if (this.currentSongPageIndex > 0)
                        {
                            this.currentSongPageIndex--;
                        }
                        this.updateFancyButtons();
                        this.framesSinceLastUpdate = 0;
                        Game1.playSound("shwip");
                    }

                    if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right) || state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                    {
                        this.currentSongPageIndex++;
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

            if (this.drawMode == DrawMode.LocationSelection)
            {
                if (framesSinceLastUpdate == updateNumber)
                {
                    var state = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                    if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left) || state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                    {
                        if (this.locationPageIndex > 0)
                        {
                            this.locationPageIndex--;
                        }
                        this.updateFancyButtons();
                        this.framesSinceLastUpdate = 0;
                        Game1.playSound("shwip");
                    }

                    if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right) || state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                    {
                        this.locationPageIndex++;
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
        }


        /// <summary>
        /// Update the position of the album artwork when displaying it using the fancy buttons menu.
        /// </summary>
        public virtual void updateFancyButtons()
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
                            Button button = this.musicAlbumButtons.ElementAt(this.currentAlbumIndex + i).clone();
                            button.bounds = new Rectangle((int)placement.X + (i * 100) + offsetX, (int)placement.Y, 64, 64);
                            fancyButtons.Add(button);
                        }
                        catch (Exception err)
                        {
                            err.ToString();
                            if (this.currentAlbumIndex + i == 0)
                            {
                                Button button = this.musicAlbumButtons.ElementAt(0).clone();
                                button.bounds = new Rectangle((int)placement.X + (i * 100) + offsetX, (int)placement.Y, 64, 64);
                                fancyButtons.Add(button);
                            }
                            else
                            {

                                try
                                {
                                    Button button = this.musicAlbumButtons.ElementAt(((this.currentAlbumIndex + i) - this.musicAlbumButtons.Count) % this.musicAlbumButtons.Count).clone();
                                    button.bounds = new Rectangle((int)placement.X + (i * 100) + offsetX, (int)placement.Y, 64, 64);
                                    fancyButtons.Add(button);
                                }
                                catch (Exception err2)
                                {
                                    err2.ToString();
                                    Button button = this.musicAlbumButtons.ElementAt(((this.currentAlbumIndex + i) + this.musicAlbumButtons.Count) % this.musicAlbumButtons.Count).clone();
                                    button.bounds = new Rectangle((int)placement.X + (i * 100) + offsetX, (int)placement.Y, 64, 64);
                                    fancyButtons.Add(button);

                                }
                            }
                        }
                    }
                    this.fancyButtons.Add(new Button("Outline", new Rectangle((int)placement.X + offsetX - 16, (int)placement.Y - 16, 64, 64), StardewSymphony.textureManager.getTexture("OutlineBox"), "", new Rectangle(0, 0, 16, 16), 6f, new StardustCore.Animations.Animation(new Rectangle(0, 0, 16, 16)), Color.White, Color.White, new ButtonFunctionality(null, null, new DelegatePairing(null, new List<object>())), false));
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

            //Song selection mode.
            if(this.drawMode == DrawMode.SongSelectionMode)
            {
                this.fancyButtons.Clear();
                //Vector4 placement = new Vector4((Game1.viewport.Width / 3), (Game1.viewport.Height / 4) + 128, this.width, this.height / 2);
                var info = (KeyValuePair<string, MusicPack>)this.currentMusicPackAlbum.buttonFunctionality.leftClick.paramaters[0];
                var musicPackSongList = info.Value.songInformation.listOfSongsWithoutTriggers;

                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 100, this.height * .9f);
                for (int i = 0; i < musicPackSongList.Count; i++)
                {

                        //Allow 8 songs to be displayed per page.
                        Texture2DExtended texture = StardewSymphony.textureManager.getTexture("MusicNote");
                        float scale = 1.00f / ((float)texture.texture.Width / 64f);
                        Song s = musicPackSongList.ElementAt(i);
                        Rectangle srcRect = new Rectangle(0, 0, texture.texture.Width, texture.texture.Height);
                        this.fancyButtons.Add(new Button(s.name, new Rectangle((int)placement2.X+25, (int)placement2.Y + ((i%6) * 100)+100, 64, 64), texture, s.name, srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));
                    
                }
            }

            //Options selection mode.
            if (this.drawMode == DrawMode.DifferntSelectionTypesMode)
            {

                this.fancyButtons.Clear();

                int buttonXPosition = 300;

                //Season Icon placement.
                Vector4 seasonPlacement = new Vector4(this.width * .3f + buttonXPosition, this.height * .25f, 5 * 100, this.height * .9f);
                if (Game1.currentSeason == "spring")
                {

                    Texture2DExtended springTexture = StardewSymphony.textureManager.getTexture("SpringIcon");
                    if (springTexture == null)
                    {
                        StardewSymphony.ModMonitor.Log("TEXTURE NULL!");
                        return;
                    }
                    float scale = 1.00f / ((float)springTexture.texture.Width / 64f);
                    Rectangle srcRect = new Rectangle(0, 0, springTexture.texture.Width, springTexture.texture.Height);
                    this.fancyButtons.Add(new Button("SeasonIcon", new Rectangle((int)seasonPlacement.X, (int)seasonPlacement.Y, 64, 64), springTexture, "Spring Music", srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));
                }
                if (Game1.currentSeason == "summer")
                {

                    Texture2DExtended summerTexture = StardewSymphony.textureManager.getTexture("SummerIcon");
                    if (summerTexture == null)
                    {
                        StardewSymphony.ModMonitor.Log("TEXTURE NULL!");
                        return;
                    }
                    float scale = 1.00f / ((float)summerTexture.texture.Width / 64f);
                    Rectangle srcRect = new Rectangle(0, 0, summerTexture.texture.Width, summerTexture.texture.Height);
                    this.fancyButtons.Add(new Button("SeasonIcon", new Rectangle((int)seasonPlacement.X, (int)seasonPlacement.Y, 64, 64), summerTexture, "Summer Music", srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));
                }
                if (Game1.currentSeason == "fall")
                {

                    Texture2DExtended fallTexture = StardewSymphony.textureManager.getTexture("FallIcon");
                    if (fallTexture == null)
                    {
                        StardewSymphony.ModMonitor.Log("TEXTURE NULL!");
                        return;
                    }
                    float scale = 1.00f / ((float)fallTexture.texture.Width / 64f);
                    Rectangle srcRect = new Rectangle(0, 0, fallTexture.texture.Width, fallTexture.texture.Height);
                    this.fancyButtons.Add(new Button("SeasonIcon", new Rectangle((int)seasonPlacement.X, (int)seasonPlacement.Y, 64, 64), fallTexture, "Fall Music", srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));
                }
                if (Game1.currentSeason == "winter")
                {

                    Texture2DExtended winterTexture = StardewSymphony.textureManager.getTexture("WinterIcon");
                    if (winterTexture == null)
                    {
                        StardewSymphony.ModMonitor.Log("TEXTURE NULL!");
                        return;
                    }
                    float scale = 1.00f / ((float)winterTexture.texture.Width / 64f);
                    Rectangle srcRect = new Rectangle(0, 0, winterTexture.texture.Width, winterTexture.texture.Height);
                    this.fancyButtons.Add(new Button("SeasonIcon", new Rectangle((int)seasonPlacement.X, (int)seasonPlacement.Y, 64, 64), winterTexture, "Winter Music", srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));
                }

                Vector4 festivalPlacement = new Vector4(this.width * .3f + buttonXPosition, this.height * .35f, 6 * 100, this.height * .9f);
                Vector4 eventPlacement = new Vector4(this.width * .3f + buttonXPosition, this.height * .45f, 7 * 100, this.height * .9f);
                Vector4 menuPlacement = new Vector4(this.width * .3f + buttonXPosition, this.height * .55f, 8 * 100, this.height * .9f);

                //Festival Icon placement.
                Texture2DExtended festivalTexture = StardewSymphony.textureManager.getTexture("FestivalIcon");
                float festivalScale = 1.00f / ((float)festivalTexture.texture.Width / 64f);
                Rectangle festivalSrcRect = new Rectangle(0, 0, festivalTexture.texture.Width, festivalTexture.texture.Height);
                this.fancyButtons.Add(new Button("FestivalIcon", new Rectangle((int)festivalPlacement.X, (int)festivalPlacement.Y, 64, 64), festivalTexture, "Festival Music", festivalSrcRect, festivalScale, new Animation(festivalSrcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));
                if (festivalTexture == null)
                {
                    StardewSymphony.ModMonitor.Log("TEXTURE NULL!");
                    return;
                }

                //Event Icon placement.
                Texture2DExtended eventTexture = StardewSymphony.textureManager.getTexture("EventIcon");
                float eventScale = 1.00f / ((float)eventTexture.texture.Width / 64f);
                Rectangle eventSrcRectangle = new Rectangle(0, 0, eventTexture.texture.Width, eventTexture.texture.Height);
                this.fancyButtons.Add(new Button("EventIcon", new Rectangle((int)eventPlacement.X, (int)eventPlacement.Y, 64, 64), eventTexture, "Event Music", eventSrcRectangle, eventScale, new Animation(eventSrcRectangle), Color.White, Color.White, new ButtonFunctionality(null, null, null)));

                if (eventTexture == null)
                {
                    StardewSymphony.ModMonitor.Log("TEXTURE NULL!");
                    return;
                }

                //Menu Icon placement.
                Texture2DExtended menuTexture = StardewSymphony.textureManager.getTexture("MenuIcon");
                float menuScale = 1.00f / ((float)menuTexture.texture.Width / 64f);
                Rectangle menuSrcRectangle = new Rectangle(0, 0, menuTexture.texture.Width, menuTexture.texture.Height);
                this.fancyButtons.Add(new Button("MenuIcon", new Rectangle((int)menuPlacement.X, (int)menuPlacement.Y, 64, 64), menuTexture, "Menu Music", menuSrcRectangle, menuScale, new Animation(menuSrcRectangle), Color.White, Color.White, new ButtonFunctionality(null, null, null)));

                if (menuTexture == null)
                {
                    StardewSymphony.ModMonitor.Log("TEXTURE NULL!");
                    return;
                }
            }


            if(this.drawMode == DrawMode.WeatherSelection)
            {
                this.fancyButtons.Clear();

                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 100, this.height * .9f);
                for (int i = 0; i < 7; i++)
                {

                    //Allow 8 songs to be displayed per page.
                    Texture2DExtended texture=new Texture2DExtended();
                    string name = "";
                    string display = "";
                    if (i == 0)
                    {
                       texture = StardewSymphony.textureManager.getTexture("SunnyIcon");
                       name = "SunnyIcon";
                       display = "Sunny Music";
                    }
                    if (i == 1)
                    {
                        texture = StardewSymphony.textureManager.getTexture("RainyIcon");
                        name = "RainyIcon";
                        display = "Rainy Music";
                    }
                    if (i == 2)
                    {
                        if (Game1.currentSeason == "spring")
                        {
                            texture = StardewSymphony.textureManager.getTexture("DebrisSpringIcon");
                            name = "WeatherDebrisIcon";
                            display = "Debris Music";
                        }
                        if (Game1.currentSeason == "summer")
                        {
                            texture = StardewSymphony.textureManager.getTexture("DebrisSummerIcon");
                            name = "WeatherDebrisIcon";
                            display = "Debris Music";
                        }
                        if (Game1.currentSeason == "fall")
                        {
                            texture = StardewSymphony.textureManager.getTexture("DebrisFallIcon");
                            name = "WeatherDebrisIcon";
                            display = "Debris Music";
                        }
                        if (Game1.currentSeason == "winter")
                        {
                            texture = StardewSymphony.textureManager.getTexture("DebrisSpringIcon");
                            name = "WeatherDebrisIcon";
                            display = "Debris Music";
                        }      
                    }
                    if (i == 3)
                    {
                        texture = StardewSymphony.textureManager.getTexture("SnowIcon");
                        name = "SnowIcon";
                        display = "Snow Music";
                    }
                    if (i == 4)
                    {
                        texture = StardewSymphony.textureManager.getTexture("StormIcon");
                        name = "StormIcon";
                        display = "Storm Music";
                    }
                    if (i == 5)
                    {
                        texture = StardewSymphony.textureManager.getTexture("WeatherFestivalIcon");
                        name = "WeatherFestivalIcon";
                        display = "Festival Day Music";
                    }
                    if (i == 6)
                    {
                        texture = StardewSymphony.textureManager.getTexture("WeddingIcon");
                        name = "WeddingIcon";
                        display = "Wedding Music";
                    }

                    float scale = 1.00f / ((float)texture.texture.Width / 64f);
                    Rectangle srcRect = new Rectangle(0, 0, texture.texture.Width, texture.texture.Height);
                    this.fancyButtons.Add(new Button(name, new Rectangle((int)placement2.X + 50, (int)placement2.Y + ((i % 7) * 100), 64, 64), texture, display, srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));

                }
            }


            if (this.drawMode == DrawMode.TimeSelection)
            {
                this.fancyButtons.Clear();

                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 300, this.height * .9f);
                for (int i = 0; i < 2; i++)
                {

                    //Allow 8 songs to be displayed per page.
                    Texture2DExtended texture = new Texture2DExtended();
                    string name = "";
                    string display = "";
                    if (i == 0)
                    {
                        texture = StardewSymphony.textureManager.getTexture("DayIcon");
                        name = "DayIcon";
                        display = "Day Music";
                    }
                    if (i == 1)
                    {
                        texture = StardewSymphony.textureManager.getTexture("NightIcon");
                        name = "NightIcon";
                        display = "Night Music";
                    }

                    if (texture == null)
                    {
                        StardewSymphony.ModMonitor.Log("HMM A TEXTURE IS NULL: " + i.ToString());
                    }
                    float scale = 1.00f / ((float)texture.texture.Width / 64f);
                    Rectangle srcRect = new Rectangle(0, 0, texture.texture.Width, texture.texture.Height);
                    this.fancyButtons.Add(new Button(name, new Rectangle((int)placement2.X + 50, (int)placement2.Y + ((i % 7) * 100), 64, 64), texture, display, srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));

                }
            }


            //Song selection mode.
            if (this.drawMode == DrawMode.LocationSelection)
            {
                this.fancyButtons.Clear();

                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 100, this.height * .9f);
                for (int i = 0; i < SongSpecifics.locations.Count; i++)
                {
                    string locName = SongSpecifics.locations.ElementAt(i);
                    //Allow 8 songs to be displayed per page.
                    Texture2DExtended texture = StardewSymphony.textureManager.getTexture("HouseIcon");
                    float scale = 1.00f / ((float)texture.texture.Width / 64f);
                    Rectangle srcRect = new Rectangle(0, 0, texture.texture.Width, texture.texture.Height);
                    this.fancyButtons.Add(new Button(locName, new Rectangle((int)placement2.X + 25, (int)placement2.Y + ((i % 6) * 100) + 100, 64, 64), texture, locName, srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));

                }
            }


            if (this.drawMode == DrawMode.DaySelection)
            {
                this.fancyButtons.Clear();

                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 100, this.height * .9f);
                for (int i = 0; i < 7; i++)
                {

                    //Allow 8 songs to be displayed per page.
                    Texture2DExtended texture = new Texture2DExtended();
                    string name = "";
                    string display = "";
                    if (i == 0)
                    {
                        texture = StardewSymphony.textureManager.getTexture("SundayIcon");
                        name = "SundayIcon";
                        display = "Sunday Music";
                    }
                    if (i == 1)
                    {
                        texture = StardewSymphony.textureManager.getTexture("MondayIcon");
                        name = "MondayIcon";
                        display = "Monday Music";
                    }
                    if (i == 2)
                    {
                        texture = StardewSymphony.textureManager.getTexture("TuesdayIcon");
                        name = "TuesdayIcon";
                        display = "Tuesday Music";
                        
                    }
                    if (i == 3)
                    {
                        texture = StardewSymphony.textureManager.getTexture("WednesdayIcon");
                        name = "WednesdayIcon";
                        display = "Wednesday Music";
                    }
                    if (i == 4)
                    {
                        texture = StardewSymphony.textureManager.getTexture("ThursdayIcon");
                        name = "ThursdayIcon";
                        display = "Thursday Music";
                    }
                    if (i == 5)
                    {
                        texture = StardewSymphony.textureManager.getTexture("FridayIcon");
                        name = "FridayIcon";
                        display = "Friday Music";
                    }
                    if (i == 6)
                    {
                        texture = StardewSymphony.textureManager.getTexture("SaturdayIcon");
                        name = "SaturdayIcon";
                        display = "Saturday Music";
                    }

                    float scale = 1.00f / ((float)texture.texture.Width / 64f);
                    Rectangle srcRect = new Rectangle(0, 0, texture.texture.Width, texture.texture.Height);
                    this.fancyButtons.Add(new Button(name, new Rectangle((int)placement2.X + 50, (int)placement2.Y + ((i % 7) * 100), 64, 64), texture, display, srcRect, scale, new Animation(srcRect), Color.White, Color.White, new ButtonFunctionality(null, null, null)));

                }
            }

            if (this.drawMode == DrawMode.NothingElseToDisplay)
            {
                this.fancyButtons.Clear();
            }
        }

        /// <summary>
        /// Functionality that occurs when right clicking a menu component.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="playSound"></param>
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

        /// <summary>
        /// Actions that occur when hovering over an icon.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
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

        /// <summary>
        /// Functionality that occurs when left clicking a menu component.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="playSound"></param>
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            bool buttonSelected = false;

            if (this.drawMode == DrawMode.AlbumSelection)
            {
                foreach (var v in this.musicAlbumButtons)
                {
                    if (v.containsPoint(x, y))
                    {
                        this.selectAlbum(v);
                        v.onLeftClick();
                    }
                }
                return;
            }

            if (this.drawMode == DrawMode.AlbumFancySelection)
            {
                int count = 0;
                Button ok = Button.Empty();
                foreach (var v in this.fancyButtons)
                {
                    if (v.containsPoint(x, y) && v.buttonFunctionality.leftClick != null)
                    {
                        v.onLeftClick();
                        this.currentAlbumIndex += count - 3;
                        //if (this.currentAlbumIndex >= this.musicAlbumButtons.Count) this.currentAlbumIndex -= (this.musicAlbumButtons.Count);
                        //StardewSymphony.ModMonitor.Log(this.currentAlbumIndex.ToString());
                        while (currentAlbumIndex < 0)
                        {
                            this.currentAlbumIndex = (this.musicAlbumButtons.Count - (this.currentAlbumIndex * -1));
                        }
                        ok = v;
                    }
                    if (v.buttonFunctionality.leftClick != null)
                    {
                        count++;
                    }
                }
                //this.updateFancyButtons();
                this.selectAlbum(ok);
                //return;
            }

            if (this.drawMode == DrawMode.SongSelectionMode)
            {
                Button ok = Button.Empty();
                int amountToShow = 6;
                this.updateFancyButtons();

                int count = this.fancyButtons.Count - 1;
                int amount = 0;
                if (0 + ((this.currentSongPageIndex + 1) * amountToShow) >= this.fancyButtons.Count)
                {
                    amount = (0 + ((this.currentSongPageIndex + 1) * (amountToShow)) - fancyButtons.Count);
                    amount = amountToShow - amount;
                    if (amount < 0) amount = 0;
                }
                else if (this.fancyButtons.Count < amountToShow)
                {
                    amount = this.fancyButtons.Count;
                }
                else
                {
                    amount = amountToShow;
                }
                if (amount == 0 && this.currentSongPageIndex > 1)
                {
                    this.currentSongPageIndex--;
                }
                var drawList = this.fancyButtons.GetRange(0 + (this.currentSongPageIndex * (amountToShow)), amount);


                bool songSelected = false;
                //Get a list of components to draw. And if I click one select the song.
                foreach (var v in drawList)
                {
                    if (v.containsPoint(x, y))
                    {
                        selectSong(v);
                        songSelected = true;
                    }
                }
                if (songSelected == true)
                {
                    this.updateFancyButtons();
                }
                return;
            }

            //Left click an option.
            if(this.drawMode == DrawMode.DifferntSelectionTypesMode)
            {
                foreach(var button in this.fancyButtons)
                {
                    if (button.containsPoint(x, y))
                    {
                        if (button == null) continue;
                        Vector2 position = new Vector2(this.width * .1f + 64, this.height * .05f + 320);
                        if (button.name == "SeasonIcon")
                        {
                            this.currentlySelectedOption = button.clone(position);
                            this.drawMode = DrawMode.WeatherSelection;
                            buttonSelected = true;
                        }
                        if (button.name == "FestivalIcon")
                        {

                        }
                        if (button.name == "EventIcon")
                        {

                        }
                        if (button.name == "MenuIcon")
                        {

                        }
                    }
                }
                if (buttonSelected == true) this.updateFancyButtons();
                return;
            }

            //Left click an option.
            if (this.drawMode == DrawMode.WeatherSelection)
            {
                foreach (var button in this.fancyButtons)
                {
                    if (button.containsPoint(x, y))
                    {
                        if (button == null) continue;
                        Vector2 position = new Vector2(this.width * .1f + 64, this.height * .05f + 384);
                        if (button.name == "SunnyIcon" || button.name == "RainyIcon" || button.name == "SnowyIcon" || button.name == "WeatherDebrisIcon" || button.name == "StormyIcon" || button.name == "WeatherFestivalIcon" || button.name == "WeddingIcon")
                        {
                            this.currentlySelectedWeather = button.clone(position);
                            this.drawMode = DrawMode.TimeSelection;
                            buttonSelected = true;
                        }
                    }                    
                }
                if (buttonSelected == true) this.updateFancyButtons();
                return;
            }

            //Left click an option.
            if (this.drawMode == DrawMode.TimeSelection)
            {
                foreach (var button in this.fancyButtons)
                {
                    if (button.containsPoint(x, y))
                    {
                        if (button == null) continue;
                        Vector2 position = new Vector2(this.width * .1f + 64, this.height * .05f + 448);
                        if (button.name == "DayIcon" || button.name == "NightIcon" )
                        {
                            this.currentlySelectedTime = button.clone(position);
                            this.drawMode = DrawMode.LocationSelection;
                            buttonSelected = true;
                        }
                    }
                }
                if (buttonSelected == true) this.updateFancyButtons();
                return;
            }

            //Left click an option.
            if (this.drawMode == DrawMode.LocationSelection)
            {
                foreach (var button in this.fancyButtons)
                {
                    if (button.containsPoint(x, y))
                    {
                        if (button == null) continue;
                        Vector2 position = new Vector2(this.width * .1f + 64, this.height * .05f + 512);
                            //Get any valid location button.
                            this.currentlySelectedLocation = button.clone(position);
                            this.drawMode = DrawMode.DaySelection;
                            buttonSelected = true;                  
                    }
                }
                if (buttonSelected == true) this.updateFancyButtons();
                return;
            }

            if (this.drawMode == DrawMode.DaySelection)
            {
                foreach (var button in this.fancyButtons)
                {
                    if (button.containsPoint(x, y))
                    {
                        if(button.name!="SundayIcon"|| button.name != "MondayIcon" || button.name != "TuesdayIcon" || button.name != "WednesdayIcon" || button.name != "ThursdayIcon" || button.name != "FridayIcon" || button.name != "SaturdayIcon")
                        if (button == null) continue;
                        Vector2 position = new Vector2(this.width * .1f + 64, this.height * .05f + 576);
                        //Get any valid location button.
                        this.currentlySelectedDay = button.clone(position);
                        this.drawMode = DrawMode.NothingElseToDisplay;
                        buttonSelected = true;
                    }
                }
                if (buttonSelected == true) this.updateFancyButtons();
                return;
            }


        }

        /// <summary>
        /// Draws the menu and it's respective components depending on the drawmode that is currently set.
        /// </summary>
        /// <param name="b"></param>
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

            if (this.drawMode == DrawMode.SongSelectionMode)
            {
                Vector4 placement = new Vector4(this.width*.1f, this.height*.05f, 4 * 100, 128 * 2);
                this.drawDialogueBoxBackground((int)placement.X, (int)placement.Y, (int)placement.Z, (int)placement.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));


                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 100, this.height*.95f);
                this.drawDialogueBoxBackground((int)placement2.X, (int)placement2.Y, (int)placement2.Z, (int)placement2.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));

                int amountToShow = 6;
                this.currentMusicPackAlbum.draw(b);

                int count = this.fancyButtons.Count-1;
                int amount = 0;
                if (0 + ( (this.currentSongPageIndex+1) * amountToShow) >= this.fancyButtons.Count)
                {
                    amount = (0 + ((this.currentSongPageIndex+1) * (amountToShow)) - fancyButtons.Count);
                    amount = amountToShow - amount;
                    if (amount < 0) amount = 0;
                }
                else if (this.fancyButtons.Count < amountToShow)
                {
                    amount = this.fancyButtons.Count;
                }
                else
                {
                    amount = amountToShow;
                }
                if (amount==0 && this.currentSongPageIndex>1)
                {
                    this.currentSongPageIndex--;
                }
                var drawList = this.fancyButtons.GetRange(0 + (this.currentSongPageIndex * (amountToShow)), amount);

                foreach(var v in drawList)
                {
                    v.draw(b);
                }

                foreach(var v in this.texturedStrings)
                {
                    v.draw(b);
                }

            }


            if (this.drawMode == DrawMode.DifferntSelectionTypesMode)
            {
                Vector4 placement = new Vector4(this.width * .1f, this.height * .05f, 4 * 100, 128 * 2);
                this.drawDialogueBoxBackground((int)placement.X, (int)placement.Y, (int)placement.Z, (int)placement.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));


                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 100, this.height * .95f);
                this.drawDialogueBoxBackground((int)placement2.X, (int)placement2.Y, (int)placement2.Z, (int)placement2.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));


                this.currentMusicPackAlbum.draw(b);
                this.currentSelectedSong.draw(b);

                foreach (Button button in fancyButtons)
                {
                    button.draw(b);
                }

                foreach (var v in this.texturedStrings)
                {
                    v.draw(b);
                }

            }

            if (this.drawMode == DrawMode.DifferntSelectionTypesMode)
            {
                Vector4 placement = new Vector4(this.width * .1f, this.height * .05f, 4 * 100, 128 * 2);
                this.drawDialogueBoxBackground((int)placement.X, (int)placement.Y, (int)placement.Z, (int)placement.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));


                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 100, this.height * .95f);
                this.drawDialogueBoxBackground((int)placement2.X, (int)placement2.Y, (int)placement2.Z, (int)placement2.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));

                //make 3rd dialogue box option;
                this.currentMusicPackAlbum.draw(b);
                this.currentSelectedSong.draw(b);
                //this.currentlySelectedOption.draw(b);

                foreach (Button button in fancyButtons)
                {
                    button.draw(b);
                }

                foreach (var v in this.texturedStrings)
                {
                    v.draw(b);
                }

            }

            this.drawMouse(b);


            if (this.drawMode == DrawMode.WeatherSelection)
            {
                Vector4 placement = new Vector4(this.width * .1f, this.height * .05f, 4 * 100, 128 * 2);
                this.drawDialogueBoxBackground((int)placement.X, (int)placement.Y, (int)placement.Z, (int)placement.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));


                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 100, this.height * .95f);
                this.drawDialogueBoxBackground((int)placement2.X, (int)placement2.Y, (int)placement2.Z, (int)placement2.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));

                //make 3rd dialogue box option;
                this.currentMusicPackAlbum.draw(b);
                this.currentSelectedSong.draw(b);
                this.currentlySelectedOption.draw(b);

                foreach (Button button in fancyButtons)
                {
                    button.draw(b);
                }

                foreach (var v in this.texturedStrings)
                {
                    v.draw(b);
                }

            }

            if (this.drawMode == DrawMode.TimeSelection)
            {
                Vector4 placement = new Vector4(this.width * .1f, this.height * .05f, 4 * 100, 128 * 2);
                this.drawDialogueBoxBackground((int)placement.X, (int)placement.Y, (int)placement.Z, (int)placement.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));


                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 100, this.height * .95f);
                this.drawDialogueBoxBackground((int)placement2.X, (int)placement2.Y, (int)placement2.Z, (int)placement2.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));

                //make 3rd dialogue box option;
                this.currentMusicPackAlbum.draw(b);
                this.currentSelectedSong.draw(b);
                this.currentlySelectedOption.draw(b);
                this.currentlySelectedWeather.draw(b);

                foreach (Button button in fancyButtons)
                {
                    button.draw(b);
                }

                foreach (var v in this.texturedStrings)
                {
                    v.draw(b);
                }

            }

            if (this.drawMode == DrawMode.LocationSelection)
            {
                Vector4 placement = new Vector4(this.width * .1f, this.height * .05f, 4 * 100, 128 * 2);
                this.drawDialogueBoxBackground((int)placement.X, (int)placement.Y, (int)placement.Z, (int)placement.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));


                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 100, this.height * .95f);
                this.drawDialogueBoxBackground((int)placement2.X, (int)placement2.Y, (int)placement2.Z, (int)placement2.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));

                int amountToShow = 6;
                this.currentMusicPackAlbum.draw(b);
                this.currentSelectedSong.draw(b);
                this.currentlySelectedOption.draw(b);
                this.currentlySelectedWeather.draw(b);
                this.currentlySelectedTime.draw(b);
                
                //Deals with logic regarding different pages.
                int count = this.fancyButtons.Count - 1;
                int amount = 0;
                if (0 + ((this.locationPageIndex + 1) * amountToShow) >= this.fancyButtons.Count)
                {
                    amount = (0 + ((this.locationPageIndex + 1) * (amountToShow)) - fancyButtons.Count);
                    amount = amountToShow - amount;
                    if (amount < 0) amount = 0;
                }
                else if (this.fancyButtons.Count < amountToShow)
                {
                    amount = this.fancyButtons.Count;
                }
                else
                {
                    amount = amountToShow;
                }
                if (amount == 0 && this.locationPageIndex > 1)
                {
                    this.locationPageIndex--;
                }
                var drawList = this.fancyButtons.GetRange(0 + (this.locationPageIndex * (amountToShow)), amount);

                
                foreach (var v in drawList)
                {
                    v.draw(b);
                }

                foreach (var v in this.texturedStrings)
                {
                    v.draw(b);
                }

            }


            if (this.drawMode == DrawMode.DaySelection)
            {
                Vector4 placement = new Vector4(this.width * .1f, this.height * .05f, 4 * 100, 128 * 2);
                this.drawDialogueBoxBackground((int)placement.X, (int)placement.Y, (int)placement.Z, (int)placement.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));


                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 100, this.height * .95f);
                this.drawDialogueBoxBackground((int)placement2.X, (int)placement2.Y, (int)placement2.Z, (int)placement2.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));

                //make 3rd dialogue box option;
                this.currentMusicPackAlbum.draw(b);
                this.currentSelectedSong.draw(b);
                this.currentlySelectedOption.draw(b);
                this.currentlySelectedWeather.draw(b);
                this.currentlySelectedTime.draw(b);
                this.currentlySelectedLocation.draw(b);

                foreach (Button button in fancyButtons)
                {
                    button.draw(b);
                }

                foreach (var v in this.texturedStrings)
                {
                    v.draw(b);
                }

            }


            if (this.drawMode == DrawMode.NothingElseToDisplay)
            {
                Vector4 placement = new Vector4(this.width * .1f, this.height * .05f, 4 * 100, 128 * 2);
                this.drawDialogueBoxBackground((int)placement.X, (int)placement.Y, (int)placement.Z, (int)placement.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));


                Vector4 placement2 = new Vector4(this.width * .2f + 400, this.height * .05f, 5 * 100, this.height * .95f);
                this.drawDialogueBoxBackground((int)placement2.X, (int)placement2.Y, (int)placement2.Z, (int)placement2.W, new Color(new Vector4(this.dialogueBoxBackgroundColor.ToVector3(), 255)));

                //make 3rd dialogue box option;
                this.currentMusicPackAlbum.draw(b);
                this.currentSelectedSong.draw(b);
                this.currentlySelectedOption.draw(b);
                this.currentlySelectedWeather.draw(b);
                this.currentlySelectedTime.draw(b);
                this.currentlySelectedLocation.draw(b);
                this.currentlySelectedDay.draw(b);

                foreach (Button button in fancyButtons)
                {
                    button.draw(b);
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
            //StardewSymphony.ModMonitor.Log(info.ToString());
            StardewSymphony.musicManager.swapMusicPacks(info.Key);
            StardewSymphony.musicManager.playRandomSongFromPack(info.Key);
            //info.Value.playRandomSong();
        }

        /// <summary>
        /// Select a album artwork and change the draw mode to go to the song selection screen.
        /// </summary>
        /// <param name="b"></param>
        public void selectAlbum(Button b)
        {
            if (b.label == "Null") return;
            this.currentMusicPackAlbum = b.clone(new Vector2(this.width*.1f+64,this.height*.05f+128));
            StardewSymphony.ModMonitor.Log("Album Selected!"+b.name);
            this.texturedStrings.Clear();
            this.texturedStrings.Add(SpriteFonts.vanillaFont.ParseString("Name:" + (string)b.name, new Microsoft.Xna.Framework.Vector2(this.width*.1f, this.height*.05f + 256), b.textColor));
            this.drawMode = DrawMode.SongSelectionMode;
        }

        public void selectSong(Button b)
        {
            if (b.label == "Null") return;
            this.currentSelectedSong = b.clone(new Vector2(this.width * .1f + 64, this.height * .05f + 256));
            /*
            StardewSymphony.ModMonitor.Log("Song Selected!" + b.name);
            var info = (KeyValuePair<string, MusicPack>)this.currentMusicPackAlbum.buttonFunctionality.leftClick.paramaters[0];
            StardewSymphony.ModMonitor.Log("Select Pack:"+info.Key);
            StardewSymphony.musicManager.swapMusicPacks(info.Key);
            StardewSymphony.musicManager.playSongFromCurrentPack(b.name);
            */
            this.drawMode = DrawMode.DifferntSelectionTypesMode;
            //this.updateFancyButtons();
        }

        

        #endregion
    }
}
