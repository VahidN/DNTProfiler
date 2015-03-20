using System.Collections.Generic;
using System.Data;

namespace DNTProfiler.Common.Models
{
    public class Command : BaseInfo
    {
        private long? _commandCpuUsage;
        private long? _commandMemoryUsage;
        private CommandStatistics _commandStatistics;
        private long? _elapsedMilliseconds;
        private int? _fieldsCount;
        private bool _isCanceled;
        private string _resultException;
        private int? _rowsReturned;
        private string _sql;

        public Command()
        {
            Parameters = new List<CommandParameter>();
            CommandStatistics = new CommandStatistics();
            InfoType = InfoType.Command;
        }

        public long? CommandCpuUsage
        {
            get { return _commandCpuUsage; }
            set
            {
                _commandCpuUsage = value;
                NotifyPropertyChanged(() => CommandCpuUsage);
            }
        }

        public int? CommandId { set; get; }

        public long? CommandMemoryUsage
        {
            get { return _commandMemoryUsage; }
            set
            {
                _commandMemoryUsage = value;
                NotifyPropertyChanged(() => CommandMemoryUsage);
            }
        }

        public CommandStatistics CommandStatistics
        {
            get { return _commandStatistics; }
            set
            {
                _commandStatistics = value;
                NotifyPropertyChanged(() => CommandStatistics);
            }
        }

        public long? ElapsedMilliseconds
        {
            get { return _elapsedMilliseconds; }
            set
            {
                _elapsedMilliseconds = value;
                NotifyPropertyChanged(() => ElapsedMilliseconds);
            }
        }

        public int? FieldsCount
        {
            get { return _fieldsCount; }
            set
            {
                _fieldsCount = value;
                NotifyPropertyChanged(() => FieldsCount);
            }
        }

        public bool IsAsync { set; get; }

        public bool IsCanceled
        {
            get { return _isCanceled; }
            set
            {
                _isCanceled = value;
                NotifyPropertyChanged(() => IsCanceled);
            }
        }

        public IsolationLevel? IsolationLevel { set; get; }

        public IList<CommandParameter> Parameters { set; get; }

        public string ResultException
        {
            get { return _resultException; }
            set
            {
                _resultException = value;
                NotifyPropertyChanged(() => ResultException);
            }
        }

        public int? RowsReturned
        {
            get { return _rowsReturned; }
            set
            {
                _rowsReturned = value;
                NotifyPropertyChanged(() => RowsReturned);
            }
        }

        public string Sql
        {
            get { return _sql; }
            set
            {
                _sql = value;
                NotifyPropertyChanged(() => Sql);
            }
        }

        public string SqlHash { set; get; }

        public override bool Equals(object obj)
        {
            var command = obj as Command;
            if (command == null)
                return false;

            return this.ConnectionId == command.ConnectionId &&
                   this.ObjectContextId == command.ObjectContextId &&
                   this.TransactionId == command.TransactionId &&
                   this.CommandId == command.CommandId &&
                   this.AtDateTime == command.AtDateTime &&
                   this.ApplicationIdentity.Equals(command.ApplicationIdentity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + (ConnectionId.HasValue ? this.ConnectionId.Value.GetHashCode() : 0);
                hash = hash * 23 + (ObjectContextId.HasValue ? this.ObjectContextId.Value.GetHashCode() : 0);
                hash = hash * 23 + (TransactionId.HasValue ? this.TransactionId.Value.GetHashCode() : 0);
                hash = hash * 23 + (CommandId.HasValue ? this.CommandId.Value.GetHashCode() : 0);
                hash = hash * 23 + this.AtDateTime.GetHashCode();
                hash = hash * 23 + this.ApplicationIdentity.AppDomainId.GetHashCode();
                hash = hash * 23 + this.ApplicationIdentity.ProcessId.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return Sql;
        }
    }
}