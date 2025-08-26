using System;


namespace Glee.Engine;


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


    protected static ServiceType Get<ServiceType>() where ServiceType : Service
    {
        return Services.Fetch<ServiceType>();
    }

    private void PrintInternal(string message)
    {
        Get<Log>().Print($"{GleeCore.GameTime.TotalGameTime}: {GetType()}: {message}");
    }

    protected void Print(object message)
    {
        PrintInternal(message.ToString());
    }


}
