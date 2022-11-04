using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.SaveData.Player
{
    public class PlayerSaveData:SaveDataInfo
    {
        public Dictionary<string, string> unlockedCraftingRecipes;

        /// <summary>
        /// Used to determine if the player has obtained the battery pack necessaery to unlock electric furnace recipes.
        /// </summary>
        public bool hasObtainedBatteryPack;

        public PlayerSaveData()
        {
            this.unlockedCraftingRecipes = new Dictionary<string, string>();
            this.hasObtainedBatteryPack = false;
        }

        public virtual void addUnlockedCraftingRecipe(string RecipeBookId, string RecipeId)
        {
            if (this.unlockedCraftingRecipes.ContainsKey(RecipeBookId)) return;
            this.unlockedCraftingRecipes.Add(RecipeBookId, RecipeId);
        }

        public override void save()
        {
            RevitalizeModCore.ModHelper.Data.WriteJsonFile(Path.Combine(RevitalizeModCore.SaveDataManager.getRelativeSaveDataPath(), "PlayerSaveData.json"), this);
        }

        public override void load()
        {
            foreach(KeyValuePair<string,string> recipe in this.unlockedCraftingRecipes)
            {
                RevitalizeModCore.ModContentManager.craftingManager.learnCraftingRecipe(recipe.Key, recipe.Value);
            }
        }

        public static PlayerSaveData LoadOrCreate()
        {
            if (File.Exists(Path.Combine(RevitalizeModCore.SaveDataManager.getFullSaveDataPath(), "PlayerSaveData.json")))
            {
                PlayerSaveData saveData = RevitalizeModCore.ModHelper.Data.ReadJsonFile<PlayerSaveData>(Path.Combine(RevitalizeModCore.SaveDataManager.getRelativeSaveDataPath(), "PlayerSaveData.json"));
                saveData.load();
                return saveData;
            }
            else
            {
                PlayerSaveData Config = new PlayerSaveData();
                RevitalizeModCore.ModHelper.Data.WriteJsonFile(Path.Combine(RevitalizeModCore.SaveDataManager.getRelativeSaveDataPath(), "PlayerSaveData.json"), Config);
                return Config;
            }
        }

    }
}
