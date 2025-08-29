using System;
using Glee.Engine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;


public class Font : GleeResource
{
    internal SpriteFont font;

    protected override IDisposable DisposableObj => null;

    private Font(string name, SpriteFont font): base(name)
    {
        this.font = font;
    }


    public static Font Create(string fontName)
    {
        try
        {
            SpriteFont font = Get<Resources>().ActiveContentManager.Load<SpriteFont>($"fonts/{fontName}");
            return new Font(fontName, font);
        }
        catch (ContentLoadException)
        {
            GleeError.AssetNotFound(fontName);
            return null;
        }
    }



    public Vector2 ComputeSize(string text)
    {
        return font.MeasureString(text);
    }
}