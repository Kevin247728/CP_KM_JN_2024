
using Logic;
using Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ILogicAPI logicAPI;
        private ObservableCollection<IBall> _balls;
        private int _numberOfBallsToAdd;

        public ObservableCollection<IBall> Balls
        {
            get => _balls;
            set
            {
                if (value.Equals(_balls))
                {
                    return;
                }
                _balls = value;
                OnPropertyChanged();
            }
        }

        public int NumberOfBallsToAdd
        {
            get => _numberOfBallsToAdd; //getter
            set //setter
            {
                _numberOfBallsToAdd = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        //public MainViewModel(ILogicAPI logicAPI)
        //{
        //    this.logicAPI = logicAPI ?? throw new ArgumentNullException(nameof(logicAPI));
        //    StartCommand = new RelayCommand(StartSimulation, CanStartSimulation);
        //    StopCommand = new RelayCommand(StopSimulation, CanStopSimulation);
        //}

        public MainViewModel()
        {
            StartCommand = new RelayCommand(StartSimulation, CanStartSimulation);
            StopCommand = new RelayCommand(StopSimulation, CanStopSimulation);
        }

        private bool CanStartSimulation(object parameter)
        {
            return NumberOfBallsToAdd > 0;
        }

        private void StartSimulation(object parameter)
        {
            logicAPI.Start(NumberOfBallsToAdd);
        }

        private bool CanStopSimulation(object parameter)
        {
            return Balls?.Count > 0;
        }

        private void StopSimulation(object parameter)
        {
            Balls?.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        private event EventHandler _canExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { _canExecuteChanged += value; }
            remove { _canExecuteChanged -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged() => _canExecuteChanged?.Invoke(this, EventArgs.Empty);
    }


}
