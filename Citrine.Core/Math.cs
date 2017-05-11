using System;
using OpenTK;

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

        public static Vector3d Pow(this Vector3d vector, double power) =>
            new Vector3d(
                System.Math.Pow(vector.X, power),
                System.Math.Pow(vector.Y, power),
                System.Math.Pow(vector.Z, power));

        public sealed class Ray
        {
            private Ray(Vector3d origin, Vector3d direction)
            {
                Origin = origin;
                Direction = direction;
            }

            public static Ray Build(Vector3d origin, Vector3d direction) =>
                new Ray(origin, direction.Normalized());

            public Vector3d Evaluate(double t) =>
                Origin + Direction * t;

            public readonly Vector3d Origin;
            public readonly Vector3d Direction;
        }
    }
}
