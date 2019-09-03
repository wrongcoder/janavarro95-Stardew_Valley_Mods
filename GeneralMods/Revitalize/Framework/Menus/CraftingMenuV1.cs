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
        /// </summary>
        public Dictionary<string, AnimatedButton> CraftingTabs;

        public Dictionary<string, List<CraftingRecipeButton>> craftingItemsToDisplay;
        public IList<Item> fromInventory;
        public IList<Item> toInventory;

        public int currentPageIndex;
        public string currentTab;
        public int currentScrollIndex;

        public Color backgroundColor;

        public int xOffset = 72;

        public string hoverText;

        /// <summary>
        /// How many crafting recipes to display at a time.
        /// </summary>
        public int amountOfRecipesToShow = 4;

        public bool playerInventory;

        public CraftingInformationPage craftingInfo;

        public CraftingMenuV1() : base()
        {

        }

        public CraftingMenuV1(int X, int Y, int Width, int Height, Color BackgroundColor, IList<Item> Inventory) : base(X, Y, Width, Height, false)
        {
            this.backgroundColor = BackgroundColor;
            this.CraftingTabs = new Dictionary<string, AnimatedButton>();
            this.craftingItemsToDisplay = new Dictionary<string, List<CraftingRecipeButton>>();
            this.currentPageIndex = 0;
            this.fromInventory = Inventory;
            this.toInventory = Inventory;
            this.playerInventory = true;
        }

        public CraftingMenuV1(int X, int Y, int Width, int Height, Color BackgroundColor, ref IList<Item> Inventory) : base(X, Y, Width, Height, false)
        {
            this.backgroundColor = BackgroundColor;
            this.CraftingTabs = new Dictionary<string, AnimatedButton>();
            this.craftingItemsToDisplay = new Dictionary<string, List<CraftingRecipeButton>>();
            this.currentPageIndex = 0;
            this.fromInventory = Inventory;
            this.toInventory = Inventory;
        }

        public CraftingMenuV1(int X, int Y, int Width, int Height, Color BackgroundColor, ref IList<Item> FromInventory, ref IList<Item> ToInventory) : base(X, Y, Width, Height, false)
        {
            this.backgroundColor = BackgroundColor;
            this.CraftingTabs = new Dictionary<string, AnimatedButton>();
            this.craftingItemsToDisplay = new Dictionary<string, List<CraftingRecipeButton>>();
            this.currentPageIndex = 0;
            this.fromInventory = FromInventory;
            this.toInventory = ToInventory;
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
                Vector2 newPos = new Vector2(100 + (48) * (count + 1), 100 + (24 * 4) * (count + 1));
                Button.Position = newPos;
                this.CraftingTabs.Add(name, Button);
                this.craftingItemsToDisplay.Add(name, new List<CraftingRecipeButton>());
            }

        }

        public void addInCraftingRecipe(CraftingRecipeButton Button, string WhichTab)
        {
            if (this.craftingItemsToDisplay.ContainsKey(WhichTab))
            {
                int count = this.craftingItemsToDisplay.Count;
                Vector2 newPos = new Vector2(100 + (64) * (count + 1), 100 + (16 * 4) * (count + 1));
                Button.displayItem.Position = newPos;
                this.craftingItemsToDisplay[WhichTab].Add(Button);
            }
            else
            {
                throw new Exception("Tab: " + WhichTab + " doesn't exist!");
            }
        }

        public override void receiveScrollWheelAction(int direction)
        {

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
            foreach (KeyValuePair<string, AnimatedButton> pair in this.CraftingTabs)
            {
                if (pair.Value.containsPoint(x, y))
                {
                    this.currentTab = pair.Key;
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
                        this.craftingInfo = new CraftingInformationPage(this.xPositionOnScreen + this.width+this.xOffset, this.yPositionOnScreen, 400, this.height, this.backgroundColor, button,ref this.fromInventory);
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
            List<CraftingRecipeButton> buttonsToDraw = this.craftingItemsToDisplay[this.currentTab].GetRange(this.currentScrollIndex, Math.Min(this.craftingItemsToDisplay[this.currentTab].Count, this.amountOfRecipesToShow));
            return buttonsToDraw;
        }

    }
}
