using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public enum Direction : byte
    {
        Left = 1,
        Right = 2,
        Top = 3,
        Bottom = 4,
        None = 5


    }
    static class DirectionMethods
    {

        public static bool isOpposite(this Direction dir, Direction prevdir)
        {
            if ((dir == Direction.Left && prevdir == Direction.Right) || (dir == Direction.Right && prevdir
                == Direction.Left) || (dir == Direction.Top && prevdir == Direction.Bottom) || (dir == Direction.Bottom && prevdir == Direction.Top))
                return true;
            else return false;
        }
        public static Direction getOpposite(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Bottom:
                    return Direction.Top;
                case Direction.Top:
                    return Direction.Bottom;
                default: return new Direction();
            }
        }
        public static Direction getAnother(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:
                    return Direction.Top;
                case Direction.Right:
                    return Direction.Bottom;
                case Direction.Bottom:
                    return Direction.Left;
                case Direction.Top:
                    return Direction.Right;
                default: return new Direction();
            }
        }
    }
}
