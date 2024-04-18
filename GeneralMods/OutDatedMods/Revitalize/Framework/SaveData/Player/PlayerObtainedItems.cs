using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.SaveData.Player
{
    public class PlayerObtainedItems: SaveDataInfo
    {
        public HashSet<string> obtainedItems = new HashSet<string>();
        public PlayerObtainedItems()
        {
        }

        
    }
}
