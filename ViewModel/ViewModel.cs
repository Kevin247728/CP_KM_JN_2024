
using Logic;
using Data;
using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System;
using System.Windows.Controls;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ModelAbstractAPI _model;
        public int _numberOfBallsToAdd;

        public MainViewModel()
        {
            _model = ModelAbstractAPI.CreateModelAPI();
            AddCommand = new RelayCommand(StartSimulation);
        }

        public Canvas Canvas
        {
            get => _model.Canvas;

        }

        public int NumberOfBallsToAdd
        {
            get => _numberOfBallsToAdd;
            set
            {
                if (_numberOfBallsToAdd != value)
                {
                    _numberOfBallsToAdd = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddCommand { get; }

        private void StartSimulation()
        {
            _model.CreateEllipses(NumberOfBallsToAdd);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}