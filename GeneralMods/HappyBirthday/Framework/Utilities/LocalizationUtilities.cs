using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using static StardewValley.LocalizedContentManager;

namespace Omegasis.HappyBirthday.Framework.Utilities
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
            if (languageCode == LanguageCode.en)
                return GetEnglishLanguageCode();
            return languageCode switch
            {
                LanguageCode.ja => "ja-JP",
                LanguageCode.ru => "ru-RU",
                LanguageCode.zh => "zh-CN",
                LanguageCode.pt => "pt-BR",
                LanguageCode.es => "es-ES",
                LanguageCode.de => "de-DE",
                LanguageCode.th => "th-TH",
                LanguageCode.fr => "fr-FR",
                LanguageCode.ko => "ko-KR",
                LanguageCode.it => "it-IT",
                LanguageCode.tr => "tr-TR",
                LanguageCode.hu => "hu-HU",
                LanguageCode.mod => (LocalizedContentManager.CurrentModLanguage ?? throw new InvalidOperationException("The game language is set to a custom one, but the language info is no longer available.")).LanguageCode,
                _ => "",
            };
        }
        /// <summary>
        /// Gets the language code string for the current <see cref="LanguageCode"/> the player is using. 
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static string GetCurrentLanguageCodeString()
        {
            return GetLanguageCodeString(Game1.content.GetCurrentLanguage());
        }

        public static string GetEnglishLanguageCode()
        {
            return "en-US";
        }
    }
}
