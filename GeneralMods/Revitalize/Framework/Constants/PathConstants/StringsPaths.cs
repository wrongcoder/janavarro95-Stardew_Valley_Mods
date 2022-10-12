using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Constants.PathConstants
{
    public class StringsPaths
    {
        /// <summary>
        /// Path constant to the error strings directory for the mod.
        /// </summary>
        public static string ErrorStrings = Path.Combine(RelativePaths.Content_Strings_Folder, "ErrorStrings");

        public static string Mail = Path.Combine(RelativePaths.Content_Strings_Folder, "Mail");
        public static string Objects = Path.Combine(RelativePaths.Content_Strings_Folder, "Objects");
        public static string ShopDialogue = Path.Combine(RelativePaths.Content_Strings_Folder, "ShopDialogue");
        

        public static string DisplayStrings = Path.Combine(Objects, "DisplayStrings");


    }
}
