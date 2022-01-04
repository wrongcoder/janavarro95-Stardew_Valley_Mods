using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize;

namespace Revitalize.Framework.Configs.ObjectsConfigs
{
    public class FeedThreasherConfig
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

        public FeedThreasherConfig()
        {

        }

        public static FeedThreasherConfig InitializeConfig()
        {
            if (File.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Configs", "ObjectConfigs", "FeedThreasherConfig.json")))
                return ModCore.ModHelper.Data.ReadJsonFile<FeedThreasherConfig>(Path.Combine("Configs", "ObjectConfigs", "FeedThreasherConfig.json"));
            else
            {
                FeedThreasherConfig Config = new FeedThreasherConfig();
                ModCore.ModHelper.Data.WriteJsonFile(Path.Combine("Configs", "ObjectConfigs", "FeedThreasherConfig.json"), Config);
                return Config;
            }
        }


    }
}
