using System;
using System.Linq;
using Citrine.Core;
using OpenTK;
using Math = Citrine.Core.Math;

namespace Citrine.Scene
{
    public class Scene2
    {
        private Scene2()
        {
            ((double x, double y), Vector3d normal) GetIntersection(Math.Ray ray, double maxDistance) =>
                ((0.0, 0.0), Vector3d.Zero);

            Vector3d GetBackground(Vector3d rayDirection) =>
                Vector3d.Zero;

            Vector3d GetColor(Vector3d position, Vector3d normal, double intersection) =>
                Vector3d.UnitX + 0.5f * Vector3d.UnitY;

            Vector3d GetLighting(Vector3d position, Vector3d normal) =>
                Vector3d.One * 0.5f;

            Vector3d GetColorAtRay(Math.Ray ray)
            {
                var (intersection, normal) = GetIntersection(ray, 1000.0);

                if (intersection.Item2 < 0.0)
                {
                    return GetBackground(ray.Direction);
                }

                var position = ray.Evaluate(intersection.Item1);

                var surfaceColor = GetColor(position, normal, intersection.Item2);
                var lightingColor = GetLighting(position, normal);

                return surfaceColor * lightingColor;
            }

            Color GetColorAtPixel(Vector2 pixelCoordinate, Vector2 resolution)
            {
                var screenCoord = (-resolution + 2.0f * pixelCoordinate) / resolution.Y;

                var cameraPosition = Vector3d.Zero;
                var cameraDirection = -Vector3d.UnitZ;
                var cameraRight = Vector3d.Cross(cameraDirection, Vector3d.UnitY);
                var cameraUp = Vector3d.Cross(cameraRight, cameraDirection);

                var ray =
                    Math.Ray.Build(
                        cameraPosition,
                        screenCoord.X * cameraRight + screenCoord.Y * cameraUp + 2.5f * cameraDirection);

                var color = GetColorAtRay(ray).Pow(0.45);

                return Color.FromRGB(color.X, color.Y, color.Z);
            }

            RenderScene =
                (width, height, random) =>
                    Enumerable.Range(0, height)
                        .Select(y =>
                            Enumerable.Range(0, width)
                                .Select(x =>
                                    new Vector2(x, y))
                                //.AsParallel()
                                //.AsOrdered()
                                .Select(pixel =>
                                    GetColorAtPixel(
                                        pixel,
                                        new Vector2(width, height)))
                                .ToArray())
                        .ToArray();
        }

        public static Scene2 BuildScene() =>
            new Scene2();

        public readonly Func<int, int, Func<int, int>, Color[][]> RenderScene;
    }
}
