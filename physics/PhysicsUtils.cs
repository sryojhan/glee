
using Glee.Components;
using Glee.Engine;

namespace Glee.Physics;


public static class Physics
{

    public static bool Raycast(Vector2 origin, Vector2 direction, float distance = float.MaxValue)
    {
        World world = GleeCore.WorldManager.Spotlight;
        return Raycast(world, origin, direction, distance);
    }


    //TODO: variation that gives the RaycastHit
    //TODO: variation with collision layes
    //TODO: variation with entity exclusion list
    public static bool Raycast(World world, Vector2 origin, Vector2 direction, float distance = float.MaxValue)
    {
        PhysicsWorld physicsWorld = world.physicsWorld;

        foreach (Collider collider in physicsWorld.colliders)
        {
            if (collider.bounds.Raycast(origin, direction, distance)) return true;
        }

        return false;
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