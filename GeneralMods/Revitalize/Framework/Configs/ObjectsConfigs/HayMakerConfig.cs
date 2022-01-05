using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize;

namespace Revitalize.Framework.Configs.ObjectsConfigs
{
    public class HayMakerConfig
    {
        public int NumberOfCornRequired = 1;
        public int NumberOfWheatRequired = 1;
        public int NumberOfFiberRequired = 5;
        public int NumberOfAmaranthRequired = 1;

        public int CornToHayOutput = 3;
        public int WheatToHayOutput = 5;
        public int FiberToHayOutput = 1;
        public int AmaranthToHayOutput = 10;

        public int MinutesToProcess = 60;

        public HayMakerConfig()
        {

        }

        public static HayMakerConfig InitializeConfig()
        {
            if (File.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Configs", "ObjectConfigs", "HayMakerConfig.json")))
                return ModCore.ModHelper.Data.ReadJsonFile<HayMakerConfig>(Path.Combine("Configs", "ObjectConfigs", "HayMakerConfig.json"));
            else
            {
                HayMakerConfig Config = new HayMakerConfig();
                ModCore.ModHelper.Data.WriteJsonFile(Path.Combine("Configs", "ObjectConfigs", "HayMakerConfig.json"), Config);
                return Config;
            }
        }


    }
}
