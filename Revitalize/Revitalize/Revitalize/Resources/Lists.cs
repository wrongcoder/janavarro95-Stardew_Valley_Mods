using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Resources
{
    class Lists
    {
      public static  List<Revitalize.Resources.DataNodes.TrackedTerrainDataNode> trackedTerrainFeatures;
        public static List<Revitalize.Resources.DataNodes.TrackedTerrainDummyDataNode> trackedTerrainFeaturesDummyList;

        public static void initializeAllLists()
        {
            trackedTerrainFeatures = new List<DataNodes.TrackedTerrainDataNode>();
            trackedTerrainFeaturesDummyList = new List<DataNodes.TrackedTerrainDummyDataNode>();
           
        }

        public static void loadAllLists()
        {
            Serialize.parseTrackedTerrainDataNodeList(Path.Combine( Serialize.PlayerDataPath ,"TrackedTerrainFeaturesList.json"));
              
            Class1.hasLoadedTerrainList = true;
          //  Log.AsyncC(Path.Combine(Serialize.PlayerDataPath, "TrackedTerrainFeaturesList.json"));
        }
    }
}
