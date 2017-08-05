using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Omegasis.BuildHealth.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Omegasis.BuildHealth
{
    /// <summary>The mod entry point.</summary>
    public class BuildHealth : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod settings and player data.</summary>
        private ModConfig Config;

        /// <summary>The XP points needed to reach the next level.</summary>
        private double ExpToNextLevel = 20;

        /// <summary>The player's current XP points.</summary>
        private double CurrentExp;

        /// <summary>The player's current level.</summary>
        private int CurrentLevel;

        /// <summary>The health points to add to the player's base health due to their current level.</summary>
        private int CurrentLevelHealthBonus;

        /// <summary>The initial health bonus to apply regardless of the player's level, from the config file.</summary>
        private int BaseHealthBonus;

        /// <summary>Whether to reset all changes by the mod to the default values (i.e. start over).</summary>
        private bool ClearModEffects;

        /// <summary>The player's original max health value, excluding mod effects.</summary>
        private int OriginalMaxHealth;

        /// <summary>Whether the player recently gained XP for tool use.</summary>
        private bool HasRecentToolExp;

        /// <summary>Whether the player was eating last time we checked.</summary>
        private bool WasEating;

        /// <summary>The player's health last time we checked it.</summary>
        private int LastHealth;

        /// <summary>Whether the player has collapsed today.</summary>
        private bool WasCollapsed;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            GameEvents.UpdateTick += this.GameEvents_UpdateTick;
            GameEvents.OneSecondTick += this.GameEvents_OneSecondTick;

            TimeEvents.AfterDayStarted += this.TimeEvents_AfterDayStarted;
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoaded;

            var configPath = Path.Combine(helper.DirectoryPath, "BuildHealthConfig.json");
            if (!File.Exists(configPath))
            {
                this.Config = new ModConfig
                {
                    CurrentLevel = 0,
                    MaxLevel = 100,
                    HealthIncreasePerLevel = 1,
                    CurrentExp = 0,
                    ExpToNextLevel = 20,
                    ExpCurve = 1.15,
                    ExpForEating = 2,
                    ExpForSleeping = 10,
                    ExpForToolUse = 1,
                    BaseHealthBonus = 0,
                    CurrentLevelHealthBonus = 0
                };
                File.WriteAllBytes(configPath, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.Config)));
            }
            else
            {
                this.Config = JsonConvert.DeserializeObject<ModConfig>(Encoding.UTF8.GetString(File.ReadAllBytes(configPath)));
                this.Monitor.Log("Found BuildHealth config file.");
            }

            this.Monitor.Log("BuildHealth Initialization Completed");
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

            // give XP for taking damage
            var player = Game1.player;
            if (this.LastHealth > player.health)
            {
                this.CurrentExp += this.LastHealth - player.health;
                this.LastHealth = player.health;
            }
            else if (this.LastHealth < player.health)
                this.LastHealth = player.health;

            // give XP when player stays up too late or collapses
            if (!this.WasCollapsed && Game1.farmerShouldPassOut)
            {
                this.CurrentExp += this.Config.ExpForCollapsing;
                this.WasCollapsed = true;
                this.Monitor.Log("The player has collapsed!");
            }
        }

        /// <summary>The method invoked after a new day starts.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void TimeEvents_AfterDayStarted(object sender, EventArgs e)
        {
            // reset data
            this.LastHealth = Game1.player.maxHealth;
            this.WasCollapsed = false;

            // update settings
            this.UpdateClearSetting();

            var player = Game1.player;
            this.CurrentExp += this.Config.ExpForSleeping;
            if (this.OriginalMaxHealth == 0)
                this.OriginalMaxHealth = player.maxHealth; //grab the initial Health value

            if (this.ClearModEffects)
            {
                this.LoadClearSettings();
                //This will run when the character goes to sleep. It will increase their sleeping skill.
                player.maxHealth = this.OriginalMaxHealth;
                this.ExpToNextLevel = this.Config.ExpToNextLevel;
                this.CurrentExp = this.Config.CurrentExp;
                this.CurrentLevelHealthBonus = 0;
                this.OriginalMaxHealth = player.maxHealth;
                this.BaseHealthBonus = 0;
                this.CurrentLevel = 0;
                this.Monitor.Log("BuildHealth Reset!");
            }

            if (!this.ClearModEffects && this.CurrentLevel < this.Config.MaxLevel)
            {
                while (this.CurrentExp >= this.ExpToNextLevel)
                {
                    this.CurrentLevel += 1;
                    this.CurrentExp = this.CurrentExp - this.ExpToNextLevel;
                    this.ExpToNextLevel =
                        (this.Config.ExpCurve * this.ExpToNextLevel);
                    player.maxHealth += this.Config.HealthIncreasePerLevel;
                    this.CurrentLevelHealthBonus += this.Config.HealthIncreasePerLevel;
                }
            }
            this.ClearModEffects = false;

            this.WriteConfig();
        }

        /// <summary>The method invoked after the player loads a save.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void SaveEvents_AfterLoaded(object sender, EventArgs e)
        {
            // initialise
            this.LoadConfig();
            this.WriteConfig();

            // grab initial health
            var player = Game1.player;
            if (this.OriginalMaxHealth == 0)
                this.OriginalMaxHealth = player.maxHealth;

            // set max health
            player.maxHealth = this.BaseHealthBonus + this.CurrentLevelHealthBonus + this.OriginalMaxHealth;

            // reset if needed
            if (this.ClearModEffects)
            {
                player.maxHealth = this.OriginalMaxHealth;
                this.Monitor.Log("BuildHealth Reset!");
            }

            // save config
            this.LastHealth = Game1.player.maxHealth;
            this.LoadConfig();
            this.WriteConfig();
        }

        /// <summary>Update the settings needed for <see cref="ClearModEffects"/> from the latest config file on disk.</summary>
        void LoadClearSettings()
        {
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData")))
                Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));

            string path = Path.Combine(Helper.DirectoryPath, "PlayerData", $"BuildHealth_data_{Game1.player.name}.txt");
            if (!File.Exists(path))
            {
                this.ClearModEffects = false;
                this.OriginalMaxHealth = 0;
                this.BaseHealthBonus = 0;
            }
            else
            {
                string[] text = File.ReadAllLines(path);
                this.BaseHealthBonus = Convert.ToInt32(text[9]);
                this.ClearModEffects = Convert.ToBoolean(text[14]);
                this.OriginalMaxHealth = Convert.ToInt32(text[16]);
            }
        }

        /// <summary>Update <see cref="ClearModEffects"/> based on the latest config file on disk.</summary>
        private void UpdateClearSetting()
        {
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData")))
                Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));

            string path = Path.Combine(Helper.DirectoryPath, "PlayerData", $"BuildHealth_data_{Game1.player.name}.txt");
            if (!File.Exists(path))
            {
                this.ClearModEffects = false;
                this.OriginalMaxHealth = 0;
                this.BaseHealthBonus = 0;
            }
            else
            {
                string[] text = File.ReadAllLines(path);
                this.ClearModEffects = Convert.ToBoolean(text[14]);
            }
        }

        /// <summary>Load the configuration settings.</summary>
        private void LoadConfig()
        {
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData")))
                Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));

            string path = Path.Combine(Helper.DirectoryPath, "PlayerData", $"BuildHealth_data_{Game1.player.name}.txr");
            if (!File.Exists(path))
            {
                this.ExpToNextLevel = this.Config.ExpToNextLevel;
                this.CurrentExp = this.Config.CurrentExp;
                this.CurrentLevel = this.Config.CurrentLevel;
                this.BaseHealthBonus = this.Config.BaseHealthBonus;
                this.CurrentLevelHealthBonus = this.Config.CurrentLevelHealthBonus;
                this.ClearModEffects = false;
                this.OriginalMaxHealth = 0;
            }
            else
            {
                string[] text = File.ReadAllLines(path);
                this.CurrentLevel = Convert.ToInt32(text[3]);
                this.ExpToNextLevel = Convert.ToDouble(text[7]);
                this.CurrentExp = Convert.ToDouble(text[5]);
                this.BaseHealthBonus = Convert.ToInt32(text[9]);
                this.CurrentLevelHealthBonus = Convert.ToInt32(text[11]);
                this.ClearModEffects = Convert.ToBoolean(text[14]);
                this.OriginalMaxHealth = Convert.ToInt32(text[16]);
            }
        }

        /// <summary>Save the configuration settings.</summary>
        private void WriteConfig()
        {
            if (!Directory.Exists(Path.Combine(Helper.DirectoryPath, "PlayerData")))
                Directory.CreateDirectory(Path.Combine(Helper.DirectoryPath, "PlayerData"));

            string path = Path.Combine(Helper.DirectoryPath, "PlayerData", $"BuildHealth_data_{Game1.player.name}.txt");
            string[] text = new string[20];
            text[0] = "Player: Build Health Data. Modification can cause errors. Edit at your own risk.";
            text[1] = "====================================================================================";

            text[2] = "Player Current Level:";
            text[3] = this.CurrentLevel.ToString();

            text[4] = "Player Current XP:";
            text[5] = this.CurrentExp.ToString();

            text[6] = "Xp to next Level:";
            text[7] = this.ExpToNextLevel.ToString();

            text[8] = "Initial Health Bonus:";
            text[9] = this.BaseHealthBonus.ToString();

            text[10] = "Additional Health Bonus:";
            text[11] = this.CurrentLevelHealthBonus.ToString();

            text[12] = "=======================================================================================";
            text[13] = "RESET ALL MOD EFFECTS? This will effective start you back at square 1. Also good if you want to remove this mod.";
            text[14] = this.ClearModEffects.ToString();
            text[15] = "OLD Health AMOUNT: This is the initial value of the Player's Health before this mod took over.";
            text[16] = this.OriginalMaxHealth.ToString();

            File.WriteAllLines(path, text);
        }
    }
}
