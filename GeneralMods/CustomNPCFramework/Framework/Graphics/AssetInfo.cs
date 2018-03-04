using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.Graphics
{
    public class AssetInfo
    {
        public string assetName;
        public string leftAssetName;
        public string rightAssetName;
        public string upAssetName;
        public string downAssetName;
        public Vector2 assetSize;
        public bool randomizeUponLoad;
       
        public AssetInfo()
        {

        }

        public AssetInfo(string assetName,string Lname, string Rname, string Uname, string Dname, Vector2 assetSize, bool randomizeUponLoad)
        {
            this.assetName = assetName;
            this.leftAssetName = Lname;
            this.rightAssetName = Rname;
            this.upAssetName = Uname;
            this.downAssetName = Dname;
            this.assetSize = assetSize;
            this.randomizeUponLoad = randomizeUponLoad;
        }

        public AssetInfo(string assetName,NamePairings pair, Vector2 assetSize, bool randomizeUponLoad)
        {
            this.assetName = assetName;
            this.leftAssetName = pair.leftString;
            this.rightAssetName = pair.rightString;
            this.upAssetName = pair.upString;
            this.downAssetName = pair.downString;
            this.assetSize = assetSize;
            this.randomizeUponLoad = randomizeUponLoad;
        }

        /// <summary>
        /// Save the json to a certain location.
        /// </summary>
        /// <param name="path"></param>
        public void writeToJson(string path)
        {
            Class1.ModHelper.WriteJsonFile<AssetInfo>(path, this);
        }

        /// <summary>
        /// Read the json from a certain location.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static AssetInfo readFromJson(string path)
        {
           return Class1.ModHelper.ReadJsonFile<AssetInfo>(path);
        }
    }
}
