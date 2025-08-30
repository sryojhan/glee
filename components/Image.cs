using Glee;
using Glee.Behaviours;
using Glee.Engine;
using Glee.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Glee.Components;

public class Image : ComponentRaw, IRenderizable
{
    public ITexture texture { get; set; }
    public Material material { get; set; }

    public void Render()
    {
        if (texture == null) return;

        texture.Render(entity.Position, entity.Size, entity.Rotation, material);
    }


    public void SetNativeSize()
    {
        entity.Size = new Vector2(texture.Width, texture.Height);
    }

}
