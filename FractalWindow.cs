using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
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

        public Vector2 offset { get; private set; }
        public Vector2 size { get; private set; }

        public FractalWindow()
        {
            size = new Vector2(3);
            offset = new Vector2();

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

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);


            GL.ShaderSource(vertexShader, File.ReadAllText(@"C:\Users\noah0\source\repos\Fractal Viewer\fractal_vert_shader.glsl"));
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, File.ReadAllText(@"C:\Users\noah0\source\repos\Fractal Viewer\universal_fractal_frag_shader.glsl"));
            GL.CompileShader(fragmentShader);

            shader = GL.CreateProgram();
            GL.AttachShader(shader, vertexShader);
            GL.AttachShader(shader, fragmentShader);

            GL.LinkProgram(shader);

            GL.DetachShader(shader, vertexShader);
            GL.DetachShader(shader, fragmentShader);

            uniformOffset = GL.GetUniformLocation(shader, "offset");
            uniformSize = GL.GetUniformLocation(shader, "scale");
            uniformIterations = GL.GetUniformLocation(shader, "iterations");
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
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.Uniform2(uniformOffset, offset);
            GL.Uniform2(uniformSize, size);
            GL.Uniform1(uniformIterations, 500);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }
    }
}
