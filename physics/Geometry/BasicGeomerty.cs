
using System;

namespace Glee.Physics;


public class Rect : Bounds
{
    public override bool Raycast(Vector2 origin, Vector2 direction, float distance, out RaycastHit hit)
    {
        hit = new RaycastHit();

        Vector2 halfSize = Size * 0.5f;
        Vector2 minPoint = Position - halfSize;
        Vector2 maxPoint = Position + halfSize;


        // Evitar división entre cero
        Vector2 invDir = new Vector2(
            direction.X != 0 ? 1f / direction.X : float.PositiveInfinity,
            direction.Y != 0 ? 1f / direction.Y : float.PositiveInfinity
        );

        // Distancia paramétrica a cada borde
        float t1 = (minPoint.X - origin.X) * invDir.X;
        float t2 = (maxPoint.X - origin.X) * invDir.X;
        float t3 = (minPoint.Y - origin.Y) * invDir.Y;
        float t4 = (maxPoint.Y - origin.Y) * invDir.Y;

        // Intervalo de entrada/salida
        float tmin = MathF.Max(MathF.Min(t1, t2), MathF.Min(t3, t4));
        float tmax = MathF.Min(MathF.Max(t1, t2), MathF.Max(t3, t4));

        // No hay colisión
        if (tmax < 0 || tmin > tmax)
            return false;

        // Primer punto válido
        float t = tmin >= 0 ? tmin : tmax;

        // Chequeamos que no exceda la distancia del rayo
        if (t < 0 || t > distance)
            return false;

        // Punto de impacto
        Vector2 hitPoint = origin + direction * t;
        hit = new RaycastHit(origin, direction, hitPoint);
        return true;

    }
}


public class Circle : Bounds
{
    public float Radius => entity.Size.X * 0.5f;

    public override bool Raycast(Vector2 origin, Vector2 direction, float distance, out RaycastHit hit)
    {
        hit = new RaycastHit();

        if (direction == Vector2.Zero)
            return false;

        direction.Normalize();


        Vector2 pointToCircle = Position - origin;

        float projection = Vector2.Dot(pointToCircle, direction);

        if (projection < 0) //Point going in a different direction
            return false;

        float circleToRaySquared = Vector2.Dot(pointToCircle, pointToCircle) - projection * projection;

        if (circleToRaySquared > Radius * Radius) //Point doesn't hit the ray
            return false;


        float distanceIntersection = MathF.Sqrt(Radius * Radius - circleToRaySquared);


        float intersection0 = projection - distanceIntersection;
        float intersection1 = projection + distanceIntersection;


        float hitDistance = float.MaxValue;

        if (intersection0 >= 0 && intersection0 <= distance)
            hitDistance = intersection0;
        else if (intersection1 >= 0 && intersection1 <= distance)
            hitDistance = intersection1;

        if (hitDistance != float.MaxValue)  //No intersection inside the range
        {
            Vector2 hitPoint = origin + direction * hitDistance;

            hit = new RaycastHit(origin, direction, hitPoint);
            return true;
        }

        return false;
    }

}