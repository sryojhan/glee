using Glee.Components;

namespace Glee.Physics;



public struct RaycastHit(Vector origin, Vector direction, Vector hitPosition)
{
    public Vector Origin { get; } = origin;
    public Vector Direction { get; } = direction;
    public Vector Hit { get; } = hitPosition;

    public readonly float Distance => Vector.Distance(Direction, Hit);
    public readonly float DistanceSquared => Vector.DistanceSquared(Direction, Hit);

    public Collider Collider { get; set; }
}
