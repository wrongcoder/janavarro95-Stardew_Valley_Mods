using Revitalize.Framework.Utilities;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Crafting
{
    public class Recipe
    {
        public Dictionary<Item, int> ingredients;
        public Dictionary<Item, int> outputs;

        private Item displayItem;

        public Item DisplayItem
        {
            get
            {
                if (this.displayItem == null) return outputs.ElementAt(0).Key;
                else
                {
                    return displayItem;
                }
            }
            set
            {
                this.displayItem = value;
            }
        }

        public string outputDescription;
        public string outputName;


        public Recipe()
        {

        }

        /// <summary>
        /// Constructor for single item output.
        /// </summary>
        /// <param name="inputs">All the ingredients required to make the output.</param>
        /// <param name="output">The item given as output with how many</param>
        public Recipe(Dictionary<Item,int> inputs,KeyValuePair<Item,int> output)
        {
            this.ingredients = inputs;
            this.DisplayItem = output.Key;
            this.outputDescription = output.Key.getDescription();
            this.outputName = output.Key.DisplayName;
            this.outputs = new Dictionary<Item, int>();
            this.outputs.Add(output.Key, output.Value);
        }

        public Recipe(Dictionary<Item, int> inputs,Dictionary<Item,int> outputs,string OutputName, string OutputDescription,Item DisplayItem=null)
        {
            this.ingredients = inputs;
            this.outputs = outputs;
            this.outputName = OutputName;
            this.outputDescription = OutputDescription;
            this.DisplayItem = DisplayItem;
        }


        /// <summary>
        /// Checks if a player contains all recipe ingredients.
        /// </summary>
        /// <returns></returns>
        public bool PlayerContainsAllIngredients()
        {
            return InventoryContainsAllIngredient(Game1.player.Items.ToList());
        }

        /// <summary>
        /// Checks if a player contains a recipe ingredient.
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public bool PlayerContainsIngredient(KeyValuePair<Item,int> pair)
        {
            return InventoryContainsIngredient(Game1.player.Items.ToList(), pair);
        }


        /// <summary>
        /// Checks if an inventory contains all items.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool InventoryContainsAllIngredient(List<Item> items)
        {
            foreach (KeyValuePair<Item, int> pair in this.ingredients)
            {
                if (InventoryContainsIngredient(items,pair) == false) return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if an inventory contains an ingredient.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pair"></param>
        /// <returns></returns>
        public bool InventoryContainsIngredient(List<Item> items,KeyValuePair<Item, int> pair)
        {
            foreach (Item i in items)
            {
                if (i == null) continue;
                if (ItemEqualsOther(i, pair.Key) && pair.Value == i.Stack)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Checks roughly if two items equal each other.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool ItemEqualsOther(Item self, Item other)
        {
            if (self.Name == other.Name && self.getCategoryName() == other.getCategoryName() && self.GetType() == other.GetType()) return true;
            return false;
        }

        public void consume(ref List<Item> from)
        {
            if (!InventoryContainsAllIngredient(from)) return;
            InventoryManager manager = new InventoryManager(from);

            List<Item> removalList = new List<Item>();

            foreach (KeyValuePair<Item,int> pair in this.ingredients)
            {
                foreach(Item InventoryItem in manager.items)
                {
                    if (InventoryItem == null) continue;
                    if (ItemEqualsOther(InventoryItem, pair.Key))
                    {
                        if (InventoryItem.Stack == pair.Value)
                        {
                            removalList.Add(InventoryItem); //remove the item
                        }
                        else
                        {
                            InventoryItem.Stack -= pair.Value; //or reduce the stack size.
                        }
                    }
                }
            }

            foreach (var v in removalList)
            {
                manager.items.Remove(v);
            }
            removalList.Clear();
            from = manager.items;
        }

        public void produce(ref List<Item> to,bool dropToGround=false)
        {
            InventoryManager manager = new InventoryManager(to);
            foreach(KeyValuePair<Item,int> pair in this.outputs)
            {
                Item I = pair.Key.getOne();
                I.addToStack(pair.Value - 1);
                bool added=manager.addItem(I);
                if (added == false && dropToGround==true)
                {
                    Game1.createItemDebris(I, Game1.player.getStandingPosition(), Game1.player.getDirection());
                }
            }
            to = manager.items;
        }

        public void craft(ref List<Item> from,ref List<Item> to,bool dropToGround=false)
        {
            consume(ref from);
            produce(ref to);
        }

        public void craft()
        {
            List<Item> playerItems = Game1.player.Items.ToList();
            craft(ref playerItems,ref playerItems, true);
            Game1.player.Items = playerItems;
        }

        public bool PlayerCanCraft()
        {
            return PlayerContainsAllIngredients();
        }

        public bool CanCraft(List<Item> items)
        {
            return InventoryContainsAllIngredient(items);
        }

    }
}
