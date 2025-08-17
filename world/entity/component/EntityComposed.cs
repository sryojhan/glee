using System.Collections.Generic;

using Glee.Engine;
using Glee.Behaviours;

namespace Glee;


public class EntityComposed : Entity, IUpdatable, IRenderizable
{
    GleeContainer components;

    readonly List<IUpdatable> updatables;
    readonly List<IRenderizable> renderizables;


    public EntityComposed(string name, World world) : base(name, world)
    {
        components = [];

        updatables = [];
        renderizables = [];
    }

    public EntityComposed(string name, Entity parent, World world) : base(name, parent, world)
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