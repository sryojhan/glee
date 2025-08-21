using Glee.Behaviours;
using Glee.Physics;

namespace Glee.Components;


public class Collider : Component, IInitializable, IRemovable
{
    public Bounds bounds { get; set; }

    public Collider()
    {
        bounds = new Rect(entity);
    }


    public void Initialize()
    {
        PhysicsWorld.RegisterCollider(this);
    }

    public void Remove()
    {
        PhysicsWorld.UnregisterCollider(this);
    }
}