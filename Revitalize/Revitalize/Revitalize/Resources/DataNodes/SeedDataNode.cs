using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Resources.DataNodes
{
    class SeedDataNode
    {
       public int parentIndex;
       public int cropIndex;

     public SeedDataNode(int parentInt, int cropInt)
        {
            parentIndex = parentInt;
            cropIndex = cropInt;

        }

    }
}
