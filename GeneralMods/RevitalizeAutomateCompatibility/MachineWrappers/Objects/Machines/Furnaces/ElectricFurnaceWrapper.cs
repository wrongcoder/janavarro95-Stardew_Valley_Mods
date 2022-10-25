using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Items;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.World.Objects.Machines.Furnaces;
using Omegasis.RevitalizeAutomateCompatibility.Recipes;
using Pathoschild.Stardew.Automate;
using StardewValley;
using static Omegasis.Revitalize.Framework.World.Objects.Machines.Machine;

namespace Omegasis.RevitalizeAutomateCompatibility.MachineWrappers.Objects.Machines.Furnaces
{
    /// <summary>
    /// Wrapper class to add Automate compatibility for 
    /// </summary>
    public class ElectricFurnaceWrapper : CustomObjectMachineWrapper<ElectricFurnace>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ElectricFurnaceWrapper()
        {

        }

        /// <summary>
        /// Base item.
        /// </summary>
        /// <param name="furnace"></param>
        /// <param name="location"></param>
        /// <param name="TileLocation"></param>
        public ElectricFurnaceWrapper(ElectricFurnace furnace, GameLocation location, Vector2 TileLocation) : base(furnace, location, TileLocation)
        {

        }

        /// <summary>
        /// Used to set the inputs for this machine to begin crafting.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool SetInput(IStorage input)
        {
            bool fuelTypeFound = this.TryGetFuelItem(input, out IConsumable fuelType);

            if (fuelTypeFound)
            {
                foreach (ITrackedStack obj in input.GetItems())
                {
                    if (this.customObject.chargesRemaining.Value <= 0 && fuelType != null)
                    {
                        fuelType.Reduce();
                        this.customObject.increaseFuelCharges();
                    }
                    Item item = obj.Sample;
                    item.Stack = obj.Count;
                    CraftingRecipeResult smeltedRecipe=this.customObject.processItemFromRecipe(item, null, false);
                    if (smeltedRecipe.successful)
                    {
                        obj.Take(smeltedRecipe.recipe.ingredients[0].requiredAmount);
                        return true;
                    }
                }

            }

            return false;


        }

        /// <summary>
        /// Attempts to get a single fuel source item (or not if the furnace is magical) from a chest.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="consumable"></param>
        /// <returns></returns>
        public virtual bool TryGetFuelItem(IStorage input, out IConsumable? consumable)
        {
            if (this.customObject.furnaceType.Value == MachineTier.Magical)
            {
                consumable = null;
                return true;
            }

            if (this.customObject.chargesRemaining.Value > 0)
            {
                consumable = null;
                return true;
            }

            if (this.customObject.furnaceType.Value == MachineTier.Electric)
            {
                bool itemFound = input.TryGetVanillaIngredient(Enums.SDVObject.BatteryPack, 1, out IConsumable? batteryPack);
                consumable = batteryPack;
                return itemFound;
            }
            if (this.customObject.furnaceType.Value == MachineTier.Nuclear)
            {
                bool itemFound = input.TryGetRevitalizeItemIngredient(MiscItemIds.RadioactiveFuel, 1, out IConsumable? fuelCell);
                consumable = fuelCell;
                return itemFound;
            }
            consumable = null;
            return false;
        }

        /// <summary>Get the output item.</summary>
        public override ITrackedStack GetOutput()
        {
            //Returns a tracked object which is set to modify the machine's held object value to be null when complete.
            return new TrackedItem(this.customObject.heldObject.Value, onEmpty: item =>
            {
                this.customObject.cleanOutFurnace(false);
            });
        }


    }
}
