namespace Data
{
    public interface IBall : INotifyPropertyChanged
    {
        double X { get; set; }
        double Y { get; set; }
        int R { get; }
        int ID { get; }
        double ANGLE { get; set; }

        void MoveBall();
    }

    internal class Ball : IBall
    {
        private double x;
        private double y;
        private double angle;
        private readonly int r;
        private readonly int id;
           
        public Ball(double x, double y, int r, int id, double angle)
        {
            this.x = x;
            this.y = y;
            this.r = r;
            this.id = id;
            this.angle = angle;
        }

        public void MoveBall()
        {
            double newX = X + Math.Cos(angle * Math.PI / 180.0);
            double newY = Y + Math.Sin(angle * Math.PI / 180.0);

            this.x = newX;
            this.y = newY;
        }

        public double X
        {
            get => x;
            set
            {
                if (value.Equals(x))
                {
                    return;
                }
                x = value;
            }
        }
        public double Y
        {
            get => y;
            set
            {
                if (value.Equals(y))
                {
                    return;
                }
                y = value;
            }
        }

        public int R
        {
            get => r;
        }

        public int ID
        {
            get => id;
        }

        public double ANGLE
        {
            get => angle;
            set
            {
                if (value.Equals(angle))
                {
                    return;
                }
                angle = value;
            }
        }
    }
}
