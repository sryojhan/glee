using Glee.Attributes;
using Glee.Behaviours;
using Glee.Physics;

namespace Glee.Components;


[DependsOn(typeof(Collider))]
public class Body : ComponentRaw, IInitializable, IRemovableObserver
{
    public Vector2 Velocity { get; set; }
    public float GravityMultiplier { get; set; } = 1;

    public Collider collider { get; set; }

    public void Initialize()
    {
        collider ??= TryGetComponent<Collider>();

        PhysicsWorld.RegisterBody(this);
    }

    public void OnRemove()
    {
        PhysicsWorld.UnregisterBody(this);
    }

    public void AddVelocity(Vector2 velocity)
    {
        Velocity += velocity * Time.physicsDeltaTime;
    }

    public void AddInstantVelocity(Vector2 velocity)
    {
        Velocity += velocity;
    }


    public void SetHorizontalVelocity(float velocity)
    {
        Velocity = new Vector2(velocity, Velocity.Y);
    }

    public void SetVerticalVelocity(float velocity)
    {
        Velocity = new Vector2(Velocity.X, velocity);
    }


}