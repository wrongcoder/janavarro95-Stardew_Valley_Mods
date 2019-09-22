using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Revitalize.Framework.Managers
{
    /// <summary>
    /// A liquid used for various mod purposes.
    /// </summary>
    public class Liquid
    {
        /// <summary>
        /// The name of the liquid.
        /// </summary>
        public string name;
        /// <summary>
        /// The color for the liquid.
        /// </summary>
        public Color color;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Liquid()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="LiquidColor"></param>
        public Liquid(string Name, Color LiquidColor)
        {
            this.name = Name;
            this.color = LiquidColor;
        }

        /// <summary>
        /// Liquid comparison check to see if two liquids are the same.
        /// </summary>
        /// <param name="Other"></param>
        /// <returns></returns>
        public bool isLiquidHomogenous(Liquid Other)
        {
            if (this.name.Equals(Other.name)) return true;
            return false;
        }

        /// <summary>
        /// Copys over the liquid.
        /// </summary>
        /// <returns></returns>
        public Liquid Copy()
        {
            return new Liquid(this.name, this.color);
        }
    }

    public class MachineLiquidTank
    {
        /// <summary>
        /// The liquid inside of the tank.
        /// </summary>
        public Liquid liquid;
        /// <summary>
        /// How much liquid is inside the tank currently.
        /// </summary>
        public int amount;
        /// <summary>
        /// How much liquid the tank can hold.
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
        public MachineLiquidTank()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Capacity"></param>
        public MachineLiquidTank(int Capacity)
        {
            this.capacity = Capacity;
            this.amount = 0;
            this.liquid = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Capacity"></param>
        /// <param name="Amount"></param>
        /// <param name="Liquid"></param>
        public MachineLiquidTank(int Capacity, int Amount, Liquid Liquid)
        {
            this.capacity = Capacity;
            this.amount = Amount;
            this.liquid = Liquid;
        }

        /// <summary>
        /// Checks to see if this tank can recieve this liquid.
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public bool CanRecieveThisLiquid(Liquid L)
        {
            if (this.IsFull) return false;
            if (this.liquid == null) return true;
            if (this.liquid.isLiquidHomogenous(L)) return true;
            if (this.IsEmpty) return true;
            else return false;
        }

        /// <summary>
        /// Takes in liquid into this tank.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="Amount"></param>
        public void intakeLiquid(Liquid L, int Amount)
        {
            if (this.CanRecieveThisLiquid(L))
            {
                if (this.liquid == null) this.liquid = L.Copy();
                else
                {
                    int intakeAmount=Math.Min(this.remainingCapacity, Amount);
                    this.amount = this.amount + intakeAmount;
                }
            }
            else return;
        }
        /// <summary>
        /// Consumes, aka reduces the internal liquid on this tank by the amount given or the amount remaining in the tank.
        /// </summary>
        /// <param name="Amount"></param>
        public void consumeLiquid(int Amount)
        {
            if (this.IsEmpty) return;
            if (this.liquid == null) return;
            int consumeAmount = Math.Min(this.amount, Amount);
            this.amount = this.amount - consumeAmount;
            if (this.amount <= 0) this.liquid = null;
        }

        /// <summary>
        /// Checks to see if this tank has enough 
        /// </summary>
        /// <param name="L"></param>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public bool DoesThisTankHaveEnoughLiquid(Liquid L, int Amount)
        {
            if (this.GetAmountOfLiquidInThisTank(L) >= Amount) return true;
            return false;
        }

        /// <summary>
        /// Drains the tank completly
        /// </summary>
        public void emptyTank()
        {
            this.liquid = null;
            this.amount = 0;
        }

        /// <summary>
        /// Gets the amount of liquid in this tank for the given liquid.
        /// </summary>
        /// <param name="L"></param>
        /// <returns> Returns 0 if the tank doesn't contain liquid of the same type. Otherwise returns the amount stored in the tank.</returns>
        public int GetAmountOfLiquidInThisTank(Liquid L)
        {
            if (this.liquid == null) return 0;
            if (this.liquid.isLiquidHomogenous(L)) return this.amount;
            return 0;
        }

        /// <summary>
        /// Gets the amount of liquid this take can take in in acordance with the parameter liquid.
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public int GetAmountOfLiquidThisTankCanReceieve(Liquid L)
        {
            if (this.liquid == null) return this.capacity;
            if (this.liquid.isLiquidHomogenous(L)) return this.remainingCapacity;
            return 0;
        }

        /// <summary>
        /// Checks to see if this tank contains this liquid at all.
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public bool DoesTankContainThisLiquid(Liquid L)
        {
            if (this.liquid == null) return false;
            if (this.liquid.isLiquidHomogenous(L)) return true;
            return false;
        }

    }
    public class LiquidManagerV2
    {
        public MachineLiquidTank inputTank1;
        public MachineLiquidTank inputTank2;
        public MachineLiquidTank outputTank;

        public bool needsUpdate;

        /// <summary>
        /// Does this machine allow the same fluid in both tanks?
        /// </summary>
        public bool allowDoubleInput;

        public LiquidManagerV2()
        {
            this.inputTank1 = new MachineLiquidTank(0);
            this.inputTank2 = new MachineLiquidTank(0);
            this.outputTank = new MachineLiquidTank(0);
            this.needsUpdate = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Capacity"></param>
        /// <param name="OnlyOutput"></param>
        /// <param name="AllowDoubleInput">Can both input tanks store the same liquid?</param>
        public LiquidManagerV2(int Capacity, bool OnlyOutput, bool AllowDoubleInput=false)
        {
            if (OnlyOutput)
            {
                this.outputTank = new MachineLiquidTank(Capacity);
                this.inputTank1 = new MachineLiquidTank(0);
                this.inputTank2 = new MachineLiquidTank(0);

            }
            else
            {
                this.outputTank = new MachineLiquidTank(Capacity);
                this.inputTank1 = new MachineLiquidTank(Capacity);
                this.inputTank2 = new MachineLiquidTank(Capacity);
            }
            this.allowDoubleInput = AllowDoubleInput;
            this.needsUpdate = false;
        }

        /// <summary>
        /// Produces a given amount of liquid and puts it into the output tank for this liquid manager.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="Amount"></param>
        public void produceLiquid(Liquid L, int Amount)
        {
            if (this.outputTank.CanRecieveThisLiquid(L))
            {
                this.outputTank.intakeLiquid(L, Amount);
                this.needsUpdate = true;
            }
        }

        /// <summary>
        /// Intakes liquid into the input takes on this liquid manager.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="Amount"></param>
        public void intakeLiquid(Liquid L, int Amount)
        {
            int remainingAmount = Amount;
            if (this.allowDoubleInput)
            {
                if (this.inputTank1.CanRecieveThisLiquid(L) && remainingAmount>0)
                {
                    int allowedAmount = this.inputTank1.remainingCapacity;
                    this.inputTank1.intakeLiquid(L, remainingAmount);
                    remainingAmount -= allowedAmount;
                }
                if (this.inputTank2.CanRecieveThisLiquid(L)&& remainingAmount>0)
                {
                    int allowedAmount = this.inputTank2.remainingCapacity;
                    this.inputTank2.intakeLiquid(L, remainingAmount);
                    remainingAmount -= allowedAmount;
                }
                this.needsUpdate = true;
            }
            else
            {

                if (this.inputTank1.CanRecieveThisLiquid(L) && remainingAmount > 0 && this.inputTank2.DoesTankContainThisLiquid(L)==false)
                {
                    int allowedAmount = this.inputTank1.remainingCapacity;
                    this.inputTank1.intakeLiquid(L, remainingAmount);
                    remainingAmount -= allowedAmount;
                    this.needsUpdate = true;
                    return;
                }
                if (this.inputTank2.CanRecieveThisLiquid(L) && remainingAmount > 0 && this.inputTank1.DoesTankContainThisLiquid(L) == false)
                {
                    int allowedAmount = this.inputTank2.remainingCapacity;
                    this.inputTank2.intakeLiquid(L, remainingAmount);
                    remainingAmount -= allowedAmount;
                    this.needsUpdate = true;
                    return;
                }
            }

        }

        /// <summary>
        /// Consumes the liquid in the input tanks. Mainly used for machine processing but shouldn't be drained outwards.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="Amount"></param>
        public void consumeLiquid(Liquid L, int Amount)
        {
            if (this.doTheInputTanksHaveEnoughLiquid(L, Amount) == false) return;

            int requiredAmount = Amount;
            int tank1Amount = this.inputTank1.GetAmountOfLiquidInThisTank(L);
            int tank2Amount= this.inputTank2.GetAmountOfLiquidInThisTank(L);
            if (tank1Amount > 0 && requiredAmount>0)
            {
                this.inputTank1.consumeLiquid(requiredAmount);
                requiredAmount -= tank1Amount;
                this.needsUpdate = true;

            }
            if(tank2Amount>0 && requiredAmount > 0)
            {
                this.inputTank2.consumeLiquid(requiredAmount);
                requiredAmount -= tank2Amount;
                this.needsUpdate = true;
            }
            //Consumes liquid from both tanks if double input is enabled. Otherwise it only drains from the appropriate tank.
        }

        public void drainOutputTank(int Amount)
        {
            this.outputTank.consumeLiquid(Amount);
            if (this.outputTank.IsEmpty) this.outputTank.liquid = null;
            this.needsUpdate = true;
        }

        /// <summary>
        /// Checks to see if the input tanks have enough liquid combined to process the request.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public bool doTheInputTanksHaveEnoughLiquid(Liquid L, int Amount)
        {
            int tankTotals = this.inputTank1.GetAmountOfLiquidInThisTank(L) + this.inputTank2.GetAmountOfLiquidInThisTank(L);
            if (tankTotals >= Amount) return true;
            else return false;
        }

        /// <summary>
        /// Gets the total amount of liquid that the input tanks can recieve
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public int getMaxAmountOfLiquidIntakePossible(Liquid L)
        {

            if (this.allowDoubleInput)
            {
                int amount = 0;
                amount += this.inputTank1.GetAmountOfLiquidThisTankCanReceieve(L);
                amount += this.inputTank2.GetAmountOfLiquidThisTankCanReceieve(L);
            }
            else
            {
                if(this.inputTank1.CanRecieveThisLiquid(L) && this.inputTank2.DoesTankContainThisLiquid(L) == false)
                {
                    return this.inputTank1.GetAmountOfLiquidThisTankCanReceieve(L);
                }
                if (this.inputTank1.CanRecieveThisLiquid(L) && this.inputTank2.DoesTankContainThisLiquid(L) == false)
                {
                    return this.inputTank2.GetAmountOfLiquidThisTankCanReceieve(L);
                }
            }
            return 0;
        }

        /// <summary>
        /// Checks to see if the input tanks on this liquid manager have the capacity to take in this liquid at all.
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public bool canRecieveThisLiquid(Liquid L)
        {
            if (L == null) return false;
            if (this.allowDoubleInput)
            {
                if (this.inputTank1.CanRecieveThisLiquid(L) || this.inputTank2.CanRecieveThisLiquid(L))
                {
                    return true;
                }
            }
            else
            {
                if (this.inputTank1.CanRecieveThisLiquid(L) && this.inputTank2.DoesTankContainThisLiquid(L) == false)
                {
                    return false;
                }
                if (this.inputTank2.CanRecieveThisLiquid(L) && this.inputTank1.DoesTankContainThisLiquid(L) == false)
                {
                    return true;
                }
            }
            return false;
           
        }

        /// <summary>
        /// Takes the fluid in this output tank and tries to transfer it to another liquid manager who has an tank available.
        /// </summary>
        /// <param name="Other"></param>
        public void outputLiquidToOtherSources(LiquidManagerV2 Other)
        {
            if (this.outputTank.liquid == null) return;
            if (Other.canRecieveThisLiquid(this.outputTank.liquid))
            {
                int actualAmount = Math.Min(this.outputTank.amount, Other.getMaxAmountOfLiquidIntakePossible(this.outputTank.liquid));
                Other.intakeLiquid(this.outputTank.liquid,actualAmount);
                this.drainOutputTank(actualAmount);
            }
        }
    }
}
