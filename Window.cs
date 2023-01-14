using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace FractalViewer
{
    class Window : GameWindow
    {
        private FractalWindow fractal;

        public Window()
            : base(GameWindowSettings.Default, new NativeWindowSettings
            {
                Title = "Fractal Viewer",
                Size = new Vector2i(720, 720)
            })
        {
            CenterWindow();
        }

        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);

            fractal = new FractalWindow();

            base.OnLoad();
        }

        Vector2 prevMousePosition;
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (Input.ButtonDown("Button1"))
            {
                fractal.SetOffset(fractal.offset - Input.mouseDeltaPosition * new Vector2(1, -1) / (Size / fractal.size));
            }
            float mouseWheel = Input.mouseWheel;
            if(mouseWheel != 0)
            {
                if (mouseWheel > 0)
                {
                    fractal.SetSize(fractal.size / 1.5f);
                    fractal.SetOffset(fractal.offset + (Input.mousePosition - Size / 2) * new Vector2(1, -1) / Size * fractal.size / 2f);
                }
                else
                {
                    fractal.SetOffset(fractal.offset - (Input.mousePosition - Size / 2) * new Vector2(1, -1) / Size * fractal.size / 2f);
                    fractal.SetSize(fractal.size * 1.5f);
                }
            }

            fractal.RenderFractal();

            Context.SwapBuffers();

            prevMousePosition = Input.mousePosition;
            base.OnRenderFrame(args);
        }

        protected override void OnUnload()
        {
            fractal.Dispose();

            base.OnUnload();
        }
    }
}
