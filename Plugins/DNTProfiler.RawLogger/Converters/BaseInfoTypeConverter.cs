using System;
using System.Globalization;
using System.Windows.Data;
using DNTProfiler.Common.Models;

namespace DNTProfiler.RawLogger.Converters
{
    public class BaseInfoTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";

            var item = value as BaseInfo;
            if (item == null)
                return "";

            switch (item.InfoType)
            {
                case InfoType.Command:
                    var command = (Command)item;
                    return string.Format("Command[{0}]", (command.CommandId == null ? 0 : command.CommandId.Value));
                case InfoType.CommandConnection:
                    var connection = (CommandConnection)item;
                    return string.Format("Connection[{0}] {1}", (connection.ConnectionId == null ? 0 : connection.ConnectionId.Value), connection.Type);
                case InfoType.CommandResult:
                    var commandResult = (CommandResult)item;
                    return string.Format("Result[{0}]", commandResult.CommandId);
                case InfoType.CommandTransaction:
                    var transaction = (CommandTransaction)item;
                    return string.Format("Transaction[{0}] {1}", (item.TransactionId == null ? 0 : item.TransactionId.Value), transaction.TransactionType);
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}