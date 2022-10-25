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
using Omegasis.Revitalize.Framework.Utilities.Extensions;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewValley;
using StardewValley.Locations;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines.ResourceGeneration
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.ResourceGeneration.MiningDrill")]
    public class MiningDrill : Machine
    {

        public readonly NetRef<StardewValley.Object> itemToMine = new NetRef<StardewValley.Object>();
        public readonly NetEnum<MachineTier> miningDrillTier = new NetEnum<MachineTier>();
        public readonly NetInt chargesRemaining = new NetInt(0);


        public MiningDrill()
        {

        }

        public MiningDrill(BasicItemInformation Info) : this(Info, Vector2.Zero)
        {

        }

        public MiningDrill(BasicItemInformation Info, Vector2 TilePosition) : base(Info, TilePosition)
        {
        }

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.itemToMine, this.miningDrillTier, this.chargesRemaining);
        }

        public override void doActualDayUpdateLogic(GameLocation location)
        {
            base.doActualDayUpdateLogic(location);



            if (this.itemToMine.Value != null)
            {
                this.heldObject.Value = this.itemToMine.Value;
                this.itemToMine.Value = null;
            }

            if (this.itemToMine.Value == null && this.miningDrillTier.Value != MachineTier.Magical && this.chargesRemaining.Value > 0)
            {
                this.chargesRemaining.Value--;
                this.generateMiningOutput();
            }

            if (this.itemToMine.Value == null && this.miningDrillTier.Value == MachineTier.Magical)
            {
                this.generateMiningOutput();
            }
        }

        public override bool rightClicked(Farmer who)
        {

            if (this.isReadyForHarvest())
                if (who.IsLocalPlayer)
                    this.getMachineOutput(true);

            return base.rightClicked(who);
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
            if (probe == true) return false; //Just checking for action.
            if (who.ActiveObject == null) return false;
            if (dropInItem == null) return false;
            if (this.itemToMine.Value != null) return false;
            if (this.heldObject.Value != null) return false;

            if (dropInItem.ParentSheetIndex == (int)Enums.SDVObject.BatteryPack && this.miningDrillTier.Value == MachineTier.Electric)
            {
                this.chargesRemaining.Value = 3;
                this.chargesRemaining.Value--;
                this.consumeFuelItemFromFarmersInventory(who);
                this.generateMiningOutput();
                return true;
            }
            if (dropInItem.ParentSheetIndex == (int)Enums.SDVObject.BatteryPack && this.miningDrillTier.Value == MachineTier.Electric)
            {
                this.chargesRemaining.Value = 14;
                this.chargesRemaining.Value--;
                this.consumeFuelItemFromFarmersInventory(who);
                this.generateMiningOutput();

                return true;
            }

            return false;
        }


        /// <summary>
        /// Attempts to consume the necessary fuel item from the player's inventory.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        protected virtual bool consumeFuelItemFromFarmersInventory(Farmer who)
        {
            if (who == null) return true; //Used for automate compatibility
            if (this.miningDrillTier.Value == MachineTier.Magical)
            {
                return true;
            }
            if (this.miningDrillTier.Value == MachineTier.Electric)
            {
                return PlayerUtilities.ReduceInventoryItemIfEnoughFound(who, Enums.SDVObject.BatteryPack, 1);
            }
            if (this.miningDrillTier.Value == MachineTier.Nuclear)
            {
                return PlayerUtilities.ReduceInventoryItemIfEnoughFound(who, MiscItemIds.RadioactiveFuel, 1);
            }
            return true;
            //Magical does not consume fuel.

        }




        public virtual void generateMiningOutput()
        {

            GameLocation objectLocation = this.getCurrentLocation();
            ObjectManager objectManager = RevitalizeModCore.ModContentManager.objectManager;

            bool playerHasMinerProfession = this.getOwner().getProfessionForSkill(Farmer.miningSkill, 5) == Farmer.miner;
            bool playerHasGeologistSkill = this.getOwner().getProfessionForSkill(Farmer.miningSkill, 5) == Farmer.geologist;

            bool playerHasProspectorProfession = this.getOwner().getProfessionForSkill(Farmer.miningSkill, 10) == Farmer.burrower;
            bool playerHasExcavatorProfession = this.getOwner().getProfessionForSkill(Farmer.miningSkill, 10) == Farmer.excavator;

            List<StardewValley.Object> potentialItems = new List<StardewValley.Object>();


            int stoneGiven = Game1.random.Next(5, 11);
            int clayGiven = Game1.random.Next(3, 6);
            int coalGiven = playerHasProspectorProfession ? Game1.random.Next(5, 11) : Game1.random.Next(3, 6);

            int oreYield = playerHasMinerProfession ? Game1.random.Next(4, 7) : Game1.random.Next(3, 6);
            int iridiumOreYield = playerHasMinerProfession ? Game1.random.Next(2, 4) : Game1.random.Next(1, 3);

            int typicalGeodeYield = playerHasExcavatorProfession ? Game1.random.Next(2, 7) : Game1.random.Next(1, 4);
            int omniGeodeYield = playerHasExcavatorProfession ? Game1.random.Next(1, 6) : Game1.random.Next(1, 4);

            int mineralYield = Game1.random.Next(1, 3);
            int gemYield = playerHasGeologistSkill ? Game1.random.Next(1, 3) : 1;

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
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.RadioactiveOre, 1));
                }
            }
            else if (GameLocationUtilities.IsLocationInSkullCaves(objectLocation))
            {
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Stone, Game1.random.Next(10, 16))); //Buff stone given since the area to place drills is small.
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
                    potentialItems.Add(objectManager.getObject(Enums.SDVObject.RadioactiveOre, 1));
                }
            }
            else if (GameLocationUtilities.IsLocationTheVolcanoDungeon(objectLocation))
            {
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.CinderShard, Game1.random.Next(1, 4)));

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

                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Stone, stoneGiven));
                potentialItems.Add(objectManager.getObject(Enums.SDVObject.Clay, clayGiven));
            }


            this.itemToMine.Value = potentialItems.GetRandom();

        }


        public virtual void getMachineOutput(bool AddToPlayersInventory)
        {

            if (AddToPlayersInventory)
            {
                SoundUtilities.PlaySound(Enums.StardewSound.coin);
                bool added = Game1.player.addItemToInventoryBool(this.heldObject.Value);
                if (added == false)
                {
                    WorldUtility.CreateItemDebrisAtTileLocation(this.getCurrentLocation(), this.heldObject.Value, this.TileLocation);
                    this.heldObject.Value = null;
                    return;
                }
            }
            else
            {
                WorldUtility.CreateItemDebrisAtTileLocation(this.getCurrentLocation(), this.heldObject.Value, this.TileLocation);
                this.heldObject.Value = null;
            }
        }

        public virtual Item getItemToMine(bool ClearValue = false)
        {
            Item item = this.itemToMine.Value;
            if (ClearValue)
            {
                this.itemToMine.Value = null;
            }
            return item;
        }

        public virtual bool isReadyForHarvest()
        {
            return this.getItemToMine() != null;
        }

        public override Item getOne()
        {
            return new MiningDrill(this.basicItemInformation.Copy());
        }

        /// <summary>What happens when the object is drawn at a tile location.</summary>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {

            x = (int)this.TileLocation.X;

            y = (int)this.TileLocation.Y;

            if (this.MinutesUntilReady > 0)
            {
                Vector2 origin = new Vector2(this.AnimationManager.getCurrentAnimationFrameRectangle().Width / 2, this.AnimationManager.getCurrentAnimationFrameRectangle().Height);

                this.basicItemInformation.animationManager.draw(spriteBatch, this.basicItemInformation.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((x + this.basicItemInformation.drawOffset.X) * Game1.tileSize) + this.basicItemInformation.shakeTimerOffset() + Game1.tileSize * origin.X / this.AnimationManager.getCurrentAnimationFrameRectangle().Width, (y + this.basicItemInformation.drawOffset.Y) * Game1.tileSize + this.basicItemInformation.shakeTimerOffset() + Game1.tileSize * (origin.Y / this.AnimationManager.getCurrentAnimationFrameRectangle().Height + 1))), new Rectangle?(this.AnimationManager.getCurrentAnimation().getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * alpha, 0f, origin, this.getScaleSizeForWorkingMachine(), this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (this.TileLocation.Y - this.basicItemInformation.drawOffset.Y) * Game1.tileSize / 10000f) + .00001f);
            }
            else
                this.basicItemInformation.animationManager.draw(spriteBatch, this.basicItemInformation.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((x + this.basicItemInformation.drawOffset.X) * Game1.tileSize) + this.basicItemInformation.shakeTimerOffset(), (y + this.basicItemInformation.drawOffset.Y) * Game1.tileSize + this.basicItemInformation.shakeTimerOffset())), new Rectangle?(this.AnimationManager.getCurrentAnimation().getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * alpha, 0f, Vector2.Zero, Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (this.TileLocation.Y - this.basicItemInformation.drawOffset.Y) * Game1.tileSize / 10000f) + .00001f);

            if (this.finishedProduction())
                this.drawStatusBubble(spriteBatch, x + (int)this.basicItemInformation.drawOffset.X, y + (int)this.basicItemInformation.drawOffset.Y, alpha);

        }

        public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
        {
            this.getMachineOutput(true);
            base.performRemoveAction(tileLocation, environment);
        }
    }
}
