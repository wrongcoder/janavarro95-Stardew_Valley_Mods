using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewSymphonyRemastered.Framework;
using StardewValley;
using StardustCore.UIUtilities;

namespace StardewSymphonyRemastered
{
    // TODO:
    //
    // Fixed Farm building glitch,
    // Added underground mine support
    // Added seasonal selection support
    // added just location support
    // added in write all config option
    // 
    // Add mod config to have silent rain option.
    // Add in shuffle song button that just selects music but probably plays a different song. same as musicManager.selectmusic(getConditionalString);
    // Add in a save button to save settings in the menu.
    // 
    // Notes:
    // All mods must add events/locations/festivals/menu information to this mod during the Entry function of their mod because once the player is loaded that's when all of the packs are initialized with all of their music.
    public class StardewSymphony : Mod
    {
        /*********
        ** Accessors
        *********/
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;
        public static MusicManager musicManager;
        public static bool menuChangedMusic;
        public static Config Config;
        public static TextureManager textureManager;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = this.Monitor;
            Config = helper.ReadConfig<Config>();
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            // EventArgsLocationsChanged += LocationEvents_CurrentLocationChanged;

            PlayerEvents.Warped += this.PlayerEvents_Warped;
            GameEvents.UpdateTick += this.GameEvents_UpdateTick;
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;
            SaveEvents.BeforeSave += this.SaveEvents_BeforeSave;

            MenuEvents.MenuChanged += this.MenuEvents_MenuChanged;
            MenuEvents.MenuClosed += this.MenuEvents_MenuClosed;

            GameEvents.FirstUpdateTick += this.GameEvents_FirstUpdateTick;
            GameEvents.OneSecondTick += this.GameEvents_OneSecondTick;


            musicManager = new MusicManager();
            textureManager = new TextureManager();
            this.LoadTextures();

            menuChangedMusic = false;


            //Initialize all of the lists upon creation during entry.
            SongSpecifics.initializeMenuList();
            SongSpecifics.initializeFestivalsList();

            this.LoadMusicPacks();
        }

        private void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            musicManager?.UpdateTimer();
        }

        /// <summary>Raised when the player changes locations. This should determine the next song to play.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void PlayerEvents_Warped(object sender, EventArgsPlayerWarped e)
        {
            musicManager.selectMusic(SongSpecifics.getCurrentConditionalString());

        }

        /// <summary>Ran once all of teh entry methods are ran. This will ensure that all custom music from other mods has been properly loaded in.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void GameEvents_FirstUpdateTick(object sender, EventArgs e)
        {
            musicManager.initializeMenuMusic(); //Initialize menu music that has been added to SongSpecifics.menus from all other mods during their Entry function.
            musicManager.initializeFestivalMusic(); //Initialize festival music that has been added to SongSpecifics.menus from all other mods during their Entry function.
            musicManager.initializeEventMusic(); //Initialize event music that has been added to SongSpecifics.menus from all other mods during their Entry function.
        }

        /// <summary>Events to occur after the game has loaded in.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            //Locaion initialization MUST occur after load. Anything else can occur before.
            SongSpecifics.initializeLocationsList(); //Gets all Game locations once the player has loaded the game, and all buildings on the player's farm and adds them to a location list.
            musicManager.initializeSeasonalMusic(); //Initialize the seasonal music using all locations gathered in the location list.
            musicManager.initializeMenuMusic();
            musicManager.initializeFestivalMusic();
            musicManager.initializeEventMusic();

            foreach (var musicPack in musicManager.MusicPacks)
                musicPack.Value.LoadSettings();

            SongSpecifics.menus.Sort();
            SongSpecifics.locations.Sort();
            SongSpecifics.festivals.Sort();
            SongSpecifics.events.Sort();

            musicManager.selectMusic(SongSpecifics.getCurrentConditionalString());
        }


        /// <summary>Choose new music when a menu is closed.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void MenuEvents_MenuClosed(object sender, EventArgsClickableMenuClosed e)
        {
            if (menuChangedMusic)
                musicManager.selectMusic(SongSpecifics.getCurrentConditionalString());
        }

        /// <summary>Choose new music when a menu is opened.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void MenuEvents_MenuChanged(object sender, EventArgsClickableMenuChanged e)
        {
            //var ok = musicManager.currentMusicPack.getNameOfCurrentSong();
            musicManager.SelectMenuMusic(SongSpecifics.getCurrentConditionalString());
        }

        private void SaveEvents_BeforeSave(object sender, EventArgs e)
        {
            // THIS IS WAY TO LONG to run. Better make it save individual lists when I am editing songs.
            foreach (var musicPack in musicManager.MusicPacks)
                musicPack.Value.SaveSettings();
        }

        /// <summary>Fires when a key is pressed to open the music selection menu.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed.ToString() == Config.KeyBinding && Game1.activeClickableMenu == null)
                Game1.activeClickableMenu = new Framework.Menus.MusicManagerMenu(Game1.viewport.Width, Game1.viewport.Height);
        }


        /// <summary>Raised every frame. Mainly used just to initiate the music packs. Probably not needed.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (musicManager == null) return;

            if (Config.DisableStardewMusic)
            {
                if (Game1.currentSong != null)
                {
                    Game1.currentSong.Stop(AudioStopOptions.Immediate); //stop the normal songs from playing over the new songs
                    Game1.currentSong.Stop(AudioStopOptions.AsAuthored);
                    Game1.nextMusicTrack = "";  //same as above line
                }
            }
            else
            {
                if (musicManager.CurrentMusicPack == null) return;
                if (Game1.currentSong != null && musicManager.CurrentMusicPack.IsPlaying())
                {
                    //ModMonitor.Log("STOP THE MUSIC!!!");
                    Game1.currentSong.Stop(AudioStopOptions.Immediate); //stop the normal songs from playing over the new songs
                    Game1.currentSong.Stop(AudioStopOptions.AsAuthored);
                    //Game1.nextMusicTrack = "";  //same as above line
                }
            }
        }

        /// <summary>Load the textures needed by the mod.</summary>
        public void LoadTextures()
        {
            Texture2DExtended LoadTexture(string name)
            {
                return new Texture2DExtended(this.Helper.Content.Load<Texture2D>($"assets/{name}"));
            }

            //Generic Icons
            textureManager.addTexture("MusicNote", LoadTexture("MusicNote.png"));
            textureManager.addTexture("MusicDisk", LoadTexture("MusicDisk.png"));
            textureManager.addTexture("MusicCD", LoadTexture("MusicDisk.png"));
            textureManager.addTexture("OutlineBox", LoadTexture("OutlineBox.png"));
            textureManager.addTexture("AddIcon", LoadTexture("AddButton.png"));
            textureManager.addTexture("DeleteIcon", LoadTexture("DeleteButton.png"));
            textureManager.addTexture("GreenBallon", LoadTexture("GreenBallon.png"));
            textureManager.addTexture("RedBallon", LoadTexture("RedBallon.png"));
            textureManager.addTexture("StarIcon", LoadTexture("StarIcon.png"));
            textureManager.addTexture("MenuIcon", LoadTexture("MenuIcon.png"));

            //Time Icons
            textureManager.addTexture("DayIcon", LoadTexture("TimeIcon_Day.png"));
            textureManager.addTexture("NightIcon", LoadTexture("TimeIcon_Night.png"));

            //Fun Icons
            textureManager.addTexture("EventIcon", LoadTexture("EventIcon.png"));
            textureManager.addTexture("FestivalIcon", LoadTexture("FestivalIcon.png"));

            //WeatherIcons
            textureManager.addTexture("SunnyIcon", LoadTexture("WeatherIcon_Sunny.png"));
            textureManager.addTexture("RainyIcon", LoadTexture("WeatherIcon_Rainy.png"));
            textureManager.addTexture("DebrisSpringIcon", LoadTexture("WeatherIcon_DebrisSpring.png"));
            textureManager.addTexture("DebrisSummerIcon", LoadTexture("WeatherIcon_DebrisSummer.png"));
            textureManager.addTexture("DebrisFallIcon", LoadTexture("WeatherIcon_DebrisFall.png"));
            textureManager.addTexture("WeatherFestivalIcon", LoadTexture("WeatherIcon_Festival.png"));
            textureManager.addTexture("SnowIcon", LoadTexture("WeatherIcon_Snowing.png"));
            textureManager.addTexture("StormIcon", LoadTexture("WeatherIcon_Stormy.png"));
            textureManager.addTexture("WeddingIcon", LoadTexture("WeatherIcon_WeddingHeart.png"));

            //Season Icons
            textureManager.addTexture("SpringIcon", LoadTexture("SeasonIcon_Spring.png"));
            textureManager.addTexture("SummerIcon", LoadTexture("SeasonIcon_Summer.png"));
            textureManager.addTexture("FallIcon", LoadTexture("SeasonIcon_Fall.png"));
            textureManager.addTexture("WinterIcon", LoadTexture("SeasonIcon_Winter.png"));

            //Day Icons
            textureManager.addTexture("MondayIcon", LoadTexture("DayIcons_Monday.png"));
            textureManager.addTexture("TuesdayIcon", LoadTexture("DayIcons_Tuesday.png"));
            textureManager.addTexture("WednesdayIcon", LoadTexture("DayIcons_Wednesday.png"));
            textureManager.addTexture("ThursdayIcon", LoadTexture("DayIcons_Thursday.png"));
            textureManager.addTexture("FridayIcon", LoadTexture("DayIcons_Friday.png"));
            textureManager.addTexture("SaturdayIcon", LoadTexture("DayIcons_Saturday.png"));
            textureManager.addTexture("SundayIcon", LoadTexture("DayIcons_Sunday.png"));

            textureManager.addTexture("HouseIcon", LoadTexture("HouseIcon.png"));

            textureManager.addTexture("PlayButton", LoadTexture("PlayButton.png"));
            textureManager.addTexture("StopButton", LoadTexture("StopButton.png"));
            textureManager.addTexture("BackButton", LoadTexture("BackButton.png"));
        }

        /// <summary>Load the available music packs.</summary>
        public void LoadMusicPacks()
        {
            foreach (IContentPack contentPack in this.Helper.ContentPacks.GetOwned())
            {
                MusicPack musicPack = new MusicPack(contentPack);
                musicPack.SongInformation.initializeMenuMusic();
                musicPack.LoadSettings();
                musicManager.addMusicPack(musicPack, true, true);
            }
        }

        public static void DebugLog(string s)
        {
            if (Config.EnableDebugLog)
                ModMonitor.Log(s);
        }
    }
}
