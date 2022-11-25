using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace SdvRevitalizeCreationUtility.Scripts
{
    public static class TemplateTransformerScript
    {

        public static void WriteDisplayStringsFile(string OutputPath, string ObjectId ,string DisplayName, string Description, string Category)
        {
            string file = System.IO.File.ReadAllText(System.IO.Path.Combine(@Game.Self.getGameDirectory(), "Templates", "DisplayStringTemplate.json"));
            file = Format(file, ObjectId, DisplayName, Description, Category);
            System.IO.File.WriteAllText(OutputPath, file);
        }

        public static void WriteCraftingBlueprintFile(string OutputPath, string ObjectId, string RecipesToLearn, string ItemToDraw)
        {
            string file = System.IO.File.ReadAllText(System.IO.Path.Combine(Game.Self.getGameDirectory(), "Templates", "CraftingBlueprintTemplate.json"));
            file = Format(file, ItemToDraw, RecipesToLearn, ObjectId);
            System.IO.File.WriteAllText(OutputPath, file);
        }

        /// <summary>
        /// Formats a given string read in from a template file, since for some reason string.format() always seems to crash when formatting a string read from a file.
        /// </summary>
        /// <param name="Original"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        public static string Format(string Original, params string[] Params)
        {
            string template = "";
            int counter = 0;
            string formatted = Original;

            foreach(string replacement in Params)
            {
                template = "{" + counter.ToString() + "}";
                formatted = formatted.Replace(template, replacement);
                counter++;
            }
            return formatted;
        }
    }
}
