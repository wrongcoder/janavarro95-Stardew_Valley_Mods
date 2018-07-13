using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocalization
{
    public class ModConfig
    {
        public List<string> translations;
        public string currentTranslation;

        public ModConfig()
        {
            translations = new List<string>();
            translations.Add("English");
            currentTranslation = "English";
        }


    }
}
