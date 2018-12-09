using System;
using System.Linq;
using Citrine.Core;
using OpenTK;
using static System.Math;
using static Citrine.Core.Math;

namespace Citrine.Scene
{
    public static class Scene3
    {
        private const int NumberOfSteps = 64;
        private const double MinimumHitDistance = 0.001f;
        private const double MaximumTraceDistance = 1000.0f;

        private sealed class Light
        {
            public Light(Vector3d position, Vector3d color)
            {
                Position = position;
                Color = color;
            }

            public readonly Vector3d Position;
            public readonly Vector3d Color;
        }

        private static readonly Light[] PointLights =
        {
            new Light(
                new Vector3d(2.0, -5.0, 3.0),
                new Vector3d(0.5, 0.0, 0.0)),
            new Light(
                new Vector3d(-1.0, -5.0, 0.0),
                new Vector3d(0.0, 0.5, 0.0)),
        };

        private static readonly Vector3d AmbientLight = Vector3d.One * 0.05;

        private static double DistanceFromSphere(Vector3d p, Vector3d c, double r) =>
            (p - c).Length - r;

        private static double DistanceFromScene(Vector3d p)
        {
            var displacement = Sin(5.0 * p.X) * Sin(5.0 * p.Y) * Sin(5.0 * p.Z) * 0.25;

            var distance = DistanceFromSphere(p, Vector3d.Zero, 1.0);

            return distance + displacement;
        }

        private static Vector3d CalculateNormal(Vector3d p)
        {
            var smallStep = new Vector3d(0.001, 0.0, 0.0);

            var gradientX = DistanceFromScene(p + smallStep.Xzy) - DistanceFromScene(p - smallStep.Xzy);
            var gradientY = DistanceFromScene(p + smallStep.Yxz) - DistanceFromScene(p - smallStep.Yxz);
            var gradientZ = DistanceFromScene(p + smallStep.Yzx) - DistanceFromScene(p - smallStep.Yzx);

            var normal = new Vector3d(gradientX, gradientY, gradientZ);

            return normal.Normalized();
        }

        private static Vector3d RayMarch(Ray ray)
        {
            var ro = ray.Origin;
            var rd = ray.Direction;

            var totalDistanceTraveled = 0.0;

            for (var i = 0; i < NumberOfSteps; ++i)
            {
                var currentPosition = ro + totalDistanceTraveled * rd;

                var distanceToClosest = DistanceFromScene(currentPosition);

                if (distanceToClosest < MinimumHitDistance)
                {
                    var normal = CalculateNormal(currentPosition);

                    // normal vis
                    // return normal * 0.5 + Vector3d.One * 0.5;

                    var diffuse =
                        PointLights
                            .Select(light =>
                            {
                                var directionToLight = (currentPosition - light.Position).Normalized();

                                var diffuseIntensity = light.Color * Max(0.0, Vector3d.Dot(normal, directionToLight));

                                return diffuseIntensity;
                            })
                            .Aggregate(Vector3d.Zero, (a, b) => a + b);

                    var ambient = AmbientLight;

                    return Vector3d.One * (diffuse + ambient);
                }

                if (totalDistanceTraveled > MaximumTraceDistance)
                {
                    break;
                }

                totalDistanceTraveled += distanceToClosest;
            }

            return Vector3d.Zero;
        }

        private static Color GetColorAtPixel(Vector2 pixelCoordinate, Vector2 resolution)
        {
            var screenCoord = (-resolution + 2.0f * pixelCoordinate) / resolution.Y;

            var cameraPosition = Vector3d.UnitZ * -5.0;
            var cameraDirection = (Vector3d.Zero - cameraPosition).Normalized();
            var cameraRight = Vector3d.Cross(Vector3d.UnitY, cameraDirection).Normalized();
            var cameraUp = Vector3d.Cross(cameraDirection, cameraRight).Normalized();

            var rd =
                screenCoord.X * cameraRight +
                screenCoord.Y * cameraUp +
                1.0f * cameraDirection;

            var ray =
                Ray.Build(
                    cameraPosition,
                    rd);

            var color = RayMarch(ray);

            return Color.FromRGB(color.X, color.Y, color.Z);
        }

        public static Color[][] RenderScene(int width, int height, Func<int, int> random) =>
            Enumerable.Range(0, height)
                .Select(y =>
                    Enumerable.Range(0, width)
                        .Select(x =>
                            GetColorAtPixel(
                                new Vector2(x, y),
                                new Vector2(width, height)))
                        .ToArray())
                .ToArray();
    }
}
