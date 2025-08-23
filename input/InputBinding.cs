using System;

namespace Glee.Input;



public class InputBinding(string name)
{
    public string Name { get; private set; } = name;


    


    public void Bind(GenericButton button)
    {

    }

    public void BindNegative(GenericButton button)
    {

    }

    public void BindSecondary(GenericButton button)
    {

    }

    public void BindSecondaryNegative(GenericButton button)
    {

    }

}