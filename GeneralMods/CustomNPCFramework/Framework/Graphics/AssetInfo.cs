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
        public NamePairings standingAssetPaths;
        public NamePairings swimmingAssetPaths;
        public NamePairings movingAssetPaths;
        public NamePairings sittingAssetPaths;
        public Vector2 assetSize;
        public bool randomizeUponLoad;
       
        public AssetInfo()
        {

        }

        public AssetInfo(string assetName,NamePairings StandingAssetPaths, NamePairings MovingAssetPaths, NamePairings SwimmingAssetPaths, NamePairings SittingAssetPaths, Vector2 assetSize, bool randomizeUponLoad)
        {
            this.assetName = assetName;
            this.sittingAssetPaths = SittingAssetPaths;
            this.standingAssetPaths = StandingAssetPaths;
            this.movingAssetPaths = MovingAssetPaths;
            this.swimmingAssetPaths = SwimmingAssetPaths;
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
