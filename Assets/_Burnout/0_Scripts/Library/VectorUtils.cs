using UnityEngine;

namespace ScriptLibrary.VectorUtils
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }
    public class VectorUtils
    {
        public static Direction VectorToDirection(Vector2 vector)
        {

            return (vector.x, vector.y) switch
            {
                (0, 1) => Direction.Up,
                (0, -1) => Direction.Down,
                (-1, 0) => Direction.Left,
                (1, 0) => Direction.Right,
                _ => Direction.None
            };
        }
    }
}
