using System.Collections.Generic;
using StardewModdingAPI;

namespace StardewSymphonyRemastered
{
    /// <summary>A class that handles all of the config files for this mod.</summary>
    public class Config
    {
        /// <summary>Whether to show debug logs in the SMAPI console.</summary>
        public bool EnableDebugLog { get; set; } = false;

        /// <summary>The minimum delay between songs in milliseconds.</summary>
        public int MinimumDelayBetweenSongsInMilliseconds { get; set; } = 5000;

        /// <summary>The maximum delay between songs in milliseconds.</summary>
        public int MaximumDelayBetweenSongsInMilliseconds { get; set; } = 60000;

        /// <summary>The key binding to open the menu music.</summary>
        public SButton KeyBinding { get; set; } = SButton.L;

        /// <summary>Whether to completely disable the Stardew Valley OST.</summary>
        public bool DisableStardewMusic { get; set; } = false;

        public List<string> LocationsToIgnoreWarpMusicChange { get; set; } = new List<string>();

        public Config()
        {
            this.LocationsToIgnoreWarpMusicChange = new List<string>();
        }
    }
}
