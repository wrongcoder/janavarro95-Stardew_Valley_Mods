using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocalization.Framework
{
    /// <summary>
    /// A class that handles all of the storage of references to the audio files for this character.
    /// </summary>
    public class CharacterVoiceCue
    {
        /// <summary>
        /// The name of the NPC.
        /// </summary>
        public string name;

        /// <summary>
        /// The name of the dialogue file to scrape for inputting values into the dictionary of dialogueCues.
        /// </summary>
        public string dialogueFileName;

        /// <summary>
        /// A dictionary of dialogue strings that correspond to audio files.
        /// </summary>
        public Dictionary<string, string> dialogueCues;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the NPC.</param>
        public CharacterVoiceCue(string name)
        {
            this.name = name;
            this.dialogueCues = new Dictionary<string, string>();
        }

        /// <summary>
        /// Plays the associated dialogue file.
        /// </summary>
        /// <param name="dialogueString">The current dialogue string to play audio for.</param>
        public void speak(string dialogueString)
        {
            string voiceFileName = "";
            bool exists = dialogueCues.TryGetValue(dialogueString, out voiceFileName);
            if (exists)
            {
                Vocalization.soundManager.swapSounds(voiceFileName);
            }
            else
            {
                Vocalization.ModMonitor.Log("The dialogue cue for the current dialogue could not be found. Please ensure that the dialogue is added the character's voice file and that the proper file for the voice exists.");
                return;
            }
        }

    }
}
