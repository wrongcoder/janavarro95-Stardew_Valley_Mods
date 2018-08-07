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



        public NetTexture2DExtended()
        {

        }

        public NetTexture2DExtended(Texture2DExtended value) : base(value)
        {
        }

        public void ReadData(BinaryReader reader, NetVersion version)
        {
            ReadDelta(reader, version);
        }

        public void WriteData(BinaryWriter writer)
        {
            WriteDelta(writer);
        }

        protected override void ReadDelta(BinaryReader reader, NetVersion version)
        {

            NetInt Width = new NetInt();
            Width.Read(reader,version);
            int width = Width.Value;

            NetInt Height = new NetInt();
            Height.Read(reader, version);
            int height = Height.Value;


            NetString name = new NetString();
            name.Read(reader, version);
            NetString path = new NetString();
            path.Read(reader, version);


            NetInt count = new NetInt();
            count.Read(reader, version);

            List<Color> bytes = new List<Color>();
            //colorsOne = reader.ReadBytes();
            
            for(int i=0; i < count.Value; i++)
            {
                NetColor col = new NetColor();
                col.Read(reader, version);
                bytes.Add(col.Value);
            }

            ModCore.ModMonitor.Log("Finished length: "+bytes.Count.ToString());
            ModCore.ModMonitor.Log("W: " + width.ToString());
            ModCore.ModMonitor.Log("H: " + height.ToString());


            //Texture2D texture = new Texture2D(Game1.graphics.GraphicsDevice,width,height);
            Texture2DExtended texture = new Texture2DExtended(ModCore.ModHelper, Path.Combine("Content", "Graphics", "MultiTest", "Test1.png"));
            this.Value = texture;
 
            //texture.SetData(bytes.ToArray());  

            //Value.Name = name.Value;
            //Value.path = path.Value;
            //Value.setTexure(texture);
            
        }

        protected override void WriteDelta(BinaryWriter writer)
        {

            int size = Value.getTexture().Width * Value.getTexture().Height;
            NetInt Width = new NetInt(Value.getTexture().Width);
            Width.Write(writer);

            NetInt Height = new NetInt(Value.getTexture().Height);
            Height.Write(writer);


            NetString name = new NetString(Value.Name);
            name.Write(writer);

            NetString path = new NetString(Value.path);
            path.Write(writer);
            //writer.Write(size);

            Texture2D texture = Value.getTexture();
            Color[] colorsOne = new Color[size]; //The hard to read,1D array
            
            texture.GetData(colorsOne);

            NetInt count = new NetInt(colorsOne.Length);
            count.Write(writer);

            ModCore.ModMonitor.Log("Color length:" + count.ToString());

            foreach(var v in colorsOne)
            {
                NetColor col = new NetColor(v);
                col.Write(writer);
            }

            
        }


    }
}
