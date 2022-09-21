using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.Revitalize.Framework.Utilities
{
    public class LocalizationUtilities
    {
        /// <summary>
        /// Gets a file path that includes translation codes if necessary. Used for loading files for different translations.
        /// </summary>
        /// <param name="RelativePathToFile"></param>
        /// <returns></returns>
        public static string GetLocalizationFilePath(string RelativePathToFile)
        {

            if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en) return RelativePathToFile;

            string languageCodeString= Game1.content.LanguageCodeString(LocalizedContentManager.CurrentLanguageCode);
            string fileExtention = Path.GetExtension(RelativePathToFile);
            if (fileExtention.StartsWith(".") == false)
            {
                fileExtention = "." + fileExtention;
            }
            string fileName = Path.GetFileNameWithoutExtension(RelativePathToFile);
            string directory = Directory.GetParent(RelativePathToFile).FullName;

            string combinedPath = Path.Combine(directory, fileName +"."+ languageCodeString + fileExtention);
            return combinedPath;
        }

    }
}
