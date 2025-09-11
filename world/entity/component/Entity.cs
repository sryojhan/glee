using System.Collections.Generic;

using Glee.Engine;
using Glee.Behaviours;
using System;
using Glee.Components;
using System.Net;

namespace Glee;


public class GleeEntity : GleeEntityRaw, IInitializable, IUpdatable, IRenderizable, ICollisionObserver
{
    //TODO: maybe it's better to not have a GleeContainer here
    readonly GleeContainer components;

    readonly List<IUpdatable> updatables;
    readonly List<IRenderizable> renderizables;

    //TODO: physics loop components

    public GleeEntity(string name, World world) : this(name, null, world)
    {

    }

    public GleeEntity(string name, GleeEntityRaw parent, World world) : base(name, parent, world)
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

                typeof(GleeEntity).
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
                GleeError.TryIfPermissive(initializable.Initialize);
            }
        }
    }



    public void Update()
    {
        foreach (IUpdatable updatable in updatables)
        {
            if (((ComponentRaw)updatable).Enabled) //TODO: remove casting
                GleeError.TryIfPermissive(updatable.Update);
        }
    }

    public void Render()
    {
        foreach (IRenderizable renderizable in renderizables)
        {
            if (((ComponentRaw)renderizable).Enabled)
                GleeError.TryIfPermissive(renderizable.Render);
        }
    }



    public void OnCollisionBegin(Collider other)
    {
        foreach (ComponentRaw component in components)
        {
            if (component is ICollisionObserver observer)
            {
                GleeError.TryIfPermissive(() =>{ observer.OnCollisionBegin(other);});
            }
        }
    }

    public void OnCollision(Collider other)
    {
        foreach (ComponentRaw component in components)
        {
            if (component is ICollisionObserver observer)
            {
                GleeError.TryIfPermissive(() =>{ observer.OnCollision(other);});
            }
        }
    }
    public void OnCollisionEnd(Collider other)
    {
        foreach (ComponentRaw component in components)
        {
            if (component is ICollisionObserver observer)
            {
                GleeError.TryIfPermissive(() =>{ observer.OnCollisionEnd(other);});
            }
        }
    }


}