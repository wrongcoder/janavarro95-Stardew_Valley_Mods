using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Netcode;
using StardewValley.Network;
using Omegasis.Revitalize.Framework.Constants.CraftingIds;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Objects;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Resources.EarthenResources;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons;
using Omegasis.StardustCore.Animations;
using Omegasis.StardustCore.UIUtilities;

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
            craftingBook.unlockRecipe(CraftingRecipeName);
            return true;
        }

        /// <summary>
        /// Intitialize all Vanilla (aka machine override crafting recipes) and new modded crafting recipes to the game.
        /// </summary>
        public virtual void initializeRecipeBooks()
        {
            this.addAlloyFurnaceRecipes();
            this.addAnvilRecipies();
            this.addWorkbenchRecipes();
        }

        protected virtual void addAlloyFurnaceRecipes()
        {
            //~~~~~~~~~~~~~~~~~~~~~~~//
            // Alloy Furnace Recipes //
            //~~~~~~~~~~~~~~~~~~~~~~~//
            CraftingRecipeBook AlloyFurnaceRecipes = new CraftingRecipeBook(CraftingRecipeBooks.AlloyFurnaceCraftingRecipes);
            AlloyFurnaceRecipes.addInCraftingTab("Default", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Default Tab", new Vector2(100 + 48, 100 + 24 * 4), new AnimationManager(TextureManager.GetExtendedTexture(RevitalizeModCore.Manifest, "Revitalize.Menus", "MenuTabHorizontal"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f), true);


            AlloyFurnaceRecipes.addCraftingRecipe("BrassIngot", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>() {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.CopperBar,1),1),
                new CraftingRecipeComponent(RevitalizeModCore.ObjectManager.getItem(Ingots.AluminumIngot),1),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Coal,5),1)
            }, new CraftingRecipeComponent(RevitalizeModCore.ObjectManager.getItem(Ingots.BrassIngot), 1), null, TimeUtilities.GetMinutesFromTime(0, 3, 0)), true));

            AlloyFurnaceRecipes.addCraftingRecipe("BronzeIngot", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>() {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.CopperBar,1),1),
                new CraftingRecipeComponent(RevitalizeModCore.ObjectManager.getItem(Ingots.TinIngot),1),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Coal,5),1)
            }, new CraftingRecipeComponent(RevitalizeModCore.ObjectManager.getItem(Ingots.BronzeIngot), 1), null, TimeUtilities.GetMinutesFromTime(0, 4, 0)), true));

            AlloyFurnaceRecipes.addCraftingRecipe("SteelIngot", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>() {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.IronBar,1),1),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Coal,5),1)
            }, new CraftingRecipeComponent(RevitalizeModCore.ObjectManager.getItem(Ingots.SteelIngot), 1), null, TimeUtilities.GetMinutesFromTime(0, 6, 0)), true));

            AlloyFurnaceRecipes.addCraftingRecipe("ElectrumIngot", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>() {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.GoldBar,1),1),
                new CraftingRecipeComponent(RevitalizeModCore.ObjectManager.getItem(Ingots.SilverIngot),1),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Coal,5),1)
            }, new CraftingRecipeComponent(RevitalizeModCore.ObjectManager.getItem(Ingots.ElectrumIngot), 1), null, TimeUtilities.GetMinutesFromTime(0, 4, 0)), true));

            if (this.modCraftingRecipesByGroup.ContainsKey(AlloyFurnaceRecipes.craftingGroup))
                foreach (KeyValuePair<string, UnlockableCraftingRecipe> recipe in AlloyFurnaceRecipes.craftingRecipes)
                    if (this.modCraftingRecipesByGroup[AlloyFurnaceRecipes.craftingGroup].craftingRecipes.ContainsKey(recipe.Key))
                    {

                    }
                    else
                        this.modCraftingRecipesByGroup[AlloyFurnaceRecipes.craftingGroup].craftingRecipes.Add(recipe.Key, recipe.Value); //Add in new recipes automatically without having to delete the old crafting recipe book.
            else
                this.modCraftingRecipesByGroup.Add(CraftingRecipeBooks.AlloyFurnaceCraftingRecipes, AlloyFurnaceRecipes);
        }

        protected virtual void addAnvilRecipies()
        {
            //~~~~~~~~~~~~~~~~~~//
            //   Anvil Recipes  //
            //~~~~~~~~~~~~~~~~~~//

            CraftingRecipeBook AnvilRecipes = new CraftingRecipeBook(CraftingRecipeBooks.AnvilCraftingRecipes);
            AnvilRecipes.addInCraftingTab("Default", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Default Tab", new Vector2(100 + 48, 100 + 24 * 4), new AnimationManager(TextureManager.GetExtendedTexture(RevitalizeModCore.Manifest, "Revitalize.Menus", "MenuTabHorizontal"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f), true);

            /*
            AnvilRecipes.addCraftingRecipe("Grinder", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>()
            {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.IronBar,10),10),
                new CraftingRecipeComponent(ModCore.ObjectManager.GetItem("SteelIngot"),30),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.CopperBar,10), 10),
                new CraftingRecipeComponent(new StardewValley.Objects.Chest(true),1)
            }, new CraftingRecipeComponent(ModCore.ObjectManager.GetItem("Grinder"), 1)), true));
            */

            AnvilRecipes.addCraftingRecipe("Mining Drill V1", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>()
            {
                new CraftingRecipeComponent(RevitalizeModCore.ObjectManager.getItem(Ingots.SteelIngot,10),10),
                new CraftingRecipeComponent(RevitalizeModCore.ObjectManager.getItem(Ingots.BrassIngot,10),10),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.BatteryPack,1),1)
            }, new CraftingRecipeComponent(RevitalizeModCore.ObjectManager.getItem(Machines.MiningDrillV1), 1)), true));

            if (this.modCraftingRecipesByGroup.ContainsKey(AnvilRecipes.craftingGroup))
            {
                foreach (KeyValuePair<string, AnimatedButton> pair in AnvilRecipes.craftingMenuTabs)
                    if (this.modCraftingRecipesByGroup[AnvilRecipes.craftingGroup].craftingMenuTabs.ContainsKey(pair.Key))
                    {

                    }
                    else
                        this.modCraftingRecipesByGroup[AnvilRecipes.craftingGroup].craftingMenuTabs.Add(pair.Key, pair.Value);
                foreach (KeyValuePair<string, UnlockableCraftingRecipe> recipe in AnvilRecipes.craftingRecipes)
                    if (this.modCraftingRecipesByGroup[AnvilRecipes.craftingGroup].craftingRecipes.ContainsKey(recipe.Key))
                    {

                    }
                    else
                        this.modCraftingRecipesByGroup[AnvilRecipes.craftingGroup].craftingRecipes.Add(recipe.Key, recipe.Value); //Add in new recipes automatically without having to delete the old crafting recipe book.
            }
            else
                this.modCraftingRecipesByGroup.Add(CraftingRecipeBooks.AnvilCraftingRecipes, AnvilRecipes);
        }

        protected virtual void addWorkbenchRecipes()
        {

            //~~~~~~~~~~~~~~~~~~~//
            // Workbench Recipes //
            //~~~~~~~~~~~~~~~~~~~//

            CraftingRecipeBook WorkbenchRecipes = new CraftingRecipeBook(CraftingRecipeBooks.WorkbenchCraftingRecipies);
            WorkbenchRecipes.addInCraftingTab("Default", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Default Tab", new Vector2(100 + 48, 100 + 24 * 4), new AnimationManager(TextureManager.GetExtendedTexture(RevitalizeModCore.Manifest, "Revitalize.Menus", "MenuTabHorizontal"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f), true);
            WorkbenchRecipes.addInCraftingTab("Furniture", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Furniture Tab", new Vector2(100 + 48, 100 + 24 * 4 * 2), new AnimationManager(TextureManager.GetExtendedTexture(RevitalizeModCore.Manifest, "Revitalize.Menus", "MenuTabHorizontal"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f), false);

            WorkbenchRecipes.addCraftingRecipe("Anvil", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>()
                {
                    //Inputs here
                   new CraftingRecipeComponent(RevitalizeModCore.ObjectManager.getItem(Ingots.SteelIngot),20)
                }, new CraftingRecipeComponent(RevitalizeModCore.ObjectManager.getItem(CraftingStations.Anvil_Id), 1)), false));
            WorkbenchRecipes.addCraftingRecipe("Pickaxe", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>()
            {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Stone,20),20),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Wood,10),10)
            }, new CraftingRecipeComponent(new StardewValley.Tools.Pickaxe() { UpgradeLevel = 0 }, 1)), true));
            WorkbenchRecipes.addCraftingRecipe("Axe", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>()
            {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Stone,20),20),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Wood,10),10)
            }, new CraftingRecipeComponent(new StardewValley.Tools.Axe() { UpgradeLevel = 0 }, 1)), true));
            WorkbenchRecipes.addCraftingRecipe("Hoe", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>()
            {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Stone,20),20),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Wood,10),10)
            }, new CraftingRecipeComponent(new StardewValley.Tools.Hoe() { UpgradeLevel = 0 }, 1)), true));
            WorkbenchRecipes.addCraftingRecipe("Watering Can", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>()
            {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Stone,20),20),
            }, new CraftingRecipeComponent(new StardewValley.Tools.WateringCan() { UpgradeLevel = 0 }, 1)), true));

            /*
            WorkbenchRecipes.addCraftingRecipe("Copper Wire", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>()
            {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.CopperBar,1),1),
            }, new CraftingRecipeComponent(ModCore.ObjectManager.GetItem("CopperWire"), 2), null, 0), true));
            */
            /*
            WorkbenchRecipes.addCraftingRecipe("Alloy Furnace", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>()
            {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Clay,20),10),
                new CraftingRecipeComponent(ModCore.ObjectManager.resources.getResource("Sand"), 10)
            }, new CraftingRecipeComponent(ModCore.ObjectManager.GetItem("AlloyFurnace"), 1), null, 0), true));
            WorkbenchRecipes.addCraftingRecipe("Sand Box", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>()
            {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Wood,100),100),
                new CraftingRecipeComponent(ModCore.ObjectManager.resources.getResource("Sand"), 25)
            }, new CraftingRecipeComponent(ModCore.ObjectManager.GetItem("SandBox"), 1), null, 0), true));
            */
            /*
            WorkbenchRecipes.addCraftingRecipe("Battery Bin", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>()
            {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Wood,100),100),
                new CraftingRecipeComponent(ModCore.ObjectManager.GetItem("ElectrumIngot"),10)
            }, new CraftingRecipeComponent(ModCore.ObjectManager.GetItem("BatteryBin"), 1), null, 0), true));
            WorkbenchRecipes.addCraftingRecipe("Capacitor", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>()
            {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Wood,50),50),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.CopperBar,10),10)
            }, new CraftingRecipeComponent(ModCore.ObjectManager.GetItem("Capacitor"), 1), null, 0), true));
            WorkbenchRecipes.addCraftingRecipe("Charging Station", new UnlockableCraftingRecipe("Default", new Recipe(new List<CraftingRecipeComponent>()
            {
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.Wood,100),100),
                new CraftingRecipeComponent(new StardewValley.Object((int)Enums.SDVObject.IronBar,10),10),
                new CraftingRecipeComponent(ModCore.ObjectManager.GetItem("CopperWire"), 20),
                new CraftingRecipeComponent(ModCore.ObjectManager.GetItem("Capacitor"), 1)
            }, new CraftingRecipeComponent(ModCore.ObjectManager.GetItem("ChargingStation"), 1), null, 0), true));
            */

            //WorkbenchRecipes.addCraftingRecipe("Oak Chair", new UnlockableCraftingRecipe("Furniture", new Recipe(new List<CraftingRecipeComponent>(), new CraftingRecipeComponent(ModCore.ObjectManager.GetItem("Oak Chair"), 1), new StatCost(0, 0, 100, 0), 0), true));

            if (this.modCraftingRecipesByGroup.ContainsKey(WorkbenchRecipes.craftingGroup))
            {
                foreach (KeyValuePair<string, AnimatedButton> pair in WorkbenchRecipes.craftingMenuTabs)
                    if (this.modCraftingRecipesByGroup[WorkbenchRecipes.craftingGroup].craftingMenuTabs.ContainsKey(pair.Key))
                    {

                    }
                    else
                        this.modCraftingRecipesByGroup[WorkbenchRecipes.craftingGroup].craftingMenuTabs.Add(pair.Key, pair.Value);
                foreach (KeyValuePair<string, UnlockableCraftingRecipe> recipe in WorkbenchRecipes.craftingRecipes)
                    if (this.modCraftingRecipesByGroup[WorkbenchRecipes.craftingGroup].craftingRecipes.ContainsKey(recipe.Key))
                    {

                    }
                    else
                        this.modCraftingRecipesByGroup[WorkbenchRecipes.craftingGroup].craftingRecipes.Add(recipe.Key, recipe.Value); //Add in new recipes automatically without having to delete the old crafting recipe book.
            }
            else
                this.modCraftingRecipesByGroup.Add(CraftingRecipeBooks.WorkbenchCraftingRecipies, WorkbenchRecipes);



        }
    }
}
