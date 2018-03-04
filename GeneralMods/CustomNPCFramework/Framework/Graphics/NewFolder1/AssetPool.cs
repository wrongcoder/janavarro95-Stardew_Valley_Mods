using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.Graphics.NewFolder1
{
    /// <summary>
    /// Used to contain all of the asset managers.
    /// </summary>
    public class AssetPool
    {

        public Dictionary<string, AssetManager> assetPool;

        public AssetPool()
        {
            this.assetPool = new Dictionary<string, AssetManager>();
        }

        public void addAssetManager(KeyValuePair<string,AssetManager> pair)
        {
            this.assetPool.Add(pair.Key,pair.Value);
        }

        public void addAssetManager(string assetManagerName, AssetManager assetManager)
        {
            this.assetPool.Add(assetManagerName, assetManager);
        }

        public AssetManager getAssetManager(string name)
        {
            assetPool.TryGetValue(name, out AssetManager asset);
            return asset;
        }

        public void removeAssetManager(string key)
        {
            assetPool.Remove(key);
        }

        public void loadAllAssets()
        {
            foreach(KeyValuePair<string,AssetManager> assetManager in this.assetPool)
            {
                assetManager.Value.loadAssets();
            }
        }


    }
}
