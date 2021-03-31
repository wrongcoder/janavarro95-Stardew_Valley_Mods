using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Objects.InformationFiles.Furniture
{
    public class TableInformation
    {
        public bool canPlaceItemsHere;
        public TableInformation()
        {
            this.canPlaceItemsHere = false;
        }
        public TableInformation(bool CanPlaceItemsHere)
        {
            this.canPlaceItemsHere = CanPlaceItemsHere;
        }

    }
}
