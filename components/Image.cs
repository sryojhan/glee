using Glee;
using Glee.Behaviours;
using Glee.Engine;
using Glee.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Glee.Components;

public class Image : Component, IRenderizable
{
    public Texture2D Texture { get; set; }

    
    public void Render()
    {
        if (Texture == null) return;


        Renderer.Render(Texture, entity.Position, entity.Size);
    }


    public void SetNativeSize()
    {
        entity.Size = new Vector2(Texture.Width, Texture.Height);
    }

}
