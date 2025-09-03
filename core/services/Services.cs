using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Security;
using Glee.Behaviours;
using Glee.Engine;
using Glee.Graphics;

namespace Glee;

//TODO: make AdminService. This service cannot be disabled, removed, created by the game...

public class Service : GleeObject
{
    //TODO: make an interface to implement enabled in various clases (entity, components)
    public bool Enabled { get; set; } = true;

    public virtual void OnPause() { }
    public virtual void OnResume() { }
}


public class CoreService : Service
{
    internal CoreService() { }
}


public class Services
{
    //TODO: make so services are executed in the order they are added
    readonly Dictionary<Type, Service> services = [];

    private static Services instance => GleeCore.Services;


    public void UpdateServices()
    {
        //TODO: optimize this in lists
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

    //TODO: the gleeObject accessor is Get, maybe change the name from fetch to get here also?
    public static ServiceType Fetch<ServiceType>() where ServiceType : Service
    {
        Type type = typeof(ServiceType);
        if (!instance.services.TryGetValue(type, out Service value))
        {
            //TODO: custom error
            GleeError.Throw($"The service {type.Name} is not initialised");
            return null;
        }

        return value as ServiceType;
    }


    public static void Append<ServiceType>(Service service) where ServiceType : Service
    {
        if (CoreCheckError<ServiceType>("Append")) return;
        instance.services.Add(typeof(ServiceType), service);
    }


    public static ServiceType Run<ServiceType>() where ServiceType : Service, new()
    {
        if (CoreCheckError<ServiceType>("Run")) return null;
        return RunInternal<ServiceType>();
    }

    public static void Shutdown<ServiceType>() where ServiceType : Service
    {
        if (CoreCheckError<ServiceType>("Shutdown")) return;
        ShutdownInternal<ServiceType>();
    }

    public static void Pause<ServiceType>() where ServiceType : Service
    {
        if (CoreCheckError<ServiceType>("Pause")) return;
        PauseInternal<ServiceType>();
    }

    public static void Resume<ServiceType>() where ServiceType : Service
    {
        if (CoreCheckError<ServiceType>("Resume")) return;
        ResumeInternal<ServiceType>();
    }

    public static ServiceType RunInternal<ServiceType>() where ServiceType : Service, new()
    {
        ServiceType serv = new();
        instance.services.Add(typeof(ServiceType), serv);

        return serv;
    }


    public static void ShutdownInternal<ServiceType>() where ServiceType : Service
    {
        ServiceType serv = Fetch<ServiceType>();

        if (serv is ICleanable removable)
            removable.CleanUp();

        instance.services.Remove(typeof(ServiceType));
    }



    internal static void PauseInternal<ServiceType>() where ServiceType : Service
    {
        ServiceType service = Fetch<ServiceType>();

        if (!service.Enabled)
        {
            service.Enabled = false;
            service.OnPause();
        }
    }



    internal static void ResumeInternal<ServiceType>() where ServiceType : Service
    {
        ServiceType service = Fetch<ServiceType>();

        if (!service.Enabled)
        {
            service.Enabled = true;
            service.OnResume();
        }
    }

    private static bool CoreCheckError<ServiceType>(string errorMessage = "")
    {
        if (typeof(CoreService).IsAssignableFrom(typeof(ServiceType)))
        {
            //TODO: custom error message
            GleeError.Throw($"Tried to alter a core service: {errorMessage}");
            return true;
        }
        return false;
    }
}
