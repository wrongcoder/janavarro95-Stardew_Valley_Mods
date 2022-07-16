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

        public static string ObjectsDataTemplatesPath = Path.Combine(ObjectsDataPath, "Templates");

    }
}
