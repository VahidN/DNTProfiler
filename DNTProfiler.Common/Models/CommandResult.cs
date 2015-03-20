namespace DNTProfiler.Common.Models
{
    public class CommandResult : BaseInfo
    {
        public CommandResult()
        {
            InfoType = InfoType.CommandResult;
        }

        public int? CommandId { set; get; }

        public long? ElapsedMilliseconds { set; get; }

        public string Exception { set; get; }

        public int? FieldsCount { set; get; }

        public bool IsCanceled { set; get; }

        public string ResultString { set; get; }

        public int? RowsReturned { set; get; }

        public override string ToString()
        {
            return ResultString;
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

        public override bool Equals(object obj)
        {
            var commandResult = obj as CommandResult;
            if (commandResult == null)
                return false;

            return this.ConnectionId == commandResult.ConnectionId &&
                   this.ObjectContextId == commandResult.ObjectContextId &&
                   this.TransactionId == commandResult.TransactionId &&
                   this.CommandId == commandResult.CommandId &&
                   this.AtDateTime == commandResult.AtDateTime &&
                   this.ApplicationIdentity.Equals(commandResult.ApplicationIdentity);
        }
    }
}