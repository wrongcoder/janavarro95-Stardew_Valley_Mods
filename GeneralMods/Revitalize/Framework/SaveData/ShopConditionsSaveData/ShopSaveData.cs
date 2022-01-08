using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.SaveData.ShopConditionsSaveData
{
    /// <summary>
    /// Unified class for storing all of the necessary save data that needs to be persisted for shops.
    /// </summary>
    public class ShopSaveData:SaveDataBase
    {
        /// <summary>
        /// Deals with necessary save data regarding the animal shop.
        /// </summary>
        public AnimalShopSaveData animalShopSaveData;
        public ShopSaveData()
        {
            this.animalShopSaveData = AnimalShopSaveData.LoadOrCreate();
        }

        public override bool getShouldSaveData()
        {
            return this.animalShopSaveData.getShouldSaveData();
        }

        public override void save()
        {
            this.animalShopSaveData.save();
        }


    }
}
