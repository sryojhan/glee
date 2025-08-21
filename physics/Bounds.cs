

namespace Glee.Physics;

public abstract class Bounds(Entity owner)
{
    public Entity entity { get; private set; } = owner;

    public Vector2 Position => entity.Position;
    public Vector2 Size => entity.Size;


    public abstract bool Raycast(Vector2 origin, Vector2 direction, float distance);
    public abstract bool Raycast(Vector2 origin, Vector2 direction, float distance, out RaycastHit hit);
}