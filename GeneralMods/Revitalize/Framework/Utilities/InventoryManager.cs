using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Netcode;
using Newtonsoft.Json;
using StardewValley;

namespace Revitalize.Framework.Utilities
{
    /// <summary>Handles dealing with objects.</summary>
    public class InventoryManager
    {
        /// <summary>How many items the inventory can hold.</summary>
        public readonly NetInt capacity = new NetInt();

        /// <summary>The hard uper limit for # of items to be held in case of upgrading or resizing.</summary>
        public readonly NetInt maxCapacity = new NetInt();

        /// <summary>The actual contents of the inventory.</summary>
        public readonly NetObjectList<Item> items = new NetObjectList<Item>();

        /// <summary>
        /// Items that are to be buffered into the inventory manager if possible.
        /// </summary>
        public readonly NetObjectList<Item> bufferItems = new NetObjectList<Item>();

        public readonly NetInt displayColumns = new NetInt();
        public readonly NetInt displayRows = new NetInt();


        public InventoryManager()
        {
            this.capacity.Value = 0;
            this.setMaxLimit(0);
        }

        /// <summary>Construct an instance.</summary>
        public InventoryManager(IList<Item> items, int DisplayRows = 6, int DisplayColumns = 6) : this((List<Item>)items, DisplayRows, DisplayColumns)
        {

        }

        /// <summary>Construct an instance.</summary>
        public InventoryManager(List<Item> items, int DisplayRows = 6, int DisplayColumns = 6)
        {
            this.capacity.Value = int.MaxValue;
            this.setMaxLimit(int.MaxValue);
            this.items.AddRange(items);
            this.displayRows.Value = DisplayRows;
            this.displayColumns.Value = DisplayColumns;
        }

        /// <summary>Construct an instance.</summary>
        public InventoryManager(List<Item> items, int capacity, int MaxCapacity, int DisplayRows = 6, int DisplayColumns = 6)
        {
            this.capacity.Value = capacity;
            this.setMaxLimit(MaxCapacity);
            this.items.AddRange(items);
            this.displayRows.Value = DisplayRows;
            this.displayColumns.Value = DisplayColumns;
        }

        public virtual List<INetSerializable> getNetFields()
        {
            return new List<INetSerializable>() {

                this.capacity,
                this.maxCapacity,
                this.items,
                this.bufferItems,
                this.displayColumns,
                this.displayRows

            };
        }

        /// <summary>Add the item to the inventory.</summary>
        public bool addItem(Item item)
        {
            if (this.isFull())
            {
                return false;
            }
            else
            {
                for (int i = 0; i < this.items.Count; i++)
                {
                    Item self = this.items[i];
                    if (self != null && self.canStackWith(item))
                    {
                        self.addToStack(item);
                        return true;
                    }
                    if (self == null)
                    {
                        this.items[i] = item;
                        return true;
                    }
                }

                this.items.Add(item);
                return true;
            }
        }

        /// <summary>Gets a reference to the object IF it exists in the inventory.</summary>
        public Item getItem(Item item)
        {
            foreach (Item i in this.items)
            {
                if (item == i)
                    return item;
            }
            return null;
        }

        /// <summary>Get the item at the specific index.</summary>
        public Item getItemAtIndex(int index)
        {
            return this.items[index];
        }

        /// <summary>Gets only one item from the stack.</summary>
        public Item getSingleItemFromStack(Item item)
        {
            if (item.Stack == 1)
                return item;

            item.Stack = item.Stack - 1;
            return item.getOne();
        }

        /// <summary>Empty the inventory.</summary>
        public void clear()
        {
            this.items.Clear();
        }

        /// <summary>Empty the inventory.</summary>
        public void empty()
        {
            this.clear();
        }

        /// <summary>Resize how many items can be held by this object.</summary>
        public void resizeCapacity(int Amount)
        {
            if (this.capacity + Amount < this.maxCapacity)
            {
                this.capacity.Value += Amount;
            }
        }

        /// <summary>Sets the upper limity of the capacity size for the inventory.</summary>
        public void setMaxLimit(int amount)
        {
            this.maxCapacity.Value = amount;
        }

        public bool canReceieveThisItem(Item I)
        {
            if (this.isFull()) return false;
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Returns a new inventory manager without the items but with the capacity limits.
        /// </summary>
        /// <returns></returns>
        public InventoryManager Copy()
        {
            return new InventoryManager(new List<Item>(), this.capacity, this.maxCapacity, this.displayRows, this.displayColumns);
        }

        public void dumpBufferToItems()
        {
            foreach (Item I in this.bufferItems)
            {
                this.addItem(I);
            }
            this.bufferItems.Clear();
        }

        /// <summary>
        /// Gets the number of non null items held by this inventory.
        /// </summary>
        /// <returns></returns>
        public virtual int getNonNullItemCount()
        {
            return this.items.Where(i => i != null).Count();
        }

        /// <summary>Checks if the inventory is full or not.</summary>
        public virtual bool isFull()
        {
            return this.getNonNullItemCount() >= this.capacity && this.items.Where(i => i == null).Count() == 0;
        }

        /// <summary>Checks to see if this core object actually has a valid inventory.</summary>
        public virtual bool hasInventory()
        {
            return this.capacity > 0;
        }

        /// <summary>
        /// Checks to see if the buffer list has any items.
        /// </summary>
        /// <returns></returns>
        public virtual bool hasItemsInBuffer()
        {
            return this.bufferItems.Count > 0;
        }
    }
}
