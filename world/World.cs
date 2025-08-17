using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Glee.Behaviours;
using Glee.Engine;

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

    public GleeContainer worldElements;
    public List<IUpdatable> updatables;
    public List<IRenderizable> renderizables;


    protected Color backgroundColor = Color.CornflowerBlue;


    public World()
    {
        Content = new(GleeCore.Content.ServiceProvider)
        {
            RootDirectory = GleeCore.Content.RootDirectory
        };

        updatables = [];
        renderizables = [];
    }

    public virtual void LoadResources() { }

    public abstract void CreateWorld();

    public void Initialize()
    {
        LoadResources();
        CreateWorld();
    }

    public void Udpate(GameTime gameTime)
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


    public void Render(GameTime gameTime)
    {
        GleeCore.GraphicsDevice.Clear(backgroundColor);
        GleeCore.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        foreach (IRenderizable entity in renderizables)
        {
            entity.Render();
        }

        if (this is IRenderizable renderizable)
        {
            renderizable.Render();
        }

        GleeCore.SpriteBatch.End();
    }



    public Entity CreateComposedEntity(string name, Entity parent = null)
    {
        Entity newEntity = new EntityComposed(name, parent, this);

        worldElements.Add(newEntity);

        return newEntity;
    }

    public Entity AddEntity(Entity entity)
    {
        worldElements.Add(entity);
        return entity;
    }
}