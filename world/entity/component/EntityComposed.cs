using System.Collections.Generic;

using Glee.Engine;
using Glee.Behaviours;

namespace Glee;


public class EntityComposed : Entity, IInitializable, IUpdatable, IRenderizable
{
    readonly GleeContainer components;

    readonly List<IUpdatable> updatables;
    readonly List<IRenderizable> renderizables;


    public EntityComposed(string name, World world) : this(name, null, world)
    {
        
    }

    public EntityComposed(string name, Entity parent, World world) : base(name, parent, world)
    {
        components = [];

        updatables = [];
        renderizables = [];
    }



    public ComponentType CreateComponent<ComponentType>() where ComponentType : Component, new()
    {
        ComponentType comp = new()
        {
            Owner = this
        };
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


    public ComponentType GetComponent<ComponentType>() where ComponentType : Component
    {
        foreach (Component comp in components)
        {
            if (comp is ComponentType found)
            {
                return found;
            }
        }


        return null;
    }

    public void Initialize()
    {
        //TODO: Manage dependencies first


        foreach (Component comp in components)
        {
            if (comp is IInitializable initializable)
            {
                initializable.Initialize();
            }
        }
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