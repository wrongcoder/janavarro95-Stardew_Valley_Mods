using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Revitalize.Framework.Utilities
{
    public class Liquid
    {
        public string name;
        public Color liquidColor;
        public int amount;
        public int capacity;

        public string liquidDisplayString
        {
            get
            {
                StringBuilder b = new StringBuilder();
                b.Append(this.amount);
                b.Append("/");
                b.Append(this.capacity);
                return b.ToString();
            }
        }

        public int capacityRemaining
        {
            get
            {
                return this.capacity - this.amount;
            }
        }

        /// <summary>
        /// Returns the energy remaining as a percent value.
        /// </summary>
        public double liquidPercentRemaining
        {
            get
            {
                return (double)this.amount / (double)this.capacity;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this.amount == 0;
            }
        }

        public bool IsFull
        {
            get
            {
                return this.amount == this.capacity;
            }
        }


        public Liquid()
        {

        }

        public Liquid(string Name, Color liquidColor, int Capacity)
        {
            this.name = Name;
            this.liquidColor = liquidColor;
            this.capacity = Capacity;
        }

        public Liquid(string Name, Color liquidColor, int Amount, int Capacity)
        {
            this.name = Name;
            this.liquidColor = liquidColor;
            this.capacity = Capacity;
            this.amount = Amount;
        }

        public void produceLiquid(int amount)
        {
            this.amount = Math.Min(this.capacity, this.amount + amount);
        }

        public void consumeLiquid(int amount)
        {
            this.amount = Math.Max(0, this.amount - amount);
        }


        /// <summary>
        /// Checks to see if this energy source has enough energy remaining.
        /// </summary>
        /// <param name="Required"></param>
        /// <returns></returns>
        public bool hasEnoughLiquid(int Required)
        {
            return this.amount >= Required;
        }

        public Liquid Copy()
        {
            return new Liquid(this.name, this.liquidColor, 0, this.capacity);
        }
    }

    public class LiquidManager
    {



        public bool requiresUpdate;
        /// <summary>
        /// How does this energy manager interface energy systems.
        /// </summary>
        public Enums.LiquidInteractionType liquidInteractionType;

        /// <summary>
        /// Checks to see if this energy manager consumes energy.
        /// </summary>
        public bool consumesLiquid
        {
            get
            {
                return this.liquidInteractionType == Enums.LiquidInteractionType.Consumes;
            }
        }
        /// <summary>
        /// Checks to see if this energy manager produces energy.
        /// </summary>
        public bool producesLiquid
        {
            get
            {
                return this.liquidInteractionType == Enums.LiquidInteractionType.Produces;
            }
        }

        /// <summary>
        /// Checks to see if this energy manager transfers energy.
        /// </summary>
        public bool transfersLiquid
        {
            get
            {
                return this.liquidInteractionType == Enums.LiquidInteractionType.Transfers;
            }
        }

        public int MaxPossibleAmountOfLiquids;
        public Dictionary<string, Liquid> liquids;
        public List<Liquid> acceptedLiquid;

        public LiquidManager()
        {

        }

        public LiquidManager(Enums.LiquidInteractionType LiquidType) : this(1, LiquidType)
        {
        }

        public LiquidManager(int NumberOfPossibleFluids, Enums.LiquidInteractionType liquidInteractionType)
        {
            this.liquidInteractionType = liquidInteractionType;
            this.MaxPossibleAmountOfLiquids = NumberOfPossibleFluids;
            this.liquids = new Dictionary<string, Liquid>();
        }


        public void consumeLiquid(int amount, string LiquidName)
        {

            this.GetLiquid(LiquidName, out Liquid selfLiquid);
            if (selfLiquid == null) return;

            selfLiquid.consumeLiquid(amount);
            this.requiresUpdate = true;
            if (selfLiquid.IsEmpty)
            {
                this.liquids.Remove(selfLiquid.name);
            }
        }

        public void produceLiquid(int amount, string LiquidName,Liquid L)
        {
            this.GetLiquid(LiquidName, out Liquid selfLiquid);
            if (selfLiquid == null)
            {
                this.AddLiquid(L);
                this.GetLiquid(LiquidName, out Liquid selfLiquid2);
                selfLiquid2.produceLiquid(amount);
                this.requiresUpdate = true;
                return;
            }

            selfLiquid.produceLiquid(amount);
            this.requiresUpdate = true;
        }

        public void AddLiquid(Liquid M)
        {
            if (this.liquids.ContainsKey(M.name))
            {
                this.liquids[M.name].produceLiquid(M.amount);
                this.requiresUpdate = true;
            }
            else
            {
                if (this.canReceieveThisLiquid(M))
                {
                    this.liquids.Add(M.name, M);
                }
            }
        }

        public void GetLiquid(string Name, out Liquid Liquid)
        {
            Liquid = new Liquid();
            if (this.liquids.ContainsKey(Name))
            {
                this.liquids.TryGetValue(Name, out Liquid);
            }
        }

        public void transferLiquidFromAnother(LiquidManager other, string LiquidName, int amount)
        {
            other.GetLiquid(LiquidName, out Liquid OtherLiquid);
            if (OtherLiquid == null) return;
            if (this.canReceieveThisLiquid(OtherLiquid))
            {
                int actualAmount = Math.Min(amount, OtherLiquid.amount);

                this.GetLiquid(LiquidName, out Liquid SelfLiquid);

                int selfCapacity = SelfLiquid!=null? SelfLiquid.capacityRemaining:OtherLiquid.amount;
                this.produceLiquid(actualAmount, OtherLiquid.name,OtherLiquid.Copy());
                other.consumeLiquid(Math.Min(actualAmount, selfCapacity),OtherLiquid.name);
            }
            else
            {
                return;
            }
        }

        public void transferLiquidToAnother(LiquidManager other, string LiquidName, int amount)
        {
            this.GetLiquid(LiquidName, out Liquid SelfLiquid);
            if (SelfLiquid == null) return;
            if (other.canReceieveThisLiquid(SelfLiquid))
            {
                int actualAmount = Math.Min(amount, SelfLiquid.amount);

                this.GetLiquid(LiquidName, out Liquid OtherLiquid);
                int selfCapacity =OtherLiquid != null ? OtherLiquid.capacityRemaining : SelfLiquid.amount;

                other.produceLiquid(Math.Min(actualAmount, selfCapacity), SelfLiquid.name,SelfLiquid.Copy());
                this.consumeLiquid(Math.Min(actualAmount, selfCapacity),SelfLiquid.name);
            }
            else
            {
                return;
            }
        }

        public LiquidManager Copy()
        {
            return new LiquidManager(this.MaxPossibleAmountOfLiquids, this.liquidInteractionType);
        }

        public bool canReceieveThisLiquid(Liquid M)
        {
            if (M == null) return false;
            if (this.liquids.Count < this.MaxPossibleAmountOfLiquids) return true;
            if (this.liquids.Values.ToList().FindAll(L => L.name == M.name).Count > 0) return true;
            if (this.acceptedLiquid.FindAll(L => L.name == M.name).Count > 0) return true;

            return false;
        }

        public bool hasLiquid(string Name)
        {
            if (this.liquids.ContainsKey(Name))
            {
                return true;
            }
            else return false;
        }

        public bool doesLiquidHaveVolume(string Name)
        {
            if (this.liquids.ContainsKey(Name))
            {
                if (this.liquids[Name].amount > 0) return true;
                return false;
            }
            else return false;
        }

        public bool doesLiquidHaveEnough(string LiquidName,int Amount)
        {
            this.GetLiquid(LiquidName, out Liquid selfLiquid);
            if (selfLiquid == null) return false;
            if (selfLiquid.amount >= Amount) return true;
            else return false;
        }

        public bool canThisLiquidReceiveMoreVolume(string LiquidName, int Amount)
        {
            this.GetLiquid(LiquidName, out Liquid selfLiquid);
            if (selfLiquid == null) return false;
            if (selfLiquid.IsFull) return false;
            else return true;
        }

    }
}
