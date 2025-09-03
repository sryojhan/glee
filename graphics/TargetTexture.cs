
using Glee.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;


public class TargetTexture : Texture
{

    public TargetTexture(string name) : this(name,
    Get<Renderer>().graphicsDevice.PresentationParameters.BackBufferWidth,
    Get<Renderer>().graphicsDevice.PresentationParameters.BackBufferHeight)
    {

    }


    public TargetTexture(string name, int width, int height): base(name)
    {
        BaseTexture = new RenderTarget2D(
            graphicsDevice: Get<Renderer>().graphicsDevice,
            width: width,
            height: height,
            mipMap: false,
            preferredFormat: SurfaceFormat.Color,
            preferredDepthFormat: DepthFormat.None
        );
    }


    public void CloneData(TargetTexture original)
    {
        BaseTexture.Dispose();

        BaseTexture = new RenderTarget2D(
            graphicsDevice: Get<Renderer>().graphicsDevice,
            width: original.Width,
            height: original.Height,
            mipMap: false,
            preferredFormat: SurfaceFormat.Color,
            preferredDepthFormat: DepthFormat.None
        );

        Color[] data = new Color[original.Width * original.Height];
        original.BaseTexture.GetData(data);
        BaseTexture.SetData(data);
    }
}