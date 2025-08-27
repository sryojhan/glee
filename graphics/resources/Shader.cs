
using Glee.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace Glee.Graphics;

public class Shader
{
    internal Effect effect;

    public Shader(string name)
    {
        effect = GleeCore.Content.Load<Effect>($"shaders/{name}");
    }
}