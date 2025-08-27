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


    public override bool Load(string name, ContentManager content)
    {
        Name = name;

        try
        {
            BaseTexture = content.Load<Texture2D>($"images/{name}");
            return true;
        }
        catch (ContentLoadException)
        {
            BaseTexture = null;
            GleeError.AssetNotFound(name);
            return false;
        }
    }


    public void Render(Vector2 position, Vector2 size, float rotation = 0, Material material = null)
    {
        Renderer.Render(this, position, size, null, rotation, material);
    }

}
 