using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Content.JsonContent.Objects;

namespace Omegasis.Revitalize.Framework.Utilities.JsonContentLoading
{
    public static class JsonContentLoaderUtilities
    {
        /// <summary>
        /// Loads a description from the Descriptions.json file as part of the mod's assets.
        /// </summary>
        /// <param name="Id">The id for </param>
        /// <returns></returns>
        public static string LoadItemDescription(string Id, bool ThrowExceptionIfNotFound = true)
        {
            if (!RevitalizeModCore.ModContentManager.objectManager.displayStrings.ContainsKey(Id) && ThrowExceptionIfNotFound)
            {
                throw new JsonContentLoadingException("The given item id {0} does not have a registered value for display strings! A file can be created under the ModAssets/Strings/Objects/DisplayStrings directory with the given info.");
            }
            return RevitalizeModCore.ModContentManager.objectManager.displayStrings[Id].description;
        }

        /// <summary>
        /// Loads a description from the Descriptions.json file as part of the mod's assets.
        /// </summary>
        /// <param name="Id">The id for </param>
        /// <returns></returns>
        public static string LoadItemDisplayName(string Id, bool ThrowExceptionIfNotFound = true)
        {
            if (!RevitalizeModCore.ModContentManager.objectManager.displayStrings.ContainsKey(Id) && ThrowExceptionIfNotFound)
            {
                throw new JsonContentLoadingException("The given item id {0} does not have a registered value for display strings! A file can be created under the ModAssets/Strings/Objects/DisplayStrings directory with the given info.");
            }
            return RevitalizeModCore.ModContentManager.objectManager.displayStrings[Id].displayName;
        }

        /// <summary>
        /// Loads in a specified error string from a given file.
        /// </summary>
        /// <param name="RelativePathToFile"></param>
        /// <param name="Key"></param>
        /// <param name="StringParamaters"></param>
        /// <returns></returns>
        public static string LoadErrorString(string RelativePathToFile, string Key, params object[] StringParamaters)
        {
            return string.Format(JsonUtilities.LoadStringFromDictionaryFile(Key, Path.Combine(Constants.PathConstants.StringsPaths.ErrorStrings, RelativePathToFile)), StringParamaters);
        }
    }
}
