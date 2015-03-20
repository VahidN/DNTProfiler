using System;

namespace DNTProfiler.Common.Toolkit
{
    public static class Hash
    {
        /// <summary>
        /// Computes the unique hash of the input.
        /// </summary>
        /// <param name="data">the input data to hash</param>
        /// <returns>Hashed data using the xxHash algorithm</returns>
        public static string ComputeHash(this string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("data");

            return string.Format("{0:X}", XxHashUnsafe.ComputeHash(data));
        }
    }
}