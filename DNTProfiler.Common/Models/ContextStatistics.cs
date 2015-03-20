using System.Collections.Generic;
using DNTProfiler.Common.Mvvm;

namespace DNTProfiler.Common.Models
{
    public class ContextStatistics : BaseViewModel
    {
        private int _numberOfCommittedTransactions;
        private int _numberOfConnections;
        private int _numberOfDuplicateQueries;
        private int _numberOfExceptions;
        private int _numberOfQueries;
        private int _numberOfRolledBackTransactions;
        private int _numberOfTransactions;
        private long _totalConnectionOpenTime;
        private int _totalNumberOfRowsReturned;
        private long _totalNumberOfThreads;
        private long _totalQueryExecutionTime;

        public ContextStatistics()
        {
            UsedInThreadIds = new List<int>();
        }

        public int NumberOfCommittedTransactions
        {
            get { return _numberOfCommittedTransactions; }
            set
            {
                _numberOfCommittedTransactions = value;
                NotifyPropertyChanged(() => NumberOfCommittedTransactions);
            }
        }

        public int NumberOfConnections
        {
            get { return _numberOfConnections; }
            set
            {
                _numberOfConnections = value;
                NotifyPropertyChanged(() => NumberOfConnections);
            }
        }

        public int NumberOfDuplicateQueries
        {
            get { return _numberOfDuplicateQueries; }
            set
            {
                _numberOfDuplicateQueries = value;
                NotifyPropertyChanged(() => NumberOfDuplicateQueries);
            }
        }

        public int NumberOfExceptions
        {
            get { return _numberOfExceptions; }
            set
            {
                _numberOfExceptions = value;
                NotifyPropertyChanged(() => NumberOfExceptions);
            }
        }

        public int NumberOfQueries
        {
            get { return _numberOfQueries; }
            set
            {
                _numberOfQueries = value;
                NotifyPropertyChanged(() => NumberOfQueries);
            }
        }
        public int NumberOfRolledBackTransactions
        {
            get { return _numberOfRolledBackTransactions; }
            set
            {
                _numberOfRolledBackTransactions = value;
                NotifyPropertyChanged(() => NumberOfRolledBackTransactions);
            }
        }

        public int NumberOfTransactions
        {
            get { return _numberOfTransactions; }
            set
            {
                _numberOfTransactions = value;
                NotifyPropertyChanged(() => NumberOfTransactions);
            }
        }

        public long TotalConnectionOpenTime
        {
            get { return _totalConnectionOpenTime; }
            set
            {
                _totalConnectionOpenTime = value;
                NotifyPropertyChanged(() => TotalConnectionOpenTime);
            }
        }

        public int TotalNumberOfRowsReturned
        {
            get { return _totalNumberOfRowsReturned; }
            set
            {
                _totalNumberOfRowsReturned = value;
                NotifyPropertyChanged(() => TotalNumberOfRowsReturned);
            }
        }

        public long TotalNumberOfThreads
        {
            get { return _totalNumberOfThreads; }
            set
            {
                if (_totalNumberOfThreads == value)
                    return;

                _totalNumberOfThreads = value;
                NotifyPropertyChanged(() => TotalNumberOfThreads);
            }
        }

        public long TotalQueryExecutionTime
        {
            get { return _totalQueryExecutionTime; }
            set
            {
                _totalQueryExecutionTime = value;
                NotifyPropertyChanged(() => TotalQueryExecutionTime);
            }
        }

        public IList<int> UsedInThreadIds { set; get; }
    }
}