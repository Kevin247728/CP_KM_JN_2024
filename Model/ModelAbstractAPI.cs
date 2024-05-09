using System;
using System.Diagnostics;
using Logic;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;

using System.Windows.Media.Animation;
using System.Numerics;


namespace Model
{
    public abstract class ModelAbstractAPI
    {
        public abstract void CreateEllipses(int nrOfBalls);
        public abstract Canvas Canvas { get; set; }
        public abstract List<Ellipse> ellipseCollection { get; }
        public abstract bool IsAnimating { get; set; }

        public static ModelAbstractAPI CreateModelAPI()
        {
            return new ModelAPI();
        }
    }

    public class ModelAPI : ModelAbstractAPI
    {
        private readonly LogicAbstractAPI logicAPI;
        public override List<Ellipse> ellipseCollection { get; }
        public override Canvas Canvas { get; set; }
        private readonly Random random;
        private int ballsCreated = 0;
        private List<(Ellipse, EventHandler)> ballHandlers;
        private List<Storyboard> ballAnimations;
        public event EventHandler IsAnimatingChanged;
        private EventHandler renderingHandler;
        private Dictionary<int, Ellipse> ellipseDictionary = new Dictionary<int, Ellipse>();


        private bool _isAnimating;
        public override bool IsAnimating
        {
            get { return _isAnimating; }
            set
            {
                if (_isAnimating != value)
                {
                    _isAnimating = value;
                    IsAnimatingChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }


        public ModelAPI()
        {
            logicAPI = LogicAbstractAPI.CreateLogicAPI();
            Canvas = new Canvas();
            ellipseCollection = new List<Ellipse>();
            Canvas = new Canvas
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = logicAPI.GetBoardWidth(),
                Height = logicAPI.GetBoardHeight(),
            };
            random = new Random();

            ballAnimations = new List<Storyboard>();
            ballHandlers = new List<(Ellipse, EventHandler)>();
        }


        public override void CreateEllipses(int numberOfBalls)
        {
            logicAPI.CreateBalls(numberOfBalls);

            for (int i = ballsCreated; i < numberOfBalls + ballsCreated; i++)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromRgb((byte)random.Next(0, 128), (byte)random.Next(128, 256), (byte)random.Next(128, 256)));
                Ellipse ellipse = new Ellipse
                {
                    Width = logicAPI.GetBallRadius(),
                    Height = logicAPI.GetBallRadius(),
                    Fill = brush
                };

                Vector2 position = logicAPI.GetBallPosition(i);

                double x = position.X;
                double y = position.Y;

                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);

                ellipseCollection.Add(ellipse);
                int ellipseIndex = ellipseCollection.Count - 1;
                ellipseDictionary.Add(ellipseIndex, ellipse);

                Canvas.Children.Add(ellipse);

                EventHandler renderingHandler = CreateRenderingHandler(ellipse, ellipseIndex);
                ballHandlers.Add((ellipse, renderingHandler));
            }

            ballsCreated += numberOfBalls;
        }

        private EventHandler CreateRenderingHandler(Ellipse ellipse, int ellipseIndex)
        {
            logicAPI.Start();

            return (sender, e) =>
            {
                Vector2 ballPosition = logicAPI.GetBallPosition(ellipseIndex);
                Canvas.SetLeft(ellipse, ballPosition.X);
                Canvas.SetTop(ellipse, ballPosition.Y);
                //double currentX = Canvas.GetLeft(ellipse);
                //double currentY = Canvas.GetTop(ellipse);

                //// Nowe pozycje po dodaniu prędkości
                //double newX = currentX + speedX;
                //double newY = currentY + speedY;

                //// Ustaw nowe pozycje
                //Canvas.SetLeft(ellipse, currentX + speedX);
                //Canvas.SetTop(ellipse, currentY + speedY);
            };
        }


        public void StartBallAnimation()
        {
            if (!IsAnimating)
            {
                IsAnimating = true;
                foreach (var handler in ballHandlers)
                {
                    CompositionTarget.Rendering += handler.Item2;
                }
            }
        }

        private double GetRandomSpeed()
        {
            Random random = new Random();
            return random.NextDouble() * 5 + 1; // Losowa prędkość z zakresu 1-6
        }

        public void StopBallAnimation()
        {
            if (IsAnimating)
            {
                foreach (var handler in ballHandlers)
                {
                    CompositionTarget.Rendering -= handler.Item2;
                }
                IsAnimating = false;
            }
        }

        public void DeleteEllipses()
        {
            foreach (var ellipse in ellipseCollection)
            {
                Canvas.Children.Remove(ellipse);
            }
            ellipseCollection.Clear();
            ballHandlers.Clear();
        }
    }
}