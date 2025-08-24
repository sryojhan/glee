
using System;

namespace Glee.Physics;


public class Rect : Bounds
{
    public override bool Raycast(Vector2 origin, Vector2 direction, float distance)
    {
        return Raycast(origin, direction, distance, out _);
    }

    public override bool Raycast(Vector2 origin, Vector2 direction, float distance, out RaycastHit hit)
    {
        hit = new RaycastHit();

        if (direction == Vector2.Zero)
            return false;

        direction.Normalize();

        return false;

    }
}


public class Circle : Bounds
{
    public float Radius => entity.Size.X * 0.5f;

    public override bool Raycast(Vector2 origin, Vector2 direction, float distance)
    {
        return Raycast(origin, direction, distance, out _);
    }

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