using System;
using System.Collections.Generic;

namespace DNTProfiler.Common.Models
{
    public class CommandConnection : BaseInfo
    {
        private DateTime? _closedAt;
        private int _commandsCount;
        private DateTime? _disposedAt;
        private int _duration;

        public CommandConnection()
        {
            CommandsIds = new SortedSet<int>();
            InfoType = InfoType.CommandConnection;
        }

        public DateTime? ClosedAt
        {
            get { return _closedAt; }
            set
            {
                _closedAt = value;
                NotifyPropertyChanged(() => ClosedAt);
            }
        }

        public int CommandsCount
        {
            get { return _commandsCount; }
            set
            {
                _commandsCount = value;
                NotifyPropertyChanged(() => CommandsCount);
            }
        }

        public SortedSet<int> CommandsIds { set; get; }

        public string ConnectionString { set; get; }

        public DateTime? DisposedAt
        {
            get { return _disposedAt; }
            set
            {
                _disposedAt = value;
                NotifyPropertyChanged(() => DisposedAt);
            }
        }

        public int Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                NotifyPropertyChanged(() => Duration);
            }
        }

        public string Exception { set; get; }

        public bool IsAsync { set; get; }

        public bool IsCanceled { set; get; }

        public CommandConnectionType Type { set; get; }

        public override bool Equals(object obj)
        {
            var commandConnection = obj as CommandConnection;
            if (commandConnection == null)
                return false;

            return this.ConnectionId == commandConnection.ConnectionId &&
                   this.ObjectContextId == commandConnection.ObjectContextId &&
                   this.TransactionId == commandConnection.TransactionId &&
                   this.AtDateTime == commandConnection.AtDateTime &&
                   this.ApplicationIdentity.Equals(commandConnection.ApplicationIdentity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + (ConnectionId.HasValue ? this.ConnectionId.Value.GetHashCode() : 0);
                hash = hash * 23 + (ObjectContextId.HasValue ? this.ObjectContextId.Value.GetHashCode() : 0);
                hash = hash * 23 + (TransactionId.HasValue ? this.TransactionId.Value.GetHashCode() : 0);
                hash = hash * 23 + this.AtDateTime.GetHashCode();
                hash = hash * 23 + this.ApplicationIdentity.AppDomainId.GetHashCode();
                hash = hash * 23 + this.ApplicationIdentity.ProcessId.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Type, ConnectionId);
        }
    }
}