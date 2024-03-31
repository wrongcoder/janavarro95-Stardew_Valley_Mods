using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishHatchery
{
    public static class ModConstants
    {

        /// <summary>
        /// The unqualified object id. Also used for referencing the object for crafting recipe buying + crafting purposes.
        /// </summary>
        public static string FishHatcheryObjectId = "Omegasis.FishHatchery.ObjectId";
        /// <summary>
        /// The fully qualified object id for the fish hatchery.
        /// </summary>
        public static string FishHatcheryQualifiedObjectId = "(BC)"+FishHatcheryObjectId;

    }
}
