

namespace Glee.Physics;

public abstract class Bounds(Entity owner)
{
    public Entity entity { get; private set; } = owner;

    public Vector2 Position => entity.Position;
    public Vector2 Size => entity.Size;
}