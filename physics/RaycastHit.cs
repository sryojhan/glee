

namespace Glee.Physics;



//TODO: add collider from the collision
public readonly struct RaycastHit(Vector2 origin, Vector2 direction, Vector2 hitPosition)
{
    public Vector2 Origin { get; } = origin;
    public Vector2 Direction { get; } = direction;
    public Vector2 Hit { get; } = hitPosition;

    public float Distance => Vector2.Distance(Direction, Hit);
    public float DistanceSquared => Vector2.DistanceSquared(Direction, Hit);
}
