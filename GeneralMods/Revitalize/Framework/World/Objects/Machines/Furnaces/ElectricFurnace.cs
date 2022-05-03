using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Player;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines.Furnaces
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.Furnaces.ElectricFurnace")]
    public class ElectricFurnace : Machine
    {

        public readonly NetInt chargesRemaining = new NetInt();
        public readonly NetEnum<Enums.SDVObject> smeltingItem = new NetEnum<Enums.SDVObject>(Enums.SDVObject.NULL);

        public ElectricFurnace()
        {

        }


        public ElectricFurnace(BasicItemInformation info) : base(info)
        {
            this.createStatusBubble();
        }

        public ElectricFurnace(BasicItemInformation info, Vector2 TileLocation) : base(info, TileLocation)
        {
            this.createStatusBubble();
        }

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.chargesRemaining, this.smeltingItem);
        }

        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            base.minutesElapsed(minutes, environment);

            if (this.MinutesUntilReady == 0 && this.smeltingItem.Value != Enums.SDVObject.NULL && this.heldObject.Value==null)
            {
                if (this.smeltingItem.Value == Enums.SDVObject.CopperOre)
                {
                    this.heldObject.Value = new StardewValley.Object((int)Enums.SDVObject.CopperBar, 1);
                }
                if (this.smeltingItem.Value == Enums.SDVObject.IronOre)
                {
                    this.heldObject.Value = new StardewValley.Object((int)Enums.SDVObject.IronBar, 1);
                }
                if (this.smeltingItem.Value == Enums.SDVObject.GoldOre)
                {
                    this.heldObject.Value = new StardewValley.Object((int)Enums.SDVObject.GoldBar, 1);
                }
                if (this.smeltingItem.Value == Enums.SDVObject.IridiumOre)
                {
                    this.heldObject.Value = new StardewValley.Object((int)Enums.SDVObject.IridiumBar, 1);
                }
                if (this.smeltingItem.Value == Enums.SDVObject.RadioactiveOre)
                {
                    this.heldObject.Value = new StardewValley.Object((int)Enums.SDVObject.RadioactiveBar, 1);
                }
                if (this.smeltingItem.Value == Enums.SDVObject.Quartz)
                {
                    this.heldObject.Value = new StardewValley.Object((int)Enums.SDVObject.RefinedQuartz, 1);
                }
                if (this.smeltingItem.Value == Enums.SDVObject.FireQuartz)
                {
                    this.heldObject.Value = new StardewValley.Object((int)Enums.SDVObject.RefinedQuartz, 3);
                }
                if (this.smeltingItem.Value == Enums.SDVObject.Bouquet)
                {
                    this.heldObject.Value = new StardewValley.Object((int)Enums.SDVObject.WiltedBouquet, 1);
                }
                this.chargesRemaining.Value--;
            }

            return true;
        }

        public override bool rightClicked(Farmer who)
        {

            if (this.heldObject.Value != null)
                if (who.IsLocalPlayer)
                    this.cleanOutFurnace(true);

            return base.rightClicked(who);
        }

        /// <summary>
        /// Cleans out the hay maker to produce more hay.
        /// </summary>
        /// <param name="addToPlayersInventory"></param>
        protected virtual void cleanOutFurnace(bool addToPlayersInventory)
        {
            if (addToPlayersInventory)
            {
                SoundUtilities.PlaySound(Enums.StardewSound.coin);
                bool added = Game1.player.addItemToInventoryBool(this.heldObject.Value);
                if (added == false) return;
            }
            this.heldObject.Value = null;
            this.AnimationManager.playDefaultAnimation();
            this.smeltingItem.Value = Enums.SDVObject.NULL;

        }

        public override bool performObjectDropInAction(Item dropInItem, bool probe, Farmer who)
        {
            if (probe == true || this.MinutesUntilReady>0) return false;

            //Smelting times are about 25% faster than normal.
            if (dropInItem.ParentSheetIndex == (int)Enums.SDVObject.CopperOre)
            {
                if (dropInItem.Stack >= 5)
                {
                    return this.smeltItem(who, Enums.SDVObject.CopperOre, 5,TimeUtilities.GetMinutesFromTime(0,0,20));
                }
                else
                {
                    Game1.showRedMessage("Requires 5 ore!");
                }
            }

            if (dropInItem.ParentSheetIndex == (int)Enums.SDVObject.IronOre)
            {
                if (dropInItem.Stack >= 5)
                {
                    return this.smeltItem(who, Enums.SDVObject.IronOre, 5, TimeUtilities.GetMinutesFromTime(0, 1, 30));
                }
                else
                {
                    Game1.showRedMessage("Requires 5 ore!");
                }
            }

            if (dropInItem.ParentSheetIndex == (int)Enums.SDVObject.GoldOre)
            {
                if (dropInItem.Stack >= 5)
                {
                    return this.smeltItem(who, Enums.SDVObject.GoldOre, 5, TimeUtilities.GetMinutesFromTime(0, 3, 40));
                }
                else
                {
                    Game1.showRedMessage("Requires 5 ore!");
                }
            }

            if (dropInItem.ParentSheetIndex == (int)Enums.SDVObject.IridiumOre)
            {
                if (dropInItem.Stack >= 5)
                {
                    return this.smeltItem(who, Enums.SDVObject.IridiumOre, 5, TimeUtilities.GetMinutesFromTime(0, 6, 0));
                }
                else
                {
                    Game1.showRedMessage("Requires 5 ore!");
                }
            }

            if (dropInItem.ParentSheetIndex == (int)Enums.SDVObject.RadioactiveOre)
            {
                if (dropInItem.Stack >= 5)
                {
                    return this.smeltItem(who, Enums.SDVObject.RadioactiveOre, 5, TimeUtilities.GetMinutesFromTime(0, 7, 30));
                }
                else
                {
                    Game1.showRedMessage("Requires 5 ore!");
                }
            }

            if (dropInItem.ParentSheetIndex == (int)Enums.SDVObject.Quartz)
            {
                return this.smeltItem(who, Enums.SDVObject.Quartz, 1, TimeUtilities.GetMinutesFromTime(0, 1, 10));

            }


            if (dropInItem.ParentSheetIndex == (int)Enums.SDVObject.FireQuartz)
            {
                return this.smeltItem(who, Enums.SDVObject.FireQuartz, 1, TimeUtilities.GetMinutesFromTime(0, 1, 10));
            }

            if (dropInItem.ParentSheetIndex == (int)Enums.SDVObject.Bouquet)
            {
                return this.smeltItem(who, Enums.SDVObject.Bouquet, 1, TimeUtilities.GetMinutesFromTime(0, 0, 10));
            }


            return base.performObjectDropInAction(dropInItem, probe, who);
        }


        public virtual bool smeltItem(Farmer who, Enums.SDVObject sdvObjectToSmelt, int requiredAmount, int TimeUntilReady ,bool showRedMessage = true)
        {
            if (this.chargesRemaining.Value <= 0) {
                this.chargesRemaining.Value = 0;
            }

                bool success = this.chargesRemaining.Value==0 ? PlayerUtilities.ReduceInventoryItemIfEnoughFound(who, Enums.SDVObject.BatteryPack, 1) : true;
            
            if (success == false && showRedMessage)
            {
                Game1.showRedMessage("Need a battery pack to operate!");
                return false;
            }
            PlayerUtilities.ReduceInventoryItemIfEnoughFound(who, sdvObjectToSmelt, requiredAmount);
            this.smeltingItem.Value = sdvObjectToSmelt;
            this.AnimationManager.playAnimation("Working", true);
            this.MinutesUntilReady = TimeUntilReady;
            return false;

        }

        public override Item getOne()
        {
            return new ElectricFurnace(this.basicItemInformation.Copy());
        }



    }
}
