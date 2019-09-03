using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revitalize.Framework.Menus.MenuComponents;
using Revitalize.Framework.Objects;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons;

namespace Revitalize.Framework.Menus
{
    /// <summary>
    /// Need to display description, required items, and a craft button. Also need to make the menu longer.
    /// Also need to make the crafting menu scroll better.
    /// </summary>
    public class CraftingInformationPage:IClickableMenuExtended
    {

        public CraftingRecipeButton infoButton;
        public Color backgroundColor;

        public Vector2 itemDisplayLocation;

        public IList<Item> inventory;

        private Dictionary<ItemDisplayButton,int> requiredItems;

        public Item actualItem
        {
            get
            {
                return this.infoButton.displayItem.item;
            }
        }

        public CraftingInformationPage():base()
        {

        }

        public CraftingInformationPage(int x, int y, int width, int height,Color BackgroundColor,CraftingRecipeButton ItemToDisplay,ref IList<Item> Inventory) : base(x, y, width, height, false)
        {
            this.backgroundColor = BackgroundColor;
            this.infoButton = ItemToDisplay;
            this.itemDisplayLocation = new Vector2(this.xPositionOnScreen + (this.width / 2) - 32, this.yPositionOnScreen + (128));
            this.inventory = Inventory;

            this.requiredItems = new Dictionary<ItemDisplayButton, int>();
            for (int i = 0; i < this.infoButton.recipe.ingredients.Count; i++)
            {
                ItemDisplayButton b = new ItemDisplayButton(this.infoButton.recipe.ingredients.ElementAt(i).Key, null, new Vector2(this.xPositionOnScreen + 64, this.getIngredientHeightOffset().Y), new Rectangle(0, 0, 64, 64), 2f, true, Color.White);
                this.requiredItems.Add(b, this.infoButton.recipe.ingredients.ElementAt(i).Value);
            }
        }

        public override void draw(SpriteBatch b)
        {
            this.drawDialogueBoxBackground(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, this.backgroundColor);
            this.infoButton.draw(b,this.itemDisplayLocation);

            b.DrawString(Game1.dialogueFont, this.actualItem.DisplayName, this.itemDisplayLocation + this.getHeightOffsetFromItem()-this.getItemNameOffset(), this.getNameColor());

            b.DrawString(Game1.smallFont, Game1.parseText(this.actualItem.getDescription(), Game1.smallFont, this.width),new Vector2(this.xPositionOnScreen+64,this.getItemDescriptionOffset().Y), Color.Black);

            foreach(KeyValuePair<ItemDisplayButton,int> button in this.requiredItems)
            {
                button.Key.draw(b);
                b.DrawString(Game1.smallFont, button.Key.item.DisplayName+ " x "+button.Value.ToString(), button.Key.Position + new Vector2(64, 16), this.getNameColor(button.Key.item, button.Value));
            }

            this.drawMouse(b);
        }

        public bool doesMenuContainPoint(int x, int y)
        {
            Rectangle r = new Rectangle(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height);
            return r.Contains(x, y);
        }

        public bool canCraftRecipe()
        {
            return this.infoButton.recipe.CanCraft(this.inventory);
        }

        public Color getNameColor()
        {
            if (this.canCraftRecipe()) return Color.Black;
            else return Color.Red;
        }

        public Color getNameColor(Item I, int amount)
        {
            KeyValuePair<Item, int> Pair = new KeyValuePair<Item, int>(I, amount);
            if (this.infoButton.recipe.InventoryContainsIngredient(this.inventory, Pair))
            {
                return Color.Black;
            }
            else
            {
                return Color.Red;
            }
            
        }

        private Vector2 getHeightOffsetFromItem()
        {
            if (ObjectUtilities.IsSameType(typeof(StardewValley.Object), this.actualItem.GetType())){
                return new Vector2(0, 64f);
            }
            if (ObjectUtilities.IsSameType(typeof(Revitalize.Framework.Objects.MultiTiledObject), this.actualItem.GetType()))
            {
                return new Vector2(0, 64f*(this.actualItem as MultiTiledObject).Height);
            }

            return new Vector2(0, 64f);
        }

        private Vector2 getItemNameOffset()
        {
            Vector2 length = Game1.dialogueFont.MeasureString(this.actualItem.DisplayName);
            length.X = length.X / 4;
            length.Y = 0;
            return length;
        }

        private Vector2 getItemDescriptionOffset()
        {
            return this.getHeightOffsetFromItem()+new Vector2(0,64)+new Vector2(0,this.itemDisplayLocation.Y);
        }

        /// <summary>
        /// Gets the height position for where to draw a required ingredient.
        /// </summary>
        /// <returns></returns>
        private Vector2 getIngredientHeightOffset()
        {
            string parsedDescription = Game1.parseText(this.actualItem.getDescription(), Game1.smallFont, this.width);
            Vector2 offset=Game1.smallFont.MeasureString(parsedDescription);
            return this.getItemDescriptionOffset()+offset+ new Vector2(0,64*(this.requiredItems.Count));
        }

    }
}
