using System.Collections.ObjectModel;
using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using OxyPlot;

namespace DNTProfiler.Infrastructure.Models
{
    public class GuiModelBase : BaseViewModel
    {
        private ObservableCollection<AppIdentity> _applicationIdentities;
        private ObservableCollection<Context> _contexts;
        private ObservableCollection<Command> _localCommands;
        private PlotModel _plotModel;
        private ObservableCollection<Command> _relatedCommand;
        private ObservableCollection<CommandConnection> _relatedConnections;
        private ObservableCollection<CallingMethodStackTrace> _relatedStackTraces;
        private ObservableCollection<TrafficUrl> _relatedTrafficUrls;
        private ObservableCollection<TrafficWebRequest> _relatedTrafficWebRequests;
        private ObservableCollection<CommandTransaction> _relatedTransactions;
        private bool _resetSort;
        private AppIdentity _selectedApplicationIdentity;
        private CommandConnection _selectedConnection;
        private Context _selectedContext;
        private Command _selectedExecutedCommand;
        private CallingMethodStackTrace _selectedStackTrace;
        private TrafficUrl _selectedTrafficUrl;
        private TrafficWebRequest _selectedTrafficWebRequest;
        private CommandTransaction _selectedTransaction;

        public GuiModelBase()
        {
            RelatedConnections = new ObservableCollection<CommandConnection>();
            RelatedCommands = new ObservableCollection<Command>();
            RelatedStackTraces = new ObservableCollection<CallingMethodStackTrace>();
            RelatedTrafficUrls = new ObservableCollection<TrafficUrl>();
            RelatedTrafficWebRequests = new ObservableCollection<TrafficWebRequest>();
            ApplicationIdentities = new ObservableCollection<AppIdentity>();
            Contexts = new ObservableCollection<Context>();
            RelatedTransactions = new ObservableCollection<CommandTransaction>();
            LocalCommands = new ObservableCollection<Command>();
            PlotModel = new PlotModel();
        }

        public ObservableCollection<AppIdentity> ApplicationIdentities
        {
            get { return _applicationIdentities; }
            set
            {
                _applicationIdentities = value;
                NotifyPropertyChanged(() => ApplicationIdentities);
            }
        }

        public ObservableCollection<Context> Contexts
        {
            get { return _contexts; }
            set
            {
                _contexts = value;
                NotifyPropertyChanged(() => Contexts);
            }
        }

        public ObservableCollection<Command> LocalCommands
        {
            get { return _localCommands; }
            set
            {
                _localCommands = value;
                NotifyPropertyChanged(() => LocalCommands);
            }
        }

        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set
            {
                _plotModel = value;
                NotifyPropertyChanged(() => PlotModel);
            }
        }

        public ObservableCollection<Command> RelatedCommands
        {
            get { return _relatedCommand; }
            set
            {
                _relatedCommand = value;
                NotifyPropertyChanged(() => RelatedCommands);
            }
        }

        public ObservableCollection<CommandConnection> RelatedConnections
        {
            get { return _relatedConnections; }
            set
            {
                _relatedConnections = value;
                NotifyPropertyChanged(() => RelatedConnections);
            }
        }

        public ObservableCollection<CallingMethodStackTrace> RelatedStackTraces
        {
            get { return _relatedStackTraces; }
            set
            {
                _relatedStackTraces = value;
                NotifyPropertyChanged(() => RelatedStackTraces);
            }
        }

        public ObservableCollection<TrafficUrl> RelatedTrafficUrls
        {
            get { return _relatedTrafficUrls; }
            set
            {
                _relatedTrafficUrls = value;
                NotifyPropertyChanged(() => RelatedTrafficUrls);
            }
        }

        public ObservableCollection<TrafficWebRequest> RelatedTrafficWebRequests
        {
            get { return _relatedTrafficWebRequests; }
            set
            {
                _relatedTrafficWebRequests = value;
                NotifyPropertyChanged(() => RelatedTrafficWebRequests);
            }
        }

        public ObservableCollection<CommandTransaction> RelatedTransactions
        {
            get { return _relatedTransactions; }
            set
            {
                _relatedTransactions = value;
                NotifyPropertyChanged(() => RelatedTransactions);
            }
        }

        public bool ResetSort
        {
            get { return _resetSort; }
            set
            {
                if (_resetSort == value)
                    return;

                _resetSort = value;
                NotifyPropertyChanged(() => ResetSort);
            }
        }

        public AppIdentity SelectedApplicationIdentity
        {
            get
            {
                if (_selectedApplicationIdentity == null)
                    SelectedApplicationIdentity = ApplicationIdentities.FirstOrDefault();
                return _selectedApplicationIdentity;
            }
            set
            {
                if (Equals(_selectedApplicationIdentity, value))
                    return;

                _selectedApplicationIdentity = value;
                NotifyPropertyChanged(() => SelectedApplicationIdentity);
            }
        }

        public CommandConnection SelectedConnection
        {
            get { return _selectedConnection; }
            set
            {
                if (Equals(_selectedConnection, value))
                    return;

                _selectedConnection = value;
                NotifyPropertyChanged(() => SelectedConnection);
            }
        }

        public Context SelectedContext
        {
            get { return _selectedContext; }
            set
            {
                if (Equals(_selectedContext, value))
                    return;

                _selectedContext = value;
                NotifyPropertyChanged(() => SelectedContext);
            }
        }

        public Command SelectedExecutedCommand
        {
            get { return _selectedExecutedCommand; }
            set
            {
                if (Equals(_selectedExecutedCommand, value))
                    return;

                _selectedExecutedCommand = value;
                NotifyPropertyChanged(() => SelectedExecutedCommand);
            }
        }

        public CallingMethodStackTrace SelectedStackTrace
        {
            get { return _selectedStackTrace; }
            set
            {
                if (Equals(_selectedStackTrace, value))
                    return;

                _selectedStackTrace = value;
                NotifyPropertyChanged(() => SelectedStackTrace);
            }
        }

        public TrafficUrl SelectedTrafficUrl
        {
            get { return _selectedTrafficUrl; }
            set
            {
                if (Equals(_selectedTrafficUrl, value))
                    return;

                _selectedTrafficUrl = value;
                NotifyPropertyChanged(() => SelectedTrafficUrl);
            }
        }

        public TrafficWebRequest SelectedTrafficWebRequest
        {
            get { return _selectedTrafficWebRequest; }
            set
            {
                _selectedTrafficWebRequest = value;
                NotifyPropertyChanged(() => SelectedTrafficWebRequest);
            }
        }

        public CommandTransaction SelectedTransaction
        {
            get { return _selectedTransaction; }
            set
            {
                if (Equals(_selectedTransaction, value))
                    return;

                _selectedTransaction = value;
                NotifyPropertyChanged(() => SelectedTransaction);
            }
        }

        public void ClearAll()
        {
            RelatedConnections.Clear();
            RelatedCommands.Clear();
            RelatedStackTraces.Clear();
            RelatedTrafficUrls.Clear();
            RelatedTrafficWebRequests.Clear();
            ApplicationIdentities.Clear();
            Contexts.Clear();
            RelatedTransactions.Clear();
            LocalCommands.Clear();
        }
    }
}