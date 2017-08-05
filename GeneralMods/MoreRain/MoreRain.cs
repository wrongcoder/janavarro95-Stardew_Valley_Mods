using System;
using System.Collections.Generic;
using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Omegasis.MoreRain
{
    /// <summary>The mod entry point.</summary>
    public class MoreRain : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The weathers that can be safely overridden.</summary>
        private readonly HashSet<int> NormalWeathers = new HashSet<int> { Game1.weather_sunny, Game1.weather_rain, Game1.weather_lightning, Game1.weather_debris, Game1.weather_snow };

        /// <summary>The chance out of 100 that it will rain tomorrow if it's spring.</summary>
        private int SpringRainChance;

        /// <summary>The chance out of 100 that it will storm tomorrow if it's spring.</summary>
        private int SpringThunderChance;

        /// <summary>The chance out of 100 that it will rain tomorrow if it's summer.</summary>
        private int SummerRainChance;

        /// <summary>The chance out of 100 that it will storm tomorrow if it's summer.</summary>
        private int SummerThunderChance;

        /// <summary>The chance out of 100 that it will rain tomorrow if it's fall.</summary>
        private int FallRainChance;

        /// <summary>The chance out of 100 that it will storm tomorrow if it's fall.</summary>
        private int FallThunderChance;

        /// <summary>The chance out of 100 that it will snow tomorrow if it's winter.</summary>
        private int WinterSnowChance;

        /// <summary>Whether the player loaded a save.</summary>
        private bool IsGameLoaded;

        /// <summary>Whether to suppress verbose logging.</summary>
        private bool SuppressLog;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            SaveEvents.BeforeSave += this.SaveEvents_BeforeSave;
            this.LoadConfig();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked after the player loads a save.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            this.IsGameLoaded = true;
            this.HandleNewDay();
        }

        /// <summary>The method invoked before the game is saved.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void SaveEvents_BeforeSave(object sender, EventArgs e)
        {
            if (this.IsGameLoaded)
                this.HandleNewDay();
        }

        /// <summary>Update all data for a new day.</summary>
        private void HandleNewDay()
        {
            // skip if special weather
            if (!this.NormalWeathers.Contains(Game1.weatherForTomorrow))
            {
                if (Game1.weatherForTomorrow == Game1.weather_festival)
                    this.VerboseLog("There is a festival tomorrow, therefore it will not rain.");
                else if (Game1.weatherForTomorrow == Game1.weather_wedding)
                    this.VerboseLog("There is a wedding tomorrow and rain on your wedding day will not happen.");
                else
                    this.VerboseLog("The weather tomorrow is unknown, so it will not rain.");
                return;
            }

            // set weather
            Random random = new Random();
            int chance = random.Next(0, 100);
            switch (Game1.currentSeason)
            {
                case "spring":
                    // set rain
                    if (chance <= this.SpringRainChance)
                    {
                        Game1.weatherForTomorrow = Game1.weather_rain;
                        this.VerboseLog("It will rain tomorrow.");
                    }
                    else
                    {
                        Game1.weatherForTomorrow = Game1.weather_sunny;
                        this.VerboseLog("It will not rain tomorrow.");
                    }

                    // set storm
                    if (Game1.weatherForTomorrow == Game1.weather_rain)
                    {
                        if (chance <= this.SpringThunderChance)
                        {
                            Game1.weatherForTomorrow = Game1.weather_lightning;
                            this.VerboseLog("It will be stormy tomorrow.");
                        }
                        else
                        {
                            Game1.weatherForTomorrow = Game1.weather_rain;
                            this.VerboseLog("There will be no lightning tomorrow.");
                        }
                    }
                    break;

                case "summer":
                    // set rain
                    if (chance <= this.SummerRainChance)
                    {
                        Game1.weatherForTomorrow = Game1.weather_rain;
                        this.VerboseLog("It will rain tomorrow.");
                    }
                    else
                    {
                        Game1.weatherForTomorrow = Game1.weather_sunny;
                        this.VerboseLog("It will not rain tomorrow.");
                    }

                    // set storm
                    if (Game1.weatherForTomorrow == Game1.weather_rain)
                    {
                        if (chance <= this.SummerThunderChance)
                        {
                            Game1.weatherForTomorrow = Game1.weather_lightning;
                            this.VerboseLog("It will be stormy tomorrow.");
                        }
                        else
                        {
                            Game1.weatherForTomorrow = Game1.weather_rain;
                            this.VerboseLog("There will be no lightning tomorrow.");
                        }
                    }
                    break;

                case "fall":
                case "autumn":
                    // set rain
                    if (chance <= this.FallRainChance)
                    {
                        Game1.weatherForTomorrow = Game1.weather_rain;
                        this.VerboseLog("It will rain tomorrow.");
                    }
                    else
                    {
                        Game1.weatherForTomorrow = Game1.weather_sunny;
                        this.VerboseLog("It will not rain tomorrow.");
                    }

                    // set storm
                    if (Game1.weatherForTomorrow == Game1.weather_rain)
                    {
                        if (chance <= this.FallThunderChance)
                        {
                            Game1.weatherForTomorrow = Game1.weather_lightning;
                            this.VerboseLog("It will be stormy tomorrow.");
                        }
                        else
                        {
                            Game1.weatherForTomorrow = Game1.weather_rain;
                            this.VerboseLog("There will be no lightning tomorrow.");
                        }
                    }
                    break;

                case "winter":
                    // set snow
                    if (chance <= this.WinterSnowChance)
                    {
                        Game1.weatherForTomorrow = Game1.weather_snow;
                        this.VerboseLog("It will snow tomorrow.");
                    }
                    else
                    {
                        //StardewValley.Game1.weatherForTomorrow = StardewValley.Game1.weather_sunny;
                        this.VerboseLog("It will not snow tomorrow.");
                    }
                    break;
            }
        }

        /// <summary>Save the configuration settings.</summary>
        private void SaveConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, "More_Rain_Config.txt");
            string[] text = new string[20];
            text[0] = "Player: More Rain Config. Feel free to edit.";
            text[1] = "====================================================================================";
            text[2] = "Spring Rain chance: The chance out of 100 that it will rain tomorrow.";
            text[3] = this.SpringRainChance.ToString();
            text[4] = "Spring Storm chance: The chance out of 100 that it will be stormy tomorrow.";
            text[5] = this.SpringThunderChance.ToString();

            text[6] = "Summer Rain chance: The chance out of 100 that it will rain tomorrow.";
            text[7] = this.SummerRainChance.ToString();
            text[8] = "Summer Storm chance: The chance out of 100 that it will be stormy tomorrow.";
            text[9] = this.SummerThunderChance.ToString();

            text[10] = "Fall Rain chance: The chance out of 100 that it will rain tomorrow.";
            text[11] = this.FallRainChance.ToString();
            text[12] = "Fall Storm chance: The chance out of 100 that it will be stormy tomorrow.";
            text[13] = this.FallThunderChance.ToString();

            text[14] = "Winter Snow chance: The chance out of 100 that it will rain tomorrow.";
            text[15] = this.WinterSnowChance.ToString();

            text[16] = "Supress Log: If true, the mod won't output any messages to the console.";
            text[17] = this.SuppressLog.ToString();

            File.WriteAllLines(path, text);
        }

        /// <summary>Load the configuration settings.</summary>
        private void LoadConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, $"More_Rain_Config.txt");
            if (!File.Exists(path))
            {
                this.SpringRainChance = 15;
                this.SummerRainChance = 5;
                this.FallRainChance = 15;
                this.WinterSnowChance = 15;

                this.SpringThunderChance = 5;
                this.SummerThunderChance = 10;
                this.FallThunderChance = 5;

                this.SuppressLog = true;
                this.SaveConfig();
            }
            else
            {
                try
                {
                    string[] text = File.ReadAllLines(path);
                    this.SpringRainChance = Convert.ToInt32(text[3]);
                    this.SpringThunderChance = Convert.ToInt32(text[5]);
                    this.SummerRainChance = Convert.ToInt32(text[7]);
                    this.SummerThunderChance = Convert.ToInt32(text[9]);
                    this.FallRainChance = Convert.ToInt32(text[11]);
                    this.FallThunderChance = Convert.ToInt32(text[13]);
                    this.WinterSnowChance = Convert.ToInt32(text[15]);
                    this.SuppressLog = Convert.ToBoolean(text[17]);
                }
                catch (Exception) //something dun goofed
                {
                    this.SpringRainChance = 15;
                    this.SummerRainChance = 5;
                    this.FallRainChance = 15;
                    this.WinterSnowChance = 15;

                    this.SpringThunderChance = 5;
                    this.SummerThunderChance = 10;
                    this.FallThunderChance = 5;

                    this.SuppressLog = true;
                    this.SaveConfig();
                }
            }
        }

        /// <summary>Log a message if <see cref="SuppressLog"/> is <c>false</c>.</summary>
        /// <param name="message">The message to log.</param>
        private void VerboseLog(string message)
        {
            if (!this.SuppressLog)
                this.Monitor.Log(message);
        }
    }
}
