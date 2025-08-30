using Glee.Engine;


namespace Glee;


public class EntityRaw : GleeObject
{
    public string Name { get; set; }
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Size { get; set; } = Vector2.Zero;
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


    //Alignments

    //TODO: move this methods to an inner class

    public void AlignTopLeft(Vector2 point)
    {
        Position = point + new Vector2(HalfSize.X, -HalfSize.Y);
    }
    public void AlignTopRight(Vector2 point)
    {
        Position = point + new Vector2(-HalfSize.X, -HalfSize.Y);
    }

    public void AlignBottomRight(Vector2 point)
    {
        Position = point + new Vector2(-HalfSize.X, HalfSize.Y);
    }

    public void AlignBottomLeft(Vector2 point)
    {
        Position = point + new Vector2(HalfSize.X, HalfSize.Y);
    }


    public Vector2 TopLeft => Position + new Vector2(-HalfSize.X, HalfSize.Y);
    public Vector2 TopRight => Position + new Vector2(HalfSize.X, HalfSize.Y);
    public Vector2 BottomRight => Position + new Vector2(HalfSize.X, -HalfSize.Y);
    public Vector2 BottomLeft => Position + new Vector2(-HalfSize.X, -HalfSize.Y);


}