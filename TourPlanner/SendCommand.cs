using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TourPlanner
{
    public class SendCommand : ICommand
    {
        private readonly ViewModel _viewModel;

        public event EventHandler CanExecuteChanged;

        public SendCommand(ViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.PropertyChanged += (sender, args) =>
            {
                 CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            };

        }

        public bool CanExecute(object parameter)
        {
            if (string.IsNullOrEmpty(_viewModel.Input))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.Output = _viewModel.Input;
            _viewModel.Input = string.Empty;
        }

    }
}
