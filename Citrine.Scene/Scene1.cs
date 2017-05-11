using System;
using System.Linq;
using Citrine.Core;

namespace Citrine.Scene
{
    public static class Scene1
    {
        public static Color[][] GenerateScene(int width, int height, Func<int, int> random)
        {
            var black =
                Enumerable.Repeat(Color.Black, height)
                    .Select(c => Enumerable.Repeat(c, width).ToArray())
                    .ToArray();
            return
                black
                    .Select((row, rowIndex) =>
                        row
                            .Select((_, columnIndex) =>
                                Color.FromRGB(
                                    (double)columnIndex / width,
                                    (double)rowIndex / height,
                                    random(255) / 255.0))
                            .ToArray())
                    .ToArray();
        }
    }
}
