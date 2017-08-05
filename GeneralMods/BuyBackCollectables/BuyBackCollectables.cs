using System;
using System.IO;
using Omegasis.BuyBackCollectables.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Omegasis.BuyBackCollectables
{
    /// <summary>The mod entry point.</summary>
    public class BuyBackCollectables : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The key which shows the menu.</summary>
        private string KeyBinding = "B";

        /// <summary>The multiplier applied to the cost of buying back a collectable.</summary>
        private double CostMultiplier = 3.0;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked after the player loads a save.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            this.LoadConfig();
            this.WriteConfig();
        }

        /// <summary>The method invoked when the presses a keyboard button.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (Context.IsPlayerFree && e.KeyPressed.ToString() == this.KeyBinding)
                Game1.activeClickableMenu = new BuyBackMenu(this.CostMultiplier);
        }

        /// <summary>Load the configuration settings.</summary>
        private void LoadConfig()
        {
            //loads the data to the variables upon loading the game.
            string path = Path.Combine(Helper.DirectoryPath, "BuyBack_Config.txt");
            if (!File.Exists(path))
            {
                this.KeyBinding = "B";
                this.CostMultiplier = 3.0;
            }
            else
            {
                string[] text = File.ReadAllLines(path);
                this.KeyBinding = Convert.ToString(text[3]);
                this.CostMultiplier = Convert.ToDouble(text[5]);
            }
        }

        /// <summary>Save the configuration settings.</summary>
        private void WriteConfig()
        {
            //write all of my info to a text file.
            string path = Path.Combine(Helper.DirectoryPath, "BuyBack_Config.txt");
            string[] text = new string[20];
            text[0] = "Config: Buy Back Collections. Feel free to mess with these settings.";
            text[1] = "====================================================================================";
            text[2] = "Key binding";
            text[3] = this.KeyBinding;
            text[4] = "Collectables Multiplier Cost: Sell Value * value listed below";
            text[5] = this.CostMultiplier.ToString();
            File.WriteAllLines(path, text);
        }
    }
}
