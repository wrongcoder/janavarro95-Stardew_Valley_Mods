using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Omegasis.NoMorePets
{
    /// <summary>The mod entry point.</summary>
    public class NoMorePets : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            GameEvents.UpdateTick += this.GameEvents_UpdateTick;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked when the game updates (roughly 60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            string petName = Game1.player.getPetName();
            if (Game1.player.currentLocation is Farm)
            {
                foreach (NPC npc in Game1.player.currentLocation.characters.ToArray())
                {
                    if (npc.name == petName)
                        Game1.removeCharacterFromItsLocation(petName);
                }
            }
        }
    }
}
