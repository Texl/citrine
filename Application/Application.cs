using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Citrine.Application
{
    public static class Application
    {
        private static int TextureId;

        private static Func<GameWindow, EventHandler<EventArgs>> GetOnLoad =>
            game =>
                (sender, e) =>
                {
                    game.VSync = VSyncMode.On;

                    var width = game.Width;
                    var height = game.Height;

                    var colorData = Enumerable.Repeat(Color.Black, height).Select(c => Enumerable.Repeat(c, width).ToArray()).ToArray();

                    Func<int, int> random = new Random().Next;

                    for (var column = 0; column < width; ++column)
                    {
                        for (var row = 0; row < height; ++row)
                        {
                            var red = column * 255 / width;
                            var green = row * 255 / height;
                            var blue = random(Math.Max(red, green));
                            colorData[row][column] = Color.FromArgb(red, green, blue);
                        }
                    }

                    Func<Color, byte[]> colorToByteArray = color => new[] {color.R, color.G, color.B, color.A};

                    var texData = colorData.Select(row => row.Select(color => colorToByteArray(color)).SelectMany(x => x).ToArray()).SelectMany(x => x).ToArray();

                    TextureId = GL.GenTexture();
                    GL.BindTexture(TextureTarget.Texture2D, TextureId);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureParameterName.ClampToEdge);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureParameterName.ClampToEdge);
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, texData);

                    var errorCode = GL.GetError();

                    if (errorCode != ErrorCode.NoError)
                    {
                        throw new InvalidOperationException("OpenGL error.");
                    }
                };

        private static Func<GameWindow, EventHandler<EventArgs>> GetOnResize =>
            game =>
                (sender, e) =>
                {
                    GL.Viewport(0, 0, game.Width, game.Height);
                };

        private static Func<GameWindow, EventHandler<FrameEventArgs>> GetOnUpdate =>
            game =>
                (sender, e) =>
                {
                    if (game.Keyboard[Key.Escape])
                    {
                        game.Exit();
                    }
                };

        private static Func<GameWindow, EventHandler<FrameEventArgs>> GetOnRender =>
            game =>
                (sender, e) =>
                {
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    var aspectRatio = (double) game.Width / game.Height;
                    GL.Ortho(-aspectRatio, aspectRatio, -1, 1, 0, 4);

                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();

                    GL.Enable(EnableCap.Texture2D);
                    GL.BindTexture(TextureTarget.Texture2D, TextureId);

                    GL.Begin(PrimitiveType.Polygon);
                    GL.TexCoord2(0, 0);
                    GL.Vertex2(-aspectRatio, -1);
                    GL.TexCoord2(1, 0);
                    GL.Vertex2(aspectRatio, -1);
                    GL.TexCoord2(1, 1);
                    GL.Vertex2(aspectRatio, 1);
                    GL.TexCoord2(0, 1);
                    GL.Vertex2(-aspectRatio, 1);
                    GL.End();

                    game.SwapBuffers();
                };

        [STAThread]
        public static void Main()
        {
            using (var game = new GameWindow())
            {
                game.Load += GetOnLoad(game);
                game.Resize += GetOnResize(game);
                game.UpdateFrame += GetOnUpdate(game);
                game.RenderFrame += GetOnRender(game);

                game.Run(60.0, 120.0);
            }
        }
    }
}
