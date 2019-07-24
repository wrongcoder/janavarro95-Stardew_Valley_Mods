using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using StardustCore.Animations;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents;

namespace Revitalize.Framework.Menus
{
    public class InventoryMenu : IClickableMenuExtended
    {
        public IList<Item> items;
        public List<StardustCore.UIUtilities.MenuComponents.ItemDisplayButton> storageDisplay;

        public int amountToDisplay = 9;
        public int pages = 1;

        public Item activeItem;

        public int rows = 6;
        public int collumns = 6;
        public int xOffset = 64;
        public int yOffset = 128;

        public StardewValley.Menus.TextBox searchBox;

        public InventoryMenu(int xPos, int yPos, int width, int height, bool showCloseButton, IList<Item> Inventory, int AmountToDisplay) : base(xPos, yPos, width, height, showCloseButton)
        {
            this.items = Inventory;
            this.storageDisplay = new List<ItemDisplayButton>();
            this.amountToDisplay = AmountToDisplay;
            this.pages = 1; //Change this to allow for more pages.
            this.populateClickableItems(this.rows, this.collumns, xPos + this.xOffset, yPos + this.yOffset);
            this.searchBox = new TextBox((Texture2D)null, (Texture2D)null, Game1.dialogueFont, Game1.textColor);
            this.searchBox.X = this.xPositionOnScreen;
            this.searchBox.Y = this.yPositionOnScreen;
            this.searchBox.Width = 256;
            this.searchBox.Height = 192;
            Game1.keyboardDispatcher.Subscriber = (IKeyboardSubscriber)this.searchBox;
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);

            this.searchBox.X = this.xPositionOnScreen;
            this.searchBox.Y = this.yPositionOnScreen;
            this.populateClickableItems(this.rows, this.collumns, this.xPositionOnScreen + this.xOffset, this.yPositionOnScreen + this.yOffset);
        }

        public void populateClickableItems(int rows, int collums, int xPosition, int yPosition)
        {
            this.storageDisplay.Clear();
            for (int page = 0; page < this.pages; page++)
            {
                for (int y = 0; y < collums; y++)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        int index = ((y * rows) + i)+(page*rows*collums);
                        if (index > this.items.Count)
                        {
                            Vector2 pos2 = new Vector2(i * 64 + xPosition, y * 64 + yPosition);
                            ItemDisplayButton b2 = new ItemDisplayButton(null, new StardustCore.Animations.AnimatedSprite("ItemBackground", pos2, new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "InventoryMenu", "ItemBackground"), new Animation(0, 0, 32, 32)), Color.White), pos2, new Rectangle(0, 0, 32 * 2, 32 * 2), 2f, true, Color.White);
                            this.storageDisplay.Add(b2);
                            continue;
                        }
                        Item item = this.getItemFromList(index);
                        Vector2 pos = new Vector2(i * 64 + xPosition, y * 64 + yPosition);
                        ItemDisplayButton b = new ItemDisplayButton(item, new StardustCore.Animations.AnimatedSprite("ItemBackground", pos, new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "InventoryMenu", "ItemBackground"), new Animation(0, 0, 32, 32)), Color.White), pos, new Rectangle(0, 0, 32 * 2, 32 * 2), 2f, true, Color.White);
                        this.storageDisplay.Add(b);
                    }
                }
            }
        }

        /// <summary>
        /// Gets an item from the list of items.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Item getItemFromList(int index)
        {
            if (this.items == null) return null;
            if (index >= this.items.Count) return null;
            else return this.items.ElementAt(index);
        }

        public override void performHoverAction(int x, int y)
        {

        }

        /// <summary>
        /// What happens when a key is pressed.
        /// </summary>
        /// <param name="key"></param>
        public override void receiveKeyPress(Keys key)
        {
            base.receiveKeyPress(key);
        }


        /// <summary>
        /// What happens when the menu is left clicked.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="playSound"></param>
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            int index = 0;
            Item swap = null;
            foreach (ItemDisplayButton button in this.storageDisplay)
            {
                if (button.receiveLeftClick(x, y))
                {
                    if (this.activeItem == null)
                    {
                        this.activeItem = button.item;
                        if (this.activeItem != null)
                        {
                            ModCore.log("Got item: " + this.activeItem.DisplayName);
                        }
                        return;
                    }
                    else if (this.activeItem != null)
                    {
                        if (button.item == null)
                        {
                            ModCore.log("Placed item: " + this.activeItem.DisplayName);
                            swap = this.activeItem;
                            this.activeItem = null;
                            break;
                        }
                        else
                        {
                            swap = button.item;
                            ModCore.log("Swap item: " + swap.DisplayName);
                            break;
                        }
                    }
                }
                index++;
            }
            if (swap != null && this.activeItem == null)
            {
                this.swapItemPosition(index, swap);
                swap = null;
            }
            else if (swap != null && this.activeItem != null)
            {
                this.swapItemPosition(index, this.activeItem);
                this.activeItem = null;
                swap = null;
            }

            Rectangle r = new Rectangle(this.searchBox.X, this.searchBox.Y, this.searchBox.Width, this.searchBox.Height/2);
            if (r.Contains(x, y))
            {
                this.searchBox.Update();
                this.searchBox.SelectMe();
            }
            else
            {
                this.searchBox.Selected = false;
            }
        }

        /// <summary>
        /// Swaps the item's position in the menu.
        /// </summary>
        /// <param name="insertIndex"></param>
        /// <param name="I"></param>
        public void swapItemPosition(int insertIndex, Item I)
        {
            if (I == null)
            {
                ModCore.log("Odd item is null");
                return;
            }
            if (insertIndex + 1 > this.items.Count)
            {
                this.items.Remove(I);
                this.items.Add(I);
                this.populateClickableItems(this.rows, this.collumns, this.xPositionOnScreen + this.xOffset, this.yPositionOnScreen + this.yOffset);
                return;
            }
            this.items.Insert(insertIndex + 1, I);
            this.items.Remove(I);
            this.populateClickableItems(this.rows, this.collumns, this.xPositionOnScreen + this.xOffset, this.yPositionOnScreen + this.yOffset);
        }

        /// <summary>
        /// Takes the active item from this menu.
        /// </summary>
        /// <returns></returns>
        public Item takeActiveItem()
        {
            this.items.Remove(this.activeItem);
            this.populateClickableItems(this.rows, this.collumns, this.xPositionOnScreen + this.xOffset, this.yPositionOnScreen + this.yOffset);
            Item i = this.activeItem;
            this.activeItem = null;
            return i;
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {

        }

        public override void update(GameTime time)
        {

        }

        public override void draw(SpriteBatch b)
        {
            this.drawDialogueBoxBackground(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, Color.Blue);

            foreach (ItemDisplayButton button in this.storageDisplay)
            {
                if (string.IsNullOrEmpty(this.searchBox.Text) == false)
                {
                    button.draw(b, 0.25f, this.getItemDrawAlpha(button.item), true);
                }
                else
                {
                    button.draw(b, 0.25f, 1f, true);
                }
            }

            this.searchBox.Draw(b, true);

            this.drawMouse(b);
            //base.draw(b);
        }

        private float getItemDrawAlpha(Item I)
        {
            if (I == null) return 1f;
            if (string.IsNullOrEmpty(this.searchBox.Text) == false)
            {
                return I.DisplayName.ToLowerInvariant().Contains(this.searchBox.Text.ToLowerInvariant()) ? 1f : 0.25f;
            }
            else
            {
                return 1f;
            }
        }
    }
}
