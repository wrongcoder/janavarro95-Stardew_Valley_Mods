using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using StardewModdingAPI;

namespace Omegasis.SaveBackup
{
    public class Class1 : Mod
    {
      static string app_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      static string stardew_path = Path.Combine(app_path, "StardewValley");
      static string stardew_save_path = Path.Combine(stardew_path, "Saves");
      static string saved_backups = Path.Combine(stardew_path, "Backed_Up_Saves"); //name of exported file
      static string pre_play_saves = Path.Combine(saved_backups, "Pre_Play_Saves");
      static string sleeping_saves = Path.Combine(saved_backups, "Nightly_InGame_Saves");
      static string backup_path = Path.Combine(stardew_path, "Backed_Up_Saves");

      public static string output;
      static int pre_iterator = 0;
      static int sleep_iterator =0;
      static string pre_iterator_string = pre_iterator.ToString();
      static string sleep_iterator_string = sleep_iterator.ToString();

      static int save_num = 30;

        public override void Entry(IModHelper helper)
        {
            DataLoader();
            MyWritter();
           
            Save_Backup(); //Yup I wrote it all in one function.... kinda

            StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += Sleep_Save_Backup;
        }

        public void Save_Backup()
        {
            //insert data read and data writer functions if I want better future optimization.

            string mydatastring = "Pre_Play_Backup";
            string back_up_savefile = Path.Combine(pre_play_saves, mydatastring);
            back_up_savefile = Path.Combine(pre_play_saves, mydatastring);
            if (!Directory.Exists(saved_backups))
            {
                Directory.CreateDirectory(saved_backups);
                Console.WriteLine("Making Backup Directory");
            }

            if (!Directory.Exists(pre_play_saves))
            {
                Directory.CreateDirectory(pre_play_saves);
                Console.WriteLine("Making Backup Directory");
            }

            if (!Directory.Exists(sleeping_saves))
            {
                Directory.CreateDirectory(sleeping_saves);
                Console.WriteLine("Making Backup Directory");
            }


            while (true)
            {

                pre_iterator++; //initial iterations
                pre_iterator_string = pre_iterator.ToString(); //string conversion

                if (File.Exists(back_up_savefile + pre_iterator_string +".zip")) continue; //if my file exists, go back to the top!

                if (!File.Exists(back_up_savefile + pre_iterator_string + ".zip")) //if my file doesnt exist, make it!
                {
                    pre_iterator_string = pre_iterator.ToString();

                    string newbackup = back_up_savefile + pre_iterator_string;
                    output = newbackup + ".zip";

                    ZipFile.CreateFromDirectory(stardew_save_path, output);
                    break;

                }
            }

            var files = new DirectoryInfo(pre_play_saves).EnumerateFiles()
            .OrderByDescending(f => f.CreationTime)
      .Skip(save_num)
      .ToList();
            files.ForEach(f => f.Delete());

        }

        static void Sleep_Save_Backup(object sender, EventArgs e)
        {
            //insert data read and data writer functions if I want better future optimization.
           string mydatastring = "Nightly_Backup";
           string back_up_savefile = Path.Combine(pre_play_saves, mydatastring);
            back_up_savefile = Path.Combine(sleeping_saves, mydatastring);


            //This shouldn't run, but just incase...
            if (!Directory.Exists(saved_backups))
            {
                Directory.CreateDirectory(saved_backups);
                Console.WriteLine("Making Backup Directory");
            }

            if (!Directory.Exists(pre_play_saves))
            {
                Directory.CreateDirectory(pre_play_saves);
                Console.WriteLine("Making Backup Directory");
            }

            if (!Directory.Exists(sleeping_saves))
            {
                Directory.CreateDirectory(sleeping_saves);
                Console.WriteLine("Making Backup Directory");
            }

            //string backup_path = Path.Combine(stardew_path, "Backed_Up_Saves");

            while (true)
            {

                sleep_iterator++; //initial iterations
                sleep_iterator_string = sleep_iterator.ToString(); //string conversion

                if (File.Exists(back_up_savefile + sleep_iterator_string + ".zip")) continue; //if my file exists, go back to the top!

                if (!File.Exists(back_up_savefile + sleep_iterator_string + ".zip")) //if my file doesnt exist, make it!
                {
                    sleep_iterator_string = sleep_iterator.ToString();

                    string newbackup = back_up_savefile + sleep_iterator_string;
                    output = newbackup + ".zip";

                    ZipFile.CreateFromDirectory(stardew_save_path, output);
                    break;

                }
            }

            var files = new DirectoryInfo(sleeping_saves).EnumerateFiles()
            .OrderByDescending(f => f.CreationTime)
      .Skip(save_num)
      .ToList();
            files.ForEach(f => f.Delete());
        }


        void DataLoader()
        {
            //loads the data to the variables upon loading the game.
            var mylocation = Path.Combine(Helper.DirectoryPath, "AutoBackup_data.txt");
            if (!File.Exists(mylocation)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                Console.WriteLine("The config file for AutoSpeed was not found, guess I'll create it...");

            }
            else
            {
                //        Console.WriteLine("HEY THERE IM LOADING DATA");

                //loads the BuildEndurance_data upon loading the mod
                string[] readtext = File.ReadAllLines(mylocation);
                save_num = Convert.ToInt32(readtext[3]);

            }
        }

        void MyWritter()
        {
            //saves the BuildEndurance_data at the end of a new day;
            var mylocation = Path.Combine(Helper.DirectoryPath, "AutoBackup_data.txt");
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation))
            {
                Console.WriteLine("The data file for AutoBackup was not found, guess I'll create it when you sleep.");

                mystring3[0] = "Player: AutoBackup Config:";
                mystring3[1] = "====================================================================================";
                mystring3[2] = "Number of Backups to Keep:";
                mystring3[3] = save_num.ToString();
                File.WriteAllLines(mylocation, mystring3);
            }
            else
            {
                //    Console.WriteLine("HEY IM SAVING DATA");

                //write out the info to a text file upon loading
                mystring3[0] = "Player: AutoBackup Config:";
                mystring3[1] = "====================================================================================";

                mystring3[2] = "Number of Backups to Keep:";
                mystring3[3] = save_num.ToString();

                File.WriteAllLines(mylocation, mystring3);
            }
        }


    }



}
