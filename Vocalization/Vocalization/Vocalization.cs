using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using Vocalization.Framework;

namespace Vocalization
{
    /// <summary>
    /// TODO:
    /// 
    /// Make a directory where all of the wav files will be stored. (Done?)
    /// Load in said wav files.(Done?)
    /// 
    /// Find way to add in supported dialogue via some sort of file system. (Done?)
    ///     -Make each character folder have a .json that has....
    ///         -Character Name(Done?)
    ///         -Dictionary of supported dialogue lines and values for .wav files. (Done?)
    ///         -*Note* The value for the dialogue dictionaries is the name of the file excluding the .wav extension.
    /// 
    /// Find way to play said wave files. (Done?)
    /// 
    /// Sanitize input to remove variables such as pet names, farm names, farmer name. (done?)
    /// 
    /// !!!!!!!Loop through common variables and add them to the dialogue list inside of ReplacementString.cs
    /// 
    /// !!!!!!!Add in dialogue for npcs into their respective VoiceCue.json files.
    /// 
    /// !!!!!!!Add support for different kinds of menus. TV, shops, etc.
    /// 
    /// 
    /// !!!!!!!!!Make moddable to support other languages, portuguese, russian, etc (Needs testing)
    /// </summary>
    public class Vocalization : Mod
    {
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;

        /// <summary>
        /// A string that keeps track of the previous dialogue said to ensure that dialogue isn't constantly repeated while the text box is open.
        /// </summary>
        public static string previousDialogue;

        /// <summary>
        /// Simple Sound Manager class that handles playing and stoping dialogue.
        /// </summary>
        public static SimpleSoundManager.Framework.SoundManager soundManager;

        /// <summary>
        /// The path to the folder where all of the NPC folders for dialogue .wav files are kept.
        /// </summary>
        public static string VoicePath = "";

        public static ReplacementStrings replacementStrings;

        
        /// <summary>
        /// A dictionary that keeps track of all of the npcs whom have voice acting for their dialogue.
        /// </summary>
        public static Dictionary<string, CharacterVoiceCue> DialogueCues;

        public override void Entry(IModHelper helper)
        {
            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_AfterLoad;
            DialogueCues = new Dictionary<string, CharacterVoiceCue>();
            replacementStrings = new ReplacementStrings();

            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
            StardewModdingAPI.Events.MenuEvents.MenuClosed += MenuEvents_MenuClosed;


            previousDialogue = "";

            ModMonitor = Monitor;
            ModHelper = Helper;

            soundManager = new SimpleSoundManager.Framework.SoundManager();
           
        }

        /// <summary>
        /// Runs whenever any onscreen menu is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuEvents_MenuClosed(object sender, StardewModdingAPI.Events.EventArgsClickableMenuClosed e)
        {
            //Clean out my previous dialogue when I close any sort of menu.
            previousDialogue = "";
        }

        /// <summary>
        /// Runs after the game is loaded to initialize all of the mod's files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            initialzeDirectories();
            loadAllVoiceFiles();
        }

        /// <summary>
        /// Runs every game tick to check if the player is talking to an npc.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.currentSpeaker != null)
            {
                string speakerName = Game1.currentSpeaker.Name;
                if (Game1.activeClickableMenu.GetType() == typeof(StardewValley.Menus.DialogueBox))
                {
                    StardewValley.Menus.DialogueBox dialogueBox =(DialogueBox)Game1.activeClickableMenu;
                    string currentDialogue = dialogueBox.getCurrentString();
                    if (previousDialogue != currentDialogue)
                    {
                        ModMonitor.Log(speakerName);
                        previousDialogue = currentDialogue; //Update my previously read dialogue so that I only read the new string once when it appears.
                        ModMonitor.Log(currentDialogue); //Print out my dialogue.

                        //Do logic here to figure out what audio clips to play.
                        //Sanitize input here!
                        //Load all game dialogue files and then sanitize input.

                        CharacterVoiceCue voice;
                        DialogueCues.TryGetValue(speakerName,out voice);
                        currentDialogue=sanitizeDialogueInGame(currentDialogue); //If contains the stuff in the else statement, change things up.
                        if (voice.dialogueCues.ContainsKey(currentDialogue))
                        {
                            //Not variable messages. Aka messages that don't contain words the user can change such as farm name, farmer name etc. 
                            voice.speak(currentDialogue);
                        }
                        else
                        {
                            ModMonitor.Log("New unregistered dialogue detected for NPC: "+speakerName+" saying: "+currentDialogue,LogLevel.Alert);
                            ModMonitor.Log("Make sure to add this to their respective VoiceCue.json file if you wish for this dialogue to have voice acting associated with it!",LogLevel.Alert);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Runs after loading and creates necessary mod directories.
        /// </summary>
        private void initialzeDirectories()
        {
            string basePath = ModHelper.DirectoryPath;
            string contentPath = Path.Combine(basePath, "Content");
            string audioPath = Path.Combine(contentPath, "Audio");
            string voicePath = Path.Combine(audioPath, "VoiceFiles");


            List<string> translationFolders = new List<string>();

            //TODO: Add more translations.
            translationFolders.Add(Path.Combine(voicePath, "English"));

            VoicePath = voicePath; //Set a static reference to my voice files directory.

            List<string> characterDialoguePaths = new List<string>();

            //Get a list of all characters in the game and make voice directories for them in each supported translation of the mod.
            foreach (var loc in Game1.locations)
            {
                foreach(var NPC in loc.characters)
                {
                    foreach (var translation in translationFolders)
                    {
                        string characterPath = Path.Combine(translation, NPC.Name);
                        characterDialoguePaths.Add(characterPath);
                    }
                }
            }

            if (!Directory.Exists(contentPath)) Directory.CreateDirectory(contentPath);
            if (!Directory.Exists(audioPath)) Directory.CreateDirectory(audioPath);
            if (!Directory.Exists(voicePath)) Directory.CreateDirectory(voicePath);


            //Create all of the necessary folders for different translations.
            foreach(var dir in translationFolders)
            {
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            }

            //Create a list of new directories if the corresponding character directory doesn't exist.
            //Note: A modder could also manually add in their own character directory for voice lines instead of having to add it via code.
            foreach (var dir in characterDialoguePaths)
            {
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            }
        }


        /// <summary>
        /// Loads in all of the .wav files associated with voice acting clips.
        /// </summary>
        public void loadAllVoiceFiles()
        {
            //get a list of all translations supported by this mod.
            List<string> translations = Directory.GetDirectories(VoicePath).ToList();
            foreach (var translation in translations)
            {
                List<string> characterVoiceLines = Directory.GetDirectories(translation).ToList();
                //get a list of all characters supported in this translation and load their voice cue file.
                foreach (var dir in characterVoiceLines)
                {

                    List<string> audioClips = Directory.GetFiles(dir, ".wav").ToList();
                    //For every .wav file in every character voice clip directory load in the voice clip.
                    foreach (var file in audioClips)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        soundManager.loadWavFile(ModHelper, fileName, file);
                        ModMonitor.Log("Loaded sound file: " + fileName + " from: " + file);
                    }

                    //Get the character dialogue cues (aka when the character should "speak") from the .json file.
                    string voiceCueFile = Path.Combine(dir, "VoiceCues.json");
                    string characterName = Path.GetFileName(dir);

                    //If a file was not found, create one and add it to the list of character voice cues.
                    if (!File.Exists(voiceCueFile))
                    {
                        CharacterVoiceCue cue = new CharacterVoiceCue(characterName);

                        var contentDirectory = Game1.content.RootDirectory;

                        var DialogueDict=ModHelper.Content.Load<Dictionary<string,string>>(cue.dialogueFileName, ContentSource.GameContent);

                        foreach(KeyValuePair<string,string>pair in DialogueDict)
                        {
                            string dialogue = pair.Value;
                            dialogue = sanitizeDialogueFromDictionaries(dialogue);
                        }

                        //Loop through all variations of...
                        //time %time
                        //adjectives $adj
                        //nouns %noun
                        //location %place
                        //spouse %spouse

                        //If found in a string of dialogue in a character file.
                        /*
                         *DialogueDict=load dict
                         * foreach(KeyValuePair<string,string> pair in ){
                         * dialogue=
                         * 
                         * }
                         */
                        ModHelper.WriteJsonFile<CharacterVoiceCue>(Path.Combine(dir, "VoiceCues.json"), cue);
                        DialogueCues.Add(characterName, cue);
                    }

                    else
                    {
                        CharacterVoiceCue cue = ModHelper.ReadJsonFile<CharacterVoiceCue>(voiceCueFile);
                        DialogueCues.Add(characterName, cue);
                    }
                }
            }
        }


        /// <summary>
        /// Removes a lot of variables that would be hard to voice act from dkialogue strings such as player's name, pet names, farm names, etc.
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        public string sanitizeDialogueInGame(string dialogue)
        {
            if (dialogue.Contains(Game1.player.Name))
            {
                dialogue = dialogue.Replace(Game1.player.name, ); //Remove player's name from dialogue.
            }

            if (Game1.player.hasPet())
            {
                if (dialogue.Contains(Game1.player.getPetName()))
                {
                    dialogue=dialogue.Replace(Game1.player.getPetName(), );
                }
            }

            if (dialogue.Contains(Game1.player.farmName.Value))
            {
                dialogue=dialogue.Replace(Game1.player.farmName.Value, );
            }

            if (dialogue.Contains(Game1.player.favoriteThing.Value))
            {
                dialogue=dialogue.Replace(Game1.player.favoriteThing.Value, );
            }

            if (dialogue.Contains(Game1.samBandName))
            {
                dialogue=dialogue.Replace(Game1.samBandName, );
            }

            if (dialogue.Contains(Game1.elliottBookName))
            {
                dialogue=dialogue.Replace(Game1.elliottBookName, );
            }

            return dialogue;
        }

        public string sanitizeDialogueFromDictionaries(string dialogue)
        {

            if (dialogue.Contains("@"))
            {
                //replace with farmer name.
                dialogue=dialogue.Replace("@",);
            }

            if (dialogue.Contains("%adj"))
            {
                //??? Loop through all possible adj combinations.
            }

            if (dialogue.Contains("%noun"))
            {
                //??? Loop through all possible noun combinations.
            }

            if (dialogue.Contains("%place"))
            {
                //??? Loop through all place combinations.
            }

            if (dialogue.Contains("%spouse"))
            {
                //Replace with all possible marriageable npcs
            }

            if (dialogue.Contains("%time"))
            {
                //Replace with all times of day. 600-2600.
                for(int i = 600; i <= 2600; i += 10)
                {
                    string time = i.ToString();
                    dialogue = dialogue.Replace("%time", time);
                }
            }

            if (dialogue.Contains("%band"))
            {
                //Replace with<Sam's Band Name>
                dialogue = dialogue.Replace("%band", );
            }

            if (dialogue.Contains("%book"))
            {
                //Replace with<Elliott's Book Name>
                dialogue = dialogue.Replace("%book",);
            }

            if (dialogue.Contains("%rival"))
            {
                //Replace with<Rival Name>
                dialogue = dialogue.Replace("%rival",);
            }

            if (dialogue.Contains("%pet"))
            {
                //Replace with <Pet Name>
                dialogue = dialogue.Replace("%pet",);
            }

            if (dialogue.Contains("%farm"))
            {
                //Replace with <Farm Name>
            }

            if (dialogue.Contains("%favorite"))
            {
                //Replace with <Favorite thing>
            }

            if (dialogue.Contains("%kid1"))
            {
                //Replace with <Kid 1's Name>
            }

            if (dialogue.Contains("%kid2"))
            {
                //Replace with <Kid 2's Name>
            }
            return dialogue;
        }
    }
}
