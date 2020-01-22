using Barbar.HexGrid.Interfaces;

namespace Barbar.HexGrid
{
    public struct Point : IPoint
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }

        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Point a, Point b) => a.X != b.X || a.Y != b.Y;
        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);
        public static Point operator *(Point a, Point b) => new Point(a.X * b.X, a.Y * b.Y);
        public static Point operator /(Point a, Point b) => new Point(a.X / b.X, a.Y / b.Y);

        public static IPoint Add(IPoint a, IPoint b) => new Point(a.X + b.X, a.Y + b.Y);
        public static IPoint Subtract(IPoint a, IPoint b) => new Point(a.X - b.X, a.Y - b.Y);
        public static IPoint Multiply(IPoint a, IPoint b) => new Point(a.X * b.X, a.Y * b.Y);
        public static IPoint Divide(IPoint a, IPoint b) => new Point(a.X / b.X, a.Y / b.Y);

        public override bool Equals(object obj)
        {
            if (!(obj is Point other))
            {
                return false;
            }

            return (Point)obj == this;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }
    }
}
