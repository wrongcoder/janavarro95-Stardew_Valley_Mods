using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.World.Objects.Machines;
using Pathoschild.Stardew.Automate;
using StardewValley;

namespace Omegasis.RevitalizeAutomateCompatibility.Objects.Machines
{
    public class MachineWrapper<T> : CustomObjectWrapper<T> where T : Machine
    {

        public MachineWrapper()
        {

        }

        /// <summary>
        /// Used to automate <see cref="CustomObject"/>s for the mod Revitalize.
        /// </summary>
        /// <param name="CustomObject"></param>
        public MachineWrapper(T CustomObject, GameLocation location, in Vector2 TileLocation) : base(CustomObject, location, TileLocation)
        {
        }

        /// <summary>
        /// Used to set the inputs for this machine to begin crafting.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool SetInput(IStorage input)
        {
            //Optimize to ensure that we don't do unnecessary logic for setting inputs.
            if (this.customObject.isWorking() || this.customObject.finishedProduction()) return false;

            foreach (ITrackedStack obj in input.GetItems())
            {
                Item item = obj.Sample;
                if (item == null) continue;
                item.Stack = obj.Count;

                CraftingResult result = this.customObject.processInput(item, null, false);
                if (result.successful)
                {
                    obj.Take(result.consumedItems[0].StackSize);
                    this.customObject.updateAnimation();
                    return true;
                }
            }
            this.customObject.updateAnimation();

            return false;
        }

        /// <summary>
        /// Gets the current state of this machine.
        /// </summary>
        /// <returns></returns>
        public override MachineState GetState()
        {
            if (this.customObject.isIdle())
                return MachineState.Empty;

            if (this.customObject.finishedProduction())
                return MachineState.Done;

            if (this.customObject.isWorking()) return MachineState.Processing;

            return this.customObject.MinutesUntilReady == 0
                ? MachineState.Done
                : MachineState.Processing;

            //Could use the following for machines that are always producing.
            /*
             *  return this.Machine.heldObject.Value != null
                ? MachineState.Done
                : MachineState.Processing;
             */
        }

    }
}
