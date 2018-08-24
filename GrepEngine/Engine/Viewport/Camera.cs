using GrepEngine.Engine.Physics.MoveDirection;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrepEngine.Engine.Viewport
{
    class Camera
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 PrevPos = Vector3.Zero;
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public float MoveSpeed = 2f;
        public float MouseSensitivity = 0.001f;
        public Vector2 lastMousePos = new Vector2();

        private Stack<MoveDirection> movementStack = new Stack<MoveDirection>();

        public Matrix4 GetViewMatrix()
        {
            Vector3 lookat = new Vector3();

            lookat.X = (float)(Math.Sin((float)Orientation.X) * Math.Cos((float)Orientation.Y));
            lookat.Y = (float)Math.Sin((float)Orientation.Y);
            lookat.Z = (float)(Math.Cos((float)Orientation.X) * Math.Cos((float)Orientation.Y));

            return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
        }

        /// <summary>
        /// Initiates movement based upon cameras movement stack
        /// </summary>
        public void Move()
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin((float)Orientation.X), 0, (float)Math.Cos((float)Orientation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            float offsetX = 0;
            float offsetY = 0;
            float offsetZ = 0;

            var clonedMovementStack = new Stack<MoveDirection>(movementStack);

            while (clonedMovementStack.Count > 0)
            {
                var movement = clonedMovementStack.Pop();

                if(movement.Direction == Direction.FORWARD)
                {
                    offset += movement.Magnitude * forward;
                    break;
                } else if(movement.Direction == Direction.BACKWARDS)
                {
                    offset -= movement.Magnitude * forward;
                    break;
                }
            }

            clonedMovementStack = new Stack<MoveDirection>(movementStack);

            while (clonedMovementStack.Count > 0)
            {
                var movement = clonedMovementStack.Pop();

                if (movement.Direction == Direction.RIGHT)
                {
                    offset += movement.Magnitude * right;
                    break;
                }
                else if (movement.Direction == Direction.LEFT)
                {
                    offset -= movement.Magnitude * right;
                    break;
                }
            }

            //  offset.Y += z; used for up down movement

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, MoveSpeed);

            Position += offset;
        }

        public void AddRotation(GameWindow window)
        {
            Vector2 delta = lastMousePos - new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            Console.WriteLine(Mouse.GetState().X);

            if(Math.Abs(delta.X) > window.Width / 2)
            {
                delta.X = 0;
            }

            if (Math.Abs(delta.Y) > window.Height / 2)
            {
                delta.Y = 0;
            }

            var x = delta.X * MouseSensitivity;
            var y = delta.Y * MouseSensitivity;

            Orientation.X = (Orientation.X + x) % ((float)Math.PI * 2.0f);
            Orientation.Y = Math.Max(Math.Min(Orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);

            lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
        }
         
        /// <summary>
        /// inserts movement command to the movement stack
        /// </summary>
        /// <param name="moveDirection"></param>
        public void AddMovementCommand(MoveDirection moveDirection)
        {
            movementStack.Push(moveDirection);
        }

        /// <summary>
        /// inserts movement command to the movement stack
        /// </summary>
        /// <param name="moveDirection"></param>
        public void RemoveMovementCommand(Direction direction)
        {
            movementStack = new Stack<MoveDirection>(movementStack.Where(x => x.Direction != direction));
        }

    }
}
