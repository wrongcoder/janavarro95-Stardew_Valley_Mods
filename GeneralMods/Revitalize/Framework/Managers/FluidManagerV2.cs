using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Revitalize.Framework.Managers
{
    /// <summary>
    /// A Fluid used for various mod purposes.
    /// </summary>
    public class Fluid
    {
        /// <summary>
        /// The name of the Fluid.
        /// </summary>
        public string name;
        /// <summary>
        /// The color for the Fluid.
        /// </summary>
        public Color color;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Fluid()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="FluidColor"></param>
        public Fluid(string Name, Color FluidColor)
        {
            this.name = Name;
            this.color = FluidColor;
        }

        /// <summary>
        /// Fluid comparison check to see if two Fluids are the same.
        /// </summary>
        /// <param name="Other"></param>
        /// <returns></returns>
        public bool isFluidHomogenous(Fluid Other)
        {
            if (this.name.Equals(Other.name)) return true;
            return false;
        }

        /// <summary>
        /// Copys over the Fluid.
        /// </summary>
        /// <returns></returns>
        public Fluid Copy()
        {
            return new Fluid(this.name, this.color);
        }
    }

    public class MachineFluidTank
    {
        /// <summary>
        /// The Fluid inside of the tank.
        /// </summary>
        public Fluid fluid;
        /// <summary>
        /// How much Fluid is inside the tank currently.
        /// </summary>
        public int amount;
        /// <summary>
        /// How much Fluid the tank can hold.
        /// </summary>
        public int capacity;

        /// <summary>
        /// The remaining capacity on the tank.
        /// </summary>
        
        public int remainingCapacity
        {
            get
            {
                return this.capacity - this.amount;
            }
        }

        /// <summary>
        /// Checks to see if this tank is full.
        /// </summary>
        public bool IsFull
        {
            get
            {
                return this.amount == this.capacity;
            }
        }

        /// <summary>
        /// Checks if there is fluid inside the tank.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.amount == 0;
            }
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public MachineFluidTank()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Capacity"></param>
        public MachineFluidTank(int Capacity)
        {
            this.capacity = Capacity;
            this.amount = 0;
            this.fluid = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Capacity"></param>
        /// <param name="Amount"></param>
        /// <param name="Fluid"></param>
        public MachineFluidTank(int Capacity, int Amount, Fluid Fluid)
        {
            this.capacity = Capacity;
            this.amount = Amount;
            this.fluid = Fluid;
        }

        /// <summary>
        /// Checks to see if this tank can recieve this Fluid.
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public bool CanRecieveThisFluid(Fluid L)
        {
            if (this.IsFull) return false;
            if (this.fluid == null) return true;
            if (this.fluid.isFluidHomogenous(L)) return true;
            if (this.IsEmpty) return true;
            else return false;
        }

        /// <summary>
        /// Takes in Fluid into this tank.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="Amount"></param>
        public void intakeFluid(Fluid L, int Amount)
        {
            if (this.CanRecieveThisFluid(L))
            {
                if (this.fluid == null) this.fluid = L.Copy();
                else
                {
                    int intakeAmount=Math.Min(this.remainingCapacity, Amount);
                    this.amount = this.amount + intakeAmount;
                }
            }
            else return;
        }
        /// <summary>
        /// Consumes, aka reduces the internal Fluid on this tank by the amount given or the amount remaining in the tank.
        /// </summary>
        /// <param name="Amount"></param>
        public void consumeFluid(int Amount)
        {
            if (this.IsEmpty) return;
            if (this.fluid == null) return;
            int consumeAmount = Math.Min(this.amount, Amount);
            this.amount = this.amount - consumeAmount;
            if (this.amount <= 0) this.fluid = null;
        }

        /// <summary>
        /// Checks to see if this tank has enough 
        /// </summary>
        /// <param name="L"></param>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public bool DoesThisTankHaveEnoughFluid(Fluid L, int Amount)
        {
            if (this.GetAmountOfFluidInThisTank(L) >= Amount) return true;
            return false;
        }

        /// <summary>
        /// Drains the tank completly
        /// </summary>
        public void emptyTank()
        {
            this.fluid = null;
            this.amount = 0;
        }

        /// <summary>
        /// Gets the amount of Fluid in this tank for the given Fluid.
        /// </summary>
        /// <param name="L"></param>
        /// <returns> Returns 0 if the tank doesn't contain Fluid of the same type. Otherwise returns the amount stored in the tank.</returns>
        public int GetAmountOfFluidInThisTank(Fluid L)
        {
            if (this.fluid == null) return 0;
            if (this.fluid.isFluidHomogenous(L)) return this.amount;
            return 0;
        }

        /// <summary>
        /// Gets the amount of Fluid this take can take in in acordance with the parameter Fluid.
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public int GetAmountOfFluidThisTankCanReceieve(Fluid L)
        {
            if (this.fluid == null) return this.capacity;
            if (this.fluid.isFluidHomogenous(L)) return this.remainingCapacity;
            return 0;
        }

        /// <summary>
        /// Checks to see if this tank contains this Fluid at all.
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public bool DoesTankContainThisFluid(Fluid L)
        {
            if (this.fluid == null) return false;
            if (this.fluid.isFluidHomogenous(L)) return true;
            return false;
        }

    }
    public class FluidManagerV2
    {
        public MachineFluidTank inputTank1;
        public MachineFluidTank inputTank2;
        public MachineFluidTank outputTank;

        public bool needsUpdate;

        /// <summary>
        /// Does this machine allow the same fluid in both tanks?
        /// </summary>
        public bool allowDoubleInput;

        public FluidManagerV2()
        {
            this.inputTank1 = new MachineFluidTank(0);
            this.inputTank2 = new MachineFluidTank(0);
            this.outputTank = new MachineFluidTank(0);
            this.needsUpdate = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Capacity"></param>
        /// <param name="OnlyOutput"></param>
        /// <param name="AllowDoubleInput">Can both input tanks store the same Fluid?</param>
        public FluidManagerV2(int Capacity, bool OnlyOutput, bool AllowDoubleInput=false)
        {
            if (OnlyOutput)
            {
                this.outputTank = new MachineFluidTank(Capacity);
                this.inputTank1 = new MachineFluidTank(0);
                this.inputTank2 = new MachineFluidTank(0);

            }
            else
            {
                this.outputTank = new MachineFluidTank(Capacity);
                this.inputTank1 = new MachineFluidTank(Capacity);
                this.inputTank2 = new MachineFluidTank(Capacity);
            }
            this.allowDoubleInput = AllowDoubleInput;
            this.needsUpdate = false;
        }

        /// <summary>
        /// Produces a given amount of Fluid and puts it into the output tank for this Fluid manager.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="Amount"></param>
        public void produceFluid(Fluid L, int Amount)
        {
            if (this.outputTank.CanRecieveThisFluid(L))
            {
                this.outputTank.intakeFluid(L, Amount);
                this.needsUpdate = true;
            }
        }

        /// <summary>
        /// Intakes Fluid into the input takes on this Fluid manager.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="Amount"></param>
        public void intakeFluid(Fluid L, int Amount)
        {
            int remainingAmount = Amount;
            if (this.allowDoubleInput)
            {
                if (this.inputTank1.CanRecieveThisFluid(L) && remainingAmount>0)
                {
                    int allowedAmount = this.inputTank1.remainingCapacity;
                    this.inputTank1.intakeFluid(L, remainingAmount);
                    remainingAmount -= allowedAmount;
                }
                if (this.inputTank2.CanRecieveThisFluid(L)&& remainingAmount>0)
                {
                    int allowedAmount = this.inputTank2.remainingCapacity;
                    this.inputTank2.intakeFluid(L, remainingAmount);
                    remainingAmount -= allowedAmount;
                }
                this.needsUpdate = true;
            }
            else
            {

                if (this.inputTank1.CanRecieveThisFluid(L) && remainingAmount > 0 && this.inputTank2.DoesTankContainThisFluid(L)==false)
                {
                    int allowedAmount = this.inputTank1.remainingCapacity;
                    this.inputTank1.intakeFluid(L, remainingAmount);
                    remainingAmount -= allowedAmount;
                    this.needsUpdate = true;
                    return;
                }
                if (this.inputTank2.CanRecieveThisFluid(L) && remainingAmount > 0 && this.inputTank1.DoesTankContainThisFluid(L) == false)
                {
                    int allowedAmount = this.inputTank2.remainingCapacity;
                    this.inputTank2.intakeFluid(L, remainingAmount);
                    remainingAmount -= allowedAmount;
                    this.needsUpdate = true;
                    return;
                }
            }

        }

        /// <summary>
        /// Consumes the Fluid in the input tanks. Mainly used for machine processing but shouldn't be drained outwards.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="Amount"></param>
        public void consumeFluid(Fluid L, int Amount)
        {
            if (this.doTheInputTanksHaveEnoughFluid(L, Amount) == false) return;

            int requiredAmount = Amount;
            int tank1Amount = this.inputTank1.GetAmountOfFluidInThisTank(L);
            int tank2Amount= this.inputTank2.GetAmountOfFluidInThisTank(L);
            if (tank1Amount > 0 && requiredAmount>0)
            {
                this.inputTank1.consumeFluid(requiredAmount);
                requiredAmount -= tank1Amount;
                this.needsUpdate = true;

            }
            if(tank2Amount>0 && requiredAmount > 0)
            {
                this.inputTank2.consumeFluid(requiredAmount);
                requiredAmount -= tank2Amount;
                this.needsUpdate = true;
            }
            //Consumes Fluid from both tanks if double input is enabled. Otherwise it only drains from the appropriate tank.
        }

        public void drainOutputTank(int Amount)
        {
            this.outputTank.consumeFluid(Amount);
            if (this.outputTank.IsEmpty) this.outputTank.fluid = null;
            this.needsUpdate = true;
        }

        /// <summary>
        /// Checks to see if the input tanks have enough Fluid combined to process the request.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public bool doTheInputTanksHaveEnoughFluid(Fluid L, int Amount)
        {
            int tankTotals = this.inputTank1.GetAmountOfFluidInThisTank(L) + this.inputTank2.GetAmountOfFluidInThisTank(L);
            if (tankTotals >= Amount) return true;
            else return false;
        }

        /// <summary>
        /// Gets the total amount of Fluid that the input tanks can recieve
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public int getMaxAmountOfFluidIntakePossible(Fluid L)
        {

            if (this.allowDoubleInput)
            {
                int amount = 0;
                amount += this.inputTank1.GetAmountOfFluidThisTankCanReceieve(L);
                amount += this.inputTank2.GetAmountOfFluidThisTankCanReceieve(L);
            }
            else
            {
                if(this.inputTank1.CanRecieveThisFluid(L) && this.inputTank2.DoesTankContainThisFluid(L) == false)
                {
                    return this.inputTank1.GetAmountOfFluidThisTankCanReceieve(L);
                }
                if (this.inputTank1.CanRecieveThisFluid(L) && this.inputTank2.DoesTankContainThisFluid(L) == false)
                {
                    return this.inputTank2.GetAmountOfFluidThisTankCanReceieve(L);
                }
            }
            return 0;
        }

        /// <summary>
        /// Checks to see if the input tanks on this Fluid manager have the capacity to take in this Fluid at all.
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public bool canRecieveThisFluid(Fluid L)
        {
            if (L == null) return false;
            if (this.allowDoubleInput)
            {
                if (this.inputTank1.CanRecieveThisFluid(L) || this.inputTank2.CanRecieveThisFluid(L))
                {
                    return true;
                }
            }
            else
            {
                if (this.inputTank1.CanRecieveThisFluid(L) && this.inputTank2.DoesTankContainThisFluid(L) == false)
                {
                    return false;
                }
                if (this.inputTank2.CanRecieveThisFluid(L) && this.inputTank1.DoesTankContainThisFluid(L) == false)
                {
                    return true;
                }
            }
            return false;
           
        }

        /// <summary>
        /// Takes the fluid in this output tank and tries to transfer it to another Fluid manager who has an tank available.
        /// </summary>
        /// <param name="Other"></param>
        public void outputFluidToOtherSources(FluidManagerV2 Other)
        {
            if (this.outputTank.fluid == null) return;
            if (Other.canRecieveThisFluid(this.outputTank.fluid))
            {
                int actualAmount = Math.Min(this.outputTank.amount, Other.getMaxAmountOfFluidIntakePossible(this.outputTank.fluid));
                Other.intakeFluid(this.outputTank.fluid,actualAmount);
                this.drainOutputTank(actualAmount);
            }
        }
    }
}
