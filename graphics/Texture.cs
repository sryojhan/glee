using System;
using System.Collections.Generic;
using Glee.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;


public class Texture : ITexture, IDisposable
{
    public Texture2D BaseTexture { get; set; }

    public Vector2 Size => new(Width, Height);
    public float Width => BaseTexture.Width;
    public float Height => BaseTexture.Height;

    public string Name { get; private set; }

    public Texture(string name, bool isGlobal = false)
    {
        Name = name;

        BaseTexture = (isGlobal ? GleeCore.Content : GleeCore.WorldManager.Spotlight.Content).Load<Texture2D>($"images/{name}");

        if (BaseTexture == null)
        {
            throw new Exception($"Can't load texture {name}");
        }
    }

    public void Render(Vector2 position, Vector2 size, float rotation = 0, Material material = null)
    {
        Renderer.Render(this, position, size, null, rotation, material);
    }

    public void Dispose()
    {
        BaseTexture.Dispose();
        GC.SuppressFinalize(this);
    }

    private readonly Dictionary<string, Rectangle> spriteDefinitions = [];
    private readonly Dictionary<string, Sprite> sprites = [];
    internal void RegisterSprite(string name, Sprite sprite)
    {
        sprites.Add(name, sprite);
    }

    internal Sprite GetSprite(string name)
    {
        if (sprites.TryGetValue(name, out Sprite value))
            return value;
        return null;
    }

    internal Rectangle GetDefinition(string name)
    {
        return spriteDefinitions[name];
    }

    public void CreateSpriteDefinition(string name, Point position, Point size)
    {
        spriteDefinitions[name] = new Rectangle(position, size);
    }

}
 