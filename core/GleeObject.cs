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


    private void PrintInternal(string message)
    {
        //TODO: change total game time representation to maybe a frame counter or something
        Console.WriteLine($"{(GleeCore.GameTime != null ? GleeCore.GameTime.TotalGameTime.ToString() : 0)}: {GetType()}: {message}");
    }

    protected void Print(object message)
    {
        PrintInternal(message.ToString());
    }


}
