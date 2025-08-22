using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;



public class Sprite : ITexture
{
    public string Name { get; private set; }
    public Texture2D BaseTexture => rawTexture.BaseTexture;

    //TODO: cambiar vector2 a point al manejar texturas
    public Vector2 Size => sourceRectangle.Size.ToVector2();

    public float Width => sourceRectangle.Width;

    public float Height => sourceRectangle.Height;

    private readonly Texture rawTexture;
    private readonly Rectangle sourceRectangle;

    private Sprite(Texture texture, string name, Rectangle source) {

        Name = name;

        rawTexture = texture;
        sourceRectangle = source;

        texture.RegisterSprite(name, this);
    }


    public void Render(Vector2 position, Vector2 size, float rotation = 0)
    {
        Renderer.Render(this, position, size, sourceRectangle, Color.White, rotation);
    }

    public static Sprite Create(Texture texture, string name) {

        Sprite spr = texture.GetSprite(name);

        spr ??= new Sprite(texture, name, texture.GetDefinition(name));

        return spr;
    }


}