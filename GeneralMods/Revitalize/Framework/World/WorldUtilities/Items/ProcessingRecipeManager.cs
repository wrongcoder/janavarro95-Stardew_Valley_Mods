using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants.PathConstants.Data;
using Omegasis.Revitalize.Framework.ContentPacks;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.World.Objects.Machines.Misc;
using StardewModdingAPI;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities.Items
{
    /// <summary>
    /// Manages all of the processing recipes for a object or a machine.
    ///
    /// When adding new specialized <see cref="ProcessingRecipe{T}"/>s with custom determing logic for outputs, you will also need to update the <see cref="ContentPackProcessingRecipeManager"/> class as well with the appropriate overrides to ensure proper file loading and prevent stack overflows.
    /// </summary>
    public class ProcessingRecipeManager
    {
        /// <summary>
        /// A list of processing recipes per object keyed by the object's id.
        /// </summary>
        public Dictionary<string, List<ProcessingRecipe<LootTableEntry>>> processingRecipes = new Dictionary<string, List<ProcessingRecipe<LootTableEntry>>>();
        /// <summary>
        /// A list of geode crusher recipes keyed in by object id.
        /// </summary>
        public Dictionary<string, List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>>> advancedGeodeCrusherRecipes = new Dictionary<string, List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>>>();

        public ProcessingRecipeManager() { }


        public virtual void addProcessingRecipe(string ObjectId,ProcessingRecipe<LootTableEntry> recipe)
        {
            if (this.processingRecipes.ContainsKey(ObjectId))
            {
                this.processingRecipes[ObjectId].Add(recipe);
            }
            else
            {
                this.processingRecipes.Add(ObjectId, new List<ProcessingRecipe<LootTableEntry>>() { recipe});
            }
        }

        public virtual void addAdvancedGeodeCrusherProcessingRecipe(string ObjectId, ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput> recipe)
        {
            if (this.advancedGeodeCrusherRecipes.ContainsKey(ObjectId))
            {
                this.advancedGeodeCrusherRecipes[ObjectId].Add(recipe);
            }
            else
            {
                this.advancedGeodeCrusherRecipes.Add(ObjectId, new List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>>() { recipe });
            }
        }

       /// <summary>
       /// Loads all recipes for the processing recipe manager.
       /// </summary>
        public virtual void loadRecipes()
        {
            //Load in general cases recipes.
            List<Dictionary<string, List<ProcessingRecipe<LootTableEntry>>>> processingRecipes = this.loadProcessingRecipesFromJsonFiles();
            foreach (Dictionary<string, List<ProcessingRecipe<LootTableEntry>>> objectIdToProcessingRecipesDict in processingRecipes)
            {

                foreach (KeyValuePair<string, List<ProcessingRecipe<LootTableEntry>>> entry in objectIdToProcessingRecipesDict)
                {
                    foreach(ProcessingRecipe<LootTableEntry> recipe in entry.Value)
                    {
                        this.addProcessingRecipe(entry.Key, recipe);
                    }
                }
            }

            //Add in special cases below.

            //Load in geode crusher recipes.
            List<Dictionary<string, List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>>>> geodeCrusherRecipes = this.loadAdvancedGeodeCrusherProcessingRecipesFromJsonFiles();
            foreach (Dictionary<string, List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>>> objectIdToProcessingRecipesDict in geodeCrusherRecipes)
            {
                foreach (KeyValuePair<string, List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>>> entry in objectIdToProcessingRecipesDict)
                {
                    foreach (ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput> recipe in entry.Value)
                    {
                        this.addAdvancedGeodeCrusherProcessingRecipe(entry.Key, recipe);
                    }
                }
            }
        }

        /// <summary>
        /// Loads in all processing files for the mod.
        /// </summary>
        /// <returns></returns>
        protected virtual List<Dictionary<string, List<ProcessingRecipe<LootTableEntry>>>> loadProcessingRecipesFromJsonFiles()
        {
            if (!Directory.Exists(ObjectsDataPaths.ProcessingRecipesGeneralCasesPath))
            {
                Directory.CreateDirectory(ObjectsDataPaths.ProcessingRecipesGeneralCasesPath);
            }
            return JsonUtilities.LoadJsonFilesFromDirectories<Dictionary<string, List<ProcessingRecipe<LootTableEntry>>>>(ObjectsDataPaths.ProcessingRecipesGeneralCasesPath);
        }

        /// <summary>
        /// Loads in all advanced geode crusher processing files for the mod.
        /// </summary>
        /// <returns></returns>
        protected virtual List<Dictionary<string, List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>>>> loadAdvancedGeodeCrusherProcessingRecipesFromJsonFiles()
        {
            if (!Directory.Exists(ObjectsDataPaths.ProcessingRecipesSpecialCases_AdvancedGeodeCrushers))
            {
                Directory.CreateDirectory(ObjectsDataPaths.ProcessingRecipesSpecialCases_AdvancedGeodeCrushers);
            }
            return JsonUtilities.LoadJsonFilesFromDirectories<Dictionary<string, List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>>>>(ObjectsDataPaths.ProcessingRecipesSpecialCases_AdvancedGeodeCrushers);
        }

        /// <summary>
        /// Gets the processing recipes for a given id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual List<ProcessingRecipe<LootTableEntry>> getProcessingRecipesForObject(string Id)
        {
            List<ProcessingRecipe<LootTableEntry>> processingRecipesForObject = new List<ProcessingRecipe<LootTableEntry>>();
            if (this.processingRecipes.ContainsKey(Id))
            {
                processingRecipesForObject.AddRange(this.processingRecipes[Id]);
            }

            foreach(RevitalizeContentPack contentPack in RevitalizeModCore.ModContentManager.revitalizeContentPackManager.getAllContentPacks())
            {
                if (contentPack.objectProcessingRecipeManager != null)
                {
                    List<ProcessingRecipe<LootTableEntry>> processingRecipesFromContentPack = contentPack.objectProcessingRecipeManager.getProcessingRecipesForObject(Id);
                    if (processingRecipesFromContentPack != null)
                    {
                        processingRecipesForObject.AddRange(processingRecipesFromContentPack);
                    }
                }
            }

            return processingRecipesForObject;
        }

        /// <summary>
        /// Gets the processing recipes for geode crushers for a given object id.
        /// </summary>
        /// <param name="GeodeCrusherId"></param>
        /// <returns></returns>
        public virtual List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>> getAdvancedGeodeCrusherProcessingRecipes(string GeodeCrusherId)
        {
            List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>> processingRecipesForObject = new List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>>();
            if (this.advancedGeodeCrusherRecipes.ContainsKey(GeodeCrusherId))
            {
                processingRecipesForObject.AddRange(this.advancedGeodeCrusherRecipes[GeodeCrusherId]);
            }

            foreach (RevitalizeContentPack contentPack in RevitalizeModCore.ModContentManager.revitalizeContentPackManager.getAllContentPacks())
            {
                if (contentPack.objectProcessingRecipeManager != null)
                {
                    List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>> processingRecipesFromContentPack = contentPack.objectProcessingRecipeManager.getAdvancedGeodeCrusherProcessingRecipes(GeodeCrusherId);
                    if (processingRecipesFromContentPack != null)
                    {
                        processingRecipesForObject.AddRange(processingRecipesFromContentPack);
                    }
                }
            }

            return processingRecipesForObject;
        }
    }
}
