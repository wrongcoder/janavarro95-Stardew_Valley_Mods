using Newtonsoft.Json.Linq;
using StarAI.PathFindingCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarAI.PathFindingCore
{
    public class TileExceptionNode
    {
        public string imageSource;
        public int index;


        public TileExceptionNode()
        {
        }

        public TileExceptionNode(string ImageSource, int TileIndex)
        {
            imageSource = ImageSource;
            index = TileIndex;
        }

        public static TileExceptionNode parseJson(string s)
        {
            dynamic obj = JObject.Parse(s);
            TileExceptionNode t = new TileExceptionNode();
            t.imageSource = obj.imageSource;
            t.index = obj.index;
            return t;
        }

        public void serializeJson(string s)
        {
            StardustCore.ModCore.SerializationManager.WriteToJsonFile(Path.Combine(s, "tileExceptionData"+ this.index.ToString() + ".json"), (TileExceptionNode)this);
        }

    }
}
