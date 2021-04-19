using System;
using System.Collections.Generic;
using System.Linq;

namespace VertexCover.Extensions
{
    public static class IEnumerableExtension
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Check if an IEnumerable is empty
        /// </summary>
        /// <typeparam name="T">The type of IEnumerable</typeparam>
        /// <param name="enumerable">The enumerable values</param>
        /// <returns>True if the enumerable is empty or null. False if it is not empty</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        /// <summary>
        /// Get a random element from the IEnumerable
        /// </summary>
        /// <typeparam name="T">'The type you want and enumerable for</typeparam>
        /// <param name="enumerable">The enumerable you want a value from</param>
        /// <returns>A random element from the enumerable</returns>
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ElementAt(new Random().Next(enumerable.Count()));
        }
    }
}
