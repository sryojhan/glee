using System;
using Glee.Behaviours;
using Glee.Engine;

namespace Glee;



public class Log : CoreService, ICleanable
{
    const string RED = "\u001b[31m";
    const string YELLOW = "\u001b[33m";


    public void Message(object obj)
    {
        Console.WriteLine(obj.ToString());
    }

    public void Error(object obj)
    {
        Message(RED + obj);
    }

    public void Warning(object obj)
    {
        Message(YELLOW + obj);
    }


    public void CleanUp()
    {
        //TODO: flushing logs into a file
    }




}

