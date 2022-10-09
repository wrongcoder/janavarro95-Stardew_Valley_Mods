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


        /// <summary>
        /// Used to determine if the player has obtained the battery pack necessaery to unlock electric furnace recipes.
        /// </summary>
        public bool hasObtainedBatteryPack;

        public CarpenterShopSaveData()
        {

        }

        public override void save()
        {
            this.save(SaveFileName);
        }

        public virtual void setObtainedBatteryPack()
        {
            this.hasObtainedBatteryPack = true;
        }

        public virtual bool getObtainedBatteryPack()
        {
            return this.hasObtainedBatteryPack;
        }

    }
}
