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

        public ModelAPI()
        {
            dataAPI = DataAbstractAPI.CreateAPI();
            logicAPI = LogicAbstractAPI.CreateLogicAPI();
            Canvas = new Canvas();
            ellipseCollection = new List<Ellipse>();
            Canvas.HorizontalAlignment = HorizontalAlignment.Left;
            Canvas.VerticalAlignment = VerticalAlignment.Bottom;
        }

        //public override void StartSimulation(int nrOfBalls)
        //{
        //    logicAPI.Start(nrOfBalls);
        //}

        public override void CreateEllipses(int numberOfBalls)
        {
            Random random = new Random();
            logicAPI.Start(numberOfBalls);

            for (int i = 0; i < 5; i++)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromRgb((byte)random.Next(0, 128), (byte)random.Next(128, 256), (byte)random.Next(128, 256)));
                Ellipse ellipse = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Fill = brush
                };
                Canvas.SetLeft(ellipse, logicAPI.getDataAPI().GetBall(i).X);
                Canvas.SetTop(ellipse, logicAPI.getDataAPI().GetBall(i).Y);

                ellipseCollection.Add(ellipse);
                Canvas.Children.Add(ellipse);
            }
        }
    }


}