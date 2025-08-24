
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
        (Rect circleA, Rect circleB) = ICollisionResolver.BoundCast<Rect, Rect>(A, B);

        Vector2 halfA = circleA.Size * 0.5f;
        Vector2 halfB = circleB.Size * 0.5f;

        Vector2 minA = circleA.Position - halfA;
        Vector2 maxA = circleA.Position + halfA;

        Vector2 minB = circleB.Position - halfB;
        Vector2 maxB = circleB.Position + halfB;

        return minA.X <= maxB.X && maxA.X >= minB.X &&
               minA.Y <= maxB.Y && maxA.Y >= minB.Y;
    }

}


public class CircleRectCollisionResolver : ICollisionResolver
{
    public bool Resolve(Bounds A, Bounds B)
    {
        throw new NotImplementedException();
    }
}


