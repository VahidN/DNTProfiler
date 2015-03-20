using DNTProfiler.Common.Mvvm;

namespace DNTProfiler.Common.Models
{
    public class AppIdentity : BaseViewModel
    {
        private int _notificationsCount;

        public AppIdentity()
        {
            ProcessId = AppMessenger.ProcessId;
            ProcessName = AppMessenger.ProcessName;
            AppDomainId = AppMessenger.AppDomainId;
            AppDomainName = AppMessenger.AppDomainName;
        }

        public AppIdentity(AppIdentity appIdentity)
        {
            this.AppDomainId = appIdentity.AppDomainId;
            this.AppDomainName = appIdentity.AppDomainName;
            this.ProcessId = appIdentity.ProcessId;
            this.ProcessName = appIdentity.ProcessName;
        }

        public int AppDomainId { set; get; }

        public string AppDomainName { set; get; }

        public int NotificationsCount
        {
            get { return _notificationsCount; }
            set
            {
                _notificationsCount = value;
                NotifyPropertyChanged(() => NotificationsCount);
            }
        }

        public int ProcessId { set; get; }

        public string ProcessName { set; get; }

        public override bool Equals(object obj)
        {
            var applicationIdentity = obj as AppIdentity;
            if (applicationIdentity == null)
                return false;

            return this.AppDomainId == applicationIdentity.AppDomainId &&
                this.ProcessId == applicationIdentity.ProcessId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + AppDomainId.GetHashCode();
                hash = hash * 23 + ProcessId.GetHashCode();
                return hash;
            }
        }
    }
}