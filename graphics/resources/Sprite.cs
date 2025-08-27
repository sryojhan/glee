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

    public Sprite(Texture texture, string name, Rectangle source) {

        Name = name;

        rawTexture = texture;
        sourceRectangle = source;
    }


    public override bool Load(string name, ContentManager _)
    {
        Name = name;
        return false;
    }


    public void Render(Vector2 position, Vector2 size, float rotation = 0, Material material = null)
    {
        Renderer.Render(this, position, size, sourceRectangle, rotation, material);
    }

    public static Sprite Create(Texture texture, string name, Point position, Point size) {

        return Get<Resources>().CreateSprite(texture, name, position, size);
    }


}