using System;
using Glee.Behaviours;

namespace Glee;



public class Log : Service, IRemovableObserver
{
    public void Message(object obj)
    {

    }

    public void Error(object obj)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Message(obj);
    }

    public void Warning(object obj)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Message(obj);
    }


    public void OnRemove()
    {
        //TODO: flushing logs into a file
    }
}
