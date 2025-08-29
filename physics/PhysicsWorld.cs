using System;
using System.Collections.Generic;
using Glee.Components;
using Glee.Engine;
using Microsoft.Xna.Framework;


namespace Glee.Physics;


public class PhysicsWorld
{
    //public Vector2 Gravity { get; set; } = new(0, -10.0f);
    public Vector2 Gravity { get; set; } = new(0, 0);

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
            B = first.GetHashCode() < second.GetHashCode() ? first : second;
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


    private delegate void CollisionCallback(Collider collider, ICollisionResolver resolver);

    //TODO: Move this to a physics manager
    internal void PhysicsStep()
    {

        foreach (Body body in bodies)
        {
            //Vector2 validPosition = body.entity.Position;

            //Update the current velocity
            body.Velocity += Gravity * body.GravityMultiplier * world.Time.deltaTime;


            //Separate collisions between axis


            if (MathF.Abs(body.Velocity.X) > Utils.Delta)
            {
                body.entity.Position = body.entity.Position + Utils.Right * body.Velocity.X * world.Time.physicsDeltaTime;

                float maxPenetration = 0;

                void OnHorizontalCollision(Collider collider, ICollisionResolver resolver)
                {
                    float penetration = resolver.CalculatePenetration(body.collider.bounds, collider.bounds, Utils.Right * body.Velocity.X);

                    if (MathF.Abs(penetration) > MathF.Abs(maxPenetration))
                        maxPenetration = penetration;
                }


                CheckCollisionWithAllColliders(body, OnHorizontalCollision);


                if (MathF.Abs(maxPenetration) > 0)
                {
                    
                    body.Velocity = new Vector2(0, body.Velocity.Y);
                    body.entity.Position -= Utils.Right * maxPenetration;
                }
            }

            if (MathF.Abs(body.Velocity.Y) > Utils.Delta)
            {
                body.entity.Position = body.entity.Position + Utils.Up * body.Velocity.Y * world.Time.physicsDeltaTime;

                float maxPenetration = 0;

                void OnHorizontalCollision(Collider collider, ICollisionResolver resolver)
                {
                    float penetration = resolver.CalculatePenetration(body.collider.bounds, collider.bounds, Utils.Up * body.Velocity.Y);


                    if (MathF.Abs(penetration) > MathF.Abs(maxPenetration))
                        maxPenetration = penetration;
                }

                CheckCollisionWithAllColliders(body, OnHorizontalCollision);

                if (MathF.Abs(maxPenetration) > 0)
                {
                    body.Velocity = new Vector2(body.Velocity.X, 0);
                    body.entity.Position -= Utils.Up * maxPenetration; 
                }

            }

        }


        //TODO: physics callbacks
    }



    private void CheckCollisionWithAllColliders(Body body, CollisionCallback onCollision)
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
                onCollision.Invoke(collider, resolver);
            }
        }
    }




    public bool CheckCollision(Body body, out Collider collidedObj, out ICollisionResolver resolver)
    {
        collidedObj = null;
        resolver = null;

        Type bodyType = body.collider.bounds.GetType();
        foreach (Collider collider in colliders)
        {
            if (collider == body.collider) continue;

            Type colliderType = collider.bounds.GetType();

            if (!CollisionResolver.TryGetValue((bodyType, colliderType), out resolver))
            {
                //TODO: correct error
                GleeError.Throw($"Cannot check collision between {bodyType} and {colliderType}");
                continue;
            }

            bool collision = resolver.Resolve(body.collider.bounds, collider.bounds);

            if (collision)
            {
                collidedObj = collider;
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