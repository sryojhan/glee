using System.Collections.Generic;
using System.Reflection;

using Glee.Engine;
using Glee.Behaviours;
using System;

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
        CreateDependencies<ComponentType>();

        ComponentType comp = new()
        {
            entity = this
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

        if (world.HasInitialisedEntities && comp is IInitializable initializable)
        {
            initializable.Initialize();
        }


        return comp;
    }


    private void CreateDependencies<ComponentType>()
    {
        Type component = typeof(ComponentType);

        foreach (object obj in component.GetCustomAttributes(false))
        {
            if (obj is Attributes.RequireAttribute requirement)
            {
                if (HasComponent(requirement.Value)) continue;

                typeof(EntityComposed).
                GetMethod(nameof(CreateComponent)).
                MakeGenericMethod(requirement.Value).Invoke(this, null);
            }
        }
    }



    public bool HasComponent(Type componentType)
    {
        foreach (Component comp in components)
        {
            if (comp.GetType() == componentType) return true;
        }

        return false;
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