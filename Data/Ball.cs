using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace Data
{
    public interface IBall
    {
        Vector2 Position { get; }
        Vector2 Velocity { get; set; }
        void StartMoving();
    }


    internal class Ball : IBall
    {
        private Vector2 position;
        private Vector2 velocity;
        private static int r = 20;

        private float mass { get; set; }
        private static readonly int MILISECONDS_PER_STEP = 10;
        private static readonly float STEP_SIZE = 0.1f;


        internal Ball(Vector2 position, Vector2 velocity)
        {
            this.velocity = velocity;
            this.position = position;
            this.mass = 100.0F;
        }

        public static int GetBallRadius()
        {
            return r;
        }

        public Vector2 Position
        {
            get
            {
                return position;     
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return velocity;

            }

            set
            {

                velocity = value;

            }
        }

        public async void StartMoving()
        {
            while (true)
            {
                Stopwatch watch = Stopwatch.StartNew();
                float steps = Velocity.Length() / STEP_SIZE;

                position += Vector2.Normalize(Velocity) * STEP_SIZE;

                watch.Stop();
                int delay = (int)(Convert.ToInt32(MILISECONDS_PER_STEP / steps) - watch.ElapsedMilliseconds);
                await Task.Delay(delay > 0 ? delay : 0);
            }
        }
    }
}
