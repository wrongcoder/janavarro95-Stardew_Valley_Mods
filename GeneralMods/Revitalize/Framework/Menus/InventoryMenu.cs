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
using StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons;

namespace Revitalize.Framework.Menus
{
    public class InventoryMenuPage
    {

        int index;
        public List<ItemDisplayButton> storageDisplay;
        public int amountToDisplay;

        public InventoryMenuPage()
        {

        }

        public InventoryMenuPage(int index, List<ItemDisplayButton> Buttons, int AmountToDisplay)
        {
            this.index = index;
            this.storageDisplay = Buttons;
            this.amountToDisplay = AmountToDisplay;
        }
    }

    /// <summary>
    /// //TODO: Combine two of these to make an item grab menu.
    /// TODO: Display Item information on hover.
    /// </summary>
    public class InventoryMenu : IClickableMenuExtended
    {
        public IList<Item> items;
        public int capacity;
        public Item activeItem;

        public int rows;
        public int collumns;
        public int xOffset = 64;
        public int yOffset = 128;

        public StardewValley.Menus.TextBox searchBox;

        public Dictionary<int, InventoryMenuPage> pages;
        public int pageIndex = 0;

        public AnimatedButton nextPage;
        public AnimatedButton previousPage;

        public InventoryMenu(int xPos, int yPos, int width, int height, int Rows, int Collumns, bool showCloseButton, IList<Item> Inventory, int maxCapacity) : base(xPos, yPos, width, height, showCloseButton)
        {
            //Amount to display is the lower cap per page.
            //

            this.items = Inventory;
            this.pages = new Dictionary<int, InventoryMenuPage>();
            this.capacity = maxCapacity;
            this.rows = Rows;
            this.collumns = Collumns;
            this.populateClickableItems(this.rows, this.collumns, xPos + this.xOffset, yPos + this.yOffset);

            this.searchBox = new TextBox((Texture2D)null, (Texture2D)null, Game1.dialogueFont, Game1.textColor);
            this.searchBox.X = this.xPositionOnScreen;
            this.searchBox.Y = this.yPositionOnScreen;
            this.searchBox.Width = 256;
            this.searchBox.Height = 192;
            Game1.keyboardDispatcher.Subscriber = (IKeyboardSubscriber)this.searchBox;

            this.nextPage = new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Next Page", new Vector2(128 + (this.searchBox.X + this.searchBox.Width), this.searchBox.Y),new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "InventoryMenu", "NextPageButton"),new Animation(0,0,32,32)),Color.White),new Rectangle(0, 0, 32, 32), 2f);
            this.previousPage= new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Previous Page", new Vector2(64 + (this.searchBox.X + this.searchBox.Width), this.searchBox.Y), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "InventoryMenu", "PreviousPageButton"), new Animation(0, 0, 32, 32)), Color.White), new Rectangle(0, 0, 32, 32), 2f);

        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);

            this.searchBox.X = this.xPositionOnScreen;
            this.searchBox.Y = this.yPositionOnScreen;
            this.populateClickableItems(this.rows, this.collumns, this.xPositionOnScreen + this.xOffset, this.yPositionOnScreen + this.yOffset);
            this.nextPage.Position = new Vector2(128 + (this.searchBox.X + this.searchBox.Width));
            this.previousPage.Position= new Vector2(64 + (this.searchBox.X + this.searchBox.Width));
        }

        public void populateClickableItems(int rows, int collums, int xPosition, int yPosition)
        {
            this.pages.Clear();


            int size = this.capacity;
            ModCore.log("Hello World! SIZE IS: " + size);

            int maxPages = ((size) / (this.rows * this.collumns)) + 1;
            for (int i = 0; i < maxPages; i++)
            {
                int amount = Math.Min(rows * collums, size);
                this.pages.Add(i, new InventoryMenuPage(i, new List<ItemDisplayButton>(), amount));
                ModCore.log("Added in a new page with size: " + size);
                size -= amount;
                for (int y = 0; y < collums; y++)
                {
                    for (int x = 0; x < rows; x++)
                    {
                        int index = ((y * rows) + x) + (rows * collums * i);
                        if (index >= this.pages[i].amountToDisplay + (rows * collums * i))
                        {
                            ModCore.log("Break page creation.");
                            ModCore.log("Index is: " + index);
                            ModCore.log("Max display is: " + this.pages[i].amountToDisplay);
                            break;
                        }

                        if (index > this.items.Count)
                        {
                            ModCore.log("Index greater than items!");
                            Vector2 pos2 = new Vector2(x * 64 + xPosition, y * 64 + yPosition);
                            ItemDisplayButton b2 = new ItemDisplayButton(null, new StardustCore.Animations.AnimatedSprite("ItemBackground", pos2, new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "InventoryMenu", "ItemBackground"), new Animation(0, 0, 32, 32)), Color.White), pos2, new Rectangle(0, 0, 32 * 2, 32 * 2), 2f, true, Color.White);
                            this.pages[i].storageDisplay.Add(b2);
                            continue;
                        }

                        ModCore.log("Add in a new display item");
                        Item item = this.getItemFromList(index);
                        Vector2 pos = new Vector2(x * 64 + xPosition, y * 64 + yPosition);
                        ItemDisplayButton b = new ItemDisplayButton(item, new StardustCore.Animations.AnimatedSprite("ItemBackground", pos, new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "InventoryMenu", "ItemBackground"), new Animation(0, 0, 32, 32)), Color.White), pos, new Rectangle(0, 0, 32 * 2, 32 * 2), 2f, true, Color.White);
                        this.pages[i].storageDisplay.Add(b);
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
            int index = 0 + (this.rows * this.collumns * this.pageIndex);
            Item swap = null;
            foreach (ItemDisplayButton button in this.pages[this.pageIndex].storageDisplay)
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

            Rectangle r = new Rectangle(this.searchBox.X, this.searchBox.Y, this.searchBox.Width, this.searchBox.Height / 2);
            if (r.Contains(x, y))
            {
                this.searchBox.Update();
                this.searchBox.SelectMe();
            }
            else
            {
                this.searchBox.Selected = false;
            }

            if (this.nextPage.containsPoint(x, y))
            {
                ModCore.log("Left click next page");
                if (this.pageIndex + 1 < this.pages.Count)
                {
                    this.pageIndex++;
                    Game1.soundBank.PlayCue("shwip");
                }
            }
            if (this.previousPage.containsPoint(x, y))
            {
                ModCore.log("Left click previous page");
                if (this.pageIndex > 0)
                {
                    this.pageIndex--;
                    Game1.soundBank.PlayCue("shwip");
                }
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

            foreach (ItemDisplayButton button in this.pages[this.pageIndex].storageDisplay)
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

            this.nextPage.draw(b,0.25f);
            this.previousPage.draw(b,0.25f);

            b.DrawString(Game1.dialogueFont, ("Page: " + (this.pageIndex + 1) + " / " + this.pages.Count).ToString(), new Vector2(this.xPositionOnScreen, this.yPositionOnScreen + this.height), Color.White);

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
