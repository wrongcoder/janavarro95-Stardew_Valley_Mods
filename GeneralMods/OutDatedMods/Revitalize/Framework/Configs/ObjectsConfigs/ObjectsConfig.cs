using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Configs.ObjectsConfigs
{
    public class ObjectsConfig
    {
        public bool showDyedColorName = true;

        public int maxQualityLevel = StardewValley.Object.bestQuality;

        public ObjectsConfig()
        {
        }

        public static ObjectsConfig InitializeConfig()
        {
            if (File.Exists(Path.Combine(RevitalizeModCore.ModHelper.DirectoryPath, "Configs", "ObjectsConfig.json")))
                return RevitalizeModCore.ModHelper.Data.ReadJsonFile<ObjectsConfig>(Path.Combine("Configs", "ObjectsConfig.json"));
            else
            {
                ObjectsConfig Config = new ObjectsConfig();
                RevitalizeModCore.ModHelper.Data.WriteJsonFile(Path.Combine("Configs", "ObjectsConfig.json"), Config);
                return Config;
            }
        }

    }
}
