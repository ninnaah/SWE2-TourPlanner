using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ITourItemFactory _tourFactory;
        private ICommand _searchCommand;
        private TourItem _currentTour;
        private string _searchTour;

        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(Search);
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

        private void CreateList()
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

    }




}
