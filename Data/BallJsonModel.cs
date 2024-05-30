using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace Data
{
    public class BallJsonModel
    {
        public BallJsonModel(int id, Vector2 position , Vector2 velocity, DateTime timestamp)
        {
            Id = id;
            Position = position;
            Velocity = velocity;
            Timestamp = timestamp;
        }

        public DateTime Timestamp { get; }
        public int Id { get; }
        public Vector2 Position { get; }
        public Vector2 Velocity { get; }
    }
}