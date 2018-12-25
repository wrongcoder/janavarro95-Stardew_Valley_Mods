using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Utilities
{
    /// <summary>
    /// Handles dealing with objects.
    /// </summary>
    public class InventoryManager
    {
        /// <summary>
        /// How many items the inventory can hold.
        /// </summary>
        public int capacity;

        /// <summary>
        /// The hard uper limit in case of upgrading or resizing.
        /// </summary>
        private int maxCapacity;

        /// <summary>
        /// The hard uper limit for # of items to be held in case of upgrading or resizing.
        /// </summary>
        public int MaxCapacity
        {
            get
            {
                return maxCapacity;
            }
        }

        /// <summary>
        /// How many items are currently stored in the inventory.
        /// </summary>
        public int ItemCount
        {
            get
            {
                return this.items.Count;
            }
        }

        /// <summary>
        /// The actual contents of the inventory.
        /// </summary>
        public List<Item> items;

        /// <summary>
        /// Checks if the inventory is full or not.
        /// </summary>
        public bool IsFull
        {
            get
            {
                return this.ItemCount >= this.capacity;
            }
        }

        /// <summary>
        /// Checks to see if this core object actually has a valid inventory.
        /// </summary>
        public bool HasInventory
        {
            get
            {
                if (this.capacity <= 0) return false;
                return true;
            }
        }

        public InventoryManager()
        {
            this.capacity = 0;
            setMaxLimit(0);
            this.items = new List<Item>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items"></param>
        public InventoryManager(List<Item> items)
        {
            this.capacity = Int32.MaxValue;
            this.setMaxLimit(Int32.MaxValue);
            this.items = items;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="capacity"></param>
        public InventoryManager(int capacity)
        {
            this.capacity = capacity;
            this.maxCapacity = Int32.MaxValue;
            this.items = new List<Item>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="MaxCapacity"></param>
        public InventoryManager(int capacity, int MaxCapacity)
        {
            this.capacity = capacity;
            setMaxLimit(MaxCapacity);
            this.items = new List<Item>();
        }

        /// <summary>
        /// Add the item to the inventory.
        /// </summary>
        /// <param name="I"></param>
        /// <returns></returns>
        public bool addItem(Item I)
        {
            if (IsFull)
            {
                return false;
            }
            else
            {
                foreach(Item self in this.items)
                {
                    if (self == null) continue;
                    if (self.canStackWith(I))
                    {
                        self.addToStack(I.Stack);
                        return true;
                    }
                }
                this.items.Add(I);
                return true;
            }
        }

        /// <summary>
        /// Gets a reference to the object IF it exists in the inventory.
        /// </summary>
        /// <param name="I"></param>
        /// <returns></returns>
        public Item getItem(Item I)
        {
            foreach(Item i in this.items)
            {
                if (I == i) return I;
            }
            return null;
        }

        /// <summary>
        /// Get the item at the specific index.
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public Item getItemAtIndex(int Index)
        {
            return items[Index];
        }

        /// <summary>
        /// Gets only one item from the stack.
        /// </summary>
        /// <param name="I"></param>
        /// <returns></returns>
        public Item getSingleItemFromStack(Item I)
        {
            if (I.Stack == 1)
            {
                return I;
            }
            else
            {
                I.Stack = I.Stack - 1;
                return I.getOne();
            }
        }

        /// <summary>
        /// Empty the inventory.
        /// </summary>
        public void clear()
        {
            this.items.Clear();
        }

        /// <summary>
        /// Empty the inventory.
        /// </summary>
        public void empty()
        {
            this.clear();
        }

        /// <summary>
        /// Resize how many items can be held by this object.
        /// </summary>
        /// <param name="Amount"></param>
        public void resizeCapacity(int Amount)
        {
            if (this.capacity + Amount < this.maxCapacity)
            {
                this.capacity += Amount;
            }
        }

        /// <summary>
        /// Sets the upper limity of the capacity size for the inventory.
        /// </summary>
        /// <param name="amount"></param>
        public void setMaxLimit(int amount)
        {
            this.maxCapacity = amount;
        }

    }
}
