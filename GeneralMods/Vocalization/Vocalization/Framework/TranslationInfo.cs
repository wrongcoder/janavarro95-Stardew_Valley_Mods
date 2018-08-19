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
    /// A class which deals with handling different translations for Vocalization should other voice teams ever wish to voice act for that language.
    /// </summary>
    public class TranslationInfo
    {
        /// <summary>
        /// The list of all supported translations by this mod.
        /// </summary>
        public List<string> translations;

        /// <summary>
        /// The current translation mode for the mod, so that it knows what files to load at the beginning of the game.
        /// </summary>
        public string currentTranslation;

        /// <summary>
        /// Holds the info for what translation has what file extension.
        /// </summary>
        public Dictionary<string, string> translationFileInfo;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TranslationInfo()
        {
            translations = new List<string>();

            translationFileInfo = new Dictionary<string, string>();

            translations.Add("English");
            translations.Add("Spanish");
            translations.Add("Chinese");
            translations.Add("Japanese");
            translations.Add("Russian");
            translations.Add("German");
            translations.Add("Brazillian Portuguese");

            currentTranslation = "English";

            translationFileInfo.Add("English", ".xnb");
            translationFileInfo.Add("Spanish", ".es - ES.xnb");
            translationFileInfo.Add("Chinese", ".zh-CN.xnb");
            translationFileInfo.Add("Japanese", ".ja-JP.xnb");
            translationFileInfo.Add("Russian", ".ru-RU.xnb");
            translationFileInfo.Add("German", ".de-DE.xnb");
            translationFileInfo.Add("Brazillian Portuguese", ".pt-BR.xnb");


        }

        /// <summary>
        /// Gets the proper file extension for the current translation.
        /// </summary>
        /// <param name="translation"></param>
        /// <returns></returns>
        public string getFileExtentionForTranslation(string translation)
        {
            bool f = translationFileInfo.TryGetValue(translation, out string value);
            if (f == false) return ".xnb";
            else return value;
        }

        /// <summary>
        /// Gets the proper XNB for Buildings (aka Blueprints) from the data folder.
        /// </summary>
        /// <param name="translation"></param>
        /// <returns></returns>
        public string getBuildingXNBForTranslation(string translation)
        {
            string buildings = "Blueprints";
            return buildings + getFileExtentionForTranslation(translation);
        }

        /// <summary>
        /// Gets the proper XNB file for the name passed in. Combines the file name with it's proper translation extension.
        /// </summary>
        /// <param name="xnbFileName"></param>
        /// <param name="translation"></param>
        /// <returns></returns>
        public string getXNBForTranslation(string xnbFileName, string translation)
        {
            return xnbFileName + translation;
        }

        /// <summary>
        /// Loads a coresponding string from a data file in Content/Strings
        /// </summary>
        /// <param name="xnbFileName"></param>
        /// <param name="key"></param>
        /// <param name="translation"></param>
        /// <returns></returns>
        public string LoadStringFromStringsFile(string xnbFileName,string key,string translation)
        {
            string str = "Strings";
            string xnb = xnbFileName + getFileExtentionForTranslation(translation);
            string path = Path.Combine(str, xnbFileName);
            Dictionary<string,string> loadedDict = Game1.content.Load<Dictionary<string, string>>(path);
            return loadedDict[key];
        }

        public string LoadStringFromStringsFile(string xnbFileName, string key, string translation, object sub1)
        {
            string loaded = LoadStringFromStringsFile(xnbFileName, key, translation);
            try
            {
                return string.Format(loaded, sub1);
            }
            catch (Exception err)
            {
                return loaded;
            }
        }

        public string LoadStringFromStringsFile(string xnbFileName, string key, string translation, object sub1, object sub2)
        {
            string loaded = LoadStringFromStringsFile(xnbFileName, key, translation);
            try
            {
                return string.Format(loaded, sub1,sub2);
            }
            catch (Exception err)
            {
                return loaded;
            }
        }

        public string LoadStringFromStringsFile(string xnbFileName, string key, string translation, object sub1, object sub2, object sub3)
        {
            string loaded = LoadStringFromStringsFile(xnbFileName, key, translation);
            try
            {
                return string.Format(loaded, sub1, sub2,sub3);
            }
            catch (Exception err)
            {
                return loaded;
            }
        }

        /// <summary>
        /// Loads a corresponding string from a data file in Content/Data
        /// </summary>
        /// <param name="xnbFileName"></param>
        /// <param name="key"></param>
        /// <param name="translation"></param>
        /// <returns></returns>
        public string LoadStringFromDataFile(string xnbFileName, string key, string translation)
        {
            string str = "Data";
            string xnb = xnbFileName + getFileExtentionForTranslation(translation);
            string path = Path.Combine(str, xnbFileName);
            Dictionary<string, string> loadedDict = Game1.content.Load<Dictionary<string, string>>(path);
            string loaded = loadedDict[key];
            return loaded;
        }

        public string LoadStringFromDataFile(string xnbFileName, string key, string translation, object sub1)
        {
            string loaded = LoadStringFromDataFile(xnbFileName, key, translation);
            try
            {
                return string.Format(loaded, sub1);
            }
            catch (Exception err)
            {
                return loaded;
            }
        }

        public string LoadStringFromDataFile(string xnbFileName, string key, string translation, object sub1, object sub2)
        {
            string loaded = LoadStringFromDataFile(xnbFileName, key, translation);
            try
            {
                return string.Format(loaded, sub1,sub2);
            }
            catch (Exception err)
            {
                return loaded;
            }
        }

        public string LoadStringFromDataFile(string xnbFileName, string key, string translation, object sub1, object sub2, object sub3)
        {
            string loaded = LoadStringFromDataFile(xnbFileName, key, translation);
            try
            {
                return string.Format(loaded, sub1,sub2,sub3);
            }
            catch (Exception err)
            {
                return loaded;
            }
        }

        public virtual string LoadString(string path, string translation,object sub1, object sub2, object sub3)
        {
            string assetName;
            string key;
            this.parseStringPath(path, out assetName, out key);

            string[] split = path.Split(new string[]{ Environment.NewLine },StringSplitOptions.None);
            string folder = split[0];
            if (folder == "Data")
            {
                if (sub3 != null) {
                    return LoadStringFromDataFile(assetName, key, translation, sub1, sub2, sub3);
                }
                if (sub2 != null)
                {
                    return LoadStringFromDataFile(assetName, key, translation, sub1, sub2);
                }

                if (sub1 != null)
                {
                    return LoadStringFromDataFile(assetName, key, translation, sub1);
                }
                return LoadStringFromDataFile(assetName, key, translation);
            }
            if (folder == "Strings")
            {
                if (sub3 != null)
                {
                    return LoadStringFromStringsFile(assetName, key, translation, sub1, sub2, sub3);
                }
                if (sub2 != null)
                {
                    return LoadStringFromStringsFile(assetName, key, translation, sub1, sub2);
                }

                if (sub1 != null)
                {
                    return LoadStringFromStringsFile(assetName, key, translation, sub1);
                }
                return LoadStringFromStringsFile(assetName, key, translation);
            }
            return "";
        }

        public virtual string LoadString(string path, string translation, object sub1, object sub2)
        {
            string assetName;
            string key;
            this.parseStringPath(path, out assetName, out key);

            string[] split = path.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string folder = split[0];
            if (folder == "Data")
            {
                if (sub2 != null)
                {
                    return LoadStringFromDataFile(assetName, key, translation, sub1, sub2);
                }

                if (sub1 != null)
                {
                    return LoadStringFromDataFile(assetName, key, translation, sub1);
                }
                return LoadStringFromDataFile(assetName, key, translation);
            }
            if (folder == "Strings")
            {
                if (sub2 != null)
                {
                    return LoadStringFromStringsFile(assetName, key, translation, sub1, sub2);
                }

                if (sub1 != null)
                {
                    return LoadStringFromStringsFile(assetName, key, translation, sub1);
                }
                return LoadStringFromStringsFile(assetName, key, translation);
            }
            return "";
        }

        public virtual string LoadString(string path, string translation, object sub1)
        {
            string assetName;
            string key;
            this.parseStringPath(path, out assetName, out key);

            string[] split = path.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string folder = split[0];
            if (folder == "Data")
            {

                if (sub1 != null)
                {
                    return LoadStringFromDataFile(assetName, key, translation, sub1);
                }
                return LoadStringFromDataFile(assetName, key, translation);
            }
            if (folder == "Strings")
            {
                if (sub1 != null)
                {
                    return LoadStringFromStringsFile(assetName, key, translation, sub1);
                }
                return LoadStringFromStringsFile(assetName, key, translation);
            }
            return "";
        }

        public virtual string LoadString(string path, string translation)
        {
            string assetName;
            string key;
            this.parseStringPath(path, out assetName, out key);

            string[] split = path.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string folder = split[0];
            if (folder == "Data")
            {
                return LoadStringFromDataFile(assetName, key, translation);
            }
            if (folder == "Strings") {

                return LoadStringFromStringsFile(assetName, key, translation);
            }
            return "";
        }

        public virtual string LoadString(string path,string translation, params object[] substitutions)
        {
            string format = this.LoadString(path,translation);
            if (substitutions.Length != 0)
            {
                try
                {
                    return string.Format(format, substitutions);
                }
                catch (Exception ex)
                {
                }
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
