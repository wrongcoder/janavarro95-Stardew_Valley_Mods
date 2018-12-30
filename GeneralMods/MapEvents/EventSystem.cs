using System;
using EventSystem.Framework;
using StardewModdingAPI;

namespace EventSystem
{
    // TODO: Make Bed/Sleep Event. 
    public class EventSystem : Mod
    {
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;

        public static EventManager eventManager;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ModHelper = this.Helper;
            ModMonitor = this.Monitor;
            StardewModdingAPI.Events.GameEvents.UpdateTick += this.GameEvents_UpdateTick;
            StardewModdingAPI.Events.SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            eventManager = new EventManager();
        }

        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            eventManager?.update();
        }
    }
}
