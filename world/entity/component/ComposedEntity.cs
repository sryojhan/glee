using System;
using System.Collections.Generic;
using Glee.Engine;
using Glee.ECS.Behaviour;

namespace Glee.ECS;



public class ComposedEntity : Entity, IUpdatable, IRenderizable
{
    GleeContainer components;

    List<IUpdatable> updatables;
    List<IRenderizable> renderizables;


    public ComposedEntity(string name, World world) : base(name, world)
    {
        components = [];

        updatables = [];
        renderizables = [];
    }

    public ComposedEntity(string name, Entity parent, World world) : base(name, parent, world)
    {
    }



    public ComponentType CreateComponent<ComponentType>() where ComponentType : Component, new()
    {
        ComponentType comp = new();
        components.Add(comp);


        if (comp is IUpdatable updatable)
        {
            updatables.Add(updatable);
        }

        if (comp is IRenderizable renderizable)
        {
            renderizables.Add(renderizable);
        }

        return comp;
    }

    
    public void Update()
    {
        foreach (IUpdatable updatable in updatables)
        {
            updatable.Update();
        }
    }

    public void Render()
    {
        foreach (IRenderizable renderizable in renderizables)
        {
            renderizable.Render();
        }
    }

}