using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.SaveData.ShopConditionsSaveData
{
    /// <summary>
    /// Unified class for storing all of the necessary save data that needs to be persisted for shops.
    /// </summary>
    public class ShopSaveDataManager : SaveDataInfo
    {
        /// <summary>
        /// Deals with necessary save data regarding the animal shop.
        /// </summary>
        public AnimalShopSaveData animalShopSaveData;

        /// <summary>
        /// Deals with necessary save data related to the carpenter's shop (aka Robin's shop).
        /// </summary>
        public CarpenterShopSaveData carpenterShopSaveData;

        public ShopSaveDataManager()
        {
            this.animalShopSaveData = this.loadOrCreate<AnimalShopSaveData>(AnimalShopSaveData.SaveFileName);
            this.carpenterShopSaveData = this.loadOrCreate<CarpenterShopSaveData>(CarpenterShopSaveData.SaveFileName);
        }

        public override void save()
        {
            this.animalShopSaveData.save();
            this.carpenterShopSaveData.save();
        }

        public override void load()
        {
            this.animalShopSaveData.load();
            this.carpenterShopSaveData.load();
        }


        public virtual string getRelativeSavePath()
        {
            return Path.Combine(RevitalizeModCore.SaveDataManager.getRelativeSaveDataPath(), "ShopConditionsSaveData");
        }

        public virtual string getFullSavePath()
        {
            return Path.Combine(RevitalizeModCore.SaveDataManager.getFullSaveDataPath(), "ShopConditionsSaveData");
        }

        /// <summary>
        /// Loads or creates a given config file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="SaveFileName"></param>
        /// <returns></returns>
        public T loadOrCreate<T>(string SaveFileName) where T: ShopSaveDataInfo, new()
        {
            if (File.Exists(Path.Combine(this.getFullSavePath(), SaveFileName)))
                return RevitalizeModCore.ModHelper.Data.ReadJsonFile<T>(Path.Combine(this.getRelativeSavePath(), SaveFileName));
            else
            {
                T Config = new T();
                Config.save();
                return Config;
            }
        }

    }
}
