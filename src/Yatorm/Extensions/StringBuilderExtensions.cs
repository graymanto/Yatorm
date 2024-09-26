﻿using System.Text;

namespace Yatorm.Extensions
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Removes the given number of characters from the end of the string builder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="numberToRemove">Number of chars to remove.</param>
        public static void RemoveCharsFromEnd(this StringBuilder builder, int numberToRemove)
        {
            if (builder.Length < numberToRemove)
            {
                throw new ArgumentOutOfRangeException(
                    "numberToRemove",
                    "Builder does not contain enough chars to remove " + numberToRemove
                );
            }

            builder.Remove(builder.Length - 1, numberToRemove);
        }

        /// <summary>
        /// Appends a value to the string builder surrounded by single quotes.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="item"></param>
        public static void AppendInSingleQuotes(this StringBuilder builder, object item)
        {
            builder.AppendFormat("'{0}'", item);
        }
    }
}
