using Glee.Attributes;
using Glee.Behaviours;
using Glee.Physics;

namespace Glee.Components;


[DependsOn(typeof(Collider))]
public class Body : ComponentRaw, IInitializable, ICleanable, ICollisionObserver
{
    public Vector Velocity { get; set; }
    public float GravityMultiplier { get; set; } = 1;

    public Collider collider { get; set; }

    public void Initialize()
    {
        collider ??= TryGetComponent<Collider>();

        PhysicsWorld.RegisterBody(this);
    }

    public void CleanUp()
    {
        PhysicsWorld.UnregisterBody(this);
    }

    public void Accelerate(Vector acceleration)
    {
        Velocity += acceleration * Time.physicsDeltaTime;
    }

    public void AddInstantVelocity(Vector velocity)
    {
        Velocity += velocity;
    }


    public void SetHorizontalVelocity(float velocity)
    {
        Velocity = new Vector(velocity, Velocity.Y);
    }

    public void SetVerticalVelocity(float velocity)
    {
        Velocity = new Vector(Velocity.X, velocity);
    }


    public void OnCollision(Collider other)
    {
        Print("AAAAAAAAA");
    }

    public void OnCollisionBegin(Collider other)
    {
        Print("Joder joder");
    }

    public void OnCollisionEnd(Collider other)
    {
        Print("Por fin!!!");
    }

}