using Glee.Engine;
using Microsoft.Xna.Framework;


namespace Glee;


public abstract class Entity : GleeObject
{
    public string Name { get; set; }
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = Vector2.One;
    public float UniformScale { set { Scale = new Vector2(value, value); }}
    public float Rotation { get; protected set; } = 0;

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