using log4net;
using log4net.Config;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.Views;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ITourItemFactory _tourFactory;
        private string _filePath;

        private ICommand _searchCommand;
        private ICommand _clearCommand;
        private ICommand _deleteCommand;
        private ICommand _printTourReportCommand;
        private ICommand _printSummarizeReportCommand;
        private TourItem _currentTour;
        private string _searchTour;
        private ICommand _openAddTourWinCommand;
        private ICommand _openEditTourWinCommand;

        private ICommand _openAddTourLogWinCommand;
        private ICommand _openEditTourLogWinCommand;
        private ICommand _deleteLogCommand;
        private TourLogItem _currentTourLog;

        private ICommand _exportToursCommand;
        private ICommand _openFileDialogCommand;

        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(Search);
        public ICommand ClearCommand => _clearCommand ??= new RelayCommand(Clear);
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(Delete);
        public ICommand PrintTourReportCommand => _printTourReportCommand ??= new RelayCommand(PrintTourReport);
        public ICommand PrintSummarizeReportCommand => _printSummarizeReportCommand ??= new RelayCommand(PrintSummarizeReport);
        public ICommand OpenAddTourWinCommand => _openAddTourWinCommand ??= new RelayCommand(AddTour);
        public ICommand OpenEditTourWinCommand => _openEditTourWinCommand ??= new RelayCommand(EditTour);


        public ICommand OpenAddTourLogWinCommand => _openAddTourLogWinCommand ??= new RelayCommand(AddLog);
        public ICommand DeleteLogCommand => _deleteLogCommand ??= new RelayCommand(DeleteLog);
        public ICommand OpenEditTourLogWinCommand => _openEditTourLogWinCommand ??= new RelayCommand(EditLog);


        public ICommand ExportToursCommand => _exportToursCommand ??= new RelayCommand(ExportTours);
        public ICommand OpenFileDialogCommand => _openFileDialogCommand ??= new RelayCommand(ImportTours);

        public ObservableCollection<TourItem> Tours { get; set; }
        public ObservableCollection<TourLogItem> TourLogs { get; set; }

        private static readonly ILog _logger = LogManager.GetLogger(typeof(MainViewModel));

        public MainViewModel(ObservableCollection<TourItem> tours)
        {
            Tours = tours;
        }

        public MainViewModel()
        {
            _tourFactory = TourItemFactory.GetInstance();
            CreateList();
            _filePath = _tourFactory.GetFilePath();
            XmlConfigurator.Configure();
        }

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

        public ImageSource CurrentMap
        {
            
            get
            {
                if (CurrentTour != null)
                {
                    string filePath = $"{_filePath}/maps/{CurrentTour.Name}.png";
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                            bitmap.UriSource = new Uri(filePath);
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();

                            return bitmap;
                        }catch(Exception ex)
                        {
                            _logger.Error($"Cannot get tourmap: {ex.Message}");
                        }
                        
                    }
                }

                return null;
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
                RaisePropertyChangedEvent(nameof(CurrentMap));
            }
        }

        //TOURS
        private void Search(object commandParameter)
        {
            if (String.IsNullOrEmpty(SearchTour))
                Clear(commandParameter);
            else
            {
                IEnumerable foundTours = this._tourFactory.Search(SearchTour);
                Tours.Clear();
                foreach (TourItem item in foundTours)
                {
                    Tours.Add(item);
                }
                RaisePropertyChangedEvent(nameof(Tours));
            }
        }
        private void Clear(object commandParameter)
        {
            _searchTour = null;
            CreateList(); 
            RaisePropertyChangedEvent(nameof(Tours));
            RaisePropertyChangedEvent(nameof(SearchTour));
        }
        private void Delete(object commandParameter)
        {
            if(CurrentTour!= null)
            {
                _tourFactory.DeleteTour(CurrentTour);
                Tours.Remove(CurrentTour);
            }
            CreateList();
            RaisePropertyChangedEvent(nameof(Tours));
        }
        private void AddTour(object commandParameter)
        {
            AddTourWindow addTourWindow = new AddTourWindow();
            AddTourViewModel addTourVM = new AddTourViewModel(Tours);

            addTourVM.AddedTour += (_, tour) => { 
                Add(tour); 
                if(tour.Distance == 0 && tour.TransportMode == "Bicycle")
                {
                    throw new ArgumentException("Cannot create bicycle tour - no appropriate roads");
                }
            };
            addTourWindow.DataContext = addTourVM;

            try
            {
                addTourWindow.ShowDialog();
            }catch(Exception ex)
            {
                _logger.Error($"Cannot add tour: {ex.Message}");
                addTourWindow.Close();
                MessageBox.Show(ex.Message);
                CreateList();
                RaisePropertyChangedEvent(nameof(Tours));
            }
        }

        private void Add(TourItem tour)
        {
            _tourFactory.AddTour(tour);

            Tours.Add(tour);

            RaisePropertyChangedEvent(nameof(Tours));
        }
        private void EditTour(object commandParameter)
        {
            if(CurrentTour != null) 
            {
                EditTourWindow editTourWindow = new EditTourWindow();
                EditTourViewModel editTourVM = new EditTourViewModel(CurrentTour, Tours);

                editTourVM.EditedTour += (_, tour) => { 
                    Edit(tour);
                    if (tour.Distance == 0 && tour.TransportMode == "Bicycle")
                    {
                        throw new ArgumentException("Cannot create bicycle tour - no appropriate roads");
                    }
                };
                editTourWindow.DataContext = editTourVM;

                try
                {
                    editTourWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    _logger.Error($"Cannot add tour: {ex.Message}");
                    editTourWindow.Close();
                    MessageBox.Show(ex.Message);
                    CreateList();
                    RaisePropertyChangedEvent(nameof(Tours));
                }
            }
        }
        private void Edit(TourItem tour)
        {
            _tourFactory.DeleteTour(CurrentTour);
           
            Tours.Clear();

            foreach (TourItem item in this._tourFactory.GetTours())
            {
                Tours.Add(item);
            }

            _tourFactory.AddTour(tour);
            Tours.Add(tour);

            RaisePropertyChangedEvent(nameof(Tours));
        }
        private void PrintTourReport(object commandParameter)
        {
            if (CurrentTour != null)
            {
                _tourFactory.CreateTourReport(CurrentTour);
            }
        }
        private void PrintSummarizeReport(object commandParameter)
        {
            _tourFactory.CreateSummarizeReport();
        }


        //LOGS
        private void AddLog(object commandParameter)
        {
            if (CurrentTour != null)
            {
                AddTourLogWindow addTourLogWindow = new AddTourLogWindow();
                AddTourLogViewModel addTourLogVM = new AddTourLogViewModel(CurrentTour);

                addTourLogVM.AddedTourLog += (_, tourLog) => { AddTourLog(tourLog); };
                addTourLogWindow.DataContext = addTourLogVM;

                addTourLogWindow.ShowDialog();
            }
        }
        private void AddTourLog(TourLogItem tourLog)
        {
            _tourFactory.AddTourLog(tourLog);

            TourLogs.Add(tourLog);

            RaisePropertyChangedEvent(nameof(TourLogs));
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
                EditTourLogViewModel editTourLogVM = new EditTourLogViewModel(CurrentTour, CurrentTourLog);

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

            RaisePropertyChangedEvent(nameof(TourLogs));
        }



        private void ExportTours(object commandParameter)
        {
            _tourFactory.ExportTours();
        }
        private void ImportTours(object commandParameter)
        {
            string fileName;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "json files (*.json)|*.json"; 
            openFileDialog.FilterIndex = 2;
            openFileDialog.Multiselect = false;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();

            fileName = openFileDialog.FileName;
            Import(fileName);
        }

        private void Import(string filePath)
        {
            IEnumerable<TourItem> tours = _tourFactory.ImportTours(filePath);

            foreach(TourItem tour in tours)
            {
                _tourFactory.AddTour(tour);
                Tours.Add(tour);
            }
            
            RaisePropertyChangedEvent(nameof(Tours));
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
