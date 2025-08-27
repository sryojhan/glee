
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;



public interface ITexture
{
    public Texture2D BaseTexture { get; }
    public Point Size { get; }
    public int Width { get; }
    public int Height { get; }


    public void Render(Vector2 position, Vector2 size, float rotation = 0, Material material = null);
}
