using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Minigames;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
    public class SpriteKeyboard : IClickableMenu
    {


        private List<ClickableComponent> labels = new List<ClickableComponent>();

        public List<ClickableTextureComponent> keyboardButtonsLowerCase;
        public List<ClickableTextureComponent> keyboardButtonsUpperCase;


        private ClickableTextureComponent okButton;

        private TextBox nameBox;



        private string hoverText;

        private string hoverTitle;

        private ClickableComponent nameLabel;


        public SpriteKeyboard() : base(Game1.viewport.Width / 2 - (632 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize, 632 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2 + Game1.tileSize, false)
        {
            keyboardButtonsLowerCase = new List<ClickableTextureComponent>();
            keyboardButtonsUpperCase = new List<ClickableTextureComponent>();
            this.setUpPositions();
            Game1.player.faceDirection(2);
            Game1.player.FarmerSprite.StopAnimation();
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
           
            base.gameWindowSizeChanged(oldBounds, newBounds);
            this.xPositionOnScreen = Game1.viewport.Width / 2 - (632 + IClickableMenu.borderWidth * 2) / 2;
            this.yPositionOnScreen = Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize;
            this.setUpPositions();
        }

        private void setUpPositions()
        {
            this.labels.Clear();
           

            this.keyboardButtonsLowerCase.Clear();
            this.keyboardButtonsUpperCase.Clear();


            this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
            this.nameBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.smallFont, Game1.textColor)
            {
                X = this.xPositionOnScreen + Game1.tileSize + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4,
                Y = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4,
                Text = Game1.player.name
            };
            this.labels.Add(this.nameLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 8, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Name", new object[0])));
                 
            int num = Game1.tileSize * 2;
       
  
         
            num = Game1.tileSize * 4 + 8;

             //   Point point = new Point(this.xPositionOnScreen + this.width + Game1.pixelZoom + Game1.tileSize / 8, this.yPositionOnScreen + IClickableMenu.borderWidth * 2);
             //   this.farmTypeButtons.Add(new ClickableTextureComponent("Standard", new Rectangle(point.X, point.Y + 22 * Game1.pixelZoom, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmStandard", new object[0]), Game1.mouseCursors, new Rectangle(0, 324, 22, 20), (float)Game1.pixelZoom, false));
             //   this.farmTypeButtons.Add(new ClickableTextureComponent("Riverland", new Rectangle(point.X, point.Y + 22 * Game1.pixelZoom * 2, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmFishing", new object[0]), Game1.mouseCursors, new Rectangle(22, 324, 22, 20), (float)Game1.pixelZoom, false));
             //   this.farmTypeButtons.Add(new ClickableTextureComponent("Forest", new Rectangle(point.X, point.Y + 22 * Game1.pixelZoom * 3, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmForaging", new object[0]), Game1.mouseCursors, new Rectangle(44, 324, 22, 20), (float)Game1.pixelZoom, false));
             //   this.farmTypeButtons.Add(new ClickableTextureComponent("Hills", new Rectangle(point.X, point.Y + 22 * Game1.pixelZoom * 4, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmMining", new object[0]), Game1.mouseCursors, new Rectangle(66, 324, 22, 20), (float)Game1.pixelZoom, false));
             //   this.farmTypeButtons.Add(new ClickableTextureComponent("Wilderness", new Rectangle(point.X, point.Y + 22 * Game1.pixelZoom * 5, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmCombat", new object[0]), Game1.mouseCursors, new Rectangle(88, 324, 22, 20), (float)Game1.pixelZoom, false));
            
           // this.labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_EyeColor", new object[0])));

            num += Game1.tileSize + 8;
      
          // this.labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_HairColor", new object[0])));

            num += Game1.tileSize + 8;
       
          // this.labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_PantsColor", new object[0])));

           
            num += Game1.tileSize + 8;
       
       }

        private void optionButtonClick(string name)
        {
            if (name == "OK")
            {
                if (!this.canLeaveMenu())
                {
                    return;
                }
                Game1.player.Name = this.nameBox.Text.Trim();
                    Game1.exitActiveMenu();
                if (Game1.currentMinigame != null && Game1.currentMinigame is Intro)
                {
                    (Game1.currentMinigame as Intro).doneCreatingCharacter();
                }
        
                
            }
            Game1.playSound("coin");
        }

        private void selectionClick(string name, int change)
        {
            if (name == "Skin")
            {
                Game1.player.changeSkinColor(Game1.player.skin + change);
                Game1.playSound("skeletonStep");
                return;
            }
            if (name == "Hair")
            {
                Game1.player.changeHairStyle(Game1.player.hair + change);
                Game1.playSound("grassyStep");
                return;
            }
            if (name == "Shirt")
            {
                Game1.player.changeShirt(Game1.player.shirt + change);
                Game1.playSound("coin");
                return;
            }
            if (name == "Acc")
            {
                Game1.player.changeAccessory(Game1.player.accessory + change);
                Game1.playSound("purchase");
                return;
            }
            if (!(name == "Direction"))
            {
                return;
            }
            Game1.player.faceDirection((Game1.player.facingDirection - change + 4) % 4);
            Game1.player.FarmerSprite.StopAnimation();
            Game1.player.completelyStopAnimatingOrDoingAction();
            Game1.playSound("pickUpItem");
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            /*
            foreach (ClickableComponent current in this.genderButtons)
            {
                if (current.containsPoint(x, y))
                {
                    this.optionButtonClick(current.name);
                    current.scale -= 0.5f;
                    current.scale = Math.Max(3.5f, current.scale);
                }
            }
            */
            /*
            foreach (ClickableComponent current4 in this.leftSelectionButtons)
            {
                if (current4.containsPoint(x, y))
                {
                    this.selectionClick(current4.name, -1);
                    current4.scale -= 0.25f;
                    current4.scale = Math.Max(0.75f, current4.scale);
                }
            }*/


            if (this.okButton.containsPoint(x, y) && this.canLeaveMenu())
            {
                this.optionButtonClick(this.okButton.name);
                this.okButton.scale -= 0.25f;
                this.okButton.scale = Math.Max(0.75f, this.okButton.scale);
            }

        
                this.nameBox.Update();

            
        }

        public override void leftClickHeld(int x, int y)
        {
           
        }

        public override void releaseLeftClick(int x, int y)
        {
            
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
        }

        public override void receiveKeyPress(Keys key)
        {
            if (key == Keys.Tab)
            {
                if (this.nameBox.Selected)
                {
                    this.nameBox.Selected = false;
                    return;
                }

                this.nameBox.SelectMe();
            }
        }

        public override void performHoverAction(int x, int y)
        {
            this.hoverText = "";
            this.hoverTitle = "";
            /*
            using (List<ClickableComponent>.Enumerator enumerator = this.leftSelectionButtons.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ClickableTextureComponent clickableTextureComponent = (ClickableTextureComponent)enumerator.Current;
                    if (clickableTextureComponent.containsPoint(x, y))
                    {
                        clickableTextureComponent.scale = Math.Min(clickableTextureComponent.scale + 0.02f, clickableTextureComponent.baseScale + 0.1f);
                    }
                    else
                    {
                        clickableTextureComponent.scale = Math.Max(clickableTextureComponent.scale - 0.02f, clickableTextureComponent.baseScale);
                    }
                }
            }
            */


            /*
            if (!this.wizardSource)
            {
                using (List<ClickableComponent>.Enumerator enumerator = this.genderButtons.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ClickableTextureComponent clickableTextureComponent3 = (ClickableTextureComponent)enumerator.Current;
                        if (clickableTextureComponent3.containsPoint(x, y))
                        {
                            clickableTextureComponent3.scale = Math.Min(clickableTextureComponent3.scale + 0.02f, clickableTextureComponent3.baseScale + 0.1f);
                        }
                        else
                        {
                            clickableTextureComponent3.scale = Math.Max(clickableTextureComponent3.scale - 0.02f, clickableTextureComponent3.baseScale);
                        }
                    }
                }
                using (List<ClickableComponent>.Enumerator enumerator = this.petButtons.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ClickableTextureComponent clickableTextureComponent4 = (ClickableTextureComponent)enumerator.Current;
                        if (clickableTextureComponent4.containsPoint(x, y))
                        {
                            clickableTextureComponent4.scale = Math.Min(clickableTextureComponent4.scale + 0.02f, clickableTextureComponent4.baseScale + 0.1f);
                        }
                        else
                        {
                            clickableTextureComponent4.scale = Math.Max(clickableTextureComponent4.scale - 0.02f, clickableTextureComponent4.baseScale);
                        }
-                    }
                }
            }
            */
            if (this.okButton.containsPoint(x, y) && this.canLeaveMenu())
            {
                this.okButton.scale = Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.1f);
            }
            else
            {
                this.okButton.scale = Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);
            }

        }

        public bool canLeaveMenu()
        {
            return true;
        }

        public override void draw(SpriteBatch b)
        {
            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
            b.Draw(Game1.daybg, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize + Game1.tileSize * 2 / 3 - 2), (float)(this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4)), Color.White);
          //  Game1.player.FarmerRenderer.draw(b, Game1.player.FarmerSprite.CurrentAnimationFrame, Game1.player.FarmerSprite.CurrentFrame, Game1.player.FarmerSprite.SourceRect, new Vector2((float)(this.xPositionOnScreen - 2 + Game1.tileSize * 2 / 3 + Game1.tileSize * 2 - Game1.tileSize / 2), (float)(this.yPositionOnScreen + IClickableMenu.borderWidth - Game1.tileSize / 4 + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 2)), Vector2.Zero, 0.8f, Color.White, 0f, 1f, Game1.player);
            
           
            /*    
                using (List<ClickableComponent>.Enumerator enumerator = this.genderButtons.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ClickableTextureComponent clickableTextureComponent = (ClickableTextureComponent)enumerator.Current;
                        clickableTextureComponent.draw(b);
                        if ((clickableTextureComponent.name.Equals("Male") && Game1.player.isMale) || (clickableTextureComponent.name.Equals("Female") && !Game1.player.isMale))
                        {
                            b.Draw(Game1.mouseCursors, clickableTextureComponent.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 34, -1, -1)), Color.White);
                        }
                    }
                }
                
                using (List<ClickableComponent>.Enumerator enumerator = this.petButtons.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ClickableTextureComponent clickableTextureComponent2 = (ClickableTextureComponent)enumerator.Current;
                        clickableTextureComponent2.draw(b);
                        if ((clickableTextureComponent2.name.Equals("Cat") && Game1.player.catPerson) || (clickableTextureComponent2.name.Equals("Dog") && !Game1.player.catPerson))
                        {
                            b.Draw(Game1.mouseCursors, clickableTextureComponent2.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 34, -1, -1)), Color.White);
                        }
                    }
                }
                */
                Game1.player.name = this.nameBox.Text;
            
            /*
            using (List<ClickableComponent>.Enumerator enumerator = this.leftSelectionButtons.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ((ClickableTextureComponent)enumerator.Current).draw(b);
                }
            }
            */
            foreach (ClickableComponent current in this.labels)
            {
                string text = "";
                Color color = Game1.textColor;
                if (current == this.nameLabel)
                {
                    color = ((Game1.player.name.Length < 1) ? Color.Red : Game1.textColor);
                    
                }
                else
                {
                    color = Game1.textColor;
                }
                Utility.drawTextWithShadow(b, current.name, Game1.smallFont, new Vector2((float)current.bounds.X, (float)current.bounds.Y), color, 1f, -1f, -1, -1, 1f, 3);
                if (text.Length > 0)
                {
                    Utility.drawTextWithShadow(b, text, Game1.smallFont, new Vector2((float)(current.bounds.X + Game1.tileSize / 3) - Game1.smallFont.MeasureString(text).X / 2f, (float)(current.bounds.Y + Game1.tileSize / 2)), color, 1f, -1f, -1, -1, 1f, 3);
                }
            }

            /*
            if (!this.wizardSource)
            {
                IClickableMenu.drawTextureBox(b, this.farmTypeButtons[0].bounds.X - Game1.pixelZoom * 4, this.farmTypeButtons[0].bounds.Y - Game1.pixelZoom * 5, 30 * Game1.pixelZoom, 110 * Game1.pixelZoom + Game1.pixelZoom * 9, Color.White);
                for (int i = 0; i < this.farmTypeButtons.Count; i++)
                {
                    this.farmTypeButtons[i].draw(b, this.farmTypeButtons[i].name.Contains("Gray") ? (Color.Black * 0.5f) : Color.White, 0.88f);
                    if (this.farmTypeButtons[i].name.Contains("Gray"))
                    {
                        b.Draw(Game1.mouseCursors, new Vector2((float)(this.farmTypeButtons[i].bounds.Center.X - Game1.pixelZoom * 3), (float)(this.farmTypeButtons[i].bounds.Center.Y - Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(107, 442, 7, 8)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
                    }
                    if (i == Game1.whichFarm)
                    {
                        IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(375, 357, 3, 3), this.farmTypeButtons[i].bounds.X, this.farmTypeButtons[i].bounds.Y - Game1.pixelZoom, this.farmTypeButtons[i].bounds.Width, this.farmTypeButtons[i].bounds.Height + Game1.pixelZoom * 2, Color.White, (float)Game1.pixelZoom, false);
                    }
                }
            }
            */
            if (this.canLeaveMenu())
            {
                this.okButton.draw(b, Color.White, 0.75f);
            }
            else
            {
                this.okButton.draw(b, Color.White, 0.75f);
                this.okButton.draw(b, Color.Black * 0.5f, 0.751f);
            }

         
                this.nameBox.Draw(b);
            
            if (this.hoverText != null && this.hoverTitle != null && this.hoverText.Count<char>() > 0)
            {
                IClickableMenu.drawHoverText(b, Game1.parseText(this.hoverText, Game1.smallFont, Game1.tileSize * 4), Game1.smallFont, 0, 0, -1, this.hoverTitle, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
            }
            base.drawMouse(b);
        }
    }
}
