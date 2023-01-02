using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Managers;
using Omegasis.Revitalize.Framework.Menus.MenuComponents;
using Omegasis.Revitalize.Framework.Utilities.JsonContentLoading;
using Omegasis.Revitalize.Framework.World.Buildings;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using Omegasis.StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons;
using StardewValley;
using StardewValley.Menus;

namespace Omegasis.Revitalize.Framework.Menus.Items
{
    public class DimensionalStorageUnitMenu : IClickableMenu
    {
        /*********
       ** Fields
       *********/
        /// <summary>The labels to draw.</summary>
        private readonly List<ClickableComponent> Labels = new List<ClickableComponent>();
        private List<ItemDisplayButton> itemButtons = new List<ItemDisplayButton>();

        /// <summary>The OK button to draw.</summary>
        private ClickableTextureComponent OkButton;

        /// <summary>
        /// The item that is currently being hover overed.
        /// </summary>
        public Item currentHoverTextureItem;

        public bool allFinished;

        private int currentPageNumber;

        private ClickableTextureComponent _leftButton;
        private ClickableTextureComponent _rightButton;

        private ClickableTextureComponent _searchModeButton;

        private ClickableTextureComponent _upgradeButton;

        private int _maxRowsToDisplay = 5;
        private int _maxColumnsToDisplay = 6;

        /// <summary>
        /// The search box used for looking for specific items.
        /// </summary>
        public ItemSearchTextBox searchBox;

        public static int menuWidth = 632 + borderWidth * 2;
        public static int menuHeight = 600 + borderWidth * 2 + Game1.tileSize;

        public StardewValley.Menus.InventoryMenu playersInventory;

        public string hoverText;

        protected string defaultTitle;
        public ClickableComponent capacityDisplayComponent;

        private string upgradePromptUnformatted;
        private string upgradePrompt;
        private Item upgradeItem;
        private string missingItemPrompt;

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="season">The initial birthday season.</param>
        /// <param name="day">The initial birthday day.</param>
        /// <param name="onChanged">The callback to invoke when the birthday value changes.</param>
        public DimensionalStorageUnitMenu()
            : base((int)getAppropriateMenuPosition().X, (int)getAppropriateMenuPosition().Y, menuWidth, menuHeight)
        {
            this.searchBox = new ItemSearchTextBox(null, null, Game1.dialogueFont, Game1.textColor, new Rectangle(0, 0, 0, 0));
            Game1.keyboardDispatcher.Subscriber = this.searchBox;
            this.searchBox.Selected = false;
            this.searchBox.onTextReceived += this.SearchBox_onTextReceived;
            this.searchBox.OnBackspacePressed += this.SearchBox_OnBackspacePressed;

            this.upgradeItem = RevitalizeModCore.ModContentManager.objectManager.getItem(Enums.SDVObject.VoidEssence);
            this.upgradePromptUnformatted = JsonContentPackUtilities.LoadStringFromDictionaryFile(Path.Combine(Constants.PathConstants.StringsPaths.Menus, "DimensionalStorageUnit.json"), "UpgradePrompt");
            this.missingItemPrompt = JsonContentPackUtilities.LoadStringFromDictionaryFile(Path.Combine(Constants.PathConstants.StringsPaths.Menus, "DimensionalStorageUnit.json"), "MissingUpgradeItem");

            this.defaultTitle = JsonContentPackUtilities.LoadStringFromDictionaryFile(Path.Combine(Constants.PathConstants.StringsPaths.Menus, "DimensionalStorageUnit.json"), "Capacity");



            this.setUpPositions();

        }

        public static Vector2 getAppropriateMenuPosition()
        {
            Vector2 defaultPosition = new Vector2(Game1.viewport.Width / 2 - menuWidth / 2, (Game1.viewport.Height / 2 - menuHeight / 2));

            //Force the viewport into a position that it should fit into on the screen???
            if (defaultPosition.X + menuWidth > Game1.viewport.Width)
            {
                defaultPosition.X = 0;
            }

            if (defaultPosition.Y + menuHeight > Game1.viewport.Height)
            {
                defaultPosition.Y = 0;
            }
            return defaultPosition;

        }

        private void SearchBox_OnBackspacePressed(TextBox sender)
        {
            SoundUtilities.PlaySound(Enums.StardewSound.Cowboy_gunshot);
            this.searchBox.backSpacePressed();
            this.populateItemsToDisplay();
        }

        private void SearchBox_onTextReceived(object sender, string e)
        {
            SoundUtilities.PlaySound(Enums.StardewSound.Cowboy_gunshot);
            this.populateItemsToDisplay();
        }

        /// <summary>The method called when the game window changes size.</summary>
        /// <param name="oldBounds">The former viewport.</param>
        /// <param name="newBounds">The new viewport.</param>
        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            this.xPositionOnScreen = Game1.viewport.Width / 2 - (632 + borderWidth * 2) / 2;
            this.yPositionOnScreen = Game1.viewport.Height / 2 - (600 + borderWidth * 2) / 2 - Game1.tileSize;

            this.setUpPositions();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Regenerate the UI.</summary>
        private void setUpPositions()
        {
            this.OkButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - borderWidth - spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - borderWidth - spaceToClearTopBorder + Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), "", null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46), 1f);
            this._leftButton = new ClickableTextureComponent("LeftButton", new Rectangle(this.xPositionOnScreen + this.width - borderWidth - spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - borderWidth - spaceToClearTopBorder + Game1.tileSize / 4 + 96, Game1.tileSize, Game1.tileSize), "", null, TextureManagers.Menus_InventoryMenu.getExtendedTexture("PreviousPageButton").getTexture(), new Rectangle(0, 0, 32, 32), 2f);
            this._rightButton = new ClickableTextureComponent("RightButton", new Rectangle(this.xPositionOnScreen + this.width - borderWidth - spaceToClearSideBorder - Game1.tileSize + 96, this.yPositionOnScreen + this.height - borderWidth - spaceToClearTopBorder + Game1.tileSize / 4 + 96, Game1.tileSize, Game1.tileSize), "", null, TextureManagers.Menus_InventoryMenu.getExtendedTexture("NextPageButton").getTexture(), new Rectangle(0, 0, 32, 32), 2f);

            this.capacityDisplayComponent = new ClickableComponent(new Rectangle(this.xPositionOnScreen + 128, this.yPositionOnScreen + 128, 1, 1), "");

            this.searchBox.Bounds = new Rectangle(this.xPositionOnScreen + 96, this.yPositionOnScreen, 256, 192);

            this._searchModeButton = new ClickableTextureComponent("SearchMode", new Rectangle(this.searchBox.X - 96, this.searchBox.Y, 64, 64), "", "", TextureManagers.Menus_InventoryMenu.getExtendedTexture("SearchButton").getTexture(), new Rectangle(0, 0, 32, 32), 2f);

            this._upgradeButton = new ClickableTextureComponent("UpgradeButton", new Rectangle(this.searchBox.X + this.searchBox.Width + 96, this.searchBox.Y, 192, 48), "", "", TextureManagers.Menus_DimensionalStorageMenu.getExtendedTexture("UpgradeButton").getTexture(), new Rectangle(0, 0, 192, 48), 1f);

            this.populateItemsToDisplay();

            int yPositionForInventory = this.yPositionOnScreen + this.height + 96;
            int xPositionForInventory = this.xPositionOnScreen;
            this.playersInventory = new StardewValley.Menus.InventoryMenu(xPositionForInventory, yPositionForInventory, true, null, null);

        }

        /// <summary>Handle a button click.</summary>
        /// <param name="name">The button name that was clicked.</param>
        private void handleButtonClick(string name)
        {
            if (name == null)
                return;

            switch (name)
            {
                // OK button
                case "OK":
                    this.allFinished = true;
                    SoundUtilities.PlaySound(Enums.StardewSound.coin);
                    Game1.exitActiveMenu();
                    return;

                case "LeftButton":
                    if (this.currentPageNumber == 0) break;
                    else
                    {
                        this.currentPageNumber--;
                        this.setUpPositions();
                    }
                    SoundUtilities.PlaySound(Enums.StardewSound.shwip);
                    return;

                case "RightButton":
                    NetObjectList<Item> ids = DimensionalStorageUnitBuilding.UniversalItems;
                    int value = (this.currentPageNumber + 1) * this._maxRowsToDisplay * this._maxColumnsToDisplay;
                    if (value >= ids.Count) break;
                    else
                    {
                        this.currentPageNumber++;
                        this.setUpPositions();
                    }
                    SoundUtilities.PlaySound(Enums.StardewSound.shwip);
                    return;

                case "SearchMode":
                    this.searchBox.cycleSearchMode();
                    SoundUtilities.PlaySound(Enums.StardewSound.coin);
                    this.populateItemsToDisplay();
                    return;


                case "UpgradeButton":

                    if (this.hasEnoughToUpgrade())
                    {
                        ulong found = this.getUpgradeItemAmount();
                        List<Item> dimensionalStorageVoidEssence = new List<Item>();
                        foreach (Item voidEssence in DimensionalStorageUnitBuilding.UniversalItems)
                        {
                            if (voidEssence.canStackWith(this.upgradeItem))
                            {
                                dimensionalStorageVoidEssence.Add(voidEssence);
                            }
                        }
                        bool updadeDimensionalStorageUnitDisplay = false;
                        foreach (Item voidEssence in dimensionalStorageVoidEssence)
                        {
                            if (voidEssence.Stack == 1)
                            {
                                //There's an odd edge case where the stack is truely never 0 so we need to have some special handling logic here.
                                DimensionalStorageUnitBuilding.UniversalItems.Remove(voidEssence);
                                updadeDimensionalStorageUnitDisplay = true;
                                found--;
                            }
                            else
                            {
                                ulong subTractionAmount = Math.Min((ulong)voidEssence.Stack, DimensionalStorageUnitBuilding.GetUpgradeCost());
                                voidEssence.Stack -= (int)subTractionAmount; //Stacks should cap at about 999 anyways so no need to worry about conversion losses here.
                                if (voidEssence.Stack == 0)
                                {
                                    DimensionalStorageUnitBuilding.UniversalItems.Remove(voidEssence);
                                    updadeDimensionalStorageUnitDisplay = true;
                                }
                                found -= subTractionAmount;
                            }

                            if (found == 0)
                            {
                                break;
                            }
                        }
                        foreach (Item item in Game1.player.items)
                        {
                            if (item == null) continue;
                            if (!item.canStackWith(this.upgradeItem))
                            {
                                continue;
                            }

                            if (item.Stack == 1)
                            {
                                //There's an odd edge case where the stack is truely never 0 so we need to have some special handling logic here.
                                Game1.player.items.Remove(item);
                                found--;
                            }
                            else
                            {
                                ulong subTractionAmount = Math.Min((ulong)item.Stack, DimensionalStorageUnitBuilding.GetUpgradeCost());
                                item.Stack -= (int)subTractionAmount; //Stacks should cap at about 999 anyways so no need to worry about conversion losses here.
                                if (item.Stack == 0)
                                {
                                    Game1.player.items.Remove(item);
                                }
                                found -= subTractionAmount;
                            }
                            if (found == 0)
                            {
                                break;
                            }
                        }
                        DimensionalStorageUnitBuilding.DimensionalStorageUnitMaxItems++;
                        if (updadeDimensionalStorageUnitDisplay)
                        {
                            this.populateItemsToDisplay();
                        }
                        this.updateUpgradeAndCapacityTexts();
                    }
                    else
                    {
                        SoundUtilities.PlaySound(Enums.StardewSound.Cowboy_gunshot);
                        return;
                    }

                    break;

                default:
                    break;
            }
            SoundUtilities.PlaySound(Enums.StardewSound.coin);
        }

        /// <summary>
        /// Gets the total amount of items found in dimensional storage unti that can be used for upgrades. In this case, void essence.
        /// </summary>
        /// <returns></returns>
        private ulong getUpgradeItemAmount()
        {
            List<Item> dimensionalStorageVoidEssence = new List<Item>();
            foreach (Item voidEssence in DimensionalStorageUnitBuilding.UniversalItems)
            {
                if (voidEssence.canStackWith(this.upgradeItem))
                {
                    dimensionalStorageVoidEssence.Add(voidEssence);
                }
            }

            ulong found = 0;
            foreach (Item voidEssence in dimensionalStorageVoidEssence)
            {
                found += (ulong)voidEssence.Stack;
            }
            foreach (Item voidEssence in Game1.player.items)
            {
                if (voidEssence == null) continue;
                if (voidEssence.canStackWith(this.upgradeItem))
                {
                    found += (ulong)voidEssence.Stack;
                }
            }
            return found;
        }

        /// <summary>
        /// Is there enough items to upgrade the dimensional storage unit?
        /// </summary>
        /// <returns></returns>
        private bool hasEnoughToUpgrade()
        {
            return this.getUpgradeItemAmount() >= DimensionalStorageUnitBuilding.GetUpgradeCost();
        }

        private void updateUpgradeAndCapacityTexts()
        {
            this.capacityDisplayComponent.name = string.Format(this.defaultTitle, DimensionalStorageUnitBuilding.UniversalItems.Count.ToString(), DimensionalStorageUnitBuilding.DimensionalStorageUnitMaxItems);
            this.upgradePrompt = string.Format(this.upgradePromptUnformatted, (DimensionalStorageUnitBuilding.DimensionalStorageUnitMaxItems + 1).ToString(), DimensionalStorageUnitBuilding.GetUpgradeCost().ToString(), this.upgradeItem.DisplayName);
        }

        /// <summary>The method invoked when the player left-clicks on the menu.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        /// <param name="playSound">Whether to enable sound.</param>
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {

            bool clickedItem = false;
            foreach (ItemDisplayButton button in this.itemButtons)
                if (button.containsPoint(x, y))
                {
                    this.handleButtonClick(button.name);
                    button.scale -= 0.5f;
                    button.scale = Math.Max(3.5f, button.scale);

                    Item i = button.item;

                    if (i == null)
                    {
                        continue;
                    }

                    Game1.player.addItemToInventory(i);
                    DimensionalStorageUnitBuilding.UniversalItems.Remove(i);
                    clickedItem = true;
                    break;
                }
            if (clickedItem)
            {
                this.populateItemsToDisplay();
                return;
            }

            bool clickedPlayersItem = false;
            foreach (ClickableComponent c in this.playersInventory.inventory)
            {
                if (!c.containsPoint(x, y))
                {
                    continue;
                }
                else
                {
                    int slotNumber = Convert.ToInt32(c.name);
                    Item item = Game1.player.items.ElementAt(slotNumber);
                    if (item == null)
                    {
                        continue;
                    }
                    SoundUtilities.PlaySound(Enums.StardewSound.coin);
                    bool added = DimensionalStorageUnitBuilding.AddItemToDimensionalStorageUnit(item);
                    if (!added) break;
                    Game1.player.removeItemFromInventory(item);
                    clickedPlayersItem = true;

                    break;
                }
            }
            if (clickedPlayersItem)
            {
                this.populateItemsToDisplay();
                return;
            }


            if (this.OkButton.containsPoint(x, y))
            {
                this.handleButtonClick(this.OkButton.name);
                this.OkButton.scale -= 0.25f;
                this.OkButton.scale = Math.Max(0.75f, this.OkButton.scale);
            }
            if (this._leftButton.containsPoint(x, y))
                this.handleButtonClick(this._leftButton.name);
            if (this._rightButton.containsPoint(x, y))
                this.handleButtonClick(this._rightButton.name);
            if (this._searchModeButton.containsPoint(x, y))
            {
                this.handleButtonClick(this._searchModeButton.name);
            }

            if (this._upgradeButton.containsPoint(x, y))
            {
                this.handleButtonClick(this._upgradeButton.name);
            }

            Rectangle r = new Rectangle(this.searchBox.X, this.searchBox.Y, this.searchBox.Width, this.searchBox.Height / 2);
            if (r.Contains(x, y))
            {
                this.searchBox.Update();
                this.searchBox.SelectMe();
            }
            else
                this.searchBox.Selected = false;


        }

        /// <summary>The method invoked when the player right-clicks on the lookup UI.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        /// <param name="playSound">Whether to enable sound.</param>
        public override void receiveRightClick(int x, int y, bool playSound = true)
        {

            bool clickedItem = false;

            int amountToGrab = 1;

            foreach (ItemDisplayButton button in this.itemButtons)
                if (button.containsPoint(x, y))
                {
                    this.handleButtonClick(button.name);
                    button.scale -= 0.5f;
                    button.scale = Math.Max(3.5f, button.scale);



                    if (button.item == null)
                    {
                        continue;
                    }

                    if (Game1.input.GetKeyboardState().IsKeyDown(Keys.LeftShift) || Game1.input.GetKeyboardState().IsKeyDown(Keys.Right))
                    {
                        amountToGrab = Math.Max(button.item.Stack / 2, 1);
                    }


                    Item i = button.item.getOne();
                    i.Stack = amountToGrab;

                    Game1.player.addItemToInventory(i);

                    button.item.Stack -= amountToGrab;

                    if (button.item.Stack <= 0)
                    {
                        clickedItem = true;
                        DimensionalStorageUnitBuilding.UniversalItems.Remove(button.item);
                    }
                    break;
                }
            if (clickedItem)
            {
                this.populateItemsToDisplay();
                return;
            }

            bool clickedPlayersItem = false;
            foreach (ClickableComponent c in this.playersInventory.inventory)
            {
                if (!c.containsPoint(x, y))
                {
                    continue;
                }
                else
                {
                    int slotNumber = Convert.ToInt32(c.name);
                    Item item = Game1.player.items.ElementAt(slotNumber);
                    if (item == null)
                    {
                        continue;
                    }
                    SoundUtilities.PlaySound(Enums.StardewSound.coin);

                    if (Game1.input.GetKeyboardState().IsKeyDown(Keys.LeftShift) || Game1.input.GetKeyboardState().IsKeyDown(Keys.Right))
                    {
                        amountToGrab = Math.Max(item.Stack / 2, 1);
                    }

                    Item itemToAdd = item.getOne();
                    itemToAdd.Stack = amountToGrab;
                    bool added = DimensionalStorageUnitBuilding.AddItemToDimensionalStorageUnit(itemToAdd);
                    if (!added)
                    {
                        break;
                    }
                    item.Stack -= amountToGrab;
                    clickedPlayersItem = true;
                    if (item.Stack <= 0)
                    {
                        Game1.player.removeItemFromInventory(item);
                    }
                    break;
                }
            }
            if (clickedPlayersItem)
            {
                this.populateItemsToDisplay();
                return;
            }

        }

        /// <summary>The method invoked when the player hovers the cursor over the menu.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        public override void performHoverAction(int x, int y)
        {
            bool hoverOverItemButtonThisFrame = false;
            foreach (ItemDisplayButton button in this.itemButtons)
            {
                if (button.containsPoint(x, y))
                {
                    button.scale = button.containsPoint(x, y)
                        ? Math.Min(button.scale + 0.02f, button.baseScale + 0.1f)
                        : Math.Max(button.scale - 0.02f, button.baseScale);
                    this.currentHoverTextureItem = button.item;
                    hoverOverItemButtonThisFrame = true;
                }
            }

            foreach (ClickableComponent c in this.playersInventory.inventory)
            {

                if (!c.containsPoint(x, y))
                {
                    continue;
                }
                else
                {
                    int slotNumber = Convert.ToInt32(c.name);
                    Item item = Game1.player.items.ElementAt(slotNumber);
                    if (item == null)
                    {
                        continue;
                    }
                    this.currentHoverTextureItem = item;
                    hoverOverItemButtonThisFrame = true;
                    break;
                }

            }

            if (hoverOverItemButtonThisFrame == false)
                this.currentHoverTextureItem = null;

            this.OkButton.scale = this.OkButton.containsPoint(x, y)
    ? Math.Min(this.OkButton.scale + 0.02f, this.OkButton.baseScale + 0.1f)
    : Math.Max(this.OkButton.scale - 0.02f, this.OkButton.baseScale);

            if (this._searchModeButton.containsPoint(x, y))
            {
                this.hoverText = this.searchBox.getCurrentSearchModeDisplayString();
                this._searchModeButton.scale = Math.Min(this._searchModeButton.scale + 0.02f, this._searchModeButton.baseScale + 0.1f);
            }
            else if (this._upgradeButton.containsPoint(x, y) && this.hasEnoughToUpgrade())
            {

                this.hoverText = this.upgradePrompt;
                this._upgradeButton.scale = Math.Min(this._upgradeButton.scale + 0.02f, this._upgradeButton.baseScale + 0.1f);
            }
            else if(this._upgradeButton.containsPoint(x, y) && !this.hasEnoughToUpgrade())
            {
                this.hoverText = this.upgradePrompt+"\n"+string.Format(this.missingItemPrompt, this.getUpgradeItemAmount().ToString(), this.upgradeItem.DisplayName, DimensionalStorageUnitBuilding.GetUpgradeCost().ToString());
                this._upgradeButton.scale = Math.Min(this._upgradeButton.scale + 0.02f, this._upgradeButton.baseScale + 0.1f);
            }
            else
            {
                this.hoverText = "";
                this._searchModeButton.scale = Math.Max(this._searchModeButton.scale - 0.02f, this._searchModeButton.baseScale);
            }
        }




        public override bool readyToClose()
        {
            return this.allFinished;
        }


        /// <summary>
        /// Populates all of the items for the menu.
        /// </summary>
        /// <returns></returns>
        private void populateItemsToDisplay()
        {
            this.updateUpgradeAndCapacityTexts();

            this.itemButtons.Clear();
            IList<Item> validItems = this.searchBox.getValidItems(DimensionalStorageUnitBuilding.UniversalItems);

            for (int row = 0; row < this._maxRowsToDisplay; row++)
                for (int column = 0; column < this._maxColumnsToDisplay; column++)
                {
                    int value = this.currentPageNumber * this._maxRowsToDisplay * this._maxColumnsToDisplay + row * this._maxColumnsToDisplay + column;
                    if (value >= validItems.Count) continue;


                    Item selectedItem = validItems.ElementAt(value);
                    float itemScale = 4f;
                    Rectangle placementBounds = new Rectangle((int)(this.xPositionOnScreen + 64 + (column * 24 * itemScale)), (int)(this.yPositionOnScreen + 256 + row * 16 * itemScale), 16, 16);
                    ItemDisplayButton itemButton = new ItemDisplayButton(selectedItem.DisplayName, selectedItem, null, placementBounds, 4f, true, Color.White);
                    itemButton.item = selectedItem;

                    this.itemButtons.Add(itemButton);
                }

        }

        public override void receiveGamePadButton(Buttons b)
        {
            if (b.Equals(Buttons.A))
            {
                this.receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
            }
            if (b.Equals(Buttons.Y) || b.Equals(Buttons.Start))
            {
                this.allFinished = true;
            }
        }

        public override void receiveKeyPress(Keys key)
        {
            if (key.Equals(Keys.Escape))
            {
                this.allFinished = true;
            }
            else
            {
                base.receiveKeyPress(key);
            }
        }

        public override bool areGamePadControlsImplemented()
        {
            return true;
        }

        /// <summary>
        /// Make this true if free cursor movement is desired.
        /// </summary>
        /// <returns></returns>
        public override bool overrideSnappyMenuCursorMovementBan()
        {
            return true;
        }

        /// <summary>Draw the menu to the screen.</summary>
        /// <param name="b">The sprite batch.</param>
        public override void draw(SpriteBatch b)
        {
            // draw menu box
            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, r: Color.Purple.R / 4, g: Color.Purple.G / 4, b: Color.Purple.B / 4);

            //Why does this have really weird positioning? Once again, not sure, but for some reason, this is what works.
            Game1.drawDialogueBox(this.playersInventory.xPositionOnScreen - 32, this.playersInventory.yPositionOnScreen - 136, (int)(this.playersInventory.width * 1.1f), (int)(this.playersInventory.height * 1.8f), false, true);

            //b.Draw(Game1.daybg, new Vector2((this.xPositionOnScreen + Game1.tileSize + Game1.tileSize * 2 / 3 - 2), (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4)), Color.White);
            //Game1.player.FarmerSprite.draw(b, new Vector2((this.xPositionOnScreen + Game1.tileSize + Game1.tileSize * 2 / 3 - 2), (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4)),1f);

            this.searchBox.Draw(b, true);
            this._searchModeButton.draw(b);
            this._upgradeButton.draw(b);

            // draw season buttons
            foreach (ItemDisplayButton button in this.itemButtons)
            {
                button.draw(b);
                //Utility.drawTinyDigits(button.item.Stack, b, new Vector2(button.boundingBox.Right - 16, button.boundingBox.Bottom - 16), 3f, 1f, Color.White);
            }

            // draw labels
            foreach (ClickableComponent label in this.Labels)
            {

            }
            Color color = Color.Violet;
            Utility.drawTextWithShadow(b, this.capacityDisplayComponent.name, Game1.smallFont, new Vector2(this.capacityDisplayComponent.bounds.X, this.capacityDisplayComponent.bounds.Y), color);
            string text = "";
            color = Color.White;
            Utility.drawTextWithShadow(b, this.capacityDisplayComponent.name, Game1.smallFont, new Vector2(this.capacityDisplayComponent.bounds.X, this.capacityDisplayComponent.bounds.Y), color);
            if (text.Length > 0)
                Utility.drawTextWithShadow(b, text, Game1.smallFont, new Vector2(this.capacityDisplayComponent.bounds.X + Game1.tileSize / 3 - Game1.smallFont.MeasureString(text).X / 2f, this.capacityDisplayComponent.bounds.Y + Game1.tileSize / 2), color);


            if (!string.IsNullOrEmpty(this.hoverText))
            {
                drawHoverText(b, this.hoverText, Game1.dialogueFont);
            }

            // draw OK button
            this.OkButton.draw(b);
            this._leftButton.draw(b);
            this._rightButton.draw(b);

            this.playersInventory.draw(b);

            if (this.currentHoverTextureItem != null)
                //Draws the item tooltip in the menu.
                drawToolTip(b, this.currentHoverTextureItem.getDescription(), this.currentHoverTextureItem.DisplayName, this.currentHoverTextureItem);

            // draw cursor
            this.drawMouse(b);
        }
    }
}
