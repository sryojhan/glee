using Glee.Attributes;
using Glee.Behaviours;
using Glee.Physics;

namespace Glee.Components;



[DependsOn(typeof(Collider))]
public class DynamicBody : Component, IInitializable, IRemovable
{
    public Vector2 Velocity { get; set; }
    //TODO: mass
    //TODO: drag
    //TODO: gravity scale
    public float GravityMultiplier { get; set; } = 1;

    public  Collider collider { get; set; }

    public void Initialize()
    {
        collider = entity.GetComponent<Collider>();

        PhysicsWorld.RegisterBody(this);
    }

    public void Remove()
    {
        PhysicsWorld.UnregisterBody(this);
    }


}