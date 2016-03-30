using System;
using System.Linq;
using Citrine.Core;

namespace Citrine.Application
{
    public static class TestScene
    {
        public static readonly Func<int, int, Func<int, int>, Color[][]> GenerateScene =
            (width, height, random) =>
                Enumerable.Repeat(Color.Black, height)
                    .Select(c => Enumerable.Repeat(c, width))
                    .Select((row, rowIndex) =>
                        row
                            .Select((_, columnIndex) =>
                                Color.FromRGB(
                                    (float)columnIndex / width,
                                    (float)rowIndex / height,
                                    (float)random(255) / 255))
                            .ToArray())
                    .ToArray();
    }

    public static class EntryPoint
    {
        [STAThread]
        public static void Main()
        {
            using (var application = new Application("citrine", TestScene.GenerateScene))
            {
                application.Run();
            }
        }
    }
}
