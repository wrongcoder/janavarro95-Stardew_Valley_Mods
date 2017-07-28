// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.ShippingMenu
// Assembly: StardewValley, Version=1.0.6054.4284, Culture=neutral, PublicKeyToken=null
// MVID: 5E41EDE0-CE7E-41F9-BCB3-07C910BA6113
// Assembly location: C:\Users\owner\Downloads\steam_cmd\ehh\StardewValley.exe

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;

namespace Omegasis.SaveAnywhere
{
    public class New_Shipping_Menu : IClickableMenu
    {
        public int currentPage = -1;
        public int currentTab = 0;
        private List<ClickableTextureComponent> categories = new List<ClickableTextureComponent>();
        private List<int> categoryTotals = new List<int>();
        private List<MoneyDial> categoryDials = new List<MoneyDial>();
        private List<List<Item>> categoryItems = new List<List<Item>>();
        private int introTimer = 3500;
        public List<TemporaryAnimatedSprite> animations = new List<TemporaryAnimatedSprite>();
        public const int farming_category = 0;
        public const int foraging_category = 1;
        public const int fishing_category = 2;
        public const int mining_category = 3;
        public const int other_category = 4;
        public const int total_category = 5;
        public const int timePerIntroCategory = 500;
        public const int outroFadeTime = 800;
        public const int smokeRate = 100;
        public const int categorylabelHeight = 25;
        public const int itemsPerCategoryPage = 9;
        private ClickableTextureComponent okButton;
        private ClickableTextureComponent forwardButton;
        private ClickableTextureComponent backButton;
        private int categoryLabelsWidth;
        private int plusButtonWidth;
        private int itemSlotWidth;
        private int itemAndPlusButtonWidth;
        private int totalWidth;
        private int centerX;
        private int centerY;
        private int outroFadeTimer;
        private int outroPauseBeforeDateChange;
        private int finalOutroTimer;
        private int smokeTimer;
        private int dayPlaqueY;
        private float weatherX;
        private bool outro;
        private bool newDayPlaque;
        private bool savedYet;
        private SaveGameMenu saveGameMenu;

        public New_Shipping_Menu(List<Item> items)
          : base(Game1.viewport.Width / 2 - 640, Game1.viewport.Height / 2 - 360, 1280, 720, false)
        {
            this.parseItems(items);
            if (!Game1.wasRainingYesterday)
                Game1.changeMusicTrack(!Game1.currentSeason.Equals("summer") ? "none" : "nightTime");
            this.categoryLabelsWidth = Game1.tileSize * 7;
            this.plusButtonWidth = 10 * Game1.pixelZoom;
            this.itemSlotWidth = 24 * Game1.pixelZoom;
            this.itemAndPlusButtonWidth = this.plusButtonWidth + this.itemSlotWidth + 2 * Game1.pixelZoom;
            this.totalWidth = this.categoryLabelsWidth + this.itemAndPlusButtonWidth;
            this.centerX = Game1.viewport.Width / 2;
            this.centerY = Game1.viewport.Height / 2;
            for (int index = 0; index < 6; ++index)
            {
                List<ClickableTextureComponent> list = this.categories;
                ClickableTextureComponent textureComponent1 = new ClickableTextureComponent("texture"+Convert.ToString(index),new Rectangle(this.centerX + this.totalWidth / 2 - this.plusButtonWidth, this.centerY - 25 * Game1.pixelZoom * 3 + index * 27 * Game1.pixelZoom, this.plusButtonWidth, 11 * Game1.pixelZoom), "", this.getCategoryName(index), Game1.mouseCursors, new Rectangle(392, 361, 10, 11), (float)Game1.pixelZoom, false);
                textureComponent1.visible = index < 5 && Enumerable.Count<Item>((IEnumerable<Item>)this.categoryItems[index]) > 0;
                ClickableTextureComponent textureComponent2 = textureComponent1;
                list.Add(textureComponent2);
            }
            this.dayPlaqueY = this.categories[0].bounds.Y - Game1.tileSize * 2;
            this.okButton = new ClickableTextureComponent("Done", new Rectangle(this.centerX + this.totalWidth / 2 - this.itemAndPlusButtonWidth + Game1.tileSize / 2, this.centerY + 25 * Game1.pixelZoom * 3 - Game1.tileSize, Game1.tileSize, Game1.tileSize), (string)null, "Done", Game1.mouseCursors, new Rectangle(128, 256, 64, 64), 1f, false);
            this.backButton = new ClickableTextureComponent("Back", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + this.height - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), (string)null, "", Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
            this.forwardButton = new ClickableTextureComponent("forward", new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), (string)null, "", Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
            if (Game1.dayOfMonth == 25 && Game1.currentSeason.Equals("winter"))
            {
                Vector2 position = new Vector2((float)Game1.viewport.Width, (float)Game1.random.Next(0, 200));
                Rectangle sourceRect = new Rectangle(640, 800, 32, 16);
                int numberOfLoops = 1000;
                this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 80f, 2, numberOfLoops, position, false, false, 0.01f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, true)
                {
                    motion = new Vector2(-4f, 0.0f),
                    delayBeforeAnimationStart = 3000
                });
            }
            Game1.stats.checkForShippingAchievements();
            if (Game1.player.achievements.Contains(34) || !Utility.hasFarmerShippedAllItems())
                return;
            Game1.getAchievement(34);
        }

        public void parseItems(List<Item> items)
        {
            Utility.consolidateStacks(items);
            for (int index = 0; index < 6; ++index)
            {
                this.categoryItems.Add(new List<Item>());
                this.categoryTotals.Add(0);
                this.categoryDials.Add(new MoneyDial(7, index == 5));
            }
            foreach (Item obj in items)
            {
                if (obj is StardewValley.Object)
                {
                    StardewValley.Object o = obj as StardewValley.Object;
                    int categoryIndexForObject = this.getCategoryIndexForObject(o);
                    this.categoryItems[categoryIndexForObject].Add((Item)o);
                    List<int> list;
                    int index;
                    (list = this.categoryTotals)[index = categoryIndexForObject] = list[index] + o.sellToStorePrice() * o.Stack;
                    Game1.stats.itemsShipped += (uint)o.Stack;
                    if (o.countsForShippedCollection())
                        Game1.player.shippedBasic(o.parentSheetIndex, o.stack);
                }
            }
            for (int index = 0; index < 5; ++index)
            {
                List<int> list;
                (list = this.categoryTotals)[5] = list[5] + this.categoryTotals[index];
                this.categoryItems[5].AddRange((IEnumerable<Item>)this.categoryItems[index]);
                this.categoryDials[index].currentValue = this.categoryTotals[index];
                this.categoryDials[index].previousTargetValue = this.categoryDials[index].currentValue;
            }
            this.categoryDials[5].currentValue = this.categoryTotals[5];
            Game1.player.Money += this.categoryTotals[5];
        }

        public int getCategoryIndexForObject(StardewValley.Object o)
        {
            switch (o.parentSheetIndex)
            {
                case 296:
                case 396:
                case 402:
                case 406:
                case 410:
                case 414:
                case 418:
                    return 1;
                default:
                    int num = o.category;
                    switch (num + 81)
                    {
                        case 0:
                            label_8:
                            return 1;
                        case 1:
                        case 2:
                        case 6:
                            label_6:
                            return 0;
                        default:
                            switch (num + 27)
                            {
                                case 0:
                                case 4:
                                    goto label_8;
                                case 1:
                                    goto label_6;
                                case 7:
                                    label_7:
                                    return 2;
                                default:
                                    switch (num + 6)
                                    {
                                        case 0:
                                        case 1:
                                            goto label_6;
                                        case 2:
                                            goto label_7;
                                        case 4:
                                            label_9:
                                            return 3;
                                        default:
                                            switch (num + 15)
                                            {
                                                case 0:
                                                case 3:
                                                    goto label_9;
                                                case 1:
                                                    goto label_6;
                                                default:
                                                    return 4;
                                            }
                                    }
                            }
                    }
            }
        }

        public string getCategoryName(int index)
        {
            switch (index)
            {
                case 0:
                    return "Farming";
                case 1:
                    return "Foraging";
                case 2:
                    return "Fishing";
                case 3:
                    return "Mining";
                case 4:
                    return "Other";
                case 5:
                    return "Total";
                default:
                    return "";
            }
        }

        public override void update(GameTime time)
        {
            base.update(time);
            if (this.saveGameMenu != null)
            {
                this.saveGameMenu.update(time);
                if (this.saveGameMenu.quit)
                {
                    this.saveGameMenu = (SaveGameMenu)null;
                    this.savedYet = true;
                }
            }
            this.weatherX += (float)time.ElapsedGameTime.Milliseconds * 0.03f;
            for (int index = Enumerable.Count<TemporaryAnimatedSprite>((IEnumerable<TemporaryAnimatedSprite>)this.animations) - 1; index >= 0; --index)
            {
                if (this.animations[index].update(time))
                    this.animations.RemoveAt(index);
            }
            if (this.outro)
            {
                if (this.outroFadeTimer > 0)
                    this.outroFadeTimer -= time.ElapsedGameTime.Milliseconds;
                else if (this.outroFadeTimer <= 0 && this.dayPlaqueY < this.centerY - Game1.tileSize)
                {
                    if (Enumerable.Count<TemporaryAnimatedSprite>((IEnumerable<TemporaryAnimatedSprite>)this.animations) > 0)
                        this.animations.Clear();
                    this.dayPlaqueY += (int)Math.Ceiling((double)time.ElapsedGameTime.Milliseconds * 0.349999994039536);
                    if (this.dayPlaqueY >= this.centerY - Game1.tileSize)
                        this.outroPauseBeforeDateChange = 700;
                }
                else if (this.outroPauseBeforeDateChange > 0)
                {
                    this.outroPauseBeforeDateChange -= time.ElapsedGameTime.Milliseconds;
                    if (this.outroPauseBeforeDateChange <= 0)
                    {
                        this.newDayPlaque = true;
                        Game1.playSound("newRecipe");
                        if (!Game1.currentSeason.Equals("winter"))
                            DelayedAction.playSoundAfterDelay(!Game1.isRaining ? "rooster" : "rainsound", 1500);
                        this.finalOutroTimer = 2000;
                        this.animations.Clear();
                        if (!this.savedYet)
                        {
                            if (this.saveGameMenu != null)
                                return;
                            try
                            {
                               // Game1.activeClickableMenu = new StardewValley.Menus.SaveGameMenu();
                            }
                            catch (Exception rrr)
                            {
                                Game1.showRedMessage("Can't save here. See log for error.");
                                Mod_Core.thisMonitor.Log(rrr.ToString(), LogLevel.Error);
                            }
                            return;
                        }
                    }
                }
                else if (this.finalOutroTimer > 0 && this.savedYet)
                {
                    this.finalOutroTimer -= time.ElapsedGameTime.Milliseconds;
                    if (this.finalOutroTimer <= 0)
                        this.exitThisMenu(false);
                }
            }
            if (this.introTimer >= 0)
            {
                int num = this.introTimer;
                this.introTimer -= time.ElapsedGameTime.Milliseconds * (Game1.oldMouseState.LeftButton != ButtonState.Pressed ? 1 : 3);
                if (num % 500 < this.introTimer % 500 && this.introTimer <= 3000)
                {
                    int which = 4 - this.introTimer / 500;
                    if (which < 6 && which > -1)
                    {
                        if (Enumerable.Count<Item>((IEnumerable<Item>)this.categoryItems[which]) > 0)
                        {
                            Game1.playSound(this.getCategorySound(which));
                            this.categoryDials[which].currentValue = 0;
                            this.categoryDials[which].previousTargetValue = 0;
                        }
                        else
                            Game1.playSound("stoneStep");
                    }
                }
                if (this.introTimer >= 0)
                    return;
                Game1.playSound("money");
                this.categoryDials[5].currentValue = 0;
                this.categoryDials[5].previousTargetValue = 0;
            }
            else
            {
                if (Game1.dayOfMonth == 28 || this.outro)
                    return;
                if (!Game1.wasRainingYesterday)
                {
                    Vector2 position = new Vector2((float)Game1.viewport.Width, (float)Game1.random.Next(200));
                    Rectangle sourceRect = new Rectangle(640, 752, 16, 16);
                    int num = Game1.random.Next(1, 4);
                    if (Game1.random.NextDouble() < 0.001)
                    {
                        bool flipped = Game1.random.NextDouble() < 0.5;
                        if (Game1.random.NextDouble() < 0.5)
                            this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(640, 826, 16, 8), 40f, 4, 0, new Vector2((float)Game1.random.Next(this.centerX * 2), (float)Game1.random.Next(this.centerY)), false, flipped)
                            {
                                rotation = 3.141593f,
                                scale = (float)Game1.pixelZoom,
                                motion = new Vector2(!flipped ? 8f : -8f, 8f),
                                local = true
                            });
                        else
                            this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(258, 1680, 16, 16), 40f, 4, 0, new Vector2((float)Game1.random.Next(this.centerX * 2), (float)Game1.random.Next(this.centerY)), false, flipped)
                            {
                                scale = (float)Game1.pixelZoom,
                                motion = new Vector2(!flipped ? 8f : -8f, 8f),
                                local = true
                            });
                    }
                    else if (Game1.random.NextDouble() < 0.0002)
                    {
                        position = new Vector2((float)Game1.viewport.Width, (float)Game1.random.Next(4, Game1.tileSize * 4));
                        this.animations.Add(new TemporaryAnimatedSprite(Game1.staminaRect, new Rectangle(0, 0, 1, 1), 9999f, 1, 10000, position, false, false, 0.01f, 0.0f, Color.White * (0.25f + (float)Game1.random.NextDouble()), 4f, 0.0f, 0.0f, 0.0f, true)
                        {
                            motion = new Vector2(-0.25f, 0.0f)
                        });
                    }
                    else if (Game1.random.NextDouble() < 5E-05)
                    {
                        position = new Vector2((float)Game1.viewport.Width, (float)(Game1.viewport.Height - Game1.tileSize * 3));
                        for (int index = 0; index < num; ++index)
                        {
                            this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float)Game1.random.Next(60, 101), 4, 100, position + new Vector2((float)((index + 1) * Game1.random.Next(15, 18)), (float)((index + 1) * -20)), false, false, 0.01f, 0.0f, Color.Black, 4f, 0.0f, 0.0f, 0.0f, true)
                            {
                                motion = new Vector2(-1f, 0.0f)
                            });
                            this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float)Game1.random.Next(60, 101), 4, 100, position + new Vector2((float)((index + 1) * Game1.random.Next(15, 18)), (float)((index + 1) * 20)), false, false, 0.01f, 0.0f, Color.Black, 4f, 0.0f, 0.0f, 0.0f, true)
                            {
                                motion = new Vector2(-1f, 0.0f)
                            });
                        }
                    }
                    else if (Game1.random.NextDouble() < 1E-05)
                    {
                        sourceRect = new Rectangle(640, 784, 16, 16);
                        this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 75f, 4, 1000, position, false, false, 0.01f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, true)
                        {
                            motion = new Vector2(-3f, 0.0f),
                            yPeriodic = true,
                            yPeriodicLoopTime = 1000f,
                            yPeriodicRange = (float)(Game1.tileSize / 8),
                            shakeIntensity = 0.5f
                        });
                    }
                }
                this.smokeTimer -= time.ElapsedGameTime.Milliseconds;
                if (this.smokeTimer <= 0)
                {
                    this.smokeTimer = 50;
                    this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(684, 1075, 1, 1), 1000f, 1, 1000, new Vector2((float)(Game1.tileSize * 2 + Game1.tileSize * 3 / 4 + Game1.pixelZoom * 3), (float)(Game1.viewport.Height - Game1.tileSize * 2 + Game1.pixelZoom * 5)), false, false)
                    {
                        color = !Game1.wasRainingYesterday ? Color.White : Color.SlateGray,
                        scale = (float)Game1.pixelZoom,
                        scaleChange = 0.0f,
                        alphaFade = (float)1.0 / (float)400.0,
                        motion = new Vector2(0.0f, (float)((double)-Game1.random.Next(25, 75) / 100.0 / 4.0)),
                        acceleration = new Vector2((float)-1.0 / (float)1000.0, 0.0f)
                    });
                }
            }
        }

        public string getCategorySound(int which)
        {
            switch (which)
            {
                case 0:
                    return !(this.categoryItems[0][0] as StardewValley.Object).isAnimalProduct() ? "harvest" : "cluck";
                case 1:
                    return "leafrustle";
                case 2:
                    return "button1";
                case 3:
                    return "hammer";
                case 4:
                    return "coin";
                case 5:
                    return "money";
                default:
                    return "stoneStep";
            }
        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);
            if (this.currentPage == -1)
            {
                this.okButton.tryHover(x, y, 0.1f);
                foreach (ClickableTextureComponent textureComponent in this.categories)
                    textureComponent.sourceRect.X = !textureComponent.containsPoint(x, y) ? 392 : 402;
            }
            else
            {
                this.backButton.tryHover(x, y, 0.5f);
                this.forwardButton.tryHover(x, y, 0.5f);
            }
        }

        public override void receiveKeyPress(Keys key)
        {
            if (this.introTimer > 0 || !key.Equals((object)Keys.Escape))
                return;
            this.receiveLeftClick(this.okButton.bounds.Center.X, this.okButton.bounds.Center.Y, true);
        }

        public override void receiveGamePadButton(Buttons b)
        {
            base.receiveGamePadButton(b);
            if (b != Buttons.B || this.currentPage == -1)
                return;
            if (this.currentTab == 0)
                this.currentPage = -1;
            else
                --this.currentTab;
            Game1.playSound("shwip");
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (this.outro && !this.savedYet)
            {
                if (this.saveGameMenu != null)
                {

                }
                    
            }
            else
            {
                if (this.savedYet)
                    return;
                base.receiveLeftClick(x, y, playSound);
                if (this.currentPage == -1 && this.okButton.containsPoint(x, y))
                {
                  //  this.outro = true;
                 //   this.outroFadeTimer = 800;
                    Game1.playSound("bigDeSelect");
                    //    Game1.changeMusicTrack("none");
                    Game1.exitActiveMenu();
                }
                if (this.currentPage == -1)
                {
                    for (int index = 0; index < Enumerable.Count<ClickableTextureComponent>((IEnumerable<ClickableTextureComponent>)this.categories); ++index)
                    {
                        if (this.categories[index].visible && this.categories[index].containsPoint(x, y))
                        {
                            this.currentPage = index;
                            Game1.playSound("shwip");
                            break;
                        }
                    }
                }
                else if (this.backButton.containsPoint(x, y))
                {
                    if (this.currentTab == 0)
                        this.currentPage = -1;
                    else
                        --this.currentTab;
                    Game1.playSound("shwip");
                }
                else if (this.showForwardButton() && this.forwardButton.containsPoint(x, y))
                {
                    ++this.currentTab;
                    Game1.playSound("shwip");
                }
            }
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
        }

        public bool showForwardButton()
        {
            return Enumerable.Count<Item>((IEnumerable<Item>)this.categoryItems[this.currentPage]) > 9 * (this.currentTab + 1);
        }

        public override void draw(SpriteBatch b)
        {
            if (Game1.wasRainingYesterday)
            {
                b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), !Game1.currentSeason.Equals("winter") ? Color.SlateGray * (float)(1.0 - (double)this.introTimer / 3500.0) : Color.LightSlateGray);
                b.Draw(Game1.mouseCursors, new Rectangle(639 * Game1.pixelZoom, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), !Game1.currentSeason.Equals("winter") ? Color.SlateGray * (float)(1.0 - (double)this.introTimer / 3500.0) : Color.LightSlateGray);
                int num1 = -61 * Game1.pixelZoom;
                while (num1 < Game1.viewport.Width + 61 * Game1.pixelZoom)
                {
                    b.Draw(Game1.mouseCursors, new Vector2((float)num1 + this.weatherX / 2f % (float)(61 * Game1.pixelZoom), (float)(Game1.tileSize / 2)), new Rectangle?(new Rectangle(643, 1142, 61, 53)), Color.DarkSlateGray * 1f * (float)(1.0 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
                    num1 += 61 * Game1.pixelZoom;
                }
                b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float)(Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, !Game1.currentSeason.Equals("winter") ? 737 : 1034, 639, 48)), (!Game1.currentSeason.Equals("winter") ? new Color(30, 62, 50) : Color.White * 0.25f) * (float)(0.5 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
                b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), (float)(Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, !Game1.currentSeason.Equals("winter") ? 737 : 1034, 639, 48)), (!Game1.currentSeason.Equals("winter") ? new Color(30, 62, 50) : Color.White * 0.25f) * (float)(0.5 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
                b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float)(Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, !Game1.currentSeason.Equals("winter") ? 737 : 1034, 639, 32)), (!Game1.currentSeason.Equals("winter") ? new Color(30, 62, 50) : Color.White * 0.5f) * (float)(1.0 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
                b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), (float)(Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, !Game1.currentSeason.Equals("winter") ? 737 : 1034, 639, 32)), (!Game1.currentSeason.Equals("winter") ? new Color(30, 62, 50) : Color.White * 0.5f) * (float)(1.0 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
                b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.tileSize * 2 + Game1.tileSize / 2), (float)(Game1.viewport.Height - Game1.tileSize * 2 + Game1.tileSize / 4 + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(653, 880, 10, 10)), Color.White * (float)(1.0 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
                int num2 = -61 * Game1.pixelZoom;
                while (num2 < Game1.viewport.Width + 61 * Game1.pixelZoom)
                {
                    b.Draw(Game1.mouseCursors, new Vector2((float)num2 + this.weatherX % (float)(61 * Game1.pixelZoom), (float)(-Game1.tileSize / 2)), new Rectangle?(new Rectangle(643, 1142, 61, 53)), Color.SlateGray * 0.85f * (float)(1.0 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.9f);
                    num2 += 61 * Game1.pixelZoom;
                }
                foreach (TemporaryAnimatedSprite temporaryAnimatedSprite in this.animations)
                    temporaryAnimatedSprite.draw(b, true, 0, 0);
                int num3 = -61 * Game1.pixelZoom;
                while (num3 < Game1.viewport.Width + 61 * Game1.pixelZoom)
                {
                    b.Draw(Game1.mouseCursors, new Vector2((float)num3 + this.weatherX * 1.5f % (float)(61 * Game1.pixelZoom), (float)(-Game1.tileSize * 2)), new Rectangle?(new Rectangle(643, 1142, 61, 53)), Color.LightSlateGray * (float)(1.0 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.9f);
                    num3 += 61 * Game1.pixelZoom;
                }
            }
            else
            {
                b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Color.White * (float)(1.0 - (double)this.introTimer / 3500.0));
                b.Draw(Game1.mouseCursors, new Rectangle(639 * Game1.pixelZoom, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Color.White * (float)(1.0 - (double)this.introTimer / 3500.0));
                b.Draw(Game1.mouseCursors, new Vector2(0.0f, 0.0f), new Rectangle?(new Rectangle(0, 1453, 639, 195)), Color.White * (float)(1.0 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
                b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), 0.0f), new Rectangle?(new Rectangle(0, 1453, 639, 195)), Color.White * (float)(1.0 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
                if (Game1.dayOfMonth == 28)
                    b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.viewport.Width - 44 * Game1.pixelZoom), (float)Game1.pixelZoom), new Rectangle?(new Rectangle(642, 835, 43, 43)), Color.White * (float)(1.0 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
                b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float)(Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, !Game1.currentSeason.Equals("winter") ? 737 : 1034, 639, 48)), (!Game1.currentSeason.Equals("winter") ? new Color(0, 20, 40) : Color.White * 0.25f) * (float)(0.649999976158142 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
                b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), (float)(Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, !Game1.currentSeason.Equals("winter") ? 737 : 1034, 639, 48)), (!Game1.currentSeason.Equals("winter") ? new Color(0, 20, 40) : Color.White * 0.25f) * (float)(0.649999976158142 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
                b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float)(Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, !Game1.currentSeason.Equals("winter") ? 737 : 1034, 639, 32)), (!Game1.currentSeason.Equals("winter") ? new Color(0, 32, 20) : Color.White * 0.5f) * (float)(1.0 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
                b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), (float)(Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, !Game1.currentSeason.Equals("winter") ? 737 : 1034, 639, 32)), (!Game1.currentSeason.Equals("winter") ? new Color(0, 32, 20) : Color.White * 0.5f) * (float)(1.0 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
                b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.tileSize * 2 + Game1.tileSize / 2), (float)(Game1.viewport.Height - Game1.tileSize * 2 + Game1.tileSize / 4 + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(653, 880, 10, 10)), Color.White * (float)(1.0 - (double)this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
            }
            if (!this.outro && !Game1.wasRainingYesterday)
            {
                foreach (TemporaryAnimatedSprite temporaryAnimatedSprite in this.animations)
                    temporaryAnimatedSprite.draw(b, true, 0, 0);
            }
            if (this.currentPage == -1)
            {
                SpriteText.drawStringWithScrollCenteredAt(b, Utility.getYesterdaysDate(), Game1.viewport.Width / 2, this.categories[0].bounds.Y - Game1.tileSize * 2, "", 1f, -1, 0, 0.88f, false);
                int num = -5 * Game1.pixelZoom;
                int index1 = 0;
                foreach (ClickableTextureComponent textureComponent in this.categories)
                {
                    if (this.introTimer < 2500 - index1 * 500)
                    {
                        Vector2 vector2 = textureComponent.getVector2() + new Vector2((float)(Game1.pixelZoom * 3), (float)(-Game1.pixelZoom * 2));
                        if (textureComponent.visible)
                        {
                            textureComponent.draw(b);
                            b.Draw(Game1.mouseCursors, vector2 + new Vector2((float)(-26 * Game1.pixelZoom), (float)(num + Game1.pixelZoom)), new Rectangle?(new Rectangle(293, 360, 24, 24)), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
                            this.categoryItems[index1][0].drawInMenu(b, vector2 + new Vector2((float)(-22 * Game1.pixelZoom), (float)(num + Game1.pixelZoom * 4)), 1f, 1f, 0.9f, false);
                        }
                        IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), (int)((double)vector2.X + (double)-this.itemSlotWidth - (double)this.categoryLabelsWidth - (double)(Game1.pixelZoom * 3)), (int)((double)vector2.Y + (double)num), this.categoryLabelsWidth, 26 * Game1.pixelZoom, Color.White, (float)Game1.pixelZoom, false);
                        SpriteText.drawString(b, textureComponent.hoverText, (int)vector2.X - this.itemSlotWidth - this.categoryLabelsWidth + Game1.pixelZoom * 2, (int)vector2.Y + Game1.pixelZoom, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
                        for (int index2 = 0; index2 < 6; ++index2)
                            b.Draw(Game1.mouseCursors, vector2 + new Vector2((float)(-this.itemSlotWidth - Game1.tileSize * 3 - Game1.pixelZoom * 6 + index2 * 6 * Game1.pixelZoom), (float)(3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(355, 476, 7, 11)), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
                        this.categoryDials[index1].draw(b, vector2 + new Vector2((float)(-this.itemSlotWidth - Game1.tileSize * 3 - Game1.pixelZoom * 12 + Game1.pixelZoom), (float)(5 * Game1.pixelZoom)), this.categoryTotals[index1]);
                        b.Draw(Game1.mouseCursors, vector2 + new Vector2((float)(-this.itemSlotWidth - Game1.tileSize - Game1.pixelZoom), (float)(3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(408, 476, 9, 11)), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
                    }
                    ++index1;
                }
                if (this.introTimer <= 0)
                    this.okButton.draw(b);
            }
            else
            {
                IClickableMenu.drawTextureBox(b, Game1.viewport.Width / 2 - 640, Game1.viewport.Height / 2 - 360, 1280, 720, Color.White);
                Vector2 location = new Vector2((float)(this.xPositionOnScreen + Game1.tileSize / 2), (float)(this.yPositionOnScreen + Game1.tileSize / 2));
                for (int index = this.currentTab * 9; index < this.currentTab * 9 + 9; ++index)
                {
                    if (Enumerable.Count<Item>((IEnumerable<Item>)this.categoryItems[this.currentPage]) > index)
                    {
                        this.categoryItems[this.currentPage][index].drawInMenu(b, location, 1f, 1f, 1f, true);
                        SpriteText.drawString(b, this.categoryItems[this.currentPage][index].Name + (this.categoryItems[this.currentPage][index].Stack <= 1 ? "" : " x" + (object)this.categoryItems[this.currentPage][index].Stack), (int)location.X + Game1.tileSize + Game1.pixelZoom * 3, (int)location.Y + Game1.pixelZoom * 3, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
                        string s = ".";
                        int num = 0;
                        while (true)
                        {
                            if (num < this.width - Game1.tileSize * 3 / 2 - SpriteText.getWidthOfString(string.Concat(new object[4]
                            {
                (object) this.categoryItems[this.currentPage][index].Name,
                (object) (this.categoryItems[this.currentPage][index].Stack <= 1 ? "" : " x" + (object) this.categoryItems[this.currentPage][index].Stack),
                (object) ((this.categoryItems[this.currentPage][index] as StardewValley.Object).sellToStorePrice() * (this.categoryItems[this.currentPage][index] as StardewValley.Object).Stack),
                (object) "g"
                            })))
                            {
                                s += " .";
                                num += SpriteText.getWidthOfString(" .");
                            }
                            else
                                break;
                        }
                        SpriteText.drawString(b, s, (int)location.X + Game1.tileSize * 5 / 4 + SpriteText.getWidthOfString(this.categoryItems[this.currentPage][index].Name + (this.categoryItems[this.currentPage][index].Stack <= 1 ? "" : " x" + (object)this.categoryItems[this.currentPage][index].Stack)), (int)location.Y + Game1.tileSize / 8, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
                        SpriteText.drawString(b, Convert.ToString(((this.categoryItems[this.currentPage][index] as StardewValley.Object).sellToStorePrice() * (this.categoryItems[this.currentPage][index] as StardewValley.Object).Stack)) +"g", (int)location.X + this.width - Game1.tileSize - SpriteText.getWidthOfString(Convert.ToString(((this.categoryItems[this.currentPage][index] as StardewValley.Object).sellToStorePrice() * (this.categoryItems[this.currentPage][index] as StardewValley.Object).Stack)) + "g"), (int)location.Y + Game1.pixelZoom * 3, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
                        location.Y += (float)(Game1.tileSize + Game1.pixelZoom);
                    }
                }
                this.backButton.draw(b);
                if (this.showForwardButton())
                    this.forwardButton.draw(b);
            }
            if (this.outro)
            {
                b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Color.Black * (float)(1.0 - (double)this.outroFadeTimer / 800.0));
                SpriteBatch b1 = b;
                string s;
                if (this.newDayPlaque)
                    s = (string)(object)Game1.dayOfMonth + (object)Utility.getNumberEnding(Game1.dayOfMonth) + " of " + Utility.getSeasonNameFromNumber(Utility.getSeasonNumber(Game1.currentSeason)) + ", Year " + (string)(object)Game1.year;
                else
                    s = Utility.getYesterdaysDate();
                int x = Game1.viewport.Width / 2;
                int y = this.dayPlaqueY;
                string placeHolderWidthText = "";
                double num1 = 1.0;
                int color = -1;
                int scrollType = 0;
                double num2 = 0.879999995231628;
                int num3 = 0;
                SpriteText.drawStringWithScrollCenteredAt(b1, s, x, y, placeHolderWidthText, (float)num1, color, scrollType, (float)num2, num3 != 0);
                foreach (TemporaryAnimatedSprite temporaryAnimatedSprite in this.animations)
                    temporaryAnimatedSprite.draw(b, true, 0, 0);
                if (this.finalOutroTimer > 0)
                    b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * (float)(1.0 - (double)this.finalOutroTimer / 2000.0));
            }
            if (this.saveGameMenu != null)
                this.saveGameMenu.draw(b);
            this.drawMouse(b);
        }
    }
}
