using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarAI.TaskCore.CropLogic
{
    class SeedLogic
    {

        public static Crop parseCropFromSeedIndex(int index)
        {
            return new Crop(index, 0, 0);
        }

        public static KeyValuePair<int, Crop> getSeedCropPair(int index) {

            return new KeyValuePair<int, Crop>(index, parseCropFromSeedIndex(index));
        }

    }
}
