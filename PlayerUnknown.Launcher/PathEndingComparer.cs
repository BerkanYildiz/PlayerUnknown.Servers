namespace PlayerUnknown.Launcher
{
    using System.Collections.Generic;

    public class PathEndingComparer : IEqualityComparer<string>
    {
        /// <summary>
        /// Determines if one value is equal to another.
        /// </summary>
        /// <param name="Value1">The first value.</param>
        /// <param name="Value2">The second value.</param>
        public bool Equals(string Value1, string Value2)
        {
            if (Value1.EndsWith(Value2))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="Object">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(string Object)
        {
            if (Object.Length > 0)
            {
                return 89 * Object[0] + Object.Length + Object[Object.Length - 1] + Object.Length;
            }

            return 0;
        }
    }
}
