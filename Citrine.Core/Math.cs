using System;

namespace Citrine.Core
{
    public static class Math
    {
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T> =>
            value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;

        public static float Clamp01(this float value) =>
            value < 0 ? 0 : value > 1 ? 1 : value;

        public static double Clamp01(this double value) =>
            value < 0 ? 0 : value > 1 ? 1 : value;

        public static decimal Clamp01(this decimal value) =>
            value < 0 ? 0 : value > 1 ? 1 : value;
    }
}
