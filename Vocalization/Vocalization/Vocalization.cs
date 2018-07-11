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
    /// Sanitize input to remove variables such as pet names, farm names, farmer name. (done)
    /// 
    /// Loop through common variables and add them to the dialogue list inside of ReplacementString.cs (done)
    ///     -ERRR that might not be fun to do.......
    ///     Dialogue.cs
    ///     adj is 679-698 (done)
    ///     noun is 699-721 (done)
    ///     verb is 722-734 ???? Not needed???
    ///     place is 735-759 (done)
    ///     colors is 795-810. What does it change though??????
    /// 
    /// Add in dialogue for npcs into their respective VoiceCue.json files. (done? Can be improved on)
    /// 
    /// 
    ///Add in sanitization for Dialogue Commands(see the wiki) (done)
    /// 
    /// !!!!!!!!!Add in dialogue for npcs into their respective VoiceCue files for events!
    /// 
    /// !!!!!!!Add support for different kinds of menus. TV, shops, etc.
    ///     -All of these strings are stored in StringsFromCS and TV/file.yaml
    /// 
    /// !!!!!!!Add support for MarriageDialogue strings.
    /// 
    /// !!!!!!!Add support for Extra dialogue via StringsFromCSFiles
    ///     -tv
    ///     -events
    ///     -above stuff
    ///     -NPC.cs
    ///     -Utility.csmmn
    /// 
    /// !!!!!!!!!Make moddable to support other languages, portuguese, russian, etc (Needs testing)
    ///     -make mod config have a list of supported languages and a variable that is the currently selected language.
    ///     
    /// Add support for adding dialogue lines when loading CharacterVoiceCue.json if the line doesn't already exist! (done)
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
            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
            StardewModdingAPI.Events.MenuEvents.MenuClosed += MenuEvents_MenuClosed;

            ModMonitor = Monitor;
            ModHelper = Helper;
            DialogueCues = new Dictionary<string, CharacterVoiceCue>();
            replacementStrings = new ReplacementStrings();

            previousDialogue = "";

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

                        var dialoguePath = Path.Combine("Characters", "Dialogue");

                        //basically this will never run but can be used below to also add in dialogue.
                        if (!String.IsNullOrEmpty(cue.dialogueFileName))
                        {
                            dialoguePath = Path.Combine(dialoguePath, cue.dialogueFileName);
                            var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath, ContentSource.GameContent);

                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                string rawDialogue = pair.Value;
                                List<string> cleanDialogues = new List<string>();
                                cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                foreach (var str in cleanDialogues)
                                {
                                    cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                }
                            }
                        }

                        ModHelper.WriteJsonFile<CharacterVoiceCue>(Path.Combine(dir, "VoiceCues.json"), cue);
                        DialogueCues.Add(characterName, cue);
                    }

                    else
                    {
                        CharacterVoiceCue cue = ModHelper.ReadJsonFile<CharacterVoiceCue>(voiceCueFile);
                        var dialoguePath = Path.Combine("Characters", "Dialogue");
                        //Add in all dialogue.
                        if (!String.IsNullOrEmpty(cue.dialogueFileName))
                        {
                            dialoguePath = Path.Combine(dialoguePath, cue.dialogueFileName);
                            var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath, ContentSource.GameContent);

                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                string rawDialogue = pair.Value;
                                List<string> cleanDialogues = new List<string>();
                                cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                foreach (var str in cleanDialogues)
                                {
                                    cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                }
                            }
                        }
                        ModHelper.WriteJsonFile<CharacterVoiceCue>(Path.Combine(dir, "VoiceCues.json"), cue);
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
                dialogue = dialogue.Replace(Game1.player.name, replacementStrings.farmerName); //Remove player's name from dialogue.
            }

            if (Game1.player.hasPet())
            {
                if (dialogue.Contains(Game1.player.getPetName()))
                {
                    dialogue=dialogue.Replace(Game1.player.getPetName(), replacementStrings.petName);
                }
            }

            if (dialogue.Contains(Game1.player.farmName.Value))
            {
                dialogue=dialogue.Replace(Game1.player.farmName.Value, replacementStrings.farmName);
            }

            if (dialogue.Contains(Game1.player.favoriteThing.Value))
            {
                dialogue=dialogue.Replace(Game1.player.favoriteThing.Value, replacementStrings.favoriteThing);
            }

            if (dialogue.Contains(Game1.samBandName))
            {
                dialogue=dialogue.Replace(Game1.samBandName, replacementStrings.bandName);
            }

            if (dialogue.Contains(Game1.elliottBookName))
            {
                dialogue=dialogue.Replace(Game1.elliottBookName, replacementStrings.bookName);
            }

            //Sanitize children names from the dialogue.
            if (Game1.player.getChildren().Count > 0)
            {
                int count = 1;
                foreach (var child in Game1.player.getChildren())
                {
                    if (dialogue.Contains(child.Name))
                    {
                        if (count == 1)
                        {
                            dialogue = dialogue.Replace(child.Name, replacementStrings.kid1Name);
                        }
                        if (count == 2)
                        {
                            dialogue = dialogue.Replace(child.Name, replacementStrings.kid2Name);
                        }
                    }
                    count++;
                }
            }

            return dialogue;
        }

        /// <summary>
        /// Load in all dialogue.xnb files and attempt to sanitize all of the dialogue from it to help making adding dialogue easier.
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        public List<string> sanitizeDialogueFromDictionaries(string dialogue)
        {
            List<string> possibleDialogues = new List<string>();

            //remove $ symbols and their corresponding letters.

            if (dialogue.Contains("$neutral"))
            {
                dialogue = dialogue.Replace("$neutral", "");
                dialogue = dialogue.Replace("  ", " "); //Remove awkward spacing.
            }

            if (dialogue.Contains("$h"))
            {
                dialogue = dialogue.Replace("$h", "");
                dialogue = dialogue.Replace("  ", " "); //Remove awkward spacing.
            }

            if (dialogue.Contains("$s"))
            {
                dialogue = dialogue.Replace("$s", "");
                dialogue = dialogue.Replace("  ", " "); //Remove awkward spacing.
            }

            if (dialogue.Contains("$u"))
            {
                dialogue = dialogue.Replace("$u", "");
                dialogue = dialogue.Replace("  ", " "); //Remove awkward spacing.
            }

            if (dialogue.Contains("$l"))
            {
                dialogue = dialogue.Replace("$l", "");
                dialogue = dialogue.Replace("  ", " "); //Remove awkward spacing.
            }

            if (dialogue.Contains("$a"))
            {
                dialogue = dialogue.Replace("$a", "");
                dialogue = dialogue.Replace("  ", " "); //Remove awkward spacing.
            }

            if (dialogue.Contains("$q"))
            {
                dialogue = dialogue.Replace("$q", "");
                dialogue = dialogue.Replace("  ", " "); //Remove awkward spacing.
            }


            //This is probably the worst possible way to do this but I don't have too much a choice.
            for (int i=0; i<=100; i++)
            {
                string combine = "";
                if (i == 1) continue;
                combine = "$" + i.ToString();
                if (dialogue.Contains(combine))
                {
                    dialogue = dialogue.Replace(combine, "");
                    dialogue = dialogue.Replace("  ", " "); //Remove awkward spacing.
                    //remove dialogue symbol.
                }
            }

            //split across % symbol
            //Just remove the %symbol for generic text boxes. Not for forks.
            if (dialogue.Contains("%") && dialogue.Contains("%fork") == false)
            {
                dialogue = dialogue.Replace("%", "");
            }

            if (dialogue.Contains("$fork"))
            {
                dialogue = dialogue.Replace("%fork", "");
            }

            //split across # symbol
            List<string> dialogueSplits1 = dialogue.Split('#').ToList(); //Returns an element size of 1 if # isn't found.

            //Split across choices
            List<string> orSplit = new List<string>();

            //Split across genders
            List<string> finalSplit = new List<string>();

            //split across | symbol
            foreach(var dia in dialogueSplits1)
            {
                if (dia.Contains("|")) //If I can split my string do so and add all the split strings into my orSplit list.
                {
                    List<string> tempSplits = dia.Split('|').ToList();
                    foreach(var v in tempSplits)
                    {
                        orSplit.Add(v);
                    }
                }
                else
                {
                    orSplit.Add(dia); //If I can't split the list just add the dialogue and keep processing.
                }
            }

            //split across ^ symbol   
            foreach (var dia in orSplit)
            {
                if (dia.Contains("^")) //If I can split my string do so and add all the split strings into my orSplit list.
                {
                    List<string> tempSplits = dia.Split('^').ToList();
                    foreach (var v in tempSplits)
                    {
                        finalSplit.Add(v);
                    }
                }
                else
                {
                    finalSplit.Add(dia); //If I can't split the list just add the dialogue and keep processing.
                }
            }


            //Loop through all adjectives and add them to our list of possibilities.
            for (int i = 0; i < finalSplit.Count(); i++)
            {
                string dia = finalSplit.ElementAt(i);
                if (dia.Contains("%adj"))
                {
                    foreach (var adj in replacementStrings.adjStrings)
                    {
                        dia = dia.Replace("%adj", adj);
                        finalSplit.Add(dia);
                    }
                }
            }

            //Loop through all nouns and add them to our list of possibilities.
            for (int i = 0; i < finalSplit.Count(); i++)
            {
                string dia = finalSplit.ElementAt(i);
                if (dia.Contains("%noun"))
                {
                    foreach (var noun in replacementStrings.nounStrings)
                    {
                        dia = dia.Replace("%noun", noun);
                        finalSplit.Add(dia);
                    }
                }
            }

            //Loop through all places and add them to our list of possibilities.
            for (int i = 0; i < finalSplit.Count(); i++)
            {
                string dia = finalSplit.ElementAt(i);
                if (dia.Contains("%place"))
                {
                    foreach (var place in replacementStrings.placeStrings)
                    {
                        dia = dia.Replace("%place", place);
                        finalSplit.Add(dia);
                    }
                }
            }

            //Loop through all spouses and add them to our list of possibilities.
            for (int i = 0; i < finalSplit.Count(); i++)
            {
                string dia = finalSplit.ElementAt(i);
                if (dia.Contains("%spouse"))
                {
                    foreach (var spouse in replacementStrings.spouseNames)
                    {
                        dia = dia.Replace("%spouse", spouse);
                        finalSplit.Add(dia);
                    }
                }
            }


            //iterate across ll dialogues and return a list of them.
            for (int i= 0;i<finalSplit.Count(); i++){
                string dia = finalSplit.ElementAt(i);

                if (dia.Contains("@"))
                {
                    //replace with farmer name.
                    dia = dia.Replace("@", replacementStrings.farmerName);
                }

                if (dia.Contains("%band"))
                {
                    //Replace with<Sam's Band Name>
                    dia = dia.Replace("%band", replacementStrings.bandName);
                }

                if (dia.Contains("%book"))
                {
                    //Replace with<Elliott's Book Name>
                    dia = dia.Replace("%book", replacementStrings.bookName);
                }

                if (dia.Contains("%rival"))
                {
                    //Replace with<Rival Name>
                    dia = dia.Replace("%rival", replacementStrings.rivalName);
                }

                if (dia.Contains("%pet"))
                {
                    //Replace with <Pet Name>
                    dia = dia.Replace("%pet", replacementStrings.petName);
                }

                if (dia.Contains("%farm"))
                {
                    dia = dia.Replace("%pet", replacementStrings.farmName);
                }

                if (dia.Contains("%favorite"))
                {
                    //Replace with <Favorite thing>
                    dia = dia.Replace("%pet", replacementStrings.favoriteThing);
                }

                if (dia.Contains("%kid1"))
                {
                    //Replace with <Kid 1's Name>
                    dia = dia.Replace("%pet", replacementStrings.kid1Name);
                }

                if (dia.Contains("%kid2"))
                {
                    //Replace with <Kid 2's Name>
                    dia = dia.Replace("%pet", replacementStrings.kid2Name);
                }

                if (dia.Contains("%time"))
                {
                    //Replace with all times of day. 600-2600.
                    for (int t = 600; t <= 2600; t += 10)
                    {
                        string time = t.ToString();
                        string diaTime = dia.Replace("%time", time);
                        possibleDialogues.Add(diaTime);
                    }
                }
                else
                {
                    possibleDialogues.Add(dia);
                }
                
            }

            List<string> removalList = new List<string>();
            //Clean out all dialogue commands.
            foreach(var dia in possibleDialogues)
            {
                if (dia.Contains("$r"))
                {
                    removalList.Add(dia);
                }

                if (dia.Contains("$p"))
                {
                    removalList.Add(dia);
                }

                if (dia.Contains("$b"))
                {
                    removalList.Add(dia);
                }

                if (dia.Contains("$e"))
                {
                    removalList.Add(dia);
                }

                if (dia.Contains("$d"))
                {
                    removalList.Add(dia);
                }

                if (dia.Contains("$k"))
                {
                    removalList.Add(dia);
                }
            }

            //Delete all garbage dialogues left over.
            foreach(var v in removalList)
            {
                possibleDialogues.Remove(v);
            }


            return possibleDialogues;
        }
    }
}
