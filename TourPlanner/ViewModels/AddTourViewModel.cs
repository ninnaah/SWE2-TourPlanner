using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class AddTourViewModel : MainViewModel
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

        public ICommand SendAddTourCommand => _sendAddTourCommand ??= new RelayCommand(AddTour);
        public ICommand CloseWinCommand => _closeWinCommand ??= new RelayCommand(CloseWindow);
        
        public string TourName
        {
            get { return _tourName; }

            set
            {
                if (_tourName != value)
                {
                    if(value.Length < 5 || value.Length > 15)
                    {
                        throw new ArgumentException("Tourname should be between 5 and 15 characters long");
                    }

                    _tourName = value;
                    RaisePropertyChangedEvent(nameof(TourName));
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

                    _tourDescription = value;
                    RaisePropertyChangedEvent(nameof(TourDescription));
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

                    _tourFrom = value;
                    RaisePropertyChangedEvent(nameof(TourFrom));
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
                    _tourTo = value;
                    RaisePropertyChangedEvent(nameof(TourTo));
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
                    _tourTransportMode = value;
                    RaisePropertyChangedEvent(nameof(TourTransportMode));
                }
            }
        }


        private void AddTour(object commandParameter)
        {
            if(String.IsNullOrEmpty(_tourName) || String.IsNullOrEmpty(_tourDescription) || String.IsNullOrEmpty(_tourFrom) 
                || String.IsNullOrEmpty(_tourTo) || String.IsNullOrEmpty(_tourTransportMode))
            {
                throw new ArgumentException("Please enter all fields");
            }

            AddedTour?.Invoke(this, new TourItem(_tourName, _tourDescription, _tourFrom, _tourTo, _tourTransportMode));
            
            
        }


        private void CloseWindow (object commandParameter)
        {
            if(commandParameter != null)
            {
                (commandParameter as Window).Close();
            }
        }
    }
}
