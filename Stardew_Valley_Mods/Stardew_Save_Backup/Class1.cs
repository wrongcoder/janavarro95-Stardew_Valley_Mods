using System;
using System.IO;
using System.IO.Compression;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;

namespace Stardew_Save_Backup
{
    public class Class1 : Mod
    {
        DateTime localDate = DateTime.Now;
        public static string output;
        static int i = 0;
        static string istring = i.ToString();

        public override void Entry(params object[] objects)
        {
            Save_Backup(); //Yup I wrote it all in one function.
        }

        public void Save_Backup()
        {
            //insert data read and data writer functions if I want better future optimization.

            //Basically a whole, bunch of paths that make magic happen. 
            string app_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string stardew_path = Path.Combine(app_path, "StardewValley");

            string stardew_save_path = Path.Combine(stardew_path, "Saves");

            string saved_backups = Path.Combine(stardew_path, "Backed_Up_Saves"); //name of exported file


            if (!Directory.Exists(saved_backups))
            {
                Directory.CreateDirectory(saved_backups);
                Console.WriteLine("Making Backup Directory");
            }

            string backup_path = Path.Combine(stardew_path, "Backed_Up_Saves");

            string mydatastring = "SaveBackup";

            string back_up_savefile = Path.Combine(backup_path, mydatastring);

            while (true)
            {

                i++; //initial iterations
                istring = i.ToString(); //string conversion

                if (File.Exists(back_up_savefile + istring +".zip")) continue; //if my file exists, go back to the top!

                if (!File.Exists(back_up_savefile + istring + ".zip")) //if my file doesnt exist, make it!
                {
                    istring = i.ToString();

                    string newbackup = back_up_savefile + istring;
                    output = newbackup + ".zip";

                    ZipFile.CreateFromDirectory(stardew_save_path, output);
                    break;

                }
            }
        }
    }
}
