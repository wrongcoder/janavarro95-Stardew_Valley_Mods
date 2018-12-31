using System;
using System.Collections.Generic;
using System.IO;
using CustomNPCFramework.Framework.Enums;

namespace CustomNPCFramework.Framework.Graphics
{
    /// <summary>Used to hold assets from specified directories.</summary>
    public class AssetManager
    {
        /// <summary>A list of all of the assets held by this asset manager.</summary>
        public List<AssetSheet> assets;

        /// <summary>A list of all of the directories managed by this asset manager.</summary>
        public Dictionary<string, string> paths;

        /// <summary>Construct an instance.</summary>
        public AssetManager()
        {
            this.assets = new List<AssetSheet>();
            this.paths = new Dictionary<string, string>();
        }

        /// <summary>Construct an instance.</summary>
        /// <param name="assetsPathsToLoadFrom">A list of all directories to be managed by the asset manager. Name, path is the key pair value.</param>
        public AssetManager(Dictionary<string, string> assetsPathsToLoadFrom)
        {
            this.assets = new List<AssetSheet>();
            this.paths = assetsPathsToLoadFrom;
        }

        /// <summary>Default loading function from hardcoded paths.</summary>
        public void loadAssets()
        {
            foreach (var path in this.paths)
                this.ProcessDirectory(path.Value);
        }

        /// <summary>Process all .json files in the given directory. If there are more nested directories, keep digging to find more .json files. Also allows us to specify a broader directory like Content/Grahphics/ModularNPC/Hair to have multiple hair styles.</summary>
        /// <param name="targetDirectory">The absolute directory path to process.</param>
        /// <remarks>Taken from Microsoft c# documented webpages.</remarks>
        private void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] files = Directory.GetFiles(targetDirectory, "*.json");
            foreach (string file in files)
                this.ProcessFile(file, targetDirectory);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                this.ProcessDirectory(subdirectory);
        }

        /// <summary>Actually load in the asset information.</summary>
        /// <param name="file">The absolute file path to process.</param>
        /// <param name="path">The absolute directory path containing the file.</param>
        private void ProcessFile(string file, string path)
        {
            try
            {
                ExtendedAssetInfo info = ExtendedAssetInfo.readFromJson(file);
                AssetSheet sheet = new AssetSheet(info, path);
                this.addAsset(sheet);
                Class1.ModMonitor.Log("Loaded in new modular asset: " + info.assetName + " asset type: " + info.type);
            }
            catch
            {
                AssetInfo info = AssetInfo.readFromJson(file);
                AssetSheet sheet = new AssetSheet(info, path);
                this.addAsset(sheet);
            }
        }

        /// <summary>Add an asset to be handled from the asset manager.</summary>
        /// <param name="asset">The asset sheet.</param>
        public void addAsset(AssetSheet asset)
        {
            this.assets.Add(asset);
        }

        /// <summary>Get an individual asset by its name.</summary>
        /// <param name="s">The asset name.</param>
        public AssetSheet getAssetByName(string s)
        {
            foreach (var v in this.assets)
            {
                if (v.assetInfo.assetName == s)
                    return v;
            }
            return null;
        }

        /// <summary>Add a new path to the asset manager and create the directory for it.</summary>
        /// <param name="path">The absolute path to add.</param>
        public void addPathCreateDirectory(KeyValuePair<string, string> path)
        {
            this.addPath(path);
            string dir = Path.Combine(Class1.ModHelper.DirectoryPath, path.Value);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(Path.Combine(Class1.ModHelper.DirectoryPath, path.Value));
        }

        /// <summary>Add a path to the dictionary.</summary>
        /// <param name="path">The absolute path to add.</param>
        private void addPath(KeyValuePair<string, string> path)
        {
            this.paths.Add(path.Key, path.Value);
        }

        /// <summary>Create appropriate directories for the path.</summary>
        private void createDirectoriesFromPaths()
        {
            foreach (var v in this.paths)
                Directory.CreateDirectory(Path.Combine(Class1.ModHelper.DirectoryPath, v.Value));
        }

        /// <summary>Get a list of assets which match the given critera.</summary>
        /// <param name="gender">The gender to match.</param>
        public List<AssetSheet> getListOfAssetsThatMatchThisCriteria(Genders gender)
        {
            List<AssetSheet> sheets = new List<AssetSheet>();
            foreach (var v in this.assets)
            {
                if (v.assetInfo is ExtendedAssetInfo info)
                {
                    if (info.gender == gender)
                        sheets.Add(v);
                }
            }
            return sheets;
        }

        /// <summary>Get a list of assets which match the given critera.</summary>
        /// <param name="type">The part type to match.</param>
        public List<AssetSheet> getListOfAssetsThatMatchThisCriteria(PartType type)
        {
            List<AssetSheet> sheets = new List<AssetSheet>();
            foreach (var v in this.assets)
            {
                if (v.assetInfo is ExtendedAssetInfo info)
                {
                    if (info.type == type)
                        sheets.Add(v);
                }
            }
            return sheets;
        }

        /// <summary>Get a list of assets which match the given critera.</summary>
        /// <param name="gender">The gender to match.</param>
        /// <param name="type">The part type to match.</param>
        public List<AssetSheet> getListOfAssetsThatMatchThisCriteria(Genders gender, PartType type)
        {
            List<AssetSheet> sheets = new List<AssetSheet>();
            foreach (var v in this.assets)
            {
                if (v.assetInfo is ExtendedAssetInfo info)
                {
                    if (info.type == type && info.gender == gender)
                        sheets.Add(v);
                }
            }
            return sheets;
        }

        /// <summary>Get a list of assets which match the given critera.</summary>
        /// <param name="season">The season to match.</param>
        public List<AssetSheet> getListOfAssetsThatMatchThisCriteria(Seasons season)
        {
            List<AssetSheet> sheets = new List<AssetSheet>();
            foreach (var v in this.assets)
            {
                if (v.assetInfo is ExtendedAssetInfo info)
                {
                    foreach (var sea in info.seasons)
                    {
                        if (sea == season)
                            sheets.Add(v);
                        break; //Only need to find first validation that this is a valid asset.
                    }
                }
            }
            return sheets;
        }

        /// <summary>Get a list of assets which match the given critera.</summary>
        /// <param name="gender">The gender to match.</param>
        /// <param name="season">The season to match.</param>
        public List<AssetSheet> getListOfAssetsThatMatchThisCriteria(Genders gender, Seasons season)
        {
            List<AssetSheet> sheets = new List<AssetSheet>();
            foreach (var v in this.assets)
            {
                if (v.assetInfo is ExtendedAssetInfo info)
                {
                    foreach (var sea in info.seasons)
                    {
                        if (sea == season && info.gender == gender)
                            sheets.Add(v);
                        break; //Only need to find first validation that this is a valid asset.
                    }
                }
            }
            return sheets;
        }

        /// <summary>Get a list of assets which match the given critera.</summary>
        /// <param name="gender">The gender to match.</param>
        /// <param name="season">The season to match.</param>
        /// <param name="type">The part type to match.</param>
        public List<AssetSheet> getListOfAssetsThatMatchThisCriteria(Genders gender, Seasons season, PartType type)
        {
            List<AssetSheet> sheets = new List<AssetSheet>();
            foreach (var v in this.assets)
            {
                if (v.assetInfo is ExtendedAssetInfo info)
                {
                    foreach (var sea in info.seasons)
                    {
                        if (sea == season && info.gender == gender && info.type == type)
                            sheets.Add(v);
                    }
                }
            }
            return sheets;
        }
    }
}
