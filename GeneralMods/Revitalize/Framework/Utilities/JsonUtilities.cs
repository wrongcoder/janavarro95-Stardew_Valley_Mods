using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Crafting.JsonContent;
using Omegasis.Revitalize.Framework.Utilities.JsonContentLoading;

namespace Omegasis.Revitalize.Framework.Utilities
{
    /// <summary>
    /// Useful utilities for handling json files.
    /// </summary>
    public class JsonUtilities
    {
        /// <summary>
        /// Writes json files to disk.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="RelativePathToSaveTo"></param>
        public static void WriteJsonFile(object obj, params string[] RelativePathToSaveTo)
        {
            RevitalizeModCore.ModHelper.Data.WriteJsonFile(Path.Combine(RelativePathToSaveTo), obj);
        }

        /// <summary>
        /// Reads json files from disk.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RelativePathToReadFrom"></param>
        public static T ReadJsonFile<T>(params string[] RelativePathToReadFrom) where T : class
        {
            return RevitalizeModCore.ModHelper.Data.ReadJsonFile<T>(Path.Combine(RelativePathToReadFrom));
        }

        public static T ReadJsonFilePathCombined<T>(string RelativePathToReadFrom) where T : class
        {
            return ReadJsonFile<T>(RelativePathToReadFrom);
        }

        /// <summary>
        /// Recursively searches all directories and sub directories under the relative path passed in, and performs the action when found.
        /// </summary>
        /// <param name="onFileFound"></param>
        /// <param name="relativePath"></param>
        public static void RecursiveSearchDirectoriesForJsonFiles(Action<string> onFileFound, params string[] relativePath)
        {
            string absPath = Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, Path.Combine(relativePath));
            foreach (string folder in Directory.GetDirectories(absPath))
            {
                RecursiveSearchDirectoriesForJsonFiles(onFileFound, Path.Combine(absPath, Path.GetFileName(folder)));
            }
            foreach (string file in Directory.GetFiles(absPath, "*.json"))
            {
                string fileRelativePath = Path.Combine(absPath, Path.GetFileName(file));
                onFileFound.Invoke(fileRelativePath);
            }
        }

        /// <summary>
        /// Recursively searches all directories and sub directories under the relative path passed in, and loads them from disk.
        /// </summary>
        /// <param name="onFileFound">The action that occurs when the json file is loaded. Note that this is not <see cref="ReadJsonFile{T}(string[])"/> incase there is post processing that should be done first.</param>
        /// <param name="relativePath"></param>
        /// <returns>A list of all json files that were loaded.</returns>
        public static List<T> LoadJsonFilesFromDirectories<T>(Func<string, T> onFileFound, params string[] relativePath) where T : class
        {
            string relativePathString = Path.Combine(relativePath);
            string absPath = Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, relativePathString);
            List<T> returnedJsonFiles = new List<T>();
            foreach (string folder in Directory.GetDirectories(absPath))
            {
                string folderRelativePath = Path.Combine(relativePathString, Path.GetFileName(folder));
                returnedJsonFiles.AddRange(LoadJsonFilesFromDirectories(onFileFound, folderRelativePath));
            }
            foreach (string file in Directory.GetFiles(absPath, "*.json"))
            {
                string fileRelativePath = Path.Combine(relativePathString, Path.GetFileName(file));
                returnedJsonFiles.Add(onFileFound.Invoke(fileRelativePath));
            }
            return returnedJsonFiles;
        }

        /// <summary>
        /// Recursively searches all directories and sub directories under the relative path passed in, and loads them from disk. <see cref="LoadJsonFilesFromDirectories{T}(Func{string, T}, string[])"/> if postprocessing is desired.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns>A list of all json files that were loaded.</returns>
        public static List<T> LoadJsonFilesFromDirectories<T>(params string[] relativePath) where T : class
        {
            return LoadJsonFilesFromDirectories<T>(new Func<string, T>(ReadJsonFilePathCombined<T>), relativePath);
        }

        /// <summary>
        /// Loads a Dictionary file where the keys are string values, and the values are the given Value type.
        /// </summary>
        /// <typeparam name="Value">The type of content stored as the values in the dictionary file.</typeparam>
        /// <param name="relativePath">The relative path to the file.</param>
        /// <returns></returns>
        public static Dictionary<string,Value> LoadDictionaryFile<Value>(params string[] relativePath)
        {
            Dictionary<string,Value> dict= ReadJsonFile<Dictionary<string, Value>>(relativePath);
            if (dict == null)
            {
                throw new JsonContentLoadingException(string.Format("Error: The given dictionary file located at {0} is null", Path.Combine(relativePath)));
            }
            return dict;
        }

        /// <summary>
        /// Loads a Value type object from a json dictionary file.
        /// </summary>
        /// <typeparam name="Value">The type of object to load from the json string dictionary.</typeparam>
        /// <param name="Key">The Key in which the value is indexed.</param>
        /// <param name="RelativePathToFile">The relative path to the json file.</param>
        /// <returns></returns>
        public static Value LoadValueFromDictionaryFile<Value>(string Key, string RelativePathToFile)
        {
            Dictionary<string, Value> dict = LoadDictionaryFile<Value>(RelativePathToFile);
            if (dict.ContainsKey(Key))
            {
                return dict[Key];
            }
            throw new JsonContentLoadingException(string.Format("Error: The given key {0} can not be found in the dictionary file located at {1}", Key, RelativePathToFile));
            //return default(Value);
        }


        /// <summary>
        /// Loads a string dictionary from a path relative to the Mod folder's location.
        /// </summary>
        /// <param name="RelativePathToFile"></param>
        /// <returns></returns>
        public static Dictionary<string, string> LoadStringDictionaryFile(string RelativePathToFile)
        {
            return ReadJsonFilePathCombined<Dictionary<string, string>>(RelativePathToFile);
        }

        /// <summary>
        /// Loads a string from a string dictionary.
        /// </summary>
        /// <param name="Key">The key in the json file to load from.</param>
        /// <param name="RelativePathToFile">The relative path to the dictionary file from the mods' content folder.</param>
        /// <returns></returns>
        public static string LoadStringFromDictionaryFile(string Key, string RelativePathToFile)
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
