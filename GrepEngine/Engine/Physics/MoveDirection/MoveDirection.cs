using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrepEngine.Engine.Physics.MoveDirection
{
    public class MoveDirection
    {
        public Direction Direction { get; set; } = Direction.FORWARD;
        public float Magnitude { get; set; } = 0;

        public MoveDirection(Direction direction, float magnitude)
        {
            this.Direction = direction;
            this.Magnitude = magnitude;
        }
    }

    public enum Direction
    {
        FORWARD = 1,
        BACKWARDS = 2,
        LEFT = 3,
        RIGHT = 4,
        UP = 5,
        DOWN = 6
    }
}
