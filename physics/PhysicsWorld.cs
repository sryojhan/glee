using System;
using System.Collections.Generic;
using Glee.Behaviours;
using Glee.Components;
using Glee.Engine;
using Microsoft.Xna.Framework;


namespace Glee.Physics;


public class PhysicsWorld
{
    public Vector2 Gravity { get; set; } = new(0, -10.0f);

    public static Dictionary<(Type a, Type b), ICollisionResolver> CollisionResolver { get; private set; } = null;

    internal readonly HashSet<Collider> colliders;
    internal readonly HashSet<Body> bodies;


    private record struct CollisionRegistry
    {
        public Collider A { get; }
        public Collider B { get; }
        public CollisionRegistry(Collider first, Collider second)
        {
            A = first.GetHashCode() < second.GetHashCode() ? first : second;
            B = first.GetHashCode() < second.GetHashCode() ? second : first;
        }
    }

    private HashSet<CollisionRegistry> collisionThisFrame;
    private HashSet<CollisionRegistry> collisionLastFrame;

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

        collisionThisFrame = [];
        collisionLastFrame = [];

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


    private delegate void CollisionCallback(Collider collider, ICollisionResolver resolver, Vector2 direction);

    //TODO: Move this to a physics manager
    internal void PhysicsStep()
    {

        foreach (Body body in bodies)
        {
            //Vector2 validPosition = body.entity.Position;

            //Update the current velocity
            body.Velocity += Gravity * body.GravityMultiplier * world.Time.deltaTime;


            //Separate collisions between axis

            float maxPenetration = 0;
            void OnCollision(Collider collider, ICollisionResolver resolver, Vector direction)
            {
                float penetration = resolver.CalculatePenetration(body.collider.bounds, collider.bounds, direction);

                if (MathF.Abs(penetration) > MathF.Abs(maxPenetration))
                    maxPenetration = penetration;

                collisionThisFrame.Add(new CollisionRegistry(body.collider, collider));
            }



            if (MathF.Abs(body.Velocity.X) > Utils.Delta)
            {
                body.entity.Position = body.entity.Position + Utils.Right * body.Velocity.X * world.Time.physicsDeltaTime;

                CheckCollisionWithAllColliders(body, OnCollision, Utils.Right * body.Velocity.X);

                if (MathF.Abs(maxPenetration) > 0)
                {

                    body.Velocity = new Vector2(0, body.Velocity.Y);
                    body.entity.Position -= Utils.Right * maxPenetration;
                }
            }

            maxPenetration = 0;

            if (MathF.Abs(body.Velocity.Y) > Utils.Delta)
            {
                body.entity.Position = body.entity.Position + Utils.Up * body.Velocity.Y * world.Time.physicsDeltaTime;

                CheckCollisionWithAllColliders(body, OnCollision, Utils.Up * body.Velocity.Y);

                if (MathF.Abs(maxPenetration) > 0)
                {
                    body.Velocity = new Vector2(body.Velocity.X, 0);
                    body.entity.Position -= Utils.Up * maxPenetration;
                }

            }

        }


        ManageCollisionCallbacks();
    }



    private void CheckCollisionWithAllColliders(Body body, CollisionCallback onCollision, Vector2 direction)
    {
        Type bodyType = body.collider.bounds.GetType();

        foreach (Collider collider in colliders)
        {
            if (collider == body.collider) continue;

            Type colliderType = collider.bounds.GetType();

            if (!CollisionResolver.TryGetValue((bodyType, colliderType), out ICollisionResolver resolver))
            {
                //TODO: error
                GleeError.Throw($"Cannot check collision between {bodyType} and {colliderType}");
                continue;
            }

            bool collision = resolver.Resolve(body.collider.bounds, collider.bounds);

            if (collision)
            {
                onCollision.Invoke(collider, resolver, direction);
            }
        }
    }



    private void ManageCollisionCallbacks()
    {
        foreach (CollisionRegistry registry in collisionThisFrame)
        {
            ICollisionStayObserver A = registry.A.entity as ICollisionStayObserver;
            ICollisionStayObserver B = registry.B.entity as ICollisionStayObserver;

            if (collisionLastFrame.Contains(registry))
            {
                // collision stay
                A?.OnCollision(registry.B);
                B?.OnCollision(registry.A);

                collisionLastFrame.Remove(registry);
            }
            else
            {
                // collision begin
                ICollisionBeginObserver A_begin = registry.A.entity as ICollisionBeginObserver;
                ICollisionBeginObserver B_begin = registry.B.entity as ICollisionBeginObserver;

                A_begin?.OnCollisionBegin(registry.B);
                B_begin?.OnCollisionBegin(registry.A);

                A?.OnCollision(registry.B);
                B?.OnCollision(registry.A);

            }
        }

        //If collisions last frame have not been cleared, it means it doesn't collide any more. Hence we call collisionEnd
        foreach (CollisionRegistry registry in collisionLastFrame)
        {
            ICollisionEndObserver A_end = registry.A.entity as ICollisionEndObserver;
            ICollisionEndObserver B_end = registry.B.entity as ICollisionEndObserver;

            A_end.OnCollisionEnd(registry.B);
            B_end.OnCollisionEnd(registry.A);
        }


        collisionLastFrame = collisionThisFrame;
        collisionThisFrame = [];
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
        if (body.collider != null)
            body.world.physicsWorld.bodies.Add(body);
        else GleeError.Throw("Tried to add a body with no collider assigned"); //TODO: custom error
    }

    public static void UnregisterBody(Body body)
    {
        body.world.physicsWorld.bodies.Remove(body);
    }


}