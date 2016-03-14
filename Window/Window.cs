using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Citrine.Window
{
    public static class Window
    {
        private static float Timer = 0.0f;

        [STAThread]
        public static void Main()
        {
            using (var game = new GameWindow())
            {
                EventHandler<EventArgs> OnLoad =
                    (sender, e) =>
                    {
                        // setup settings, load textures, sounds
                        game.VSync = VSyncMode.On;
                    };

                EventHandler<EventArgs> OnResize =
                    (sender, e) =>
                    {
                        GL.Viewport(0, 0, game.Width, game.Height);
                    };

                EventHandler<FrameEventArgs> OnUpdate =
                    (sender, e) =>
                    {
                        // add game logic, input handling
                        if (game.Keyboard[Key.Escape])
                        {
                            game.Exit();
                        }
                    };

                EventHandler<FrameEventArgs> OnRender =
                    (sender, e) => 
                    {
                        Timer += (float) e.Time;
                        var angle = (float) (10.0f * Timer * Math.PI);

                        // render graphics
                        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                        GL.MatrixMode(MatrixMode.Projection);
                        GL.LoadIdentity();
                        double aspectRatio = (float)game.Width / game.Height;
                        GL.Ortho(-aspectRatio, aspectRatio, -1.0, 1.0, 0.0, 4.0);
                        GL.Rotate(angle, Vector3.UnitZ);

                        GL.Begin(PrimitiveType.Triangles);

                        GL.Color3(Color.MidnightBlue);
                        GL.Vertex2(-1.0f, 1.0f);
                        GL.Color3(Color.SpringGreen);
                        GL.Vertex2(-1.0f, -1.0f);
                        GL.Color3(Color.Ivory);
                        GL.Vertex2(1.0f, -1.0f);

                        GL.Color3(Color.MidnightBlue);
                        GL.Vertex2(-1.0f, 1.0f);
                        GL.Color3(Color.SpringGreen);
                        GL.Vertex2(1.0f, -1.0f);
                        GL.Color3(Color.Ivory);
                        GL.Vertex2(1.0f, 1.0f);

                        GL.End();

                        game.SwapBuffers();
                    };

                game.Load += OnLoad;
                game.Resize += OnResize;
                game.UpdateFrame += OnUpdate;
                game.RenderFrame += OnRender;

                // Run the game at 60 updates per second
                game.Run(60.0, 120.0);
            }
        }
    }
}
