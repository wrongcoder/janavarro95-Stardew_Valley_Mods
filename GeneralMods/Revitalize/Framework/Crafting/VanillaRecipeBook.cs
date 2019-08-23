using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Revitalize.Framework.Crafting
{
    public class VanillaRecipeBook
    {
        public static VanillaRecipeBook VanillaRecipes;

        public Dictionary<string,Dictionary<string,VanillaRecipe>> recipesByObjectName;
        /// <summary>
        /// All of the recipes bassed off of pytk id. the first key is the name of the SDV object, the second key is the player's held object pktk id if it exists. 
        /// </summary>
        public Dictionary<string, Dictionary<string, VanillaRecipe>> recipesByObjectPyTKID;
        public Dictionary<string, Dictionary<Item, VanillaRecipe>> recipesByObject;

        public VanillaRecipeBook()
        {
            this.recipesByObjectName = new Dictionary<string, Dictionary<string, VanillaRecipe>>();
            this.recipesByObjectPyTKID = new Dictionary<string, Dictionary<string, VanillaRecipe>>();
            this.recipesByObject = new Dictionary<string, Dictionary<Item, VanillaRecipe>>();
            if (VanillaRecipes == null)
            {
                VanillaRecipes = this;
            }

            this.recipesByObjectName = new Dictionary<string, Dictionary<string, VanillaRecipe>>();
            this.recipesByObjectName.Add("Furnace", new Dictionary<string, VanillaRecipe>());

            VanillaRecipe furnace_tinOre = new VanillaRecipe(new Dictionary<Item, int>()
            {
                {ModCore.ObjectManager.resources.getOre("Tin"),5 },
                {new StardewValley.Object(382,1),1}
            }, new KeyValuePair<Item, int>(ModCore.ObjectManager.GetItem("TinIngot"), 1), 50, new StatCost(), false);

            this.recipesByObjectName["Furnace"].Add("Tin Ore", furnace_tinOre);
        }

        /// <summary>
        /// Trys to get a recipe list for a machine based off of the SDV Machine Name.
        /// </summary>
        /// <param name="Machine"></param>
        /// <returns></returns>
        public Dictionary<string,VanillaRecipe> GetRecipesForNamedRecipeBook(StardewValley.Object Machine)
        {
            if (this.recipesByObjectName.ContainsKey(Machine.Name))
            {
                return this.recipesByObjectName[Machine.Name];
            }
            else
            {
                return null;
            }
        }

        public bool DoesARecipeExistForHeldObjectName(StardewValley.Object Machine)
        {
            if (Game1.player.ActiveObject == null) return false;

            Dictionary<string, VanillaRecipe> recipes = this.GetRecipesForNamedRecipeBook(Machine);
            if (recipes == null) return false;

            if (recipes.ContainsKey(Game1.player.ActiveObject.Name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Trys to get a vanilla recipe from the recipe book related to what the player is looking at and 
        /// </summary>
        /// <param name="Machine"></param>
        /// <returns></returns>
        public VanillaRecipe GetVanillaRecipeFromHeldObjectName(StardewValley.Object Machine)
        {
            if (Game1.player.ActiveObject == null) return null;

            Dictionary<string, VanillaRecipe> recipes = this.GetRecipesForNamedRecipeBook(Machine);
            if (recipes == null) return null;

            if (recipes.ContainsKey(Game1.player.ActiveObject.Name))
            {
                return recipes[Game1.player.ActiveObject.Name];
            }
            else
            {
                return null;
            }
        }

        public bool TryToCraftRecipe(StardewValley.Object Machine)
        {
            if (this.DoesARecipeExistForHeldObjectName(Machine))
            {
                ModCore.log("Recipe exists!");
                VanillaRecipe rec = this.GetVanillaRecipeFromHeldObjectName(Machine);
                bool crafted=rec.craft(Machine);
                if(crafted)this.playCraftingSound(Machine);
                return crafted;
            }
            else
            {
                ModCore.log("No recipe!");
                return false;
            }
        }

        /// <summary>
        /// Trys to play any additional sounds associated with crafting.
        /// </summary>
        /// <param name="Machine"></param>
        public void playCraftingSound(StardewValley.Object Machine)
        {
            if (Machine.Name == "Furnace")
            {
                Game1.playSound("furnace");
            }
        }
       
    }
}
