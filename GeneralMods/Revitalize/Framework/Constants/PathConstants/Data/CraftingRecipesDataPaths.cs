using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Constants.PathConstants.Data
{
    public class CraftingRecipesDataPaths
    {
        public static string CraftingRecipesPath = Path.Combine(RelativePaths.Content_Data_Folder, "Crafting");
        public static string CraftingRecipesTemplatesPath = Path.Combine(CraftingRecipesPath, "Templates");

    }
}
