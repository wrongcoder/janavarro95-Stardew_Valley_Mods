using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using Vocalization.Framework;

namespace Vocalization
{
    /*
     * Things to sanitize/load in
     * 
     * NPC Dialogue(sanitized, not loaded);
     *  -Characters/Dialogue/CharacterName
     * Rainy Dialogue(sanitized, not loaded);
     *  -Characters/Dialogue/rainy.yaml
     * Marriage dialogue(sanitized?,not loaded);
     *  -Characters/Dialogue/MarriageDialogue<NPC NAME>
     *  -Characters/Dialogue/MarriageDialogue.yaml
     * Engagement dialogue(sanitized, not loaded);
     *  -Data/EngagementDialogue.yaml
     * Misc
     *  -Strings/StringsFromCSFiles.yaml
     * 
     * TV shows
     *  -cooking (sanitized, not loaded)
     *      -Data/TV/CookingChannel.yaml
     *  -interview(sanitized, not loaded)
     *      -Data/TV/InterviewShow.yaml
     *  -tip(sanitized, not loaded)
     *      -Data/TV/TipChannel.yaml
     *  -oracle(sanitized, not loaded)
     *      -Strings/StringsFromCSFiles.yaml
     *  -weather(sanitized, not loaded)
     *      -Strings/StringsFromCSFiles.yaml
     * 
     * 
     * Shops(sanitized, not loaded);
     *  -Strings/StringsFromCSFiles.yaml
     *  
     * Extra dialogue(sanitized, not loaded);
     *  -Data/ExtraDialogue.yaml
     * 
     * Letters(sanitized, not loaded);
     *  -Data/mail.yaml
     * 
     * Events(sanitized, not loaded); 
     *  -Strings/StringsFromCSFiles.yaml
     *  -Strings/Events.yaml
     * 
     * Characters:
     *  -Strings/Characters.yaml (sanitized, not loaded);
     *  
     * Strings/Events.yaml (sanitized, not loaded);
     *  -Strings/StringsFromCSFiles.yaml
     *  
     * Strings/Locations.yaml(sanitized, not loaded);
     *  -Strings/Loctions.yaml
     *  -Strings/StringsFromMaps.yaml
     * 
     * Strings/Notes.yaml(sanitized, not loaded);
     *  -Strings/Notes.yaml
     *  -Data/SecretNotes.yaml
     * 
     * Strings/Objects.yaml (not needed);
     * 
     * Utility
     *  -Strings/StringsFromCS.yaml
     *  
     *  Quests (done)
     *  -Strings/Quests
     */

    /// <summary>
    /// TODO:
    /// 
    /// Validate that all paths are loading from proper places.
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
    /// 
    ///Add support for different kinds of menus. TV, shops, etc. (Done)
    ///     -All of these strings are stored in StringsFromCS and TV/file.yaml
    /// 
    ///Add support for MarriageDialogue strings. (Done)
    ///Add support for EngagementDialogue strings.(Done)
    ///Add support for ExtraDialogue.yaml file (Done)
    /// 
    /// 
    ///Add support for mail dialogue(Done)
    ///         -split using ^ to get the sender's name as the last element in the split list. Then sanitize the % information out by splitting across % and getting the first element.
    /// 
    /// 
    ///Add support for Extra dialogue via StringsFromCSFiles(Done)
    ///     -tv
    ///     -events
    ///     -NPC.cs
    ///     -Utility.cs
    /// 
    /// Make moddable to support other languages, portuguese, russian, etc (Needs testing)
    ///     -make mod config have a list of supported languages and a variable that is the currently selected language.
    ///     
    /// Remove text typing sound from game? (done) Just turn off the option for Game1.options.dialogueTyping.
    ///     
    /// Add support for adding dialogue lines when loading CharacterVoiceCue.json if the line doesn't already exist! (done)
    /// 
    /// 
    /// 
    /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! All in Strings folder
    /// -Quests (done)
    /// -NPC Gift tastes (done)
    /// speech bubbles (done)
    /// -temp
    /// -ui
    /// /// 
    /// 
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

        public static ModConfig config;

        
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

            config = new ModConfig();
            ModHelper.ReadConfig<ModConfig>();
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
            if (String.IsNullOrEmpty(soundManager.previousSound.Key) || soundManager.previousSound.Value == null) return;
            soundManager.stopPreviousSound();
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

        public static object GetInstanceField(Type type, object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            /*
            FieldInfo[] meh = type.GetFields(bindFlags);
            foreach(var v in meh)
            {
                if (v.Name == null)
                {
                    continue;
                }
                Monitor.Log(v.Name);
            }
            */
            return field.GetValue(instance);
        }
        /// <summary>
        /// Runs every game tick to check if the player is talking to an npc.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.player != null) {
                if (Game1.player.currentLocation != null) {
                    foreach (NPC v in Game1.currentLocation.characters)
                    {
                        string text = (string)GetInstanceField(typeof(NPC), v, "textAboveHead");
                        if (text == null) continue;
                        string currentDialogue = text;
                        if (previousDialogue != currentDialogue)
                        {
                            List<string> tries = new List<string>();
                            tries.Add("SpeechBubbles");
                            foreach (var speech in tries)
                            {
                                CharacterVoiceCue voice;
                                DialogueCues.TryGetValue(speech, out voice);
                                currentDialogue = sanitizeDialogueInGame(currentDialogue); //If contains the stuff in the else statement, change things up.
                                if (voice.dialogueCues.ContainsKey(currentDialogue))
                                {
                                    //Not variable messages. Aka messages that don't contain words the user can change such as farm name, farmer name etc. 
                                    voice.speak(currentDialogue);
                                }
                                else
                                {
                                    ModMonitor.Log("New unregistered dialogue detected for NPC: " + speech + " saying: " + currentDialogue, LogLevel.Alert);
                                    ModMonitor.Log("Make sure to add this to their respective VoiceCue.json file if you wish for this dialogue to have voice acting associated with it!", LogLevel.Alert);

                                }
                            }
                        }
                    }
                }
            }

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

                        List<string> tries = new List<string>();
                        tries.Add(speakerName);
                        tries.Add("Events");
                        tries.Add("CharactersStrings");
                        tries.Add("LocationDialogue");
                        tries.Add("Utility");
                        tries.Add("Quests");
                        tries.Add("NPCGiftTastes");
                        foreach (var v in tries)
                        {
                            CharacterVoiceCue voice;
                            DialogueCues.TryGetValue(v, out voice);
                            currentDialogue = sanitizeDialogueInGame(currentDialogue); //If contains the stuff in the else statement, change things up.
                            if (voice.dialogueCues.ContainsKey(currentDialogue))
                            {
                                //Not variable messages. Aka messages that don't contain words the user can change such as farm name, farmer name etc. 
                                voice.speak(currentDialogue);
                            }
                            else
                            {
                                ModMonitor.Log("New unregistered dialogue detected for NPC: " + v + " saying: " + currentDialogue, LogLevel.Alert);
                                ModMonitor.Log("Make sure to add this to their respective VoiceCue.json file if you wish for this dialogue to have voice acting associated with it!", LogLevel.Alert);
                               
                            }
                        }
                    }
                }
            }
            else
            {
                if (Game1.activeClickableMenu == null) return;
                    //Support for TV
                    if (Game1.activeClickableMenu.GetType() == typeof(StardewValley.Menus.DialogueBox))
                    {
                        StardewValley.Menus.DialogueBox dialogueBox = (DialogueBox)Game1.activeClickableMenu;
                        string currentDialogue = dialogueBox.getCurrentString();
                        if (previousDialogue != currentDialogue)
                        {
                            previousDialogue = currentDialogue; //Update my previously read dialogue so that I only read the new string once when it appears.
                            ModMonitor.Log(currentDialogue); //Print out my dialogue.

                        List<string> tries = new List<string>();
                        tries.Add("TV");
                        tries.Add("Events");
                        tries.Add("Characters");
                        tries.Add("LocationDialogue");
                        tries.Add("Notes");
                        tries.Add("Utility");
                        tries.Add("Quests");
                        tries.Add("NPCGiftTastes");
                        foreach (var v in tries)
                        {
                            //Add in support for TV Shows
                            CharacterVoiceCue voice;
                            bool f=DialogueCues.TryGetValue(v, out voice);
                            currentDialogue = sanitizeDialogueInGame(currentDialogue); //If contains the stuff in the else statement, change things up.
                            if (voice.dialogueCues.ContainsKey(currentDialogue))
                            {
                                //Not variable messages. Aka messages that don't contain words the user can change such as farm name, farmer name etc. 
                                voice.speak(currentDialogue);
                            }
                            else
                            {
                                ModMonitor.Log("New unregistered dialogue detected saying: " + currentDialogue, LogLevel.Alert);
                                ModMonitor.Log("Make sure to add this to their respective VoiceCue.json file if you wish for this dialogue to have voice acting associated with it!", LogLevel.Alert);
                               
                            }
                        }
                        }
                    }

                //Support for Letters
                if (Game1.activeClickableMenu.GetType() == typeof(StardewValley.Menus.LetterViewerMenu))
                {
                    //Use reflection to get original text back.
                    var menu = (StardewValley.Menus.LetterViewerMenu)Game1.activeClickableMenu;
                    //mail dialogue text will probably need to be sanitized as well....
                    List<string> mailText=(List<string>)ModHelper.Reflection.GetField<List<string>>(menu, "mailMessage", true);
                    string currentDialogue = "";
                    foreach(var v in mailText)
                    {
                        currentDialogue += mailText;
                    }

                    previousDialogue = currentDialogue; //Update my previously read dialogue so that I only read the new string once when it appears.
                    ModMonitor.Log(currentDialogue); //Print out my dialogue.


                    //Add in support for TV Shows
                    CharacterVoiceCue voice;
                    DialogueCues.TryGetValue("Mail", out voice);
                    currentDialogue = sanitizeDialogueInGame(currentDialogue); //If contains the stuff in the else statement, change things up.
                    if (voice.dialogueCues.ContainsKey(currentDialogue))
                    {
                        //Not variable messages. Aka messages that don't contain words the user can change such as farm name, farmer name etc. 
                        voice.speak(currentDialogue);
                    }
                    else
                    {
                        ModMonitor.Log("New unregistered Mail dialogue detected saying: " + currentDialogue, LogLevel.Alert);
                        ModMonitor.Log("Make sure to add this to their respective VoiceCue.json file if you wish for this dialogue to have voice acting associated with it!", LogLevel.Alert);
                    }
                
                }

                //Support for shops
                if (Game1.activeClickableMenu.GetType() == typeof(StardewValley.Menus.ShopMenu))
                {

                    var menu = (StardewValley.Menus.ShopMenu)Game1.activeClickableMenu;
                    string currentDialogue = menu.potraitPersonDialogue; //Check this string to the dict of voice cues
                    NPC npc = menu.portraitPerson;
                    
                    previousDialogue = currentDialogue; //Update my previously read dialogue so that I only read the new string once when it appears.
                    ModMonitor.Log(currentDialogue); //Print out my dialogue.


                    //Add in support for Shops
                    CharacterVoiceCue voice=new CharacterVoiceCue("");
                    try
                    {
                        //character shops
                        bool f=DialogueCues.TryGetValue(Path.Combine("Shops"), out voice);
                        if (f == false)
                        {
                            ModMonitor.Log("Can't find the dialogue for the shop: " + npc.Name);
                        }
                    }
                    catch(Exception err) { 

                    }
                    currentDialogue = sanitizeDialogueInGame(currentDialogue); //If contains the stuff in the else statement, change things up.
                    if (voice.dialogueCues.ContainsKey(currentDialogue))
                    {
                        //Not variable messages. Aka messages that don't contain words the user can change such as farm name, farmer name etc. 
                        voice.speak(currentDialogue);
                    }
                    else
                    {
                        ModMonitor.Log("New unregistered dialogue detected saying: " + currentDialogue, LogLevel.Alert);
                        ModMonitor.Log("Make sure to add this to their respective VoiceCue.json file if you wish for this dialogue to have voice acting associated with it!", LogLevel.Alert);
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


            VoicePath = voicePath; //Set a static reference to my voice files directory.

            List<string> characterDialoguePaths = new List<string>();

            //Get a list of all characters in the game and make voice directories for them in each supported translation of the mod.
            foreach (var loc in Game1.locations)
            {
                foreach(var NPC in loc.characters)
                {
                    foreach (var translation in config.translations)
                    {
                        string characterPath = Path.Combine(translation, NPC.Name);
                        characterDialoguePaths.Add(characterPath);
                    }
                }
            }

            //Add in folder for TV Shows
            foreach (var translation in config.translations)
            {
                string TVPath = Path.Combine(translation, "TV");
                characterDialoguePaths.Add(TVPath);
            }

            //Add in folder for shop support
            foreach (var translation in config.translations)
            {
                string shop = Path.Combine(translation, "Shops"); //Used to hold NPC Shops
                characterDialoguePaths.Add(shop);
            }

            //Add in folder for Mail support.
            foreach (var translation in config.translations)
            {
                string mail = Path.Combine(translation, "Mail");
                characterDialoguePaths.Add(mail);
            }

            //Add in folder for ExtraDiaogue.yaml
            foreach (var translation in config.translations)
            {
                string extra = Path.Combine(translation, "ExtraDialogue");
                characterDialoguePaths.Add(extra);
            }

            foreach (var translation in config.translations)
            {
                string extra = Path.Combine(translation, "Events");
                characterDialoguePaths.Add(extra);
            }

            foreach (var translation in config.translations)
            {
                string extra = Path.Combine(translation, "Characters");
                characterDialoguePaths.Add(extra);
            }

            foreach (var translation in config.translations)
            {
                string extra = Path.Combine(translation, "LocationDialogue");
                characterDialoguePaths.Add(extra);
            }

            foreach (var translation in config.translations)
            {
                string extra = Path.Combine(translation, "Notes");
                characterDialoguePaths.Add(extra);
            }

            foreach (var translation in config.translations)
            {
                string extra = Path.Combine(translation, "Utility");
                characterDialoguePaths.Add(extra);
            }

            foreach (var translation in config.translations)
            {
                string extra = Path.Combine(translation, "NPCGiftTastes");
                characterDialoguePaths.Add(extra);
            }

            foreach (var translation in config.translations)
            {
                string extra = Path.Combine(translation, "SpeechBubbles");
                characterDialoguePaths.Add(extra);
            }

            foreach (var translation in config.translations)
            {
                string kent = Path.Combine(translation, "Kent");
                characterDialoguePaths.Add(kent);


                string gil = Path.Combine(translation, "Gil");
                characterDialoguePaths.Add(gil);


                string governor = Path.Combine(translation, "Governor");
                characterDialoguePaths.Add(governor);


                string grandpa = Path.Combine(translation, "Grandpa");
                characterDialoguePaths.Add(grandpa);


                string morris = Path.Combine(translation, "Morris");
                characterDialoguePaths.Add(morris);
            }



            foreach (var translation in config.translations)
            {
                string extra = Path.Combine(translation, "Quests");
                characterDialoguePaths.Add(extra);
            }

            if (!Directory.Exists(contentPath)) Directory.CreateDirectory(contentPath);
            if (!Directory.Exists(audioPath)) Directory.CreateDirectory(audioPath);
            if (!Directory.Exists(voicePath)) Directory.CreateDirectory(voicePath);


            //Create all of the necessary folders for different translations.
            foreach(var dir in config.translations)
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
                        cue.initializeEnglishScrape();
                        scrapeDictionaries(voiceCueFile,cue);
                    }
                    else
                    {
                        CharacterVoiceCue cue = ModHelper.ReadJsonFile<CharacterVoiceCue>(voiceCueFile);
                        scrapeDictionaries(voiceCueFile,cue);
                    }
                }
            }
        }

        /// <summary>
        /// Used to obtain all strings for almost all possible dialogue in the game.
        /// </summary>
        /// <param name="cue"></param>
        public void scrapeDictionaries(string path,CharacterVoiceCue cue)
        {

            var dialoguePath = Path.Combine("Characters", "Dialogue");
            var stringsPath = Path.Combine("Strings"); //Used for all sorts of extra strings and stuff for like StringsFromCS
            var dataPath = Path.Combine("Data"); //Used for engagement dialogue strings, and ExtraDialogue, Notes, Secret Notes, Mail

            ModMonitor.Log("Scraping dialogue for character: " + cue.name,LogLevel.Info);
            //If the "character"'s name is TV which means I'm watching tv, scrape the data from the TV shows.
            if (cue.name == "TV")
            {
                
                foreach (var fileName in cue.dataFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName,LogLevel.Info);
                    //basically this will never run but can be used below to also add in dialogue.
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        string dialoguePath2 = Path.Combine(dataPath,"TV",fileName);
                        var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);

                        //Scraping the CookingChannel dialogue
                        if (fileName.Contains("CookingChannel"))
                        {
                            //Scrape the whole dictionary looking for the character's name.
                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                string key = pair.Key;
                                string rawDialogue = pair.Value;
                                List<string> splitDialogues = new List<string>();
                                splitDialogues = rawDialogue.Split('/').ToList();

                                string cookingDialogue = splitDialogues.ElementAt(1);
                                //If the key contains the character's name.
                                    List<string> cleanDialogues = new List<string>();
                                    cleanDialogues = sanitizeDialogueFromDictionaries(cookingDialogue);
                                    foreach (var str in cleanDialogues)
                                    {
                                        cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                    }
                                
                            }
                            continue;
                        }

                        //Interview Show
                        if (fileName.Contains("InterviewShow"))
                        {
                            //Scrape the whole dictionary looking for the character's name.
                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                string key = pair.Key;
                                string rawDialogue = pair.Value;
                                if (key != "intro") continue;
                                //If the key contains the character's name.
                                    List<string> cleanDialogues = new List<string>();
                                    cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                    foreach (var str in cleanDialogues)
                                    {
                                        cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                    }
                                
                            }
                            continue;
                        }

                        //Tip channel
                        if (fileName.Contains("TipChannel"))
                        {
                            //Scrape the whole dictionary looking for the character's name.
                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                string key = pair.Key;
                                string rawDialogue = pair.Value;

                                    List<string> cleanDialogues = new List<string>();
                                    cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                    foreach (var str in cleanDialogues)
                                    {
                                        cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                    }
                                
                            }
                            continue;
                        }
                    }
                }

                foreach (var fileName in cue.stringsFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    //basically this will never run but can be used below to also add in dialogue.
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        string dialoguePath2 = Path.Combine(stringsPath, fileName);
                        var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);
                        if (fileName.Contains("StringsFromCSFiles"))
                        {
                            //Scrape the whole dictionary looking for the character's name.
                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                string key = pair.Key;
                                string rawDialogue = pair.Value;
                                if (!key.Contains("TV")) continue;
                                //If the key contains the character's name.
                                List<string> cleanDialogues = new List<string>();
                                cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                
                                foreach (var str in cleanDialogues)
                                {
                                    string ahh = sanitizeDialogueFromMailDictionary(str);
                                    cue.addDialogue(ahh, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                }

                            }
                            continue;
                        }
                    }
                }


            }

            //If the "character"'s name is Shops which means I'm talking to a shopkeeper.
            else if (cue.name == "Shops")
            {
                foreach (var fileName in cue.stringsFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    //basically this will never run but can be used below to also add in dialogue.
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        string dialoguePath2 = Path.Combine(stringsPath, fileName);
                        var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);

                        //Scraping the CookingChannel dialogue

                        if (fileName.Contains("StringsFromCSFiles"))
                        {
                            //Scrape the whole dictionary looking for the character's name.
                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                string key = pair.Key;
                                string rawDialogue = pair.Value;
                                if (!key.Contains("ShopMenu")) continue;
                                //If the key contains the character's name.
                              
                                    List<string> cleanDialogues = new List<string>();
                                    cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                    foreach (var str in cleanDialogues)
                                    {
                                        cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                    }
                                
                            }
                            continue;
                        }
                        //For moddablity add a generic scrape here!
                    }
                }

            }

            //Scrape Content/Data/ExtraDialogue.yaml
            else if (cue.name == "ExtraDialogue")
            {
                foreach (var fileName in cue.dataFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    //basically this will never run but can be used below to also add in dialogue.
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        string dialoguePath2 = Path.Combine(dataPath, fileName);
                        var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);

                        //Scraping the CookingChannel dialogue
                            //Scrape the whole dictionary looking for the character's name.
                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                string key = pair.Key;
                                string rawDialogue = pair.Value;
                                //If the key contains the character's name.

                                List<string> cleanDialogues = new List<string>();
                                cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                foreach (var str in cleanDialogues)
                                {
                                    cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                }

                            }
                    }
                }

            }

            //Used to scrape Strings/Locations.yaml and Strings/StringsFromMaps.yaml
            else if (cue.name == "LocationDialogue")
            {
                foreach (var fileName in cue.stringsFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    //basically this will never run but can be used below to also add in dialogue.
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        string dialoguePath2 = Path.Combine(stringsPath, fileName);
                        var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);

                        //Scraping the CookingChannel dialogue
                        //Scrape the whole dictionary looking for the character's name.
                        foreach (KeyValuePair<string, string> pair in DialogueDict)
                        {
                            //Get the key in the dictionary
                            string key = pair.Key;
                            string rawDialogue = pair.Value;
                            //If the key contains the character's name.

                            List<string> cleanDialogues = new List<string>();
                            cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                            foreach (var str in cleanDialogues)
                            {
                                cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                            }

                        }
                    }
                }

            }

            //Scrape for event dialogue.
            else if (cue.name == "Events")
            {
                foreach (var fileName in cue.stringsFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        string dialoguePath2 = Path.Combine(stringsPath, fileName);
                        var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);

                        //Scrape Strings/Events.yaml for dialogue strings
                        if (fileName.Contains("StringsFromCSFiles"))
                        {
                            //Scrape Strings/StringsFromCSFiles.yaml
                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                string key = pair.Key;
                                string rawDialogue = pair.Value;
                                //If the key contains the character's name.
                                if (!key.Contains("Event")) continue;
                                List<string> cleanDialogues = new List<string>();
                                cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                foreach (var str in cleanDialogues)
                                {
                                    cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                }
                            }
                        }
                        //Scrape Strings/Events.yaml
                        if (fileName.Contains("Events"))
                        {
                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                string key = pair.Key;
                                string rawDialogue = pair.Value;
                                //If the key contains the character's name.
                                List<string> cleanDialogues = new List<string>();
                                cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                foreach (var str in cleanDialogues)
                                {
                                    cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                }
                            }
                        }
                    }
                }
            }

            //Scrape for mail dialogue.
            else if (cue.name == "Mail")
            {
                foreach (var fileName in cue.dataFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    //basically this will never run but can be used below to also add in dialogue.
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        string dialoguePath2 = Path.Combine(dataPath, fileName);
                        var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);

                        //Scrape the whole dictionary looking for the character's name.
                        foreach (KeyValuePair<string, string> pair in DialogueDict)
                        {
                            //Get the key in the dictionary
                            string key = pair.Key;
                            string rawDialogue = pair.Value;
                            //If the key contains the character's name.

                            string cleanDialogue = "";
                            cleanDialogue = sanitizeDialogueFromMailDictionary(rawDialogue);                    
                            cue.addDialogue(cleanDialogue, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty
                        }
                    }
                }

            }

            //Used to scrape Content/strings/Characters.yaml.
            else if (cue.name == "Characters")
            {         
                foreach (var fileName in cue.stringsFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        string dialoguePath2 = Path.Combine(stringsPath, fileName);
                        var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);

                        if (fileName.Contains("Characters"))
                        {
                            //Scrape the whole dictionary looking for the character's name.
                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                string key = pair.Key;
                                string rawDialogue = pair.Value;
                                //If the key contains the character's name.

                                List<string> cleanDialogues = new List<string>();
                                cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                foreach (var str in cleanDialogues)
                                {
                                    cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                }

                            }
                            continue;
                        }
                        //!!!!!!!!!!!!If I ever want to make this moddable add a generic scrape here.
                    }
                }

            }

            else if (cue.name == "Notes")
            {
                //Used mainly to scrape Content/Strings/Notes.yaml
                foreach (var fileName in cue.stringsFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    //basically this will never run but can be used below to also add in dialogue.
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        string dialoguePath2 = Path.Combine(stringsPath, fileName);
                        var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);

                            //Scrape the whole dictionary looking for the character's name.
                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                string key = pair.Key;
                                string rawDialogue = pair.Value;
                                //If the key contains the character's name.

                                List<string> cleanDialogues = new List<string>();
                                cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                foreach (var str in cleanDialogues)
                                {
                                    cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                }

                            }
                            continue;
                    }
                }


                //Used mainly to scrape Content/Data/SecretNotes.yaml
                foreach (var fileName in cue.dataFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        string dialoguePath2 = Path.Combine(dataPath, fileName);
                        var DialogueDict = ModHelper.Content.Load<Dictionary<int, string>>(dialoguePath2, ContentSource.GameContent);

                            //Scrape the whole dictionary looking for the character's name.
                            foreach (KeyValuePair<int, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                int key = pair.Key;
                                string rawDialogue = pair.Value;
                                //If the key contains the character's name.

                                List<string> cleanDialogues = new List<string>();
                                cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                foreach (var str in cleanDialogues)
                                {
                                    cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                }

                            }
                            continue;
                        
                    }
                }

            }

            //used to scrape Content/Strings/Utility.yaml
            else if (cue.name == "Utility")
            {
                foreach (var fileName in cue.stringsFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    //basically this will never run but can be used below to also add in dialogue.
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        string dialoguePath2 = Path.Combine(stringsPath, fileName);
                        var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);

                        //Scrape the whole dictionary looking for the character's name.
                        foreach (KeyValuePair<string, string> pair in DialogueDict)
                        {
                            //Get the key in the dictionary
                            string key = pair.Key;
                            string rawDialogue = pair.Value;
                            //If the key contains the character's name.

                            string cleanDialogue = "";
                            cleanDialogue = sanitizeDialogueFromMailDictionary(rawDialogue);
                            cue.addDialogue(cleanDialogue, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty
                        }
                    }
                }

            }

            else if (cue.name == "Quests")
            {
                foreach (var fileName in cue.stringsFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    string dialoguePath2 = Path.Combine(stringsPath, fileName);
                    string root = Game1.content.RootDirectory;///////USE THIS TO CHECK FOR EXISTENCE!!!!!
                    if (!File.Exists(Path.Combine(root, dialoguePath2)))
                    {
                        ModMonitor.Log("Dialogue file not found for:" + fileName + ". This might not necessarily be a mistake just a safety check.");
                        continue; //If the file is not found for some reason...
                    }
                    var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);
                        //Scrape the whole dictionary looking for the character's name.
                        foreach (KeyValuePair<string, string> pair in DialogueDict)
                        {
                            //Get the key in the dictionary
                            string key = pair.Key;
                            string rawDialogue = pair.Value;
                        //If the key contains the character's name.

                        List<string> strippedRawQuestDialogue = new List<string>();
                        List<string> strippedFreshQuestDialogue = new List<string>();
                        strippedRawQuestDialogue = rawDialogue.Split('/').ToList();
                        string prompt = strippedRawQuestDialogue.ElementAt(2);
                        string response = strippedRawQuestDialogue.ElementAt(strippedRawQuestDialogue.Count - 1);

                        strippedFreshQuestDialogue.Add(prompt);
                        if (response != "true" || response != "false")
                        {
                            strippedFreshQuestDialogue.Add(response);
                        }
                       
                        List<string> cleanDialogues = new List<string>();
                        foreach (var dia in strippedFreshQuestDialogue)
                        {
                            cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                            foreach (var str in cleanDialogues)
                            {
                                cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                            }
                        }
                        }
                        continue;
                    

                }
                }

            else if (cue.name == "NPCGiftTastes")
            {
                foreach (var fileName in cue.dataFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    string dialoguePath2 = Path.Combine(stringsPath, fileName);
                    string root = Game1.content.RootDirectory;///////USE THIS TO CHECK FOR EXISTENCE!!!!!
                    if (!File.Exists(Path.Combine(root, dialoguePath2)))
                    {
                        ModMonitor.Log("Dialogue file not found for:" + fileName + ". This might not necessarily be a mistake just a safety check.");
                        continue; //If the file is not found for some reason...
                    }
                    var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);
                    //Scrape the whole dictionary looking for the character's name.

                    List<string> ignoreKeys = new List<string>();
                    ignoreKeys.Add("Universal_Love");
                    ignoreKeys.Add("Universal_Like");
                    ignoreKeys.Add("Universal_Neutral");
                    ignoreKeys.Add("Universal_Dislike");
                    ignoreKeys.Add("Universal_Hate");

                    foreach (KeyValuePair<string, string> pair in DialogueDict)
                    {
                        //Get the key in the dictionary
                        string key = pair.Key;
                        string rawDialogue = pair.Value;

                       //Check to see if I need to ignore this key in my dictionary I am scaping.
                        bool ignore = false;
                        foreach(var value in ignoreKeys)
                        {
                            if (key == value)
                            {
                                ignore = true;
                                break;
                            }
                        }

                        if (ignore) continue;

                        List<string> strippedRawQuestDialogue = new List<string>();
                        List<string> strippedFreshQuestDialogue = new List<string>();
                        strippedRawQuestDialogue = rawDialogue.Split('/').ToList();

                        string prompt1 = strippedRawQuestDialogue.ElementAt(0);
                        string prompt2 = strippedRawQuestDialogue.ElementAt(2);
                        string prompt3 = strippedRawQuestDialogue.ElementAt(4);
                        string prompt4 = strippedRawQuestDialogue.ElementAt(6);
                        string prompt5 = strippedRawQuestDialogue.ElementAt(8);

                        strippedFreshQuestDialogue.Add(prompt1);
                        strippedFreshQuestDialogue.Add(prompt2);
                        strippedFreshQuestDialogue.Add(prompt3);
                        strippedFreshQuestDialogue.Add(prompt4);
                        strippedFreshQuestDialogue.Add(prompt5);

                        List<string> cleanDialogues = new List<string>();
                        foreach (var dia in strippedFreshQuestDialogue)
                        {
                            cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                            foreach (var str in cleanDialogues)
                            {
                                cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                            }
                        }
                    }
                    continue;
                }
            }

            else if (cue.name == "SpeechBubbles")
            {
                foreach (var fileName in cue.dataFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    string dialoguePath2 = Path.Combine(stringsPath, fileName);
                    string root = Game1.content.RootDirectory;///////USE THIS TO CHECK FOR EXISTENCE!!!!!
                    if (!File.Exists(Path.Combine(root, dialoguePath2)))
                    {
                        ModMonitor.Log("Dialogue file not found for:" + fileName + ". This might not necessarily be a mistake just a safety check.");
                        continue; //If the file is not found for some reason...
                    }
                    var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);
                    //Scrape the whole dictionary looking for the character's name.

                    foreach (KeyValuePair<string, string> pair in DialogueDict)
                    {
                        //Get the key in the dictionary
                        string key = pair.Key;
                        string rawDialogue = pair.Value;
                        string cleanString = sanitizeDialogueFromSpeechBubblesDictionary(rawDialogue);
                        cue.addDialogue(cleanString, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                    }
                    continue;
                }
            }


            //Dialogue scrape for npc specific text.
            else
            {
                foreach (var fileName in cue.dialogueFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    //basically this will never run but can be used below to also add in dialogue.
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        string dialoguePath2 = Path.Combine(dialoguePath, fileName);
                        string root=Game1.content.RootDirectory;///////USE THIS TO CHECK FOR EXISTENCE!!!!!
                        if (!File.Exists(Path.Combine(root,dialoguePath2)))
                        {
                            ModMonitor.Log("Dialogue file not found for:" + fileName+". This might not necessarily be a mistake just a safety check.");
                            continue; //If the file is not found for some reason...
                        }
                        var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);

                        //Scraping the rainy dialogue file.
                        if (fileName.Contains("rainy"))
                        {
                            //Scrape the whole dictionary looking for the character's name.
                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                string key = pair.Key;
                                string rawDialogue = pair.Value;
                                //If the key contains the character's name.
                                if (key.Contains(cue.name))
                                {
                                    List<string> cleanDialogues = new List<string>();
                                    cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                    foreach (var str in cleanDialogues)
                                    {
                                        cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                    }
                                }
                            }
                            continue;
                        }

                        //Check for just my generic file
                        if (fileName.Contains("MarriageDialogue") && !fileName.Contains("MarriageDialogue"+cue.name))
                        {
                            //Scrape the whole dictionary looking for other character's names to ignore.
                            if (!replacementStrings.spouseNames.Contains(cue.name)) continue;
                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                string key = pair.Key;
                                string rawDialogue = pair.Value;

                                //get my current charcter's name
                                //check the current key
                                //if my key contains a different spouse's name continue the loop
                                //else sanitize it and add it to my list
                                foreach (var spouse in replacementStrings.spouseNames)
                                {
                                    if (key.Contains(spouse) && spouse != cue.name)
                                    {
                                        //If the key contains a spouse name and it is not my character's name...
                                        continue;
                                    }
                                    //If the key contains the character's name or is generic dialogue.
                                    if (key.Contains(cue.name))
                                    {
                                        List<string> cleanDialogues = new List<string>();
                                        cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                        foreach (var str in cleanDialogues)
                                        {
                                            cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                        }
                                    }
                                }
                            }

                        }

                        //Check for character specific marriage dialogue
                        if (fileName.Contains("MarriageDialogue"+cue.name))
                        {
                            //Scrape the whole dictionary looking for other character's names to ignore.
                            if (!replacementStrings.spouseNames.Contains(cue.name)) continue;
                            foreach (KeyValuePair<string, string> pair in DialogueDict)
                            {
                                //Get the key in the dictionary
                                string key = pair.Key;
                                string rawDialogue = pair.Value;
                                //If the key contains the character's name or is generic dialogue.
                                if (key.Contains(cue.name))
                                {
                                    List<string> cleanDialogues = new List<string>();
                                    cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                    foreach (var str in cleanDialogues)
                                    {
                                        cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                    }
                                }
                            }
                        }
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
                }
                foreach (var fileName in cue.dataFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    string dialoguePath2 = Path.Combine(dataPath, fileName);
                    string root = Game1.content.RootDirectory;///////USE THIS TO CHECK FOR EXISTENCE!!!!!
                    if (!File.Exists(Path.Combine(root, dialoguePath2)))
                    {
                        ModMonitor.Log("Dialogue file not found for:" + fileName + ". This might not necessarily be a mistake just a safety check.");
                        continue; //If the file is not found for some reason...
                    }

                    var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);

                    //Load in engagement dialogue for this npc.
                    if (fileName.Contains("EngagementDialogue"))
                    {
                        //Scrape the whole dictionary looking for the character's name.
                        foreach (KeyValuePair<string, string> pair in DialogueDict)
                        {
                            //Get the key in the dictionary
                            string key = pair.Key;
                            string rawDialogue = pair.Value;
                            //If the key contains the character's name.
                            if (key.Contains(cue.name))
                            {
                                List<string> cleanDialogues = new List<string>();
                                cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                foreach (var str in cleanDialogues)
                                {
                                    cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                }
                            }
                        }
                        continue;
                    }
                }
                foreach (var fileName in cue.stringsFileNames)
                {
                    ModMonitor.Log("    Scraping dialogue file: " + fileName, LogLevel.Info);
                    string dialoguePath2 = Path.Combine(stringsPath, fileName);
                    string root = Game1.content.RootDirectory;///////USE THIS TO CHECK FOR EXISTENCE!!!!!
                    if (!File.Exists(Path.Combine(root, dialoguePath2)))
                    {
                        ModMonitor.Log("Dialogue file not found for:" + fileName + ". This might not necessarily be a mistake just a safety check.");
                        continue; //If the file is not found for some reason...
                    }
                    var DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);

                    //Load in super generic dialogue for this npc. This may or may not be a good idea....
                    if (fileName.Contains("StringsFromCSFiles"))
                    {
                        //Scrape the whole dictionary looking for the character's name.
                        foreach (KeyValuePair<string, string> pair in DialogueDict)
                        {
                            //Get the key in the dictionary
                            string key = pair.Key;
                            string rawDialogue = pair.Value;
                            //If the key contains the character's name.
                            if (key.Contains("NPC"))
                            {
                                List<string> cleanDialogues = new List<string>();
                                cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                foreach (var str in cleanDialogues)
                                {
                                    cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                }
                            }
                        }
                        continue;
                    }

                    if (fileName.Contains(cue.name))
                    {
                        dialoguePath2 = Path.Combine(stringsPath,"schedules", fileName);
                        root = Game1.content.RootDirectory;///////USE THIS TO CHECK FOR EXISTENCE!!!!!
                        if (!File.Exists(Path.Combine(root, dialoguePath2)))
                        {
                            ModMonitor.Log("Dialogue file not found for:" + fileName + ". This might not necessarily be a mistake just a safety check.");
                            continue; //If the file is not found for some reason...
                        }
                        DialogueDict = ModHelper.Content.Load<Dictionary<string, string>>(dialoguePath2, ContentSource.GameContent);
                        //Scrape the whole dictionary looking for the character's name.
                        foreach (KeyValuePair<string, string> pair in DialogueDict)
                        {
                            //Get the key in the dictionary
                            string key = pair.Key;
                            string rawDialogue = pair.Value;
                            //If the key contains the character's name.
                                List<string> cleanDialogues = new List<string>();
                                cleanDialogues = sanitizeDialogueFromDictionaries(rawDialogue);
                                foreach (var str in cleanDialogues)
                                {
                                    cue.addDialogue(str, ""); //Make a new dialogue line based off of the text, but have the .wav value as empty.
                                }
                            
                        }
                        continue;
                    }
                }
            }

            ModHelper.WriteJsonFile<CharacterVoiceCue>(path,cue);
            DialogueCues.Add(cue.name, cue);
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

        public string sanitizeDialogueFromSpeechBubblesDictionary(string text)
        {
            if (text.Contains("{0}"))
            {
                text = text.Replace("{0}", replacementStrings.farmerName);
            }
            if (text.Contains("{1}"))
            {
                text = text.Replace("{1}", replacementStrings.farmName);
            }
            return text;
        }

        /// <summary>
        /// Used to remove all garbage strings from Content/Data/mail.yaml
        /// </summary>
        /// <param name="mailText"></param>
        /// <returns></returns>
        public string sanitizeDialogueFromMailDictionary(string mailText)
        {

            List<string> texts = mailText.Split('%').ToList();

            string splicedText = texts.ElementAt(0); //The actual message of the mail minus the items stored at the end.

            if (splicedText.Contains("@"))
            {
                splicedText = splicedText.Replace("@", replacementStrings.farmerName);
            }

            if (splicedText.Contains("^"))
            {
                splicedText = splicedText.Replace("^", "");
            }

            if (splicedText.Contains("\""))
            {
                splicedText = splicedText.Replace("\"", "");
            }

            return splicedText;

        }
    }
}
