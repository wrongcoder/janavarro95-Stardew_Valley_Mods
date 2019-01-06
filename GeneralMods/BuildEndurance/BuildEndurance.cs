using System;
using System.IO;
using Omegasis.BuildEndurance.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Omegasis.BuildEndurance
{
    /// <summary>The mod entry point.</summary>
    public class BuildEndurance : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The relative path for the current player's data file.</summary>
        private string RelativeDataPath => Path.Combine("data", $"{Constants.SaveFolderName}.json");

        /// <summary>The mod settings.</summary>
        private ModConfig Config;

        /// <summary>The data for the current player.</summary>
        private PlayerData PlayerData;

        /// <summary>Whether the player has been exhausted today.</summary>
        private bool WasExhausted;

        /// <summary>Whether the player has collapsed today.</summary>
        private bool WasCollapsed;

        /// <summary>Whether the player recently gained XP for tool use.</summary>
        private bool HasRecentToolExp;

        /// <summary>Whether the player was eating last time we checked.</summary>
        private bool WasEating;

        public IModHelper ModHelper;
        public IMonitor ModMonitor;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();

            GameEvents.UpdateTick += this.GameEvents_UpdateTick;
            GameEvents.OneSecondTick += this.GameEvents_OneSecondTick;
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            SaveEvents.BeforeSave += this.SaveEvents_BeforeSave;

            this.ModHelper = this.Helper;
            this.ModMonitor = this.Monitor;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked once per second during a game update.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            // nerf how quickly tool xp is gained (I hope)
            if (this.HasRecentToolExp)
                this.HasRecentToolExp = false;
        }

        /// <summary>The method invoked when the game updates (roughly 60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            // give XP when player finishes eating
            if (Game1.player.isEating)
                this.WasEating = true;
            else if (this.WasEating)
            {
                this.PlayerData.CurrentExp += this.Config.ExpForEating;
                this.WasEating = false;
            }

            // give XP when player uses tool
            if (!this.HasRecentToolExp && Game1.player.UsingTool)
            {
                this.PlayerData.CurrentExp += this.Config.ExpForToolUse;
                this.HasRecentToolExp = true;
            }

            // give XP when exhausted
            if (!this.WasExhausted && Game1.player.exhausted.Value)
            {
                this.PlayerData.CurrentExp += this.Config.ExpForExhaustion;
                this.WasExhausted = true;
                //this.Monitor.Log("The player is exhausted");
            }

            // give XP when player stays up too late or collapses
            if (!this.WasCollapsed && this.shouldFarmerPassout())
            {

                this.PlayerData.CurrentExp += this.Config.ExpForCollapsing;
                this.WasCollapsed = true;
                //this.Monitor.Log("The player has collapsed!");
            }
        }

        /// <summary>The method invoked after the player loads a save.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            // reset state
            this.WasExhausted = false;
            this.WasCollapsed = false;
            this.HasRecentToolExp = false;
            this.WasEating = false;

            // load player data
            this.PlayerData = this.Helper.Data.ReadJsonFile<PlayerData>(this.RelativeDataPath) ?? new PlayerData();
            if (this.PlayerData.OriginalMaxStamina == 0)
                this.PlayerData.OriginalMaxStamina = Game1.player.MaxStamina;

            // reset if needed
            if (this.PlayerData.ClearModEffects)
            {
                Game1.player.MaxStamina = this.PlayerData.OriginalMaxStamina;
                this.PlayerData.ExpToNextLevel = this.Config.ExpToNextLevel;
                this.PlayerData.CurrentExp = this.Config.CurrentExp;
                this.PlayerData.CurrentLevelStaminaBonus = 0;
                this.PlayerData.OriginalMaxStamina = Game1.player.MaxStamina;
                this.PlayerData.BaseStaminaBonus = 0;
                this.PlayerData.CurrentLevel = 0;
                this.PlayerData.ClearModEffects = false;
            }

            // else apply stamina
            else
            {
                Game1.player.MaxStamina = this.PlayerData.NightlyStamina <= 0
                    ? this.PlayerData.BaseStaminaBonus + this.PlayerData.CurrentLevelStaminaBonus + this.PlayerData.OriginalMaxStamina
                    : this.PlayerData.NightlyStamina;
            }
        }

        /// <summary>The method invoked just before the game is saved.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void SaveEvents_BeforeSave(object sender, EventArgs e)
        {
            // reset data
            this.WasExhausted = false;
            this.WasCollapsed = false;

            // update player data
            this.PlayerData.CurrentExp += this.Config.ExpForSleeping;
            if (this.PlayerData.OriginalMaxStamina == 0)
                this.PlayerData.OriginalMaxStamina = Game1.player.MaxStamina; //grab the initial stamina value

            if (this.PlayerData.CurrentLevel < this.Config.MaxLevel)
            {
                while (this.PlayerData.CurrentExp >= this.PlayerData.ExpToNextLevel)
                {
                    this.PlayerData.CurrentLevel += 1;
                    this.PlayerData.CurrentExp = this.PlayerData.CurrentExp - this.PlayerData.ExpToNextLevel;
                    this.PlayerData.ExpToNextLevel = (this.Config.ExpCurve * this.PlayerData.ExpToNextLevel);
                    Game1.player.MaxStamina += this.Config.StaminaIncreasePerLevel;
                    this.PlayerData.CurrentLevelStaminaBonus += this.Config.StaminaIncreasePerLevel;
                }
            }
            this.PlayerData.ClearModEffects = false;
            this.PlayerData.NightlyStamina = Game1.player.MaxStamina;

            // save data
            this.Helper.Data.WriteJsonFile(this.RelativeDataPath, this.PlayerData);
        }

        /// <summary>Try and emulate the old Game1.shouldFarmerPassout logic.</summary>
        public bool shouldFarmerPassout()
        {
            if (Game1.player.stamina <= 0 || Game1.player.health <= 0 || Game1.timeOfDay >= 2600) return true;
            else return false;
        }
    }
}
