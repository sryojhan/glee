
using System.Collections.Generic;
using System.Linq;
using Glee.Components;
using Glee.Engine;

namespace Glee.Physics;

//TODO: change the namespace name
public static class Physics
{
    public static bool Raycast
    (
        Vector origin,
        Vector direction,
        float distance = float.MaxValue,
        World world = null,
        HashSet<EntityRaw> exclusionList = null
    )
    {
        return Raycast(origin, direction, out RaycastHit _, distance, world, exclusionList);
    }


    public static bool Raycast
    (
        Vector origin,
        Vector direction,
        out RaycastHit hit,
        float distance = float.MaxValue,
        World world = null,
        HashSet<EntityRaw> exclusionList = null
    )
    {
        hit = default;
        var result = RaycastEnumerable(origin, direction, distance, world, exclusionList);

        RaycastHit first = result.FirstOrDefault();

        if (first.Equals(default(RaycastHit)))
        {
            return false;
        }

        hit = first;
        return true;
    }


    public static RaycastHit[] RaycastAll
    (
        Vector origin,
        Vector direction,
        float distance = float.PositiveInfinity,
        World world = null,
        HashSet<EntityRaw> exclusionList = null
    )
    {
        var result = RaycastEnumerable(origin, direction, distance, world, exclusionList);
        return [.. result];
    }


    private static IEnumerable<RaycastHit> RaycastEnumerable
    (
        Vector origin,
        Vector direction,
        float distance = float.MaxValue,
        World world = null,
        HashSet<EntityRaw> exclusionList = null
    )
    {
        world ??= Services.Fetch<WorldManager>().Spotlight;

        PhysicsWorld physicsWorld = world.physicsWorld;

        foreach (Collider collider in physicsWorld.colliders)
        {
            if (exclusionList?.Contains(collider.entity) == true) continue;

            if (collider.bounds.Raycast(origin, direction, distance, out RaycastHit hit))
            {
                hit.Collider = collider;
                yield return hit;
            }
        }
    }



    public static bool CircleCast()
    {
        return false;
    }

    public static bool PointCast()
    {
        return false;
    }
}