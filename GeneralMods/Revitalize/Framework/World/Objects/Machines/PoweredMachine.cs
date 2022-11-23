using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Items;
using Omegasis.Revitalize.Framework.HUD;
using Omegasis.Revitalize.Framework.Player;
using Omegasis.Revitalize.Framework.Utilities.JsonContentLoading;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines
{
    /// <summary>
    /// Machines that are powered and used the machine tier system.
    /// </summary>
    ///
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.PoweredMachine")]
    public class PoweredMachine : Machine
    {
        public enum PoweredMachineTier
        {
            Coal,
            Electric,
            Nuclear,
            Magical,
            /// <summary>
            /// If ever implemented, this will have machines process instantly.
            /// </summary>
            Galaxy
        }

        public readonly NetEnum<PoweredMachineTier> machineTier = new NetEnum<PoweredMachineTier>();
        public readonly NetInt fuelChargesRemaining = new NetInt(0);


        public PoweredMachine()
        {

        }

        public PoweredMachine(BasicItemInformation info, PoweredMachineTier machineTier) : base(info)
        {
            this.createStatusBubble();
            this.machineTier.Value = machineTier;
        }

        public PoweredMachine(BasicItemInformation info, Vector2 TileLocation, PoweredMachineTier machineTier) : base(info, TileLocation)
        {
            this.createStatusBubble();
            this.machineTier.Value = machineTier;
        }

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.machineTier, this.fuelChargesRemaining);
        }

        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            base.minutesElapsed(minutes, environment);

            if (this.finishedProduction())
            {
                this.updateAnimation();
            }

            return true;
        }


        /// <summary>
        /// Consumes a single charge of fuel used on this funace.
        /// </summary>
        public virtual void consumeFuelCharge()
        {
            if (this.machineTier.Value == PoweredMachineTier.Magical) return;
            this.fuelChargesRemaining.Value--;
            if (this.fuelChargesRemaining.Value <= 0) this.fuelChargesRemaining.Value = 0;
        }


        public override bool rightClicked(Farmer who)
        {
            if (Game1.menuUp || Game1.currentMinigame != null) return false;
            if (this.finishedProduction() && who.IsLocalPlayer)
            {
                this.getMachineOutputs(true, false, true);
            }

            this.updateAnimation();
            return base.rightClicked(who);
        }

        /// <summary>
        /// Gets the output for this machine.
        /// </summary>
        /// <param name="AddToPlayersInventory">Attempts to add the items to the player's inventory, or to the ground if they can't pickup any more items.</param>
        /// <param name="DropAsItemDebris">Just drops the items to the ground as item debris.</param>
        /// <returns>The items produced by this machine.</returns>
        public virtual List<Item> getMachineOutputs(bool AddToPlayersInventory, bool DropAsItemDebris, bool ShowInventoryFullError)
        {
            List<Item> items = this.getMachineOutputItems(true);
            bool anyAdded = false;
            bool shouldShowInventoryFullError = false;
            foreach (Item item in items)
            {
                if (AddToPlayersInventory)
                {

                    bool added = Game1.player.addItemToInventoryBool(item);
                    if (added == false && DropAsItemDebris)
                    {
                        WorldUtility.CreateItemDebrisAtTileLocation(this.getCurrentLocation(), item, this.TileLocation);
                    }
                    else if (added == false && DropAsItemDebris == false)
                    {
                        shouldShowInventoryFullError = true;
                    }
                    else
                    {
                        anyAdded = true;
                    }
                    if (anyAdded)
                    {
                        SoundUtilities.PlaySound(Enums.StardewSound.coin);
                    }
                }
                if (DropAsItemDebris)
                {
                    WorldUtility.CreateItemDebrisAtTileLocation(this.getCurrentLocation(), item, this.TileLocation);
                }
            }

            if (shouldShowInventoryFullError && ShowInventoryFullError)
            {
                //Show inventory full error.
                HudUtilities.ShowInventoryFullErrorMessage();
            }

            return items;
        }

        /// <summary>
        /// Used for automate compatibility.
        /// </summary>
        /// <param name="ClearValue"></param>
        /// <returns></returns>
        public virtual Item getMachineOutputItem(bool ClearValue = false)
        {
            if (this.heldObject.Value == null) return null;
            Item item = this.heldObject.Value;
            if (ClearValue)
            {
                this.heldObject.Value = null;
            }
            return item;
        }

        public virtual List<Item> getMachineOutputItems(bool ClearValue = false)
        {
            if (this.heldObject.Value == null) return new List<Item>();
            return new List<Item>() { this.getMachineOutputItem(ClearValue) };
        }

        public override bool finishedProduction()
        {
            return this.getMachineOutputItem() != null && this.MinutesUntilReady == 0;
        }

        public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
        {
            this.getMachineOutputs(true, true, false);
            base.performRemoveAction(tileLocation, environment);
        }


        public virtual int getCoalFuelChargeIncreaseAmount()
        {
            return 3;
        }

        public virtual int getElectricFuelChargeIncreaseAmount()
        {
            return 3;
        }

        public virtual int getNuclearFuelChargeIncreaseAmount()
        {
            return 14;
        }

        public virtual int getMagicalFuelChargeIncreaseAmount()
        {
            return int.MaxValue;
        }

        public virtual int getGalaxyFuelChargeIncreaseAmount()
        {
            return int.MaxValue;
        }


        /// <summary>
        /// Increases the fuel type for the furnace. Public for automate compatibility
        /// </summary>
        public virtual void increaseFuelCharges()
        {
            if (this.machineTier.Value == PoweredMachineTier.Coal)
            {
                this.fuelChargesRemaining.Value = this.getCoalFuelChargeIncreaseAmount();
            }
            if (this.machineTier.Value == PoweredMachineTier.Electric)
            {
                this.fuelChargesRemaining.Value = this.getElectricFuelChargeIncreaseAmount();
            }
            if (this.machineTier.Value == PoweredMachineTier.Nuclear)
            {
                this.fuelChargesRemaining.Value = this.getNuclearFuelChargeIncreaseAmount();
            }
            if (this.machineTier.Value == PoweredMachineTier.Magical)
            {
                this.fuelChargesRemaining.Value = this.getMagicalFuelChargeIncreaseAmount();
            }
            if (this.machineTier.Value == PoweredMachineTier.Galaxy)
            {
                this.fuelChargesRemaining.Value = this.getGalaxyFuelChargeIncreaseAmount();
            }
        }

        /// <summary>
        /// Attempts to consume the necessary fuel item from the player's inventory.
        /// </summary>
        /// <param name="who"></param>
        /// <param name="RequireFuelToBeActiveObject">Forces the active object from the player to be the correct fuel type to prevent wasting fuel.</param>
        /// <returns></returns>
        protected virtual bool consumeFuelItemFromFarmersInventory(Farmer who, bool RequireFuelToBeActiveObject)
        {
            if (who == null)
            {
                return true; //Used for automate compatibility
            }
            if (this.machineTier.Value == PoweredMachineTier.Magical)
            {
                return true;
            }
            if (this.machineTier.Value == PoweredMachineTier.Coal)
            {
                if (RequireFuelToBeActiveObject)
                {
                    Item item = Game1.player.ActiveObject;
                    if (item.ParentSheetIndex != (int)Enums.SDVObject.Coal)
                    {
                        return false;
                    }
                }
                return PlayerUtilities.ReduceInventoryItemIfEnoughFound(who, Enums.SDVObject.Coal, 1);
            }
            if (this.machineTier.Value == PoweredMachineTier.Electric)
            {
                if (RequireFuelToBeActiveObject)
                {
                    Item item = Game1.player.ActiveObject;
                    if (item.ParentSheetIndex != (int)Enums.SDVObject.BatteryPack)
                    {
                        return false;
                    }
                }
                return PlayerUtilities.ReduceInventoryItemIfEnoughFound(who, Enums.SDVObject.BatteryPack, 1);
            }
            if (this.machineTier.Value == PoweredMachineTier.Nuclear)
            {
                if (RequireFuelToBeActiveObject)
                {
                    Item item = Game1.player.ActiveObject;
                    if (item is IBasicItemInfoProvider)
                    {
                        IBasicItemInfoProvider infoProvider = (IBasicItemInfoProvider)item;
                        //Check to see if the items can stack. If they can simply add them together and then continue on.
                        if (!infoProvider.Id.Equals(MiscItemIds.RadioactiveFuel))
                        {
                            return false;
                        }
                    }
                }
                return PlayerUtilities.ReduceInventoryItemIfEnoughFound(who, MiscItemIds.RadioactiveFuel, 1);
            }
            return true;
            //Magical does not consume fuel.
        }

        /// <summary>
        /// Tries to use the fuel item to increase fuel charges and consumes it in the same process.
        /// </summary>
        /// <param name="who"></param>
        /// <param name="ShowRedMessage"></param>
        /// <returns>True if successful or not necessaryt o consume a fuel charge. False if either the player is null (Automate Compatibility) or the player doesn't have enough fuel in their inventory.</returns>
        public virtual bool useFuelItemToIncreaseCharges(Farmer who, bool RequireFuelToBeActiveObject, bool ShowRedMessage = true)
        {
            if (this.fuelChargesRemaining > 0)
            {
                return true;
            }
            if (who != null)
            {
                //Make sure enough fuel is present for the furnace to operate (if necessary!)
                bool playerHasItemInInventory = this.consumeFuelItemFromFarmersInventory(who, RequireFuelToBeActiveObject);

                if (playerHasItemInInventory == false && ShowRedMessage)
                {
                    if (ShowRedMessage)
                    {
                        this.showRedMessageForMissingFuel();
                    }
                    return false;
                }
                this.increaseFuelCharges();
                return true;
            }
            return false;
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
            if (who != null && who.ActiveObject == null) return false;
            if (dropInItem == null) return false;
            if (this.heldObject.Value != null) return false;

            return true;
        }

        /// <summary>
        /// Updates the animation manager to play the correct animation.
        /// </summary>
        public virtual void updateAnimation()
        {
            this.AnimationManager.playDefaultAnimation();
        }


        /// <summary>
        /// Shows an error message if there is no correct fuel present for the furnace.
        /// </summary>
        protected virtual void showRedMessageForMissingFuel()
        {
            if (this.machineTier.Value == PoweredMachineTier.Coal)
            {
                Game1.showRedMessage(JsonContentLoaderUtilities.LoadErrorString(this.getErrorStringFile(), "NeedCoal"));
                return;
            }
            if (this.machineTier.Value == PoweredMachineTier.Electric)
            {
                Game1.showRedMessage(JsonContentLoaderUtilities.LoadErrorString(this.getErrorStringFile(), "NeedBatteryPack"));
                return;
            }
            if (this.machineTier.Value == PoweredMachineTier.Nuclear)
            {
                Game1.showRedMessage(JsonContentLoaderUtilities.LoadErrorString(this.getErrorStringFile(), "NeedNuclearFuel"));
                return;
            }
            Game1.showRedMessage(JsonContentLoaderUtilities.LoadErrorString(this.getErrorStringFile(), "MagicalFurnaceFuelError"));
            return;
        }

        /// <summary>
        /// Gets the relative path to the file to load the error strings from the ErrorStrings directory.
        /// </summary>
        /// <returns></returns>
        public virtual string getErrorStringFile()
        {
            return Path.Combine("Objects", "Machines", "CommonErrorStrings.json");
        }

        public override Item getOne()
        {
            return new PoweredMachine(this.basicItemInformation.Copy(), this.machineTier.Value);
        }
    }
}
