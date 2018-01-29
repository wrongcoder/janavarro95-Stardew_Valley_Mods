using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewSymphonyRemastered
{
    public static class StaticExtentions
    {

        public static bool isNull<T>(this T obj)
        {
            if (obj == null) return true;
            else return false;
        }
    }
}
