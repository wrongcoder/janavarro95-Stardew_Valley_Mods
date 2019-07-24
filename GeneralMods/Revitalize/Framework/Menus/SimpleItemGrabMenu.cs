using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardustCore.UIUtilities;

namespace Revitalize.Framework.Menus
{
    public class SimpleItemGrabMenu: StardustCore.UIUtilities.IClickableMenuExtended
    {
        public List<Item> stroageInventory;
        public List<Item> receivingInventory;
        public List<StardustCore.UIUtilities.MenuComponents.ItemDisplayButton> storageDisplay;
        public StardewValley.Menus.ItemGrabMenu playerInventory;

        public int amountToDisplay = 9;
        public SimpleItemGrabMenu(int xPos,int yPos,int width, int height,bool showCloseButton,List<Item> StorageInventory,List<Item>ReceivingInventory) : this(xPos, yPos, width, height, showCloseButton,StorageInventory,ReceivingInventory,9)
        {

        }

        public SimpleItemGrabMenu(int xPos, int yPos, int width, int height, bool showCloseButton, List<Item> StorageInventory, List<Item> ReceivingInventory,int AmountToDisplay) : base(xPos, yPos, width, height, showCloseButton)
        {
            this.stroageInventory = StorageInventory;
            this.receivingInventory = ReceivingInventory;
            this.storageDisplay = new List<StardustCore.UIUtilities.MenuComponents.ItemDisplayButton>();
            if (this.receivingInventory == null)
            {
                this.receivingInventory = (List<Item>)Game1.player.Items;
            }
            this.amountToDisplay = AmountToDisplay;

            this.playerInventory = new ItemGrabMenu(this.receivingInventory);
        }

        public override void performHoverAction(int x, int y)
        {
            
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            this.playerInventory.receiveLeftClick(x, y, playSound);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            
        }

        public override void update(GameTime time)
        {
            
        }

        public override void draw(SpriteBatch b)
        {
            this.drawDialogueBoxBackground();
            this.drawMouse(b);
            //base.draw(b);
        }
    }
}
