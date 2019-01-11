using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Revitalize.Framework.Utilities.Serialization.ContractResolvers;

namespace Revitalize.Framework.Utilities
{
    /// <summary>
    /// Handles serialization of all objects in existence.
    ///
    /// TODO: Make JConvert that has same settings to implement a toJSon string obj
    /// </summary>
    public class Serializer
    {
        private JsonSerializer serializer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Serializer()
        {
            this.serializer = new JsonSerializer();
            this.serializer.Formatting = Formatting.Indented;
            this.serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            this.serializer.NullValueHandling = NullValueHandling.Include;

            this.serializer.ContractResolver = new NetFieldContract();

            this.addConverter(new Framework.Utilities.Serialization.Converters.RectangleConverter());
            this.addConverter(new Framework.Utilities.Serialization.Converters.Texture2DConverter());
            this.addConverter(new Framework.Utilities.Serialization.Converters.ItemCoverter());
            //this.addConverter(new Framework.Utilities.Serialization.Converters.NetFieldConverter());
            //this.addConverter(new Framework.Utilities.Serialization.Converters.Vector2Converter());
        }

        /// <summary>
        /// Adds a new converter to the json serializer.
        /// </summary>
        /// <param name="converter"></param>
        public void addConverter(JsonConverter converter)
        {
            this.serializer.Converters.Add(converter);
        }


        /// <summary>
        /// Deserializes an object from a .json file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p"></param>
        /// <returns></returns>
        public T Deserialize<T>(string p)
        {
            string json = "";
            foreach (string line in File.ReadLines(p))
            {
                json += line;
            }
            using (StreamReader sw = new StreamReader(p))
            using (JsonReader reader = new JsonTextReader(sw))
            {
                var obj = this.serializer.Deserialize<T>(reader);
                return obj;
            }
        }

        public object Deserialize(string p,Type T)
        {
            string json = "";
            foreach (string line in File.ReadLines(p))
            {
                json += line;
            }
            using (StreamReader sw = new StreamReader(p))
            using (JsonReader reader = new JsonTextReader(sw))
            {
                object obj = this.serializer.Deserialize(reader,T);
                return obj;
            }
        }

        /// <summary>
        /// Serializes an object to a .json file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="o"></param>
        public void Serialize(string path, object o)
        {
            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                this.serializer.Serialize(writer, o);
            }
        }
    }
}
