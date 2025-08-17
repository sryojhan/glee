

using Glee.Engine;
using Microsoft.Xna.Framework;

namespace Glee.ECS;


/// <summary>
/// 
/// </summary>
public abstract class Entity : GleeObject
{
    public string Name { get; set; }
    public Vector2 Position { get; protected set; }
    public Entity Parent { get; protected set; }


    public World World { get; protected set; }


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