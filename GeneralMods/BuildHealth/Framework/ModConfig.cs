namespace Omegasis.BuildHealth.Framework
{
    /// <summary>The mod settings and player data.</summary>
    internal class ModConfig
    {
        /// <summary>The XP points needed to reach the next level.</summary>
        public double ExpToNextLevel { get; set; }

        /// <summary>The player's current XP points.</summary>
        public double CurrentExp { get; set; }

        /// <summary>The player's current level.</summary>
        public int CurrentLevel { get; set; }

        /// <summary>The initial health bonus to apply regardless of the player's level, from the config file.</summary>
        public int BaseHealthBonus { get; set; }

        /// <summary>The health points to add to the player's base health due to their current level.</summary>
        public int CurrentLevelHealthBonus { get; set; }

        /// <summary>The multiplier for the experience points to need to reach an endurance level relative to the previous one.</summary>
        public double ExpCurve { get; set; }

        /// <summary>The maximum endurance level the player can reach.</summary>
        public int MaxLevel { get; set; }

        /// <summary>The amount of stamina the player should gain for each endurance level.</summary>
        public int HealthIncreasePerLevel { get; set; }

        /// <summary>The experience points to gain for using a tool.</summary>
        public int ExpForToolUse { get; set; }

        /// <summary>The experience points to gain for eating or drinking.</summary>
        public int ExpForEating { get; set; }

        /// <summary>The experience points to gain for sleeping.</summary>
        public int ExpForSleeping { get; set; }

        /// <summary>The experience points to gain for collapsing for the day.</summary>
        public int ExpForCollapsing { get; set; }
    }
}
