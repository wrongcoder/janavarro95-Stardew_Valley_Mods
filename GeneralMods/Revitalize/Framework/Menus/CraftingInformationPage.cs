using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revitalize.Framework.Crafting;
using Revitalize.Framework.Menus.MenuComponents;
using Revitalize.Framework.Objects;
using Revitalize.Framework.Objects.Machines;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons;

namespace Revitalize.Framework.Menus
{
    /// <summary>
    /// Need to display a craft button.
    /// Also need to make the crafting menu scroll better.
    /// </summary>
    public class CraftingInformationPage:IClickableMenuExtended
    {

        public CraftingRecipeButton infoButton;
        public Color backgroundColor;

        public Vector2 itemDisplayLocation;

        public IList<Item> inventory;
        public IList<Item> outputInventory;

        private Dictionary<ItemDisplayButton,int> requiredItems;

        public AnimatedButton craftingButton;

        public bool isPlayerInventory;

        private Machine machine;

        string hoverText;

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

        public CraftingInformationPage(int x, int y, int width, int height,Color BackgroundColor,CraftingRecipeButton ItemToDisplay,ref IList<Item> Inventory,bool IsPlayerInventory) : base(x, y, width, height, false)
        {
            this.backgroundColor = BackgroundColor;
            this.infoButton = ItemToDisplay;
            this.itemDisplayLocation = new Vector2(this.xPositionOnScreen + (this.width / 2) - 32, this.yPositionOnScreen + (128));
            this.inventory = Inventory;
            this.isPlayerInventory = IsPlayerInventory;

            this.requiredItems = new Dictionary<ItemDisplayButton, int>();
            for (int i = 0; i < this.infoButton.recipe.ingredients.Count; i++)
            {
                ItemDisplayButton b = new ItemDisplayButton(this.infoButton.recipe.ingredients.ElementAt(i).item, null, new Vector2(this.xPositionOnScreen + 64, this.getIngredientHeightOffset().Y), new Rectangle(0, 0, 64, 64), 2f, true, Color.White);
                this.requiredItems.Add(b, this.infoButton.recipe.ingredients.ElementAt(i).requiredAmount);
            }
            this.craftingButton = new AnimatedButton(new StardustCore.Animations.AnimatedSprite("CraftingButton", new Vector2(this.xPositionOnScreen + this.width / 2-96, this.getCraftingButtonHeight()),new StardustCore.Animations.AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "CraftingMenu", "CraftButton"),new StardustCore.Animations.Animation(0,0,48,16)), Color.White),new Rectangle(0,0,48,16),4f);
            this.outputInventory = this.inventory;
        }

        public CraftingInformationPage(int x, int y, int width, int height, Color BackgroundColor, CraftingRecipeButton ItemToDisplay, ref IList<Item> Inventory,ref IList<Item> OutputInventory ,bool IsPlayerInventory, Machine Machine) : base(x, y, width, height, false)
        {
            this.backgroundColor = BackgroundColor;
            this.infoButton = ItemToDisplay;
            this.itemDisplayLocation = new Vector2(this.xPositionOnScreen + (this.width / 2) - 32, this.yPositionOnScreen + (128));
            this.inventory = Inventory;
            this.isPlayerInventory = IsPlayerInventory;

            this.requiredItems = new Dictionary<ItemDisplayButton, int>();
            for (int i = 0; i < this.infoButton.recipe.ingredients.Count; i++)
            {
                ItemDisplayButton b = new ItemDisplayButton(this.infoButton.recipe.ingredients.ElementAt(i).item, null, new Vector2(this.xPositionOnScreen + 64, this.getIngredientHeightOffset().Y), new Rectangle(0, 0, 64, 64), 2f, true, Color.White);
                this.requiredItems.Add(b, this.infoButton.recipe.ingredients.ElementAt(i).requiredAmount);
            }
            this.craftingButton = new AnimatedButton(new StardustCore.Animations.AnimatedSprite("CraftingButton", new Vector2(this.xPositionOnScreen + this.width / 2 - 96, this.getCraftingButtonHeight()), new StardustCore.Animations.AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "CraftingMenu", "CraftButton"), new StardustCore.Animations.Animation(0, 0, 48, 16)), Color.White), new Rectangle(0, 0, 48, 16), 4f);

            if (OutputInventory == null)
            {
                this.outputInventory = this.inventory;
            }
            this.outputInventory = OutputInventory;
            this.machine = Machine;
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (this.craftingButton.containsPoint(x, y))
            {
                if (this.canCraftRecipe())
                {
                    Game1.soundBank.PlayCue("coin");

                    this.infoButton.craftItem(this.inventory, this.outputInventory);
                    if (this.machine != null)
                    {
                        if (this.infoButton.recipe.timeToCraft == 0)
                        {
                            this.machine.InventoryManager.dumpBufferToItems();
                        }
                        else
                        {
                            this.machine.MinutesUntilReady = this.infoButton.recipe.timeToCraft;
                        }
                    }

                    if (this.isPlayerInventory)
                    {
                        this.inventory = Game1.player.Items;
                    }
                }
            }
        }

        public override void performHoverAction(int x, int y)
        {
            bool hovered = false;
            if (this.craftingButton.containsPoint(x, y))
            {
                if (this.infoButton.recipe.CanCraft(this.inventory) == false)
                {
                    this.hoverText = "Not enough items.";
                    hovered = true;
                }
                if (this.machine != null)
                {
                    if (this.machine.MinutesUntilReady > 0)
                    {
                        this.hoverText = "Crafting in progress...";
                        hovered = true;
                    }
                    if (this.machine.MinutesUntilReady == 0 && this.machine.InventoryManager.hasItemsInBuffer)
                    {
                        this.hoverText = "Items in buffer. Please make room in the inventory for: " + System.Environment.NewLine + this.machine.InventoryManager + " items.";
                        hovered = true;
                    }
                    if (this.machine.InventoryManager.IsFull)
                    {
                        this.hoverText = "Inventory is full!";
                        hovered = true;
                    }
                }
            }
            if (hovered == false)
            {
                this.hoverText = "";
            }
        }

        public override void draw(SpriteBatch b)
        {
            this.drawDialogueBoxBackground(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, this.backgroundColor);
            this.infoButton.draw(b,this.itemDisplayLocation);

            b.DrawString(Game1.dialogueFont, this.actualItem.DisplayName,new Vector2(this.xPositionOnScreen+ (this.width/2),this.itemDisplayLocation.Y)+ this.getHeightOffsetFromItem()-this.getItemNameOffset(), this.getNameColor());

            b.DrawString(Game1.smallFont, Game1.parseText(this.actualItem.getDescription(), Game1.smallFont, this.width),new Vector2(this.xPositionOnScreen+64,this.getItemDescriptionOffset().Y), Color.Black);

            foreach(KeyValuePair<ItemDisplayButton,int> button in this.requiredItems)
            {
                button.Key.draw(b);
                b.DrawString(Game1.smallFont, button.Key.item.DisplayName+ " x "+button.Value.ToString(), button.Key.Position + new Vector2(64, 16), this.getNameColor(button.Key.item, button.Value));
            }

            this.craftingButton.draw(b, this.getCraftableColor().A);

            this.drawMouse(b);
        }

        public bool doesMenuContainPoint(int x, int y)
        {
            Rectangle r = new Rectangle(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height);
            return r.Contains(x, y);
        }

        public bool canCraftRecipe()
        {
            bool canCraft = true;
            if (this.infoButton.recipe.CanCraft(this.inventory) == false) canCraft = false;

            if (this.machine != null)
            {
                if (this.machine.InventoryManager.hasItemsInBuffer) canCraft = false;
                if (this.machine.InventoryManager.IsFull) canCraft = false;
            }

            return canCraft;
        }

        /// <summary>
        /// Gets the color for the crafting button.
        /// </summary>
        /// <returns></returns>
        private Color getCraftableColor()
        {
            if (this.canCraftRecipe()) return Color.White;
            else return new Color(1f, 1f, 1f, 0.25f);
        }

        public Color getNameColor()
        {
            if (this.canCraftRecipe()) return Color.Black;
            else return Color.Red;
        }

        public Color getNameColor(Item I, int amount)
        {
            CraftingRecipeComponent Pair = new CraftingRecipeComponent(I, amount);
               
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
            length.X = length.X / 2;
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

        private float getCraftingButtonHeight()
        {
            return this.yPositionOnScreen + this.height - 64*2;
        }

    }
}
