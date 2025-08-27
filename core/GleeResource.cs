using System;
using Microsoft.Xna.Framework.Content;

namespace Glee.Engine;

public abstract class GleeResource : GleeObject, IDisposable
{
    public string Name { get; protected set; }
    protected IDisposable DisposableObj { get; set; }


    public abstract bool Load(string path, ContentManager content);


    public void Dispose()
    {
        DisposableObj?.Dispose();
        GC.SuppressFinalize(this);
    }
}