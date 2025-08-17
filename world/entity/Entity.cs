using Glee.Engine;
using Microsoft.Xna.Framework;


namespace Glee;


public abstract class Entity : GleeObject
{
    public string Name { get; set; }
    public Vector2 Position { get; protected set; }
    public Entity Parent { get; protected set; }


    public World World { get; protected set; }


    public Entity(World world)
    {
        Name = "New entity";
        World = world;
    }

    public Entity(string name, World world)
    {
        Name = name;
        World = world;
    }

    public Entity(string name, Entity parent, World world) : this(name, world)
    {
        Parent = parent;
    }

}