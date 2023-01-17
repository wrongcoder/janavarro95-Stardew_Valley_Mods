using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants.PathConstants.Data;
using Omegasis.Revitalize.Framework.ContentPacks;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.World.Objects.Machines.Misc;
using StardewModdingAPI.Utilities;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Items
{
    public class ContentPackProcessingRecipeManager : ProcessingRecipeManager
    {
        public RevitalizeContentPack contentPack;

        public ContentPackProcessingRecipeManager()
        {


        }

        public ContentPackProcessingRecipeManager(RevitalizeContentPack contentPack)
        {
            this.contentPack = contentPack;
        }

        public override void loadRecipes()
        {
            //Load in general cases recipes.
            List<Dictionary<string, List<ProcessingRecipe<LootTableEntry>>>> processingRecipes = this.loadProcessingRecipesFromJsonFiles();
            foreach (Dictionary<string, List<ProcessingRecipe<LootTableEntry>>> objectIdToProcessingRecipesDict in processingRecipes)
            {

                foreach (KeyValuePair<string, List<ProcessingRecipe<LootTableEntry>>> entry in objectIdToProcessingRecipesDict)
                {
                    foreach (ProcessingRecipe<LootTableEntry> recipe in entry.Value)
                    {
                        this.addProcessingRecipe(entry.Key, recipe);
                        RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.addProcessingRecipe(entry.Key, recipe);
                    }
                }
            }
        }

        public override List<ProcessingRecipe<LootTableEntry>> getProcessingRecipesForObject(string Id)
        {
            if (this.processingRecipes.ContainsKey(Id))
            {
                return this.processingRecipes[Id];
            }
            else
            {
                return null;
            }
        }

        protected override List<Dictionary<string, List<ProcessingRecipe<LootTableEntry>>>> loadProcessingRecipesFromJsonFiles()
        {
            if(!Directory.Exists(Path.Combine(this.contentPack.baseContentPack.DirectoryPath, ObjectsDataPaths.ProcessingRecipesPath)))
            {
                Directory.CreateDirectory(Path.Combine(this.contentPack.baseContentPack.DirectoryPath, ObjectsDataPaths.ProcessingRecipesPath));
            }

            return JsonUtilities.LoadJsonFilesFromDirectories<Dictionary<string, List<ProcessingRecipe<LootTableEntry>>>>(this.contentPack, ObjectsDataPaths.ProcessingRecipesPath);
        }

    }
}
