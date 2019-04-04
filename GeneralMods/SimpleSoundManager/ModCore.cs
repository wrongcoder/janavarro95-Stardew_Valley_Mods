using SimpleSoundManager.Framework;
using SimpleSoundManager.Framework.Music;
using StardewModdingAPI;

namespace SimpleSoundManager
{
    public class ModCore : Mod
    {
        internal static IModHelper ModHelper;
        internal static IMonitor ModMonitor;
        internal static Config Config;
        public static MusicManager MusicManager;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = this.Monitor;
            Config = helper.ReadConfig<Config>();
            this.loadContentPacks();
            this.Helper.Events.GameLoop.OneSecondUpdateTicked += this.GameLoop_OneSecondUpdateTicked;
        }

        /// <summary>
        /// Update all music packs every second.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameLoop_OneSecondUpdateTicked(object sender, StardewModdingAPI.Events.OneSecondUpdateTickedEventArgs e)
        {
            foreach(MusicPack pack in MusicManager.MusicPacks.Values)
            {
                pack.update();
            }
        }

        /// <summary>
        /// Loads all content packs for SimpleSoundManager
        /// </summary>
        private void loadContentPacks()
        {
            MusicManager = new MusicManager();
            foreach (IContentPack contentPack in this.Helper.ContentPacks.GetOwned())
            {
                this.Monitor.Log($"Reading content pack: {contentPack.Manifest.Name} {contentPack.Manifest.Version} from {contentPack.DirectoryPath}");
                MusicPack musicPack = new MusicPack(contentPack);
                MusicManager.addMusicPack(musicPack, true, true);
            }
        }

        /// <summary>
        /// Easy way to display debug logs when allowing for a check to see if they are enabled.
        /// </summary>
        /// <param name="s">The message to display.</param>
        public static void DebugLog(string s)
        {
            if (Config.EnableDebugLog)
                ModMonitor.Log(s);
        }
    }
}
