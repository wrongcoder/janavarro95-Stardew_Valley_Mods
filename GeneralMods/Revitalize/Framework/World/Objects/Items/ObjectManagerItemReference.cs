using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netcode;
using StardewValley;

namespace Revitalize.Framework.World.Objects.Items
{
    public class ObjectManagerItemReference:ItemReference
    {

        public NetString itemId = new NetString();

        public ObjectManagerItemReference()
        {

        }

        public ObjectManagerItemReference(string ItemId)
        {
            this.itemId.Value = ItemId;
        }

        public override Item getItem(int StackSize = 1)
        {
            return ModCore.ObjectManager.GetItem(this.itemId.Value, StackSize);
        }

        public override List<INetSerializable> getNetFields()
        {
            return new List<INetSerializable>()
            {
                this.itemId
            };
        }
    }
}
