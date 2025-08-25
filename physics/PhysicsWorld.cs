using System;
using System.Collections.Generic;
using Glee.Components;
using Glee.Engine;


namespace Glee.Physics;


public class PhysicsWorld
{
    public Vector2 Gravity { get; set; } = new(0, -10.0f);

    public static Dictionary<(Type a, Type b), ICollisionResolver> CollisionResolver { get; private set; } = null;

    internal readonly HashSet<Collider> colliders;
    internal readonly HashSet<Body> bodies;

    private float lastPhysicsUpdate;

    public float TargetPhysicsFrameRate
    {
        private get => targetPhysicsFrameRate;
        set
        {
            targetPhysicsFrameRate = value;
            targetPhysicsEnlapsedTime = 1.0f / value;
        }
    }

    public World world { get; private set; }

    private float targetPhysicsFrameRate;
    private float targetPhysicsEnlapsedTime;

    private float currentEnlapsedTime;

    internal PhysicsWorld(World world)
    {
        this.world = world;

        colliders = [];
        bodies = [];

        TargetPhysicsFrameRate = 50.0f;

        lastPhysicsUpdate = 0;
        currentEnlapsedTime = 0;


        if (CollisionResolver == null)
        {
            CollisionResolver = [];

            CollisionResolver.Add((typeof(Circle), typeof(Circle)), new CircleCircleCollisionResolver());
            CollisionResolver.Add((typeof(Rect), typeof(Rect)), new RectRectCollisionResolver());

            CollisionResolver.Add((typeof(Circle), typeof(Rect)), new CircleRectCollisionResolver());
            CollisionResolver.Add((typeof(Rect), typeof(Circle)), new CircleRectCollisionResolver());
        }
    }


    public bool ShouldUpdatePhysics()
    {
        currentEnlapsedTime = world.Time.realActiveTime - lastPhysicsUpdate;

        // This is done like this for a future enhancement. Right now physics and logic run at the same fixed frameRate
        return true;
    }


    internal float UpdatePhysicsTime()
    {

        Time time = world.Time;

        float deltaTime = time.deltaTime;

        time.realPhysicsDeltaTime = currentEnlapsedTime;
        time.physicsDeltaTime = currentEnlapsedTime * time.speed;

        time.deltaTime = time.physicsDeltaTime;

        lastPhysicsUpdate = time.realActiveTime;

        time.physicsFrame++;

        return deltaTime;
    }

    internal void PhysicsStep()
    {

        foreach (Body body in bodies)
        {
            Vector2 previousPosition = body.entity.Position;

            body.Velocity += Gravity * body.GravityMultiplier * world.Time.deltaTime;
            body.entity.Position += body.Velocity * world.Time.deltaTime;

            if (CheckCollision(body))
            {
                body.Velocity = Vector2.Zero;
                body.entity.Position = previousPosition;
            }

        }
    }



    public bool CheckCollision(Body body)
    {
        Type bodyType = body.collider.bounds.GetType();
        foreach (Collider collider in colliders)
        {
            if (collider == body.collider) continue;

            Type colliderType = collider.bounds.GetType();

            if (!CollisionResolver.TryGetValue((bodyType, colliderType), out ICollisionResolver resolver))
            {
                throw new Exception($"Cannot check collision between {bodyType} and {colliderType}");
            }

            bool collision = resolver.Resolve(body.collider.bounds, collider.bounds);

            if (collision)
            {
                return true;
            }
        }

        return false;
    }



    public static void RegisterCollider(Collider collider)
    {
        collider.world.physicsWorld.colliders.Add(collider);

    }

    public static void UnregisterCollider(Collider collider)
    {
        collider.world.physicsWorld.colliders.Remove(collider);
    }

    public static void RegisterBody(Body body)
    {
        body.world.physicsWorld.bodies.Add(body);
    }

    public static void UnregisterBody(Body body)
    {
        body.world.physicsWorld.bodies.Remove(body);
    }


}