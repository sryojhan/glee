

using System;

namespace Glee.Attributes;



[AttributeUsage(AttributeTargets.Class)]
public class UniqueAttribute : Attribute
{
    
}


[AttributeUsage(AttributeTargets.Class)]
public class DependsOnAttribute(Type componentType) : Attribute
{
    public Type Value { get; private set; } = componentType;
}
