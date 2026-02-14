using System;

namespace InterpolationVisualizer.Models
{
    public class DataPoint : IComparable<DataPoint>
    {
        public double X { get; set; }
        public double Y { get; set; }

        public DataPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public int CompareTo(DataPoint? other)
        {
            if (other == null) return 1;
            return X.CompareTo(other.X);
        }
    }
}