using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.Revitalize.Framework.Utilities.Extensions
{
    public static class RandomExtensions
    {

        /// <summary>
        /// Returns a random element from the given enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this IEnumerable<T> list) where T : class
        {
            if (list.Count() == 0) return default(T);
            if (list.Count() == 1) return list.First();
            int randPosition = Game1.random.Next(0, list.Count());
            return list.ElementAt(randPosition);
        }

        /// <summary>
        /// Returns a random element from the given collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this ICollection<T> list) where T : class
        {
            if (list.Count() == 0) return default(T);
            if (list.Count() == 1) return list.First();
            int randPosition = Game1.random.Next(0, list.Count());
            return list.ElementAt(randPosition);
        }


        /// <summary>
        /// Returns a random element from the given list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this List<T> list) where T : class
        {
            if (list.Count() == 0) return default(T);
            if (list.Count() == 1) return list.First();
            int randPosition = Game1.random.Next(0, list.Count());
            return list.ElementAt(randPosition);
        }

    }
}
