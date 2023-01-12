using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractal_Viewer
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

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            fractal.RenderFractal();

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUnload()
        {
            fractal.Dispose();

            base.OnUnload();
        }
    }
}
