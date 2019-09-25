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

        public double solarPanelNonSunnyDayEnergyMultiplier;
        public double solarPanelNightEnergyGenerationMultiplier;
        public bool showMachineNotificationBubble_InventoryFull;
        public float machineNotificationBubbleAlpha;

        public int grinderEnergyConsumption;
        public int grinderTimeToGrind;

        public int miningDrillEnergyConsumption;
        public int miningDrillTimeToMine;

        public int steamBoilerV1_requiredWaterPerOperation;
        public int steamBoilerV1_producedSteamPerOperation;

        public GlobalMachineConfig()
        {
            this.doMachinesConsumeEnergy = true;
            this.solarPanelNonSunnyDayEnergyMultiplier = 0.0d;
            this.solarPanelNightEnergyGenerationMultiplier = .125d;
            this.showMachineNotificationBubble_InventoryFull = true;
            this.machineNotificationBubbleAlpha = 0.75f;
            this.grinderEnergyConsumption = 20;
            this.grinderTimeToGrind = 30;
            this.miningDrillEnergyConsumption = 50;
            this.miningDrillTimeToMine = 60;
            this.steamBoilerV1_requiredWaterPerOperation = 200;
            this.steamBoilerV1_producedSteamPerOperation = 100;
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
