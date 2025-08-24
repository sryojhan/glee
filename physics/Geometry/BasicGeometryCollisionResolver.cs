
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
        (Circle circle, Rect rect) = ICollisionResolver.BoundCast<Circle, Rect>(A, B);

        Vector2 half = rect.Size * 0.5f;
        Vector2 min = rect.Position - half;
        Vector2 max = rect.Position + half;

        // Clamp: punto más cercano del rectángulo al círculo
        float closestX = Math.Clamp(circle.Position.X, min.X, max.X);
        float closestY = Math.Clamp(circle.Position.Y, min.Y, max.Y);
        Vector2 closestPoint = new Vector2(closestX, closestY);

        // Distancia del círculo al punto más cercano
        Vector2 diff = circle.Position - closestPoint;
        float distSq = diff.LengthSquared();

        return distSq <= circle.Radius * circle.Radius;
    }
}


