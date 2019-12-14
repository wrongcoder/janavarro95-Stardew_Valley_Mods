using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.Events
{
    /// <summary>
    /// Used to parse additional
    /// </summary>
    public static class EventExtensions
    {
        /// <summary>
        /// Gets the current command string with all of it's data.
        /// </summary>
        /// <param name="CurrentEvent"></param>
        /// <returns></returns>
        public static string currentCommandString(this StardewValley.Event CurrentEvent)
        {
            if (CurrentEvent.currentCommand >= CurrentEvent.eventCommands.Length) return "";
            return CurrentEvent.eventCommands.ElementAt(CurrentEvent.currentCommand);
        }

        /// <summary>
        /// Gets the name of the current command string.
        /// </summary>
        /// <param name="CurrentEvent"></param>
        /// <returns></returns>
        public static string currentCommandStringName(this StardewValley.Event CurrentEvent)
        {
            if (CurrentEvent.currentCommand >= CurrentEvent.eventCommands.Length) return "";
            return CurrentEvent.eventCommands.ElementAt(CurrentEvent.currentCommand).Split(' ')[0];
        }

    }
}
