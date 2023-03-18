using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalViewer
{
    class JuliaFractalViewer : FramebufferObject
    {
        public JuliaFractalViewer()
        {
            InitializeFBOs();
        }

        void InitializeFBOs()
        {
            framebuffer = CreateFramebuffer();
            texture = CreateTextureAttachment(size.X, size.Y);
            depthTexture = CreateDepthTextureAttachment(size.X, size.Y);
            UnbindFramebuffer();
        }
    }
}
