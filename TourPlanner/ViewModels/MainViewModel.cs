using System;
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

        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(Search);
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(Delete);
        public ICommand OpenAddTourWinCommand => _openAddTourWinCommand ??= new RelayCommand(Add);
        public ICommand OpenEditTourWinCommand => _openEditTourWinCommand ??= new RelayCommand(Edit);
        public ObservableCollection<TourItem> Tours { get; set; }
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
                    RaisePropertyChangedEvent(nameof(CurrentTour));
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
        }

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

        private void Edit(object commandParameter)
        {
            if(CurrentTour != null) 
            {
                EditTourWindow editTourWindow = new EditTourWindow();
                AddTourViewModel addTourVM = new AddTourViewModel();

                addTourVM.AddedTour += (_, tour) => { EditTour(tour); };
                editTourWindow.DataContext = addTourVM;


                editTourWindow.ShowDialog();
            }
        }

        private void AddTour(TourItem tour)
        {
            _tourFactory.AddTour(tour);

            Tours.Add(tour);
        }

        private void EditTour(TourItem tour)
        {
            _tourFactory.DeleteTour(CurrentTour);
            _tourFactory.AddTour(tour);

            Tours.Add(tour);
        }

    }




}
