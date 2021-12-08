using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netcode;
using StardewValley;

namespace Revitalize.Framework.World.Objects.Items
{
    public class ItemReference
    {

        public ItemReference()
        {

        }

        public virtual Item getItem(int StackSize = 1)
        {
            return null;
        }

        public virtual List<INetSerializable> getNetFields()
        {
            return new List<INetSerializable>(); 
        }
    }
}
