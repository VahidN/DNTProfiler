using System;

namespace DNTProfiler.Common.Models
{
    public class Context : StatisticsBase
    {
        private DateTime? _disposedAt;

        public AppIdentity ApplicationIdentity { set; get; }

        public DateTime AtDateTime { set; get; }

        public DateTime? DisposedAt
        {
            get { return _disposedAt; }
            set
            {
                _disposedAt = value;
                NotifyPropertyChanged(() => DisposedAt);
            }
        }

        public int? HttpContextCurrentId { set; get; }

        public int ManagedThreadId { set; get; }

        public string ObjectContextName { set; get; }

        public int? ObjectContextId { set; get; }

        public string Url { set; get; }

        public override bool Equals(object obj)
        {
            var context = obj as Context;
            if (context == null)
                return false;

            return this.ObjectContextId == context.ObjectContextId &&
                this.ApplicationIdentity.Equals(context.ApplicationIdentity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + (ObjectContextId.HasValue ? this.ObjectContextId.Value.GetHashCode() : 0);
                hash = hash * 23 + this.ApplicationIdentity.AppDomainId.GetHashCode();
                hash = hash * 23 + this.ApplicationIdentity.ProcessId.GetHashCode();
                return hash;
            }
        }
    }
}