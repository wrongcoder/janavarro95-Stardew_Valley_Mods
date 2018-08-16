using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocalization
{
    /// <summary>
    /// The configuration file for the mod.
    /// </summary>
    public class ModConfig
    {
        /// <summary>
        /// A list of all of the translations currently supported by this mod.
        /// </summary>
        public List<string> translations;
        /// <summary>
        /// The currently selected translation to use.
        /// </summary>
        public string currentTranslation;

        /// <summary>
        /// Keeps track of the voice modes for determining how much audio is played.
        /// </summary>
        public List<string> modes;

        /// <summary>
        /// The curent mode for the mod.
        /// </summary>
        public string currentMode;

        /// <summary>
        /// The volume at which the sound for voices is played at.
        /// </summary>
        public float voiceVolume;

        public ModConfig()
        {
            translations = new List<string>();
            modes = new List<string>();

            modes.Add("Simple");
            modes.Add("Full");
            modes.Add("HeartEvents");
            modes.Add("SimpleAndHeartEvents");
            currentMode = "Full";


            translations.Add("English");
            currentTranslation = "English";

            this.voiceVolume = 1.0f;
        }

        /// <summary>
        /// Validates
        /// </summary>
        public void verifyValidMode()
        {
            if (!modes.Contains(currentMode))
            {
                Vocalization.ModMonitor.Log("Invalid configuration: " + currentMode + ". Changing to Full voiced mode.");
                currentMode = "Full";
            }
        }


    }
}
