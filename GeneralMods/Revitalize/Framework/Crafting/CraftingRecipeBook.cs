using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using Omegasis.Revitalize.Framework.Menus;
using Omegasis.Revitalize.Framework.World.Objects.Machines;
using Omegasis.StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons;

namespace Omegasis.Revitalize.Framework.Crafting
{
    public class CraftingRecipeBook
    {


        /// <summary>
        /// All of the crafting recipes contained by this crafting list.
        /// </summary>
        public Dictionary<string, UnlockableCraftingRecipe> craftingRecipes;


        /// <summary>
        /// All of the crafting tabs to be used for the menu.
        /// </summary>
        public Dictionary<string, AnimatedButton> craftingMenuTabs;

        /// <summary>
        /// Which group of crafting recipes this book belongs to.
        /// </summary>
        public string craftingGroup;

        public string defaultTab;

        public CraftingRecipeBook()
        {

        }

        public CraftingRecipeBook(string CraftingGroup)
        {
            this.craftingGroup = CraftingGroup;
            this.craftingRecipes = new Dictionary<string, UnlockableCraftingRecipe>();
            this.craftingMenuTabs = new Dictionary<string, AnimatedButton>();
        }

        /// <summary>
        /// Adds in a new crafting recipe.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Recipe"></param>
        public void addCraftingRecipe(string Name, UnlockableCraftingRecipe Recipe)
        {
            if (this.craftingRecipes.ContainsKey(Name) == false)
                this.craftingRecipes.Add(Name, Recipe);
            else
                throw new Exception("This crafting book already contains a recipe with the same id!");
        }

        /// <summary>
        /// Adds in a crafting recipe.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Recipe"></param>
        /// <param name="Unlocked">Is this recipe already unlocked?</param>
        public void addCraftingRecipe(string Name, Recipe Recipe, bool Unlocked)
        {
            UnlockableCraftingRecipe recipe = new UnlockableCraftingRecipe(this.craftingGroup, Recipe, Unlocked);
            this.addCraftingRecipe(Name, recipe);
        }

        /// <summary>
        /// Adds in a crafting tab to the recipe book to be used for the menu generated for crafting recipes.
        /// </summary>
        /// <param name="TabName"></param>
        /// <param name="TabSprite"></param>
        /// <param name="IsDefaultTab"></param>
        public void addInCraftingTab(string TabName, AnimatedButton TabSprite, bool IsDefaultTab)
        {
            if (this.craftingMenuTabs.ContainsKey(TabName))
                throw new Exception("A tab with the same name already exists!");
            else
                this.craftingMenuTabs.Add(TabName, TabSprite);
            if (IsDefaultTab)
                this.defaultTab = TabName;
        }

        /// <summary>
        /// Gets the crafting recipe by it's name.
        /// </summary>
        /// <param name="CraftingRecipeName"></param>
        /// <returns></returns>
        public UnlockableCraftingRecipe getCraftingRecipe(string CraftingRecipeName)
        {
            if (this.containsCraftingRecipe(CraftingRecipeName))
                return this.craftingRecipes[CraftingRecipeName];
            else return null;
        }

        /// <summary>
        /// Checks to see if this crafting recipe book contains a given recipe.
        /// </summary>
        /// <param name="CraftingRecipeName"></param>
        /// <returns></returns>
        public virtual bool containsCraftingRecipe(string CraftingRecipeName)
        {

            return this.craftingRecipes.ContainsKey(CraftingRecipeName);
        }

        /// <summary>
        /// Checks to see if a crafting recipe has been unlocked.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool hasUnlockedCraftingRecipe(string Name)
        {
            UnlockableCraftingRecipe recipe = this.getCraftingRecipe(Name);
            if (recipe == null) return false;
            else return recipe.hasUnlocked;
        }

        /// <summary>
        /// Unlocks the crating recipe so that it can be shown in the menu.
        /// </summary>
        /// <param name="Name"></param>
        public void unlockRecipe(string Name)
        {
            UnlockableCraftingRecipe recipe = this.getCraftingRecipe(Name);
            if (recipe == null) return;
            else recipe.unlock();
        }

        /// <summary>
        /// Opens up a crafting menu from this crafting book.
        /// </summary>
        public void openCraftingMenu()
        {
            CraftingMenuV1 menu = new Framework.Menus.CraftingMenuV1(100, 100, 400, 700, Color.White, Game1.player.Items);
            //menu.addInCraftingPageTab("Default", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Default Tab", new Vector2(100 + 48, 100 + (24 * 4)), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Menus", "MenuTabHorizontal"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f));

            foreach (KeyValuePair<string, AnimatedButton> pair in this.craftingMenuTabs)
                menu.addInCraftingPageTab(pair.Key, pair.Value);

            foreach (KeyValuePair<string, UnlockableCraftingRecipe> pair in this.craftingRecipes)
                if (pair.Value.hasUnlocked)
                    menu.addInCraftingRecipe(new Framework.Menus.MenuComponents.CraftingRecipeButton(pair.Value.recipe, null, new Vector2(), new Rectangle(0, 0, 16, 16), 4f, true, Color.White), pair.Value.whichTab);
                else
                    RevitalizeModCore.log("Recipe is locked!");
            menu.currentTab = this.defaultTab;
            menu.sortRecipes();
            if (Game1.activeClickableMenu == null) Game1.activeClickableMenu = menu;
        }

        public CraftingMenuV1 getCraftingMenuForMachine(int x, int y, int width, int height, ref IList<Item> Items, ref IList<Item> Output, Machine Machine)
        {
            CraftingMenuV1 menu = new Framework.Menus.CraftingMenuV1(x, y, width, height, Color.White, ref Items, ref Output, Machine);
            //menu.addInCraftingPageTab("Default", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Default Tab", new Vector2(100 + 48, 100 + (24 * 4)), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Menus", "MenuTabHorizontal"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f));

            foreach (KeyValuePair<string, AnimatedButton> pair in this.craftingMenuTabs)
                menu.addInCraftingPageTab(pair.Key, pair.Value);

            foreach (KeyValuePair<string, UnlockableCraftingRecipe> pair in this.craftingRecipes)
                if (pair.Value.hasUnlocked)
                    menu.addInCraftingRecipe(new Framework.Menus.MenuComponents.CraftingRecipeButton(pair.Value.recipe, null, new Vector2(), new Rectangle(0, 0, 16, 16), 4f, true, Color.White), pair.Value.whichTab);
                else
                    RevitalizeModCore.log("Recipe is locked!");
            menu.currentTab = this.defaultTab;
            menu.sortRecipes();
            return menu;
        }

        public void openCraftingMenu(int x, int y, int width, int height, ref IList<Item> items)
        {
            CraftingMenuV1 menu = new Framework.Menus.CraftingMenuV1(x, y, width, height, Color.White, items);
            //menu.addInCraftingPageTab("Default", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Default Tab", new Vector2(100 + 48, 100 + (24 * 4)), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Menus", "MenuTabHorizontal"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f));

            foreach (KeyValuePair<string, AnimatedButton> pair in this.craftingMenuTabs)
                menu.addInCraftingPageTab(pair.Key, pair.Value);

            foreach (KeyValuePair<string, UnlockableCraftingRecipe> pair in this.craftingRecipes)
                if (pair.Value.hasUnlocked)
                    menu.addInCraftingRecipe(new Framework.Menus.MenuComponents.CraftingRecipeButton(pair.Value.recipe, null, new Vector2(), new Rectangle(0, 0, 16, 16), 4f, true, Color.White), pair.Value.whichTab);
                else
                    RevitalizeModCore.log("Recipe is locked!");
            menu.currentTab = this.defaultTab;
            menu.sortRecipes();
            if (Game1.activeClickableMenu == null) Game1.activeClickableMenu = menu;
        }
    }
}
