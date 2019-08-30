using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Environment;

namespace Revitalize.Framework.Configs
{
    public class VanillaMachineRecipeConfig
    {
        /// <summary>
        /// Should the more expensive recipe be used for smelting. If true the 7 gems smelt a sigle nugget. If false they smelt a prismatic shard after 7 days. 
        /// </summary>
        public bool ExpensiveGemstoneToPrismaticFurnaceRecipe;

        public VanillaMachineRecipeConfig()
        {
            this.ExpensiveGemstoneToPrismaticFurnaceRecipe = false;
        }

        public static VanillaMachineRecipeConfig InitializeConfig()
        {
            if (File.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Configs", "VanillaMachineRecipeConfig.json")))
                return ModCore.ModHelper.Data.ReadJsonFile<VanillaMachineRecipeConfig>(Path.Combine("Configs", "VanillaMachineRecipeConfig.json"));
            else
            {
                VanillaMachineRecipeConfig Config = new VanillaMachineRecipeConfig();
                ModCore.ModHelper.Data.WriteJsonFile<VanillaMachineRecipeConfig>(Path.Combine("Configs", "VanillaMachineRecipeConfig.json"), Config);
                return Config;
            }
        }

    }
}
