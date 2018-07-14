using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocalization
{
    /// <summary>
    /// The configuration file for the mod.
    /// </summary>
    public class ModConfig
    {
        /// <summary>
        /// A list of all of the translations currently supported by this mod.
        /// </summary>
        public List<string> translations;
        /// <summary>
        /// The currently selected translation to use.
        /// </summary>
        public string currentTranslation;

        public ModConfig()
        {
            translations = new List<string>();
            translations.Add("English");
            currentTranslation = "English";
        }


    }
}
