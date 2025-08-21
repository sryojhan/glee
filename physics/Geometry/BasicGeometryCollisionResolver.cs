
using System;

namespace Glee.Physics;



public class CircleCircleCollisionResolver : ICollisionResolver
{
    public bool Resolve(Bounds A, Bounds B)
    {
        (Circle circleA, Circle circleB) = ICollisionResolver.BoundCast<Circle, Circle>(A, B);

        float radiusSquared = (circleA.Radius + circleB.Radius) * (circleA.Radius + circleB.Radius);

        float distanceSquared =
        Vector2.DistanceSquared(circleA.Position, circleB.Position);


        return distanceSquared < radiusSquared;
    }
}


public class RectRectCollisionResolver : ICollisionResolver
{
    public bool Resolve(Bounds A, Bounds B)
    {
        throw new NotImplementedException();
    }
}


public class CircleRectCollisionResolver : ICollisionResolver
{
    public bool Resolve(Bounds A, Bounds B)
    {
        throw new NotImplementedException();
    }
}


