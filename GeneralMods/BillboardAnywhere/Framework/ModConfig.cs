using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace Omegasis.BillboardAnywhere.Framework
{
    /// <summary>The mod configuration.</summary>
    internal class ModConfig
    {
        /// <summary>The key which shows the billboard menu.</summary>
        public SButton CalendarKeyBinding { get; set; } = SButton.B;
        /// <summary>The key which shows the quest menu.</summary>
        public SButton QuestBoardKeyBinding { get; set; } = SButton.H;
        /// <summary>The key which shows the special order menu</summary>
        public SButton SpecialOrderKeyBinding { get; set; } = SButton.N;
        /// <summary>The key which shows the qi board menu</summary>
        public SButton QiBoardKeyBinding { get; set;} = SButton.J;

        /// <summary>The offset for the calendar button from the active menu</summary>
        public Vector2 CalendarOffsetFromMenu { get; set; } = new Vector2(-100, 0);
        /// <summary>The offset for the quest button from the active menu</summary>
        public Vector2 QuestOffsetFromMenu { get; set; } = new Vector2(-200, 0);
        /// <summary>The offset for the special order button from the active menu</summary>
        public Vector2 SpecialOrderOffsetFromMenu { get; set; } = new Vector2(-100, 100);
        /// <summary>The offset for the qi menu button from the active menu</summary>
        public Vector2 QiOffsetFromMenu { get; set; } = new Vector2(-200, 100);

        /// <summary>
        /// If true the calendar button is enabled for the in-game menu.
        /// </summary>
        public bool EnableInventoryCalendarButton { get; set; } = true;
        /// <summary>
        /// If true the quest button is enabled for the in-game menu.
        /// </summary>
        public bool EnableInventoryQuestButton { get; set; } = true;
        /// <summary>
        /// If true the special orders button is enabled for the in-game menu.
        /// </summary>
        public bool EnableSpecialOrdersButton { get; set; } = true;
        /// <summary>
        /// If true the qi board button is enabled for the in-game menu.
        /// </summary>
        public bool EnableQiBoardButton { get; set; } = true;
    }
}
