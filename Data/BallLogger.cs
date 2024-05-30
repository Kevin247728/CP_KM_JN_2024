using System;
using System.Diagnostics;
using System.Timers;
using System.Threading.Tasks;

namespace Data
{
    internal class BallLogger : IDisposable
    {
        private readonly ILogger _logger;
        private readonly Timer _timer;
        private readonly IBall _ball;

        public BallLogger(IBall ball, ILogger logger)
        {
            _logger = logger;
            _ball = ball;

            _timer = new Timer()
            {
                Interval = 5000
            };
            _timer.Elapsed += async (sender, e) =>
            {
                BallJsonModel ballToSave;
                lock (_ball)
                {
                    ballToSave = new BallJsonModel(_ball.Id, _ball.Position, _ball.Velocity, DateTime.Now);
                }
                await _logger.Log(ballToSave);
            };
            _timer.Start();
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        public void Start()
        {
            _timer.Elapsed += async (sender, e) =>
            {
                BallJsonModel ballToSave;
                lock (_ball)
                {
                    Debug.WriteLine(DateTime.Now.Millisecond.ToString() + "   " + _ball.Position);
                    ballToSave = new BallJsonModel(_ball.Id, _ball.Position, _ball.Velocity, DateTime.Now);
                }
                await _logger.Log(ballToSave);
            };
            _timer.Start();
        }
    }
}


