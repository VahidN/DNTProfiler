using System;

namespace DNTProfiler.Common.Toolkit
{
    public static class Guard
    {
        public static int ToInt(this string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return 0;

            int result;
            return int.TryParse(data, out result) ? result : 0;
        }

        public static bool ApproxEquals(this double d1, double d2)
        {
            const double epsilon = 2.2204460492503131E-16;

            if (d1 == d2)
                return true;

            var tolerance = ((Math.Abs(d1) + Math.Abs(d2)) + 10.0) * epsilon;
            var difference = d1 - d2;

            return (-tolerance < difference && tolerance > difference);
        }

        public static int StringSize(this string value)
        {
            return !string.IsNullOrWhiteSpace(value) ? value.Length * sizeof(char) : 0;
        }
    }
}