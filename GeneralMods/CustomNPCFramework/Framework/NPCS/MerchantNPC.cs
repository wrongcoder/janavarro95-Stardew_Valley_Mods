using CustomNPCFramework.Framework.ModularNPCS;
using CustomNPCFramework.Framework.ModularNPCS.ModularRenderers;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.NPCS
{
    class MerchantNPC: ExtendedNPC
    {
        public List<Item> stock;
        public MerchantNPC(List<Item> Stock, Sprite sprite, BasicRenderer renderer,Vector2 position,int facingDirection,string name): base(sprite,renderer,position,facingDirection,name)
        {
            this.stock = Stock;
        }

        public MerchantNPC(List<Item> Stock, ExtendedNPC npcBase): base(npcBase.spriteInformation, npcBase.portraitInformation, npcBase.position, npcBase.facingDirection, npcBase.name)
        {
            this.stock = Stock;
        }

        public override bool checkAction(StardewValley.Farmer who, GameLocation l)
        {
            if (Game1.activeClickableMenu == null)
            {
                Game1.activeClickableMenu = new StardewValley.Menus.ShopMenu(this.stock);
                return true;
            }
            else
            {
                return base.checkAction(Game1.player, Game1.player.currentLocation);
            }
        }
    }
}
