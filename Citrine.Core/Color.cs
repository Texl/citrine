namespace Citrine.Core
{
    public sealed class Color
    {
        public const int ByteSize = 4;

        public double[] AsDoubleArray() => new[] {Red, Green, Blue, Alpha};

        public byte[] AsByteArray() => new[] {(byte)(Red * 255), (byte)(Green * 255), (byte)(Blue * 255), (byte)(Alpha * 255)};

        public static Color FromRGBA(double red, double green, double blue, double alpha) =>
            new Color(red.Clamp01(), green.Clamp01(), blue.Clamp01(), alpha.Clamp01());

        public static Color FromRGB(double red, double green, double blue) =>
            FromRGBA(red, green, blue, 1.0);

        public static Color FromRGBA(byte red, byte green, byte blue, byte alpha) =>
            FromRGBA(red / 255.0f, green / 255.0f, blue / 255.0f, alpha / 255.0f);

        public static Color FromRGB(byte red, byte green, byte blue, byte alpha) =>
            FromRGB(red / 255.0f, green / 255.0f, blue / 255.0f);

        public static readonly Color Black = FromRGB(0.0, 0.0, 0.0);
        public static readonly Color White = FromRGB(1.0, 1.0, 1.0);

        private Color(double red, double green, double blue, double alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        private readonly double Red;
        private readonly double Green;
        private readonly double Blue;
        private readonly double Alpha;
    }
}
