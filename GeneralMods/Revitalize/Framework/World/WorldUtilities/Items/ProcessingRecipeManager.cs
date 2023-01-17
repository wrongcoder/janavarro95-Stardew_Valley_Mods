using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants.PathConstants.Data;
using Omegasis.Revitalize.Framework.ContentPacks;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.Crafting.JsonContent;
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

        public ProcessingRecipeManager() { }


        public virtual void addProcessingRecipe(string ObjectId, ProcessingRecipe<LootTableEntry> recipe)
        {
            if (this.processingRecipes.ContainsKey(ObjectId))
            {
                this.processingRecipes[ObjectId].Add(recipe);
            }
            else
            {
                this.processingRecipes.Add(ObjectId, new List<ProcessingRecipe<LootTableEntry>>() { recipe });
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
                    foreach (ProcessingRecipe<LootTableEntry> recipe in entry.Value)
                    {
                        this.addProcessingRecipe(entry.Key, recipe);
                    }
                }
            }

            //Add in special cases below.

            this.convertJsonCraftingRecipeBookToProcessingRecipeBook();
        }

        /// <summary>
        /// Loads in all processing files for the mod.
        /// </summary>
        /// <returns></returns>
        protected virtual List<Dictionary<string, List<ProcessingRecipe<LootTableEntry>>>> loadProcessingRecipesFromJsonFiles()
        {
            if (!Directory.Exists(Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, ObjectsDataPaths.ProcessingRecipesPath)))
            {
                Directory.CreateDirectory(Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, ObjectsDataPaths.ProcessingRecipesPath));
            }
            return JsonUtilities.LoadJsonFilesFromDirectories<Dictionary<string, List<ProcessingRecipe<LootTableEntry>>>>(ObjectsDataPaths.ProcessingRecipesPath);
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

            foreach (RevitalizeContentPack contentPack in RevitalizeModCore.ModContentManager.revitalizeContentPackManager.getAllContentPacks())
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
        /// Converts the old <see cref="JsonCraftingRecipe"/> format into the new <see cref="ProcessingRecipe{LootTableEntry}"/> format.
        /// </summary>
        protected virtual void convertJsonCraftingRecipeBookToProcessingRecipeBook(string SubDirectory = "", string RelativeSubDirectory = "")
        {

            string craftingDirectoryPath = string.IsNullOrEmpty(SubDirectory) ? Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, Constants.PathConstants.Data.CraftingDataPaths.RevitalizeMachinesPath) : SubDirectory;
            string relativeCraftingDirectoryPath = string.IsNullOrEmpty(RelativeSubDirectory) ? Constants.PathConstants.Data.CraftingDataPaths.RevitalizeMachinesPath : RelativeSubDirectory;
            foreach (string craftingStationPath in Directory.GetDirectories(craftingDirectoryPath))
            {
                if (Path.GetFileName(craftingStationPath).Equals("_Templates") || Path.GetFileName(craftingStationPath).Equals("Templates"))
                {
                    //Ignore templates folder.
                    continue;
                }

                string relativeCraftingStationPath = Path.Combine(relativeCraftingDirectoryPath, Path.GetFileName(craftingStationPath));
                if (!File.Exists(Path.Combine(craftingStationPath, "RecipeBookDefinition.json")))
                {
                    this.convertJsonCraftingRecipeBookToProcessingRecipeBook(craftingStationPath, relativeCraftingStationPath);
                }


                JsonCraftingRecipeBookDefinition recipeBookDefinition = JsonUtilities.ReadJsonFile<JsonCraftingRecipeBookDefinition>(relativeCraftingStationPath, "RecipeBookDefinition.json");

                string craftingRecipesPath = Path.Combine(craftingStationPath, "Recipes");
                string relativeCraftingRecipesPath = Path.Combine(relativeCraftingStationPath, "Recipes");

                if (!Directory.Exists(craftingRecipesPath))
                {
                    continue;
                }


                if (!this.processingRecipes.ContainsKey(recipeBookDefinition.craftingRecipeBookId))
                {
                    this.processingRecipes.Add(recipeBookDefinition.craftingRecipeBookId, new List<ProcessingRecipe<LootTableEntry>>());
                }


                //RevitalizeModCore.logWarning("Attempting to load recipes from " + relativeCraftingRecipesPath);
                Dictionary<string, UnlockableJsonCraftingRecipe> craftingRecipes = JsonUtilities.LoadJsonFilesFromDirectoriesWithPaths<UnlockableJsonCraftingRecipe>(relativeCraftingRecipesPath);



                string refinedDirectoryPath = Path.Combine(relativeCraftingStationPath.Split("\\").Skip(4).ToArray());
                if (!Directory.Exists(refinedDirectoryPath))
                {

                    Directory.CreateDirectory(Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, ObjectsDataPaths.ProcessingRecipesPath, refinedDirectoryPath));
                }

                foreach (KeyValuePair<string, UnlockableJsonCraftingRecipe> recipe in craftingRecipes)
                {
                    string newFilePath = Path.Combine(ObjectsDataPaths.ProcessingRecipesPath, refinedDirectoryPath, recipe.Key.Split("\\").Last());
                    if (File.Exists(Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, newFilePath))) continue;

                    ProcessingRecipe<LootTableEntry> entry = new ProcessingRecipe<LootTableEntry>(recipe.Value.recipe.craftingRecipeId, new GameTimeStamp(recipe.Value.recipe.MinutesToCraft), recipe.Value.recipe.inputs.Select(recipe => recipe.item).ToList(), (recipe.Value.recipe.outputs.Select(recipe => new LootTableEntry(recipe.item)).ToList()));
                    this.processingRecipes[recipeBookDefinition.craftingRecipeBookId].Add(entry);
                    List<ProcessingRecipe<LootTableEntry>> toJsonEntries = new List<ProcessingRecipe<LootTableEntry>>() { entry };

                    Dictionary<string, List<ProcessingRecipe<LootTableEntry>>> newRecipes = new();
                    newRecipes.Add(recipeBookDefinition.craftingRecipeBookId, toJsonEntries);
                    JsonUtilities.WriteJsonFile(newRecipes, newFilePath);
                }

            }
        }
    }
}
