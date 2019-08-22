using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Utilities
{
    public class ObjectUtilities
    {
        public static bool IsObjectHoldingItem(StardewValley.Object obj)
        {
            if (obj.heldObject.Value != null) return true;
            else return false;
        }

        /// <summary>
        /// Checks to see if the given object is a SDV vanilla furnace.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsObjectFurnace(StardewValley.Object obj)
        {
            if (obj.ParentSheetIndex == 13 && obj.bigCraftable.Value && obj.Category == -9 && obj.Name == "Furnace")
            {
                return true;
            }
            else return false;
        }

    }
}
