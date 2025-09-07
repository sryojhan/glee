using System;
using System.Security.AccessControl;
using Glee.Behaviours;
using static Glee.Events;


namespace Glee.Engine;

/// <summary>
/// 
/// List of avaliable extension methods:
///     - Services => Get
///     - LOG => Print
///     - Resources => Load
/// 
/// </summary>
public class GleeObject
{
    public UID UID { get; private set; } = new UID();
    public bool IsValid { get; private set; } = true;


    public static implicit operator UID(GleeObject obj)
    {
        obj.CheckValid();
        return obj.UID;
    }

    public static bool operator true(GleeObject obj)
    {
        return obj != null && obj.IsValid;
    }

    public static bool operator false(GleeObject obj)
    {
        return obj == null || !obj.IsValid;
    }

    public static bool operator !(GleeObject obj)
    {
        return obj == null || !obj.IsValid;
    }

    public static bool IsNullOrInvalid(GleeObject obj)
    {
        return obj == null || !obj.IsValid;       
    }

    internal void Clear()
    {
        if (CheckValid()) return;

        if (this is ICleanable cleanable)
        {
            cleanable.CleanUp();
        }

        IsValid = false;
    }

    protected bool CheckValid()
    {
        if (!IsValid)
            GleeError.InvalidGleeObject();

        return !IsValid;
    }


    //TODO: move each of this to a partial class
    protected void Print(object message)
    {
                if (CheckValid()) return;

        Services.Fetch<Log>().Message($"{GleeCore.GameTime.TotalGameTime}: {GetType()}: {message}");
    }

    protected static ServiceType Get<ServiceType>() where ServiceType : Service
    {
        return Services.Fetch<ServiceType>();
    }

    protected static ResourceType Load<ResourceType>(string name) where ResourceType : GleeResource
    {
        return Get<Resources>().Load<ResourceType>(name);
    }


    protected void Raise<EventType>(EventType data = null, Scope scope = Scope.World) where EventType : GleeEvent
    {
        if (CheckValid()) return;
        Get<Events>().Raise(this, data, scope);
    }

    protected void Observe<EventType>(OnEventObserved callback) where EventType : GleeEvent
    {
        if (CheckValid()) return;
        Get<Events>().Observe<EventType>(this, callback);
    }

}
