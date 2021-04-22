﻿using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
                Tours.Remove(CurrentTour);
                _tourFactory.DeleteTour(CurrentTour);
            }

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

            Tours.Add(tour);
            Tours.Remove(CurrentTour);
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
            _tourFactory.AddTourLog(tourLog);

            TourLogs.Add(tourLog);
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
            _tourFactory.DeleteTourLog(CurrentTourLog);
            _tourFactory.AddTourLog(tourLog);

            TourLogs.Add(tourLog);
            TourLogs.Remove(CurrentTourLog);
        }

    }




}
