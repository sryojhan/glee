
using Glee.Engine;

namespace Glee;


public abstract class Component: GleeObject
{
    public bool Enabled { get; set; } = true;
    public Entity Owner { get; internal set; } = null;

}