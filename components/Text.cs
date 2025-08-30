using Glee.Graphics;
using Glee.Behaviours;

namespace Glee.Components;



public class Text : ComponentRaw, IRenderizable
{
    // Text properties
    public string Content { get; set; } = "";

    public Font font { get; set; }

    public virtual void Render()
    {
        if (font == null || string.IsNullOrWhiteSpace(Content)) return;

    

        Renderer.RenderText(Content, font, entity.Position);
    }
}