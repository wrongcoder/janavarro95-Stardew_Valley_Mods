using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using Revitalize.Resources.DataNodes;

namespace Revitalize.Resources
{

    class Dictionaries
    {

        public delegate CoreObject par(string data);
        public delegate void ser(Item item);
        public delegate void world(CoreObject obj);

        public delegate void interactFunction();

        public static Dictionary<string, SerializerDataNode> acceptedTypes;
        public static Dictionary<string, interactFunction> interactionTypes;
        public static   Dictionary<string, QuarryDataNode> quarryList;
        public static Dictionary<string, SeedDataNode> seedList;


        public static void initializeDictionaries()
        {
            acceptedTypes = new Dictionary<string, SerializerDataNode>();
            quarryList = new Dictionary<string, QuarryDataNode>();
            interactionTypes = new Dictionary<string, interactFunction>();
            seedList = new Dictionary<string, SeedDataNode>();
            fillAllDictionaries();
       }

        public static void fillAllDictionaries()
        {
            addAllAcceptedTypes();
            addAllInteractionTypes();
            fillQuaryList();
            fillSeedList();
        }
     

        public static void addAllAcceptedTypes()
        {
            acceptedTypes.Add("Revitalize.Objects.Decoration", new SerializerDataNode(new ser(Serialize.serializeDecoration) ,new par(Serialize.parseDecoration),new world(Serialize.serializeDecorationFromWorld)));
            acceptedTypes.Add("Revitalize.Objects.Light", new SerializerDataNode(new ser(Serialize.serializeLight), new par(Serialize.parseLight),new world(Serialize.serializeLightFromWorld)));
            acceptedTypes.Add("Revitalize.Objects.shopObject", new SerializerDataNode(new ser(Serialize.serializeShopObject), new par(Serialize.parseShopObject), new world(Serialize.serializeShopObjectFromWorld)));
            acceptedTypes.Add("Revitalize.Objects.Machines.Quarry", new SerializerDataNode(new ser(Serialize.serializeQuarry), new par(Serialize.parseQuarry),new world(Serialize.serializeQuarryFromWorld)));
            acceptedTypes.Add("Revitalize.Objects.Machines.Spawner", new SerializerDataNode(new ser(Serialize.serializeSpawner), new par(Serialize.parseSpawner), new world(Serialize.serializeSpawnerFromWorld)));
            acceptedTypes.Add("Revitalize.Objects.GiftPackage", new SerializerDataNode(new ser(Serialize.serializeGiftPackage), new par(Serialize.parseGiftPackage),null));
            acceptedTypes.Add("Revitalize.Objects.ExtraSeeds", new SerializerDataNode(new ser(Serialize.serializeExtraSeeds), new par(Serialize.parseExtraSeeds),null));
        }

        public static void addAllInteractionTypes()
        {
            interactionTypes.Add("Seed", Util.plantCropHere); //for generic stardew seeds
            interactionTypes.Add("Seeds", Util.plantExtraCropHere); //for modded stardew seeds
            interactionTypes.Add("Gift Package", Util.getGiftPackageContents);

        }

       

        public static void fillQuaryList()
        {
            quarryList.Add("clay", new QuarryDataNode("clay", new StardewValley.Object(330, 1, false), 60));
            quarryList.Add("stone", new QuarryDataNode("stone", new StardewValley.Object(390, 1, false), 60));
            quarryList.Add("coal", new QuarryDataNode("coal", new StardewValley.Object(382, 1, false), 240));
            quarryList.Add("copper", new QuarryDataNode("copper",new StardewValley.Object(378,1,false),120));
            quarryList.Add("iron", new QuarryDataNode("iron", new StardewValley.Object(380, 1, false), 480));
            quarryList.Add("gold", new QuarryDataNode("gold", new StardewValley.Object(384, 1, false), 1440));
            quarryList.Add("irridium", new QuarryDataNode("irridium", new StardewValley.Object(386, 1, false), 4320));

        }

        public static void fillSeedList()
        {
            //crop row number is actually counts row 0 on upper left and row right on upper right.
            //parentsheetindex for seeds image, actualCropNumber from crops.xnb
            seedList.Add("Pink Turnip Seeds", new SeedDataNode(1,1)); 
            seedList.Add("Blue Charm Seeds", new SeedDataNode(2, 2));
        }


    }
}
