
using Glee.Engine;

namespace Glee;


public abstract class Component : GleeObject
{
    public EntityComposed entity { get; internal set; } = null;
    public World world => entity.world;
    public Time Time => world.Time;
    public bool Enabled { get; set; } = true;


    public ComponentType GetComponent<ComponentType>() where ComponentType : Component => entity.GetComponent<ComponentType>();
}