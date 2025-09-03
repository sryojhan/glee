using System;
using System.Collections.Generic;
using Glee.Engine;

namespace Glee;


public class GleeEvent : GleeObject { }

public class Events : CoreService
{
    public enum Scope { World, Local, Global }

    public delegate void OnEventObserved(GleeEvent ev);

    private readonly Dictionary<Type, List<ObserverData>> observers = [];

    private record struct ObserverData(WeakReference<GleeObject> observer, OnEventObserved callback);

    internal void Observe<EventType>(GleeObject observer, OnEventObserved callback) where EventType : GleeEvent
    {
        if (observer == null)
        {
            GleeError.InvalidInitialization($"EventObserver creation [{observer} is null]");
            return;
        }

        if (callback == null)
        {
            GleeError.InvalidInitialization($"EventObserver creation [{callback} is null]");
            return;
        }

        if (!observers.ContainsKey(typeof(EventType)))
            observers.Add(typeof(EventType), []);

        observers[typeof(EventType)].Add(new ObserverData(new WeakReference<GleeObject>(observer), callback));
    }

    internal void Raise<EventType>(GleeObject caller, EventType eventData = null, Scope scope = Scope.World) where EventType : GleeEvent
    {
        if (scope == Scope.Local && caller is World)
        {
            scope = Scope.World;
        }

        GleeObject ingameElement = scope == Scope.Local ? GetEntity(caller) : (scope == Scope.World ? GetWorld(caller) : null);

        if (observers.TryGetValue(typeof(EventType), out var observersData))
        {
            foreach (ObserverData observer in observersData)
            {
                if (!observer.observer.TryGetTarget(out GleeObject observerObj))
                {
                    //TODO: error type
                    GleeError.Throw("Tried to call a dead object");

                    //TODO: remove
                    //observersData.Remove(observer);
                    continue;
                }
                if (scope == Scope.Local)
                {
                    if (ingameElement != null && ingameElement == GetEntity(observerObj))
                    {
                        observer.callback.Invoke(eventData);
                    }
                }

                else if (scope == Scope.World)
                {
                    if (ingameElement != null && ingameElement == GetWorld(observerObj))
                    {
                        observer.callback.Invoke(eventData);
                    }
                }

                else
                {
                    observer.callback.Invoke(eventData);
                }
            }
        }
    }


    private static World GetWorld(GleeObject obj)
    {
        //TODO: base class for world, component and entity
        if (obj is Component component)
        {
            return component.world;
        }

        else if (obj is EntityRaw entity)
        {
            return entity.world;
        }
        else if (obj is World world)
        {
            return world;
        }

        else if (obj is Service service)
        {
            return null;
        }
        else
        {
            //TODO: create specific error types
            GleeError.Throw("Invalid called for the event");
            return null;
        }
    }


    private static EntityRaw GetEntity(GleeObject obj)
    {
        if (obj is Component component)
        {
            return component.entity;
        }

        else if (obj is EntityRaw entity)
        {
            return entity;
        }
        else if (obj is World world)
        {
            return null;
        }

        else if (obj is Service service)
        {
            return null;
        }
        else
        {
            //TODO: create specific error types
            GleeError.Throw("Invalid called for the event");
            return null;
        }
    }


}
