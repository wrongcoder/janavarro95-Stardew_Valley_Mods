using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Constants.PathConstants.Data
{
    /// <summary>
    /// Relative path references to objects data information for the mod's assets.
    /// </summary>
    public class ObjectsDataPaths
    {
        public static string ObjectsDataPath = Path.Combine(RelativePaths.Content_Data_Folder, "Objects");


        public static string CraftingBlueprintsPath = Path.Combine(ObjectsDataPath, "CraftingBlueprints");

        public static string ObjectsDataTemplatesPath = Path.Combine(ObjectsDataPath, RelativePaths.TemplatesFoldersName);
        public static string ObjectsDataDumpPath = Path.Combine(ObjectsDataPath, "DataDump");

    }
}
