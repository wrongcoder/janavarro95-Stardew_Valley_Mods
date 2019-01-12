using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Revitalize.Framework.Utilities.Serialization.ContractResolvers;
using StardewValley;

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
        /// All files to be cleaned up after loading.
        /// </summary>
        private Dictionary<string, List<string>> filesToDelete = new Dictionary<string, List<string>>();




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

            gatherAllFilesForCleanup();

        }

        /// <summary>
        /// Process all the save data for objects to be deleted by this mod.
        /// </summary>
        private void gatherAllFilesForCleanup()
        {

            string[] directories = Directory.GetDirectories(Path.Combine(Revitalize.ModCore.ModHelper.DirectoryPath, "SaveData"));
            foreach (string playerData in directories)
            {
                string objectPath = Path.Combine(playerData, "SavedObjectInformation");
                string[] objectFiles = Directory.GetFiles(objectPath);
                foreach (string file in objectFiles)
                {
                    string playerName = new DirectoryInfo(objectPath).Parent.Name;
                    if (this.filesToDelete.ContainsKey(playerName)){
                        this.filesToDelete[playerName].Add(file);
                        //Revitalize.ModCore.log("Added File: " + file);
                    }
                    else
                    {
                        this.filesToDelete.Add(playerName, new List<string>());
                        //Revitalize.ModCore.log("Added Player Key: " + playerName);
                        this.filesToDelete[playerName].Add(file);
                        //Revitalize.ModCore.log("Added File: " + file);
                    }
                    
                }
            }
        }

        /// <summary>
        /// Called after load to deal with internal file cleanUp
        /// </summary>
        public void afterLoad()
        {
            deleteAllUnusedFiles();
        }

        /// <summary>
        /// Removes the file from all files that will be deleted.
        /// </summary>
        /// <param name="playerDirectory"></param>
        /// <param name="fileName"></param>
        private void removeFileFromDeletion(string playerDirectory, string fileName)
        {
            if (this.filesToDelete.ContainsKey(playerDirectory))
            {
                //Revitalize.ModCore.log("Removing from deletion: " + fileName);
                this.filesToDelete[playerDirectory].Remove(fileName);
            }
            else
            {
                //Revitalize.ModCore.log("Found key: " + playerDirectory);
                //Revitalize.ModCore.log("Found file: " + fileName);
            }
        }

        /// <summary>
        /// Deletes unused object data.
        /// </summary>
        private void deleteAllUnusedFiles()
        {
            foreach(KeyValuePair<string,List<string>> pair in this.filesToDelete)
            {
                foreach (string file in pair.Value)
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Adds a new converter to the json serializer.
        /// </summary>
        /// <param name="converter">The type of json converter to add to the Serializer.</param>
        public void addConverter(JsonConverter converter)
        {
            this.serializer.Converters.Add(converter);
        }


        /// <summary>
        /// Deserializes an object from a .json file.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize into.</typeparam>
        /// <param name="p">The path to the file.</param>
        /// <returns>An object of specified type T.</returns>
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

        /// <summary>
        /// Deserializes an object from a .json file.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize into.</typeparam>
        /// <param name="p">The path to the file.</param>
        /// <returns>An object of specified type T.</returns>
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

        /// <summary>
        /// Serialize a data structure into an file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="obj"></param>
        public void SerializeGUID(string fileName,object obj)
        {
            string path = Path.Combine(Revitalize.ModCore.ModHelper.DirectoryPath, "SaveData", Game1.player.name + "_" + Game1.player.uniqueMultiplayerID, "SavedObjectInformation", fileName + ".json");
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            Serialize(path, obj);
        }

        /// <summary>
        /// Deserialze a file into it's proper data structure.
        /// </summary>
        /// <typeparam name="T">The type of data structure to deserialze to.</typeparam>
        /// <param name="fileName">The name of the file to deserialize from.</param>
        /// <returns>A data structure object deserialize from a json string in a file.</returns>
        public object DeserializeGUID(string fileName,Type T)
        {
            string path=Path.Combine(Revitalize.ModCore.ModHelper.DirectoryPath, "SaveData", Game1.player.name + "_" + Game1.player.uniqueMultiplayerID, "SavedObjectInformation", fileName + ".json");
            removeFileFromDeletion((Game1.player.name + "_" + Game1.player.uniqueMultiplayerID), path);
            return Deserialize(path, T);
        }

        /// <summary>
        /// Deserialze a file into it's proper data structure.
        /// </summary>
        /// <typeparam name="T">The type of data structure to deserialze to.</typeparam>
        /// <param name="fileName">The name of the file to deserialize from.</param>
        /// <returns>A data structure object deserialize from a json string in a file.</returns>
        public T DeserializeGUID<T>(string fileName)
        {
            string path = Path.Combine(Revitalize.ModCore.ModHelper.DirectoryPath, "SaveData", Game1.player.name + "_" + Game1.player.uniqueMultiplayerID, "SavedObjectInformation", fileName + ".json");
            removeFileFromDeletion((Game1.player.name + "_" + Game1.player.uniqueMultiplayerID),path);
            if (File.Exists(path))
            {

                return Deserialize<T>(path);
            }
            else
            {
                return default(T);
            }
        }



    }
}
