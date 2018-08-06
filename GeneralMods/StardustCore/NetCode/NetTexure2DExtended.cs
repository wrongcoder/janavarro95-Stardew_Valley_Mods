using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley;
using StardustCore.UIUtilities;

namespace StardustCore.NetCode
{
    public class NetTexture2DExtended : Netcode.NetField<UIUtilities.Texture2DExtended, NetTexture2DExtended>
    {

        public string Name;
        public Texture2D texture;
        public string path;
        public int width;
        public int height;


        public NetTexture2DExtended()
        {

        }

        public NetTexture2DExtended(Texture2DExtended value) : base(value)
        {
        }

        protected override void ReadDelta(BinaryReader reader, NetVersion version)
        {
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();
            Byte[] colorsOne = new byte[width*height*4];
            colorsOne = reader.ReadBytes(width*height*4);
            texture = new Texture2D(Game1.graphics.GraphicsDevice,width,height);
            texture.SetData(colorsOne);

            string Name = reader.ReadString();
            string path = reader.ReadString();

            if (version.IsPriorityOver(this.ChangeVersion))
            {
                this.CleanSet(new UIUtilities.Texture2DExtended(ModCore.ModHelper, path),true);
            }
        }

        protected override void WriteDelta(BinaryWriter writer)
        {

            int size = base.Value.getTexture().Width * base.Value.getTexture().Height * 4;
            writer.Write(base.Value.getTexture().Width);
            writer.Write(base.Value.getTexture().Height);
            //writer.Write(size);
            texture = Value.getTexture();
            Byte[] colorsOne = new byte[size]; //The hard to read,1D array
            texture.GetData(colorsOne);
            writer.Write(colorsOne);
            writer.Write(base.Value.Name);
            writer.Write(base.Value.path);
        }


    }
}
