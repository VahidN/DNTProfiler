using System.Collections.ObjectModel;

namespace DNTProfiler.Common.Models
{
    public class ProfilerData
    {
        public ProfilerData()
        {
            Transactions = new ObservableCollection<CommandTransaction>();
            Results = new ObservableCollection<CommandResult>();
            Commands = new ObservableCollection<Command>();
            Connections = new ObservableCollection<CommandConnection>();
            Contexts = new ObservableCollection<Context>();
            StackTraces = new ObservableCollection<CallingMethodStackTrace>();
            TrafficUrls = new ObservableCollection<TrafficUrl>();
            TrafficWebRequests = new ObservableCollection<TrafficWebRequest>();
            ApplicationIdentities = new ObservableCollection<AppIdentity>();
        }

        public ObservableCollection<AppIdentity> ApplicationIdentities { set; get; }

        public ObservableCollection<Command> Commands { set; get; }

        public ObservableCollection<CommandConnection> Connections { set; get; }

        public ObservableCollection<Context> Contexts { set; get; }

        public ObservableCollection<CommandResult> Results { set; get; }

        public ObservableCollection<CallingMethodStackTrace> StackTraces { set; get; }

        public ObservableCollection<TrafficUrl> TrafficUrls { set; get; }

        public ObservableCollection<TrafficWebRequest> TrafficWebRequests { set; get; }

        public ObservableCollection<CommandTransaction> Transactions { set; get; }

        public void ClearAll()
        {
            Transactions.Clear();
            Results.Clear();
            Commands.Clear();
            Connections.Clear();
            Contexts.Clear();
            StackTraces.Clear();
            TrafficUrls.Clear();
            TrafficWebRequests.Clear();
            ApplicationIdentities.Clear();
        }
    }
}