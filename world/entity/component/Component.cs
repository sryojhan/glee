
using Glee.Engine;

namespace Glee;


public abstract class Component : GleeObject
{
    public Entity entity { get; internal set; } = null;
    public World world => entity.world;
    public Time Time => world.Time;
    public bool Enabled { get; set; } = true;
}