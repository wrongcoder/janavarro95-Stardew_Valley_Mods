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
        public static Dictionary<string, SerializerDataNode> acceptedTypes;
        public static   Dictionary<string, QuarryDataNode> quarryList;



        public static void initializeDictionaries()
        {
            acceptedTypes = new Dictionary<string, SerializerDataNode>();
            quarryList = new Dictionary<string, QuarryDataNode>();

            fillAllDictionaries();
       }

        public static void fillAllDictionaries()
        {
            addAllAcceptedTypes();
            fillQuaryList();
        }
     

        public static void addAllAcceptedTypes()
        {
            acceptedTypes.Add("Revitalize.Objects.Decoration", new SerializerDataNode(new ser(Serialize.serializeDecoration) ,new par(Serialize.parseDecoration)));
            acceptedTypes.Add("Revitalize.Objects.Light", new SerializerDataNode(new ser(Serialize.serializeLight), new par(Serialize.parseLight)));
            acceptedTypes.Add("Revitalize.Objects.shopObject", new SerializerDataNode(new ser(Serialize.serializeShopObject), new par(Serialize.parseShopObject)));
            acceptedTypes.Add("Revitalize.Objects.Machines.Quarry", new SerializerDataNode(new ser(Serialize.serializeQuarry), new par(Serialize.parseQuarry)));

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




    }
}
