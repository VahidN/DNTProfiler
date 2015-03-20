using System;
using System.Runtime.Serialization;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Common.Threading;

namespace DNTProfiler.Common.Models
{
    public abstract class BaseInfo : BaseViewModel
    {
        private string _jsonContent;

        protected BaseInfo()
        {
            AtDateTime = DateTime.Now;
            ManagedThreadId = DispatcherHelper.GetManagedThreadId();
            ApplicationIdentity = new AppIdentity();
            HttpInfo = new HttpInfo();
            AppDomainSnapshot = new AppDomainMonitorSnapshot();
        }

        public AppDomainMonitorSnapshot AppDomainSnapshot { set; get; }

        public AppIdentity ApplicationIdentity { set; get; }

        public DateTime AtDateTime { set; get; }

        public int? ConnectionId { set; get; }

        public HttpInfo HttpInfo { set; get; }

        public InfoType InfoType { set; get; }

        [IgnoreDataMember]
        public string JsonContent
        {
            get { return _jsonContent; }
            set
            {
                _jsonContent = value;
                NotifyPropertyChanged(() => JsonContent);
            }
        }

        public int ManagedThreadId { set; get; }

        public string ObjectContextName { set; get; }

        public int? ObjectContextId { set; get; }

        public int ReceivedId { set; get; }

        public CallingMethodStackTrace StackTrace { set; get; }

        public int? TransactionId { set; get; }
    }
}