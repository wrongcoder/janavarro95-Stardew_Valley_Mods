using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Omegasis.Revitalize.Framework.SaveData.ShopConditionsSaveData
{
    public class CarpenterShopSaveData : ShopSaveDataInfo
    {

        public const string SaveFileName = "CarpenterShopSaveData.json";

        public CarpenterShopSaveData()
        {

        }

        public override void save()
        {
            this.save(SaveFileName);
        }

    }
}
