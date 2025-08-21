using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;


public class RawTexture(Texture2D texture) : ITexture, IDisposable
{
    public Texture2D BaseTexture { get; set; } = texture ?? throw new System.Exception("Tried to instantiate a non existing texture");

    public Vector2 Size => new(Width, Height);
    public float Width => BaseTexture.Width;
    public float Height => BaseTexture.Height;

    public void Render(Vector2 position, Vector2 size, float rotation = 0)
    {
        Renderer.Render(BaseTexture, position, size, null, null, rotation);
    }

    public void Dispose()
    {
        BaseTexture.Dispose();
        GC.SuppressFinalize(this);
    }
}
 