
using System;
using Glee.Engine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;

public class Shader : GleeResource
{
    internal Effect effect;
    protected override IDisposable DisposableObj => effect;

    private Shader(string name, Effect effect)
    {
        Name = name;
        this.effect = effect;
    }

    public static Shader Create(string name)
    {
        try
        {
            Effect effect = GleeCore.Content.Load<Effect>($"shaders/{name}");
            return new Shader(name, effect);
        }
        catch (ContentLoadException)
        {
            GleeError.AssetNotFound(name, "shaders");
            return null;
        }
    }
}