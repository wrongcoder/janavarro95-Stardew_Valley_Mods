using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using static StardewValley.LocalizedContentManager;

namespace Omegasis.HappyBirthday.Framework.Localization
{
    /// <summary>
    /// Utilities for dealing with localizaions for content packs.
    /// </summary>
    public class LocalizationUtilities
    {
        /// <summary>
        /// Gets a string representing a given language code.
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static string GetLanguageCodeString(LanguageCode languageCode)
        {
            if(languageCode== LanguageCode.en)
            {
                return GetEnglishLanguageCode();
            }
            return Game1.content.LanguageCodeString(languageCode);
        }
        /// <summary>
        /// Gets the language code string for the current <see cref="LanguageCode"/> the player is using. 
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static string GetCurrentLanguageCodeString()
        {
            return GetLanguageCodeString(LocalizedContentManager.CurrentLanguageCode);
        }

        public static string GetEnglishLanguageCode()
        {
            return "en-US";
        }
    }
}
