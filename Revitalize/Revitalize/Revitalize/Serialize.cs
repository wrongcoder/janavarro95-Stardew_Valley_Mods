using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Revitalize.Objects;
using Revitalize.Objects.Machines;
using Revitalize.Resources;
using Revitalize.Resources.DataNodes;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize
{
    class Serialize
    {
        public static string InvPath;

        public static string PlayerDataPath;
        public static string DataDirectoryPath;

        public static string TrackedTerrainDataPath;


        public static void createDirectories()
        {

            DataDirectoryPath = Path.Combine(Class1.path, "PlayerData");
            PlayerDataPath = Path.Combine(DataDirectoryPath, Game1.player.name);
            InvPath = Path.Combine(PlayerDataPath, "Inventory");
          

            Log.AsyncC(TrackedTerrainDataPath);

            if (!Directory.Exists(DataDirectoryPath))
            {
                Directory.CreateDirectory(DataDirectoryPath);
            }
            if (!Directory.Exists(PlayerDataPath))
            {
                Directory.CreateDirectory(PlayerDataPath);
            }
            if (!Directory.Exists(InvPath))
            {
                Directory.CreateDirectory(InvPath);
            }
        }

        public static void cleanUpInventory()
        {
           


            List<Item> removalList = new List<Item>();
            foreach (Item d in Game1.player.items)
            {
                try
                {
                    if (d == null)
                    {
                        //Log.AsyncG("WTF");
                        continue;
                    }
                    // Log.AsyncC(d.GetType());
                }
                catch (Exception e)
                {

                }
                string s = Convert.ToString((d.GetType()));




                if (Dictionaries.acceptedTypes.ContainsKey(s))
                {
                    SerializerDataNode t;

                    bool works = Dictionaries.acceptedTypes.TryGetValue(s, out t);
                    if (works == true)
                    {
                        t.serialize.Invoke(d);
                        removalList.Add(d);
                    }
                }
            }
            foreach (var i in removalList)
            {
                Game1.player.removeItemFromInventory(i);
            }
            removalList.Clear();

            Log.AsyncM("Done cleaning inventory!");
        }

        public static void restoreInventory()
        {
          
            //   Log.AsyncG(InvPath);
            ProcessDirectoryForCleanUp(InvPath);
        }



        public static void ProcessDirectoryForCleanUp(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                ProcessFileForCleanUp(fileName);
                File.Delete(fileName);
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectoryForCleanUp(subdirectory);

        }

        // Insert logic for processing found files here.
        public static void ProcessFileForCleanUp(string path)
        {
            string[] ehh = File.ReadAllLines(path);
            string data = ehh[0];

            Log.AsyncC(data);
            dynamic obj = JObject.Parse(data);


            //   Log.AsyncC(obj.thisType);

            string a = obj.thisType;
            string[] b = a.Split(',');
            string s = b.ElementAt(0);
            //   Log.AsyncC(s);

            if (Dictionaries.acceptedTypes.ContainsKey(s))
            {
                //  Log.AsyncC("FUUUUU");
                foreach (KeyValuePair<string, SerializerDataNode> pair in Dictionaries.acceptedTypes)
                {
                    if (pair.Key == s)
                    {
                        var cObj = pair.Value.parse.Invoke(data);
                        Log.AsyncC("NEED TO HANDLE PUTTING OBJECTS BACK INTO A LOCATION!!!!");
                        if (cObj.thisLocation == null)
                        {
                            Game1.player.addItemToInventory(cObj);
                            return;
                        }
                    }
                }
            }




        }


        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                //settings.TypeNameHandling = TypeNameHandling.Auto;
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
              //  settings.Formatting = Formatting.Indented;
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite, settings);
                int i = 0;
                string s = filePath;
                while (File.Exists(s) == true)
                {
                    s = filePath;
                    s = (s + Convert.ToString(i));
                    i++;
                }
                filePath = s;

                 writer = new StreamWriter(filePath, append);
               
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();

                JsonSerializerSettings settings = new JsonSerializerSettings();
               // settings.TypeNameHandling = TypeNameHandling.Auto;
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //settings.Formatting = Formatting.Indented;
                return JsonConvert.DeserializeObject<T>(fileContents,settings);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }







        public static Item parseItemFromJson(string data)
        {

            dynamic obj = JObject.Parse(data);            
            string a = obj.thisType;
            string[] b = a.Split(',');
            string s = b.ElementAt(0);

            if (Dictionaries.acceptedTypes.ContainsKey(s))
            {
                foreach (KeyValuePair<string, SerializerDataNode> pair in Dictionaries.acceptedTypes)
                {
                    if (pair.Key == s)
                    {
                     var   cObj = pair.Value.parse.Invoke(data);
                        return cObj;
                    }
                }
            }

            return null;
           // return cObj;
            
        }

        public static Decoration parseDecoration(string data)
        {
            
            dynamic obj = JObject.Parse(data);


          //  Log.AsyncC(obj.thisType);
            

            Decoration d = new Decoration(false);

            d.price = obj.price;
            d.Decoration_type = obj.Decoration_type;
            d.rotations = obj.rotations;
            d.currentRotation = obj.currentRotation;
            string s1 = Convert.ToString(obj.sourceRect);
            d.sourceRect=Util.parseRectFromJson(s1);
            string s2 = Convert.ToString(obj.defaultSourceRect);
            d.defaultSourceRect = Util.parseRectFromJson(s2);
            string s3 = Convert.ToString(obj.defaultBoundingBox);
            d.defaultBoundingBox = Util.parseRectFromJson(s3);
            d.description = obj.description;
            d.flipped = obj.flipped;
            d.flaggedForPickUp = obj.flaggedForPickUp;
            d.tileLocation = obj.tileLocation;
            d.parentSheetIndex = obj.parentSheetIndex;
            d.owner = obj.owner;
            d.name = obj.name;
            d.type = obj.type;
            d.canBeSetDown = obj.canBeSetDown;
            d.canBeGrabbed = obj.canBeGrabbed;
            d.isHoedirt = obj.isHoedirt;
            d.isSpawnedObject = obj.isSpawnedObject;
            d.questItem = obj.questItem;
            d.isOn = obj.isOn;
            d.fragility = obj.fragility;
            d.edibility = obj.edibility;
            d.stack = obj.stack;
            d.quality = obj.quality;
            d.bigCraftable = obj.bigCraftable;
            d.setOutdoors = obj.setOutdoors;
            d.setIndoors = obj.setIndoors;
            d.readyForHarvest = obj.readyForHarvest;
            d.showNextIndex = obj.showNextIndex;
            d.hasBeenPickedUpByFarmer = obj.hasBeenPickedUpByFarmer;
            d.isRecipe = obj.isRecipe;
            d.isLamp = obj.isLamp;
            d.heldObject = obj.heldObject;
            d.minutesUntilReady = obj.minutesUntilReady;
            string s4 = Convert.ToString(obj.boundingBox);
            d.boundingBox = Util.parseRectFromJson(s4);
            d.scale = obj.scale;
            d.lightSource = obj.lightSource;
            d.shakeTimer = obj.shakeTimer;
            d.internalSound = obj.internalSound;
            d.specialVariable = obj.specialVariable;
            d.category = obj.category;
            d.specialItem = obj.specialItem;
            d.hasBeenInInventory = obj.hasBeenInInventory;
            string t = obj.texturePath;
            d.TextureSheet = Game1.content.Load<Texture2D>(t);
            d.texturePath = t;


            JArray array = obj.inventory;
            d.inventory = array.ToObject<List<Item>>();


           d.inventoryMaxSize = obj.inventoryMaxSize;
            d.itemReadyForHarvest = obj.itemReadyForHarvest;
            d.lightsOn = obj.lightsOn;
            d.thisLocation = obj.thisLocation;
            d.lightColor = obj.lightColor;
            d.thisType = obj.thisType;
            d.removable = obj.removable;

            try {
                return d;
            }
            catch(Exception e)
            {
                Log.AsyncM(e);
                return null;
            }
            



        }
        public static void serializeDecoration(Item d)
        {
            Serialize.WriteToJsonFile(Path.Combine(InvPath, d.Name + ".json"), (Decoration)d);
        }


        public static Spawner parseSpawner(string data)
        {

            dynamic obj = JObject.Parse(data);


            //  Log.AsyncC(obj.thisType);


            Spawner d = new Spawner(false);

            d.price = obj.price;
            d.Decoration_type = obj.Decoration_type;
            d.rotations = obj.rotations;
            d.currentRotation = obj.currentRotation;
            string s1 = Convert.ToString(obj.sourceRect);
            d.sourceRect = Util.parseRectFromJson(s1);
            string s2 = Convert.ToString(obj.defaultSourceRect);
            d.defaultSourceRect = Util.parseRectFromJson(s2);
            string s3 = Convert.ToString(obj.defaultBoundingBox);
            d.defaultBoundingBox = Util.parseRectFromJson(s3);
            d.description = obj.description;
            d.flipped = obj.flipped;
            d.flaggedForPickUp = obj.flaggedForPickUp;
            d.tileLocation = obj.tileLocation;
            d.parentSheetIndex = obj.parentSheetIndex;
            d.owner = obj.owner;
            d.name = obj.name;
            d.type = obj.type;
            d.canBeSetDown = obj.canBeSetDown;
            d.canBeGrabbed = obj.canBeGrabbed;
            d.isHoedirt = obj.isHoedirt;
            d.isSpawnedObject = obj.isSpawnedObject;
            d.questItem = obj.questItem;
            d.isOn = obj.isOn;
            d.fragility = obj.fragility;
            d.edibility = obj.edibility;
            d.stack = obj.stack;
            d.quality = obj.quality;
            d.bigCraftable = obj.bigCraftable;
            d.setOutdoors = obj.setOutdoors;
            d.setIndoors = obj.setIndoors;
            d.readyForHarvest = obj.readyForHarvest;
            d.showNextIndex = obj.showNextIndex;
            d.hasBeenPickedUpByFarmer = obj.hasBeenPickedUpByFarmer;
            d.isRecipe = obj.isRecipe;
            d.isLamp = obj.isLamp;
            d.heldObject = obj.heldObject;
            d.minutesUntilReady = obj.minutesUntilReady;
            string s4 = Convert.ToString(obj.boundingBox);
            d.boundingBox = Util.parseRectFromJson(s4);
            d.scale = obj.scale;
            d.lightSource = obj.lightSource;
            d.shakeTimer = obj.shakeTimer;
            d.internalSound = obj.internalSound;
            d.specialVariable = obj.specialVariable;
            d.category = obj.category;
            d.specialItem = obj.specialItem;
            d.hasBeenInInventory = obj.hasBeenInInventory;
            string t = obj.texturePath;
            d.TextureSheet = Game1.content.Load<Texture2D>(t);
            d.texturePath = t;


            JArray array = obj.inventory;
            d.inventory = array.ToObject<List<Item>>();


            d.inventoryMaxSize = obj.inventoryMaxSize;
            d.itemReadyForHarvest = obj.itemReadyForHarvest;
            d.lightsOn = obj.lightsOn;
            d.thisLocation = obj.thisLocation;
            d.lightColor = obj.lightColor;
            d.thisType = obj.thisType;
            d.removable = obj.removable;

            try
            {
                return d;
            }
            catch (Exception e)
            {
                Log.AsyncM(e);
                return null;
            }




        }
        public static void serializeSpawner(Item d)
        {
            Serialize.WriteToJsonFile(Path.Combine(InvPath, d.Name + ".json"), (Spawner)d);
        }


        public static GiftPackage parseGiftPackage(string data)
        {

            dynamic obj = JObject.Parse(data);


            //  Log.AsyncC(obj.thisType);


            GiftPackage d = new GiftPackage(false);

            d.price = obj.price;
            d.Decoration_type = obj.Decoration_type;
            d.rotations = obj.rotations;
            d.currentRotation = obj.currentRotation;
            string s1 = Convert.ToString(obj.sourceRect);
            d.sourceRect = Util.parseRectFromJson(s1);
            string s2 = Convert.ToString(obj.defaultSourceRect);
            d.defaultSourceRect = Util.parseRectFromJson(s2);
            string s3 = Convert.ToString(obj.defaultBoundingBox);
            d.defaultBoundingBox = Util.parseRectFromJson(s3);
            d.description = obj.description;
            d.flipped = obj.flipped;
            d.flaggedForPickUp = obj.flaggedForPickUp;
            d.tileLocation = obj.tileLocation;
            d.parentSheetIndex = obj.parentSheetIndex;
            d.owner = obj.owner;
            d.name = obj.name;
            d.type = obj.type;
            d.canBeSetDown = obj.canBeSetDown;
            d.canBeGrabbed = obj.canBeGrabbed;
            d.isHoedirt = obj.isHoedirt;
            d.isSpawnedObject = obj.isSpawnedObject;
            d.questItem = obj.questItem;
            d.isOn = obj.isOn;
            d.fragility = obj.fragility;
            d.edibility = obj.edibility;
            d.stack = obj.stack;
            d.quality = obj.quality;
            d.bigCraftable = obj.bigCraftable;
            d.setOutdoors = obj.setOutdoors;
            d.setIndoors = obj.setIndoors;
            d.readyForHarvest = obj.readyForHarvest;
            d.showNextIndex = obj.showNextIndex;
            d.hasBeenPickedUpByFarmer = obj.hasBeenPickedUpByFarmer;
            d.isRecipe = obj.isRecipe;
            d.isLamp = obj.isLamp;
            d.heldObject = obj.heldObject;
            d.minutesUntilReady = obj.minutesUntilReady;
            string s4 = Convert.ToString(obj.boundingBox);
            d.boundingBox = Util.parseRectFromJson(s4);
            d.scale = obj.scale;
            d.lightSource = obj.lightSource;
            d.shakeTimer = obj.shakeTimer;
            d.internalSound = obj.internalSound;
            d.specialVariable = obj.specialVariable;
            d.category = obj.category;
            d.specialItem = obj.specialItem;
            d.hasBeenInInventory = obj.hasBeenInInventory;
            string t = obj.texturePath;
            d.TextureSheet = Game1.content.Load<Texture2D>(t);
            d.texturePath = t;


            JArray array = obj.inventory;
            d.inventory = array.ToObject<List<Item>>();


            d.inventoryMaxSize = obj.inventoryMaxSize;
            d.itemReadyForHarvest = obj.itemReadyForHarvest;
            d.lightsOn = obj.lightsOn;
            d.thisLocation = obj.thisLocation;
            d.lightColor = obj.lightColor;
            d.thisType = obj.thisType;
            d.removable = obj.removable;

            try
            {
                return d;
            }
            catch (Exception e)
            {
                Log.AsyncM(e);
                return null;
            }




        }
        public static void serializeGiftPackage(Item d)
        {
            Serialize.WriteToJsonFile(Path.Combine(InvPath, d.Name + ".json"), (GiftPackage)d);
        }



        public static Light parseLight(string data)
        {

            dynamic obj = JObject.Parse(data);


          //  Log.AsyncC(obj.thisType);


            Light d = new Light(false);

            d.price = obj.price;
            d.Decoration_type = obj.Decoration_type;
            d.rotations = obj.rotations;
            d.currentRotation = obj.currentRotation;
            string s1 = Convert.ToString(obj.sourceRect);
            d.sourceRect = Util.parseRectFromJson(s1);
            string s2 = Convert.ToString(obj.defaultSourceRect);
            d.defaultSourceRect = Util.parseRectFromJson(s2);
            string s3 = Convert.ToString(obj.defaultBoundingBox);
            d.defaultBoundingBox = Util.parseRectFromJson(s3);
            d.description = obj.description;
            d.flipped = obj.flipped;
            d.flaggedForPickUp = obj.flaggedForPickUp;
            d.tileLocation = obj.tileLocation;
            d.parentSheetIndex = obj.parentSheetIndex;
            d.owner = obj.owner;
            d.name = obj.name;
            d.type = obj.type;
            d.canBeSetDown = obj.canBeSetDown;
            d.canBeGrabbed = obj.canBeGrabbed;
            d.isHoedirt = obj.isHoedirt;
            d.isSpawnedObject = obj.isSpawnedObject;
            d.questItem = obj.questItem;
            d.isOn = obj.isOn;
            d.fragility = obj.fragility;
            d.edibility = obj.edibility;
            d.stack = obj.stack;
            d.quality = obj.quality;
            d.bigCraftable = obj.bigCraftable;
            d.setOutdoors = obj.setOutdoors;
            d.setIndoors = obj.setIndoors;
            d.readyForHarvest = obj.readyForHarvest;
            d.showNextIndex = obj.showNextIndex;
            d.hasBeenPickedUpByFarmer = obj.hasBeenPickedUpByFarmer;
            d.isRecipe = obj.isRecipe;
            d.isLamp = obj.isLamp;
            d.heldObject = obj.heldObject;
            d.minutesUntilReady = obj.minutesUntilReady;
            string s4 = Convert.ToString(obj.boundingBox);
            d.boundingBox = Util.parseRectFromJson(s4);
            d.scale = obj.scale;
            d.lightSource = obj.lightSource;
            d.shakeTimer = obj.shakeTimer;
            d.internalSound = obj.internalSound;
            d.specialVariable = obj.specialVariable;
            d.category = obj.category;
            d.specialItem = obj.specialItem;
            d.hasBeenInInventory = obj.hasBeenInInventory;
            string t = obj.texturePath;

          //  Log.AsyncC(t);

            d.TextureSheet = Game1.content.Load<Texture2D>(t);
            d.texturePath = t;
            JArray array = obj.inventory;
            d.inventory = array.ToObject<List<Item>>();
            d.inventoryMaxSize = obj.inventoryMaxSize;
            d.itemReadyForHarvest = obj.itemReadyForHarvest;
            d.lightsOn = obj.lightsOn;
            d.thisLocation = obj.thisLocation;
            d.lightColor = obj.lightColor;
            d.thisType = obj.thisType;
            d.removable = obj.removable;

            try
            {
                return d;
            }
            catch (Exception e)
            {
                Log.AsyncM(e);
                return null;
            }




        }
        public static void serializeLight(Item d)
        {
            Serialize.WriteToJsonFile(Path.Combine(InvPath, d.Name + ".json"), (Light)d);
        }

        public static Quarry parseQuarry(string data)
        {

            dynamic obj = JObject.Parse(data);


            //  Log.AsyncC(obj.thisType);


            Quarry d = new Quarry(false);

            d.price = obj.price;
            d.Decoration_type = obj.Decoration_type;
            d.rotations = obj.rotations;
            d.currentRotation = obj.currentRotation;
            string s1 = Convert.ToString(obj.sourceRect);
            d.sourceRect = Util.parseRectFromJson(s1);
            string s2 = Convert.ToString(obj.defaultSourceRect);
            d.defaultSourceRect = Util.parseRectFromJson(s2);
            string s3 = Convert.ToString(obj.defaultBoundingBox);
            d.defaultBoundingBox = Util.parseRectFromJson(s3);
            d.description = obj.description;
            d.flipped = obj.flipped;
            d.flaggedForPickUp = obj.flaggedForPickUp;
            d.tileLocation = obj.tileLocation;
            d.parentSheetIndex = obj.parentSheetIndex;
            d.owner = obj.owner;
            d.name = obj.name;
            d.type = obj.type;
            d.canBeSetDown = obj.canBeSetDown;
            d.canBeGrabbed = obj.canBeGrabbed;
            d.isHoedirt = obj.isHoedirt;
            d.isSpawnedObject = obj.isSpawnedObject;
            d.questItem = obj.questItem;
            d.isOn = obj.isOn;
            d.fragility = obj.fragility;
            d.edibility = obj.edibility;
            d.stack = obj.stack;
            d.quality = obj.quality;
            d.bigCraftable = obj.bigCraftable;
            d.setOutdoors = obj.setOutdoors;
            d.setIndoors = obj.setIndoors;
            d.readyForHarvest = obj.readyForHarvest;
            d.showNextIndex = obj.showNextIndex;
            d.hasBeenPickedUpByFarmer = obj.hasBeenPickedUpByFarmer;
            d.isRecipe = obj.isRecipe;
            d.isLamp = obj.isLamp;
            d.heldObject = obj.heldObject;
            d.minutesUntilReady = obj.minutesUntilReady;
            string s4 = Convert.ToString(obj.boundingBox);
            d.boundingBox = Util.parseRectFromJson(s4);
            d.scale = obj.scale;
            d.lightSource = obj.lightSource;
            d.shakeTimer = obj.shakeTimer;
            d.internalSound = obj.internalSound;
            d.specialVariable = obj.specialVariable;
            d.category = obj.category;
            d.specialItem = obj.specialItem;
            d.hasBeenInInventory = obj.hasBeenInInventory;
            string t = obj.texturePath;

            //  Log.AsyncC(t);

            d.TextureSheet = Game1.content.Load<Texture2D>(t);
            d.texturePath = t;
            JArray array = obj.inventory;
            d.inventory = array.ToObject<List<Item>>();
            d.inventoryMaxSize = obj.inventoryMaxSize;
            d.itemReadyForHarvest = obj.itemReadyForHarvest;
            d.lightsOn = obj.lightsOn;
            d.thisLocation = obj.thisLocation;
            d.lightColor = obj.lightColor;
            d.thisType = obj.thisType;
            d.removable = obj.removable;

            d.ResourceName = obj.ResourceName;
            d.dataNode = obj.dataNode;
            try
            {
                return d;
            }
            catch (Exception e)
            {
                Log.AsyncM(e);
                return null;
            }




        }
        public static void serializeQuarry(Item d)
        {
            Serialize.WriteToJsonFile(Path.Combine(InvPath, d.Name + ".json"), (Quarry)d);
        }

        public static shopObject parseShopObject(string data)
        {

            dynamic obj = JObject.Parse(data);


         //   Log.AsyncC(obj.thisType);


            shopObject d = new shopObject(false);

            d.price = obj.price;
            d.Decoration_type = obj.Decoration_type;
            d.rotations = obj.rotations;
            d.currentRotation = obj.currentRotation;
            string s1 = Convert.ToString(obj.sourceRect);
            d.sourceRect = Util.parseRectFromJson(s1);
            string s2 = Convert.ToString(obj.defaultSourceRect);
            d.defaultSourceRect = Util.parseRectFromJson(s2);
            string s3 = Convert.ToString(obj.defaultBoundingBox);
            d.defaultBoundingBox = Util.parseRectFromJson(s3);
            d.description = obj.description;
            d.flipped = obj.flipped;
            d.flaggedForPickUp = obj.flaggedForPickUp;
            d.tileLocation = obj.tileLocation;
            d.parentSheetIndex = obj.parentSheetIndex;
            d.owner = obj.owner;
            d.name = obj.name;
            d.type = obj.type;
            d.canBeSetDown = obj.canBeSetDown;
            d.canBeGrabbed = obj.canBeGrabbed;
            d.isHoedirt = obj.isHoedirt;
            d.isSpawnedObject = obj.isSpawnedObject;
            d.questItem = obj.questItem;
            d.isOn = obj.isOn;
            d.fragility = obj.fragility;
            d.edibility = obj.edibility;
            d.stack = obj.stack;
            d.quality = obj.quality;
            d.bigCraftable = obj.bigCraftable;
            d.setOutdoors = obj.setOutdoors;
            d.setIndoors = obj.setIndoors;
            d.readyForHarvest = obj.readyForHarvest;
            d.showNextIndex = obj.showNextIndex;
            d.hasBeenPickedUpByFarmer = obj.hasBeenPickedUpByFarmer;
            d.isRecipe = obj.isRecipe;
            d.isLamp = obj.isLamp;
            d.heldObject = obj.heldObject;
            d.minutesUntilReady = obj.minutesUntilReady;
            string s4 = Convert.ToString(obj.boundingBox);
            d.boundingBox = Util.parseRectFromJson(s4);
            d.scale = obj.scale;
            d.lightSource = obj.lightSource;
            d.shakeTimer = obj.shakeTimer;
            d.internalSound = obj.internalSound;
            d.specialVariable = obj.specialVariable;
            d.category = obj.category;
            d.specialItem = obj.specialItem;
            d.hasBeenInInventory = obj.hasBeenInInventory;
            string t = obj.texturePath;
            d.TextureSheet = Game1.content.Load<Texture2D>(t);
            d.texturePath = t;
            JArray array = obj.inventory;
            //  d.inventory = array.ToObject<List<Item>>();
            d.inventory = parseInventoryList(array);


          //  Log.AsyncC(array);

            d.inventoryMaxSize = obj.inventoryMaxSize;
            d.itemReadyForHarvest = obj.itemReadyForHarvest;
            d.lightsOn = obj.lightsOn;
            d.thisLocation = obj.thisLocation;
            d.lightColor = obj.lightColor;
            d.thisType = obj.thisType;
            d.removable = obj.removable;

            try
            {
                return d;
            }
            catch (Exception e)
            {
                Log.AsyncM(e);
                return null;
            }




        }
        public static void serializeShopObject(Item d)
        {
            Serialize.WriteToJsonFile(Path.Combine(InvPath, d.Name + ".json"), (shopObject)d);
        }

        public static void parseTrackedTerrainDataNodeList(string data)
        {
            if (File.Exists(data))
            {
                 Lists.trackedTerrainFeaturesDummyList = ReadFromJsonFile<List<TrackedTerrainDummyDataNode>>(data);
                foreach(var v in Lists.trackedTerrainFeaturesDummyList)
                {
                    GameLocation location = Game1.getLocationFromName(v.location);
                    Vector2 position = v.position;
                   
                    TerrainFeature t;
                     bool ehh = location.terrainFeatures.TryGetValue(position, out t);

                    if (t == null)
                    {
                        Log.AsyncC("BOOOOO");
                    }

                    Lists.trackedTerrainFeatures.Add(new TrackedTerrainDataNode(location, (HoeDirt)t, position));
                   // Log.AsyncG("YAY");
                }
                Lists.trackedTerrainFeaturesDummyList.Clear();
            }
        }
        public static void serializeTrackedTerrainDataNodeList(List<TrackedTerrainDataNode> list)
        {
            Lists.trackedTerrainFeaturesDummyList.Clear();
            foreach(var v in list)
            {
                Lists.trackedTerrainFeaturesDummyList.Add(new TrackedTerrainDummyDataNode(v.location.name, v.position));
            }


            if (File.Exists(Path.Combine(PlayerDataPath, "TrackedTerrainFeaturesList.json")))
            {
                File.Delete(Path.Combine(PlayerDataPath, "TrackedTerrainFeaturesList.json"));
            }
            if (Lists.trackedTerrainFeaturesDummyList.Count == 0)
            {
                File.Delete(Path.Combine(PlayerDataPath, "TrackedTerrainFeaturesList.json"));
                return;
            }

            WriteToJsonFile<List<TrackedTerrainDummyDataNode>>(Path.Combine(PlayerDataPath, "TrackedTerrainFeaturesList.json"), Lists.trackedTerrainFeaturesDummyList);
        }



        public static List<Item> parseInventoryList(JArray array)
        {

            if (array == null)
            {
               // Log.AsyncC("WTF");
                return new List<Item>();

            }

            List<Item> inventory = new List<Item>();

            foreach(var v in array)
            {
                string data = v.ToString();
                if (data != null)
                {
                  //  Log.AsyncM(data);
                    inventory.Add(parseItemFromJson(data));
                }
            }



            return inventory;

        }
    }
}
