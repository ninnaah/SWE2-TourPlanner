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
        private float _tourLogDuration;
        private float _tourLogDistance;
        private string _tourLogReport;
        private int _tourLogRating;
        public event EventHandler<TourLogItem> EditedTourLog;

        public ICommand SendEditTourLogCommand => _sendEditTourLogCommand ??= new RelayCommand(EditTourLog);
        public ICommand CloseWinCommand => _closeWinCommand ??= new RelayCommand(CloseWindow);

        public EditTourLogViewModel(string tourName, TourLogItem currentTourLog)
        {
            _currentTourName = tourName;
            CurrentTourLog = currentTourLog;
        }

        public float TourLogDuration
        {
            get
            {
                return _tourLogDuration;
            }

            set
            {
                if (_tourLogDuration != value)
                {
                    _tourLogDuration = value;
                    RaisePropertyChangedEvent(nameof(TourLogDuration));
                }
            }
        }
        public float TourLogDistance
        {
            get
            {
                return _tourLogDistance;
            }

            set
            {
                if (_tourLogDistance != value)
                {
                    _tourLogDistance = value;
                    RaisePropertyChangedEvent(nameof(TourLogDistance));
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
            EditedTourLog?.Invoke(this, new TourLogItem(_currentTourName, DateTime.Now, _tourLogDuration, _tourLogDistance, _tourLogReport, _tourLogRating));
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
