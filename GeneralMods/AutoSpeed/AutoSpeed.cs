using System;
using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Omegasis.AutoSpeed
{
    /// <summary>The mod entry point.</summary>
    public class AutoSpeed : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The speed multiplier.</summary>
        private int Speed = 5;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            GameEvents.UpdateTick += this.GameEvents_UpdateTick;

            string configLocation = Path.Combine(helper.DirectoryPath, "AutoSpeed_Data.txt");
            if (!File.Exists(configLocation))
                this.Speed = 1;

            this.LoadConfig();
            this.Monitor.Log("AutoSpeed Initialization Completed", LogLevel.Info);
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked when the game updates (roughly 60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            Game1.player.addedSpeed = Speed;
        }

        /// <summary>Load the configuration settings.</summary>
        private void LoadConfig()
        {
            string path = Path.Combine(this.Helper.DirectoryPath, "AutoSpeed_data.txt");
            if (!File.Exists(path))
                this.WriteConfig();
            else
            {
                string[] text = File.ReadAllLines(path);
                this.Speed = Convert.ToInt32(text[3]);
            }
        }

        /// <summary>Save the configuration settings.</summary>
        void WriteConfig()
        {
            string path = Path.Combine(this.Helper.DirectoryPath, "AutoSpeed_data.txt");
            string[] text = new string[20];
            text[0] = "Player: AutoSpeed Config:";
            text[1] = "====================================================================================";
            text[2] = "Player Added Speed:";
            text[3] = Speed.ToString();
            File.WriteAllLines(path, text);
        }
    }
}
