using Data;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class LogicAbstractAPI
    {
        public abstract void Start();
        public abstract int GetBoardWidth();
        public abstract int GetBoardHeight();
        public abstract Vector2 GetBallPosition(int index);
        public abstract int GetBallRadius();
        public abstract void CreateBalls(int nrOfBalls);
        public abstract IBall GetBall(int index);
        public static LogicAbstractAPI CreateLogicAPI()
        {
            return new LogicAPI();
        }
    }

    public class LogicAPI : LogicAbstractAPI
    {
        private DataAbstractAPI dataAPI;

        public LogicAPI()
        {
            this.dataAPI = DataAbstractAPI.CreateAPI();
        }

        public override Vector2 GetBallPosition(int index)
        {
            return dataAPI.GetBall(index).Position;
        }

        public override IBall GetBall(int index)
        {
            return dataAPI.GetBall(index);
        }

        public override int GetBallRadius()
        {
            return dataAPI.GetBallRadius();
        }

        public override int GetBoardWidth()
        {
            return dataAPI.GetBoardWidth();
        }

        public override int GetBoardHeight()
        {
            return dataAPI.GetBoardHeight();
        }

        public override void CreateBalls(int nrOfBalls)
        {
            Random rand = new Random();
            List<Vector2> ballPositions = dataAPI.GetBallsPositions();
            float ballRadius = dataAPI.GetBallRadius();

            for (int i = 0; i < nrOfBalls; i++)
            {
                bool overlap = true;
                Vector2 position;

                // Repeat until a non-overlapping position is found
                while (overlap)
                {
                    overlap = false;
                    position = new Vector2(
                        rand.Next(GetBallRadius(), dataAPI.GetBoardWidth() - GetBallRadius()),
                        rand.Next(GetBallRadius(), dataAPI.GetBoardWidth() - GetBallRadius()));

                    // Check if the new position overlaps with any existing ball
                    foreach (var existingPosition in ballPositions)
                    {
                        if (Vector2.Distance(existingPosition, position) < ballRadius)
                        {
                            overlap = true;
                            break;
                        }
                    }

                    // If no overlap, add the position to the list
                    if (!overlap)
                    {
                        ballPositions.Add(position);
                        IBall newBall = dataAPI.CreateBall(position, Vector2.Zero);

                        Vector2 maxVelocity = new Vector2(2.0f, 2.0f);
                        Vector2 velocity = new Vector2(
                            (float)(rand.NextDouble() * maxVelocity.X - (maxVelocity.X / 2)),
                            (float)(rand.NextDouble() * maxVelocity.Y - (maxVelocity.Y / 2))
                        );
                        newBall.Velocity = velocity;
                    }
                }
            }
        }

        public override void Start()
        {
            int numberOfBalls = dataAPI.GetBallsCount();

            for (int i = 0; i < numberOfBalls; i++)
            {
                IBall ball = dataAPI.GetBall(i);
                _ = Task.Run(() => { ball.StartMoving(); });
            }
        }


    }
}