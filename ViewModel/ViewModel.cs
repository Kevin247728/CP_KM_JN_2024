
using Logic;
using Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged 
    {
        private readonly ModelAbstractAPI _ballService; 
        private List<Ball> _balls; //przechowuje kolekcje kul.
        private int _numberOfBallsToAdd; //przechowuje liczbe kul do dodania.

        public List<Ball> Balls 
        {
            get => _balls; //getter - zwraca wartość pola _balls.
            set //setter - ustawia wartość pola _balls i wywołuje metodę OnPropertyChanged
            {
                _balls = value; 
                OnPropertyChanged(); //informuje o zmianie właściwości.
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
