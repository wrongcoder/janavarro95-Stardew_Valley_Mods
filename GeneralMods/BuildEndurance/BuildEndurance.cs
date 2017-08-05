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
        /// <summary>The mod settings and player data.</summary>
        private ModConfig Config;

        /// <summary>Whether the player has been exhausted today.</summary>
        private bool WasExhausted;

        /// <summary>Whether the player has collapsed today.</summary>
        private bool WasCollapsed;

        /// <summary>The XP points needed to reach the next endurance level.</summary>
        private double ExpToNextLevel = 20;

        /// <summary>The player's current endurance XP points.</summary>
        private double CurrentExp;

        /// <summary>The player's current endurance level.</summary>
        private int CurrentLevel;

        /// <summary>The stamina points to add to the player's base stamina due to their current endurance level.</summary>
        private int CurrentLevelStaminaBonus;

        /// <summary>The initial stamina bonus to apply regardless of the player's endurance level, from the config file.</summary>
        private int BaseStaminaBonus;

        /// <summary>Whether to reset all changes by the mod to the default values (i.e. start over).</summary>
        private bool ClearModEffects;

        /// <summary>The player's original stamina value, excluding mod effects.</summary>
        private int OriginalStamina;

        /// <summary>Whether the player recently gained XP for tool use.</summary>
        private bool HasRecentToolExp;

        /// <summary>Whether the player was eating last time we checked.</summary>
        private bool WasEating;

        /// <summary>The player's stamina last time they slept.</summary>
        private int NightlyStamina;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            GameEvents.UpdateTick += this.GameEvents_UpdateTick;
            GameEvents.OneSecondTick += this.GameEvents_OneSecondTick;
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            TimeEvents.AfterDayStarted += this.TimeEvents_AfterDayStarted;

            this.Config = helper.ReadConfig<ModConfig>();
        }

        /// <summary>The method invoked once per second during a game update.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            // nerf how quickly tool xp is gained (I hope)
            if (this.HasRecentToolExp)
                this.HasRecentToolExp = false;
        }

        /// <summary>The method invoked when the game updates (roughly 60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            // give XP when player finishes eating
            if (Game1.isEating)
                this.WasEating = true;
            else if (this.WasEating)
            {
                this.CurrentExp += this.Config.ExpForEating;
                this.WasEating = false;
            }

            // give XP when player uses tool
            if (!this.HasRecentToolExp && Game1.player.usingTool)
            {
                this.CurrentExp += this.Config.ExpForToolUse;
                this.HasRecentToolExp = true;
            }

            // give XP when exhausted
            if (!this.WasExhausted && Game1.player.exhausted)
            {
                this.CurrentExp += this.Config.ExpForExhaustion;
                this.WasExhausted = true;
                this.Monitor.Log("The player is exhausted");
            }

            // give XP when player stays up too late or collapses
            if (!this.WasCollapsed && Game1.farmerShouldPassOut)
            {
                this.CurrentExp += this.Config.ExpForCollapsing;
                this.WasCollapsed = true;
                this.Monitor.Log("The player has collapsed!");
            }
        }

        /// <summary>The method invoked after the player loads a save.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            // initialise
            this.LoadConfig();
            this.WriteConfig();

            // grab initial stamina
            var player = Game1.player;
            if (this.OriginalStamina == 0)
                this.OriginalStamina = player.MaxStamina;

            // set nightly stamina
            player.MaxStamina = this.NightlyStamina;
            if (this.NightlyStamina == 0)
                player.MaxStamina = this.BaseStaminaBonus + this.CurrentLevelStaminaBonus + this.OriginalStamina;

            // reset if needed
            if (this.ClearModEffects)
                player.MaxStamina = this.OriginalStamina;

            // save config
            this.LoadConfig();
            this.WriteConfig();
        }

        /// <summary>The method invoked when a new day starts.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void TimeEvents_AfterDayStarted(object sender, EventArgs e)
        {
            // reset data
            this.WasExhausted = false;
            this.WasCollapsed = false;

            // update settings
            this.Monitor.Log(this.CurrentExp.ToString());
            this.UpdateClearSetting();
            this.Monitor.Log(this.ClearModEffects.ToString());

            var player = Game1.player;
            this.CurrentExp += this.Config.ExpForSleeping;
            if (this.OriginalStamina == 0)
                this.OriginalStamina = player.MaxStamina; //grab the initial stamina value

            if (this.ClearModEffects)
            {
                this.LoadClearSettings();
                player.MaxStamina = this.OriginalStamina;
                this.ExpToNextLevel = this.Config.ExpToNextLevel;
                this.CurrentExp = this.Config.CurrentExp;
                this.CurrentLevelStaminaBonus = 0;
                this.OriginalStamina = player.MaxStamina;
                this.BaseStaminaBonus = 0;
                this.CurrentLevel = 0;
            }

            if (!this.ClearModEffects && this.CurrentLevel < this.Config.MaxLevel)
            {
                while (this.CurrentExp >= this.ExpToNextLevel)
                {
                    this.CurrentLevel += 1;
                    this.CurrentExp = this.CurrentExp - this.ExpToNextLevel;
                    this.ExpToNextLevel = (this.Config.ExpCurve * this.ExpToNextLevel);
                    player.MaxStamina += this.Config.StaminaIncreasePerLevel;
                    this.CurrentLevelStaminaBonus += this.Config.StaminaIncreasePerLevel;

                }
            }
            this.ClearModEffects = false;
            this.NightlyStamina = Game1.player.maxStamina;
            this.WriteConfig();
        }

        /// <summary>Update the settings needed for <see cref="ClearModEffects"/> from the latest config file on disk.</summary>
        void LoadClearSettings()
        {
            this.LoadConfig();
            this.WriteConfig();

            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData")))
                Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));

            string path = Path.Combine(Helper.DirectoryPath, "PlayerData", $"BuildEndurance_data_{Game1.player.name}.txt");
            if (!File.Exists(path))
            {
                this.Monitor.Log("Clear Data Loaded could not find the correct file.");

                this.ClearModEffects = false;
                this.OriginalStamina = 0;
                this.BaseStaminaBonus = 0;
            }
            else
            {
                string[] text = File.ReadAllLines(path);
                this.BaseStaminaBonus = Convert.ToInt32(text[9]);
                this.ClearModEffects = Convert.ToBoolean(text[14]);
                this.OriginalStamina = Convert.ToInt32(text[16]);
            }
        }

        /// <summary>Update <see cref="ClearModEffects"/> based on the latest config file on disk.</summary>
        private void UpdateClearSetting()
        {
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData")))
                Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));

            string path = Path.Combine(Helper.DirectoryPath, "PlayerData", $"BuildEndurance_data_{Game1.player.name}.txt");
            if (!File.Exists(path))
            {
                this.Monitor.Log("Clear Data Loaded could not find the correct file.");

                this.ClearModEffects = false;
                this.OriginalStamina = 0;
                this.BaseStaminaBonus = 0;
            }
            else
            {
                string[] text = File.ReadAllLines(path);
                this.ClearModEffects = Convert.ToBoolean(text[14]);
            }
        }

        /// <summary>Load the configuration settings.</summary>
        void LoadConfig()
        {
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData")))
                Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));

            string path = Path.Combine(Helper.DirectoryPath, "PlayerData", $"BuildEndurance_data_{Game1.player.name}.txt");
            if (!File.Exists(path))
            {
                this.ExpToNextLevel = this.Config.ExpToNextLevel;
                this.CurrentExp = this.Config.CurrentExp;
                this.CurrentLevel = this.Config.CurrentLevel;
                this.BaseStaminaBonus = this.Config.BaseStaminaBonus;
                this.CurrentLevelStaminaBonus = this.Config.CurrentLevelStaminaBonus;
                this.ClearModEffects = false;
                this.OriginalStamina = 0;
            }
            else
            {
                string[] text = File.ReadAllLines(path);
                this.CurrentLevel = Convert.ToInt32(text[3]);
                this.ExpToNextLevel = Convert.ToDouble(text[7]);
                this.CurrentExp = Convert.ToDouble(text[5]);
                this.BaseStaminaBonus = Convert.ToInt32(text[9]);
                this.CurrentLevelStaminaBonus = Convert.ToInt32(text[11]);
                this.ClearModEffects = Convert.ToBoolean(text[14]);
                this.OriginalStamina = Convert.ToInt32(text[16]);
                this.NightlyStamina = Convert.ToInt32(text[18]);
            }
        }

        /// <summary>Save the configuration settings.</summary>
        void WriteConfig()
        {
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData")))
                Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));

            string path = Path.Combine(Helper.DirectoryPath, "PlayerData", $"BuildEndurance_data_{Game1.player.name}.txt");
            string[] text = new string[20];
            text[0] = "Player: Build Endurance Data. Modification can cause errors. Edit at your own risk.";
            text[1] = "====================================================================================";

            text[2] = "Player Current Level:";
            text[3] = this.CurrentLevel.ToString();

            text[4] = "Player Current XP:";
            text[5] = this.CurrentExp.ToString();

            text[6] = "Xp to next Level:";
            text[7] = this.ExpToNextLevel.ToString();

            text[8] = "Initial Stam Bonus:";
            text[9] = this.BaseStaminaBonus.ToString();

            text[10] = "Additional Stam Bonus:";
            text[11] = this.CurrentLevelStaminaBonus.ToString();

            text[12] = "=======================================================================================";
            text[13] = "RESET ALL MOD EFFECTS? This will effective start you back at square 1. Also good if you want to remove this mod.";
            text[14] = this.ClearModEffects.ToString();
            text[15] = "OLD STAMINA AMOUNT: This is the initial value of the Player's Stamina before this mod took over.";
            text[16] = this.OriginalStamina.ToString();

            text[17] = "Nightly Stamina Value: This is the value of the player's stamina that was saved when the player slept.";
            text[18] = this.NightlyStamina.ToString();

            File.WriteAllLines(path, text);
        }
    }
}
