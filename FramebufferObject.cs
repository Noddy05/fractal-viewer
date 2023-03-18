using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalViewer
{
    class FramebufferObject : IDisposable
    {
        //Protected means only this class, but its subclasses can call the function
        protected int framebuffer;
        protected int depthbuffer;
        protected int depthTexture;
        protected int texture;
        protected Vector2i size = new(1920, 1080);

        public int GetFramebuffer() => framebuffer;
        public int GetDepthbuffer() => depthbuffer;
        public int GetDepthTexture() => depthTexture;
        public int GetTexture() => texture;
        public Vector2i GetSize() => size;
        public void SetSize(Vector2i size) => this.size = size;

        public void Dispose()
        {
            GL.DeleteFramebuffer(framebuffer);
            GL.DeleteRenderbuffer(depthbuffer);
            GL.DeleteTexture(texture);
        }

        public void BindCurrentFramebuffer()
        {
            BindFramebuffer(framebuffer, size.X, size.Y);
        }

        public void UnbindFramebuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            Window? gameWindow = Program.GetWindow();
            if (gameWindow == null)
                throw new Exception("No window to render to!");

            GL.Viewport(0, 0, gameWindow.Size.X, gameWindow.Size.Y);
        }

        protected void BindFramebuffer(int frameBuffer, int width, int height)
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            GL.Viewport(0, 0, width, height);
        }

        protected int CreateFramebuffer()
        {
            int framebuffer = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
            return framebuffer;
        }

        protected int CreateTextureAttachment(int width, int height)
        {
            int texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0,
                PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, texture, 0);

            return texture;
        }

        protected int CreateDepthTextureAttachment(int width, int height)
        {
            int texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32, width, height, 0,
                PixelFormat.DepthComponent, PixelType.UnsignedByte, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, texture, 0);

            return texture;
        }

        protected int CreateDepthBufferAttachment(int width, int height)
        {
            int depthBuffer = GL.GenBuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment,
                RenderbufferTarget.Renderbuffer, depthBuffer);
            return depthBuffer;
        }
    }
}
