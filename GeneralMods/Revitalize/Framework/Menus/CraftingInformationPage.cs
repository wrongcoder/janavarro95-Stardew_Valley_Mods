using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revitalize.Framework.Menus.MenuComponents;
using Revitalize.Framework.Objects;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.UIUtilities;

namespace Revitalize.Framework.Menus
{
    public class CraftingInformationPage:IClickableMenuExtended
    {

        public CraftingRecipeButton infoButton;
        public Color backgroundColor;

        public Vector2 itemDisplayLocation;

        public IList<Item> inventory;

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

        public CraftingInformationPage(int x, int y, int width, int height,Color BackgroundColor,CraftingRecipeButton ItemToDisplay,ref IList<Item> Inventory) : base(x, y, width, height, false)
        {
            this.backgroundColor = BackgroundColor;
            this.infoButton = ItemToDisplay;
            this.itemDisplayLocation = new Vector2(this.xPositionOnScreen + (this.width / 2) - 32, this.yPositionOnScreen + (128));
            this.inventory = Inventory;
        }

        public override void draw(SpriteBatch b)
        {
            this.drawDialogueBoxBackground(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, this.backgroundColor);
            this.infoButton.draw(b,this.itemDisplayLocation);

            b.DrawString(Game1.dialogueFont, this.actualItem.DisplayName, this.itemDisplayLocation + this.getHeightOffsetFromItem()-this.getItemNameOffset(), this.getNameColor());

            this.drawMouse(b);
        }

        public bool doesMenuContainPoint(int x, int y)
        {
            Rectangle r = new Rectangle(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height);
            return r.Contains(x, y);
        }

        public bool canCraftRecipe()
        {
            return this.infoButton.recipe.CanCraft(this.inventory);
        }

        public Color getNameColor()
        {
            if (this.canCraftRecipe()) return Color.Black;
            else return Color.Red;
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
            length.X = length.X / 4;
            length.Y = 0;
            return length;
        }

    }
}
