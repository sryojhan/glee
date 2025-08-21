using Glee.Attributes;
using Glee.Behaviours;
using Glee.Physics;

namespace Glee.Components;



public class Body : Component, IInitializable, IRemovableObserver
{
    public Vector2 Velocity { get; set; }
    public float GravityMultiplier { get; set; } = 1;

    public  Collider collider { get; set; }

    public void Initialize()
    {
        collider = entity.GetComponent<Collider>();

        PhysicsWorld.RegisterBody(this);
    }

    public void OnRemove()
    {
        PhysicsWorld.UnregisterBody(this);
    }


}