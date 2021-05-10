﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class EditTourLogViewModel : MainViewModel
    {
        private string _currentTourName;

        private ICommand _sendEditTourLogCommand;
        private ICommand _closeWinCommand;
        public Action CloseWin { get; set; }
        private string _tourLogTransportMode;
        private string _tourLogWeather;
        private int _tourLogEffort;
        private string _tourLogReport;
        private int _tourLogRating;
        public event EventHandler<TourLogItem> EditedTourLog;

        public ICommand SendEditTourLogCommand => _sendEditTourLogCommand ??= new RelayCommand(EditTourLog);
        public ICommand CloseWinCommand => _closeWinCommand ??= new RelayCommand(CloseWindow);

        public EditTourLogViewModel(string tourName, TourLogItem currentTourLog)
        {
            _currentTourName = tourName;
            CurrentTourLog = currentTourLog;
            _tourLogRating = CurrentTourLog.Rating;
            _tourLogReport = CurrentTourLog.Report;
        }
        public string TourLogTransportMode
        {
            get
            {
                return _tourLogTransportMode;
            }

            set
            {
                if (_tourLogTransportMode != value)
                {
                    _tourLogTransportMode = value;
                    RaisePropertyChangedEvent(nameof(TourLogTransportMode));
                }
            }
        }

        public string TourLogWeather
        {
            get
            {
                return _tourLogWeather;
            }

            set
            {
                if (_tourLogWeather != value)
                {
                    _tourLogWeather = value;
                    RaisePropertyChangedEvent(nameof(TourLogWeather));
                }
            }
        }
        public int TourLogEffort
        {
            get
            {
                return _tourLogEffort;
            }

            set
            {
                if (_tourLogEffort != value)
                {
                    _tourLogEffort = value;
                    RaisePropertyChangedEvent(nameof(TourLogEffort));
                }
            }
        }
        public string TourLogReport
        {
            get
            {
                return _tourLogReport;
            }

            set
            {
                if (_tourLogReport != value)
                {
                    _tourLogReport = value;
                    RaisePropertyChangedEvent(nameof(TourLogReport));
                }
            }
        }
        public int TourLogRating
        {
            get
            {
                return _tourLogRating;
            }

            set
            {
                if (_tourLogRating != value)
                {
                    _tourLogRating = value;
                    RaisePropertyChangedEvent(nameof(TourLogRating));
                }
            }
        }


        private void EditTourLog(object commandParameter)
        {
            EditedTourLog?.Invoke(this, new TourLogItem(_currentTourName, DateTime.Now, _tourLogTransportMode, _tourLogReport, _tourLogRating, _tourLogWeather, _tourLogEffort));
        }


        private void CloseWindow(object commandParameter)
        {
            if (commandParameter != null)
            {
                (commandParameter as Window).Close();
            }
        }
    }
}
