using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore
{
   public static class StaticExtentions
    {

        /// <summary>
        /// Thank you stack overflow. https://stackoverflow.com/questions/3907299/if-statements-matching-multiple-values  
        /// Ex:) if(1.In(1,2,3)) //This returns true since 1 is in the parameter list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Object to pass in value.</param>
        /// <param name="args">A list like (1,2,3) to see if it's contained.</param>
        /// <returns></returns>
        public static bool In<T>(this T obj, params T[] args)
        {
            return args.Contains(obj);
        }

        public static bool HasValue(this double value)
        {
            return !Double.IsNaN(value) && !Double.IsInfinity(value);
        }
    }
}
