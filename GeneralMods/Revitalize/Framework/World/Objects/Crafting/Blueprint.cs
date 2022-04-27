using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Netcode;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Items.Tools;
using StardewValley;
using StardewValley.Network;

namespace Omegasis.Revitalize.Framework.World.Objects.Crafting
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Crafting.Blueprint")]
    public class Blueprint : CustomItem
    {
        /// <summary>
        /// A mapping from the name of the crafting book to the name of the crafting recipe to unlock.
        /// </summary>
        public readonly NetStringDictionary<string, NetString> craftingRecipesToUnlock = new();

        public Blueprint()
        {

        }

        public Blueprint(BasicItemInformation Info, string CraftingRecipeBookName, string CraftingRecipe ) : base(Info)
        {
            this.addCraftingRecipe(CraftingRecipeBookName, CraftingRecipe);
        }

        public Blueprint(BasicItemInformation Info, Dictionary<string,string> CraftingRecipesToUnlock) : base(Info)
        {
            this.addCraftingRecipe(CraftingRecipesToUnlock);
        }

        public Blueprint(BasicItemInformation Info, NetStringDictionary<string, NetString> CraftingRecipesToUnlock) : base(Info)
        {
            foreach (var craftingBookNameToCraftingRecipeName in CraftingRecipesToUnlock)
            {
                this.addCraftingRecipe(craftingBookNameToCraftingRecipeName);
            }
        }

        protected virtual void addCraftingRecipe(Dictionary<string,string> CraftingRecipes)
        {
            foreach (KeyValuePair<string, string> craftingBookNameToCraftingRecipeName in CraftingRecipes)
            {
                this.addCraftingRecipe(craftingBookNameToCraftingRecipeName.Key, craftingBookNameToCraftingRecipeName.Value);
            }
        }

        /// <summary>
        /// Adds a single crafting recipe to this blueprint when used.
        /// </summary>
        /// <param name="CraftingBookName"></param>
        /// <param name="CraftingRecipeName"></param>
        protected virtual void addCraftingRecipe(string CraftingBookName, string CraftingRecipeName)
        {
            this.craftingRecipesToUnlock.Add(CraftingBookName, CraftingRecipeName);
        }

        protected override void initNetFieldsPostConstructor()
        {
            base.initNetFieldsPostConstructor();
            this.NetFields.AddField(this.craftingRecipesToUnlock);
        }

        public override bool performUseAction(GameLocation location)
        {
            return this.learnRecipes();
        }


        protected virtual bool learnRecipes()
        {
            bool anyUnlocked = false;
            Dictionary<KeyValuePair<string, string>, bool> recipiesLearned = RevitalizeModCore.CraftingManager.learnCraftingRecipes(this.craftingRecipesToUnlock);

            foreach(var bookRecipePairToLearnedValues in recipiesLearned) {
                if (bookRecipePairToLearnedValues.Value == true)
                {
                    anyUnlocked = true;

                    Game1.drawObjectDialogue(string.Format("You learned how to make {0}! You can make it on a {1}. ", bookRecipePairToLearnedValues.Key.Value, Constants.ItemIds.Objects.CraftingStations.GetCraftingStationNameFromRecipeBookId(bookRecipePairToLearnedValues.Key.Key)));

                }
            }
            return anyUnlocked;
        }



        public override Item getOne()
        {
            Blueprint component = new Blueprint(this.basicItemInformation.Copy(), this.craftingRecipesToUnlock);
            return component;
        }

    }
}
