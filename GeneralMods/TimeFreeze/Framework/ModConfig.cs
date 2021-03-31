using System.Collections.Generic;

namespace Omegasis.TimeFreeze.Framework
{
    /// <summary>The mod configuration.</summary>
    internal class ModConfig
    {
        //public List<string> LocationsToIgnoreTimeFreeze { get; set; } = new List<string>();

        public SortedDictionary<string, bool> freezeTimeInThisLocation { get; set; } = new SortedDictionary<string, bool>();

        /// <summary>Whether time should be unfrozen while the player is swimming in the vanilla bathhouse.</summary>
        public bool PassTimeWhileSwimmingInBathhouse { get; set; } = true;

        /// <summary>Checks if just one player meets the conditions to freeze time, and then freeze time.</summary>
        public bool freezeIfEvenOnePlayerMeetsTimeFreezeConditions { get; set; } = false;

        /// <summary>Checks if the majority of players can freeze time and then freeze time.</summary>
        public bool freezeIfMajorityPlayersMeetsTimeFreezeConditions { get; set; } = true;

        /// <summary>Checks if all players can freeze time and if so, do so.</summary>
        public bool freezeIfAllPlayersMeetTimeFreezeConditions { get; set; } = false;
    }
}
