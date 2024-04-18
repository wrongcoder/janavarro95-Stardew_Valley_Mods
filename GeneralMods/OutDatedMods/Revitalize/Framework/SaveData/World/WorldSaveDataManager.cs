using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.SaveData.World
{
    public class WorldSaveDataManager
    {

        public BuildingSaveData buildingSaveData = new BuildingSaveData();

        public WorldSaveDataManager()
        {

        }

        public virtual void save()
        {
            this.buildingSaveData.save();
        }

        public virtual void load()
        {
            this.buildingSaveData = RevitalizeModCore.SaveDataManager.initializeSaveData<BuildingSaveData>(this.getRelativeSavePath(), BuildingSaveData.SaveFileName);
        }


        public virtual string getRelativeSavePath()
        {
            return Path.Combine(RevitalizeModCore.SaveDataManager.getRelativeSaveDataPath(), "World");
        }

        public virtual string getFullSavePath()
        {
            return Path.Combine(RevitalizeModCore.SaveDataManager.getFullSaveDataPath(), "World");
        }
    }
}
