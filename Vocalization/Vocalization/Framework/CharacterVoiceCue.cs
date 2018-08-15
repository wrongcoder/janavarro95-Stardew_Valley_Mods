using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// The name of the dialogue file to scrape from Content/Characters/Dialogue for inputting values into the dictionary of dialogueCues.
        /// </summary>
        public List<string> dialogueFileNames;

        /// <summary>
        /// The name of the files in Content/Strings to scrape for dialogue.
        /// </summary>
        public List<string> stringsFileNames;

        /// <summary>
        /// The names of the files in Content/Data to scrape for dialogue.
        /// </summary>
        public List<string> dataFileNames;

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
            this.stringsFileNames = new List<string>();
            this.dialogueFileNames = new List<string>();
            this.dataFileNames = new List<string>();
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

        public void addDialogue(string key, string value)
        {
            if (dialogueCues.ContainsKey(key))
            {
                return;
            }
            else
            {
                this.dialogueCues.Add(key, value);
            }
        }


        public void initializeEnglishScrape()
        {
            if (name == "TV")
            {
                dataFileNames.Add("CookingChannel.xnb");
                dataFileNames.Add("InterviewShow.xnb");
                dataFileNames.Add("TipChannel.xnb");
                stringsFileNames.Add("StringsFromCSFiles.xnb");

            }
            else if (name == "Shops")
            {
                stringsFileNames.Add("StringsFromCSFiles.xnb");
            }
            else if (name == "ExtraDialogue")
            {
                dataFileNames.Add("ExtraDialogue.xnb");
            }
            else if (name == "LocationDialogue")
            {
                stringsFileNames.Add("Locations.xnb");
                stringsFileNames.Add("StringsFromMaps.xnb");
            }
            else if (name == "Events")
            {
                stringsFileNames.Add("Events.xnb");
                stringsFileNames.Add("StringsFromCSFiles.xnb");

            }
            else if (name == "Mail")
            {
                dataFileNames.Add("mail.xnb");
            }
            else if (name == "Characters")
            {
                stringsFileNames.Add("Characters.xnb");
            }
            else if (name == "Notes")
            {
                stringsFileNames.Add("Notes.xnb");
                dataFileNames.Add("SecretNotes.xnb");
            }
            else if (name == "Utility")
            {
                stringsFileNames.Add("StringsFromCSFiles.xnb");
            }

            else if (name == "GiftTastes")
            {
                dataFileNames.Add("NPCGiftTastes.xnb");
            }

            else if (name == "SpeechBubbles")
            {
                stringsFileNames.Add("SpeechBubbles.xnb");
            }

            else if (name == "Temp")
            {
                Vocalization.ModMonitor.Log("Scraping dialogue file: Temp.xnb", StardewModdingAPI.LogLevel.Debug);
                //dataFileNames.Add(Path.Combine("Events", "Temp.xnb"));

                Dictionary<string, string> meh = Game1.content.Load<Dictionary<string, string>>(Path.Combine("Data", "Events", "Temp.xnb"));

                foreach(KeyValuePair<string,string> pair in meh)
                {
                    if(pair.Key== "decorate")
                    {
                        string dia = pair.Value;
                        Vocalization.ModMonitor.Log(dia);
                        string[]values = dia.Split('\"');
                        
                        foreach(var v in values)
                        {
                            Vocalization.ModMonitor.Log(v);
                            Vocalization.ModMonitor.Log("HELLO?");
                        }

                        List<string> goodValues = new List<string>();
                        goodValues.Add(values.ElementAt(1));
                        goodValues.Add(values.ElementAt(3));
                        goodValues.Add(values.ElementAt(5));

                        foreach(var sentence in goodValues)
                        {
                            List<string> clean = Vocalization.sanitizeDialogueFromDictionaries(sentence);
                            foreach(var cleanSentence in clean)
                            {
                                this.dialogueCues.Add(cleanSentence, "");
                            }
                        }
                        
                    }

                    if (pair.Key == "leave")
                    {
                        string dia = pair.Value;
                        string[] values = dia.Split('\"');
                        List<string> goodValues = new List<string>();
                        goodValues.Add(values.ElementAt(1));
                        goodValues.Add(values.ElementAt(3));
                        goodValues.Add(values.ElementAt(5));

                        foreach (var sentence in goodValues)
                        {
                            List<string> clean = Vocalization.sanitizeDialogueFromDictionaries(sentence);
                            foreach (var cleanSentence in clean)
                            {
                                this.dialogueCues.Add(cleanSentence, "");
                            }
                        }

                    }

                    if (pair.Key == "tooBold")
                    {
                        string dia = pair.Value;
                        string[] values = dia.Split('\"');
                        List<string> goodValues = new List<string>();
                        goodValues.Add(values.ElementAt(1));

                        foreach (var sentence in goodValues)
                        {
                            List<string> clean = Vocalization.sanitizeDialogueFromDictionaries(sentence);
                            foreach (var cleanSentence in clean)
                            {
                                this.dialogueCues.Add(cleanSentence, "");
                            }
                        }
                    }

                    if (pair.Key == "poppy" || pair.Key=="heavy" || pair.Key=="techno"|| pair.Key=="honkytonk")
                    {
                        string dia = pair.Value;
                        string[] values = dia.Split('\"');
                        List<string> goodValues = new List<string>();
                        goodValues.Add(values.ElementAt(1));
                        goodValues.Add(values.ElementAt(3));
                        goodValues.Add(values.ElementAt(5));
                        goodValues.Add(values.ElementAt(7));
                        goodValues.Add(values.ElementAt(9));
                        goodValues.Add(values.ElementAt(11));
                        goodValues.Add(values.ElementAt(13));
                        goodValues.Add(values.ElementAt(15));
                        goodValues.Add(values.ElementAt(17));
                        goodValues.Add(values.ElementAt(19));

                        foreach (var sentence in goodValues)
                        {
                            List<string> clean = Vocalization.sanitizeDialogueFromDictionaries(sentence);
                            foreach (var cleanSentence in clean)
                            {
                                try
                                {
                                    this.dialogueCues.Add(cleanSentence, "");
                                }
                                catch (Exception err)
                                {

                                }
                            }
                        }

                    }
                }



            }

            else
            {
                dialogueFileNames.Add(name + ".xnb");
                dialogueFileNames.Add("rainy.xnb");
                dialogueFileNames.Add("MarriageDialogue.xnb");
                dialogueFileNames.Add("MarriageDialogue"+name+".xnb");

                dataFileNames.Add("EngagementDialogue.xnb");

                stringsFileNames.Add("StringsFromCSFiles.xnb");
                stringsFileNames.Add(name + ".xnb");
            }



        }
    }
}
