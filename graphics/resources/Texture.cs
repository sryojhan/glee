using System;
using System.Collections.Generic;
using Glee.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;


public class Texture : GleeResource, ITexture
{
    public Texture2D BaseTexture { get; set; }

    public Point Size => new(Width, Height);
    public int Width => BaseTexture.Width;
    public int Height => BaseTexture.Height;

    protected override IDisposable DisposableObj => BaseTexture;

    protected Texture(string name) : base(name) { }

    private Texture(string name, Texture2D texture): base(name)
    {
        BaseTexture = texture;
    }

    public static Texture Create(string name)
    {
        try
        {
            Texture2D tex = Get<Resources>().ActiveContentManager.Load<Texture2D>($"images/{name}");
            return new Texture(name, tex);
        }
        catch (ContentLoadException)
        {
            GleeError.AssetNotFound(name);
            return null;
        }
    }

    public void Render(Vector2 position, Vector2 size, float rotation = 0, Material material = null)
    {
        Renderer.Render(this, position, size, null, rotation, material);
    }

}
 