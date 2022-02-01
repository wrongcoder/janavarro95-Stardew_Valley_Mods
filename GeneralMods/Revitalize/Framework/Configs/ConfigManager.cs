using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace Revitalize.Framework.Configs
{
    /// <summary>
    /// Handles holding all of the config information.
    /// </summary>
    public class ConfigManager
    {

        public ObjectsConfigs.ObjectConfigManager objectConfigManager;

        public ShopConfigs.ShopsConfigManager shopsConfigManager;

        public WorldConfigs.WorldConfigManager worldConfigManager;

        public ConfigManager()
        {

            this.objectConfigManager = new ObjectsConfigs.ObjectConfigManager();
            this.shopsConfigManager = new ShopConfigs.ShopsConfigManager();
            this.worldConfigManager = new WorldConfigs.WorldConfigManager();
        }

        /// <summary>
        /// Initializes a config for Revitalize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RelativePathToConfig"></param>
        /// <returns></returns>
        public static T initializeConfig<T>(params string[] RelativePathToConfig) where T : class
        {
            return initializeConfig<T>(Revitalize.RevitalizeModCore.ModHelper, RelativePathToConfig);
        }

        /// <summary>
        /// Initializes the config at the given relative path or creates it. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The mod helper to use to get the full path for file existence checking.</param>
        /// <param name="RelativePathToConfig"></param>
        /// <returns></returns>
        public static T initializeConfig<T>(IModHelper helper, params string[] RelativePathToConfig) where T : class
        {
            string relativePath = Path.Combine(RelativePathToConfig);
            if (File.Exists(Path.Combine(helper.DirectoryPath,relativePath )))
                return helper.Data.ReadJsonFile<T>(relativePath);
            else
            {
                T Config = (T)Activator.CreateInstance(typeof(T));
                helper.Data.WriteJsonFile(relativePath, Config);
                return Config;
            }
        }
    }
}
