using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Diagnostics;
using System.IO;

namespace FractalViewer
{
    class FractalWindow : IDisposable
    {
        private int vbo;
        private int vao;
        private int ibo;
        private int shader;

        private int uniformOffset;
        private int uniformSize;
        private int uniformIterations;
        private int uniformT;
        private int uniformXRatio;
        private int uniformNPower;
        public float NPower = 2;

        public Vector2 offset { get; private set; }
        public Vector2 size { get; private set; }

        Stopwatch sw;
        public FractalWindow()
        {
            sw = Stopwatch.StartNew();
            size = new Vector2(4);
            offset = new Vector2(0, 0);

            float[] vertices = new float[]
            {
                -1, -1,
                -1,  1,
                 1,  1,
                 1, -1
            };

            //  1----2
            //  |    |
            //  0----3
            uint[] indices = new uint[]{
                0, 1, 3,
                1, 2, 3
            };

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticCopy);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            ibo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticCopy);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);

            shader = Shader.GenerateShader(@"../../../fractal_vert_shader.glsl",
                @"../../../fractal_frag_shader.glsl");

            uniformXRatio = GL.GetUniformLocation(shader, "xRatio");
            uniformOffset = GL.GetUniformLocation(shader, "offset");
            uniformSize = GL.GetUniformLocation(shader, "scale");
            uniformIterations = GL.GetUniformLocation(shader, "iterations");
            uniformNPower = GL.GetUniformLocation(shader, "NPower");
            uniformT = GL.GetUniformLocation(shader, "t");
        }

        public void Dispose()
        {
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(vbo);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(ibo);

            GL.UseProgram(0);
            GL.DeleteProgram(shader);
        }
        public void SetOffset(Vector2 position)
        {
            GL.Uniform2(uniformOffset, position);
            offset = position;
        }

        public void SetSize(Vector2 size)
        {
            GL.Uniform2(uniformSize, size);
            this.size = size;
        }

        public void RenderFractal()
        {
            GL.UseProgram(shader);
            GL.Uniform1(uniformNPower, NPower);
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.Uniform2(uniformOffset, offset);
            Vector2 windowSize = Program.GetWindow().Size;
            GL.Uniform2(uniformSize, size * windowSize / 720.0f);
            float t = sw.ElapsedMilliseconds;
            //GL.Uniform1(uniformIterations, MathF.Min(MathF.Pow(t, 3.5f), 1000));
            GL.Uniform1(uniformIterations, 1000f);
            GL.Uniform1(uniformT, t);

            GL.Uniform1(uniformXRatio, 1);

            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }
    }
}
