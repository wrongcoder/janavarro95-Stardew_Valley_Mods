using System.Collections.Generic;
using System.IO;
using StardewModdingAPI;
using StardewValley;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.SpriteFonts;

namespace StardustCore
{
    public class ModCore : Mod
    {
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;
        public static IManifest Manifest;
        public static TextureManager TextureManager;
        public static Dictionary<string, TextureManager> TextureManagers;

        public ModConfig config;

        public static string ContentDirectory;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ModHelper = this.Helper;
            ModMonitor = this.Monitor;
            Manifest = this.ModManifest;

            IlluminateFramework.Colors.initializeColors();
            ContentDirectory = "Content";
            if (!Directory.Exists(ContentDirectory)) Directory.CreateDirectory(Path.Combine(ModHelper.DirectoryPath, "Content"));
            SpriteFonts.initialize();

            TextureManagers = new Dictionary<string, TextureManager>();
            TextureManager = new TextureManager("StardustCore");
            TextureManager.addTexture("Test1", new Texture2DExtended(ModCore.ModHelper,Manifest,Path.Combine("Content", "Graphics", "MultiTest", "Test1.png")));
            TextureManager.addTexture("Test2", new Texture2DExtended(ModCore.ModHelper,Manifest, Path.Combine("Content", "Graphics", "MultiTest", "Test2.png")));
            TextureManager.addTexture("Test3", new Texture2DExtended(ModCore.ModHelper, Manifest,Path.Combine("Content", "Graphics", "MultiTest", "Test3.png")));
            TextureManagers.Add(this.ModManifest.UniqueID, TextureManager);

            this.Helper.Events.GameLoop.GameLaunched += this.GameLoop_GameLaunched;

            this.config = ModHelper.ReadConfig<ModConfig>();
        }

        private void GameLoop_GameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            string soundbankPath=Path.Combine(Game1.content.RootDirectory, "XACT", "Sound Bank.xsb");
            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "ProcessedGameFiles"));
            //this.Monitor.Log(Utilities.HexDumper.HexDumpString(soundbankPath), LogLevel.Info);
            Utilities.HexDumper.StripSoundCuesToFile(Path.Combine(this.Helper.DirectoryPath, "ProcessedGameFiles", "SoundCues.json"),Utilities.HexDumper.StripSoundCuesFromHex(Utilities.HexDumper.HexDumpString(soundbankPath)));
            //Utilities.HexDumper.HexDumpFile(soundbankPath, Path.Combine(this.Helper.DirectoryPath, "ProcessedGameFiles", "SoundCuesRaw.json"));
        }

        public static void log(string message)
        {
            ModMonitor.Log(message);
        }
    }
}
