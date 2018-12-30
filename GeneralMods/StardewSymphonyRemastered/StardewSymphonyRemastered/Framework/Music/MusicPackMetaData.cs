using System;
using System.IO;
using StardustCore.UIUtilities;

namespace StardewSymphonyRemastered.Framework
{
    /// <summary>Holds information regarding information relating to music packs such as name, description, author, and version.</summary>
    public class MusicPackMetaData
    {
        public string name;
        public string author;
        public string description;
        public string versionInfo;
        public string pathToMusicPackIcon;
        private Texture2DExtended Icon;

        /// <summary>Construct an instance.</summary>
        /// <param name="name">The name to be displayed for the music pack.</param>
        /// <param name="author">The author who compiled ths music pack.</param>
        /// <param name="description">The description of</param>
        public MusicPackMetaData(string name, string author, string description, string versionInfo, string pathToMusicPackIcon)
        {
            this.name = name;
            this.author = author;
            this.description = description;
            this.versionInfo = versionInfo;
            this.pathToMusicPackIcon = pathToMusicPackIcon;
            try
            {
                this.Icon = new Texture2DExtended(StardewSymphony.ModHelper, StardewSymphony.Manifest, this.pathToMusicPackIcon + ".png");
            }
            catch (Exception err)
            {
                this.Icon = null;
                if (StardewSymphony.Config.EnableDebugLog)
                    StardewSymphony.ModMonitor.Log(err.ToString());
            }
        }

        /// <summary>Construct an instance.</summary>
        public MusicPackMetaData() { }

        /// <summary>Loads the music pack information from a json file.</summary>
        public static MusicPackMetaData readFromJson(string path)
        {
            string json = Path.Combine(path, "MusicPackInformation.json");
            var meta = StardewSymphony.ModHelper.ReadJsonFile<MusicPackMetaData>(json);

            string[] pathParse = path.Split(new string[] { StardewSymphony.ModHelper.DirectoryPath }, StringSplitOptions.None);

            try
            {
                try
                {
                    meta.Icon = new Texture2DExtended(StardewSymphony.ModHelper, StardewSymphony.Manifest, Path.Combine(pathParse[1].Remove(0, 1), meta.pathToMusicPackIcon + ".png"));
                }
                catch
                {
                    meta.Icon = new Texture2DExtended(StardewSymphony.ModHelper, StardewSymphony.Manifest, Path.Combine(pathParse[1].Remove(0, 1), meta.pathToMusicPackIcon));
                }
            }
            catch (Exception err)
            {
                if (StardewSymphony.Config.EnableDebugLog)
                    StardewSymphony.ModMonitor.Log(err.ToString());
            }
            return meta;
        }

        /// <summary>Writes the music pack information to a json file.</summary>
        public void writeToJson(string path)
        {
            StardewSymphony.ModHelper.WriteJsonFile<MusicPackMetaData>(path, this);
        }

        public Texture2DExtended getTexture()
        {
            return this.Icon;
        }
    }
}
