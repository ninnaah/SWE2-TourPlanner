using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class AddTourViewModel : ViewModelBase
    {
        private ICommand _sendAddTourCommand;
        private ICommand _closeWinCommand;
        public Action CloseWin { get; set; }
        private string _tourName;
        private string _tourDescription;
        private string _tourFrom;
        private string _tourTo;
        private string _tourTransportMode;
        public event EventHandler<TourItem> AddedTour;

        public ObservableCollection<TourItem> Tours { get; set; }

        public ICommand SendAddTourCommand => _sendAddTourCommand ??= new RelayCommand(AddTour);
        public ICommand CloseWinCommand => _closeWinCommand ??= new RelayCommand(CloseWindow);

        public AddTourViewModel(ObservableCollection<TourItem> tours)
        {
            this.Tours = tours;
        }

        public string TourName
        {
            get { return _tourName; }

            set
            {
                if (_tourName != value)
                {
                    if(CheckIfTourNameExists(value) == false)
                    {
                        throw new ArgumentException("Tourname should be unique");
                    }
                    else if(value.Length < 5 || value.Length > 15)
                    {
                        throw new ArgumentException("Tourname should be between 5 and 15 characters long");
                    }
                    else
                    {
                        _tourName = value;
                        RaisePropertyChangedEvent(nameof(TourName));
                    }
                }
            }
        }
        public string TourDescription
        {
            get { return _tourDescription; }

            set
            {
                if (_tourDescription != value)
                {
                    if (value.Length < 5 || value.Length > 50)
                    {
                        throw new ArgumentException("Description should be between 5 and 50 characters long");
                    }
                    else
                    {
                        _tourDescription = value;
                        RaisePropertyChangedEvent(nameof(TourDescription));
                    }

                }
            }
        }
        public string TourFrom
        {
            get { return _tourFrom; }

            set
            {
                if (_tourFrom != value)
                {
                    if (value.Length < 3 || value.Length > 20)
                    {
                        throw new ArgumentException("Starting point should be between 3 and 20 characters long");
                    }
                    else
                    {
                         _tourFrom = value;
                        RaisePropertyChangedEvent(nameof(TourFrom));
                    }
                }
            }
        }
        public string TourTo
        {
            get { return _tourTo; }

            set
            {
                if (_tourTo != value)
                {
                    if (value.Length < 3 || value.Length > 20)
                    {
                        throw new ArgumentException("End point should be between 3 and 20 characters long");
                    }
                    else
                    {
                        _tourTo = value;
                        RaisePropertyChangedEvent(nameof(TourTo));
                    }
                }
            }
        }

        public string TourTransportMode
        {
            get { return _tourTransportMode; }

            set
            {
                if (_tourTransportMode != value)
                {
                    if (String.IsNullOrEmpty(value))
                    {
                        throw new ArgumentException("Please choose transport mode");
                    }
                    else
                    {
                        _tourTransportMode = value;
                        RaisePropertyChangedEvent(nameof(TourTransportMode));
                    }
                    
                }
            }
        }


        private void AddTour(object commandParameter)
        {
            if(!String.IsNullOrEmpty(_tourName) && !String.IsNullOrEmpty(_tourDescription) && !String.IsNullOrEmpty(_tourFrom) 
                && !String.IsNullOrEmpty(_tourTo) && !String.IsNullOrEmpty(_tourTransportMode))
            {
                AddedTour?.Invoke(this, new TourItem(_tourName, _tourDescription, _tourFrom, _tourTo, _tourTransportMode));
            }
            else
            {
                throw new ArgumentException("Please fill in all fields");
            }
        }


        private void CloseWindow (object commandParameter)
        {
            if(commandParameter != null)
            {
                (commandParameter as Window).Close();
            }
        }

        public bool CheckIfTourNameExists(string tourname)
        {
            foreach (TourItem tour in Tours)
            {
                if (tourname == tour.Name)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
