using System;
using System.IO;

namespace Omegasis.SaveAnywhere.Framework
{
    /// <summary>Provides methods for reading and writing the config file.</summary>
    internal class ConfigUtilities
    {
        /*********
        ** Properties
        *********/
        /// <summary>The full path to the mod folder.</summary>
        private readonly string ModPath;


        /*********
        ** Accessors
        *********/
        /// <summary>The key which saves the game.</summary>
        public string KeyBinding { get; private set; } = "K";


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="modPath">The full path to the mod folder.</param>
        public ConfigUtilities(string modPath)
        {
            this.ModPath = modPath;
        }

        /// <summary>Load the configuration settings.</summary>
        public void LoadConfig()
        {
            string path = Path.Combine(this.ModPath, "Save_Anywhere_Config.txt");
            if (!File.Exists(path))
                this.KeyBinding = "K";
            else
            {
                string[] text = File.ReadAllLines(path);
                this.KeyBinding = Convert.ToString(text[3]);
            }
        }

        /// <summary>Save the configuration settings.</summary>
        public void WriteConfig()
        {
            string path = Path.Combine(this.ModPath, "Save_Anywhere_Config.txt");

            string[] text = new string[20];
            text[0] = "Config: Save_Anywhere Info. Feel free to mess with these settings.";
            text[1] = "====================================================================================";

            text[2] = "Key binding for saving anywhere. Press this key to save anywhere!";
            text[3] = this.KeyBinding;

            File.WriteAllLines(path, text);
        }
    }
}
