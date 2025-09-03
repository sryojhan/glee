using System;
using System.Security.AccessControl;
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


    public static implicit operator UID(GleeObject obj)
    {
        return obj.UID;
    }

    public static bool operator true(GleeObject obj)
    {
        return obj != null;
    }

    public static bool operator false(GleeObject obj)
    {
        return obj == null;
    }

    public static bool operator !(GleeObject obj)
    {
        return obj == null;
    }

    protected void Print(object message)
    {
        Services.Fetch<Log>().Message($"{GleeCore.GameTime.TotalGameTime}: {GetType()}: {message}");
    }

    //TODO: move each of this to a partial class

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
        Get<Events>().Raise(this, data, scope);
    }

    protected void Observe<EventType>(OnEventObserved callback) where EventType : GleeEvent
    {
        Get<Events>().Observe<EventType>(this, callback);
    }

}
