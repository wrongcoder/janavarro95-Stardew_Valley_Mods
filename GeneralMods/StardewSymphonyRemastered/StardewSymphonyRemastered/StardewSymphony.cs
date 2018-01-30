using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Audio;
using StardewModdingAPI;
using StardewValley;
using StardewSymphonyRemastered.Framework;

namespace StardewSymphonyRemastered
{

    /// <summary>
    /// BIG WIP. Don't use this at all because it does nothing right now.
    /// TODO:
    /// 1.Make Xwb packs work
    /// 1.5. Make way to load in music packs.
    /// 2.Make stream files work
    /// 2.5. Make Music Manager
    /// 3.Make interface.
    /// 4.Make sure stuff doesn't blow up.
    /// 5.Release
    /// 6.Make videos documenting how to make this mod work.
    /// 7.Make way to generate new music packs.
    /// </summary>
    public class StardewSymphony : Mod
    {
        public static WaveBank DefaultWaveBank;
        public static SoundBank DefaultSoundBank;


        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;

        public static MusicManager musicManager;


        public override void Entry(IModHelper helper)
        {
            DefaultSoundBank = Game1.soundBank;
            DefaultWaveBank = Game1.waveBank;
            ModHelper = helper;
            ModMonitor = Monitor;

            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_AfterLoad;
            StardewModdingAPI.Events.LocationEvents.CurrentLocationChanged += LocationEvents_CurrentLocationChanged;

            musicManager = new MusicManager();
            //load in all packs here.
        }

        /// <summary>
        /// Raised when the player changes locations. This should determine the next song to play.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocationEvents_CurrentLocationChanged(object sender, StardewModdingAPI.Events.EventArgsCurrentLocationChanged e)
        {
            musicManager.selectMusic(SongSpecifics.getCurrentConditionalString());
        }

        /// <summary>
        /// Events to occur after the game has loaded in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            StardewSymphonyRemastered.Framework.SongSpecifics.addLocations();
            StardewSymphonyRemastered.Framework.SongSpecifics.addFestivals();
            StardewSymphonyRemastered.Framework.SongSpecifics.addEvents();

           

           
        }


        /// <summary>
        /// Reset the music files for the game.
        /// </summary>
        public static void Reset()
        {
            Game1.waveBank = DefaultWaveBank;
            Game1.soundBank = DefaultSoundBank;
        }


    }
}
