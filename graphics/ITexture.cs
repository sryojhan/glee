
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;



public interface ITexture
{
    public Texture2D BaseTexture { get; }
    public Vector2 Size { get; }
    public float Width { get; }
    public float Height { get; }


    public void Render(Vector2 position, Vector2 size, float rotation = 0);
}
