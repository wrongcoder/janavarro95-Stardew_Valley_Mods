using System;
using EventSystem.Framework.Events;
using EventSystem.Framework.FunctionEvents;
using EventSystem.Framework.Information;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
namespace SundropMapEvents
{
    public class Class1 : Mod
    {
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ModHelper = this.Helper;
            ModMonitor = this.Monitor;
            StardewModdingAPI.Events.SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            EventSystem.EventSystem.eventManager.addEvent(Game1.getLocationFromName("BusStop"), new WarpEvent("toRR", Game1.getLocationFromName("BusStop"), new Vector2(6, 11), new PlayerEvents(null, null), new WarpInformation("BusStop", 10, 12, 2, false)));
            EventSystem.EventSystem.eventManager.addEvent(Game1.getLocationFromName("BusStop"), new DialogueDisplayEvent("Hello.", Game1.getLocationFromName("BusStop"), new Vector2(10, 13), new MouseButtonEvents(null, null), new MouseEntryLeaveEvent(null, null), "Hello there!"));
        }
    }
}
