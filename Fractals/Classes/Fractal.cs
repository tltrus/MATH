using System.Windows.Media.Imaging;


namespace Fractals
{
    internal class Fractal
    {
        public struct TComplex
        {
            public double x, y;
        }
        public int iter = 80;
        public int max = 16;

        public TComplex z, c;

        public WriteableBitmap wb;
        public double zoom = 0.9; // увеличение\уменьшение изображения [0 ... 1]

        public int mx, my, n;
        public byte r, g, b;

        public Fractal(WriteableBitmap wb, int width, int height)
        {
            z = new TComplex();
            c = new TComplex();

            mx = width / 2;
            my = height / 2;

            this.wb = wb;
        }
    }
}
