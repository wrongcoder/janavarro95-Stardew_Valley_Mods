using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize;

namespace Omegasis.Revitalize.Framework.Utilities
{
    /// <summary>
    /// Useful utilities for handling json files.
    /// </summary>
    public class JsonUtilities
    {
        /// <summary>
        /// Loads a string dictionary from a path relative to the Mod folder's location.
        /// </summary>
        /// <param name="RelativePathToFile"></param>
        /// <returns></returns>
        public static Dictionary<string,string> LoadStringDictionaryFile(string RelativePathToFile)
        {
            return ModCore.ModHelper.Content.Load<Dictionary<string, string>>(RelativePathToFile);
        }

        /// <summary>
        /// Loads a string from a string dictionary.
        /// </summary>
        /// <param name="Key">The key in the json file to load from.</param>
        /// <param name="RelativePathToFile">The relative path to the dictionary file from the mods' content folder.</param>
        /// <returns></returns>
        public static string LoadStringFromDictionaryFile(string Key,string RelativePathToFile)
        {
            Dictionary<string, string> dictFile = LoadStringDictionaryFile(RelativePathToFile);
            if (dictFile.ContainsKey(Key))
            {
                return dictFile[Key];
            }
            return null;
        }

    }
}
