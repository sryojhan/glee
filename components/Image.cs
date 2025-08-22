using Glee;
using Glee.Behaviours;
using Glee.Engine;
using Glee.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Glee.Components;

public class Image : Component, IRenderizable
{
    public ITexture Texture { get; set; }


    public void Render()
    {
        if (Texture == null) return;

        Texture.Render(entity.Position, entity.Size, entity.Rotation);
    }


    public void SetNativeSize()
    {
        entity.Size = new Vector2(Texture.Width, Texture.Height);
    }

}
