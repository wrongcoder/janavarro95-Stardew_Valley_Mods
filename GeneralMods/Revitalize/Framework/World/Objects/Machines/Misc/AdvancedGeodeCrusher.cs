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
using Omegasis.Revitalize.Framework.Constants.ItemIds.Items;
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
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.Misc.AdvancedGeodeCrusher")]
    public class AdvancedGeodeCrusher : PoweredMachine
    {

        public readonly NetRef<ItemReference> itemToReceive = new NetRef<ItemReference>();


        public AdvancedGeodeCrusher()
        {

        }

        public AdvancedGeodeCrusher(BasicItemInformation Info, PoweredMachineTier machineTier) : this(Info, Vector2.Zero, machineTier)
        {

        }

        public AdvancedGeodeCrusher(BasicItemInformation Info, Vector2 TilePosition, PoweredMachineTier machineTier) : base(Info, TilePosition, machineTier)
        {
        }

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.itemToReceive);
        }

        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            base.minutesElapsed(minutes, environment);

            if (this.MinutesUntilReady == 0 && this.itemToReceive.Value!=null)
            {
                this.heldObject.Value = (StardewValley.Object)this.itemToReceive.Value.getItem();
                this.itemToReceive.Value = null;
            }
            return true;
        }

        /// <summary>
        /// Performed when dropping in an object into the mining drill.
        /// </summary>
        /// <param name="dropInItem"></param>
        /// <param name="probe"></param>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool performObjectDropInAction(Item dropInItem, bool probe, Farmer who)
        {
            //Prevent overriding and destroying the previous operation.
            if (this.itemToReceive.Value != null) return false;
            if (this.heldObject.Value != null)
            {
                Game1.player.addItemToInventory(this.heldObject.Value);
                this.heldObject.Value = null;
            }
            bool success = base.performObjectDropInAction(dropInItem, probe, who) && this.useFuelItemToIncreaseCharges(who, false, true);
            bool generatedOutput = false;
            if (success)
            {
                if (this.fuelChargesRemaining.Value > 0)
                {
                    this.consumeFuelCharge();
                    generatedOutput = this.generateOutput(dropInItem);
                }
                this.updateAnimation();
            }
            if (generatedOutput)
            {
                if (who != null)
                {
                    Utility.addSmokePuff(who.currentLocation, this.tileLocation.Value * 64f + new Vector2(4f, -48f), 200);
                    Utility.addSmokePuff(who.currentLocation, this.tileLocation.Value * 64f + new Vector2(-16f, -56f), 300);
                    Utility.addSmokePuff(who.currentLocation, this.tileLocation.Value * 64f + new Vector2(16f, -52f), 400);
                    Utility.addSmokePuff(who.currentLocation, this.tileLocation.Value * 64f + new Vector2(32f, -56f), 200);
                    Utility.addSmokePuff(who.currentLocation, this.tileLocation.Value * 64f + new Vector2(40f, -44f), 500);
                    SoundUtilities.PlaySound(Enums.StardewSound.drumkit4);
                    SoundUtilities.PlaySound(Enums.StardewSound.stoneCrack);
                    SoundUtilities.PlaySoundWithDelay(Enums.StardewSound.steam, 200);
                }
                PlayerUtilities.ReduceInventoryItemStackSize(who, dropInItem, 1);
                return false;
            }
            return false;
        }

        public override void updateAnimation()
        {
            if (this.itemToReceive.Value != null)
            {
                this.AnimationManager.playAnimation(Machine.WORKING_ANIMATION_KEY);
            }
            else
            {
                this.AnimationManager.playDefaultAnimation();
            }
        }

        /// <summary>
        /// Generates a potential item to be produced for the next day.
        /// </summary>
        public virtual bool generateOutput(Item item, Farmer who = null)
        {

            GameLocation objectLocation = this.getCurrentLocation();
            if (objectLocation == null) return false;

            ObjectManager objectManager = RevitalizeModCore.ModContentManager.objectManager;

            bool playerHasMinerProfession = this.getOwner().getProfessionForSkill(Farmer.miningSkill, 5) == Farmer.miner;
            bool playerHasProspectorProfession = this.getOwner().getProfessionForSkill(Farmer.miningSkill, 10) == Farmer.burrower;

            List<StardewValley.Object> potentialItems = new List<StardewValley.Object>();

            int bonusForMachineTier = 0;
            if (this.machineTier.Value == PoweredMachineTier.Nuclear)
            {
                bonusForMachineTier = 1;
            }
            if (this.machineTier.Value == PoweredMachineTier.Magical)
            {
                bonusForMachineTier = 2;
            }

            int stoneBonus = Game1.random.Next(0, bonusForMachineTier * 5 + 1);
            int clayBonus = Game1.random.Next(1, 4 + bonusForMachineTier);
            int bonusCoalGiven = playerHasProspectorProfession ? Game1.random.Next(0, bonusForMachineTier * 3 + 1) : Game1.random.Next(0, bonusForMachineTier * 2 + 1);

            int bonusOreYield = playerHasMinerProfession ? Game1.random.Next(bonusForMachineTier, 3 + bonusForMachineTier * 3) : Game1.random.Next(bonusForMachineTier, 2 + bonusForMachineTier * 2);
            int iridiumOreYield = playerHasMinerProfession ? Game1.random.Next(0, 2 + bonusForMachineTier * 2) : Game1.random.Next(bonusForMachineTier, 1 + bonusForMachineTier);

            int mineralBonus = Game1.random.Next(1, 2 + bonusForMachineTier);

            //Common resources across all geodes.
            if (item.ParentSheetIndex == (int)Enums.SDVObject.Geode || item.ParentSheetIndex == (int)Enums.SDVObject.OmniGeode || item.ParentSheetIndex == (int)Enums.SDVObject.FrozenGeode || item.ParentSheetIndex == (int)Enums.SDVObject.MagmaGeode)
            {
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Clay, Game1.random.Next(1,2+clayBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.CopperOre, this.getStackSizeForResource() + bonusOreYield));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Coal, this.getStackSizeForResource() + bonusCoalGiven));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Stone, this.getStackSizeForResource() + stoneBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.IronOre, this.getStackSizeForResource() + bonusOreYield));

                Game1.stats.GeodesCracked++;
            }


            if (item.ParentSheetIndex == (int)Enums.SDVObject.Geode || item.ParentSheetIndex == (int)Enums.SDVObject.OmniGeode)
            {
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.DwarvishHelm, 1));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.EarthCrystal, Game1.random.Next(1, mineralBonus)));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Alamite, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Calcite, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Celestine, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Granite, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Jagoite, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Jamborite, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Limestone, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Malachite, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Mudstone, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Nekoite, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Orpiment, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.PetrifiedSlime, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Sandstone, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Slate, Game1.random.Next(1, mineralBonus)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.ThunderEgg, Game1.random.Next(1, mineralBonus)));

            }
            if (item.ParentSheetIndex == (int)Enums.SDVObject.FrozenGeode || item.ParentSheetIndex == (int)Enums.SDVObject.OmniGeode)
            {
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.AncientDrum, 1));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.FrozenTear, mineralBonus));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Aerinite, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Esperite, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.FairyStone, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Fluorapatite, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Geminite, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.GhostCrystal, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Hematite, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Kyanite, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Lunarite, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Marble, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.OceanStone, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Opal, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Pyrite, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Soapstone, mineralBonus));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.GoldOre, this.getStackSizeForResource() + bonusOreYield));

            }
            if (item.ParentSheetIndex == (int)Enums.SDVObject.MagmaGeode || item.ParentSheetIndex == (int)Enums.SDVObject.OmniGeode)
            {
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.DwarfGadget, 1));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.FireQuartz, mineralBonus));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Baryte, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Basalt, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Bixite, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Dolomite, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.FireOpal, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Helvite, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Jasper, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.LemonStone, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Neptunite, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Obsidian, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.StarShards, mineralBonus));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Tigerseye, mineralBonus));

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.IridiumOre, this.getStackSizeForRareResource() + iridiumOreYield));
            }
            if (item.ParentSheetIndex == (int)Enums.SDVObject.OmniGeode)
            {
                //Chance to get 2 primsatic shards from advanced geode crackers. Broken, but incentivises upgrading it.
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.PrismaticShard, Game1.random.Next(1, bonusForMachineTier > 0 ? 2 + (int)Math.Ceiling((float)bonusForMachineTier / 2f) : 2)));
            }
            if (item.ParentSheetIndex == (int)Enums.SDVObject.GoldenCoconut && (this.machineTier.Value == PoweredMachineTier.Nuclear || this.machineTier.Value == PoweredMachineTier.Magical))
            {

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.BananaSapling, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.MangoSapling, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.PineappleSeeds, Game1.random.Next(1, 4)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.TaroTuber, Game1.random.Next(1, 6)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.MahoganySeed, Game1.random.Next(1, 2 + bonusForMachineTier)));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.FossilizedSkull, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.IridiumOre, this.getStackSizeForRareResource() + iridiumOreYield));

            }
            if (item.ParentSheetIndex == (int)Enums.SDVObject.ArtifactTrove && (this.machineTier.Value == PoweredMachineTier.Nuclear || this.machineTier.Value == PoweredMachineTier.Magical))
            {
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Anchor, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.AncientDoll, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.AncientDrum, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.AncientSeed, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.AncientSword, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Arrowhead, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.BoneFlute, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.ChewingStick, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.ChickenStatue, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.ChippedAmphora, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.DriedStarfish, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.DwarfGadget, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.DwarvishHelm, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.ElvishJewelry, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.GlassShards, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.GoldenMask, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.GoldenPumpkin, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.GoldenRelic, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Anchor, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.OrnamentalFan, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Pearl, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.PrehistoricHandaxe, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.PrehistoricTool, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.RareDisc, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.RustyCog, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.RustySpoon, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.RustySpur, 1));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.TreasureChest, 1));
            }

            if (potentialItems.Count == 0)
            {
                return false;
            }

            this.itemToReceive.Value = new ItemReference(potentialItems.GetRandom());

            if (this.itemToReceive.Value != null)
            {

                //Coal isn't used but just in case.
                if (this.machineTier.Value == PoweredMachineTier.Coal)
                {
                    this.MinutesUntilReady = 30;
                }
                if (this.machineTier.Value == PoweredMachineTier.Electric)
                {
                    this.MinutesUntilReady = 30;
                }
                if (this.machineTier.Value == PoweredMachineTier.Nuclear)
                {
                    this.MinutesUntilReady = 10;
                }
                if (this.machineTier.Value == PoweredMachineTier.Magical)
                {
                    this.MinutesUntilReady = 10;
                }
                if (this.machineTier.Value == PoweredMachineTier.Galaxy)
                {
                    this.MinutesUntilReady = 0;
                    this.heldObject.Value =(StardewValley.Object)this.itemToReceive.Value.getItem();
                    this.itemToReceive.Value = null;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual int getStackSizeForResource()
        {
            int potentialAmountRNG = Game1.random.Next(1, 101);
            int potentialStackSize = 0;
            if (potentialAmountRNG >= 0 && potentialAmountRNG <= 29)
            {
                potentialStackSize = 1;
            }
            if (potentialAmountRNG >= 30 && potentialAmountRNG <= 59)
            {
                potentialStackSize = 3;
            }
            if (potentialAmountRNG >= 60 && potentialAmountRNG <= 89)
            {
                potentialStackSize = 5;
            }
            if (potentialAmountRNG >= 90 && potentialAmountRNG <= 99)
            {
                potentialStackSize = 10;
            }
            if (potentialAmountRNG == 100)
            {
                potentialStackSize = 20;
            }
            return potentialStackSize;
        }

        public virtual int getStackSizeForRareResource()
        {
            int potentialAmountRNG = Game1.random.Next(1, 101);
            int potentialStackSize = 0;
            if (potentialAmountRNG >= 0 && potentialAmountRNG <= 29)
            {
                potentialStackSize = 1;
            }
            if (potentialAmountRNG >= 30 && potentialAmountRNG <= 59)
            {
                potentialStackSize = 2;
            }
            if (potentialAmountRNG >= 60 && potentialAmountRNG <= 89)
            {
                potentialStackSize = 3;
            }
            if (potentialAmountRNG >= 90 && potentialAmountRNG <= 99)
            {
                potentialStackSize = 6;
            }
            if (potentialAmountRNG == 100)
            {
                potentialStackSize = 11;
            }
            return potentialStackSize;
        }

        public override Item getOne()
        {
            return new AdvancedGeodeCrusher(this.basicItemInformation.Copy(), this.machineTier.Value);
        }
    }
}
