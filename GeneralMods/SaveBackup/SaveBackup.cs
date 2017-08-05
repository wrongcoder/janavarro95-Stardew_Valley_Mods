using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace Omegasis.SaveBackup
{
    /// <summary>The mod entry point.</summary>
    public class SaveBackup : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The folder path containing the game's app data.</summary>
        private static readonly string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley");

        /// <summary>The folder path containing the game's saves.</summary>
        private static readonly string SavesPath = Path.Combine(SaveBackup.AppDataPath, "Saves");

        /// <summary>The folder path containing backups of the save before the player starts playing.</summary>
        private static readonly string PrePlayBackupsPath = Path.Combine(SaveBackup.AppDataPath, "Backed_Up_Saves", "Pre_Play_Saves");

        /// <summary>The folder path containing nightly backups of the save.</summary>
        private static readonly string NightlyBackupsPath = Path.Combine(SaveBackup.AppDataPath, "Backed_Up_Saves", "Nightly_InGame_Saves");

        /// <summary>The number of save backups to keep for each type.</summary>
        private int SaveCount = 30;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.LoadConfig();
            this.WriteConfig();

            this.BackupSaves(SaveBackup.PrePlayBackupsPath);

            SaveEvents.BeforeSave += this.SaveEvents_BeforeSave;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked before the save is updated.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void SaveEvents_BeforeSave(object sender, EventArgs e)
        {
            this.BackupSaves(SaveBackup.NightlyBackupsPath);
        }

        /// <summary>Back up saves to the specified folder.</summary>
        /// <param name="folderPath">The folder path in which to generate saves.</param>
        private void BackupSaves(string folderPath)
        {
            // back up saves
            Directory.CreateDirectory(folderPath);
            ZipFile.CreateFromDirectory(SaveBackup.SavesPath, Path.Combine(folderPath, $"backup-{DateTime.Now:yyyyMMdd'-'HHmmss}.zip"));

            // delete old backups
            new DirectoryInfo(folderPath)
                .EnumerateFiles()
                .OrderByDescending(f => f.CreationTime)
                .Skip(this.SaveCount)
                .ToList()
                .ForEach(file => file.Delete());
        }

        /// <summary>Load the configuration settings.</summary>
        private void LoadConfig()
        {
            var path = Path.Combine(Helper.DirectoryPath, "AutoBackup_data.txt");
            if (File.Exists(path))
            {
                string[] text = File.ReadAllLines(path);
                this.SaveCount = Convert.ToInt32(text[3]);
            }
        }

        /// <summary>Save the configuration settings.</summary>
        private void WriteConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, "AutoBackup_data.txt");
            string[] text = new string[20];
            text[0] = "Player: AutoBackup Config:";
            text[1] = "====================================================================================";
            text[2] = "Number of Backups to Keep:";
            text[3] = this.SaveCount.ToString();

            File.WriteAllLines(path, text);
        }
    }
}
