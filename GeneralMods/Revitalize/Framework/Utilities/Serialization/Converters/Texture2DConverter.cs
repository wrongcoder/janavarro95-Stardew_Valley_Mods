using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewValley;

namespace Revitalize.Framework.Utilities.Serialization.Converters
{
    public class Texture2DConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(" ");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {

            return new Texture2D(Game1.graphics.GraphicsDevice,16,16);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Texture2D);
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;
    }
}
