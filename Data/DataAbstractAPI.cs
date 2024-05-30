using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Serilog;


namespace Data
{
    public abstract class DataAbstractAPI
    {
        public abstract int GetBoardWidth();
        public abstract int GetBoardHeight();
        public abstract IBall CreateBall(Vector2 position, Vector2 velocity);
        public abstract IBall GetBall(int index);
        public abstract int GetBallsCount();
        public abstract int GetBallRadius();
        public abstract List<Vector2> GetBallsPositions();
        public abstract List<IBall> GetBalls();
        public abstract void ClearBalls();
        public abstract int GetBallIndex(IBall ball);
        public abstract float GetBallMass();

        public abstract ILogger Logger { get; }

        public abstract void EnableLogging(ILogger logger);

        public static DataAbstractAPI CreateAPI()
        {
            ILogger logger = new FileLogger();
            return new DataAPI(logger);
        }
    }

    internal class DataAPI : DataAbstractAPI
    {
        private readonly BallsCollection balls;
        private List<BallLogger> _loggers = new List<BallLogger>();
        private readonly ILogger _logger;

        public override ILogger Logger => _logger;

        public DataAPI(ILogger logger)
        {
            balls = new BallsCollection(logger);
            _logger = logger;
        }

        public override int GetBoardWidth()
        {
            return Board.width;
        }

        public override float GetBallMass()
        {
            return Board.BallMass;
        }

        public override List<Vector2> GetBallsPositions()
        {
            return balls.GetBallsPositions();
        }

        public override List<IBall> GetBalls()
        {
            return balls.GetBalls();
        }

        public override int GetBoardHeight()
        {
            return Board.height;
        }

        public override IBall CreateBall(Vector2 position, Vector2 velocity)
        {
            return balls.CreateBall(position, velocity);
        }

        public override int GetBallsCount()
        {
            return balls.GetBallsCount();
        }

        public override void ClearBalls()
        {
            _loggers.ForEach(b => b.Dispose());
            _loggers.Clear();
            balls.Clear();
        }

        public override IBall GetBall(int index)
        {
            return balls.GetBall(index);
        }

        public override int GetBallIndex(IBall ball)
        {
            return balls.GetBallIndex(ball);
        }

        public override int GetBallRadius()
        {
            return Ball.GetBallRadius();
        }

        public override void EnableLogging(ILogger logger)
        {
            foreach (Ball b in balls.GetBalls())
            {
                _loggers.Add(new BallLogger(b, logger));
            }
        }
    }
}
