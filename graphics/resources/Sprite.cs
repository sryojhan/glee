using System;
using Glee.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;



public class Sprite : GleeResource, ITexture
{
    public Texture2D BaseTexture => rawTexture.BaseTexture;

    public Point Size => sourceRectangle.Size;

    public int Width => sourceRectangle.Width;

    public int Height => sourceRectangle.Height;

    private readonly Texture rawTexture;
    private readonly Rectangle sourceRectangle;


    protected override IDisposable DisposableObj => null;


    private Sprite(Texture texture, string name, Rectangle source): base(name)
    {
        rawTexture = texture;
        sourceRectangle = source;
    }

    public static Sprite Create(Texture texture, string name, Rectangle source)
    {
        if (texture == null)
        {
            GleeError.InvalidInitialization($"Sprite [{nameof(texture)} is null]");
            return null;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            GleeError.InvalidInitialization($"Sprite [{nameof(name)} is null or empty]");
            return null;
        }

        if (source.Left < 0 || source.Right > texture.Width || source.Top < 0 || source.Bottom > texture.Height)
        {
            GleeError.InvalidInitialization($"Sprite [source rect outside of bounds Rect({source}) TextureSize({texture.Size})]");
            return null;
        }


        return new Sprite(texture, name, source);
    }


    public void Render(Vector2 position, Vector2 size, float rotation = 0, Material material = null)
    {
        Renderer.Render(this, position, size, sourceRectangle, rotation, material);
    }

    public static Sprite Create(Texture texture, string name, Point position, Point size, bool combineName)
    {

        return Get<Resources>().CreateSprite(texture, name, position, size, combineName);
    }


    public static string CombineName(Texture texture, string spriteName)
    {
        return $"{texture.Name}: {spriteName}";
    }

}