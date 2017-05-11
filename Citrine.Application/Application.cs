using System;
using System.Linq;
using Citrine.Core;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Citrine.Application
{
    public class Application : GameWindow
    {
        public Application(
            string title,
            Func<int, int, Func<int, int>, Color[][]> getImage)
        {
            Title = title;
            GetImage = getImage;
        }

        protected override void OnLoad(EventArgs e)
        {
            VSync = VSyncMode.On;
            TextureId = GL.GenTexture();

            Console.WriteLine(Timer.TimeAction(() => GenerateTexture(TextureId, Width, Height, Random, GetImage)));
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            Console.WriteLine(Timer.TimeAction(() => GenerateTexture(TextureId, Width, Height, Random, GetImage)));
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard[Key.Escape])
            {
                Exit();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var aspectRatio = (double)Width / Height;
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

            SwapBuffers();
        }

        private int TextureId;
        private readonly Func<int, int, Func<int, int>, Color[][]> GetImage;
        private readonly Func<int, int> Random = new Random().Next;

        private static void GenerateTexture(int textureId, int width, int height, Func<int, int> random, Func<int, int, Func<int, int>, Color[][]> getImage)
        {
            var image = getImage(width, height, random);

            var texData = new byte[width * height * Color.ByteSize];

            for (var rowIndex = 0; rowIndex < height; ++rowIndex)
            {
                for (var colIndex = 0; colIndex < width; ++colIndex)
                {
                    var color = image[rowIndex][colIndex].AsByteArray();

                    for (var byteIndex = 0; byteIndex < Color.ByteSize; ++byteIndex)
                    {
                        texData[(rowIndex * width + colIndex) * Color.ByteSize + byteIndex] = color[byteIndex];
                    }
                }
            }

            GL.BindTexture(TextureTarget.Texture2D, textureId);
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
        }
    }
}
