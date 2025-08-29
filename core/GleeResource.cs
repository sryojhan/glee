using System;
using Microsoft.Xna.Framework.Content;

namespace Glee.Engine;

public abstract class GleeResource(string name) : GleeObject, IDisposable
{
    public string Name { get; protected set; } = name;
    protected abstract IDisposable DisposableObj { get; }


    public void Dispose()
    {
        DisposableObj?.Dispose();
        GC.SuppressFinalize(this);
    }
}