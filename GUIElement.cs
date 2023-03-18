using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalViewer
{
    class GUIElement 
    {
        private Vector2 position;
        private Vector2 size;
        private int texture;

        private int vbo;
        private int ibo;
        private int vao;

        private int shaderProgram;
        private Window window;

        public void SetTexture(int texture) => this.texture = texture;
        public Vector2 GetSize() => size;
        public Vector2 GetPosition() => position;

        public GUIElement(int shaderProgram, float x, float y, float width, float height, int texture)
        {
            this.texture = texture;
            window = Program.GetWindow();
            this.shaderProgram = shaderProgram;
            position = new Vector2(x, y);
            size = new Vector2(width, height);

            float[] vertices = new float[]
            {
                x * 2-1, y * 2-1,    0, 0,
                x * 2-1,  y * 2-1+height * 2,    0, 1,
                 x * 2-1+width * 2,  y * 2-1+height * 2,    1, 1,
                 x * 2-1+width * 2, y * 2-1,    1, 0
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
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.BindVertexArray(0);
        }

        public bool HoveringWindow()
        {
            return
                window.MousePosition.X > window.Size.X * ( position.X + 0.5 - MathF.Abs(size.X / 2))
             && window.MousePosition.X < window.Size.X * ( position.X + 0.5 + MathF.Abs(size.X / 2))
             && window.MousePosition.Y > window.Size.Y * (-position.Y + 0.5 - MathF.Abs(size.Y / 2))
             && window.MousePosition.Y < window.Size.Y * (-position.Y + 0.5 + MathF.Abs(size.Y / 2));
        }

        public void DrawElement()
        {
            GL.UseProgram(shaderProgram);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo!);

            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }
    }
}
