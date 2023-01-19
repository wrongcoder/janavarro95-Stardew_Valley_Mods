using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.Player;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.Utilities.Extensions;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using Omegasis.Revitalize.Framework.World.WorldUtilities.Items;
using StardewValley;
using StardewValley.Locations;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines.ResourceGeneration
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.ResourceGeneration.MiningDrill")]
    public class MiningDrill : PoweredMachine
    {


        public MiningDrill()
        {

        }

        public MiningDrill(BasicItemInformation Info, PoweredMachineTier machineTier) : this(Info, Vector2.Zero, machineTier)
        {

        }

        public MiningDrill(BasicItemInformation Info, Vector2 TilePosition, PoweredMachineTier machineTier) : base(Info, TilePosition, machineTier)
        {
        }

        public override void doActualDayUpdateLogic(GameLocation location)
        {
            base.doActualDayUpdateLogic(location);
            if (this.hasFuel())
            {
                this.generateMiningOutput();
                this.updateAnimation();
                this.consumeFuelCharge();
            }
        }

        public override CraftingResult processInput(IList<Item> dropInItem, Farmer who, bool ShowRedMessage = true)
        {
            //Since we don't use a recipe book here, we need to return true so that the logic properly updates.
            //return new CraftingResult(true);


            if (string.IsNullOrEmpty(this.getCraftingRecipeBookId()) || this.isWorking() || this.finishedProduction())
            {
                return new CraftingResult(false);
            }

            List<KeyValuePair<IList<Item>, ProcessingRecipe>> validRecipes = this.getListOfValidRecipes(dropInItem, who, ShowRedMessage);

            if (validRecipes.Count > 0)
            {
                int randElement = Game1.random.Next(validRecipes.Count);
                return this.onSuccessfulRecipeFound(validRecipes.ElementAt(randElement).Key, validRecipes.ElementAt(randElement).Value, who);
            }

            return new CraftingResult(false);
        }

        public override void playDropInSound()
        {
            SoundUtilities.PlaySound(Enums.StardewSound.Ship);
        }

        public override int getElectricFuelChargeIncreaseAmount()
        {
            return 3;
        }

        public override int getNuclearFuelChargeIncreaseAmount()
        {
            return this.getElectricFuelChargeIncreaseAmount() * 10;
        }

        /// <summary>
        /// Generates a potential item to be produced for the next day.
        /// </summary>
        public virtual void generateMiningOutput()
        {

            GameLocation objectLocation = this.getCurrentLocation();
            if (objectLocation == null) return;
            ObjectManager objectManager = RevitalizeModCore.ModContentManager.objectManager;

            bool playerHasMinerProfession = this.getOwner().getProfessionForSkill(Farmer.miningSkill, 5) == Farmer.miner;
            bool playerHasGeologistSkill = this.getOwner().getProfessionForSkill(Farmer.miningSkill, 5) == Farmer.geologist;

            bool playerHasProspectorProfession = this.getOwner().getProfessionForSkill(Farmer.miningSkill, 10) == Farmer.burrower;
            bool playerHasExcavatorProfession = this.getOwner().getProfessionForSkill(Farmer.miningSkill, 10) == Farmer.excavator;

            List<StardewValley.Object> potentialItems = new List<StardewValley.Object>();

            int bonusForMiningDrillTier = 0;
            if (this.machineTier.Value == PoweredMachineTier.Coal)
            {
                bonusForMiningDrillTier = 0;
            }
            if (this.machineTier.Value == PoweredMachineTier.Electric)
            {
                bonusForMiningDrillTier = 1;
            }
            if (this.machineTier.Value == PoweredMachineTier.Nuclear)
            {
                bonusForMiningDrillTier = 2;
            }
            if (this.machineTier.Value == PoweredMachineTier.Magical)
            {
                bonusForMiningDrillTier = 3;
            }
            if (this.machineTier.Value == PoweredMachineTier.Galaxy)
            {
                bonusForMiningDrillTier = 4;
            }

            int stoneGiven = Game1.random.Next(5, 11 + bonusForMiningDrillTier * 2);
            int clayGiven = Game1.random.Next(3, 6 + bonusForMiningDrillTier);
            int coalGiven = (playerHasProspectorProfession ? Game1.random.Next(5, 11 + Math.Max(0, bonusForMiningDrillTier - 1)) : Game1.random.Next(3, 6 + Math.Max(0, bonusForMiningDrillTier - 1)));
            int woodGiven = Game1.random.Next(3, 9) + bonusForMiningDrillTier / 2;

            int oreYield = playerHasMinerProfession ? Game1.random.Next(4, 7 + bonusForMiningDrillTier / 2) : Game1.random.Next(3, 6 + bonusForMiningDrillTier / 2);
            int iridiumOreYield = playerHasMinerProfession ? Game1.random.Next(2, 4 + bonusForMiningDrillTier / 2) : Game1.random.Next(1, 3 + bonusForMiningDrillTier / 2);

            int typicalGeodeYield = playerHasExcavatorProfession ? Game1.random.Next(2, 7 + bonusForMiningDrillTier / 2) : Game1.random.Next(1, 4 + bonusForMiningDrillTier / 2);
            int omniGeodeYield = playerHasExcavatorProfession ? Game1.random.Next(1, 6 + bonusForMiningDrillTier / 2) : Game1.random.Next(1, 4 + bonusForMiningDrillTier / 2);

            int mineralYield = Game1.random.Next(1, 3 + bonusForMiningDrillTier / 2);
            int gemYield = playerHasGeologistSkill ? Game1.random.Next(1, 3 + bonusForMiningDrillTier / 2) : Game1.random.Next(1, 2 + bonusForMiningDrillTier / 2);

            if (GameLocationUtilities.IsLocationTheEntranceToTheMines(objectLocation))
            {
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Stone, stoneGiven));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Clay, clayGiven));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.CopperOre, oreYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Geode, typicalGeodeYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Coal, coalGiven));

            }
            else if (GameLocationUtilities.IsLocationInTheMines(objectLocation))
            {
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Quartz, mineralYield));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Stone, stoneGiven));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Coal, coalGiven));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.OmniGeode, omniGeodeYield));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Amethyst, gemYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Aquamarine, gemYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Emerald, gemYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Jade, gemYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Ruby, gemYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Topaz, gemYield));

                int floorLevel = GameLocationUtilities.CurrentMineLevel(objectLocation);



                if (floorLevel <= 39)
                {
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.CopperOre, oreYield));
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.Geode, typicalGeodeYield));
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.EarthCrystal, mineralYield));
                }
                if (floorLevel >= 50)
                {
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.Diamond, gemYield));
                }

                if (floorLevel >= 40 && floorLevel <= 80)
                {
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.IronOre, oreYield));
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.FrozenGeode, typicalGeodeYield));
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.FrozenTear, mineralYield));
                }
                if (floorLevel >= 80)
                {
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.GoldOre, oreYield));
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.MagmaGeode, typicalGeodeYield));
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.FireQuartz, mineralYield));
                }

                if (Game1.player.hasOrWillReceiveMail("reachedBottomOfHardMines"))
                {
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.RadioactiveOre, Game1.random.Next(1, 2 + +Math.Min(0, bonusForMiningDrillTier - 2))));
                }
            }
            else if (GameLocationUtilities.IsLocationInSkullCaves(objectLocation))
            {
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Stone, Game1.random.Next(10, 16 + bonusForMiningDrillTier * 2))); //Buff stone given since the area to place drills is small.
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Coal, coalGiven));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Amethyst, gemYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Aquamarine, gemYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Emerald, gemYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Jade, gemYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Ruby, gemYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Topaz, gemYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Diamond, gemYield));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.CopperOre, oreYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.IronOre, oreYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.GoldOre, oreYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.IridiumOre, iridiumOreYield));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.OmniGeode, omniGeodeYield));

                if (Game1.player.hasOrWillReceiveMail("reachedBottomOfHardMines"))
                {
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.RadioactiveOre, Game1.random.Next(1, 2 + Math.Min(0, bonusForMiningDrillTier - 2))));
                }
            }
            else if (GameLocationUtilities.IsLocationTheVolcanoDungeon(objectLocation))
            {
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.CinderShard, Game1.random.Next(1, 4 + bonusForMiningDrillTier / 2)));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Ruby, gemYield));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.CopperOre, oreYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.IronOre, oreYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.GoldOre, oreYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.IridiumOre, iridiumOreYield));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.OmniGeode, omniGeodeYield));
            }

            else
            {
                //Maybe eventually decide to add in resources for the hilltop and maybe the 4 corners farm?

                //Small easter egg. When using a mining drill on wooden floors, the player can get some wood as the output.
                if (ObjectUtilities.GetFloorType(this) == Enums.FloorType.Wood)
                {
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.Wood, woodGiven));
                }
                else
                {
                    //General outputs if not used in a proper location.
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.Stone, stoneGiven));
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.Clay, clayGiven));
                }


            }

        }

        public override List<KeyValuePair<IList<Item>, ProcessingRecipe>> getListOfValidRecipes(IList<Item> inputItems, Farmer who, bool ShowRedMessage = true)
        {
            GameLocation objectLocation = this.getCurrentLocation();
            List<KeyValuePair<IList<Item>, ProcessingRecipe>> validRecipes = new();
            if (objectLocation == null) return validRecipes;


            //Load different recipes depending on the different conditions.
            if (GameLocationUtilities.IsLocationTheEntranceToTheMines(objectLocation))
            {
                foreach (ProcessingRecipe recipe in RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.getProcessingRecipesForObject(this.getCraftingRecipeBookId() + "_MinesEntrance"))
                {
                    validRecipes.Add(new KeyValuePair<IList<Item>, ProcessingRecipe>(new List<Item>(), recipe));
                }
            }
            else if (GameLocationUtilities.IsLocationInTheMines(objectLocation))
            {


                int floorLevel = GameLocationUtilities.CurrentMineLevel(objectLocation);


                foreach (ProcessingRecipe recipe in RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.getProcessingRecipesForObject(this.getCraftingRecipeBookId() + "_Mines"))
                {
                    validRecipes.Add(new KeyValuePair<IList<Item>, ProcessingRecipe>(new List<Item>(), recipe));
                }

                if (floorLevel <= 39)
                {
                    foreach (ProcessingRecipe recipe in RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.getProcessingRecipesForObject(this.getCraftingRecipeBookId() + "_Mines_0-39"))
                    {
                        validRecipes.Add(new KeyValuePair<IList<Item>, ProcessingRecipe>(new List<Item>(), recipe));
                    }

                }
                if (floorLevel >= 50)
                {
                    foreach (ProcessingRecipe recipe in RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.getProcessingRecipesForObject(this.getCraftingRecipeBookId() + "_Mines_50+"))
                    {
                        validRecipes.Add(new KeyValuePair<IList<Item>, ProcessingRecipe>(new List<Item>(), recipe));
                    }
                }

                if (floorLevel >= 40 && floorLevel <= 80)
                {
                    foreach (ProcessingRecipe recipe in RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.getProcessingRecipesForObject(this.getCraftingRecipeBookId() + "_Mines_40_80"))
                    {
                        validRecipes.Add(new KeyValuePair<IList<Item>, ProcessingRecipe>(new List<Item>(), recipe));
                    }
                }
                if (floorLevel >= 80)
                {
                    foreach (ProcessingRecipe recipe in RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.getProcessingRecipesForObject(this.getCraftingRecipeBookId() + "_Mines_80+"))
                    {
                        validRecipes.Add(new KeyValuePair<IList<Item>, ProcessingRecipe>(new List<Item>(), recipe));
                    }
                }

                if (Game1.player.hasOrWillReceiveMail("reachedBottomOfHardMines"))
                {
                    foreach (ProcessingRecipe recipe in RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.getProcessingRecipesForObject(this.getCraftingRecipeBookId() + "_ReachedBottomOfHardMines"))
                    {
                        validRecipes.Add(new KeyValuePair<IList<Item>, ProcessingRecipe>(new List<Item>(), recipe));
                    }
                }
            }
            else if (GameLocationUtilities.IsLocationInSkullCaves(objectLocation))
            {
                foreach (ProcessingRecipe recipe in RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.getProcessingRecipesForObject(this.getCraftingRecipeBookId() + "_SkullCavern"))
                {
                    validRecipes.Add(new KeyValuePair<IList<Item>, ProcessingRecipe>(new List<Item>(), recipe));
                }

                if (Game1.player.hasOrWillReceiveMail("reachedBottomOfHardMines"))
                {
                    foreach (ProcessingRecipe recipe in RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.getProcessingRecipesForObject(this.getCraftingRecipeBookId() + "_ReachedBottomOfHardMines"))
                    {
                        validRecipes.Add(new KeyValuePair<IList<Item>, ProcessingRecipe>(new List<Item>(), recipe));
                    }
                }
            }
            else if (GameLocationUtilities.IsLocationTheVolcanoDungeon(objectLocation) || GameLocationUtilities.IsLocationTheCaldera(objectLocation))
            {
                foreach (ProcessingRecipe recipe in RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.getProcessingRecipesForObject(this.getCraftingRecipeBookId() + "_VolcanoDungeon"))
                {
                    validRecipes.Add(new KeyValuePair<IList<Item>, ProcessingRecipe>(new List<Item>(), recipe));
                }
            }

            else
            {
                //Maybe eventually decide to add in resources for the hilltop and maybe the 4 corners farm?

                //Small easter egg. When using a mining drill on wooden floors, the player can get some wood as the output.
                if (ObjectUtilities.GetFloorType(this) == Enums.FloorType.Wood)
                {
                    foreach (ProcessingRecipe recipe in RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.getProcessingRecipesForObject(this.getCraftingRecipeBookId() + "_EasterEggs_WoodFloor"))
                    {
                        validRecipes.Add(new KeyValuePair<IList<Item>, ProcessingRecipe>(new List<Item>(), recipe));
                    }
                }
                else
                {
                    foreach (ProcessingRecipe recipe in RevitalizeModCore.ModContentManager.objectProcessingRecipesManager.getProcessingRecipesForObject(this.getCraftingRecipeBookId() + "_UndeterminedLocation"))
                    {
                        validRecipes.Add(new KeyValuePair<IList<Item>, ProcessingRecipe>(new List<Item>(), recipe));
                    }
                }


            }


            return validRecipes;
        }

        public override Item getOne()
        {
            return new MiningDrill(this.basicItemInformation.Copy(), this.machineTier.Value);
        }
    }
}
