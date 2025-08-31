using Glee.Engine;


namespace Glee;


public class EntityRaw : GleeObject
{
    public string Name { get; set; }
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Size { get; set; } = Vector2.One;
    public Vector2 HalfSize => Size * 0.5f;
    public float UniformSize { set { Size = new Vector2(value, value); } }
    public float Width { get { return Size.X; } set { Size = new Vector2(value, Size.Y); } }
    public float Height { get { return Size.Y; } set { Size = new Vector2(Size.X, value); } }

    public Vector2 Scale { get; set; } = Vector2.One;
    public float UniformScale { set { Scale = new Vector2(value, value); } }
    public float Rotation { get; protected set; } = 0;
    public EntityRaw Parent { get; protected set; }
    public World world { get; protected set; }
    public Time Time => world.Time;

    public EntityRaw(World world)
    {
        Name = "New entity";
        this.world = world;
    }

    public EntityRaw(string name, World world)
    {
        Name = name;
        this.world = world;
    }

    public EntityRaw(string name, EntityRaw parent, World world) : this(name, world)
    {
        Parent = parent;
    }


    public override string ToString()
    {
        return Name;
    }


}