using Glee.Components;

namespace Glee.Physics;



public struct RaycastHit(Vector2 origin, Vector2 direction, Vector2 hitPosition)
{
    public Vector2 Origin { get; } = origin;
    public Vector2 Direction { get; } = direction;
    public Vector2 Hit { get; } = hitPosition;

    public readonly float Distance => Vector2.Distance(Direction, Hit);
    public readonly float DistanceSquared => Vector2.DistanceSquared(Direction, Hit);

    public Collider Collider { get; set; }
}
