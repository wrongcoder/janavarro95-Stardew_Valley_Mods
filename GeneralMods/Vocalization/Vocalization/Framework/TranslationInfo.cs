using System;
using System.Collections.Generic;
using System.IO;
using StardewValley;

namespace Vocalization.Framework
{
    /// <summary>A class which deals with handling different translations for Vocalization should other voice teams ever wish to voice act for that language.</summary>
    public class TranslationInfo
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The language names supported by this mod.</summary>
        public List<string> LanguageNames { get; } = new List<string>();

        /// <summary>The current translation mode for the mod, so that it knows what files to load at the beginning of the game.</summary>
        public string CurrentTranslation { get; set; } = "English";

        /// <summary>Holds the info for what translation has what file extension.</summary>
        public Dictionary<string, string> TranslationFileInfo { get; } = new Dictionary<string, string>();

        public Dictionary<string, LocalizedContentManager.LanguageCode> TranslationCodes { get; } = new Dictionary<string, LocalizedContentManager.LanguageCode>();


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        public TranslationInfo()
        {
            this.LanguageNames.Add("English");
            this.LanguageNames.Add("Spanish");
            this.LanguageNames.Add("Chinese");
            this.LanguageNames.Add("Japanese");
            this.LanguageNames.Add("Russian");
            this.LanguageNames.Add("German");
            this.LanguageNames.Add("Brazillian Portuguese");

            this.TranslationFileInfo.Add("English", ".xnb");
            this.TranslationFileInfo.Add("Spanish", ".es-ES.xnb");
            this.TranslationFileInfo.Add("Chinese", ".zh-CN.xnb");
            this.TranslationFileInfo.Add("Japanese", ".ja-JP.xnb");
            this.TranslationFileInfo.Add("Russian", ".ru-RU.xnb");
            this.TranslationFileInfo.Add("German", ".de-DE.xnb");
            this.TranslationFileInfo.Add("Brazillian Portuguese", ".pt-BR.xnb");
            
            this.TranslationCodes.Add("English", LocalizedContentManager.LanguageCode.en);
            this.TranslationCodes.Add("Spanish", LocalizedContentManager.LanguageCode.es);
            this.TranslationCodes.Add("Chinese", LocalizedContentManager.LanguageCode.zh);
            this.TranslationCodes.Add("Japanese", LocalizedContentManager.LanguageCode.ja);
            this.TranslationCodes.Add("Russian", LocalizedContentManager.LanguageCode.ru);
            this.TranslationCodes.Add("German", LocalizedContentManager.LanguageCode.de);
            this.TranslationCodes.Add("Brazillian Portuguese", LocalizedContentManager.LanguageCode.pt);
        }

        public string getTranslationNameFromPath(string fullPath)
        {
            return Path.GetFileName(fullPath);
        }

        public void changeLocalizedContentManagerFromTranslation(string translation)
        {
            string tra = this.getTranslationNameFromPath(translation);
            LocalizedContentManager.CurrentLanguageCode = !this.TranslationCodes.TryGetValue(tra, out LocalizedContentManager.LanguageCode code)
                ? LocalizedContentManager.LanguageCode.en
                : code;
            return;
        }

        public void resetLocalizationCode()
        {
            LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.en;
        }

        /// <summary>Gets the proper file extension for the current translation.</summary>
        public string getFileExtentionForTranslation(string path)
        {
            /*
            bool f = translationFileInfo.TryGetValue(translation, out string value);
            if (!f) return ".xnb";
            else return value;
            */
            string translation = Path.GetFileName(path);
            try
            {
                return this.TranslationFileInfo[translation];
            }
            catch (Exception err)
            {

                Vocalization.ModMonitor.Log(err.ToString());
                Vocalization.ModMonitor.Log("Attempted to get translation: " + translation);
                return ".xnb";
            }
        }

        /// <summary>Gets the proper XNB for Buildings (aka Blueprints) from the data folder.</summary>
        public string getBuildingXNBForTranslation(string translation)
        {
            string buildings = "Blueprints";
            return buildings + this.getFileExtentionForTranslation(translation);
        }

        /// <summary>Gets the proper XNB file for the name passed in. Combines the file name with it's proper translation extension.</summary>
        public string getXNBForTranslation(string xnbFileName, string translation)
        {
            return xnbFileName + this.getFileExtentionForTranslation(translation);
        }

        /// <summary>Loads an XNB file from StardewValley/Content</summary>
        public string LoadXNBFile(string xnbFileName, string key, string translation)
        {
            string xnb = xnbFileName + this.getFileExtentionForTranslation(translation);
            Dictionary<string, string> loadedDict = Game1.content.Load<Dictionary<string, string>>(xnb);

            if (!loadedDict.TryGetValue(key, out string loaded))
            {
                Vocalization.ModMonitor.Log("Big issue: Key not found in file:" + xnb + " " + key);
                return "";
            }
            return loaded;
        }

        public virtual string LoadString(string path, string translation, object sub1, object sub2, object sub3)
        {
            string format = this.LoadString(path, translation);
            try
            {
                return string.Format(format, sub1, sub2, sub3);
            }
            catch { }

            return format;
        }

        public virtual string LoadString(string path, string translation, object sub1, object sub2)
        {
            string format = this.LoadString(path, translation);
            try
            {
                return string.Format(format, sub1, sub2);
            }
            catch { }

            return format;
        }

        public virtual string LoadString(string path, string translation, object sub1)
        {
            string format = this.LoadString(path, translation);
            try
            {
                return string.Format(format, sub1);
            }
            catch { }

            return format;
        }

        public virtual string LoadString(string path, string translation)
        {
            this.parseStringPath(path, out string assetName, out string key);

            return this.LoadXNBFile(assetName, key, translation);
        }

        public virtual string LoadString(string path, string translation, params object[] substitutions)
        {
            string format = this.LoadString(path, translation);
            if (substitutions.Length != 0)
            {
                try
                {
                    return string.Format(format, substitutions);
                }
                catch { }
            }
            return format;
        }
        
        private void parseStringPath(string path, out string assetName, out string key)
        {
            int length = path.IndexOf(':');
            assetName = path.Substring(0, length);
            key = path.Substring(length + 1, path.Length - length - 1);
        }
    }
}
