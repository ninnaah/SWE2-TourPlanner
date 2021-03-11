using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TourPlanner
{
    public class SendCommand : ICommand
    {
        private readonly ViewModel _viewModel;

        public SendCommand(ViewModel viewModel)
        {
            _viewModel = viewModel;

            //WHAT IS THIS
             _viewModel.PropertyChanged += (sender, args) =>
             {
                 if (args.PropertyName == "Input")
                 {
                     CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                 }
             };

            //_viewModel.PropertyChanged += (sender, args);

        }

        public bool CanExecute(object parameter)
        {
            if (string.IsNullOrEmpty(_viewModel.Input))
            {
                return false;
            }
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.Output = _viewModel.Input;
            _viewModel.Input = string.Empty;
        }

        public event EventHandler CanExecuteChanged;

    }
}
