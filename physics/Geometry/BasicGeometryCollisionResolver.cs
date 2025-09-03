
using System;

namespace Glee.Physics;



public class CircleCircleCollisionResolver : ICollisionResolver
{
    public bool Resolve(Bounds A, Bounds B)
    {
        (Circle circleA, Circle circleB) = ICollisionResolver.BoundCast<Circle, Circle>(A, B);

        float radiusSquared = (circleA.Radius + circleB.Radius) * (circleA.Radius + circleB.Radius);

        float distanceSquared =
        Vector.DistanceSquared(circleA.Position, circleB.Position);


        return distanceSquared < radiusSquared;
    }

    public float CalculatePenetration(Bounds A, Bounds B, Vector velocityA) => throw new NotImplementedException();
}


public class RectRectCollisionResolver : ICollisionResolver
{
    public bool Resolve(Bounds A, Bounds B)
    {
        (Rect circleA, Rect circleB) = ICollisionResolver.BoundCast<Rect, Rect>(A, B);

        Vector halfA = circleA.Size * 0.5f;
        Vector halfB = circleB.Size * 0.5f;

        Vector minA = circleA.Position - halfA;
        Vector maxA = circleA.Position + halfA;

        Vector minB = circleB.Position - halfB;
        Vector maxB = circleB.Position + halfB;

        return minA.X <= maxB.X && maxA.X >= minB.X &&
               minA.Y <= maxB.Y && maxA.Y >= minB.Y;
    }


    public float CalculatePenetration(Bounds A, Bounds B, Vector velocityA)
    {
        // AABB centrados
        Vector halfA = A.Size * 0.5f;
        Vector halfB = B.Size * 0.5f;

        Vector minA = A.Position - halfA;
        Vector maxA = A.Position + halfA;

        Vector minB = B.Position - halfB;
        Vector maxB = B.Position + halfB;

        // Solapamiento (si no hay, 0)
        float overlapX = MathF.Min(maxA.X, maxB.X) - MathF.Max(minA.X, minB.X);
        float overlapY = MathF.Min(maxA.Y, maxB.Y) - MathF.Max(minA.Y, minB.Y);
        if (overlapX <= 0f || overlapY <= 0f) return 0f;

        // Eje de mínima penetración (MTV)
        bool mtvIsX = overlapX < overlapY;

        // Eje de movimiento (ortogonal)
        bool movingX = MathF.Abs(velocityA.X) > MathF.Abs(velocityA.Y);
        bool movingY = MathF.Abs(velocityA.Y) > MathF.Abs(velocityA.X);

        // Si el eje de movimiento NO es el MTV, no corrijas en este paso
        if (movingX && !mtvIsX) return 0f;
        if (movingY && mtvIsX) return 0f;

        // Devuelve magnitud con signo según dirección
        if (movingX)
        {
            float v = velocityA.X > 0f
                ? (maxA.X - minB.X)   // A venía desde la izquierda
                : (minA.X - maxB.X);  // A venía desde la derecha

            return v;

        }
        else // movingY
        {
            float v = velocityA.Y > 0f
                ? (maxA.Y - minB.Y)   // A venía desde arriba (Y+)
                : (minA.Y - maxB.Y);  // A venía desde abajo (Y-)

            return v;
        }
    }


}



public class CircleRectCollisionResolver : ICollisionResolver
{
    public bool Resolve(Bounds A, Bounds B)
    {
        (Circle circle, Rect rect) = ICollisionResolver.BoundCast<Circle, Rect>(A, B);

        Vector half = rect.Size * 0.5f;
        Vector min = rect.Position - half;
        Vector max = rect.Position + half;

        // Clamp: punto más cercano del rectángulo al círculo
        float closestX = Math.Clamp(circle.Position.X, min.X, max.X);
        float closestY = Math.Clamp(circle.Position.Y, min.Y, max.Y);
        Vector closestPoint = new Vector(closestX, closestY);

        // Distancia del círculo al punto más cercano
        Vector diff = circle.Position - closestPoint;
        float distSq = diff.LengthSquared();

        return distSq <= circle.Radius * circle.Radius;
    }

    public float CalculatePenetration(Bounds A, Bounds B, Vector velocityA) => throw new NotImplementedException();
}


