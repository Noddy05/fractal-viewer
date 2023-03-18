using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Diagnostics;
using System.IO;

namespace FractalViewer
{
    class JuliaWindow 
    {
        private GUIElement GUIElement;
        private int uniformC;
        private int uniformIterations;
        private int uniformNPower;
        private int uniformT;
        private int shaderProgram;
        public float NPower = 2;

        Stopwatch sw;
        public JuliaWindow(int shaderProgram, float x, float y, float width, float height, int texture)
        {
            sw = Stopwatch.StartNew();
            this.shaderProgram = shaderProgram;
            GUIElement = new(shaderProgram, x, y, width, height, texture);
            uniformC = GL.GetUniformLocation(shaderProgram, "c");
            uniformIterations = GL.GetUniformLocation(shaderProgram, "iterations");
            uniformT = GL.GetUniformLocation(shaderProgram, "t");
            uniformNPower = GL.GetUniformLocation(shaderProgram, "NPower");
        }

        public void RenderFractal(Vector2 v)
        {
            float t = sw.ElapsedMilliseconds;
            GL.UseProgram(shaderProgram);
            GL.Uniform1(uniformIterations, 1000);
            GL.Uniform1(uniformNPower, NPower);
            GL.Uniform1(uniformT, t);
            GL.Uniform2(uniformC, v);
            GUIElement.DrawElement();
        }
    }
}
