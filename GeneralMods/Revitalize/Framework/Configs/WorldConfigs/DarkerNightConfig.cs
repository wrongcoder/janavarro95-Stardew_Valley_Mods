using System.IO;

namespace Revitalize.Framework.Configs.WorldConfigs
{
    /// <summary>
    /// Deals with configurations for darker night.
    /// </summary>
    public class DarkerNightConfig
    {
        /// <summary>
        /// Is darker night enabled?
        /// </summary>
        public bool Enabled;
        /// <summary>
        /// The intensity for how dark it gets at night.
        /// </summary>
        public float DarknessIntensity;
        public DarkerNightConfig()
        {
            this.Enabled = true;
            this.DarknessIntensity = .9f;
        }


        public static DarkerNightConfig InitializeConfig()
        {
            if (File.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Configs", "WorldConfigs", "DarkerNightConfig.json")))
                return ModCore.ModHelper.Data.ReadJsonFile<DarkerNightConfig>(Path.Combine("Configs", "WorldConfigs", "DarkerNightConfig.json"));
            else
            {
                DarkerNightConfig Config = new DarkerNightConfig();
                ModCore.ModHelper.Data.WriteJsonFile(Path.Combine("Configs", "Shops", "DarkerNightConfig.json"), Config);
                return Config;
            }
        }
    }
}
