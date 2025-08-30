using System.Collections.Generic;

using Glee.Engine;
using Glee.Behaviours;
using System;

namespace Glee;


public class Entity : EntityRaw, IInitializable, IUpdatable, IRenderizable
{
    readonly GleeContainer components;

    readonly List<IUpdatable> updatables;
    readonly List<IRenderizable> renderizables;


    public Entity(string name, World world) : this(name, null, world)
    {

    }

    public Entity(string name, EntityRaw parent, World world) : base(name, parent, world)
    {
        components = [];

        updatables = [];
        renderizables = [];
    }


    public ComponentType CreateComponent<ComponentType>() where ComponentType : ComponentRaw, new()
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
            if (obj is Attributes.DependsOnAttribute requirement)
            {
                if (HasComponent(requirement.Value)) continue;

                typeof(Entity).
                GetMethod(nameof(CreateComponent)).
                MakeGenericMethod(requirement.Value).Invoke(this, null);
            }
        }
    }



    public bool HasComponent(Type componentType)
    {
        foreach (ComponentRaw comp in components)
        {
            if (comp.GetType() == componentType) return true;
        }

        return false;
    }

    public ComponentType GetComponent<ComponentType>() where ComponentType : ComponentRaw
    {
        foreach (ComponentRaw comp in components)
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
        foreach (ComponentRaw comp in components)
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
            //TODO: do something to avoid making this conversion everyframe
            if (((ComponentRaw)updatable).Enabled)
                updatable.Update();
        }
    }

    public void Render()
    {
        foreach (IRenderizable renderizable in renderizables)
        {
            if (((ComponentRaw)renderizable).Enabled)
                renderizable.Render();
        }
    }




}