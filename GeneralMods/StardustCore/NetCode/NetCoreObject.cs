using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Netcode;
using StardustCore.UIUtilities;

namespace StardustCore.NetCode
{
    class NetCoreObject : Netcode.NetField<CoreObject,NetCoreObject>
    {

        public NetTexture2DExtended texture;
        public NetInt which;
        public NetVector2 tilePos;
        public NetInt InventoryMaxSize;
        public NetRectangle sourceRect;
        public NetRectangle boundingBox;

        public NetCoreObject()
        {

        }

        public NetCoreObject(CoreObject value) : base(value)
        {
        }


        protected override void ReadDelta(BinaryReader reader, NetVersion version)
        {
            texture = new NetTexture2DExtended();
            texture.Read(reader, version);
            Value.setExtendedTexture(texture.Value);

            which = new NetInt();
            which.Read(reader, version);
            Value.ParentSheetIndex = which.Value;

            tilePos = new NetVector2();
            tilePos.Read(reader, version);
            Value.TileLocation = tilePos.Value;

            InventoryMaxSize = new NetInt();
            InventoryMaxSize.Read(reader, version);
            Value.inventoryMaxSize = InventoryMaxSize.Value;

            sourceRect = new NetRectangle();
            sourceRect.Read(reader, version);
            Value.sourceRect = sourceRect.Value;

            boundingBox = new NetRectangle();
            boundingBox.Read(reader, version);
            Value.boundingBox.Value = boundingBox.Value;
        }

        protected override void WriteDelta(BinaryWriter writer)
        {
            texture = new NetTexture2DExtended(Value.getExtendedTexture());
            texture.Write(writer);

            which = new NetInt(Value.ParentSheetIndex);
            which.Write(writer);

            tilePos = new NetVector2(Value.TileLocation);
            tilePos.Write(writer);

            InventoryMaxSize = new NetInt(Value.inventoryMaxSize);
            InventoryMaxSize.Write(writer);

            sourceRect = new NetRectangle(Value.sourceRect);
            sourceRect.Write(writer);

            boundingBox = new NetRectangle(Value.boundingBox.Value);
            sourceRect.Write(writer);
        }
    }
}
