using System;
using System.Collections.Generic;
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
            base.loadRecipes();

            foreach (KeyValuePair<string, List<ProcessingRecipe<LootTableEntry>>> entry in this.processingRecipes)
            {
                foreach (ProcessingRecipe<LootTableEntry> recipe in entry.Value)
                {
                    RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.addProcessingRecipe(entry.Key, recipe);
                }
            }

            foreach (KeyValuePair<string, List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>>> entry in this.advancedGeodeCrusherRecipes)
            {
                foreach (ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput> recipe in entry.Value)
                {
                    RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.addAdvancedGeodeCrusherProcessingRecipe(entry.Key, recipe);
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

        public override List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>> getAdvancedGeodeCrusherProcessingRecipes(string GeodeCrusherId)
        {
            return base.getAdvancedGeodeCrusherProcessingRecipes(GeodeCrusherId);
        }

        protected override List<Dictionary<string, List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>>>> loadAdvancedGeodeCrusherProcessingRecipesFromJsonFiles()
        {
            return JsonUtilities.LoadJsonFilesFromDirectories<Dictionary<string, List<ProcessingRecipe<AdvancedGeodeCrusher.GeodeCrusherOutput>>>>(this.contentPack, ObjectsDataPaths.ProcessingRecipesSpecialCases_AdvancedGeodeCrushers);
        }

        protected override List<Dictionary<string, List<ProcessingRecipe<LootTableEntry>>>> loadProcessingRecipesFromJsonFiles()
        {
            return JsonUtilities.LoadJsonFilesFromDirectories<Dictionary<string, List<ProcessingRecipe<LootTableEntry>>>>(this.contentPack, ObjectsDataPaths.ProcessingRecipesGeneralCasesPath);
        }
    }
}
