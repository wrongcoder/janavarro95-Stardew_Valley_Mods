using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Audio;
using StardewModdingAPI;
using StardewValley;

namespace StardewSymphonyRemastered
{

    /// <summary>
    /// BIG WIP. Don't use this at all because it does nothing right now.
    /// TODO:
    /// 1.Make Xwb packs work
    /// 2.Make stream files work
    /// 2.5. Make Music Manager
    /// 3.Make interface.
    /// 4.Make sure stuff doesn't blow up.
    /// 5.Release
    /// 6.Make videos documenting how to make this mod work.
    /// 7.Make way to generate new music packs.
    /// </summary>
    public class Class1 : Mod
    {
        public static WaveBank DefaultWaveBank;
        public static SoundBank DefaultSoundBank;


        public override void Entry(IModHelper helper)
        {
            DefaultSoundBank = Game1.soundBank;
            DefaultWaveBank = Game1.waveBank;

            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_AfterLoad;
        }

        /// <summary>
        /// 
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
