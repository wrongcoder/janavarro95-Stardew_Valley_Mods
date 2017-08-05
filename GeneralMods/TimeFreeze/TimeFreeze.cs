using System;
using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;

namespace Omegasis.TimeFreeze
{
    /// <summary>The mod entry point.</summary>
    public class TimeFreeze : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>Whether time should be unfrozen while the player is swimming.</summary>
        private bool PassTimeWhileSwimming = true;

        /// <summary>Whether time should be unfrozen while the player is swimming in the vanilla bathhouse.</summary>
        private bool PassTimeWhileSwimmingInBathhouse = true;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            GameEvents.UpdateTick += this.GameEvents_UpdateTick;
            this.LoadConfig();
        }

        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked when the game updates (roughly 60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (this.ShouldFreezeTime(Game1.player, Game1.player.currentLocation))
                Game1.gameTimeInterval = 0;
        }

        /// <summary>Get whether time should be frozen for the player at the given location.</summary>
        /// <param name="player">The player to check.</param>
        /// <param name="location">The location to check.</param>
        private bool ShouldFreezeTime(StardewValley.Farmer player, GameLocation location)
        {
            if (location.name == "Mine" || location.name == "SkullCave" || location.name == "UndergroundMine" || location.isOutdoors)
                return false;
            if (player.swimming)
            {
                if (this.PassTimeWhileSwimmingInBathhouse && location is BathHousePool)
                    return false;
                if (this.PassTimeWhileSwimming)
                    return false;
            }
            return true;
        }

        /// <summary>Save the configuration settings.</summary>
        private void WriteConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, "ModConfig.txt");
            string[] text = new string[6];
            text[0] = "Player: TimeFreeze Config";
            text[1] = "====================================================================================";
            text[2] = "Whether to unfreeze time while swimming in the vanilla bathhouse.";
            text[3] = this.PassTimeWhileSwimmingInBathhouse.ToString();
            text[4] = "Whether to unfreeze time while swimming anywhere.";
            text[5] = this.PassTimeWhileSwimming.ToString();
            File.WriteAllLines(path, text);
        }

        /// <summary>Load the configuration settings.</summary>
        private void LoadConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, "ModConfig.txt");
            if (!File.Exists(path))
            {
                this.PassTimeWhileSwimming = true;
                this.PassTimeWhileSwimmingInBathhouse = true;
                this.WriteConfig();
            }
            else
            {
                string[] text = File.ReadAllLines(path);
                this.PassTimeWhileSwimming = Convert.ToBoolean(text[3]);
                this.PassTimeWhileSwimmingInBathhouse = Convert.ToBoolean(text[5]);
            }
        }
    }
}
