using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netcode;
namespace ModdedUtilitiesNetworking.Framework.Extentions
{
    public static class BinaryReadWriteExtentions
    {       
      public static Vector3 ReadVector3(this BinaryReader reader)
      {
            float x=reader.ReadSingle();
            float y=reader.ReadSingle();
            float z=reader.ReadSingle();
            return new Vector3(x, y, z);
      }
        
       public static string ReadString(this BinaryReader reader)
        {
            String s= reader.ReadString();
            return new string(s.ToCharArray());
        }

        public static void WriteString(this BinaryWriter writer, object str)
        {
            writer.WriteString((string)str);
        }

        /// <summary>
        /// Read the custom info packet sent from a modded client or server.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static object[] ReadModdedInfoPacket(this BinaryReader reader)
        {
            object[] o = new object[2]
            {
                reader.ReadString(),
                reader.ReadString()
            };
            return o;
        }

        /// <summary>
        /// Read the remaining byte data in an array.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            using (var memoryStream = new MemoryStream())
            {
                reader.BaseStream.CopyTo(memoryStream);
                
                return memoryStream.ToArray();
            }
        }

        //Can do custom classes here for reading and writing.
        //That way it will be better to save/load data
    }
}
