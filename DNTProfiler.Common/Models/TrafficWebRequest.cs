using System.Collections.ObjectModel;
using DNTProfiler.Common.Mvvm;

namespace DNTProfiler.Common.Models
{
    public class TrafficWebRequest : BaseViewModel
    {
        public TrafficWebRequest()
        {
            Contexts = new ObservableCollection<Context>();
        }

        public AppIdentity ApplicationIdentity { set; get; }

        public ObservableCollection<Context> Contexts { set; get; }

        public int HttpContextCurrentId { set; get; }

        public override bool Equals(object obj)
        {
            var trafficWebRequest = obj as TrafficWebRequest;
            if (trafficWebRequest == null)
                return false;

            return this.HttpContextCurrentId == trafficWebRequest.HttpContextCurrentId &&
                this.ApplicationIdentity.Equals(trafficWebRequest.ApplicationIdentity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + HttpContextCurrentId.GetHashCode();
                hash = hash * 23 + this.ApplicationIdentity.AppDomainId.GetHashCode();
                hash = hash * 23 + this.ApplicationIdentity.ProcessId.GetHashCode();
                return hash;
            }
        }
    }
}