using System;
using System.Collections.Generic;
using System.Linq;
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
using StardewValley;
using StardewValley.Locations;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines.ResourceGeneration
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.ResourceGeneration.MiningDrill")]
    public class MiningDrill : PoweredMachine
    {

        public readonly NetRef<ItemReference> itemToMine = new NetRef<ItemReference>();


        public MiningDrill()
        {

        }

        public MiningDrill(BasicItemInformation Info, PoweredMachineTier machineTier) : this(Info, Vector2.Zero, machineTier)
        {

        }

        public MiningDrill(BasicItemInformation Info, Vector2 TilePosition, PoweredMachineTier machineTier) : base(Info, TilePosition, machineTier)
        {
        }

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.itemToMine);
        }

        public override void doActualDayUpdateLogic(GameLocation location)
        {
            base.doActualDayUpdateLogic(location);

            if (this.itemToMine.Value != null)
            {
                this.heldObject.Value = (StardewValley.Object)this.itemToMine.Value.getItem();
                this.itemToMine.Value = null;
            }

            this.tryToRunMiningDrill();
        }

        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            this.updateAnimation();
            return base.minutesElapsed(minutes, environment);
        }

        /// <summary>
        /// Attempts to run the mining drill again to generate it's outputs for the next day.
        /// </summary>
        public virtual void tryToRunMiningDrill()
        {
            if (this.fuelChargesRemaining.Value > 0)
            {
                this.consumeFuelCharge();
                this.generateMiningOutput();
            }
            this.updateAnimation();
        }

        /// <summary>
        /// Performed when dropping an item into the mining drill.
        /// </summary>
        /// <param name="dropInItem"></param>
        /// <param name="probe"></param>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool performItemDropInAction(Item dropInItem, bool probe, Farmer who)
        {
            if (this.itemToMine.Value != null) return false;
            bool success = base.performItemDropInAction(dropInItem, probe, who);

            if (success)
            {
                this.tryToRunMiningDrill();
            }
            if (who != null && success)
            {
                SoundUtilities.PlaySound(Enums.StardewSound.Ship);
            }

            //If this is true, an extra battery pack is consumed, so we need to return false here.
            return success;
        }

        public override bool tryToIncreaseFuelCharges(Farmer who)
        {
            bool hasFuel = this.useFuelItemToIncreaseCharges(who, true, true);
            return hasFuel;
        }

        public override CraftingResult processInput(IList<Item> dropInItem, Farmer who, bool ShowRedMessage = true)
        {
            //Since we don't use a recipe book here, we need to return true so that the logic properly updates.
            return new CraftingResult(true);
        }

        public override void updateAnimation()
        {
            if (this.itemToMine.Value != null)
            {
                this.AnimationManager.playAnimation("Working");
            }
            else
            {
                this.AnimationManager.playDefaultAnimation();
            }
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


            this.itemToMine.Value = new ItemReference(potentialItems.GetRandom());

        }

        public override Item getOne()
        {
            return new MiningDrill(this.basicItemInformation.Copy(), this.machineTier.Value);
        }

        public override bool isWorking()
        {
            return this.itemToMine.Value != null;
        }
    }
}
