
using Logic;
using Model;
using Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged 
    {
        private readonly ModelAbstractAPI _ballService; 
        private ObservableCollection<IBall> _balls; 
        private int _numberOfBallsToAdd; //przechowuje liczbe kul do dodania.

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

        public MainViewModel(ModelAbstractAPI ballService) //konstruktor klasy
        {
            _ballService = ballService; 
            _ballService.BallsUpdated += (sender, balls) => Balls = balls; 
        }

        public void StartSimulation() 
        {
            _ballService.AddBalls(NumberOfBallsToAdd); //dodaje kulki do symulacji
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
