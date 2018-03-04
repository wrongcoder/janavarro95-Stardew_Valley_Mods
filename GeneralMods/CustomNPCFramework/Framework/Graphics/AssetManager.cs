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

        public AssetManager(Dictionary<string,string> assetsPathsToLoadFrom)
        {
            this.assets = new List<AssetSheet>();
            this.paths = assetsPathsToLoadFrom;
        }

        /// <summary>
        /// Default loading function from hard coded paths.
        /// </summary>
        public void loadAssets()
        {
            foreach(var path in this.paths)
            {
                ProcessDirectory(path.Value);
            }
        }

        /// <summary>
        /// Taken from Microsoft c# documented webpages.
        /// Process all .json files in the given directory. If there are more nested directories, keep digging to find more .json files. Also allows us to specify a broader directory like Content/Grahphics/ModularNPC/Hair to have multiple hair styles.
        /// </summary>
        /// <param name="targetDirectory"></param>
        private void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] files = Directory.GetFiles(targetDirectory, "*.json");
            foreach (var file in files)
            {
                ProcessFile(file,targetDirectory);
            }
            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        private void ProcessFile(string file,string path)
        {
            AssetInfo info = AssetInfo.readFromJson(file);
            AssetSheet sheet = new AssetSheet(info, path);
            addAsset(sheet);
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
