using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Revitalize.Framework.Menus
{
    public class SimpleItemGrabMenu: StardustCore.UIUtilities.IClickableMenuExtended
    {
        public List<Item> stroageInventory;
        public List<Item> receivingInventory;
        public List<StardustCore.UIUtilities.MenuComponents.Button> clickableItems;
        public SimpleItemGrabMenu(int xPos,int yPos,int width, int height,bool showCloseButton,List<Item> StorageInventory,List<Item>ReceivingInventory) : base(xPos, yPos, width, height, showCloseButton)
        {
            this.stroageInventory = StorageInventory;
            this.receivingInventory = ReceivingInventory;
            this.clickableItems = new List<StardustCore.UIUtilities.MenuComponents.Button>();
        }

        public override void performHoverAction(int x, int y)
        {
            
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            
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
