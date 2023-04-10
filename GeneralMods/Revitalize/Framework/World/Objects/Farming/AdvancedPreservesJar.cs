using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;
using Omegasis.Revitalize.Framework.World.WorldUtilities.Items;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewValley.Tools;
using StardewValley;
using Omegasis.Revitalize.Framework.World.Objects.Machines;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace Omegasis.Revitalize.Framework.World.Objects.Farming
{
    /// <summary>
    /// An advanced preserves jar that can process goods faster than a normal preserves jar.
    /// </summary>
    [XmlType("Mods_Omegasis.Revitalize.Framework.World.Objects.Farming.AdvancedPreservesJar")]
    public class AdvancedPreservesJar :ItemRecipeDropInMachine
    {
        /// <summary>
        /// Used to determine how fast the speed bonus this preserve jar should have when processing goods.
        /// </summary>
        public readonly NetDouble processingSpeedMultiplierBonus = new NetDouble(1);

        public AdvancedPreservesJar()
        {

        }

        public AdvancedPreservesJar(BasicItemInformation Info, double ProcessingSpeedMultiplier) : this(Info, Vector2.Zero, ProcessingSpeedMultiplier)
        {

        }

        public AdvancedPreservesJar(BasicItemInformation Info, Vector2 TilePosition, double ProcessingSpeedMultiplier) : base(Info, TilePosition)
        {
            this.processingSpeedMultiplierBonus.Value = ProcessingSpeedMultiplier;
        }

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.processingSpeedMultiplierBonus);
        }

        public override CraftingResult processInput(IList<Item> inputItems, Farmer who, bool ShowRedMessage = true)
        {
            if (string.IsNullOrEmpty(this.getCraftingRecipeBookId()) || this.isWorking() || this.finishedProduction())
            {
                return new CraftingResult(false);
            }

            List<KeyValuePair<IList<Item>, ProcessingRecipe>> validRecipes = this.getListOfValidRecipes(inputItems, who, ShowRedMessage);

            if (validRecipes.Count > 0)
            {
                return this.onSuccessfulRecipeFound(validRecipes.First().Key, validRecipes.First().Value, who);
            }

            return new CraftingResult(false);
        }

        /// <summary>
        /// Generate the list of potential recipes based on the contents of the farmer's inventory.
        /// </summary>
        /// <param name="inputItems"></param>
        /// <returns></returns>
        public override List<ProcessingRecipe> getListOfPotentialRecipes(IList<Item> inputItems)
        {
            List<ProcessingRecipe> possibleRecipes = new List<ProcessingRecipe>();
            possibleRecipes.AddRange(base.getListOfPotentialRecipes(inputItems)); //Still allow getting recipes from recipe books and prefer those first.

            //Attempt to generate recipes automatically from items passed in.
            foreach (Item item in inputItems)
            {
                if (item == null) continue;

                ProcessingRecipe recipe= this.createProcessingRecipeFromItem(item);
                if (recipe != null)
                {
                    possibleRecipes.Add(recipe);
                }
            }
            return possibleRecipes;
        }



        public virtual ProcessingRecipe createProcessingRecipeFromItem(Item item)
        {
            ItemReference input = new ItemReference(item.getOne());
            int quality = 0;
            if (item is StardewValley.Object)
            {
                quality = (item as StardewValley.Object).Quality;
            }

            if (RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(item.ParentSheetIndex) == RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(Enums.SDVObject.Ginger) || item.Category == StardewValley.Object.VegetableCategory)
            {
                return new ProcessingRecipe(input.RegisteredObjectId, new GameTimeStamp((int)(4000 * this.processingSpeedMultiplierBonus.Value)), input, new LootTableEntry(new ItemReference(new ArtisanGoodItemReference(item.ParentSheetIndex, PreserveType.Pickle), 1, quality)));
            }

            if (item.Category == StardewValley.Object.FruitsCategory)
            {
                return new ProcessingRecipe(input.RegisteredObjectId, new GameTimeStamp((int)(4000 * this.processingSpeedMultiplierBonus.Value)), input, new LootTableEntry(new ItemReference(new ArtisanGoodItemReference(item.ParentSheetIndex, PreserveType.Jelly), 1, quality)));
            }

            //It's odd but Roe is considered a type of preserve good, while aged roe is what you get by processing it.
            ArtisanGoodItemReference typeOfRoeToCheck = new ArtisanGoodItemReference(item.ParentSheetIndex, PreserveType.Roe);
            ArtisanGoodItemReference sturgeonRoe = new ArtisanGoodItemReference(Enums.SDVObject.Sturgeon, PreserveType.Roe);


            if (typeOfRoeToCheck.equalsOtherArtisinalGood(sturgeonRoe))
            {
                return new ProcessingRecipe(input.RegisteredObjectId, new GameTimeStamp((int)(6000 * this.processingSpeedMultiplierBonus.Value)), input, new LootTableEntry(new ItemReference(Enums.SDVObject.Caviar,1,quality)));
            }
            if (RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(item.ParentSheetIndex) == RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(Enums.SDVObject.Roe))
            {
                return new ProcessingRecipe(input.RegisteredObjectId, new GameTimeStamp((int)(4000 * this.processingSpeedMultiplierBonus.Value)), input, new LootTableEntry(new ItemReference(new ArtisanGoodItemReference(item.ParentSheetIndex, PreserveType.AgedRoe), 1, quality)));
            }

            return null;
        }

        

        public override void playDropInSound()
        {
            SoundUtilities.PlaySound(Enums.StardewSound.Ship);
        }

        public override Item getOne()
        {
            return new AdvancedPreservesJar(this.basicItemInformation.Copy(), this.processingSpeedMultiplierBonus.Value);
        }
    }
}
