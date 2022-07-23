using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Omegasis.Revitalize.Framework.SaveData.ShopConditionsSaveData
{
    public class AnimalShopSaveData : SaveDataBase
    {
        [JsonProperty]
        public bool hasBuiltTier2BarnOrCoop;

        public AnimalShopSaveData()
        {

        }

        public override void save()
        {
            RevitalizeModCore.ModHelper.Data.WriteJsonFile(Path.Combine(RevitalizeModCore.SaveDataManager.getRelativeSaveDataPath(), "ShopConditionsSaveData", "AnimalShopSaveData.json"), this);
        }

        public virtual void setHasBuiltTier2OrHigherBarnOrCoop()
        {
            this.hasBuiltTier2BarnOrCoop = true;
        }

        public virtual bool getHasBuiltTier2OrHigherBarnOrCoop()
        {
            return this.hasBuiltTier2BarnOrCoop;
        }

        public static AnimalShopSaveData LoadOrCreate()
        {
            if (File.Exists(Path.Combine(RevitalizeModCore.SaveDataManager.getFullSaveDataPath(), "ShopConditionsSaveData", "AnimalShopSaveData.json")))
                return RevitalizeModCore.ModHelper.Data.ReadJsonFile<AnimalShopSaveData>(Path.Combine(RevitalizeModCore.SaveDataManager.getRelativeSaveDataPath(), "ShopConditionsSaveData", "AnimalShopSaveData.json"));
            else
            {
                AnimalShopSaveData Config = new AnimalShopSaveData();
                RevitalizeModCore.ModHelper.Data.WriteJsonFile(Path.Combine(RevitalizeModCore.SaveDataManager.getRelativeSaveDataPath(), "ShopConditionsSaveData", "AnimalShopSaveData.json"), Config);
                return Config;
            }
        }

    }
}
