using DNTProfiler.Common.Mvvm;

namespace DNTProfiler.Common.Models
{
    public class StatisticsBase : BaseViewModel
    {
        private CommandStatistics _commandStatistics;
        private ContextStatistics _contextStatistics;

        public StatisticsBase()
        {
            CommandStatistics = new CommandStatistics();
            ContextStatistics = new ContextStatistics();
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

        public ContextStatistics ContextStatistics
        {
            get { return _contextStatistics; }
            set
            {
                _contextStatistics = value;
                NotifyPropertyChanged(() => ContextStatistics);
            }
        }
    }
}