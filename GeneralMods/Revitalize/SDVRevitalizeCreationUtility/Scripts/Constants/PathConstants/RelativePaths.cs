using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDVRevitalizeCreationUtility.Scripts.Constants.PathConstants
{
    public static class RelativePaths
    {
        public static string ModAssetsFolder = "ModAssets";
        public static string TemplatesFoldersName = "_Templates";


        public static string Graphics_Folder = Path.Combine(RelativePaths.ModAssetsFolder, "Graphics");

        public static string Content_Strings_Folder = Path.Combine(ModAssetsFolder, "Strings");

        public static string Content_Data_Folder = Path.Combine(ModAssetsFolder, "Data");
    }
}
