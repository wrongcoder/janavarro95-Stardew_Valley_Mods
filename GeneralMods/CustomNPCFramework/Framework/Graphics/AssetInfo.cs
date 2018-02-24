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
        public string name;
        public Vector2 assetSize;
        public bool randomizeUponLoad;
        /// <summary>
        /// A constructor use to create asset info which can then be used to create asset sheets.
        /// </summary>
        /// <param name="name">The name of the texture sheet. Can be different than the actual file name.</param>
        /// <param name="assetSize">The size of the individual sprites on the texture sheet. Ex 16x16 pixels.</param>
        /// <param name="randomizeUponLoad">If true, the index for the asset will be randomized. Good for getting variation from a texture.</param>
        public AssetInfo(string name, Vector2 assetSize, bool randomizeUponLoad)
        {
            this.name = name;
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
