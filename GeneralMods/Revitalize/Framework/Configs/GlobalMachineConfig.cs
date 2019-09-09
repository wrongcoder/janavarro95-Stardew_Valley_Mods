using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Configs
{
    public class GlobalMachineConfig
    {

        public bool doMachinesConsumeEnergy;


        public GlobalMachineConfig()
        {
            this.doMachinesConsumeEnergy = true;
        }

        public static GlobalMachineConfig InitializeConfig()
        {
            if (File.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Configs", "MachinesConfig.json")))
                return ModCore.ModHelper.Data.ReadJsonFile<GlobalMachineConfig>(Path.Combine("Configs", "MachinesConfig.json"));
            else
            {
                GlobalMachineConfig Config = new GlobalMachineConfig();
                ModCore.ModHelper.Data.WriteJsonFile<GlobalMachineConfig>(Path.Combine("Configs", "MachinesConfig.json"), Config);
                return Config;
            }
        }
    }
}
