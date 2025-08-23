using Glee.Behaviours;

namespace Glee.Graphics;



public class Text : Component, IRenderizable
{
    // Text properties
    public string Content { get; set; } = "";

    public Font font { get; set; }

    public void Render()
    {
        if (font == null || string.IsNullOrWhiteSpace(Content)) return;

        Renderer.RenderText(Content, font, entity.Position);
    }
}