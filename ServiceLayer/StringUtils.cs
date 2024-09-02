// <copyright file="StringUtils.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace ServiceLayer
{
    using System;

    /// <summary>
    /// Provides utility methods for string manipulation.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Calculates the distance between two strings.
        /// </summary>
        /// <param name="source">The first string to compare.</param>
        /// <param name="target">The second string to compare.</param>
        /// <returns>
        /// The distance between the <paramref name="source"/> and <paramref name="target"/> strings.
        /// The distance is a measure of the minimum number of single-character edits (insertions, deletions, or substitutions)
        /// required to change one string into the other.
        /// </returns>
        /// <remarks>
        /// This implementation uses a dynamic programming approach to calculate the distance.
        /// If either <paramref name="source"/> or <paramref name="target"/> is null or empty, it will return the length of the other string,
        /// as this represents the number of edits required to convert an empty string to the non-empty one (or vice versa).
        /// </remarks>
        public static int CalculateLevenshteinDistance(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.IsNullOrEmpty(target) ? 0 : target.Length;
            }

            if (string.IsNullOrEmpty(target))
            {
                return source.Length;
            }

            int[,] d = new int[source.Length + 1, target.Length + 1];

            for (int i = 0; i <= source.Length; i++)
            {
                d[i, 0] = i;
            }

            for (int j = 0; j <= target.Length; j++)
            {
                d[0, j] = j;
            }

            for (int i = 1; i <= source.Length; i++)
            {
                for (int j = 1; j <= target.Length; j++)
                {
                    int cost = target[j - 1] == source[i - 1] ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[source.Length, target.Length];
        }
    }
}