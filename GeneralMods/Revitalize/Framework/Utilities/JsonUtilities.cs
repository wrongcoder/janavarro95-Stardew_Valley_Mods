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
        public static Dictionary<string,string> loadStringDictionaryFile(string RelativePathToFile)
        {
            return ModCore.ModHelper.Content.Load<Dictionary<string, string>>(RelativePathToFile);
        }

    }
}
