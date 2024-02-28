using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.StardustCore.World.Objects
{
    public static class ItemRegistryHelper
    {


        public static string ConvertParentSheetIndexToQualifiedItemId(int parentSheetIndex)
        {
            string qualifiedItemId = Convert.ToString(parentSheetIndex);
            if (!ItemRegistry.IsQualifiedItemId(qualifiedItemId))
            {
                qualifiedItemId = ItemRegistry.QualifyItemId(qualifiedItemId);
            }
            return qualifiedItemId;
        }
    }
}
