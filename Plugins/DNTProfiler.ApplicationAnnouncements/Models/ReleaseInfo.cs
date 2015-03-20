using System;

namespace DNTProfiler.ApplicationAnnouncements.Models
{
    public class ReleaseInfo
    {
        public string ReleaseHtmlUrl { get; set; }

        public string ReleaseTagName { get; set; }

        public DateTime AssetCreatedAt { get; set; }

        public string AssetName { get; set; }

        public int AssetSize { get; set; }

        public int AssetDownloadCount { get; set; }
    }
}