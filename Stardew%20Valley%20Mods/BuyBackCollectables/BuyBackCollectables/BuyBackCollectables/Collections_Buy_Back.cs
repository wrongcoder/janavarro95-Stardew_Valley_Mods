using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
namespace StardewValley.Menus
{
    public class Collections_Buy_Back : IClickableMenu
    {
        public const int organicsTab = 0;

        public const int fishTab = 1;

        public const int archaeologyTab = 2;

        public const int mineralsTab = 3;

        public const int cookingTab = 4;

        public const int achievementsTab = 5;

        public const int distanceFromMenuBottomBeforeNewPage = 128;

        public static int widthToMoveActiveTab = Game1.tileSize / 8;

        public string descriptionText = "";

        public string hoverText = "";

        public ClickableTextureComponent backButton;

        public ClickableTextureComponent forwardButton;

        public List<ClickableTextureComponent> sideTabs = new List<ClickableTextureComponent>();

        public int currentTab;

        public int currentPage;

        public Dictionary<int, List<List<ClickableTextureComponent>>> collections = new Dictionary<int, List<List<ClickableTextureComponent>>>();

        public int value;

        public Item new_item;

        public Collections_Buy_Back(int x, int y, int width, int height) : base(x, y, width, height, false)
        {
            this.sideTabs.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4 + CollectionsPage.widthToMoveActiveTab, this.yPositionOnScreen + Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), "", "Items Shipped (Farm & Forage)", Game1.mouseCursors, new Rectangle(640, 80, 16, 16), (float)Game1.pixelZoom));
            this.collections.Add(0, new List<List<ClickableTextureComponent>>());
            this.sideTabs.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 3, Game1.tileSize, Game1.tileSize), "", "Fish", Game1.mouseCursors, new Rectangle(640, 64, 16, 16), (float)Game1.pixelZoom));
            this.collections.Add(1, new List<List<ClickableTextureComponent>>());
            this.sideTabs.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 4, Game1.tileSize, Game1.tileSize), "", "Artifacts", Game1.mouseCursors, new Rectangle(656, 64, 16, 16), (float)Game1.pixelZoom));
            this.collections.Add(2, new List<List<ClickableTextureComponent>>());
            this.sideTabs.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 5, Game1.tileSize, Game1.tileSize), "", "Minerals", Game1.mouseCursors, new Rectangle(672, 64, 16, 16), (float)Game1.pixelZoom));
            this.collections.Add(3, new List<List<ClickableTextureComponent>>());
            this.sideTabs.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 6, Game1.tileSize, Game1.tileSize), "", "Cooking", Game1.mouseCursors, new Rectangle(688, 64, 16, 16), (float)Game1.pixelZoom));
            this.collections.Add(4, new List<List<ClickableTextureComponent>>());
            this.sideTabs.Add(new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 7, Game1.tileSize, Game1.tileSize), "", "Achievements", Game1.mouseCursors, new Rectangle(656, 80, 16, 16), (float)Game1.pixelZoom));
            this.collections.Add(5, new List<List<ClickableTextureComponent>>());
            Collections_Buy_Back.widthToMoveActiveTab = Game1.tileSize / 8;
            this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3 / 4, this.yPositionOnScreen + height - 20 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false, false);
            this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width - Game1.tileSize / 2 - 15 * Game1.pixelZoom, this.yPositionOnScreen + height - 20 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false, false);
            int[] array = new int[this.sideTabs.Count<ClickableTextureComponent>()];
            int num = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder;
            int num2 = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4;
            int num3 = 10;
            foreach (KeyValuePair<int, string> current in Game1.objectInformation)
            {
                string text = current.Value.Split(new char[]
                {
                    '/'
                })[3];
                bool flag = false;
                int num4;
                if (text.Contains("Arch"))
                {
                    num4 = 2;
                    if (Game1.player.archaeologyFound.ContainsKey(current.Key))
                    {
                        flag = true;
                    }
                }
                else if (text.Contains("Fish"))
                {
                    if (current.Key >= 167 && current.Key < 173)
                    {
                        continue;
                    }
                    num4 = 1;
                    if (Game1.player.fishCaught.ContainsKey(current.Key))
                    {
                        flag = true;
                    }
                }
                else if (text.Contains("Mineral") || text.Substring(text.Count<char>() - 3).Equals("-2"))
                {
                    num4 = 3;
                    if (Game1.player.mineralsFound.ContainsKey(current.Key))
                    {
                        flag = true;
                    }
                }
                else if (text.Contains("Cooking") || text.Substring(text.Count<char>() - 3).Equals("-7"))
                {
                    num4 = 4;
                    if (Game1.player.recipesCooked.ContainsKey(current.Key))
                    {
                        flag = true;
                    }
                    if (current.Key == 217 || current.Key == 772)
                    {
                        continue;
                    }
                    if (current.Key == 773)
                    {
                        continue;
                    }
                }
                else
                {
                    if (!StardewValley.Object.isPotentialBasicShippedCategory(current.Key, text.Substring(text.Count<char>() - 3)))
                    {
                        continue;
                    }
                    num4 = 0;
                    if (Game1.player.basicShipped.ContainsKey(current.Key))
                    {
                        flag = true;
                    }
                }
                int x2 = num + array[num4] % num3 * (Game1.tileSize + 4);
                int num5 = num2 + array[num4] / num3 * (Game1.tileSize + 4);
                if (num5 > this.yPositionOnScreen + height - 128)
                {
                    this.collections[num4].Add(new List<ClickableTextureComponent>());
                    array[num4] = 0;
                    x2 = num;
                    num5 = num2;
                }
                if (this.collections[num4].Count<List<ClickableTextureComponent>>() == 0)
                {
                    this.collections[num4].Add(new List<ClickableTextureComponent>());
                }
                this.collections[num4].Last<List<ClickableTextureComponent>>().Add(new ClickableTextureComponent(new Rectangle(x2, num5, Game1.tileSize, Game1.tileSize), current.Key + " " + flag, "", Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, current.Key, 16, 16), (float)Game1.pixelZoom, false, flag));
                array[num4]++;
            }
            if (this.collections[5].Count<List<ClickableTextureComponent>>() == 0)
            {
                this.collections[5].Add(new List<ClickableTextureComponent>());
            }
            foreach (KeyValuePair<int, string> current2 in Game1.achievements)
            {
                bool flag2 = Game1.player.achievements.Contains(current2.Key);
                string[] array2 = current2.Value.Split(new char[]
                {
                    '^'
                });
                if (flag2 || (array2[2].Equals("true") && (array2[3].Equals("-1") || this.farmerHasAchievements(array2[3]))))
                {
                    int x3 = num + array[5] % num3 * (Game1.tileSize + 4);
                    int y2 = num2 + array[5] / num3 * (Game1.tileSize + 4);
                    this.collections[5][0].Add(new ClickableTextureComponent(new Rectangle(x3, y2, Game1.tileSize, Game1.tileSize), current2.Key + " " + flag2, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 25, -1, -1), 1f, false, false));
                    array[5]++;
                }
            }
        }

        public virtual bool farmerHasAchievements(string listOfAchievementNumbers)
        {
            string[] array = listOfAchievementNumbers.Split(new char[]
            {
                ' '
            });
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                if (!Game1.player.achievements.Contains(Convert.ToInt32(text)))
                {
                    return false;
                }
            }
            return true;
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            for (int i = 0; i < this.sideTabs.Count; i++)
            {
                if (this.sideTabs[i].containsPoint(x, y) && this.currentTab != i)
                {
                    Game1.playSound("smallSelect");
                    ClickableTextureComponent expr_45_cp_0 = this.sideTabs[this.currentTab];
                    expr_45_cp_0.bounds.X = expr_45_cp_0.bounds.X - CollectionsPage.widthToMoveActiveTab;
                    this.currentTab = i;
                    this.currentPage = 0;
                    ClickableTextureComponent expr_75_cp_0 = this.sideTabs[i];
                    expr_75_cp_0.bounds.X = expr_75_cp_0.bounds.X + CollectionsPage.widthToMoveActiveTab;
                }
            }
            if (this.currentPage > 0 && this.backButton.containsPoint(x, y))
            {
                this.currentPage--;
                Game1.playSound("shwip");
                this.backButton.scale = this.backButton.baseScale;
                this.new_item = null;
            }
            if (this.currentPage < this.collections[this.currentTab].Count<List<ClickableTextureComponent>>() - 1 && this.forwardButton.containsPoint(x, y))
            {
                this.currentPage++;
                Game1.playSound("shwip");
                this.forwardButton.scale = this.forwardButton.baseScale;
                this.new_item = null;
            }


            foreach (ClickableTextureComponent current2 in this.collections[this.currentTab][this.currentPage])
            {
                if (current2.containsPoint(x, y))
                {
                    if (new_item != null)
                    {
                        if (Game1.player.money > new_item.salePrice() * Buy_Back_Collectables.Class1.cost)
                        {
                            Game1.player.money -= value;
                            Game1.player.addItemByMenuIfNecessary(new_item);
                        }

                    }
                }
            }


            
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            if (new_item != null)
            {
                if (Game1.player.money > new_item.salePrice() * Buy_Back_Collectables.Class1.cost)
                {
                  
                    Game1.player.money -= value;
                    Game1.player.addItemByMenuIfNecessary(new_item);
                }

            }
        }

        public override void performHoverAction(int x, int y)
        {
            this.descriptionText = "";
            this.hoverText = "";
            this.value = -1;
            foreach (ClickableTextureComponent current in this.sideTabs)
            {
                if (current.containsPoint(x, y))
                {
                    this.hoverText = current.hoverText;
                    return;
                }
            }
            foreach (ClickableTextureComponent current2 in this.collections[this.currentTab][this.currentPage])
            {
                if (current2.containsPoint(x, y))
                {
                    current2.scale = Math.Min(current2.scale + 0.02f, current2.baseScale + 0.1f);
                    if (Convert.ToBoolean(current2.name.Split(new char[]
                    {
                        ' '
                    })[1]) || this.currentTab == 5)
                    {
                        this.hoverText = this.createDescription(Convert.ToInt32(current2.name.Split(new char[]
                        {
                            ' '
                        })[0]));
                    }
                    else
                    {
                        this.hoverText = "???";
                        this.new_item = null;
                    }
                }
                else
                {
                    current2.scale = Math.Max(current2.scale - 0.02f, current2.baseScale);
                }
            }
            this.forwardButton.tryHover(x, y, 0.5f);
            this.backButton.tryHover(x, y, 0.5f);
        }

        public virtual string createDescription(int index)
        {
            string text = "";
            if (this.currentTab == 5)
            {
                string[] array = Game1.achievements[index].Split(new char[]
                {
                    '^'
                });
                text = text + array[0] + Environment.NewLine + Environment.NewLine;
                text += array[1];
                new_item = null;
            }
            else
            {
                string[] array2 = Game1.objectInformation[index].Split(new char[]
                {
                    '/'
                });
               
                string text2 = text;
                foreach(KeyValuePair<int, string> meh in Game1.objectInformation)
                {
                    string[] array3 = meh.Value.Split(new char[]
                    {
                    '/'
                    });
                    if (array3[0] == array2[0])
                    {
                        new_item = (Item)new Object(Convert.ToInt32(meh.Key), 1, false, -1, 0);
                        if (new_item.Name == "Stone" || new_item.Name=="stone") new_item = (Item)new Object(390, 1, false, -1, 0);
                    }
                }
                text = string.Concat(new string[]
                {
                    text2,
                    array2[0],
                    Environment.NewLine,
                    Environment.NewLine,
                    Game1.parseText(array2[4], Game1.smallFont, Game1.tileSize * 4),
                    Environment.NewLine,
                    Environment.NewLine
                });
                if (array2[3].Contains("Arch"))
                {
                    text += (Game1.player.archaeologyFound.ContainsKey(index) ? ("Total Found: " + Game1.player.archaeologyFound[index][0]) : "");
                }
                else if (array2[3].Contains("Cooking"))
                {
                    text += (Game1.player.recipesCooked.ContainsKey(index) ? ("Times Cooked: " + Game1.player.recipesCooked[index]) : "");
                }
                else if (array2[3].Contains("Fish"))
                {
                    text = text + "Number Caught: " + (Game1.player.fishCaught.ContainsKey(index) ? Game1.player.fishCaught[index][0] : 0);
                    if (Game1.player.fishCaught.ContainsKey(index) && Game1.player.fishCaught[index][1] > 0)
                    {
                        object obj = text;
                        text = string.Concat(new object[]
                        {
                            obj,
                            Environment.NewLine,
                            "Biggest Catch: ",
                            Game1.player.fishCaught[index][1],
                            " in."
                        });
                    }
                }
                else if (array2[3].Contains("Minerals") || array2[3].Substring(array2[3].Count<char>() - 3).Equals("-2"))
                {
                    text = text + "Number Found: " + (Game1.player.mineralsFound.ContainsKey(index) ? Game1.player.mineralsFound[index] : 0);
                }
                else
                {
                    text = text + "Number Shipped: " + (Game1.player.basicShipped.ContainsKey(index) ? Game1.player.basicShipped[index] : 0);
                }
                this.value = Convert.ToInt32(array2[1]);
                this.value =(int)(this.value * Buy_Back_Collectables.Class1.cost);
            }
            return text;
        }

        public override void draw(SpriteBatch b)
        {
            foreach (ClickableTextureComponent current in this.sideTabs)
            {
                current.draw(b);
            }
            if (this.currentPage > 0)
            {
                this.backButton.draw(b);
            }
            if (this.currentPage < this.collections[this.currentTab].Count<List<ClickableTextureComponent>>() - 1)
            {
                this.forwardButton.draw(b);
            }
            b.End();
            b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
            foreach (ClickableTextureComponent current2 in this.collections[this.currentTab][this.currentPage])
            {
                bool flag = Convert.ToBoolean(current2.name.Split(new char[]
                {
                    ' '
                })[1]);
                current2.draw(b, flag ? Color.White : (Color.Black * 0.2f), 0.86f);
                if (this.currentTab == 5 && flag)
                {
                    int num = new Random(Convert.ToInt32(current2.name.Split(new char[]
                    {
                        ' '
                    })[0])).Next(12);
                    b.Draw(Game1.mouseCursors, new Vector2((float)(current2.bounds.X + 16 + Game1.tileSize / 4), (float)(current2.bounds.Y + 20 + Game1.tileSize / 4)), new Rectangle?(new Rectangle(256 + num % 6 * Game1.tileSize / 2, 128 + num / 6 * Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4)), current2.scale, SpriteEffects.None, 0.88f);
                }
            }
            b.End();
            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            if (!this.hoverText.Equals(""))
            {
                IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, this.value, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
            }
            b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }
    }
}
