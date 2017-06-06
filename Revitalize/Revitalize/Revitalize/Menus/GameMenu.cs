using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Revitalize.Menus.MenuComponents;
using Revitalize.Resources;
using Revitalize.Resources.DataNodes;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Revitalize.Menus
{
    public class GameMenu : IClickableMenu
    {
        public const int inventoryTab = 0;

        public const int skillsTab = 1;

        public const int socialTab = 2;

        public const int mapTab = 3;

        public const int craftingTab = 4;

        public const int collectionsTab = 5;

        public const int optionsTab = 6;

        public const int exitTab = 7;

        public const int numberOfTabs = 7;

        public int currentTab;

        private string hoverText = "";

        private string descriptionText = "";

        public const int maxTabsPerPage = 11;

        private int maxTabValue;

        public int currentMenuPage;
        public int currentMenuPageMax;

        public int currentTabIndex;

        private List<ClickableComponentExtended> tabs = new List<ClickableComponentExtended>();

        private List<IClickableMenu> pages = new List<IClickableMenu>();

        private List<Texture2D> pageTextureSheets = new List<Texture2D>();

        public bool invisible;

        public static bool forcePreventClose;

        private ClickableTextureComponent junimoNoteIcon;

        public ClickableTextureComponent LeftButton;
        public ClickableTextureComponent RightButton;

        /// <summary>
        /// Creates a custom game menu using specific tabs, which allows a wide variety of options. This is the default form, and is hardcoded by the Revitalize mod.
        /// </summary>
        /// <param name="tabsToAdd"></param> The tab components to be added. They must have a "value" field assigned to them otherwise they won't be used. 
        /// <param name="pagesToAdd"></param> The corresponding menus to add 
        /// <param name="pageTexturesToAdd"></param> The textures to be used .for the menu sheets. Only assign 1 per 12 tabs used.
        public GameMenu() : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
        {
            //can only hold about 12 tabs per menu page
            this.tabs.Add(new ClickableComponentExtended(new Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "inventory", Game1.content.LoadString("Strings\\UI:GameMenu_Inventory", new object[0]),0));
            this.pages.Add(new Revitalize.Menus.InventoryPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));            
            this.tabs.Add(new ClickableComponentExtended(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "skills", Game1.content.LoadString("Strings\\UI:GameMenu_Skills", new object[0]),1));
            this.pages.Add(new SkillsPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
            this.tabs.Add(new ClickableComponentExtended(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "social", Game1.content.LoadString("Strings\\UI:GameMenu_Social", new object[0]),2));
            this.pages.Add(new SocialPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
            this.tabs.Add(new ClickableComponentExtended(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 4, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "map", Game1.content.LoadString("Strings\\UI:GameMenu_Map", new object[0]),3));
            this.pages.Add(new MapPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
            this.tabs.Add(new ClickableComponentExtended(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 5, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "crafting", Game1.content.LoadString("Strings\\UI:GameMenu_Crafting", new object[0]),4));
            this.pages.Add(new CraftingPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false));
            this.tabs.Add(new ClickableComponentExtended(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 6, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "collections", Game1.content.LoadString("Strings\\UI:GameMenu_Collections", new object[0]),5));
            this.pages.Add(new CollectionsPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width - Game1.tileSize - Game1.tileSize / 4, this.height));
            this.tabs.Add(new ClickableComponentExtended(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 7, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "options", Game1.content.LoadString("Strings\\UI:GameMenu_Options", new object[0]),6));
            this.pages.Add(new OptionsPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width - Game1.tileSize - Game1.tileSize / 4, this.height));
            this.tabs.Add(new ClickableComponentExtended(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 8, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "exit", Game1.content.LoadString("Strings\\UI:GameMenu_Exit", new object[0]),7));
            this.pages.Add(new ExitPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width - Game1.tileSize - Game1.tileSize / 4, this.height));


            this.tabs.Add(new ClickableComponentExtended(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 1, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "bungalo", "12", 12));
            this.pages.Add(new ExitPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width - Game1.tileSize - Game1.tileSize / 4, this.height));
            this.tabs.Add(new ClickableComponentExtended(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "bungalo", "13", 13));
            this.pages.Add(new CollectionsPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width - Game1.tileSize - Game1.tileSize / 4, this.height));
            this.tabs.Add(new ClickableComponentExtended(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 1, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "bungalo", "24", 24));
            this.pages.Add(new Revitalize.Menus.InventoryPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
            pageTextureSheets.Add(Game1.mouseCursors);
            pageTextureSheets.Add(Game1.mouseCursors);
            pageTextureSheets.Add(Game1.mouseCursors);
            currentMenuPage = 0;
            if (Game1.activeClickableMenu == null)
            {
                Game1.playSound("bigSelect");
            }
            if (Game1.player.hasOrWillReceiveMail("canReadJunimoText") && !Game1.player.hasOrWillReceiveMail("JojaMember") && !Game1.player.hasCompletedCommunityCenter())
            {
                this.junimoNoteIcon = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + Game1.tileSize * 3 / 2, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:GameMenu_JunimoNote_Hover", new object[0]), Game1.mouseCursors, new Rectangle(331, 374, 15, 14), (float)Game1.pixelZoom, false);
            }
            GameMenu.forcePreventClose = false;
            if (Game1.options.gamepadControls && Game1.isAnyGamePadButtonBeingPressed())
            {
                this.setUpForGamePadMode();
            }
            int i = -1;
            foreach(var v in this.tabs)
            {
                if (v.value > i) i = v.value;
            }
            currentTabIndex = 0;
            currentMenuPageMax =(int)(Math.Floor(Convert.ToDouble(i / 12)));
            maxTabValue = i;
            TextureDataNode d;
            Dictionaries.spriteFontList.TryGetValue("leftArrow", out d);
            TextureDataNode f;
            Dictionaries.spriteFontList.TryGetValue("rightArrow", out f);
            this.LeftButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - (Game1.tileSize * 3), this.yPositionOnScreen / 4, Game1.tileSize, Game1.tileSize), d.texture, new Rectangle(0, 0, 16, 16), 4f, false);
            this.RightButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize, this.yPositionOnScreen / 4, Game1.tileSize, Game1.tileSize), f.texture, new Rectangle(0, 0, 16, 16), 4f, false);
        }

        /// <summary>
        /// Pretty sure this implementation is broken right now. Woops.
        /// </summary>
        /// <param name="tabsToAdd"></param> The tabs to be added to this custom menu.
        /// <param name="pagesToAdd"></param>  The menus to add
        /// <param name="pageTexturesToAdd"></param> Only add one page texture for every 12 tabs to ensure correct rendering.
        public GameMenu(List<List<ClickableComponentExtended>> tabsToAdd, List<List<IClickableMenu>> pagesToAdd, List<List<Texture2D>> pageTexturesToAdd) : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
        {
            //can only hold about 12 tabs per menu page
            int i = -1;
            foreach (var v in tabsToAdd)
            {
                foreach (var k in v)
                {
                    i++;
                    k.value = i;
                    tabs.Add(k);
                }
            }
            foreach (var v in pagesToAdd)
            {
                foreach (var k in v)
                {
                    pages.Add(k);
                }
            }
            foreach (var v in pageTexturesToAdd)
            {
                foreach (var k in v)
                {
                    pageTextureSheets.Add(k);
                }
            }
            currentMenuPage = 0;
            if (Game1.activeClickableMenu == null)
            {
                Game1.playSound("bigSelect");
            }
            if (Game1.player.hasOrWillReceiveMail("canReadJunimoText") && !Game1.player.hasOrWillReceiveMail("JojaMember") && !Game1.player.hasCompletedCommunityCenter())
            {
                this.junimoNoteIcon = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + Game1.tileSize * 3 / 2, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:GameMenu_JunimoNote_Hover", new object[0]), Game1.mouseCursors, new Rectangle(331, 374, 15, 14), (float)Game1.pixelZoom, false);
            }
            GameMenu.forcePreventClose = false;
            if (Game1.options.gamepadControls && Game1.isAnyGamePadButtonBeingPressed())
            {
                this.setUpForGamePadMode();
            }

            currentMenuPageMax = (int)(Math.Floor(Convert.ToDouble(this.tabs.Count / 13)));
            TextureDataNode d;
            Dictionaries.spriteFontList.TryGetValue("leftArrow", out d);
            TextureDataNode f;
            Dictionaries.spriteFontList.TryGetValue("rightArrow", out f);
            this.LeftButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - (Game1.tileSize * 3), this.yPositionOnScreen / 4, Game1.tileSize, Game1.tileSize), d.texture, new Rectangle(0, 0, 16, 16), 4f, false);
            this.RightButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize, this.yPositionOnScreen / 4, Game1.tileSize, Game1.tileSize), f.texture, new Rectangle(0, 0, 16, 16), 4f, false);
        }

        /// <summary>
        /// Creates a custom game menu using specific tabs, which allows a wide variety of options. Hypothetically should work but would require outside sources to manager their own tab values.
        /// </summary>
        /// <param name="tabsToAdd"></param> The tab components to be added. They must have a "value" field assigned to them otherwise they won't be used. 
        /// <param name="pagesToAdd"></param> The corresponding menus to add 
        /// <param name="pageTexturesToAdd"></param> The textures to be used .for the menu sheets. Only assign 1 per 12 tabs used.
        public GameMenu(List<ClickableComponentExtended> tabsToAdd, List<IClickableMenu> pagesToAdd, List<Texture2D> pageTexturesToAdd) : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
        {
            //can only hold about 12 tabs per menu page
            int i = 0;
            foreach(var v in tabsToAdd)
            {
                if (v.value == -1) continue;
                tabs.Add(v);
                pages.Add(pagesToAdd[i]);
                i++;
            }
            foreach(var v in pageTexturesToAdd)
            {
                pageTextureSheets.Add(v);
            }
            currentMenuPage = 0;
            if (Game1.activeClickableMenu == null)
            {
                Game1.playSound("bigSelect");
            }
            if (Game1.player.hasOrWillReceiveMail("canReadJunimoText") && !Game1.player.hasOrWillReceiveMail("JojaMember") && !Game1.player.hasCompletedCommunityCenter())
            {
                this.junimoNoteIcon = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + Game1.tileSize * 3 / 2, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:GameMenu_JunimoNote_Hover", new object[0]), Game1.mouseCursors, new Rectangle(331, 374, 15, 14), (float)Game1.pixelZoom, false);
            }
            GameMenu.forcePreventClose = false;
            if (Game1.options.gamepadControls && Game1.isAnyGamePadButtonBeingPressed())
            {
                this.setUpForGamePadMode();
            }

            currentMenuPageMax = (int)(Math.Floor(Convert.ToDouble(this.tabs.Count / 13)));
            TextureDataNode d;
            Dictionaries.spriteFontList.TryGetValue("leftArrow", out d);
            TextureDataNode f;
            Dictionaries.spriteFontList.TryGetValue("rightArrow", out f);
            this.LeftButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - (Game1.tileSize * 3), this.yPositionOnScreen / 4, Game1.tileSize, Game1.tileSize), d.texture, new Rectangle(0, 0, 16, 16), 4f, false);
            this.RightButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize, this.yPositionOnScreen / 4, Game1.tileSize, Game1.tileSize), f.texture, new Rectangle(0, 0, 16, 16), 4f, false);
        }

        public GameMenu(int startingTab, int extra = -1) : this()
        {
            this.changeTab(startingTab);
            if (startingTab == 6 && extra != -1)
            {
                (this.pages[6] as OptionsPage).currentItemIndex = extra;
            }
        }

        public override void receiveGamePadButton(Buttons b)
        {
            base.receiveGamePadButton(b);
            if (b == Buttons.RightTrigger)
            {
                if (this.currentTab == 3)
                {
                    Game1.activeClickableMenu = new GameMenu(4, -1);
                    return;
                }
                if (this.currentTab < 6 && this.pages[this.currentTab].readyToClose())
                {
                    this.changeTab(this.currentTab + 1);
                    return;
                }
            }
            else if (b == Buttons.LeftTrigger)
            {
                if (this.currentTab == 3)
                {
                    Game1.activeClickableMenu = new GameMenu(2, -1);
                    return;
                }
                if (this.currentTab > 0 && this.pages[this.currentTab].readyToClose())
                {
                    this.changeTab(this.currentTab - 1);
                    return;
                }
            }
        }

        public override void setUpForGamePadMode()
        {
            base.setUpForGamePadMode();
            if (this.pages.Count > this.currentTab)
            {
                this.pages[this.currentTab].setUpForGamePadMode();
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);

            if (this.LeftButton.containsPoint(x, y))
            {
                if (this.currentMenuPage > 0) this.currentMenuPage--;
            }
            if (this.RightButton.containsPoint(x, y))
            {
                if (this.currentMenuPage < currentMenuPageMax) this.currentMenuPage++;
            }

            if (!this.invisible && !GameMenu.forcePreventClose)
            {
                int i = -1;
                foreach (var current in this.tabs)
                {
                    i++;
                    if (current.value > (11 + (12 * currentMenuPage))) continue;
                    if (current.value < (0 + (12 * currentMenuPage))) continue;
                    if (current.containsPoint(x, y) && this.currentTab != current.value && this.pages[this.currentTabIndex].readyToClose())
                    {
                        currentTabIndex = i;
                        this.changeTab(current.value);
                        return;
                    }
                }
                if (this.junimoNoteIcon != null && this.junimoNoteIcon.containsPoint(x, y))
                {
                    Game1.activeClickableMenu = new JunimoNoteMenu(true, 1, false);
                }
            }
            try
            {
                this.pages[this.currentTabIndex].receiveLeftClick(x, y, true);
            }
            catch(Exception e)
            {

            }
        }

        public static string getLabelOfTabFromIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return Game1.content.LoadString("Strings\\UI:GameMenu_Inventory", new object[0]);
                case 1:
                    return Game1.content.LoadString("Strings\\UI:GameMenu_Skills", new object[0]);
                case 2:
                    return Game1.content.LoadString("Strings\\UI:GameMenu_Social", new object[0]);
                case 3:
                    return Game1.content.LoadString("Strings\\UI:GameMenu_Map", new object[0]);
                case 4:
                    return Game1.content.LoadString("Strings\\UI:GameMenu_Crafting", new object[0]);
                case 5:
                    return Game1.content.LoadString("Strings\\UI:GameMenu_Collections", new object[0]);
                case 6:
                    return Game1.content.LoadString("Strings\\UI:GameMenu_Options", new object[0]);
                case 7:
                    return Game1.content.LoadString("Strings\\UI:GameMenu_Exit", new object[0]);
                default:
                    return "";
            }
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            this.pages[this.currentTabIndex].receiveRightClick(x, y, true);
        }

        public override void receiveScrollWheelAction(int direction)
        {
            base.receiveScrollWheelAction(direction);
            if (currentTab >= this.tabs.Count) this.pages[currentTab - (maxTabValue - this.tabs.Count) - 1].receiveScrollWheelAction(direction);
            else this.pages[this.currentTab].receiveScrollWheelAction(direction);

        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);
            this.hoverText = "";
            this.pages[this.currentTabIndex].performHoverAction(x, y);
            foreach (var current in this.tabs)
            {
                if (current.value > (11 + (12 * currentMenuPage))) continue;
                if (current.value < (0 + (12 * currentMenuPage))) continue;
                if (current.containsPoint(x, y))
                {
                    this.hoverText = current.label;
                    return;
                }
            }
            if (this.junimoNoteIcon != null)
            {
                this.junimoNoteIcon.tryHover(x, y, 0.1f);
                if (this.junimoNoteIcon.containsPoint(x, y))
                {
                    this.hoverText = this.junimoNoteIcon.hoverText;
                }
            }
            if (LeftButton.containsPoint(Game1.getMousePosition().X, Game1.getMousePosition().Y) && this.currentMenuPage!=0) this.hoverText = "Previous Page";
            if (RightButton.containsPoint(Game1.getMousePosition().X, Game1.getMousePosition().Y)&& this.currentMenuPage!=this.currentMenuPageMax) this.hoverText = "Next Page";

        }


        public override void releaseLeftClick(int x, int y)
        {
            base.releaseLeftClick(x, y);
           
        this.pages[this.currentTabIndex].releaseLeftClick(x, y);

        }

        public override void leftClickHeld(int x, int y)
        {
            base.leftClickHeld(x, y);
            this.pages[this.currentTabIndex].leftClickHeld(x, y);

        }

        public override bool readyToClose()
        {
            return this.pages[this.currentTabIndex].readyToClose();

        }

        public void changeTab(int whichTab)
        {
            if (this.currentTab == 2)
            {
                if (this.junimoNoteIcon != null)
                {
                    this.junimoNoteIcon = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + Game1.tileSize * 3 / 2, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:GameMenu_JunimoNote_Hover", new object[0]), Game1.mouseCursors, new Rectangle(331, 374, 15, 14), (float)Game1.pixelZoom, false);
                }
            }
            else if (whichTab == 2 && this.junimoNoteIcon != null)
            {
                ClickableTextureComponent expr_AA_cp_0_cp_0 = this.junimoNoteIcon;
                expr_AA_cp_0_cp_0.bounds.X = expr_AA_cp_0_cp_0.bounds.X + Game1.tileSize;
            }
            this.currentTab = this.tabs[currentTabIndex].value;

            if (this.currentTab == 3)
            {
                this.invisible = true;
                this.width += Game1.tileSize * 2;
                base.initializeUpperRightCloseButton();
            }
            else
            {
                this.width = 800 + IClickableMenu.borderWidth * 2;
                base.initializeUpperRightCloseButton();
                this.invisible = false;
            }
            Game1.playSound("smallSelect");
        }

        public override void draw(SpriteBatch b)
        {
            if (!this.invisible)
            {
                if (!Game1.options.showMenuBackground)
                {
                    b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
                }

                Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.pages[currentTabIndex].width, this.pages[currentTabIndex].height, false, true, null, false);
             


                this.pages[currentTabIndex].draw(b);

                b.End();
                b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
                if (!GameMenu.forcePreventClose)
                {
                    //foreach (ClickableComponentExtended current in this.tabs)
                    //{
               
                    foreach(var current in this.tabs)
                    {
                        if (current.value > (11+(12 * currentMenuPage))) continue;
                        if (current.value < (0 + (12 * currentMenuPage))) continue;
                        int num = current.value;
                        string name = current.name;
                        //!!!!!BINGO! HERE ARE THE TEXTURES
                        b.Draw(pageTextureSheets[currentMenuPage], new Vector2((float)current.bounds.X, (float)(current.bounds.Y + ((this.currentTab == current.value) ? 8 : 0))), new Rectangle?(new Rectangle(num * 16, 368, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.0001f);
                        if (current.name.Equals("skills"))
                        {
                            Game1.player.FarmerRenderer.drawMiniPortrat(b, new Vector2((float)(current.bounds.X + 8), (float)(current.bounds.Y + 12 + ((this.currentTab == current.value) ? 8 : 0))), 0.00011f, 3f, 2, Game1.player);
                        }
                    }
                    //}  //end for each
                    b.End();
                    b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    if (this.junimoNoteIcon != null)
                    {
                        this.junimoNoteIcon.draw(b);
                    }
                    if (!this.hoverText.Equals(""))
                    {
                        IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
                    }
                }
            }
            else
            {
                this.pages[this.currentTab].draw(b);
            }
           if(this.currentMenuPage!=0) LeftButton.draw(b);
           if(this.currentMenuPage!=currentMenuPageMax) RightButton.draw(b);
            if (!GameMenu.forcePreventClose)
            {
                base.draw(b);
            }
            if (!Game1.options.hardwareCursor)
            {
                b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
            }
        }

        public override bool areGamePadControlsImplemented()
        {
                return this.pages[this.currentTabIndex].gamePadControlsImplemented;
        }

        public override void receiveKeyPress(Keys key)
        {
            if (Game1.options.menuButton.Contains(new InputButton(key)) && this.readyToClose())
            {
                Game1.exitActiveMenu();
                Game1.playSound("bigDeSelect");
            }

             this.pages[this.currentTabIndex].receiveKeyPress(key);
        }
    }
}
