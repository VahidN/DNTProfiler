using System;
using System.Collections.Generic;
using System.Data;

namespace DNTProfiler.Common.Models
{
    public class CommandTransaction : BaseInfo
    {
        private DateTime? _closedAt;
        private int _commandsCount;
        private DateTime? _disposedAt;
        private DateTime? _rolledBackAt;

        public CommandTransaction()
        {
            InfoType = InfoType.CommandTransaction;
            CommandsIds = new SortedSet<int>();
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

        public DateTime? DisposedAt
        {
            get { return _disposedAt; }
            set
            {
                _disposedAt = value;
                NotifyPropertyChanged(() => DisposedAt);
            }
        }

        public string Exception { set; get; }

        public bool IsAsync { set; get; }

        public bool IsCanceled { set; get; }

        public IsolationLevel? IsolationLevel { set; get; }

        public DateTime? RolledBackAt
        {
            get { return _rolledBackAt; }
            set
            {
                _rolledBackAt = value;
                NotifyPropertyChanged(() => RolledBackAt);
            }
        }

        public CommandTransactionType TransactionType { set; get; }

        public override bool Equals(object obj)
        {
            var commandTransaction = obj as CommandTransaction;
            if (commandTransaction == null)
                return false;

            return this.ConnectionId == commandTransaction.ConnectionId &&
                   this.ObjectContextId == commandTransaction.ObjectContextId &&
                   this.TransactionId == commandTransaction.TransactionId &&
                   this.TransactionType == commandTransaction.TransactionType &&
                   this.AtDateTime == commandTransaction.AtDateTime &&
                   this.ApplicationIdentity.Equals(commandTransaction.ApplicationIdentity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + (ConnectionId.HasValue ? this.ConnectionId.Value.GetHashCode() : 0);
                hash = hash * 23 + (ObjectContextId.HasValue ? this.ObjectContextId.Value.GetHashCode() : 0);
                hash = hash * 23 + (TransactionId.HasValue ? this.TransactionId.Value.GetHashCode() : 0);
                hash = hash * 23 + this.TransactionType.GetHashCode();
                hash = hash * 23 + this.AtDateTime.GetHashCode();
                hash = hash * 23 + this.ApplicationIdentity.AppDomainId.GetHashCode();
                hash = hash * 23 + this.ApplicationIdentity.ProcessId.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return TransactionType.ToString();
        }
    }
}