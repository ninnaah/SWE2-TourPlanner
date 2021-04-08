using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class AddTourViewModel : MainViewModel
    {
        private ICommand _sendNewTourCommand;
        public Action CloseWin { get; set; }
        private string _tourName;
        private string _tourDescription;
        private string _tourFrom;
        private string _tourTo;
        private int _tourDistance;

        public ICommand SendNewTourCommand => _sendNewTourCommand ??= new RelayCommand(AddTour);


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
        public int TourDistance
        {
            get
            {
                return _tourDistance;
            }

            set
            {
                if (_tourDistance != value)
                {
                    _tourDistance = value;
                    RaisePropertyChangedEvent(nameof(TourDistance));
                }
            }
        }
       

        private void AddTour(object commandParameter)
        {
            TourItem item = new TourItem();
            item.Name = _tourName;
            item.Description = _tourDescription;
            item.Distance = _tourDistance;
            Tours.Add(item);
            CreateList();
            CloseWin();
        }
    }
}
