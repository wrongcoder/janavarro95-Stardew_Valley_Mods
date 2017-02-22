using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using StardewModdingAPI;
namespace Stardew_Omegasis_Utilities.Mod
{
    class Error_Handling
    {

        public static void Log_Error(List<string> error_list, Exception ex)
        {
            if (!Directory.Exists(Save_Anywhere_V2.Mod_Core.Error_Path))
            {
                Directory.CreateDirectory(Save_Anywhere_V2.Mod_Core.Error_Path);
            }
            string path = string.Format("{0}-{1}Error_Log-{2:yyyy-MM-dd_hh-mm-ss-tt}.txt", Save_Anywhere_V2.Mod_Core.Error_Path, Path.DirectorySeparatorChar, DateTime.Now);
            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (var str in error_list)
                    {
                        sw.WriteLine(str);
                    }
                    Log.AsyncM("Save Anywhere V2: Error Occured. Please refer to the error log for more details.");
                    sw.WriteLine(ex);
                    Log.AsyncM(ex);
                }
            }

        }
    }
}
