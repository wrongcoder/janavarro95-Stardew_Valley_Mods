using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Constants.PathConstants.Data
{
    class ItemsDataPaths
    {
        public static string ItemsDataPath = Path.Combine(RelativePaths.Content_Data_Folder, "Items");

        public static string ItemsDataTemplatesPath = Path.Combine(ItemsDataPath, "Templates");
    }
}
