using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Crafting.JsonContent;

namespace Omegasis.Revitalize.Framework.Utilities
{
    /// <summary>
    /// Useful utilities for handling json files.
    /// </summary>
    public class JsonUtilities
    {
        public static void saveToRevitaliveModContentFolder(object objectToSave, string RelativePathToSaveTo, string FileName)
        {
            RevitalizeModCore.Serializer.SerializeContentFile(FileName,objectToSave,RelativePathToSaveTo);
        }


        /// <summary>
        /// Loads a string dictionary from a path relative to the Mod folder's location.
        /// </summary>
        /// <param name="RelativePathToFile"></param>
        /// <returns></returns>
        public static Dictionary<string,string> LoadStringDictionaryFile(string RelativePathToFile)
        {
            return RevitalizeModCore.ModHelper.Content.Load<Dictionary<string, string>>(RelativePathToFile);
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

        /// <summary>
        /// Loads a content file containing json information from disk.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RelativePathToFile"></param>
        /// <returns></returns>
        public static T LoadContentFile<T>(string RelativePathToFile)
        {
            return RevitalizeModCore.ModHelper.Content.Load<T>(RelativePathToFile);
        }

        /// <summary>
        /// Loads a json crafting recipe from disk.
        /// </summary>
        /// <param name="RelativePathToFile"></param>
        /// <returns></returns>
        public static JsonCraftingRecipe LoadCraftingRecipe(string RelativePathToFile)
        {
            return RevitalizeModCore.ModHelper.Content.Load<JsonCraftingRecipe>(RelativePathToFile);
        }

    }
}
