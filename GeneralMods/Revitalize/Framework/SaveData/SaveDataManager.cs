using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Player;
using StardewValley;

namespace Revitalize.Framework.SaveData
{
    /// <summary>
    /// Save data manager for managing custom save data for the Revitalize mod.
    /// </summary>
    public class SaveDataManager
    {
        /// <summary>
        /// Save data persisting to shop conditions.
        /// </summary>
        public ShopConditionsSaveData.ShopSaveData shopSaveData;

        public SaveDataManager()
        {
        }

        public virtual void loadOrCreateSaveData()
        {
            this.shopSaveData = new ShopConditionsSaveData.ShopSaveData();
        }

        /// <summary>
        /// Gets the save data path for the mod save data and creates the necessary directory if it doesn't exist.
        /// </summary>
        /// <returns></returns>
        public virtual string getFullSaveDataPath()
        {

            string save_directory = Path.Combine(ModCore.ModHelper.DirectoryPath, this.getRelativeSaveDataPath());
            Directory.CreateDirectory(save_directory);
            return save_directory;
        }

        public virtual string getRelativeSaveDataPath()
        {
            string friendlyName = SaveGame.FilterFileName(Game1.GetSaveGameName());
            string filenameNoTmpString = friendlyName + "_" + Game1.uniqueIDForThisGame;
            filenameNoTmpString += Game1.player.name + "_" + Game1.player.uniqueMultiplayerID.Value;

            return Path.Combine("SaveData", filenameNoTmpString + Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Saves all of the necessary save data for the mod.
        /// </summary>
        public virtual void save()
        {
            if (this.shopSaveData.getShouldSaveData())
            {
                this.shopSaveData.save();
            }

        }
    }
}
