using System.Collections.Generic;
using DNTProfiler.Common.Converters;
using DNTProfiler.Common.JsonToolkit;

namespace DNTProfiler.Common.ClipboardUtils
{
    public static class ListToClipboard
    {
        public static string ConvertToJsonString(this IEnumerable<object> rows)
        {
            return rows.ToFormattedJson();
        }

        public static string ConvertToJsonString(this object row)
        {
            return ConvertToJsonString(new List<object> { row });
        }

        public static void CopyToClipboard(this object row)
        {
            CopyToClipboard(new List<object> { row });
        }

        public static void CopyToClipboard(this IEnumerable<object> rows)
        {
            ConvertToJsonString(rows).ClipboardSetText();
        }
    }
}