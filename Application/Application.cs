using System;
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

                    const int size = 256;

                    var texData = new byte[size * size];

                    for (var i = 0; i < size * size; ++i)
                    { 
                        texData[i] = 255;
                    }

                    Func<int, int> random = new Random().Next;

                    for (var i = 0; i < 10; ++i)
                    {
                        texData[random(size) * size + random(size)] = 0;
                    }

                    TextureId = GL.GenTexture();
                    GL.BindTexture(TextureTarget.Texture2D, TextureId);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureParameterName.ClampToEdge);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureParameterName.ClampToEdge);
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Luminance8, size, size, 0, PixelFormat.Luminance, PixelType.UnsignedByte, texData);
                    var errorCode = GL.GetError();

                    if (errorCode != ErrorCode.NoError)
                    {
                        game.Exit();
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
