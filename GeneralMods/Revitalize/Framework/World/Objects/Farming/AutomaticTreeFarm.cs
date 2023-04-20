using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;
using Omegasis.Revitalize.Framework.World.WorldUtilities.Items;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewValley;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Omegasis.Revitalize.Framework.World.Objects.Machines;
using Omegasis.Revitalize.Framework.Utilities.Ranges;
using Omegasis.Revitalize.Framework.Utilities;

namespace Omegasis.Revitalize.Framework.World.Objects.Farming
{
    /// <summary>
    /// A machine that takes tree seeds and gives tree products as a result.
    /// </summary>
    [XmlType("Mods_Omegasis.Revitalize.Framework.World.Objects.Farming.AutomaticTreeFarm")]
    public class AutomaticTreeFarm : ItemRecipeDropInMachine
    {
        /// <summary>
        /// Used to determine how fast the speed bonus this preserve jar should have when processing goods.
        /// </summary>
        public readonly NetDouble processingSpeedMultiplierBonus = new NetDouble(1);

        public AutomaticTreeFarm()
        {

        }

        public AutomaticTreeFarm(BasicItemInformation Info, double ProcessingSpeedMultiplier) : this(Info, Vector2.Zero, ProcessingSpeedMultiplier)
        {

        }

        public AutomaticTreeFarm(BasicItemInformation Info, Vector2 TilePosition, double ProcessingSpeedMultiplier) : base(Info, TilePosition)
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
        public override List<ProcessingRecipe> getListOfPotentialRecipes(IList<Item> inputItems, Farmer who = null)
        {
            List<ProcessingRecipe> possibleRecipes = new List<ProcessingRecipe>();
            possibleRecipes.AddRange(base.getListOfPotentialRecipes(inputItems)); //Still allow getting recipes from recipe books and prefer those first.

            //Attempt to generate recipes automatically from items passed in.
            foreach (Item item in inputItems)
            {
                if (item == null) continue;

                ProcessingRecipe recipe = this.createProcessingRecipeFromItem(item, who);
                if (recipe != null)
                {
                    possibleRecipes.Add(recipe);
                }
            }
            return possibleRecipes;
        }



        public virtual ProcessingRecipe createProcessingRecipeFromItem(Item item, Farmer who = null)
        {
            ItemReference input = new ItemReference(item.getOne());
            if (!(item is StardewValley.Object))
            {
                return null;
            }

            string objectId = RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(item.ParentSheetIndex);


            List<LootTableEntry> outputs = new List<LootTableEntry>();


            outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.Sap), new IntRange(5, 5), new DoubleRange(0, 100)));
            outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.Wood), new IntRange(12, 16), new DoubleRange(0, 100)));

            outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.Hardwood), new List<IntOutcomeChanceDeterminer>() { new IntOutcomeChanceDeterminer(new DoubleRange(50, 75), 1) }, new DoubleRange(0, 100)));
            outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.Hardwood), new List<IntOutcomeChanceDeterminer>() { new IntOutcomeChanceDeterminer(new DoubleRange(75, 82), 2) }, new DoubleRange(0, 100)));
            outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.Hardwood), new List<IntOutcomeChanceDeterminer>() { new IntOutcomeChanceDeterminer(new DoubleRange(82, 88), 3) }, new DoubleRange(0, 100)));
            outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.Hardwood), new List<IntOutcomeChanceDeterminer>() { new IntOutcomeChanceDeterminer(new DoubleRange(88, 93), 4) }, new DoubleRange(0, 100)));
            outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.Hardwood), new List<IntOutcomeChanceDeterminer>() { new IntOutcomeChanceDeterminer(new DoubleRange(93, 97), 5) }, new DoubleRange(0, 100)));
            outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.Hardwood), new List<IntOutcomeChanceDeterminer>() { new IntOutcomeChanceDeterminer(new DoubleRange(97, 99), 7) }, new DoubleRange(0, 100)));
            outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.Hardwood), new List<IntOutcomeChanceDeterminer>() { new IntOutcomeChanceDeterminer(new DoubleRange(99, 100), 10) }, new DoubleRange(0, 100)));

            //Seeds have a chance of not being produced.
            IntRange stackSizeRange = new IntRange(0, 2);
            int stackSize = stackSizeRange.getRandomInclusive();

            int timeToGrow = GameTimeStamp.MinutesPerDay * 5;

            if (objectId.Equals(RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(Enums.SDVObject.PineCone)))
            {
                if (stackSize > 0)
                {
                    outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.PineCone), stackSize, new DoubleRange(0, 100)));
                }

                outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.PineTar), 1, new DoubleRange(0, 100)));

                return new ProcessingRecipe(input.RegisteredObjectId, new GameTimeStamp((int)(timeToGrow * this.processingSpeedMultiplierBonus.Value)), input, outputs);
            }

            if (objectId.Equals(RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(Enums.SDVObject.MapleSeed)))
            {
                if (stackSize > 0)
                {
                    outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.MapleSeed), stackSize, new DoubleRange(0, 100)));
                }

                outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.MapleSyrup), 1, new DoubleRange(0, 100)));

                return new ProcessingRecipe(input.RegisteredObjectId, new GameTimeStamp((int)(timeToGrow * this.processingSpeedMultiplierBonus.Value)), input, outputs);
            }

            if (objectId.Equals(RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(Enums.SDVObject.Acorn)))
            {

                if (stackSize > 0)
                {
                    outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.Acorn), stackSize, new DoubleRange(0, 100)));
                }

                outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.OakResin), 1, new DoubleRange(0, 100)));

                return new ProcessingRecipe(input.RegisteredObjectId, new GameTimeStamp((int)(timeToGrow * this.processingSpeedMultiplierBonus.Value)), input, outputs);
            }

            if (objectId.Equals(RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(Enums.SDVObject.MahoganySeed)))
            {
                outputs.Clear();
                outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.Sap), new IntRange(5, 5), new DoubleRange(0, 100)));
                outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.Hardwood), new IntRange(10, 10), new DoubleRange(0, 100)));

                if (stackSize > 0)
                {
                    outputs.Add(new LootTableEntry(new ItemReference(Enums.SDVObject.MahoganySeed), stackSize, new DoubleRange(0, 100)));
                }

                return new ProcessingRecipe(input.RegisteredObjectId, new GameTimeStamp((int)(timeToGrow * this.processingSpeedMultiplierBonus.Value)), input, outputs);
            }

            return null;
        }



        public override void playDropInSound()
        {
            SoundUtilities.PlaySound(Enums.StardewSound.Ship);
        }

        public override Item getOne()
        {
            return new AutomaticTreeFarm(this.basicItemInformation.Copy(), this.processingSpeedMultiplierBonus.Value);
        }

    }
}
