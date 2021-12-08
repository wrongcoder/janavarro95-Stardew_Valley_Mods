using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Netcode;
using StardewValley;

namespace Revitalize.Framework.World.Objects.Items
{
    /// <summary>
    /// TODO: Add way to get big craftable here as well?
    /// </summary>
    public class StardewValleyItemReference:ItemReference
    {

        public readonly NetEnum<Enums.SDVObject> objectId = new NetEnum<Enums.SDVObject>();

        public StardewValleyItemReference()
        {
            this.objectId.Value =  Enums.SDVObject.NULL;
        }

        public StardewValleyItemReference(Enums.SDVObject objectId, int StackSize = 1):base(StackSize)
        {
            this.objectId.Value = objectId;
        }

        public override Item getItem()
        {
            return this.getItem(this.stackSize.Value);
        }

        public override Item getItem(int StackSize = 1)
        {
            if (this.objectId.Value != Enums.SDVObject.NULL)
            {
                return new StardewValley.Object(Vector2.Zero, (int)this.objectId.Value, StackSize);
            }
            return null;
        }

        public override List<INetSerializable> getNetFields()
        {
            List<INetSerializable> netFields = base.getNetFields();
            netFields.Add(this.objectId);
            return netFields;
        }

        public override ItemReference readItemReference(BinaryReader reader)
        {
            base.readItemReference(reader);
            this.objectId.Value = reader.ReadEnum<Enums.SDVObject>();
            return this;
        }

        public override void writeItemReference(BinaryWriter writer)
        {
            base.writeItemReference(writer);
            writer.WriteEnum(this.objectId.Value);
        }
    }
}
