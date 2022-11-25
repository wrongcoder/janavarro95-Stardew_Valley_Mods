using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDVRevitalizeCreationUtility.Scripts.Constants.PathConstants.Data
{
    public class CraftingDataPaths
    {
        public static string CraftingPath = Path.Combine(RelativePaths.Content_Data_Folder, "Crafting");
        public static string CraftingStationsPath = Path.Combine(CraftingPath, "CraftingStations");
        public static string CraftingStationTemplatesPath = Path.Combine(CraftingStationsPath, RelativePaths.TemplatesFoldersName);

        public static string RevitalizeMachinesPath = Path.Combine(CraftingPath, "RevitalizeMachines");
        public static string StardewValleyMachinesPath = Path.Combine(CraftingPath, "StardewValleyMachinesMachines");

    }
}
