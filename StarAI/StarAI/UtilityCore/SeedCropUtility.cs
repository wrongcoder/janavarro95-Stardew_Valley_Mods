using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarAI.UtilityCore
{
    class SeedCropUtility
    {
       public static Dictionary<KeyValuePair<int, Crop>, float> CropSeedUtilityDictionary = new Dictionary<KeyValuePair<int, Crop>, float>();

        public static void setUpCropUtilityDictionaryDaily()
        {
            CropSeedUtilityDictionary.Clear();
            int numberOfInitializedUtilitySeedValues = 0;
            foreach(var item in Game1.objectInformation)
            {
                StardewValley.Object test = new StardewValley.Object(item.Key, 1);
                if (test.getCategoryName() == "Seed")
                {
                    KeyValuePair<int, Crop> pair = StarAI.TaskCore.CropLogic.SeedLogic.getSeedCropPair(test.parentSheetIndex); 
                    if (pair.Value == null) continue;
                    if (!pair.Value.seasonsToGrowIn.Contains(Game1.currentSeason)) continue;
                    float utilityValue = averageGoldPerDay(pair.Value,test);

                    if (utilityValue <= 0) continue;
                    CropSeedUtilityDictionary.Add(pair, utilityValue); //CHANGE THIS TO BE BASED ON THE MENU UTILITY
                    numberOfInitializedUtilitySeedValues++;
                    ModCore.CoreMonitor.Log("Star AI: Utility Core: Calculating " + test.name + " for utility picking with a value of: "+utilityValue.ToString());
                }
            }
            ModCore.CoreMonitor.Log("Star AI: Utility Core: Calculating " + numberOfInitializedUtilitySeedValues + " seeds for utility picking.");
            if (numberOfInitializedUtilitySeedValues == 0)
            {
                ModCore.CoreMonitor.Log("No seed values initialized. There must be no possible seeds I can grow for the rest of this season.",StardewModdingAPI.LogLevel.Alert);
            }
        }

        public static int numberOfDaysToGrow(Crop c)
        {
            int total = 0;
            foreach(var v in c.phaseDays)
            {
                if (v != -1) total += v;
            }
            return (total-100000+1);
        }


        //Doesn't recalculate crops that regrow. Favors crops with mutiple yields. Quantity over quality I suppose.
        public static float averageGoldPerDay(Crop c,Item seeds)
        {
     
            StardewValley.Object crop = new StardewValley.Object(c.indexOfHarvest, 1);

           
            int days = numberOfDaysToGrow(c);
            //ModCore.CoreMonitor.Log("DAYS: " + days);
            if (days <= 0) return 0;
            int maxHarvest = maxHarvestsRemaining(c);
            if (maxHarvest == 0) return 0;
            if (c.maxHarvest <= 0)
            {
                return ((Game1.player.farmingLevel * .02f + 1.01f) * (maxHarvest) * crop.price) - seeds.salePrice() / days;
            }
            else
            {
                return ((Game1.player.farmingLevel * .02f + 1.01f) * (maxHarvest *c.maxHarvest) * crop.price) - seeds.salePrice() / days;
            }
           
        }

        public static int maxHarvestsRemaining(Crop c)
        {
            return (28-Game1.dayOfMonth)/numberOfDaysToGrow(c);
        }


        public static List<Item> sortSeedListByUtility(List<Item> seedList)
        {
            List<KeyValuePair<KeyValuePair<int, Crop>, float>> sortList = new List<KeyValuePair<KeyValuePair<int, Crop>, float>>();
            foreach(var seeds in seedList)
            {
                var ret = getKeyPairValueBySeedIndex(seeds.parentSheetIndex);
                if (ret.Key.Key == -999) continue;
                sortList.Add(ret);
            }

            //Sort the list by utility of the seeds.
            sortList.Sort(delegate (KeyValuePair < KeyValuePair < int, Crop >,float> t1, KeyValuePair<KeyValuePair<int, Crop>, float> t2)
            {
                return t1.Value.CompareTo(t2.Value);
            });

            sortList.Reverse(); //I want max to be first instead of min.

            float maxUtility = sortList.ElementAt(0).Value;
            List<int> finalList = new List<int>();
            foreach(var utilitySeed in sortList)
            {
                if (utilitySeed.Value >= maxUtility) finalList.Add(utilitySeed.Key.Key);
            }
            List<Item> finalShopStock = new List<Item>();
            foreach(var seedIndex in finalList)
            {
                foreach(var seeds in seedList)
                {
                    if (seedIndex == seeds.parentSheetIndex) finalShopStock.Add(seeds);
                }
            }

            return finalShopStock;
        }

        public static KeyValuePair<KeyValuePair<int,Crop>,float> getKeyPairValueBySeedIndex(int seedIndex)
        {
            foreach(var key in CropSeedUtilityDictionary)
            {
                if (key.Key.Key == seedIndex) return key;
            }
            return new KeyValuePair<KeyValuePair<int, Crop>, float>(new KeyValuePair<int, Crop>(-999,null),-999);
        }

    }
}
