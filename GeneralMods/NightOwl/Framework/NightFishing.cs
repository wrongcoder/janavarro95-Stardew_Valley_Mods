using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Locations;

namespace Omegasis.NightOwl.Framework
{
    public class NightFishing
    {

        /// <summary>Edit a matched asset.</summary>
        /// <param name="asset">A helper which encapsulates metadata about an asset and enables changes to it.</param>
        public static void TryToEditFishDataAsset(object sender, AssetRequestedEventArgs assetRequestedEventArgs)
        {

            if (assetRequestedEventArgs.NameWithoutLocale.IsEquivalentTo("Data/FIsh"))
            {
                assetRequestedEventArgs.Edit(EditFishDataAsset);
            }
        }

        public static void EditFishDataAsset(IAssetData data)
        {
            IDictionary<string, string> fishData = data.AsDictionary<string, string>().Data;
            foreach(string fishId in fishData.Keys)
            {
                string dataString = fishData[fishId];
                string[] splitData = dataString.Split("/");
                string[] splitTimeString = splitData[5].Split(" ");
                if (splitTimeString.Length <= 1)
                {
                    continue;
                }
                string endTime = splitTimeString[1];
                if(endTime == "2600")
                {
                    endTime = "3000";
                    splitData[5] = splitTimeString[0] + " " + endTime;
                    dataString = string.Join("/", splitData);
                    fishData[fishId] = dataString;
                }
            }
        }
    }
}
