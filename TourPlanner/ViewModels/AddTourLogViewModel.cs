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
    public class AddTourLogViewModel : MainViewModel
    {
        private TourItem _currentTour;

        private ICommand _sendAddTourLogCommand;
        private ICommand _closeWinCommand;
        public Action CloseWin { get; set; }
        private float _tourLogDistance;
        private float _tourLogDuration;
        private float _tourLogFuelUsed;
        private string _tourLogWeather;
        private float _tourLogTemperature;
        private int _tourLogEffort;
        private string _tourLogReport;
        private int _tourLogRating;
        public event EventHandler<TourLogItem> AddedTourLog;

        public ICommand SendAddTourLogCommand => _sendAddTourLogCommand ??= new RelayCommand(AddTourLog);
        public ICommand CloseWinCommand => _closeWinCommand ??= new RelayCommand(CloseWindow);

        public AddTourLogViewModel(TourItem currentTour)
        {
            _currentTour = currentTour;
        }

        public float TourLogDistance
        {
            get { return _tourLogDistance; }

            set
            {
                if (_tourLogDistance != value)
                {
                    if (value < (_currentTour.Distance - 50)  || value > (_currentTour.Distance + 50))
                    {
                        throw new ArgumentException("Distance should be about (+-50 km) the same as the tour");
                    }
                    _tourLogDistance = value;
                    RaisePropertyChangedEvent(nameof(TourLogDistance));
                }
            }
        }
        public float TourLogDuration
        {
            get { return _tourLogDuration; }

            set
            {
                if (_tourLogDuration != value)
                {
                    if (value < (_currentTour.Duration - 120) || value > (_currentTour.Duration + 120))
                    {
                        throw new ArgumentException("Duration should be about (+-120 min) the same as the tour");
                    }
                    _tourLogDuration = value;
                    RaisePropertyChangedEvent(nameof(TourLogDuration));
                }
            }
        }
        public float TourLogFuelUsed
        {
            get { return _tourLogFuelUsed; }

            set
            {
                if (_tourLogFuelUsed != value)
                {
                    if (value < (_currentTour.FuelUsed - 10) || value > (_currentTour.FuelUsed + 10))
                    {
                        throw new ArgumentException("Used fuel should be about (+-10 liter) the same as the tour");
                    }
                    _tourLogFuelUsed = value;
                    RaisePropertyChangedEvent(nameof(TourLogFuelUsed));
                }
            }
        }
        public string TourLogWeather
        {
            get { return _tourLogWeather; }

            set
            {
                if (_tourLogWeather != value)
                {
                    if (value.Length < 5 || value.Length > 10)
                    {
                        throw new ArgumentException("Weather should be between 5 and 10 characters long");
                    }
                    _tourLogWeather = value;
                    RaisePropertyChangedEvent(nameof(TourLogWeather));
                }
            }
        }
        public float TourLogTemperature
        {
            get { return _tourLogTemperature; }

            set
            {
                if (_tourLogTemperature != value)
                {
                    if (value < -20 || value > 40)
                    {
                        throw new ArgumentException("Temperature should be between -20 and 40 °C");
                    }
                    _tourLogTemperature = value;
                    RaisePropertyChangedEvent(nameof(TourLogTemperature));
                }
            }
        }
        public int TourLogEffort
        {
            get { return _tourLogEffort; }

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
            get { return _tourLogReport; }

            set
            {
                if (_tourLogReport != value)
                {
                    if (value.Length < 5 || value.Length > 50)
                    {
                        throw new ArgumentException("Report should be between 5 and 50 characters long");
                    }
                    _tourLogReport = value;
                    RaisePropertyChangedEvent(nameof(TourLogReport));
                }
            }
        }
        public int TourLogRating
        {
            get { return _tourLogRating; }

            set
            {
                if (_tourLogRating != value)
                {
                    _tourLogRating = value;
                    RaisePropertyChangedEvent(nameof(TourLogRating));
                }
            }
        }


        private void AddTourLog(object commandParameter)
        {
            AddedTourLog?.Invoke(this, new TourLogItem(_currentTour.Name, DateTime.Now, _tourLogDistance, _tourLogDuration, _tourLogReport, _tourLogRating,  _tourLogFuelUsed, _tourLogWeather, _tourLogTemperature, _tourLogEffort ));
            CloseWindow(commandParameter);
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
