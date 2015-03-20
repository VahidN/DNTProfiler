namespace DNTProfiler.Common.Models
{
    public class TrafficUrl : StatisticsBase
    {
        private int _numberOfContexts;

        public AppIdentity ApplicationIdentity { set; get; }

        public int NumberOfContexts
        {
            get { return _numberOfContexts; }
            set
            {
                _numberOfContexts = value;
                NotifyPropertyChanged(() => NumberOfContexts);
            }
        }

        public string Url { set; get; }

        public string UrlHash { set; get; }

        public override bool Equals(object obj)
        {
            var trafficUrl = obj as TrafficUrl;
            if (trafficUrl == null)
                return false;

            return this.UrlHash == trafficUrl.UrlHash &&
                   this.ApplicationIdentity.Equals(trafficUrl.ApplicationIdentity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + UrlHash.GetHashCode();
                hash = hash * 23 + this.ApplicationIdentity.AppDomainId.GetHashCode();
                hash = hash * 23 + this.ApplicationIdentity.ProcessId.GetHashCode();
                return hash;
            }
        }
    }
}