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
        public event EventHandler<TourItem> AddedTour;

        public ICommand SendAddTourCommand => _sendAddTourCommand ??= new RelayCommand(AddTour);
        public ICommand CloseWinCommand => _closeWinCommand ??= new RelayCommand(CloseWindow);
        
        public string TourName
        {
            get
            {
                return _tourName;
            }

            set
            {
                if (_tourName != value)
                {
                    _tourName = value;
                    RaisePropertyChangedEvent(nameof(TourName));
                }
            }
        }
        public string TourDescription
        {
            get
            {
                return _tourDescription;
            }

            set
            {
                if (_tourDescription != value)
                {
                    _tourDescription = value;
                    RaisePropertyChangedEvent(nameof(TourDescription));
                }
            }
        }
        public string TourFrom
        {
            get
            {
                return _tourFrom;
            }

            set
            {
                if (_tourFrom != value)
                {
                    _tourFrom = value;
                    RaisePropertyChangedEvent(nameof(TourFrom));
                }
            }
        }
        public string TourTo
        {
            get
            {
                return _tourTo;
            }

            set
            {
                if (_tourTo != value)
                {
                    _tourTo = value;
                    RaisePropertyChangedEvent(nameof(TourTo));
                }
            }
        }


        private void AddTour(object commandParameter)
        {
            AddedTour?.Invoke(this, new TourItem(_tourName, _tourDescription, _tourFrom, _tourTo));
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
