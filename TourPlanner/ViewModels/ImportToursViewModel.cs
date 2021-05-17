using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TourPlanner.ViewModels
{
    public class ImportToursViewModel : MainViewModel
    {
        private ICommand _sendImportToursCommand;
        private ICommand _closeWinCommand;
        public Action CloseWin { get; set; }
        private string _filePath;
        public event EventHandler<string> ImportedTours;

        public ICommand SendImportToursCommand => _sendImportToursCommand ??= new RelayCommand(ImportTours);
        public ICommand CloseWinCommand => _closeWinCommand ??= new RelayCommand(CloseWindow);

       

        public string FilePath
        {
            get
            {
                return _filePath;
            }

            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    RaisePropertyChangedEvent(nameof(FilePath));
                }
            }
        }
        
        private void ImportTours(object commandParameter)
        {
            ImportedTours?.Invoke(this, _filePath);
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
