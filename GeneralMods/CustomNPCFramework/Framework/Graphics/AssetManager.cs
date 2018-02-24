using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.Graphics
{
    public class AssetManager
    {
        public List<AssetSheet> assets;
        public Dictionary<string,string> paths;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        public AssetManager()
        {
            this.assets = new List<AssetSheet>();
            this.paths = new Dictionary<string, string>();
        }

        /// <summary>
        /// Default loading function from paths.
        /// </summary>
        public void loadAssets()
        {
            foreach(var path in this.paths)
            {
               string[] files= Directory.GetFiles(path.Value, "*.json");
                foreach(var file in files)
                {
                    AssetInfo info = AssetInfo.readFromJson(file);
                    AssetSheet sheet = new AssetSheet(info,path.Value);
                    this.assets.Add(sheet);
                }
            }
        }

        /// <summary>
        /// Add an asset to be handled from the asset manager.
        /// </summary>
        /// <param name="asset"></param>
        public void addAsset(AssetSheet asset)
        {
            this.assets.Add(asset);
        }

        /// <summary>
        /// Get an individual asset by its name.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public AssetSheet getAssetByName(string s)
        {
            foreach(var v in assets)
            {
                if (v.assetInfo.name == s) return v;
            }
            return null;
        }
        
        /// <summary>
        /// Add a new path to the asset manager and create the directory for it.
        /// </summary>
        /// <param name="path"></param>
        public void addPathCreateDirectory(KeyValuePair<string,string> path)
        {
            this.addPath(path);
            string dir = Path.Combine(Class1.ModHelper.DirectoryPath, path.Value);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(Path.Combine(Class1.ModHelper.DirectoryPath, path.Value));
            }
        }

        /// <summary>
        /// Add a path to the dictionary.
        /// </summary>
        /// <param name="path"></param>
        private void addPath(KeyValuePair<string,string> path)
        {
            this.paths.Add(path.Key, path.Value);
        }

        /// <summary>
        /// Create appropriate directories for the path.
        /// </summary>
        private void createDirectoriesFromPaths()
        {
            foreach(var v in paths)
            {
                Directory.CreateDirectory(Path.Combine(Class1.ModHelper.DirectoryPath,v.Value));
            }
        }
    }
}
