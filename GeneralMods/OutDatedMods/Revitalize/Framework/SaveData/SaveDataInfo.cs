using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.SaveData
{
    /// <summary>
    /// Base class for handling persistent save data for the game for individual saves.
    /// </summary>
    public class SaveDataInfo
    {

        public SaveDataInfo()
        {

        }

        public virtual void save()
        {

        }
        /// <summary>
        /// Writes this save intormation to a specific file.
        /// </summary>
        /// <param name="FileName"></param>
        public virtual void save(string FileName)
        {

        }

        public virtual void load()
        {

        }

    }
}
