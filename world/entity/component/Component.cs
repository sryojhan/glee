
using Glee.Engine;

namespace Glee;


public abstract class Component: GleeObject
{
    public bool Enabled { get; set; } = true;
    public Entity Entity { get; internal set; } = null;
    public World World => Entity.World;
}