using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revitalize.Framework.Menus.MenuComponents;
using StardewValley;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons;

namespace Revitalize.Framework.Menus
{
    public class CraftingMenuV1 : IClickableMenuExtended
    {

        /// <summary>
        /// All the different pages for crafting.
        ///
        /// Sort recipes by recipe name.
        /// Add in search box
        /// </summary>
        public Dictionary<string, AnimatedButton> CraftingTabs;

        public Dictionary<string, List<CraftingRecipeButton>> craftingItemsToDisplay;
        public IList<Item> fromInventory;
        public IList<Item> toInventory;

        public int currentPageIndex;
        public string currentTab;

        public Color backgroundColor;

        public int xOffset = 72;

        public string hoverText;

        /// <summary>
        /// How many crafting recipes to display at a time.
        /// </summary>
        public int amountOfRecipesToShow = 9;

        public bool playerInventory;

        public CraftingInformationPage craftingInfo;

        public AnimatedButton leftButton;
        public AnimatedButton rightButton;


        private int maxPages
        {
            get
            {
                if (string.IsNullOrEmpty(this.currentTab)) return 0;
                return (int)(Math.Ceiling((double)(this.craftingItemsToDisplay[this.currentTab].Count / this.amountOfRecipesToShow)));
            }
        }

        public CraftingMenuV1() : base()
        {

        }

        /// <summary>
        /// Constructor to be used when the inventory is the player's
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="BackgroundColor"></param>
        /// <param name="Inventory"></param>
        public CraftingMenuV1(int X, int Y, int Width, int Height, Color BackgroundColor, IList<Item> Inventory) : base(X, Y, Width, Height, false)
        {
            this.backgroundColor = BackgroundColor;
            this.CraftingTabs = new Dictionary<string, AnimatedButton>();
            this.craftingItemsToDisplay = new Dictionary<string, List<CraftingRecipeButton>>();
            this.currentPageIndex = 0;
            this.fromInventory = Inventory;
            this.toInventory = Inventory;
            this.playerInventory = true;
            this.initializeButtons();
        }

        /// <summary>
        /// Constructor to be used when inventory destination is the same and not the player.
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="BackgroundColor"></param>
        /// <param name="Inventory"></param>
        public CraftingMenuV1(int X, int Y, int Width, int Height, Color BackgroundColor, ref IList<Item> Inventory) : base(X, Y, Width, Height, false)
        {
            this.backgroundColor = BackgroundColor;
            this.CraftingTabs = new Dictionary<string, AnimatedButton>();
            this.craftingItemsToDisplay = new Dictionary<string, List<CraftingRecipeButton>>();
            this.currentPageIndex = 0;
            this.fromInventory = Inventory;
            this.toInventory = Inventory;
            this.initializeButtons();
        }

        /// <summary>
        /// Inventory constructor to be used when the input and output inventories are different.
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="BackgroundColor"></param>
        /// <param name="FromInventory"></param>
        /// <param name="ToInventory"></param>
        public CraftingMenuV1(int X, int Y, int Width, int Height, Color BackgroundColor, ref IList<Item> FromInventory, ref IList<Item> ToInventory) : base(X, Y, Width, Height, false)
        {
            this.backgroundColor = BackgroundColor;
            this.CraftingTabs = new Dictionary<string, AnimatedButton>();
            this.craftingItemsToDisplay = new Dictionary<string, List<CraftingRecipeButton>>();
            this.currentPageIndex = 0;
            this.fromInventory = FromInventory;
            this.toInventory = ToInventory;
            this.initializeButtons();
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            if (this.craftingInfo != null)
            {
                this.craftingInfo.gameWindowSizeChanged(oldBounds, newBounds);

            }
        }

        private void initializeButtons()
        {
            this.leftButton = new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Left Button", new Vector2(this.xPositionOnScreen, this.yPositionOnScreen), new StardustCore.Animations.AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "InventoryMenu", "PreviousPageButton"), new StardustCore.Animations.Animation(0, 0, 32, 32)), Color.White), new Rectangle(0, 0, 32, 32), 2f);
            this.rightButton = new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Right Button", new Vector2(this.xPositionOnScreen + this.width, this.yPositionOnScreen), new StardustCore.Animations.AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "InventoryMenu", "NextPageButton"), new StardustCore.Animations.Animation(0, 0, 32, 32)), Color.White), new Rectangle(0, 0, 32, 32), 2f);
        }

        public void sortRecipes()
        {
            foreach(KeyValuePair<string,List<CraftingRecipeButton>> pair in this.craftingItemsToDisplay)
            {
                List<CraftingRecipeButton> copy = pair.Value.ToList();
                pair.Value.Clear();

                copy=copy.OrderBy(x => x.displayItem.item.DisplayName).ToList();
                foreach(CraftingRecipeButton b in copy)
                {
                    this.addInCraftingRecipe(b, pair.Key);
                }
            }
            
            
        }

        public void addInCraftingPageTab(string name, AnimatedButton Button)
        {
            int count = this.CraftingTabs.Count;

            if (this.CraftingTabs.ContainsKey(name))
            {
                return;
            }
            else
            {
                Vector2 newPos = new Vector2(100 + (48) * (count + 1), this.yPositionOnScreen + (24 * 4) * (count + 1));
                Button.Position = newPos;
                this.CraftingTabs.Add(name, Button);
                this.craftingItemsToDisplay.Add(name, new List<CraftingRecipeButton>());
            }

        }

        public void addInCraftingRecipe(CraftingRecipeButton Button, string WhichTab)
        {
            if (this.craftingItemsToDisplay.ContainsKey(WhichTab))
            {
                int count = this.craftingItemsToDisplay[WhichTab].Count % this.amountOfRecipesToShow;
                Vector2 newPos = new Vector2(this.xPositionOnScreen + (128), (this.yPositionOnScreen + 64) + (64 * (count + 1)));
                Button.displayItem.Position = newPos;
                this.craftingItemsToDisplay[WhichTab].Add(Button);
            }
            else
            {
                throw new Exception("Tab: " + WhichTab + " doesn't exist!");
            }
        }

        public override void performHoverAction(int x, int y)
        {
            bool hovered = false;
            foreach (KeyValuePair<string, AnimatedButton> pair in this.CraftingTabs)
            {
                if (pair.Value.containsPoint(x, y))
                {
                    this.hoverText = pair.Key;
                    hovered = true;
                }
            }

            //get range of buttons to show

            if (string.IsNullOrEmpty(this.currentTab) == false)
            {
                List<CraftingRecipeButton> buttonsToDraw = this.getRecipeButtonsToDisplay();
                foreach (CraftingRecipeButton button in buttonsToDraw)
                {
                    if (button.containsPoint(x, y))
                    {
                        this.hoverText = button.recipe.outputName;
                        hovered = true;
                    }
                }
            }
            if (hovered == false)
            {
                this.hoverText = "";
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (this.leftButton.containsPoint(x, y))
            {
                if (this.currentPageIndex <= 0) this.currentPageIndex = 0;
                else
                {
                    this.currentPageIndex--;
                    Game1.playSound("shwip");
                }
            }
            if (this.rightButton.containsPoint(x, y))
            {
                if (this.currentPageIndex < this.maxPages)
                {
                    this.currentPageIndex++;
                    Game1.playSound("shwip");
                }
            }

            foreach (KeyValuePair<string, AnimatedButton> pair in this.CraftingTabs)
            {
                if (pair.Value.containsPoint(x, y))
                {
                    this.currentTab = pair.Key;
                    this.currentPageIndex = 0;
                    return;
                }
            }

            //get range of buttons to show

            if (string.IsNullOrEmpty(this.currentTab) == false)
            {
                List<CraftingRecipeButton> buttonsToDraw = this.getRecipeButtonsToDisplay();
                foreach (CraftingRecipeButton button in buttonsToDraw)
                {
                    if (button.containsPoint(x, y))
                    {
                        //button.craftItem(this.fromInventory, this.toInventory);

                        if (this.playerInventory)
                        {
                            this.fromInventory = Game1.player.Items;
                        }

                        this.craftingInfo = new CraftingInformationPage(this.xPositionOnScreen + this.width + this.xOffset, this.yPositionOnScreen, 400, this.height, this.backgroundColor, button, ref this.fromInventory, this.playerInventory);
                        Game1.soundBank.PlayCue("coin");
                        if (this.playerInventory)
                        {
                            Game1.player.Items = this.toInventory;
                            return;
                        }

                    }
                }
            }

            if (this.craftingInfo != null)
            {
                this.craftingInfo.receiveLeftClick(x, y);
                if (this.craftingInfo.doesMenuContainPoint(x, y)) return;
            }
            this.craftingInfo = null;
        }


        public override void draw(SpriteBatch b)
        {
            this.drawDialogueBoxBackground(this.xPositionOnScreen + this.xOffset, this.yPositionOnScreen, this.width, this.height, this.backgroundColor);

            this.leftButton.draw(b);
            //Draw page numbers here.
            //b.DrawString(Game1.smallFont,"Page: "+this.currentPageIndex.ToString()/)
            b.DrawString(Game1.dialogueFont, ("Page: " + (this.currentPageIndex + 1) + " / " + (this.maxPages+1)).ToString(), new Vector2(this.xPositionOnScreen + 128, this.yPositionOnScreen), Color.White);
            this.rightButton.draw(b);

            //this.drawDialogueBoxBackground();
            foreach (KeyValuePair<string, AnimatedButton> pair in this.CraftingTabs)
            {
                pair.Value.draw(b);
            }

            if (string.IsNullOrEmpty(this.currentTab))
            {
                if (string.IsNullOrEmpty(this.hoverText) == false)
                {
                    IClickableMenuExtended.drawHoverText(b, this.hoverText, Game1.dialogueFont);
                }
                this.drawMouse(b);
                return;
            }

            List<CraftingRecipeButton> buttonsToDraw = this.getRecipeButtonsToDisplay();

            foreach (CraftingRecipeButton button in buttonsToDraw)
            {
                if (button.recipe.CanCraft(this.fromInventory))
                {
                    button.draw(b);
                }
                else
                {
                    button.draw(b, .25f);
                }

                b.DrawString(Game1.smallFont, button.displayItem.item.DisplayName, button.displayItem.Position + new Vector2(64, 0), Color.Brown);
            }

            if (this.craftingInfo != null)
            {
                this.craftingInfo.draw(b);
            }

            if (string.IsNullOrEmpty(this.hoverText) == false)
            {
                IClickableMenuExtended.drawHoverText(b, this.hoverText, Game1.dialogueFont);
            }

            this.drawMouse(b);
        }

        public override void update(GameTime time)
        {
            base.update(time);
        }

        public List<CraftingRecipeButton> getRecipeButtonsToDisplay()
        {

            int amount = this.craftingItemsToDisplay[this.currentTab].Count / this.amountOfRecipesToShow;
            int min = this.currentPageIndex == amount ? this.craftingItemsToDisplay[this.currentTab].Count % this.amountOfRecipesToShow : this.amountOfRecipesToShow;
            List<CraftingRecipeButton> buttonsToDraw = this.craftingItemsToDisplay[this.currentTab].GetRange(this.currentPageIndex * this.amountOfRecipesToShow, min);
            return buttonsToDraw;
        }

    }
}
