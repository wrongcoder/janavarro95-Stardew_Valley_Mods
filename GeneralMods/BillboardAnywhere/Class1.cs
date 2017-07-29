using System;
using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace Omegasis.BillboardAnywhere
{
    /// <summary>The mod entry point.</summary>
    public class BillboardAnywhere : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The key which shows the billboard menu.</summary>
        private string KeyBinding = "B";

        /// <summary>Whether the player loaded a save.</summary>
        private bool IsGameLoaded;


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
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            this.IsGameLoaded = true;
            this.LoadConfig();
        }

        /// <summary>The method invoked when the presses a keyboard button.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (Game1.player == null || Game1.player.currentLocation == null || this.IsGameLoaded == false || Game1.activeClickableMenu != null)
                return;

            // load menu if key pressed
            if (e.KeyPressed.ToString() == this.KeyBinding)
                Game1.activeClickableMenu = new Billboard();
        }

        /// <summary>Load the configuration settings.</summary>
        void LoadConfig()
        {
            string path = Path.Combine(this.Helper.DirectoryPath, "Billboard_Anywhere_Config.txt");
            if (!File.Exists(path)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                this.KeyBinding = "B";
                this.WriteConfig();
            }
            else
            {
                string[] text = File.ReadAllLines(path);
                this.KeyBinding = Convert.ToString(text[3]);
            }
        }

        /// <summary>Save the configuration settings.</summary>
        void WriteConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, "Billboard_Anywhere_Config.txt");
            string[] text = new string[20];
            if (!File.Exists(path))
            {
                this.Monitor.Log("Billboard_Anywhere: The Billboard Anywhere Config doesn't exist. Creating it now.");
                text[0] = "Config: Billboard_Anywhere. Feel free to mess with these settings.";
                text[1] = "====================================================================================";
                text[2] = "Key binding for opening the billboard anywhere. Press this key to do so";
                text[3] = this.KeyBinding;
                File.WriteAllLines(path, text);
            }

            else
            {
                text[0] = "Config: Billboard_Anywhere Info. Feel free to mess with these settings.";
                text[1] = "====================================================================================";
                text[2] = "Key binding for opening the billboard anywhere. Press this key to do so";
                text[3] = this.KeyBinding;
                File.WriteAllLines(path, text);
            }
        }
    }
}
