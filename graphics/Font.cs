using System;
using Glee.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;


public class Font
{
    internal SpriteFont font;

    public Font(string fontName)
    {
        font = GleeCore.Content.Load<SpriteFont>($"fonts/{fontName}");
    }

    public Vector2 CalculateWidth(string text)
    {
        return font.MeasureString(text);
    }
}