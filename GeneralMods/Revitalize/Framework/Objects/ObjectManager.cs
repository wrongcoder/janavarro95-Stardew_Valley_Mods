using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Objects.Furniture;
using StardewModdingAPI;
using StardewValley;

namespace Revitalize.Framework.Objects
{
    /// <summary>
    /// Deals with handling all objects for the mod.
    /// </summary>
    public class ObjectManager
    {
        /// <summary>
        /// All of the object managers id'd by a mod's or content pack's unique id.
        /// </summary>
        public static Dictionary<string, ObjectManager> ObjectPools;


        /// <summary>
        /// The name of this object manager.
        /// </summary>
        public string name;

        /// <summary>
        /// All of the chairs held by this object pool.
        /// </summary>
        public Dictionary<string, ChairMultiTiledObject> chairs;
        /// <summary>
        /// All of the tables held by this object pool.
        /// </summary>
        public Dictionary<string, TableMultiTiledObject> tables;
        /// <summary>
        /// All of the lamps held by this object pool.
        /// </summary>
        public Dictionary<string, LampMultiTiledObject> lamps;
        /// <summary>
        /// All of the rugs held by this object pool.
        /// </summary>
        public Dictionary<string, RugMultiTiledObject> rugs;
        public Dictionary<string, StorageFurnitureOBJ> furnitureStorage;

        public Dictionary<string, MultiTiledObject> generic;
        /// <summary>
        /// Misc. items for this mod.
        /// </summary>
        public Dictionary<string, MultiTiledObject> miscellaneous;

        public ResourceManager resources;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ObjectManager()
        {
            this.initialize();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="manifest"></param>
        public ObjectManager(IManifest manifest)
        {
            this.name = manifest.UniqueID;
            this.initialize();
        }

        /// <summary>
        /// Initialize all objects used to manage this class.
        /// </summary>
        private void initialize()
        {
            this.chairs = new Dictionary<string, ChairMultiTiledObject>();
            this.tables = new Dictionary<string, TableMultiTiledObject>();
            this.lamps = new Dictionary<string, LampMultiTiledObject>();
            this.rugs = new Dictionary<string, RugMultiTiledObject>();
            this.furnitureStorage = new Dictionary<string, StorageFurnitureOBJ>();

            this.generic = new Dictionary<string, MultiTiledObject>();
            this.miscellaneous = new Dictionary<string, MultiTiledObject>();

            this.resources = new ResourceManager();
        }

        /// <summary>
        /// Gets a random object from the dictionary passed in.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public Item getRandomObject(Dictionary<string,CustomObject> dictionary)
        {
            if (dictionary.Count == 0) return null;
            List<CustomObject> objs = new List<CustomObject>();
            foreach(KeyValuePair<string,CustomObject> pair in dictionary)
            {
                objs.Add(pair.Value);
            }
            int rand = Game1.random.Next(0,objs.Count);
            return objs[rand].getOne();
        }

        
        /// <summary>
        /// Gets an object from the dictionary that is passed in.
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public Item getObject(string objectName, Dictionary<string,CustomObject> dictionary)
        {
            if (dictionary.ContainsKey(objectName))
            {
                return dictionary[objectName].getOne();
            }
            else
            {
                throw new Exception("Object pool doesn't contain said object.");
            }
        }
        

        public Item getObject(string objectName, Dictionary<string, MultiTiledObject> dictionary)
        {
            if (dictionary.ContainsKey(objectName))
            {
                return dictionary[objectName].getOne();
            }
            else
            {
                throw new Exception("Object pool doesn't contain said object.");
            }
        }

        /// <summary>
        /// Gets a chair from the object manager.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ChairMultiTiledObject getChair(string name)
        {
            if (this.chairs.ContainsKey(name))
            {
                return (ChairMultiTiledObject)this.chairs[name].getOne();
            }
            else
            {
                throw new Exception("Object pool doesn't contain said object.");
            }
        }
        /// <summary>
        /// Gets a table from the object manager.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TableMultiTiledObject getTable(string name)
        {
            if (this.tables.ContainsKey(name))
            {
                return (TableMultiTiledObject)this.tables[name].getOne();
            }
            else
            {
                throw new Exception("Object pool doesn't contain said object.");
            }
        }

        /// <summary>
        /// Gets a lamp from the object manager.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LampMultiTiledObject getLamp(string name)
        {
            if (this.lamps.ContainsKey(name))
            {
                return (LampMultiTiledObject)this.lamps[name].getOne();
            }
            else
            {
                throw new Exception("Object pool doesn't contain said object.");
            }
        }

        /// <summary>
        /// Gets storage furniture from the object manager.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StorageFurnitureOBJ getStorageFuriture(string name)
        {
            if (this.furnitureStorage.ContainsKey(name))
            {
                return (StorageFurnitureOBJ)this.furnitureStorage[name].getOne();
            }
            else
            {
                throw new Exception("Object pool doesn't contain said object.");
            }
        }

        /// <summary>
        /// Adds a new object manager to the master pool of managers.
        /// </summary>
        /// <param name="Manifest"></param>
        public static void AddObjectManager(IManifest Manifest)
        {
            if (ObjectPools == null) ObjectPools = new Dictionary<string, ObjectManager>();
            ObjectPools.Add(Manifest.UniqueID, new ObjectManager(Manifest));
        }


        /// <summary>
        /// Cleans up all stored information.
        /// </summary>
        public void returnToTitleCleanUp()
        {

        }
        

    }
}
