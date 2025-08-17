using System;


namespace Glee.Engine;


public class GleeObject
{
    public UID UID { get; private set; }




    public static implicit operator UID(GleeObject obj)
    {
        return obj.UID;
    }


    private void PrintInternal(string message)
    {
        //TODO: add time stamps
        Console.WriteLine($"{0000}: {GetType()}: {message}");
    }

    protected void Print(object message)
    {
        PrintInternal(message.ToString());
    }


}
