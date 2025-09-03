
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;



public interface ITexture
{
    public Texture2D BaseTexture { get; }
    public VectorInt Size { get; }
    public int Width { get; }
    public int Height { get; }


    public void Render(Vector position, Vector size, float rotation = 0, Material material = null);
}
