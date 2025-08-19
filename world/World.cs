using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Glee.Behaviours;
using Glee.Engine;
using Glee.Graphics;

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

    public GleeContainer worldObjects;
    public List<IUpdatable> updatables;
    public List<IRenderizable> renderizables;


    protected Color backgroundColor = Color.CornflowerBlue;


    public World()
    {
        Content = new(GleeCore.Content.ServiceProvider)
        {
            RootDirectory = GleeCore.Content.RootDirectory
        };


        worldObjects = [];

        updatables = [];
        renderizables = [];
    }

    public virtual void LoadResources() { }

    public abstract void CreateWorld();

    private void InitialiseObjects()
    {
        foreach (GleeObject gleeObj in worldObjects)
        {
            if (gleeObj is IInitializable initializable)
            {
                initializable.Initialize();
            }
        }
    }

    public void Initialize()
    {
        LoadResources();
        CreateWorld();
        InitialiseObjects();
    }

    public void Udpate()
    {
        
        if (this is IUpdatable updatable)
        {
            updatable.Update();
        }

        foreach (IUpdatable entity in updatables)
        {
            entity.Update();
        }
        
    }


    public void Render()
    {
        Renderer.Clear(backgroundColor);
        Renderer.BeginBatch();

        foreach (IRenderizable entity in renderizables)
        {
            entity.Render();
        }

        if (this is IRenderizable renderizable)
        {
            renderizable.Render();
        }

        Renderer.EndBatch();
    }



    public EntityComposed CreateComposedEntity(string name, Entity parent = null)
    {
        EntityComposed newEntity = new EntityComposed(name, parent, this);

        worldObjects.Add(newEntity);
        
        updatables.Add(newEntity);
        renderizables.Add(newEntity);

        return newEntity;
    }

    public Entity AddEntity(Entity entity)
    {
        worldObjects.Add(entity);

        if(entity is IUpdatable updatable)
        {
            updatables.Add(updatable);
        }

        if(entity is IRenderizable renderizable)
        {
            renderizables.Add(renderizable);
        }

        return entity;
    }
}