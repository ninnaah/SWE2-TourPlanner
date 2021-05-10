using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.Views;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ITourItemFactory _tourFactory;
        private ICommand _searchCommand;
        private ICommand _deleteCommand;
        private ICommand _printTourReportCommand;
        private ICommand _printSummarizeReportCommand;
        private TourItem _currentTour;
        private string _searchTour;
        private ICommand _openAddTourWinCommand;
        private ICommand _openEditTourWinCommand;

        private ICommand _openAddTourLogWinCommand;
        private ICommand _openEditTourLogWinCommand;
        private ICommand _deleteLogCommand;
        private TourLogItem _currentTourLog;

        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(Search);
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(Delete);
        public ICommand PrintTourReportCommand => _printTourReportCommand ??= new RelayCommand(PrintTourReport);
        public ICommand PrintSummarizeReportCommand => _printSummarizeReportCommand ??= new RelayCommand(PrintSummarizeReport);
        public ICommand OpenAddTourWinCommand => _openAddTourWinCommand ??= new RelayCommand(Add);
        public ICommand OpenEditTourWinCommand => _openEditTourWinCommand ??= new RelayCommand(Edit);


        public ICommand OpenAddTourLogWinCommand => _openAddTourLogWinCommand ??= new RelayCommand(AddLog);
        public ICommand DeleteLogCommand => _deleteLogCommand ??= new RelayCommand(DeleteLog);
        public ICommand OpenEditTourLogWinCommand => _openEditTourLogWinCommand ??= new RelayCommand(EditLog);


        public ObservableCollection<TourItem> Tours { get; set; }
        public ObservableCollection<TourLogItem> TourLogs { get; set; }
        

        public TourItem CurrentTour 
        {
            get
            {
                return _currentTour;
            }

            set
            {
                if((_currentTour != value) && (value != null))
                {
                    _currentTour = value;
                    CreateList();
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                }
            }
        }

        public TourLogItem CurrentTourLog
        {
            get
            {
                return _currentTourLog;
            }

            set
            {
                if ((_currentTourLog != value) && (value != null))
                {
                    _currentTourLog = value;
                    RaisePropertyChangedEvent(nameof(CurrentTourLog));
                }
            }
        }

        public string CurrentMap
        {
            get
            {
                if (CurrentTour != null)
                {
                    return Path.GetFullPath($"../../../../tours/maps/{CurrentTour.Name}.png");
                }
                return null;
                
            }
        }

        public string SearchTour
        {
            get
            {
                return _searchTour;
            }

            set
            {
                if (_searchTour != value)
                {
                    _searchTour = value;
                    RaisePropertyChangedEvent(nameof(SearchTour));
                }
            }
        }

        public MainViewModel()
        {
            _tourFactory = TourItemFactory.GetInstance();
            CreateList();
        }

        protected void CreateList()
        {
            Tours = new ObservableCollection<TourItem>();

            foreach (TourItem item in this._tourFactory.GetTours())
            {
                Tours.Add(item);
            }

            if (CurrentTour != null)
            {
                TourLogs = new ObservableCollection<TourLogItem>();
                foreach (TourLogItem item in this._tourFactory.GetTourLogsForTour(CurrentTour.Name))
                {
                    TourLogs.Add(item);
                }
                RaisePropertyChangedEvent(nameof(TourLogs));
                RaisePropertyChangedEvent(nameof(CurrentMap));
            }
        }

        //TOURS
        private void Search(object commandParameter)
        {
            IEnumerable foundTours = this._tourFactory.Search(SearchTour);
            Tours.Clear();
            foreach (TourItem item in foundTours)
            {
                Tours.Add(item);
            }
        }
        private void Delete(object commandParameter)
        {
            if(CurrentTour!= null)
            {
                _tourFactory.DeleteTour(CurrentTour);
                Tours.Remove(CurrentTour);
            }
            CreateList();
            RaisePropertyChangedEvent(nameof(Tours));
        }
        private void Add(object commandParameter)
        {
            AddTourWindow addTourWindow = new AddTourWindow();
            AddTourViewModel addTourVM = new AddTourViewModel();

            addTourVM.AddedTour += (_, tour) => { AddTour(tour); };
            addTourWindow.DataContext = addTourVM;

            addTourWindow.ShowDialog();
        }
        private void AddTour(TourItem tour)
        {
            _tourFactory.AddTour(tour);
            Tours.Add(tour);

            RaisePropertyChangedEvent(nameof(Tours));
        }
        private void Edit(object commandParameter)
        {
            if(CurrentTour != null) 
            {
                EditTourWindow editTourWindow = new EditTourWindow();
                EditTourViewModel editTourVM = new EditTourViewModel(CurrentTour);

                editTourVM.EditedTour += (_, tour) => { EditTour(tour); };
                editTourWindow.DataContext = editTourVM;

                editTourWindow.ShowDialog();
            }
        }
        private void EditTour(TourItem tour)
        {
            _tourFactory.DeleteTour(CurrentTour);
            _tourFactory.AddTour(tour);

            Tours.Remove(CurrentTour);
            Tours.Add(tour);

            RaisePropertyChangedEvent(nameof(Tours));
            CreateList();
        }
        private void PrintTourReport(object commandParameter)
        {
            if (CurrentTour != null)
            {
                _tourFactory.CreateTourReport(CurrentTour);
            }
        }
        private void PrintSummarizeReport(object commandParameter)
        {
            _tourFactory.CreateSummarizeReport();
        }


        //LOGS
        private void AddLog(object commandParameter)
        {
            AddTourLogWindow addTourLogWindow = new AddTourLogWindow();
            AddTourLogViewModel addTourLogVM = new AddTourLogViewModel(CurrentTour.Name);

            addTourLogVM.AddedTourLog += (_, tourLog) => { AddTourLog(tourLog); };
            addTourLogWindow.DataContext = addTourLogVM;

            addTourLogWindow.ShowDialog();
        }
        private void AddTourLog(TourLogItem tourLog)
        {
            TourItem tourForLog = new TourItem();
            foreach(TourItem tour in Tours)
            {
                if(tour.Name == tourLog.TourName)
                {
                    tourForLog = tour;
                }
            }

            _tourFactory.AddTourLog(tourLog, tourForLog);

            TourLogs.Add(tourLog);

            RaisePropertyChangedEvent(nameof(TourLogs));
        }
        private void DeleteLog(object commandParameter)
        {
            if (CurrentTourLog != null)
            {
                TourLogs.Remove(CurrentTourLog);
                _tourFactory.DeleteTourLog(CurrentTourLog);
            }
        }
        private void EditLog(object commandParameter)
        {
            if (CurrentTourLog != null)
            {
                EditTourLogWindow editTourLogWindow = new EditTourLogWindow();
                EditTourLogViewModel editTourLogVM = new EditTourLogViewModel(CurrentTour.Name, CurrentTourLog);

                editTourLogVM.EditedTourLog += (_, tourLog) => { EditTourLog(tourLog); };
                editTourLogWindow.DataContext = editTourLogVM;

                editTourLogWindow.ShowDialog();
            }
        }
        private void EditTourLog(TourLogItem tourLog)
        {
            TourItem tourForLog = new TourItem();
            foreach (TourItem tour in Tours)
            {
                if (tour.Name == tourLog.TourName)
                {
                    tourForLog = tour;
                }
            }

            _tourFactory.DeleteTourLog(CurrentTourLog);
            _tourFactory.AddTourLog(tourLog, tourForLog);

            TourLogs.Add(tourLog);
            TourLogs.Remove(CurrentTourLog);

            RaisePropertyChangedEvent(nameof(TourLogs));
        }

    }




}
