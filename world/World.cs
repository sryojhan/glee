using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Glee.Behaviours;
using Glee.Engine;
using Glee.Graphics;
using Glee.Physics;

namespace Glee;


/// <summary>
/// Class that manages all the active entities in the game
/// </summary>
/// 
///TODO: Unique element in world with Attributes: Attribute.IsDefined()
/// 
public abstract class World : GleeObject
{
    protected ContentManager Content { get; }
    private readonly Time worldTimeInstance;

    public Time Time => worldTimeInstance;

    public GleeContainer worldObjects;
    public PhysicsWorld physicsWorld;
    public List<IUpdatable> updatables;
    public List<IRenderizable> renderizables;


    protected Color backgroundColor = Color.CornflowerBlue;

    public float PixelsPerUnit { get; protected set; } = 100;

    public bool HasInitialisedEntities { get; private set; } = false;


    public World()
    {
        Content = new(GleeCore.Content.ServiceProvider)
        {
            RootDirectory = GleeCore.Content.RootDirectory
        };


        worldObjects = [];

        updatables = [];
        renderizables = [];

        worldTimeInstance = new Time();
        physicsWorld = new(this);
    }

    public virtual void LoadResources() { }

    public abstract void CreateWorld();


    public void Initialize()
    {
        LoadResources();
        CreateWorld();
        InitialiseObjects();
    }

    private void InitialiseObjects()
    {
        foreach (GleeObject gleeObj in worldObjects)
        {
            if (gleeObj is IInitializable initializable)
            {
                initializable.Initialize();
            }
        }

        HasInitialisedEntities = true;
    }

    public void ProcessFrame()
    {
        UpdateTime();

        if (this is IUpdatable updatable)
        {
            updatable.Update();
        }

        if (physicsWorld.ShouldUpdatePhysics())
        {
            float deltaTime = physicsWorld.UpdatePhysicsTime();

            physicsWorld.PhysicsStep();

            Time.deltaTime = deltaTime;
        }

        foreach (IUpdatable entity in updatables)
        {
            entity.Update();
        }

        //TODO: physics loop

        //TODO: remove elements
    }


    private void UpdateTime()
    {
        float speed = worldTimeInstance.speed;
        float deltaTime = (float)GleeCore.GameTime.ElapsedGameTime.TotalSeconds;

        worldTimeInstance.realDeltaTime = deltaTime;
        worldTimeInstance.deltaTime = deltaTime * speed;

        worldTimeInstance.activeTime += worldTimeInstance.deltaTime;
        worldTimeInstance.realActiveTime += worldTimeInstance.realDeltaTime;


        worldTimeInstance.frame++;

        //Physics time are updated
    }

    public void RenderFrame()
    {
        Renderer.Clear(backgroundColor);
        Renderer.BeginBatch();

        if (this is IRenderizable renderizable)
        {
            renderizable.Render();
        }

        foreach (IRenderizable entity in renderizables)
        {
            entity.Render();
        }

        Renderer.EndBatch();
    }


    public EntityComposed CreateComposedEntity(string name, Entity parent = null)
    {
        return CreateComposedEntity(name, parent, Vector2.Zero, Vector2.One);
    }


    public EntityComposed CreateComposedEntity(string name, Vector2 position, Vector2 size)
    {
        return CreateComposedEntity(name, null, position, size);
    }

    public EntityComposed CreateComposedEntity(string name, Entity parent, Vector2 position, Vector2 size)
    {
        EntityComposed newEntity = new EntityComposed(name, parent, this);

        worldObjects.Add(newEntity);

        updatables.Add(newEntity);
        renderizables.Add(newEntity);

        newEntity.Position = position;
        newEntity.Size = size;

        return newEntity;
    }


    public Entity AddEntity(Entity entity)
    {
        worldObjects.Add(entity);

        if (entity is IUpdatable updatable)
        {
            updatables.Add(updatable);
        }

        if (entity is IRenderizable renderizable)
        {
            renderizables.Add(renderizable);
        }

        return entity;
    }
}