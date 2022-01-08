using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.SaveData
{
    /// <summary>
    /// Base class for handling persistent save data for the game for individual saves.
    /// </summary>
    public class SaveDataBase
    {

        protected bool shouldSaveData;

        public SaveDataBase()
        {

        }

        public virtual void save()
        {

        }

        public void cleanUpPostSave()
        {
            this.shouldSaveData = false;
        }

        public virtual bool getShouldSaveData()
        {
            return this.shouldSaveData;
        }



    }
}
