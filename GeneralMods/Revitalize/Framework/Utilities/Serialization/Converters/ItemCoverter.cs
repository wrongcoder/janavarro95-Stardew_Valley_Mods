using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using StardewValley;

namespace Revitalize.Framework.Utilities.Serialization.Converters
{
    public class ItemCoverter:Newtonsoft.Json.JsonConverter
    {
        public static Dictionary<string, Type> AllTypes = new Dictionary<string, Type>();

        JsonSerializerSettings settings;
        public ItemCoverter()
        {
            this.settings = new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new Framework.Utilities.Serialization.Converters.RectangleConverter(),
                    new Framework.Utilities.Serialization.Converters.Texture2DConverter(),
                    new Vector2Converter()
                },
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Include
            };
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string convertedString = JsonConvert.SerializeObject((Item)value, this.settings);
            DefaultContractResolver resolver = serializer.ContractResolver as DefaultContractResolver;
            writer.WriteStartObject();
            writer.WritePropertyName("Type");
            serializer.Serialize(writer, value.GetType().FullName.ToString());

            List<PropertyInfo> properties=value.GetType().GetProperties().ToList();
            List<FieldInfo> fields=value.GetType().GetFields().ToList();

            writer.WritePropertyName("Item");
            writer.WriteStartObject();

            for(int i = 0; i < properties.Count; i++) {
                PropertyInfo p = properties[i];
                writer.WritePropertyName(p.Name);
                serializer.Serialize(writer, p.GetValue(value)!=null? p.GetValue(value).ToString():null);
            }
            

            foreach (FieldInfo f in fields)
            {
                writer.WritePropertyName(f.Name);
                serializer.Serialize(writer, f.GetValue(value) != null ? f.GetValue(value).ToString() : null);
            }
            writer.WriteEndObject();

            //writer.WritePropertyName("Item");

            //serializer.Serialize(writer, convertedString);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {

                throw new JsonSerializationException("Cant convert to item!");
                return null;
            }

            JObject jo = JObject.Load(reader);

            string t = jo["Type"].Value<string>();

            //See if the type has already been cached and if so return it for deserialization.
            if (AllTypes.ContainsKey(t))
            {

                return JsonConvert.DeserializeObject(jo["Item"].ToString(), AllTypes[t], this.settings);
            }

            Assembly asm = typeof(StardewValley.Object).Assembly;
            Type type = null;
            
            type = asm.GetType(t);

            //Check if the type exists in the SDV assembly. If not then try to load it from revitalize.
            if (type == null)
            {
                asm = typeof(Revitalize.ModCore).Assembly;
                type = asm.GetType(t);
            }

            //If the type doesn't exist from revitalize look through ALL loded assemblies and try to load it.
            if (type == null)
            {
                foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    asm = assembly;
                    type = asm.GetType(t);
                    if (t != null) break;
                }
            }

            if (type == null)
            {
                throw new Exception("Unsupported type found when Deserializing Unsure what to do so we can't deserialize this thing!: " + t);
            }

            //Cache the newly found type.
            AllTypes.Add(t, type);

            return JsonConvert.DeserializeObject(jo["Item"].ToString(),type, this.settings);
            /*
            if (t== typeof(StardewValley.Tools.Axe).FullName.ToString())
            {
                Revitalize.ModCore.log("DESERIALIZE AXE!!!");
                //return jo["Item"].Value<StardewValley.Tools.Axe>();
                return JsonConvert.DeserializeObject<StardewValley.Tools.Axe>(jo["Item"].ToString(),this.settings);
            }
            else if (t == typeof(Revitalize.Framework.Objects.MultiTiledObject).FullName.ToString())
            {
                
                Revitalize.ModCore.log("DESERIALIZE Multi Tile Object!!!");
                return JsonConvert.DeserializeObject<Revitalize.Framework.Objects.MultiTiledObject>(jo["Item"].ToString(), this.settings);
                // return jo["Item"].Value<Revitalize.Framework.Objects.MultiTiledObject>();

            }
            else if (t == typeof(Revitalize.Framework.Objects.MultiTiledComponent).FullName.ToString())
            {

                Revitalize.ModCore.log("DESERIALIZE Multi Tile Component!!!");
                return JsonConvert.DeserializeObject<Revitalize.Framework.Objects.MultiTiledComponent>(jo["Item"].ToString(), this.settings);
                // return jo["Item"].Value<Revitalize.Framework.Objects.MultiTiledObject>();

            }

            else
            {
               
                throw new NotImplementedException("CANT DESERIALIZE: " + t.ToString());
            }
            */


        }

        public override bool CanWrite => true;
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return this.IsSameOrSubclass(typeof(StardewValley.Item),objectType);
        }

        /// <summary>
        /// https://stackoverflow.com/questions/2742276/how-do-i-check-if-a-type-is-a-subtype-or-the-type-of-an-object
        /// </summary>
        /// <param name="potentialBase"></param>
        /// <param name="potentialDescendant"></param>
        /// <returns></returns>
        public bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }
    }
}
