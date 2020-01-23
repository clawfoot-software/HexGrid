using Barbar.HexGrid.Interfaces;
using System;
using System.Collections.Generic;

namespace Barbar.HexGrid
{
    public struct HexLayout
    {
        private readonly Offset _offset;
        private readonly Orientation _orientation;
        private readonly IPoint _size;
        private readonly IPoint _origin;
        
        internal HexLayout(Orientation orientation, IPoint size, IPoint origin, Offset offset)
        {
            _orientation = orientation;
            _size = size;
            _origin = origin;
            _offset = offset;
        }

        public IPoint HexToPixel(CubeCoordinates h)
        {
            double x = (_orientation.f0 * h.Q + _orientation.f1 * h.R) * _size.X;
            double y = (_orientation.f2 * h.Q + _orientation.f3 * h.R) * _size.Y;
            return new Point(x + _origin.X, y + _origin.Y);
        }
        
        public CubeFractionCoordinates PixelToHex(IPoint pixel)
        {
            var x = (pixel.X - _origin.X) / _size.X;
            var y = (pixel.Y - _origin.Y) / _size.Y;

            double q = _orientation.b0 * x + _orientation.b1 * y;
            double r = _orientation.b2 * x + _orientation.b3 * y;
            return new CubeFractionCoordinates(q, r, -q - r);
        }

        public IPoint HexCornerOffset(int corner)
        {
            double angle = 2.0 * Math.PI * (_orientation.start_angle - corner) / 6;
            return new Point(_size.X * Math.Cos(angle), _size.Y * Math.Sin(angle));
        }
        
        public IList<IPoint> PolygonCorners(CubeCoordinates h)
        {
            var corners = new List<IPoint>(6);
            var center = HexToPixel(h);
            for (int i = 0; i < 6; i++)
            {
                var offset = HexCornerOffset(i);
                corners.Add(Point.Add(center, offset));
            }
            return corners;
        }

        public OffsetCoordinates ToOffsetCoordinates(CubeCoordinates h)
        {
            int column;
            int row;
            switch (_orientation.name)
            {
                case OrientationName.FlatHexagons:
                    column = h.Q;
                    row = h.R + ((h.Q + _offset.Value * (h.Q & 1)) / 2);
                    return new OffsetCoordinates(column, row);
                case OrientationName.PointyHexagons:
                    column = h.Q + ((h.R + _offset.Value * (h.R & 1)) / 2);
                    row = h.R;
                    return new OffsetCoordinates(column, row);
                default:
                    throw new NotImplementedException();
            }
        }

        public CubeCoordinates ToCubeCoordinates(OffsetCoordinates h)
        {
            int q, r, s;
            switch (_orientation.name)
            {
                case OrientationName.FlatHexagons:

                    q = h.Column;
                    r = h.Row - ((h.Column + _offset.Value * (h.Column & 1)) / 2);
                    s = -q - r;
                    return new CubeCoordinates(q, r, s);
                case OrientationName.PointyHexagons:
                    q = h.Column - ((h.Row + _offset.Value * (h.Row & 1)) / 2);
                    r = h.Row;
                    s = -q - r;
                    return new CubeCoordinates(q, r, s);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
