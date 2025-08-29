using System;
using System.Collections.Generic;
using Glee.Behaviours;
using Glee.Engine;
using Glee.Graphics;

namespace Glee;

//TODO: make AdminService. This service cannot be disabled, removed, created by the game...

public class Service : GleeObject
{
    //TODO: make an interface to implement enabled in various clases (entity, components)
    public bool Enabled { get; set; } = true;
}

public class Services
{
    readonly Dictionary<Type, Service> services = [];

    private static Services instance => GleeCore.Services;


    public void UpdateServices()
    {
        foreach (Service service in services.Values)
        {
            if (service.Enabled && service is IUpdatable updatable)
            {
                updatable.Update();
            }
        }
    }

    public void RenderServices()
    {
        Renderer.BeginBatchAbsolute();

        foreach (Service service in services.Values)
        {
            if (service.Enabled && service is IRenderizable renderizable)
            {
                renderizable.Render();
            }
        }

        Renderer.EndBatch();
    }


    public static ServiceType Fetch<ServiceType>() where ServiceType : Service
    {
        foreach (Service service in instance.services.Values)
        {
            if (service is ServiceType type)
            {
                return type;
            }
        }

        return null;
    }


    public static void Append<ServiceType>(Service service) where ServiceType : Service
    {
        instance.services.Add(typeof(ServiceType), service);
    }


    public static ServiceType Run<ServiceType>() where ServiceType : Service, new()
    {
        ServiceType serv = new();
        instance.services.Add(typeof(ServiceType), serv);

        return serv;
    }

    public static void Shutdown<ServiceType>() where ServiceType : Service
    {
        ServiceType serv = Fetch<ServiceType>();

        if (serv is IRemovableObserver removable)
            removable.OnRemove();

        instance.services.Remove(typeof(ServiceType));
    }

    public static void Pause<ServiceType>() where ServiceType : Service
    {
        Fetch<ServiceType>().Enabled = false;
    }

    public static void Resume<ServiceType>() where ServiceType : Service
    {
        Fetch<ServiceType>().Enabled = true;
    }
}
