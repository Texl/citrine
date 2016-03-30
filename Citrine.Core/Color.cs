namespace Citrine.Core
{
    public sealed class Color
    {
        public float[] AsFloatArray() => new[] {Red, Green, Blue, Alpha};

        public byte[] AsByteArray() => new[] {(byte)(Red * 255), (byte)(Green * 255), (byte)(Blue * 255), (byte)(Alpha * 255)};

        public static Color FromRGBA(float red, float green, float blue, float alpha) =>
            new Color(red.Clamp01(), green.Clamp01(), blue.Clamp01(), alpha.Clamp01());

        public static Color FromRGB(float red, float green, float blue) =>
            FromRGBA(red, green, blue, 1.0f);

        public static Color FromRGBA(byte red, byte green, byte blue, byte alpha) =>
            FromRGBA(red / 255.0f, green / 255.0f, blue / 255.0f, alpha / 255.0f);

        public static Color FromRGB(byte red, byte green, byte blue, byte alpha) =>
            FromRGB(red / 255.0f, green / 255.0f, blue / 255.0f);

        public static readonly Color Black = FromRGB(0.0f, 0.0f, 0.0f);
        public static readonly Color White = FromRGB(1.0f, 1.0f, 1.0f);

        private Color(float red, float green, float blue, float alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        private readonly float Red;
        private readonly float Green;
        private readonly float Blue;
        private readonly float Alpha;
    }
}
