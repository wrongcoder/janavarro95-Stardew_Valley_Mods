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
            Serialize.WriteToJsonFile(Path.Combine(CleanUp.InvPath, d.Name + ".json"), (Decoration)d);
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
            Serialize.WriteToJsonFile(Path.Combine(CleanUp.InvPath, d.Name + ".json"), (Light)d);
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
            Serialize.WriteToJsonFile(Path.Combine(CleanUp.InvPath, d.Name + ".json"), (Quarry)d);
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
            Serialize.WriteToJsonFile(Path.Combine(CleanUp.InvPath, d.Name + ".json"), (shopObject)d);
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
