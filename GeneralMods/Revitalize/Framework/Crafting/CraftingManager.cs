using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Netcode;
using StardewValley.Network;
using Omegasis.Revitalize.Framework.Constants.CraftingIds;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons;
using Omegasis.StardustCore.Animations;
using Omegasis.StardustCore.UIUtilities;
using Omegasis.Revitalize.Framework.Managers;
using Omegasis.Revitalize.Framework.Crafting.JsonContent;
using System.IO;
using Omegasis.Revitalize.Framework.Constants.Ids.Resources.EarthenResources;

namespace Omegasis.Revitalize.Framework.Crafting
{
    public class CraftingManager
    {

        /// <summary>
        /// Organizes crafting recipes by group. So a workbench would have a workbench crafting book, and anvil has different recipes, etc.
        /// </summary>
        public Dictionary<string, CraftingRecipeBook> modCraftingRecipesByGroup;
        /// <summary>
        /// Vanilla crafting recipes that are used to do things like smelt additional ore in the SDV vanilla furnace.
        /// </summary>
        public VanillaRecipeBook vanillaCraftingRecipes;

        public CraftingManager()
        {

            this.modCraftingRecipesByGroup = new Dictionary<string, CraftingRecipeBook>();
            this.vanillaCraftingRecipes = new VanillaRecipeBook();
        }

        /// <summary>
        /// Checks to see if a given crafting book exists in the list of registered crafting books.
        /// </summary>
        /// <param name="CraftingBookName"></param>
        /// <returns></returns>
        public virtual bool craftingRecipeBookExists(string CraftingBookName)
        {
            return this.modCraftingRecipesByGroup.ContainsKey(CraftingBookName);
        }

        /// <summary>
        /// Gets a crafting book that has been registered in <see cref="modCraftingRecipesByGroup"/>
        /// </summary>
        /// <param name="CraftingBookName">The name of the crafting book.</param>
        /// <returns></returns>
        public virtual CraftingRecipeBook getCraftingRecipeBook(string CraftingBookName)
        {
            if (this.craftingRecipeBookExists(CraftingBookName))
                return this.modCraftingRecipesByGroup[CraftingBookName];
            return null;
        }


        /// <summary>
        /// Gets all crafting recipes that have been unlocked for a given crafting book.
        /// </summary>
        /// <param name="CraftingBookName"></param>
        /// <returns></returns>
        public virtual List<Recipe> getUnlockedCraftingRecipes(string CraftingBookName)
        {
            List<Recipe> unlockedRecipes = new List<Recipe>();
            CraftingRecipeBook book = this.getCraftingRecipeBook(CraftingBookName);
            if (book == null) return unlockedRecipes;
            foreach (UnlockableCraftingRecipe recipe in book.craftingRecipes.Values)
            {
                if (recipe.hasUnlocked)
                {
                    unlockedRecipes.Add(recipe.recipe);
                }
            }
            return unlockedRecipes;

        }

        /// <summary>
        /// Learns all of the passed in recipies.
        /// </summary>
        /// <param name="CraftingRecipeBooksToRecipeNameMapping"></param>
        /// <returns>A dictionary mapping with a keyvalue pair as the key representing the crafting book name and recipe, and a value representing if the recipe was learned or not.</returns>
        public virtual Dictionary<KeyValuePair<string, string>, bool> learnCraftingRecipes(NetStringDictionary<string, NetString> CraftingRecipeBooksToRecipeNameMapping)
        {
            Dictionary<KeyValuePair<string, string>, bool> recipesLearned = new Dictionary<KeyValuePair<string, string>, bool>();
            foreach (var craftingBookToRecipes in CraftingRecipeBooksToRecipeNameMapping)
            {
                Dictionary<KeyValuePair<string, string>, bool> learnedRecipes = this.learnCraftingRecipes(craftingBookToRecipes);
                foreach (var learnedRecipe in learnedRecipes)
                    recipesLearned.Add(learnedRecipe.Key, learnedRecipe.Value);
            }
            return recipesLearned;
        }

        /// <summary>
        /// Learns all of the passed in recipies.
        /// </summary>
        /// <param name="CraftingRecipeBooksToRecipeNameMapping"></param>
        /// <returns>A dictionary mapping with a keyvalue pair as the key representing the crafting book name and recipe, and a value representing if the recipe was learned or not.</returns>
        public virtual Dictionary<KeyValuePair<string, string>, bool> learnCraftingRecipes(Dictionary<string, string> CraftingRecipeBooksToRecipeNameMapping)
        {
            Dictionary<KeyValuePair<string, string>, bool> recipesLearned = new Dictionary<KeyValuePair<string, string>, bool>();
            foreach (KeyValuePair<string, string> craftingBookToRecipes in CraftingRecipeBooksToRecipeNameMapping)
            {
                bool learned = this.learnCraftingRecipe(craftingBookToRecipes.Key, craftingBookToRecipes.Value);
                recipesLearned.Add(craftingBookToRecipes, learned);
            }
            return recipesLearned;
        }

        /// <summary>
        /// Unlocks a given crafting recipe.
        /// </summary>
        /// <param name="CraftingBookName"></param>
        /// <param name="CraftingRecipeName"></param>
        /// <returns></returns>
        public virtual bool learnCraftingRecipe(string CraftingBookName, string CraftingRecipeName)
        {
            if (!this.craftingRecipeBookExists(CraftingBookName)) return false;
            CraftingRecipeBook craftingBook = this.getCraftingRecipeBook(CraftingBookName);
            if (!craftingBook.containsCraftingRecipe(CraftingRecipeName)) return false;
            if (craftingBook.craftingRecipes[CraftingRecipeName].hasUnlocked) return false;
            craftingBook.unlockRecipe(CraftingRecipeName);

            //The player save data will only be null when loading from a .json file when starting up the game.
            if (RevitalizeModCore.SaveDataManager.playerSaveData != null)
            {
                RevitalizeModCore.SaveDataManager.playerSaveData.addUnlockedCraftingRecipe(CraftingBookName, CraftingRecipeName);
            }
            return true;
        }


        /// <summary>
        /// Checks to see if a dictionary of crafting recipes have already been learned. Returns false if even a single crafting recipe hasn't been learned.
        /// </summary>
        /// <param name="CraftingRecipeBooksToRecipeNameMapping"></param>
        /// <returns></returns>
        public virtual bool knowsCraftingRecipes(NetStringDictionary<string, NetString> CraftingRecipeBooksToRecipeNameMapping)
        {
            bool allRecipesLearned = true;
            foreach (var craftingBookToRecipes in CraftingRecipeBooksToRecipeNameMapping)
            {
                bool learned = this.knowsCraftingRecipes(craftingBookToRecipes);
                if (learned == false)
                {
                    allRecipesLearned = false;
                }
            }
            return allRecipesLearned;
        }


        /// <summary>
        /// Checks to see if a dictionary of crafting recipes have already been learned. Returns false if even a single crafting recipe hasn't been learned.
        /// </summary>
        /// <param name="CraftingRecipeBooksToRecipeNameMapping"></param>
        /// <returns></returns>
        public virtual bool knowsCraftingRecipes(Dictionary<string, string> CraftingRecipeBooksToRecipeNameMapping)
        {
            bool allRecipesLearned = true;
            foreach (KeyValuePair<string, string> craftingBookToRecipes in CraftingRecipeBooksToRecipeNameMapping)
            {
                bool learned = this.knowsCraftingRecipe(craftingBookToRecipes.Key, craftingBookToRecipes.Value);
                if (learned == false)
                {
                    allRecipesLearned = false;
                }
            }
            return allRecipesLearned;
        }

        /// <summary>
        /// Checks to see if a specific crafting recipe is already learned.
        /// </summary>
        /// <param name="CraftingBookName"></param>
        /// <param name="CraftingRecipeName"></param>
        /// <returns></returns>
        public virtual bool knowsCraftingRecipe(string CraftingBookName, string CraftingRecipeName)
        {
            if (!this.craftingRecipeBookExists(CraftingBookName)) return false;
            CraftingRecipeBook craftingBook = this.getCraftingRecipeBook(CraftingBookName);
            if (!craftingBook.containsCraftingRecipe(CraftingRecipeName)) return false;
            return craftingBook.craftingRecipes[CraftingRecipeName].hasUnlocked;
        }

        /// <summary>
        /// Intitialize all Vanilla (aka machine override crafting recipes) and new modded crafting recipes to the game.
        /// </summary>
        public virtual void initializeRecipeBooks()
        {

            this.addInCraftingRecipesForCraftingStationsFromJsonFiles();
            this.addInCraftingRecipesForRevitalizeMachinesFromJsonFiles();

            // this.addAlloyFurnaceRecipes();
            // this.addAnvilRecipies();
            // this.addWorkbenchRecipes();
        }

        protected virtual void addAlloyFurnaceRecipes()
        {
            //~~~~~~~~~~~~~~~~~~~~~~~//
            // Alloy Furnace Recipes //
            //~~~~~~~~~~~~~~~~~~~~~~~//
            CraftingRecipeBook AlloyFurnaceRecipes = new CraftingRecipeBook(MachineCraftingRecipeBooks.AlloyFurnaceCraftingRecipes);
            AlloyFurnaceRecipes.addInCraftingTab("Default", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Default Tab", new Vector2(100 + 48, 100 + 24 * 4), new AnimationManager(TextureManager.GetExtendedTexture(RevitalizeModCore.Manifest, "Revitalize.Menus", "MenuTabHorizontal"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f), true);


            AlloyFurnaceRecipes.addCraftingRecipe("BrassIngot", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>() {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.CopperBar,1),1),
                new CraftingRecipeComponent(RevitalizeModCore.ModContentManager.objectManager.getItem(Ingots.AluminumIngot),1),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Coal,5),1)
            }, new CraftingRecipeComponent(RevitalizeModCore.ModContentManager.objectManager.getItem(Ingots.BrassIngot), 1), null, TimeUtilities.GetMinutesFromTime(0, 3, 0)), true));

            AlloyFurnaceRecipes.addCraftingRecipe("BronzeIngot", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>() {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.CopperBar,1),1),
                new CraftingRecipeComponent(RevitalizeModCore.ModContentManager.objectManager.getItem(Ingots.TinIngot),1),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Coal,5),1)
            }, new CraftingRecipeComponent(RevitalizeModCore.ModContentManager.objectManager.getItem(Ingots.BronzeIngot), 1), null, TimeUtilities.GetMinutesFromTime(0, 4, 0)), true));

            AlloyFurnaceRecipes.addCraftingRecipe("SteelIngot", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>() {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.IronBar,1),1),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Coal,5),1)
            }, new CraftingRecipeComponent(RevitalizeModCore.ModContentManager.objectManager.getItem(Ingots.SteelIngot), 1), null, TimeUtilities.GetMinutesFromTime(0, 6, 0)), true));

            AlloyFurnaceRecipes.addCraftingRecipe("ElectrumIngot", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>() {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.GoldBar,1),1),
                new CraftingRecipeComponent(RevitalizeModCore.ModContentManager.objectManager.getItem(Ingots.SilverIngot),1),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Coal,5),1)
            }, new CraftingRecipeComponent(RevitalizeModCore.ModContentManager.objectManager.getItem(Ingots.ElectrumIngot), 1), null, TimeUtilities.GetMinutesFromTime(0, 4, 0)), true));

            if (this.modCraftingRecipesByGroup.ContainsKey(AlloyFurnaceRecipes.craftingRecipeBookId))
                foreach (KeyValuePair<string, UnlockableCraftingRecipe> recipe in AlloyFurnaceRecipes.craftingRecipes)
                    if (this.modCraftingRecipesByGroup[AlloyFurnaceRecipes.craftingRecipeBookId].craftingRecipes.ContainsKey(recipe.Key))
                    {

                    }
                    else
                        this.modCraftingRecipesByGroup[AlloyFurnaceRecipes.craftingRecipeBookId].craftingRecipes.Add(recipe.Key, recipe.Value); //Add in new recipes automatically without having to delete the old crafting recipe book.
            else
                this.modCraftingRecipesByGroup.Add(MachineCraftingRecipeBooks.AlloyFurnaceCraftingRecipes, AlloyFurnaceRecipes);
        }

        /// <summary>
        /// Adds in crafting recipes from json files for all crafting stations. DOES NOT CURRENTLY support deeply nested directories. See the revitalize machines method for that logic.
        /// </summary>
        protected virtual void addInCraftingRecipesForCraftingStationsFromJsonFiles()
        {

            string craftingDirectoryPath = Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, Constants.PathConstants.Data.CraftingDataPaths.CraftingStationsPath);
            string relativeCraftingDirectoryPath = Constants.PathConstants.Data.CraftingDataPaths.CraftingStationsPath;
            foreach (string craftingStationPath in Directory.GetDirectories(craftingDirectoryPath))
            {
                if (Path.GetFileName(craftingStationPath).Equals("_Templates") || Path.GetFileName(craftingStationPath).Equals("Templates"))
                {
                    //Ignore templates folder.
                    continue;
                }


                string relativeCraftingStationPath = Path.Combine(relativeCraftingDirectoryPath, Path.GetFileName(craftingStationPath));
                JsonCraftingRecipeBookDefinition recipeBookDefinition = JsonUtilities.ReadJsonFile<JsonCraftingRecipeBookDefinition>(relativeCraftingStationPath, "RecipeBookDefinition.json");

                string CraftingTabsPath = Path.Combine(craftingStationPath, "CraftingMenuTabs");
                string relativeCratingTabsPath = Path.Combine(relativeCraftingStationPath, "CraftingMenuTabs");
                string craftingRecipesPath = Path.Combine(craftingStationPath, "Recipes");
                string relativeCraftingRecipesPath = Path.Combine(relativeCraftingStationPath, "Recipes");

                if (!Directory.Exists(craftingRecipesPath))
                {
                    continue;
                }

                //RevitalizeModCore.logWarning("Attempting to load recipes from " + relativeCraftingRecipesPath);
                CraftingRecipeBook craftingRecipeBook = new CraftingRecipeBook(recipeBookDefinition, JsonUtilities.LoadJsonFilesFromDirectories<JsonCraftingMenuTab>(relativeCratingTabsPath), JsonUtilities.LoadJsonFilesFromDirectories<UnlockableJsonCraftingRecipe>(relativeCraftingRecipesPath));

                //Add validation + add in recipies that may or may not be added specifically from json to the already existing data.
                if (this.modCraftingRecipesByGroup.ContainsKey(recipeBookDefinition.craftingRecipeBookId))
                {
                    foreach (KeyValuePair<string, AnimatedButton> pair in craftingRecipeBook.craftingMenuTabs)
                        if (this.modCraftingRecipesByGroup[craftingRecipeBook.craftingRecipeBookId].craftingMenuTabs.ContainsKey(pair.Key))
                        {

                        }
                        else
                            this.modCraftingRecipesByGroup[craftingRecipeBook.craftingRecipeBookId].craftingMenuTabs.Add(pair.Key, pair.Value);
                    foreach (KeyValuePair<string, UnlockableCraftingRecipe> recipe in craftingRecipeBook.craftingRecipes)
                        if (this.modCraftingRecipesByGroup[craftingRecipeBook.craftingRecipeBookId].craftingRecipes.ContainsKey(recipe.Key))
                        {

                        }
                        else
                            this.modCraftingRecipesByGroup[craftingRecipeBook.craftingRecipeBookId].craftingRecipes.Add(recipe.Key, recipe.Value); //Add in new recipes automatically without having to delete the old crafting recipe book.
                }
                else
                    this.modCraftingRecipesByGroup.Add(craftingRecipeBook.craftingRecipeBookId, craftingRecipeBook);

            }

        }


        /// <summary>
        /// Adds in crafting recipes from json files for all crafting stations. Also has recurrisve functionality to seach deeply nested directories.
        /// </summary>
        protected virtual void addInCraftingRecipesForRevitalizeMachinesFromJsonFiles(string SubDirectory="", string RelativeSubDirectory="")
        {

            string craftingDirectoryPath = string.IsNullOrEmpty(SubDirectory)? Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, Constants.PathConstants.Data.CraftingDataPaths.RevitalizeMachinesPath):SubDirectory;
            string relativeCraftingDirectoryPath = string.IsNullOrEmpty(RelativeSubDirectory)? Constants.PathConstants.Data.CraftingDataPaths.RevitalizeMachinesPath:RelativeSubDirectory;
            foreach (string craftingStationPath in Directory.GetDirectories(craftingDirectoryPath))
            {
                if (Path.GetFileName(craftingStationPath).Equals("_Templates") || Path.GetFileName(craftingStationPath).Equals("Templates"))
                {
                    //Ignore templates folder.
                    continue;
                }

                string relativeCraftingStationPath = Path.Combine(relativeCraftingDirectoryPath, Path.GetFileName(craftingStationPath));
                if (!File.Exists(Path.Combine(craftingStationPath, "RecipeBookDefinition.json"))){
                    this.addInCraftingRecipesForRevitalizeMachinesFromJsonFiles(craftingStationPath, relativeCraftingStationPath);
                }


                JsonCraftingRecipeBookDefinition recipeBookDefinition = JsonUtilities.ReadJsonFile<JsonCraftingRecipeBookDefinition>(relativeCraftingStationPath, "RecipeBookDefinition.json");

                string craftingRecipesPath = Path.Combine(craftingStationPath, "Recipes");
                string relativeCraftingRecipesPath = Path.Combine(relativeCraftingStationPath, "Recipes");

                if (!Directory.Exists(craftingRecipesPath))
                {
                    continue;
                }

                //RevitalizeModCore.logWarning("Attempting to load recipes from " + relativeCraftingRecipesPath);
                CraftingRecipeBook craftingRecipeBook = new CraftingRecipeBook(recipeBookDefinition, new List<JsonCraftingMenuTab>(), JsonUtilities.LoadJsonFilesFromDirectories<UnlockableJsonCraftingRecipe>(relativeCraftingRecipesPath));

                //Add validation + add in recipies that may or may not be added specifically from json to the already existing data.
                if (this.modCraftingRecipesByGroup.ContainsKey(recipeBookDefinition.craftingRecipeBookId))
                {
                    foreach (KeyValuePair<string, UnlockableCraftingRecipe> recipe in craftingRecipeBook.craftingRecipes)
                        if (!this.modCraftingRecipesByGroup[craftingRecipeBook.craftingRecipeBookId].craftingRecipes.ContainsKey(recipe.Key))
                        {
                            this.modCraftingRecipesByGroup[craftingRecipeBook.craftingRecipeBookId].craftingRecipes.Add(recipe.Key, recipe.Value);
                        }
                }
                else
                    this.modCraftingRecipesByGroup.Add(craftingRecipeBook.craftingRecipeBookId, craftingRecipeBook);

            }
        }

    }


}
