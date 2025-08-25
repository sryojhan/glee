
using Glee.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;


public class TargetTexture : Texture
{

    public TargetTexture(string name) : this(name,
    GleeCore.Renderer.graphicsDevice.PresentationParameters.BackBufferWidth,
    GleeCore.Renderer.graphicsDevice.PresentationParameters.BackBufferHeight)
    {

    }


    public TargetTexture(string name, int width, int height) : base(name)
    {
        BaseTexture = new RenderTarget2D(
            graphicsDevice: GleeCore.Renderer.graphicsDevice,
            width: width,
            height: height,
            mipMap: false,
            preferredFormat: SurfaceFormat.Color,
            preferredDepthFormat: DepthFormat.None
        );
    }


}