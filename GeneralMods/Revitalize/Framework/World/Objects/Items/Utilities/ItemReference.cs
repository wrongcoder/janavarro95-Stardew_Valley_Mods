using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Items.Utilities
{
    /// <summary>
    /// Used to reference the many types of items that can be used.
    /// </summary>
    [XmlType("Mods_Revitalize.Framework.World.Objects.Items.Utilities.ItemReference")]
    public class ItemReference
    {
        /// <summary>
        /// The default stack size for getting the item when using <see cref="getItem()"/>
        /// </summary>
        public readonly NetInt stackSize = new NetInt(1);
        public readonly NetString objectManagerId = new NetString("");
        public readonly NetEnum<Enums.SDVObject> sdvObjectId = new NetEnum<Enums.SDVObject>(Enums.SDVObject.NULL);
        public readonly NetEnum<Enums.SDVBigCraftable> sdvBigCraftableId = new NetEnum<Enums.SDVBigCraftable>(Enums.SDVBigCraftable.NULL);

        public ItemReference()
        {

        }

        public ItemReference(string ObjectId,int StackSize = 1)
        {
            this.objectManagerId.Value = ObjectId;
            this.stackSize.Value = StackSize;
        }

        public ItemReference(Enums.SDVObject ObjectId, int StackSize = 1)
        {
            this.sdvObjectId.Value = ObjectId;
            this.stackSize.Value = StackSize;
        }

        public ItemReference(Enums.SDVBigCraftable ObjectId, int StackSize = 1)
        {
            this.sdvBigCraftableId.Value = ObjectId;
            this.stackSize.Value = StackSize;
        }

        public virtual Item getItem(int StackSize = 1)
        {
            if (this.sdvObjectId.Value != Enums.SDVObject.NULL)
            {
                return RevitalizeModCore.ModContentManager.objectManager.getItem(this.sdvObjectId.Value, StackSize);
            }
            if (this.sdvBigCraftableId.Value != Enums.SDVBigCraftable.NULL)
            {
                return RevitalizeModCore.ModContentManager.objectManager.getItem(this.sdvBigCraftableId.Value, StackSize);
            }
            if (!string.IsNullOrEmpty(this.objectManagerId.Value))
            {
                return RevitalizeModCore.ModContentManager.objectManager.getItem(this.objectManagerId.Value, StackSize);
            }
            throw new InvalidObjectManagerItemException("An ItemReference must have one of the id fields set to be valid.");
        }

        public virtual Item getItem()
        {
            return this.getItem(this.stackSize.Value);
        }

        public virtual List<INetSerializable> getNetFields()
        {
            return new List<INetSerializable>()
            {
                this.stackSize,
                this.objectManagerId,
                this.sdvObjectId,
                this.sdvBigCraftableId
            };
        }

        public virtual ItemReference readItemReference(BinaryReader reader)
        {
            this.stackSize.Value = reader.ReadInt32();
            this.objectManagerId.Value = reader.ReadString();
            this.sdvObjectId.Value = reader.ReadEnum<Enums.SDVObject>();
            this.sdvBigCraftableId.Value = reader.ReadEnum<Enums.SDVBigCraftable>();
            return this;
        }

        public virtual void writeItemReference(BinaryWriter writer)
        {
            writer.Write(this.stackSize.Value);
            writer.Write(this.objectManagerId.Value);
            writer.WriteEnum<Enums.SDVObject>(this.sdvObjectId.Value);
            writer.WriteEnum<Enums.SDVBigCraftable>(this.sdvBigCraftableId.Value);

        }
    }
}
