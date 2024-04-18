using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.World.Buildings;

namespace Omegasis.Revitalize.Framework.SaveData.World
{
    public class BuildingSaveData:SaveDataInfo
    {

        public const string SaveFileName = "BuildingSaveData.json";
        /// <summary>
        /// The maximum items that can be stored in the dimensional storage unit.
        /// </summary>
        public long DimensionalStorageUnitMaxItems;
        public BuildingSaveData()
        {

        }

        public override void save()
        {
            RevitalizeModCore.ModHelper.Data.WriteJsonFile(Path.Combine(RevitalizeModCore.SaveDataManager.getRelativeSaveDataPath(),"World", "BuildingSaveData.json"), this);
        }
    
    }
}
