using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revitalize.Framework.Energy;
using Revitalize.Framework.Objects;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons;

namespace Revitalize.Framework.Menus.Machines
{
    /// <summary>
    /// TODO:
    /// Add in minutes remaining display
    /// Add in remaining inventory space display.
    /// Make crafting menu require the object passed in to count down before crafting the recipe.
    /// </summary>
    public class MachineSummaryMenu : IClickableMenuExtended
    {

        /// <summary>
        /// The custom object to be gathering information from.
        /// </summary>
        private CustomObject objectSource;
        /// <summary>
        /// The background color for the menu.
        /// </summary>
        private Color backgroundColor;
        /// <summary>
        /// The hover text to display for the menu.
        /// </summary>
        private string hoverText;


        private AnimatedButton batteryBackground;
        private AnimatedButton battergyEnergyGuage;
        private Vector2 energyPosition;
        private Texture2D energyTexture;
        private Vector2 itemDisplayOffset;

        private EnergyManager energy
        {
            get
            {
                return this.objectSource.EnergyManager;
            }
        }


        /// <summary>
        /// Should this menu draw the battery for the energy guage?
        /// </summary>
        private bool shouldDrawBattery
        {
            get
            {
                return this.energy.maxEnergy != 0 || ModCore.Configs.machinesConfig.doMachinesConsumeEnergy==false;
            }
        }

        public MachineSummaryMenu()
        {

        }

        public MachineSummaryMenu(int x, int y, int width, int height, Color BackgroundColor, CustomObject SourceObject) : base(x, y, width, height, false)
        {

            this.objectSource = SourceObject;
            this.backgroundColor = BackgroundColor;
            this.energyTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            this.colorSwap();

            this.energyPosition = new Vector2(this.xPositionOnScreen + this.width - 128, this.yPositionOnScreen + this.height - 72 * 4);
            this.batteryBackground = new AnimatedButton(new StardustCore.Animations.AnimatedSprite("BatteryFrame", this.energyPosition, new StardustCore.Animations.AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Menus.EnergyMenu", "BatteryFrame"), new StardustCore.Animations.Animation(0, 0, 32, 64)), Color.White), new Rectangle(0, 0, 32, 64), 4f);
            this.battergyEnergyGuage = new AnimatedButton(new StardustCore.Animations.AnimatedSprite("BatteryEnergyGuage", this.energyPosition, new StardustCore.Animations.AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Menus.EnergyMenu", "BatteryEnergyGuage"), new StardustCore.Animations.Animation(0, 0, 32, 64)), Color.White), new Rectangle(0, 0, 32, 64), 4f);

            this.itemDisplayOffset = ObjectUtilities.GetDimensionOffsetFromItem(this.objectSource);
        }

        public override void performHoverAction(int x, int y)
        {
            bool hovered = false;
            if (this.batteryBackground.containsPoint(x, y) && this.shouldDrawBattery)
            {
                this.hoverText = "Energy: " + this.energy.energyDisplayString;
                hovered = true;
            }

            if (hovered == false)
            {
                this.hoverText = "";
            }

        }


        /// <summary>
        /// Draws the menu to the screen.
        /// </summary>
        /// <param name="b"></param>
        public override void draw(SpriteBatch b)
        {
            this.drawDialogueBoxBackground(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, this.backgroundColor);

            //Draw the energy on the screen.

            if (this.shouldDrawBattery)
            {
                this.batteryBackground.draw(b, 1f, 1f);
                this.colorSwap();
                b.Draw(this.energyTexture, new Rectangle((int)this.energyPosition.X + (int)(11 * this.batteryBackground.scale), (int)this.energyPosition.Y + (int)(18 * this.batteryBackground.scale)+ (int)(46 * this.batteryBackground.scale), (int)((9 * this.batteryBackground.scale)), (int)(46 * this.batteryBackground.scale * this.energy.energyPercentRemaining)), new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0f,1f), SpriteEffects.None, 0.2f);
                this.battergyEnergyGuage.draw(b, 1f, 1f);
            }


            this.objectSource.drawFullyInMenu(b, new Vector2((int)(this.xPositionOnScreen + (this.width / 2) - (this.itemDisplayOffset.X / 2)), (int)(this.yPositionOnScreen + 128f)), .24f);
            Vector2 nameOffset = Game1.dialogueFont.MeasureString(this.objectSource.DisplayName);

            b.DrawString(Game1.dialogueFont, this.objectSource.DisplayName, new Vector2(this.xPositionOnScreen + (this.width / 2) - nameOffset.X / 2, (this.yPositionOnScreen + 150f)) + new Vector2(0, ObjectUtilities.GetHeightOffsetFromItem(this.objectSource)), Color.Black);

            if (string.IsNullOrEmpty(this.hoverText) == false)
            {
                IClickableMenuExtended.drawHoverText(b, this.hoverText, Game1.dialogueFont);
            }


            this.drawMouse(b);


        }
        /// <summary>
        /// Swaps the color for the energy bar meter depending on how much energy is left.
        /// </summary>

        private void colorSwap()
        {
            Color col = new Color();
            //ModCore.log("Energy is: " + this.energy.energyPercentRemaining);
            if (this.energy.energyPercentRemaining > .75d)
            {
                col = Color.Green;
            }
            else if (this.energy.energyPercentRemaining > .5d && this.energy.energyPercentRemaining <= .75d)
            {
                col = Color.GreenYellow;
            }
            else if (this.energy.energyPercentRemaining > .25d && this.energy.energyPercentRemaining <= .5d)
            {
                col = Color.Yellow;
            }
            else if (this.energy.energyPercentRemaining > .10d && this.energy.energyPercentRemaining <= .25d)
            {
                col = Color.Orange;
            }
            else
            {
                col = Color.Red;
            }

            Color[] color = new Color[1]
            {
                col
            };
            this.energyTexture.SetData<Color>(color);
        }
    }
}
