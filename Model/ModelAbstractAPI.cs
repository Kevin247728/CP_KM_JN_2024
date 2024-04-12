using System;
using System.Diagnostics;
using Logic;
using Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;

namespace Model
{
    public abstract class ModelAbstractAPI
    {
        //public abstract void StartSimulation(int nrOfBalls);
        public abstract void CreateEllipses(int nrOfBalls);
        public abstract Canvas Canvas { get; set; }
        public abstract List<Ellipse> ellipseCollection { get; }

        public static ModelAbstractAPI CreateModelAPI()
        {
            return new ModelAPI();
        }
    }

    internal class ModelAPI : ModelAbstractAPI
    {
        private readonly LogicAbstractAPI logicAPI;
        private readonly DataAbstractAPI dataAPI;
        public override List<Ellipse> ellipseCollection { get; }
        public override Canvas Canvas { get; set; }
        private readonly Random random;

        public ModelAPI()
        {
            dataAPI = DataAbstractAPI.CreateAPI();
            logicAPI = LogicAbstractAPI.CreateLogicAPI();
            Canvas = new Canvas();
            ellipseCollection = new List<Ellipse>();
            Canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            Canvas.VerticalAlignment = VerticalAlignment.Stretch;
            Canvas.Width = 300;
            Canvas.Height = 500;
            random = new Random();
        }


        public override void CreateEllipses(int numberOfBalls)
        {
            logicAPI.Start(numberOfBalls);

            for (int i = 0; i < numberOfBalls; i++)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromRgb((byte)random.Next(0, 128), (byte)random.Next(128, 256), (byte)random.Next(128, 256)));
                Ellipse ellipse = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Fill = brush
                };

                // Obliczamy losowe pozycje elipsy w obrębie ramki
                double x = random.Next(0, (int)Canvas.Width - 10);
                double y = random.Next(0, (int)Canvas.Height - 10);

                // Sprawdzamy, czy nowa elipsa nie nakłada się na istniejące elipsy
                bool isOverlapping = CheckForOverlap(x, y);
                while (isOverlapping)
                {
                    x = random.Next(0, (int)Canvas.Width - 10);
                    y = random.Next(0, (int)Canvas.Height - 10);
                    isOverlapping = CheckForOverlap(x, y);
                }

                // Ustawiamy pozycję nowej elipsy
                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);

                ellipseCollection.Add(ellipse);
                Canvas.Children.Add(ellipse);
            }
        }


        // Metoda sprawdzająca, czy nowa elipsa nie nakłada się na istniejące elipsy
        private bool CheckForOverlap(double x, double y)
        {
            foreach (var existingEllipse in ellipseCollection)
            {
                double existingX = Canvas.GetLeft(existingEllipse);
                double existingY = Canvas.GetTop(existingEllipse);

                // Sprawdzamy czy nowa elipsa nakłada się na istniejącą elipsę
                if (Math.Abs(existingX - x) < 20 && Math.Abs(existingY - y) < 20)
                {
                    return true;
                }
            }
            return false;
        }
    }



}