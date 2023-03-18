using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace FractalViewer
{
    class Window : GameWindow
    {
        public FractalWindow fractal;
        private MandelbrotFractalViewer mandelbrotViewer;
        private int mandelbrotFramebufferTexture;
        private GUIElement mandelbrotViewerGUI;

        private int juliaFramebufferTexture;
        private JuliaFractalViewer juliaViewer;
        private GUIElement juliaViewerGUI;
        public JuliaWindow juliaWindow;
        private int juliaShader;
        private int mandelbrotViewerShader;

        public Window()
            : base(GameWindowSettings.Default, new NativeWindowSettings
            {
                Title = "Fractal Viewer",
                Size = new Vector2i(1920, 1080)
            })
        {
            CenterWindow();
        }

        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);

            fractal = new FractalWindow();
            mandelbrotViewer = new MandelbrotFractalViewer();
            mandelbrotFramebufferTexture = mandelbrotViewer.GetTexture();

            mandelbrotViewerShader = Shader.GenerateShader(@"C:\Users\noah0\source\repos\Fractal Viewer\mandelbrotViewerVert.glsl",
                @"C:\Users\noah0\source\repos\Fractal Viewer\mandelbrotViewerFrag.glsl");

            juliaViewer = new JuliaFractalViewer();
            juliaFramebufferTexture = juliaViewer.GetTexture();
            juliaShader = Shader.GenerateShader(@"C:\Users\noah0\source\repos\Fractal Viewer\julia_vert.glsl",
                @"C:\Users\noah0\source\repos\Fractal Viewer\julia_frag.glsl");
            float heightToWidthRatio = Size.Y / (float)Size.X;
            float width = 1f * heightToWidthRatio;
            juliaViewerGUI = new GUIElement(mandelbrotViewerShader, 1 - width, 0, width, 1f, juliaFramebufferTexture);

            mandelbrotViewerGUI = new GUIElement(mandelbrotViewerShader, 0, 0, 1 - width, 1, mandelbrotFramebufferTexture);
            fractal.SetSize(mandelbrotViewerGUI.GetSize());

            juliaWindow = new JuliaWindow(juliaShader, 0, 0, 1, 1, -1);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {

            Vector2 mandelbrotWindowSize = mandelbrotViewerGUI.GetSize();
            if (Input.ButtonDown("Button1"))
            {
                fractal.SetOffset(fractal.offset - Input.mouseDeltaPosition 
                    * new Vector2(1, -1) / (new Vector2(720f, 720f) / fractal.size) / mandelbrotWindowSize);
            }
            float mouseWheel = Input.mouseWheel;
            if (mouseWheel != 0)
            {
                if (mouseWheel > 0)
                {
                    //zoom in
                    fractal.SetSize(fractal.size / 1.5f);
                    
                    /*fractal.SetOffset(fractal.offset + (Input.mousePosition - Size / 2) * new Vector2(1, -1)
                        / 720f * fractal.size / 2f / mandelbrotWindowSize);*/
                    
                }
                else
                {
                    //zoom out
                    fractal.SetSize(fractal.size * 1.5f);
                    /*
                    fractal.SetOffset(fractal.offset - (Input.mousePosition 
                        - new Vector2(Size.X * mandelbrotViewerGUI.GetSize().X / 2f, Size.Y / 2f)) * new Vector2(1, -1) 
                        / 720f * fractal.size / 2f / mandelbrotWindowSize); */


                }
            }

            mandelbrotViewer.BindCurrentFramebuffer();

            GL.Clear(ClearBufferMask.ColorBufferBit);
            fractal.RenderFractal();

            mandelbrotViewer.UnbindFramebuffer();

            juliaViewer.BindCurrentFramebuffer();

            GL.Clear(ClearBufferMask.ColorBufferBit);
            //float xRatio = Size.X / Size.Y;
            juliaWindow.RenderFractal((Input.mousePosition - Size / 2 + new Vector2(Size.Y / 2, 0)) * new Vector2(1, -1) 
                / (new Vector2(720f, 720f) / fractal.size) / mandelbrotWindowSize + fractal.offset);

            juliaViewer.UnbindFramebuffer();

            mandelbrotViewerGUI.DrawElement();

            juliaViewerGUI.DrawElement();

            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
            juliaViewer = new JuliaFractalViewer();
            juliaFramebufferTexture = juliaViewer.GetTexture();
            juliaShader = Shader.GenerateShader(@"C:\Users\noah0\source\repos\Fractal Viewer\julia_vert.glsl",
                @"C:\Users\noah0\source\repos\Fractal Viewer\julia_frag.glsl");
            float heightToWidthRatio = Size.Y / (float)Size.X;
            float width = 1f * heightToWidthRatio;
            juliaViewerGUI = new GUIElement(mandelbrotViewerShader, 1 - width, 0, width, 1f, juliaFramebufferTexture);

            juliaWindow = new JuliaWindow(juliaShader, 0, 0, 1, 1, -1);
        }

        protected override void OnUnload()
        {
            fractal.Dispose();

            base.OnUnload();
        }
    }
}
