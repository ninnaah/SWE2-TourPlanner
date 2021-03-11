﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace TourPlanner
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string _output = "Hallo du!";
        private string _input;

        public string Input
        {
            get
            {
                return _input;
            }

            set
            {
                if(Input != value)
                {
                    _input = value;
                    OnPropertyChanged(nameof(Input));
                }

            }
        }

        public string Output
        {
            get
            {
                return _output;
            }
            set
            {
                if (_output != value)
                {
                    _output = value;
                    OnPropertyChanged(null);
                }
            }
        }

        public ICommand SendCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            this.SendCommand = new SendCommand(this);
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            //PropertyChangedEventHandler handler = this.PropertyChanged;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }




}
