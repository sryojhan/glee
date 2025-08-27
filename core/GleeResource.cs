using System;
using Microsoft.Xna.Framework.Content;

namespace Glee.Engine;

public abstract class GleeResource : GleeObject, IDisposable
{
    public string Name { get; protected set; }
    protected abstract IDisposable DisposableObj { get; }


    public void Dispose()
    {
        DisposableObj?.Dispose();
        GC.SuppressFinalize(this);
    }
}