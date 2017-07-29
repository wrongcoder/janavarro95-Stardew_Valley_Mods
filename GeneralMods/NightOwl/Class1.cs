using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

/*TODO:
Issues:
-Mail can't be wiped without destroying all mail.
-Lighting transition does not work if it is raining.
        -set the weather to clear if you are stayig up late.
        -transition still doesnt work. However atleast it is dark now.

-Known glitched
*/
namespace Omegasis.NightOwl
{
    /// <summary>The mod entry point.</summary>
    public class Class1 : Mod
    {
        /*********
        ** Properties
        *********/
        /****
        ** Context
        ****/
        /// <summary>Whether the player loaded a save.</summary>
        private bool IsGameLoaded;

        /// <summary>Whether the player stayed up all night.</summary>
        private bool IsUpLate;

        /// <summary>Whether the player should be reset to their pre-collapse details for the morning transition on the next update.</summary>
        private bool ShouldResetPlayerAfterCollapseNow;

        /// <summary>Whether the player just started a new day.</summary>
        private bool JustStartedNewDay;

        /// <summary>Whether the player just collapsed for the morning transition.</summary>
        private bool JustCollapsed;

        /****
        ** Pre-collapse state
        ****/
        /// <summary>The player's location name before they collapsed.</summary>
        private string PreCollapseMap;

        /// <summary>The player's tile position before they collapsed.</summary>
        private Point PreCollapseTile;

        /// <summary>The player's money before they collapsed.</summary>
        private int PreCollapseMoney;

        /// <summary>The player's stamina before they collapsed.</summary>
        private float PreCollapseStamina;

        /// <summary>The player's health before they collapsed.</summary>
        private int PreCollapseHealth;

        /****
        ** Settings
        ****/
        /// <summary>Whether lighting should transition to day from 2am to 6am. If <c>false</c>, the world will stay dark until the player passes out or goes to bed.</summary>
        private bool MorningLightTransition;

        /// <summary>Whether the player can stay up until 6am.</summary>
        private bool StayUp;

        /// <summary>Whether to remove the mail received for collapsing like 'we charged X gold for your health fees'.</summary>
        private bool SkipCollapseMail;

        /// <summary>Whether to restore the player's position after they collapse.</summary>
        private bool KeepPositionAfterCollapse;

        /// <summary>Whether to restore the player's money after they collapse (i.e. prevent the automatic deduction).</summary>
        private bool KeepMoneyAfterCollapse;

        /// <summary>Whether to keep stamina as-is after the player collapses.</summary>
        private bool KeepHealthAfterCollapse;

        /// <summary>Whether to keep stamina as-is after the player collapses.</summary>
        private bool KeepStaminaAfterCollapse;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            TimeEvents.TimeOfDayChanged += this.TimeEvents_TimeOfDayChanged;
            TimeEvents.DayOfMonthChanged += this.TimeEvents_DayOfMonthChanged;
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            GameEvents.FourthUpdateTick += this.GameEvents_FourthUpdateTick;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked every fourth game update (roughly 15 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void GameEvents_FourthUpdateTick(object sender, EventArgs e)
        {
            try
            {
                // reset position after collapse
                if (!Game1.eventUp && this.IsGameLoaded && this.JustStartedNewDay && this.KeepPositionAfterCollapse)
                {
                    if (this.PreCollapseMap != null)
                        Game1.warpFarmer(this.PreCollapseMap, this.PreCollapseTile.X, this.PreCollapseTile.Y, false);

                    this.PreCollapseMap = null;
                    this.JustStartedNewDay = false;
                    this.JustCollapsed = false;
                }
            }
            catch (Exception ex)
            {
                this.Monitor.Log(ex.ToString(), LogLevel.Error);
                this.WriteErrorLog();
            }
        }

        /// <summary>The method invoked after the player loads a save.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            this.LoadConfig();
            this.WriteConfig();
            this.IsGameLoaded = true;
            this.IsUpLate = false;
            this.JustStartedNewDay = false;
            this.JustCollapsed = false;
        }

        /// <summary>The method invoked when <see cref="Game1.dayOfMonth"/> changes.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void TimeEvents_DayOfMonthChanged(object sender, EventArgsIntChanged e)
        {
            if (!this.IsGameLoaded)
                return;

            try
            {
                // reset data
                this.LoadConfig();
                this.WriteConfig();
                this.IsUpLate = false;
                Game1.farmerShouldPassOut = false;

                // transition to the next day
                if (this.ShouldResetPlayerAfterCollapseNow)
                {
                    this.ShouldResetPlayerAfterCollapseNow = false;

                    if (this.KeepStaminaAfterCollapse)
                        Game1.player.stamina = this.PreCollapseStamina;
                    if (this.KeepHealthAfterCollapse)
                        Game1.player.health = this.PreCollapseHealth;
                    if (this.KeepMoneyAfterCollapse)
                        Game1.player.money = this.PreCollapseMoney;
                    if (this.KeepPositionAfterCollapse)
                        Game1.warpFarmer(this.PreCollapseMap, this.PreCollapseTile.X, this.PreCollapseTile.Y, false);
                }

                // delete annoying charge messages (if only I could do this with mail IRL)
                if (this.SkipCollapseMail)
                {
                    string[] validMail = Game1.mailbox
                        .Where(p => !p.Contains("passedOut"))
                        .ToArray();

                    Game1.mailbox.Clear();
                    foreach (string mail in validMail)
                        Game1.mailbox.Enqueue(mail);
                }

                this.JustStartedNewDay = true;
            }
            catch (Exception ex)
            {
                this.Monitor.Log(ex.ToString(), LogLevel.Error);
                this.WriteErrorLog();
            }
        }

        /// <summary>The method invoked when <see cref="Game1.timeOfDay"/> changes.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void TimeEvents_TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            if (!this.IsGameLoaded)
                return;

            try
            {
                // transition morning light more realistically
                if (this.MorningLightTransition && Game1.timeOfDay > 400 && Game1.timeOfDay < 600)
                {
                    float colorMod = (1300 - Game1.timeOfDay) / 1000f;
                    Game1.outdoorLight = Game1.ambientLight * colorMod;
                }

                // transition to next morning
                if (this.StayUp && Game1.timeOfDay == 2550)
                {
                    Game1.isRaining = false; // remove rain, otherwise lighting gets screwy
                    Game1.updateWeatherIcon();
                    Game1.timeOfDay = 150; //change it from 1:50 am late, to 1:50 am early
                }

                // collapse player at 6am to save & reset
                if (Game1.timeOfDay == 550)
                    this.IsUpLate = true;
                if (this.IsUpLate && Game1.timeOfDay == 600 && !this.JustCollapsed)
                {
                    this.JustCollapsed = true;

                    this.ShouldResetPlayerAfterCollapseNow = true;
                    this.PreCollapseTile = new Point(Game1.player.getTileX(), Game1.player.getTileY());
                    this.PreCollapseMap = Game1.player.currentLocation.name;
                    this.PreCollapseStamina = Game1.player.stamina;
                    this.PreCollapseHealth = Game1.player.health;
                    this.PreCollapseMoney = Game1.player.money;

                    if (Game1.currentMinigame != null)
                        Game1.currentMinigame = null;
                    Game1.farmerShouldPassOut = true;
                }
            }
            catch (Exception ex)
            {
                this.Monitor.Log(ex.ToString(), LogLevel.Error);
                this.WriteErrorLog();
            }
        }

        /// <summary>Save the configuration settings.</summary>
        private void WriteConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, "Night_Owl_Config_.txt");
            string[] text = new string[20];

            text[0] = "Player: Night Owl Config. Feel free to edit.";
            text[1] = "====================================================================================";
            text[2] = "Whether you can stay up until 6am.";
            text[3] = this.StayUp.ToString();
            text[4] = "Whether the lighting should transition to daytime from 2am to 6am. Setting this to false will keep the world dark until the player passes out or goes to bed.";
            text[5] = this.MorningLightTransition.ToString();
            text[6] = "Whether to keep your position as-is after you collapse at 6am. If false, you'll warp back home.";
            text[7] = this.KeepPositionAfterCollapse.ToString();
            text[8] = "Whether to prevent money from being deducted after you collapse at 6am.";
            text[9] = this.KeepMoneyAfterCollapse.ToString();
            text[10] = "Whether to keep your stamina as-is after you collapse at 6am.";
            text[11] = this.KeepStaminaAfterCollapse.ToString();
            text[12] = "Whether to keep your health as-is after you collapse at 6am.";
            text[13] = this.KeepHealthAfterCollapse.ToString();
            text[14] = "Whether to remove the mail you receive for collapsing like 'we charged X gold for your health fees'.";
            text[15] = this.SkipCollapseMail.ToString();

            File.WriteAllLines(path, text);
        }

        /// <summary>Load the configuration settings.</summary>
        private void LoadConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, "Night_Owl_Config_.txt");
            if (!File.Exists(path)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                this.Monitor.Log("Loading Night_Owl_Config");
                this.MorningLightTransition = true;
                this.KeepPositionAfterCollapse = true;
                this.StayUp = true;

                this.KeepHealthAfterCollapse = true;
                this.KeepStaminaAfterCollapse = true;
                this.SkipCollapseMail = true;
                this.KeepMoneyAfterCollapse = true;
            }
            else
            {
                string[] text = File.ReadAllLines(path);
                this.StayUp = Convert.ToBoolean(text[3]);
                this.MorningLightTransition = Convert.ToBoolean(text[5]);
                this.KeepPositionAfterCollapse = Convert.ToBoolean(text[7]);
                this.KeepMoneyAfterCollapse = Convert.ToBoolean(text[9]);
                this.KeepStaminaAfterCollapse = Convert.ToBoolean(text[11]);
                this.KeepHealthAfterCollapse = Convert.ToBoolean(text[13]);
                this.SkipCollapseMail = text[15] == "" || Convert.ToBoolean(text[15]);
            }
        }

        /// <summary>Write the current mod state to the error log file.</summary>
        private void WriteErrorLog()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                string path = Path.Combine(this.Helper.DirectoryPath, "Error_Logs", "Mod_State.json");
                using (StreamWriter sw = new StreamWriter(path))
                {
                    using (JsonWriter writer2 = new JsonTextWriter(sw))
                        serializer.Serialize(writer2, this);
                }
            }
            catch (Exception ex)
            {
                this.Monitor.Log(ex.ToString(), LogLevel.Error);
            }
        }
    }
}
